using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using yayks.Models;

namespace yayks.MyHelpers
{
    public class DataLayer
    {
        Entities data = new Entities();

        public async Task<List<ProductModel>> getProductsListFromCart(string userId)
        {
                 var _orderDetail = await (from o in data.Carts.Where(i => i.AspNetUserId == userId)
                                      join r in data.Products on o.ProductId equals r.Id
                                      join rr in data.ProductDetails on r.Id equals rr.ProductId
                                      select new ProductModel
                                      {

                                          Id = r.Id,
                                          Amount = r.Amount,
                                          Quantity = o.Quantity,
                                          Brand = r.ProductBrand.Name,
                                          Color = rr.ProductColor.ProductColorName,
                                          Description = r.Description,
                                          Name = r.ProductName,
                                          Measurement = rr.ProductMeasurement.MeasurementValue.ToString(),
                                          Images = rr.ProductDetailImages.ToList().Select(i=>i.ImageUrl).ToList(),
                                          IsSelected = o.IsSelected,
                                          CartId = o.Id
                                       
                                      }).ToListAsync();

            return _orderDetail;

        }

    }
}