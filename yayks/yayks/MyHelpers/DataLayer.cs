using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using yayks.Models;

namespace yayks.MyHelpers
{
    public class DataLayer
    {
        Entities data = new Entities();

        public async Task<List<ProductModel>> getProductsListFromCart(string userId, bool? isSelected)
        {

            if (isSelected == null)
            {
                var _orderDetail = await (from o in data.Carts.Where(i => i.AspNetUserId == userId)
                                          join r in data.Products on o.ProductId equals r.Id
                                          join rr in data.ProductDetails on r.Id equals rr.ProductId
                                          select new ProductModel
                                          {

                                              Id = r.Id,
                                              Amount = r.Amount,
                                              Quantity = o.Quantity,
                                              Brand = r.ProductBrand.Name,
                                              Color = rr.ProductColor.ProductColorName,
                                              Description = r.Description,
                                              Name = r.ProductName,
                                              Measurement = rr.ProductMeasurement.MeasurementValue.ToString(),
                                              Images = rr.ProductDetailImages.ToList().Select(i => i.ImageUrl).ToList(),
                                              IsSelected = o.IsSelected,
                                              CartId = o.Id

                                          }).ToListAsync();

                return _orderDetail;
            }
            else
            {
                var _orderDetail = await (from o in data.Carts.Where(i => i.AspNetUserId == userId && i.IsSelected == true)
                                          join r in data.Products on o.ProductId equals r.Id
                                          join rr in data.ProductDetails on r.Id equals rr.ProductId
                                          select new ProductModel
                                          {

                                              Id = r.Id,
                                              Amount = r.Amount,
                                              Quantity = o.Quantity,
                                              Brand = r.ProductBrand.Name,
                                              Color = rr.ProductColor.ProductColorName,
                                              Description = r.Description,
                                              Name = r.ProductName,
                                              Measurement = rr.ProductMeasurement.MeasurementValue.ToString(),
                                              Images = rr.ProductDetailImages.ToList().Select(i => i.ImageUrl).ToList(),
                                              IsSelected = o.IsSelected,
                                              CartId = o.Id

                                          }).ToListAsync();

                return _orderDetail;

            }

        }


        public async Task<bool> createOrder(CheckOutModels model)
        {
            var dateTime = DateTime.UtcNow;
            var _orderNewStatus = "";

            Order _order = new Order()
            {
                Id = Guid.NewGuid().ToString(),
                AspNetUserId = model.UserId,
                DateCreated = dateTime,

            };

            //set order status and orderdetail status if paid
            if (model.PaymentStatus == "paid")
            {
                _orderNewStatus = "shipping";
            }



            OrderDetailShippingAddress _shipping = new OrderDetailShippingAddress()
            {
                Id = Guid.NewGuid().ToString(),
                Line1 = model.ShippingAddress.Line1,
                Line2 = model.ShippingAddress.Line2,
                State = model.ShippingAddress.State,
                City = model.ShippingAddress.City,
                OdersId = _order.Id
            };

            data.Orders.Add(_order);
            data.OrderDetailShippingAddresses.Add(_shipping);

            foreach (var o in model.Cart)
            {
                OrderDetail _orderDetail = new OrderDetail()
                {
                    Id = Guid.NewGuid().ToString(),
                    DateAdded = dateTime,
                    ProductsId = o.Id,
                    OrdersId = _order.Id,
                    ProductAmount = o.Amount,
                    Quantity = o.Quantity,
                    OrderDetailsStatus = _orderNewStatus


                };

                data.OrderDetails.Add(_orderDetail);

            }

            //payment entry
            var _payment = new Payment()
            {
                Id = Guid.NewGuid().ToString(),
                AspNetUserId = model.UserId,
                PaymentDate = dateTime,
                OrdersId = _order.Id
            };

            var _paymentDetail = new PaymentDetail()
            {
                Id = Guid.NewGuid().ToString(),
                PaymentId = _payment.Id,
                PaymentRef = model.PaymentRef,
                PayerName = model.BillingAddress.name,
                PaymentAmount = model.TotalAmount,
                PaymentMethodId = model.PaymentMethodId

            };

            data.Payments.Add(_payment);
            data.PaymentDetails.Add(_paymentDetail);


            //remove selected item from cart
            var _forDeleteItem = await data.Carts.Where(i => i.AspNetUserId == model.UserId && i.IsSelected == true).ToListAsync();
            data.Carts.RemoveRange(_forDeleteItem);



            try
            {
                await data.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }



        }

