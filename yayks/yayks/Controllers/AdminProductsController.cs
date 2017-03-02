using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using yayks.Helpers;
using yayks.Models;

namespace yayks.Controllers
{
    public class AdminProductsController : Controller
    {
        AzureBlob azureBlob = new AzureBlob();

        // GET: AdminProducts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {

            return View();
        }

        public ActionResult AddNewProduct()
        {

            return View();
        }

        public ActionResult AddNewProduct(NewProductModel product)
        {

            return RedirectToAction("Products");
        }

    }
}