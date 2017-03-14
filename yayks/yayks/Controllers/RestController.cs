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
    }
}