        public async Task<bool> AddToCart(string Id, string UserId)
        {

            var res = await data.Carts.Where(i => i.ProductId == Id).ToListAsync();

            if (res.Any())
            {
                res.First().Quantity += 1;

            }
            else
            {
                Cart _myCart = new Cart()
                {
                    Id = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    ProductId = Id,
                    DateCreated = DateTime.UtcNow,
                    AspNetUserId = UserId

                };

                data.Carts.Add(_myCart);
            }


            await data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFromCart(string Id)
        {

            var res = await data.Carts.FindAsync(Id);

            data.Carts.Remove(res);

            try
            {
                await data.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;

            }

        }


        public async Task<int> GetProductCountInOrdered(string Id)
        {
            var _purchaseCount = 0;
            var _ordersCount = 0;

            var _purchases = await (from o in data.Purchases
                                    join p in data.PurchaseDetails.Where(i => i.PurchaseDetailStatus == "delivered" && i.ProductId == Id) on o.Id equals p.PurchaseId
                                    group p by p.ProductId into opp
                                    select new
                                    {
                                        PurchaseCount = opp.Sum(i => i.Quantity)

                                    }).FirstOrDefaultAsync();


            var _orders = await (from o in data.Orders
                                 join p in data.OrderDetails.Where(i => i.ProductsId == Id && (i.OrderDetailsStatus == "delivered" || i.OrderDetailsStatus == "shipping")) on o.Id equals p.OrdersId
                                 group p by p.ProductsId into opp
                                 select new
                                 {
                                     OrdersCount = opp.Sum(i => i.Quantity)

                                 }).FirstOrDefaultAsync();



            if (_purchases != null)
            {

                _purchaseCount = _purchases.PurchaseCount;

            }
            if (_orders != null)
            {
                _ordersCount = _orders.OrdersCount;
            }


            return _purchaseCount - _ordersCount;

        }

        public async Task<int> GetProductCountInMyCart(string Id, string UserId)
        {

            var _cartCount = 0;

            var _stocks = await GetProductCountInOrdered(Id);


            var _cart = await (from o in data.Carts.Where(i => i.AspNetUserId == UserId && i.ProductId == Id)
                               group o by o.ProductId into og
                               select new
                               {
                                   cartCount = og.Sum(i => i.Quantity)

                               }).FirstOrDefaultAsync();



            if (_cart != null)
            {
                _cartCount = _cart.cartCount;
            }


            return _stocks - _cartCount;

        }



        public async Task<bool> setCartItemSelection(string id, bool selection, string userId)
        {

            var _res = await data.Carts.FindAsync(id);

            var itemCount = await GetProductCountInMyCart(_res.ProductId, userId);

            if (itemCount < 0)
            {
                return false;
            }
            else
            {
                _res.IsSelected = selection;

                await data.SaveChangesAsync();
                return true;
            }

        }


        public async Task<ProductModel> GetProduct(string Id)
        {

            var _stocks = await GetProductCountInOrdered(Id);

            return await (from o in data.Products.Where(i => i.Id == Id)
                          select new ProductModel
                          {
                              Id = o.Id,
                              Name = o.ProductName,
                              Description = o.Description,
                              Brand = o.ProductBrand.Name,
                              Amount = o.Amount,
                              Images = o.ProductDetails.Select(i => i.ProductDetailImages.FirstOrDefault()).Select(i => i.ImageUrl).ToList(),
                              Stocks = _stocks

                          }).SingleAsync();

        }



    }
}