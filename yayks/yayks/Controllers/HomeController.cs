using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yayks.Models;

namespace yayks.Controllers
{
    public class HomeController : Controller
    {
        Entities data = new Entities();
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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
                               Images = o.ProductDetails.Select(i => i.ProductDetailImages.FirstOrDefault()).Select(i => i.ThumbUrl).ToList()
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

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));


        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}