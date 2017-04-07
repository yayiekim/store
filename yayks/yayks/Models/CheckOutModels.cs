using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class CheckOutModels
    {
        public CartModel Cart { get; set; }
        public OrderDetailShippingAddress ShippingAddress { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        public string CardToken { get; set; }

    }




}