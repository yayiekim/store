using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class PaymentGatewayModels
    {


    }

    public class AuthBillingAddressModel
    {
        [Required]
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string zipCode { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public ChargeAuthorizeServiceOptionsModel charge { get; set; }

    }

    public class ChargeAuthorizeServiceOptionsModel
    {

        public decimal total { get; set; }
        public string currency { get; set; }
        public string merchantOrderId { get; set; }
        public string token { get; set; }



    }

}