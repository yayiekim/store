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

        
        public async Task<JsonResult> AddToCart(string Id)
        {
            var userId = User.Identity.GetUserId();


            var itemCount = await dataLayer.GetProductCountInMyCart(Id, userId);


            if (itemCount < 1)
            {
                return Json("out of stock", JsonRequestBehavior.AllowGet);
            }
            else
            {                
                return Json(await dataLayer.AddToCart(Id, userId), JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public async Task<bool> RemoveFromCart(string Id)
        {

            return await dataLayer.RemoveFromCart(Id);

        }

        public async Task<ActionResult> Cart()
        {
            var userId = User.Identity.GetUserId();
            var _productList = await dataLayer.getProductsListFromCart(userId, null);
            decimal _total = 0;

            foreach (var o in _productList)
            {
                if (o.IsSelected)
                {

                    o.IsSelected = await dataLayer.setCartItemSelection(o.CartId, o.IsSelected, userId);

                    if (!o.IsSelected)
                    {
                        o.Status = "out of stock";
                    }
                      

                }

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

        public async Task<ActionResult> Shipping()
        {

            var userId = User.Identity.GetUserId();

            var _shippingAddressList = await (from o in data.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId)
                                              select o).ToListAsync();


            ViewBag.ShippingAddresses = _shippingAddressList;

            var _model = new CheckOutModels();
           

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
                Cart = await dataLayer.getProductsListFromCart(userId, true),

            };

            return View(_model);
        }

        [HttpPost]
        public async Task<ActionResult> Pay(CheckOutModels model, FormCollection collection)
        {

            var userId = User.Identity.GetUserId();

            //Credit Card
            var _totalAmount = Convert.ToDouble((from o in model.Cart
                                                 group o by new { product = o.Id, amount = o.Amount * o.Quantity } into g
                                                 select new
                                                 {

                                                     Total = g.Key.amount

                                                 }).ToList().Sum(i => i.Total));




            var Billing = new AuthBillingAddress()
            {
                addrLine1 = model.BillingAddress.addressLine1,
                city = model.BillingAddress.city,
                state = model.BillingAddress.state,
                country = model.BillingAddress.country,
                name = model.BillingAddress.name,
                email = User.Identity.GetUserName(),
                phoneNumber = "N/A",
                zipCode = "0",

            };



            var _charge = new ChargeAuthorizeServiceOptions()
            {
                total = (decimal)_totalAmount,
                currency = "USD",
                merchantOrderId = "1",
                token = collection["token"].ToString(),
                billingAddr = Billing

            };


            var _paymentResult = new PaymentGatewayResponse();


            if (model.PaymentMethodId == 1)
            {
                _paymentResult = _payment.CheckOutTwoCheckOut(_charge);

            }


            model.ShippingAddress = await data.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId && i.IsDefault == true).FirstOrDefaultAsync();


            if (_paymentResult.PaymentStatus != "failed")
            {
                model.PaymentRef = _paymentResult.refNo;
                model.TotalAmount = _paymentResult.amount ?? 0;
                model.UserId = userId;
                model.PaymentStatus = _paymentResult.PaymentStatus;

                dataLayer.createOrder(model);

                ViewBag.paymentStat = "Success";
            }
            else
            {
                ViewBag.paymentStat = "Failed";
            }

           
            return View(model);
        }

        public ActionResult Card()
        {
            return View();
        }


    }
}