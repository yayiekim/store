using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class CheckOutModels
    {
        public List<ProductModel> Cart { get; set; }
        public CustomerShippingAddress ShippingAddress { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        public string UserId { get; set; }
        public string CardToken { get; set; }

    }




}