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
            var res = await data.Carts.Where(i=>i.ProductId == Id).ToListAsync();

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
                    AspNetUserId = userId

                };

                data.Carts.Add(_myCart);
            }



           

            await data.SaveChangesAsync();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
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

        public async Task<ActionResult> Cart()
        {
            var userId = User.Identity.GetUserId();
            var _productList = await dataLayer.getProductsListFromCart(userId);
            decimal _total = 0;
            foreach (var o in _productList)
            {

                if (o.Quantity == 1)
                {
                    _total = _total + o.Amount;
                }
                else
                {

                    _total = _total + (o.Amount * o.Quantity);

                }

            }

            CartModel _model = new Models.CartModel()
            {
                ProductList = _productList

            };



            return View(_model);

        }

        public async Task<ActionResult> Shipping(CartModel model)
        {

            var userId = User.Identity.GetUserId();

            var _shippingAddressList = await (from o in data.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId)
                                              select o).ToListAsync();


            ViewBag.ShippingAddresses = _shippingAddressList;

            var _model = new CheckOutModels()
            {
                Cart = model,

            };


            return View(_model);
        }

        public async Task<ActionResult> PaymentMethod(CheckOutModels model)
        {
            var userId = User.Identity.GetUserId();

            //update shipping address if has changes and add if new
            if (model.ShippingAddress.Id == null)
            {

                var _newAddress = new CustomerShippingAddress()
                {
                    Id = Guid.NewGuid().ToString(),
                    AspNetUserId = userId,
                    City = model.ShippingAddress.City,
                    IsDefault = model.ShippingAddress.IsDefault,
                    Line1 = model.ShippingAddress.Line1,
                    Line2 = model.ShippingAddress.Line2,
                    State = model.ShippingAddress.State

                };

                //unselect all except this
                if (model.ShippingAddress.IsDefault)
                {
                    var _defaults = await data.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId).ToListAsync();

                    foreach (var o in _defaults)
                    {
                        if (o.Id != model.ShippingAddress.Id)
                        {
                            o.IsDefault = false;

                        }

                    }
                }



                data.CustomerShippingAddresses.Add(_newAddress);
                await data.SaveChangesAsync();

            }
            else
            {

                bool hasChanges = false;
                bool defaultChanges = false;

                var _address = await data.CustomerShippingAddresses.FindAsync(model.ShippingAddress.Id);


                //if address changes
                if (_address.IsDefault != model.ShippingAddress.IsDefault)
                {
                    _address.IsDefault = model.ShippingAddress.IsDefault;
                    hasChanges = true;
                    defaultChanges = true;
                }
                if (_address.Line1 != model.ShippingAddress.Line1)
                {
                    _address.Line1 = model.ShippingAddress.Line1;
                    hasChanges = true;
                }
                if (_address.Line2 != model.ShippingAddress.Line2)
                {
                    _address.Line2 = model.ShippingAddress.Line2;
                    hasChanges = true;
                }
                if (_address.City != model.ShippingAddress.City)
                {
                    _address.City = model.ShippingAddress.City;
                    hasChanges = true;
                }
                if (_address.State != model.ShippingAddress.State)
                {
                    _address.State = model.ShippingAddress.State;
                    hasChanges = true;
                }

                //unselect all except this
                if (defaultChanges)
                {
                    var _defaults = await data.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId).ToListAsync();

                    foreach (var o in _defaults)
                    {
                        if (o.Id != model.ShippingAddress.Id)
                        {
                            o.IsDefault = false;

                        }

                    }
                }

                if (hasChanges)
                {
                    await data.SaveChangesAsync();

                }
            }


            var _model = new CheckOutModels()
            {
                Cart = model.Cart,
                ShippingAddress = model.ShippingAddress

            };

            return View(_model);
        }

        [HttpPost]
        public async Task<ActionResult> Pay(CheckOutModels model)
        {
            var _model = new CheckOutModels()
            {
                Cart = model.Cart,
                ShippingAddress = model.ShippingAddress,
                PaymentDetail = model.PaymentDetail
            };


            var Billing = new AuthBillingAddress();

            var _charge = new ChargeAuthorizeServiceOptions()
            {
                total = model.PaymentDetail.PaymentAmount,
                currency = "USD",
                merchantOrderId = "123",
                token = model.CardToken,
                billingAddr = Billing

            };


            _payment.CheckOutTwoCheckOut(_charge);
            return View(model);
        }

        public ActionResult Card()
        {
            return View();
        }


    }
}