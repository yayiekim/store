using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class ShippingModel
    {
        public CustomerShippingAddress CurrentShippingAddress { get; set; }
        public List<CustomerShippingAddress> ShippingAddressList { get; set; }

    }
}