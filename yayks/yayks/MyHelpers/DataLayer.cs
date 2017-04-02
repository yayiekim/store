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

        public async Task<List<ProductModel>> getProductsListByOrderId(string id)
        {
            var _orderDetail = await (from o in data.Orders.Where(i => i.Id == id)
                                      join p in data.OrderDetails on o.Id equals p.OrdersId
                                      join r in data.Products on p.ProductsId equals r.Id
                                      join rr in data.ProductDetails on r.Id equals rr.ProductId
                                      select new ProductModel
                                      {

                                          Id = r.Id,
                                          Amount = r.Amount,
                                          Quantity = p.Quantity,
                                          Brand = r.ProductBrand.Name,
                                          Color = rr.ProductColor.ProductColorName,
                                          Description = r.Description,
                                          Name = r.ProductName,
                                          Measurement = rr.ProductMeasurement.MeasurementValue.ToString(),
                                          Images = rr.ProductDetailImages.ToList().Select(i=>i.ImageUrl).ToList(),
                                          IsSelected = false

                                      }).ToListAsync();

            return _orderDetail;

        }

    }
}