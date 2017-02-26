using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class ProductGenders
    {
        public string Gender { get; set; }

        public List<ProductGenders> GenderList()
        {
            List<ProductGenders> list = new List<Models.ProductGenders>();

            list.Add(new ProductGenders() { Gender = "Male" });
            list.Add(new ProductGenders() { Gender = "Female" });

            return list;
        }

    }
    
}