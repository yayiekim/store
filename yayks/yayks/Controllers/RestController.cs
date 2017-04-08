using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using yayks.Models;
using yayks.MyHelpers;

namespace yayks.Controllers
{
    public class RestController : Controller
    {

        Entities data = new Entities();
        AzureBlob azureBlob = new AzureBlob();
        CommonModels common = new CommonModels();

        // GET: Rest
        public ActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> getMeasurement(List<int> Categories)
        {

            var tmp = await (from p in data.ProductMeasurements.
                             Where(a => Categories.Contains(a.ProductCategoryId))
                             select new {
                                 
                                 Id = p.Id,
                                 Name = p.MeasurementName,
                                 Value = p.MeasurementValue,


                             }).ToListAsync();

            return Json(tmp, JsonRequestBehavior.AllowGet);
        }


        #region Customers

        public async Task<JsonResult> getCartCount()
        {

            var _userId = User.Identity.GetUserId();

            var _tmp = await data.Carts.Where(i => i.AspNetUserId == _userId).CountAsync();
                              

            return Json(_tmp, JsonRequestBehavior.AllowGet);


        }

        public async Task<JsonResult> getShipping()
        {
            var userId = User.Identity.GetUserId();
            var _res = await data.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId).ToListAsync();


            return Json(_res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> createShipping(CustomerShippingAddress customerShippingAddress)
        {

            var userId = User.Identity.GetUserId();

            customerShippingAddress.AspNetUserId = userId;
            customerShippingAddress.Id = Guid.NewGuid().ToString();

            if (ModelState.IsValid)
            {
                data.CustomerShippingAddresses.Add(customerShippingAddress);
                await data.SaveChangesAsync();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> editShipping(CustomerShippingAddress customerShippingAddress)
        {
            if (ModelState.IsValid)
            {
                data.Entry(customerShippingAddress).State = EntityState.Modified;
                await data.SaveChangesAsync();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> deleteShipping(string id)
        {
            var _defaultId = "";
            var userId = User.Identity.GetUserId();

            CustomerShippingAddress customerShippingAddress = await data.CustomerShippingAddresses.FindAsync(id);

            var _NoDefaultAddress = await data.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId && i.Id != id).ToListAsync();


            if (customerShippingAddress.IsDefault)
            {

                if (_NoDefaultAddress.Count >= 1)
                {
                    _defaultId = _NoDefaultAddress.Last().Id;
                    _NoDefaultAddress.Last().IsDefault = true;
                }
                else {

                    _defaultId = "0";

                }
                

            }
            else {

                _defaultId = _NoDefaultAddress.Where(i => i.IsDefault == true).Single().Id;

            }

            data.CustomerShippingAddresses.Remove(customerShippingAddress);
            
            await data.SaveChangesAsync();
            return Json(_defaultId, JsonRequestBehavior.AllowGet);
                      
        }


        #endregion



    }
}