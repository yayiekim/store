using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        Entities data = new Entities();
        AzureBlob azureBlob = new AzureBlob();
        CommonModels common = new CommonModels();
        // GET: AdminProducts
        public ActionResult Index()
        {
            return View();
        }
         
        public ActionResult Products(string sortOrder, string currentFilter, string searchString, int? page)
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
                               Amount = o.Amount,
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

        public ActionResult AddNewProduct()
        {

            NewProductModel model = new Models.NewProductModel();

            var genders = from o in common.GetGenders()
                          select new CheckBoxModel
                          {
                              Id = o,
                              IsSelected = false,
                              Label = o

                          };


            var categories = from o in data.ProductCategories
                             select new CheckBoxModel
                             {
                                 Id = o.Id.ToString(),
                                 IsSelected = false,
                                 Label = o.CategoryName

                             };


            ViewBag.Colors = from o in data.ProductColors
                             select new
                             {
                                 Id = o.Id,
                                 Color = o.ProductColorName

                             };


            model.Genders = genders.ToList();
            model.Categories = categories.ToList();


            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewProduct(NewProductModel product, IEnumerable<HttpPostedFileBase> files)
        {



            Product dbProduct = new Product()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = product.Amount,
                Description = product.Description,
                ProductName = product.Name,

            };

            //many to many insert mapping
            List<string> tmpint = product.Categories
                                    .Where(i => i.IsSelected).Select(i => i.Id).ToList();

            var tmp = await (from p in data.ProductCategories
                             where tmpint.Contains(p.Id.ToString())
                             select p).ToListAsync();


            foreach (var o in tmp)
            {

                dbProduct.ProductCategories.Add(o);

            }

            ProductDetail dbProductDetail = new ProductDetail()
            {
                Id = Guid.NewGuid().ToString(),
                ProductMeasurementId = product.MeasurementId,
                ProductColorId = product.ColordId

            };


            foreach (var x in files)
            {

                var res = await azureBlob.UploadImageAsync(x);
                ProductDetailImage dbProductImage = new ProductDetailImage()
                {

                    Id = Guid.NewGuid().ToString(),
                    BaseAddress = res.BaseUrl,
                    ImagePath = res.URL

                };


                dbProductDetail.ProductDetailImages.Add(dbProductImage);

            }


            foreach (var o in product.Genders)
            {
                if (o.IsSelected)
                {
                    ProductsInGender dbProdcutsInGender = new ProductsInGender()
                    {
                        Gender = o.Id,
                        Product = dbProduct
                    };

                    data.ProductsInGenders.Add(dbProdcutsInGender);
                }

            }


            dbProduct.ProductDetails.Add(dbProductDetail);
            data.Products.Add(dbProduct);
            await data.SaveChangesAsync();

            return RedirectToAction("Products");
        }

    }
}