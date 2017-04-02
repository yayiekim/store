using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class CartModel
    {

        public List<ProductModel> ProductList { get; set; }
        public decimal Total { get; set; }
        public string OrdersId { get; set; }

    }
}