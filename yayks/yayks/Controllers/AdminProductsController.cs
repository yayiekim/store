using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PagedList;
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

            ViewBag.Brands = from o in data.ProductBrands
                             select new
                             {
                                 Id = o.Id,
                                 Name = o.Name

                             };

            model.Genders = genders.ToList();
            model.Categories = categories.ToList();


            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewProduct(NewProductModel product, List<HttpPostedFileBase> files)
        {
            var UserId = User.Identity.GetUserId();

            Product dbProduct = new Product()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = product.Amount,
                Description = product.Description,
                ProductName = product.Name,
                ProductBrandId = product.ProductBrandId,
                CreatedByUserId = UserId,
                DateCreated = DateTime.UtcNow


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
                ProductColorId = product.ColordId,
                Length = product.Lenght,
                Width = product.Width,
                Height = product.Height,
                Weight = product.Weight

            };

            if (files.Count() >= 1 && files[0] != null)
            {
                foreach (var x in files)
                {

                    var id = Guid.NewGuid().ToString();

                    NewIMageModel img = new Models.NewIMageModel()
                    {
                        Id = id,
                        FileExtention = x.ContentType,
                        File = x,

                    };

                    var res = await azureBlob.UploadImageAsync(img);


                    ProductDetailImage dbProductImage = new ProductDetailImage()
                    {

                        Id = img.Id,
                        ImageUrl = res.URL,
                        FileName = res.FileName,
                        ThumbUrl = res.ThumbUrl

                    };


                    dbProductDetail.ProductDetailImages.Add(dbProductImage);

                }
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

        public async Task<ActionResult> Edit(string Id)
        {
            ViewBag.Colors = await (from o in data.ProductColors
                             select new
                             {
                                 Id = o.Id,
                                 Name = o.ProductColorName

                             }).ToListAsync();


            var _product = await data.Products.FindAsync(Id);
            List<CheckBoxModel> cbCategoriesList = new List<CheckBoxModel>();
            List<CheckBoxModel> cbGenderList = new List<CheckBoxModel>();

            foreach (var x in await data.ProductCategories.ToListAsync())
            {
                var IsSelected = false;

                if (_product.ProductCategories.Where(i => i.Id == x.Id).Count() == 1)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }

                CheckBoxModel cb = new Models.CheckBoxModel()
                {
                    Id = x.Id.ToString(),
                    IsSelected = IsSelected,
                    Label = x.CategoryName
                };

                cbCategoriesList.Add(cb);
            }


            foreach (var x in common.GetGenders())
            {
                var IsSelected = false;

                if (_product.ProductsInGenders.Where(i => i.Gender == x).Count() == 1)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }

                CheckBoxModel cb = new CheckBoxModel()
                {
                    Id = x,
                    IsSelected = IsSelected,
                    Label = x
                };

                cbGenderList.Add(cb);
            }

            var _tmpImages = from o in _product.ProductDetails
                             select o.ProductDetailImages;

            List<NewIMageModel> _images = new List<NewIMageModel>();

            foreach (var x in _tmpImages.ToList())
            {

                foreach (var y in x)
                {
                    NewIMageModel _imgModel = new NewIMageModel()
                    {
                        Id = y.Id,
                        Url = y.ImageUrl
                    };

                    _images.Add(_imgModel);
                }

            }

            NewProductModel model = new NewProductModel()
            {
                Id = _product.Id,
                Amount = _product.Amount,
                Description = _product.Description,
                Name = _product.ProductName,
                ColordId = _product.ProductDetails.Select(i => i.ProductColor.Id).First(),
                MeasurementId = _product.ProductDetails.Select(i => i.ProductMeasurement.Id).First(),
                Categories = cbCategoriesList,
                Genders = cbGenderList,
                Images = _images,
                Lenght = _product.ProductDetails.Select(i => i.Length).First(),
                Width = _product.ProductDetails.Select(i => i.Width).First(),
                Height = _product.ProductDetails.Select(i => i.Height).First(),
                Weight = _product.ProductDetails.Select(i => i.Weight).First(),

            };



            string[] catArray = cbCategoriesList.Where(i => i.IsSelected).Select(i => i.Id).ToArray();

            ViewBag.Measurements = await (from p in data.ProductMeasurements.
                           Where(a => catArray.Contains(a.ProductCategoryId.ToString()))
                                          select new
                                          {

                                              Id = p.Id,
                                              Name = p.MeasurementName,
                                              Value = p.MeasurementValue,


                                          }).ToListAsync();

            ViewBag.Brands = from o in data.ProductBrands
                             select new
                             {
                                 Id = o.Id,
                                 Name = o.Name

                             };


            ViewBag.Colors = from o in data.ProductColors
                             select new
                             {
                                 Id = o.Id,
                                 Color = o.ProductColorName

                             };



            return View(model);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(NewProductModel product, List<HttpPostedFileBase> files)
        {

            var _data = await data.Products.FindAsync(product.Id);

            _data.ProductName = product.Id;
            _data.Amount = product.Amount;
            _data.Description = product.Description;
            _data.ProductName = product.Name;

            //many to many insert mapping Categories
            List<string> tmpCategoryInt = product.Categories
                                    .Where(i => i.IsSelected).Select(i => i.Id).ToList();

            var tmpCategory = await (from p in data.ProductCategories
                                     where tmpCategoryInt.Contains(p.Id.ToString())
                                     select p).ToListAsync();


            foreach (var x in data.ProductCategories)
            {
                if (!tmpCategory.Contains(x))
                {
                    _data.ProductCategories.Remove(x);

                }
                else
                {

                    if (!_data.ProductCategories.Contains(x))
                    {
                        _data.ProductCategories.Add(x);
                    }

                }


            }


            foreach (var o in tmpCategory)
            {

                if (!_data.ProductCategories.Contains(o) && tmpCategoryInt.Contains(o.Id.ToString()))
                {
                    _data.ProductCategories.Add(o);

                }
                else if (_data.ProductCategories.Contains(o) && !tmpCategoryInt.Contains(o.Id.ToString()))
                { }

            }


            //many to many insert mapping Genders
            List<string> tmpGenderName = product.Genders
                                    .Where(i => i.IsSelected).Select(i => i.Id).ToList();


            foreach (var x in common.GetGenders())
            {
                if (!tmpGenderName.Contains(x) && _data.ProductsInGenders.Where(i => i.Gender == x).Any())
                {
                    _data.ProductsInGenders.Remove(_data.ProductsInGenders.Where(i => i.Gender == x).Single());
                }
                else if (tmpGenderName.Contains(x) && !_data.ProductsInGenders.Where(i => i.Gender == x).Any())
                {

                    _data.ProductsInGenders.Add(new ProductsInGender() { Gender = x, ProductId = product.Id });


                }


            }

            //Product details
            foreach (var x in _data.ProductDetails)
            {
                x.ProductColorId = product.ColordId;
                x.Length = product.Lenght;
                x.Width = product.Width;
                x.Height = product.Height;
                x.Weight = product.Weight;

            }

            //Add new images
            if (files.Count() >= 1 && files[0] != null)
            {
                foreach (var x in files)
                {

                    var id = Guid.NewGuid().ToString();

                    NewIMageModel img = new Models.NewIMageModel()
                    {
                        Id = id,
                        FileExtention = x.ContentType,
                        File = x,

                    };

                    var res = await azureBlob.UploadImageAsync(img);


                    ProductDetailImage dbProductImage = new ProductDetailImage()
                    {

                        Id = img.Id,
                        ProductDetailsId = _data.ProductDetails.First().Id,
                        ImageUrl = res.URL,
                        FileName = res.FileName,
                        ThumbUrl = res.ThumbUrl

                    };


                    data.ProductDetailImages.Add(dbProductImage);

                }
            }

            //remove images
            if (product.ForDeleteImages != null)
            {
                string[] imgs = product.ForDeleteImages.Split(',');


                foreach (var x in imgs)
                {

                    var y = await data.ProductDetailImages.FindAsync(x);

                    data.ProductDetailImages.Remove(y);

                }

                try
                {
                    await azureBlob.DeleteBlobs(product.ForDeleteImages.Split(','));
                }
                catch
                {

                }

            }


            data.Entry(_data).State = EntityState.Modified;
            await data.SaveChangesAsync();

            return RedirectToAction("Products");
        }


        public async Task<ActionResult> Delete(string Id)
        {

            ViewBag.Colors = await (from o in data.ProductColors
                             select new
                             {
                                 Id = o.Id,
                                 Name = o.ProductColorName

                             }).ToListAsync();


            var _product = await data.Products.FindAsync(Id);
            List<CheckBoxModel> cbCategoriesList = new List<CheckBoxModel>();
            List<CheckBoxModel> cbGenderList = new List<CheckBoxModel>();

            foreach (var x in await data.ProductCategories.ToListAsync())
            {
                var IsSelected = false;

                if (_product.ProductCategories.Where(i => i.Id == x.Id).Count() == 1)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }

                CheckBoxModel cb = new Models.CheckBoxModel()
                {
                    Id = x.Id.ToString(),
                    IsSelected = IsSelected,
                    Label = x.CategoryName
                };

                cbCategoriesList.Add(cb);
            }


            foreach (var x in common.GetGenders())
            {
                var IsSelected = false;

                if (_product.ProductsInGenders.Where(i => i.Gender == x).Count() == 1)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }

                CheckBoxModel cb = new CheckBoxModel()
                {
                    Id = x,
                    IsSelected = IsSelected,
                    Label = x
                };

                cbGenderList.Add(cb);
            }

            var _tmpImages = from o in _product.ProductDetails
                             select o.ProductDetailImages;

            List<NewIMageModel> _images = new List<NewIMageModel>();

            foreach (var x in _tmpImages)
            {

                foreach (var y in x)
                {
                    NewIMageModel _imgModel = new NewIMageModel()
                    {
                        Id = y.Id,
                        Url = y.ImageUrl
                    };

                    _images.Add(_imgModel);
                }

            }

            NewProductModel model = new NewProductModel()
            {
                Id = _product.Id,
                Amount = _product.Amount,
                Description = _product.Description,
                Name = _product.ProductName,
                ColordId = _product.ProductDetails.Select(i => i.ProductColor.Id).First(),
                MeasurementId = _product.ProductDetails.Select(i => i.ProductMeasurement.Id).First(),
                Categories = cbCategoriesList,
                Genders = cbGenderList,
                Images = _images

            };



            string[] catArray = cbCategoriesList.Where(i => i.IsSelected).Select(i => i.Id).ToArray();

            ViewBag.Measurements = await (from p in data.ProductMeasurements.
                           Where(a => catArray.Contains(a.ProductCategoryId.ToString()))
                                          select new
                                          {

                                              Id = p.Id,
                                              Name = p.MeasurementName,
                                              Value = p.MeasurementValue,


                                          }).ToListAsync();

            ViewBag.Brands = await (from o in data.ProductBrands
                             select new
                             {
                                 Id = o.Id,
                                 Name = o.Name

                             }).ToListAsync();


            ViewBag.Colors = await (from o in data.ProductColors
                             select new
                             {
                                 Id = o.Id,
                                 Color = o.ProductColorName

                             }).ToListAsync();



            return View(model);



        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string Id)
        {
            var x = await data.Products.FindAsync(Id);

            foreach (var y in x.ProductDetails)
            {

                string[] c = y.ProductDetailImages.Select(i => i.Id).ToArray();

                try
                {
                    await azureBlob.DeleteBlobs(c);

                }
                catch { }


            }



            data.Products.Remove(x);
            await data.SaveChangesAsync();



            return RedirectToAction("Products");
        }

    }
}