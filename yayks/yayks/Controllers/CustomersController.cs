using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwoCheckout;
using yayks.Models;
using yayks.MyHelpers;

namespace yayks.Controllers
{
    public class CustomersController : Controller
    {
        DataLayer dataLayer = new DataLayer();
        Entities data = new Entities();
        AzureBlob azureBlob = new AzureBlob();
        CommonModels common = new CommonModels();
        PaymentGateways _payment = new PaymentGateways();

        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchProducts(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;



            var products = from o in data.Products
                           select new ProductModel
                           {
                               Id = o.Id,
                               Name = o.ProductName,
                               Description = o.Description,
                               Brand = o.ProductBrand.Name,
                               Amount = o.Amount,
                               Images = o.ProductDetails.Select(i => i.ProductDetailImages.FirstOrDefault()).Select(i => i.ImageUrl).ToList()
                           };


            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));


            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                //case "Date":
                //    products = products.OrderBy(s => s.Description);
                //    break;
                //case "date_desc":
                //    products = products.OrderByDescending(s => s.Description);
                //    break;
                default:  // Name ascending 
                    products = products.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));


        }

        public async Task<JsonResult> AddToCart(string Id)
        {
            var userId = User.Identity.GetUserId();
            var _currentOrder = data.Orders.Where(i => i.AspNetUserId == userId && i.OrderStatus == "new");

            OrderDetail _orderDetail = new OrderDetail();

            var _product = await data.Products.FindAsync(Id);

            if (_currentOrder.Any())
            {
                _orderDetail.Id = Guid.NewGuid().ToString();
                _orderDetail.OrdersId = _currentOrder.FirstOrDefault().Id;
                _orderDetail.Amount = _product.Amount;
                _orderDetail.Quantity = 1;
                _orderDetail.ProductsId = Id;
                _orderDetail.DateAdded = DateTime.UtcNow;
                data.OrderDetails.Add(_orderDetail);

            }
            else
            {


                Order _order = new Order()
                {

                    Id = Guid.NewGuid().ToString(),
                    AspNetUserId = userId,
                    DateCreated = DateTime.UtcNow,
                    OrderStatus = "new",

                };

                _orderDetail.Id = Guid.NewGuid().ToString();
                _orderDetail.OrdersId = _order.Id;
                _orderDetail.Amount = _product.Amount;
                _orderDetail.Quantity = 1;
                _orderDetail.ProductsId = Id;
                _orderDetail.DateAdded = DateTime.UtcNow;



                _order.OrderDetails.Add(_orderDetail);

                data.Orders.Add(_order);

            }

            await data.SaveChangesAsync();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> RemoveFromCart(string Id)
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Cart()
        {

            var _currentOrderId = "296b9871-beb5-41d8-b0f8-8865a3172a06";
            var _productList = await dataLayer.getProductsListByOrderId(_currentOrderId);
            decimal _total = 0;
            foreach (var o in _productList)
            {

                if (o.Quantity == 1)
                {
                    _total = _total + o.Amount;
                }
                else {

                    _total = _total + (o.Amount*o.Quantity);

                }
                
            }
            
            CartModel _model = new Models.CartModel()
            {
                OrdersId = _currentOrderId,
                ProductList = _productList,
                Total = _total

            };



            return View(_model);

        }

        public async Task<ActionResult> Shipping()
        {
            var userId = User.Identity.GetUserId();
          
            var _shippingAddressList = await (from o in data.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId)
                                              select o).ToListAsync();
            var _currentShippingAddress = (from o in _shippingAddressList.Where(i => i.IsDefault == true) select o).FirstOrDefault();

            var _shipping = new ShippingModel()
            {
                CurrentShippingAddress = _currentShippingAddress,
                ShippingAddressList = _shippingAddressList

            };

            return View(_shipping);
        }

        public ActionResult Card()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(FormCollection collection)
        {

            var Billing = new AuthBillingAddress();
            Billing.addrLine1 = "123 test st";
            Billing.city = "Columbus";
            Billing.zipCode = "43123";
            Billing.state = "OH";
            Billing.country = "USA";
            Billing.name = "Testing Tester";
            Billing.email = "example@2co.com";
            Billing.phoneNumber = "5555555555";

            var _charge = new ChargeAuthorizeServiceOptions()
            {
                total = (decimal)1.00,
                currency = "USD",
                merchantOrderId = "123",
                token = collection["token"].ToString(),
                billingAddr = Billing

            };


            _payment.CheckOutTwoCheckOut(_charge);

            return View();

        }






    }
}