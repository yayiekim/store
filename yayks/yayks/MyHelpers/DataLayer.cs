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
           
            Order _order = new Order()
            {
                Id = Guid.NewGuid().ToString(),
                AspNetUserId = model.UserId,
                DateCreated = DateTime.UtcNow,
                OrderStatus = "new",
                
            };


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
                    DateAdded = DateTime.UtcNow,
                    ProductsId = o.Id,
                    OrdersId = _order.Id,
                    ProductAmount = o.Amount,
                    Quantity = o.Quantity,
                    OrderDetailsStatus = "new"
                                       

                };

                data.OrderDetails.Add(_orderDetail);

            }
            

            await data.SaveChangesAsync();

            return true;
        }
               
    }
}