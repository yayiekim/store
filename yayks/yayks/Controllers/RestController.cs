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

            var _tmp = await (from o in data.Orders.Where(i => i.AspNetUserId == _userId)
                              join p in data.OrderDetails on o.Id equals p.OrdersId into op
                              select new
                              {
                                  Count = op.Sum(i => i.Quantity)

                              }).SingleOrDefaultAsync();


            return Json(_tmp, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public async Task<JsonResult> CreateShipping(CustomerShippingAddress customerShippingAddress)
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
        public async Task<JsonResult> EditShipping(CustomerShippingAddress customerShippingAddress)
        {
            if (ModelState.IsValid)
            {
                data.Entry(customerShippingAddress).State = EntityState.Modified;
                await data.SaveChangesAsync();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DeleteShipping(string id)
        {
            CustomerShippingAddress customerShippingAddress = await data.CustomerShippingAddresses.FindAsync(id);
            data.CustomerShippingAddresses.Remove(customerShippingAddress);
            await data.SaveChangesAsync();
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        #endregion



    }
}