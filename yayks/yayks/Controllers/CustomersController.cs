using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
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

        public ActionResult Cart()
        {

            return View();

        }

    
        public ActionResult Card()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(FormCollection collection)
        {

            //var _charge = new ChargeAuthorizeServiceOptionsModel()
            //{
            //    total = (decimal)1.00,
            //    currency = "USD",
            //    merchantOrderId = "123",
            //    token = "YjdhN2Q0ZDItZDEzYS00Y2RlLWI3MDUtYjkzYzZlMjA2OWY2"
            //};


            //AuthBillingAddressModel _address = new AuthBillingAddressModel();

            //_address.charge = _charge;


            //var _billingAddress = new AuthBillingAddress()
            //{
            //    addrLine1 = billingAddress.addressLine1,
            //    addrLine2 = billingAddress.addressLine2,
            //    city = billingAddress.city,
            //    country = billingAddress.country,
            //    zipCode = billingAddress.zipCode,
            //    email = billingAddress.email,
            //    name = billingAddress.name,
            //    phoneNumber = billingAddress.phoneNumber,
            //    state = billingAddress.state

            //};

            //var _charge = new ChargeAuthorizeServiceOptions()
            //{
            //    billingAddr = _billingAddress,
            //    total = billingAddress.charge.total,
            //    currency = billingAddress.charge.currency,
            //    merchantOrderId = billingAddress.charge.merchantOrderId,
            //    token = billingAddress.charge.token

            //};

            //_payment.CheckOutTwoCheckOut(_charge);

            return View();

        }


    }
}