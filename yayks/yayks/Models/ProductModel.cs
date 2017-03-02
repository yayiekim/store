using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public List<string> Images { get; set; }

    }

    public class NewProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Gender { get; set; }
        public int CategoryId { get; set; }
        public int MeasurementId { get; set; }
        public List<HttpPostedFileBase> Images { get; set; }

    }




}