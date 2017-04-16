using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class StatusModels
    {
    }

    public class PaymentGatewayResponse
    {

        public string refNo { get; set; }
        public decimal? amount { get; set; }
        public string PaymentStatus { get; set; }
        public string message { get; set; }
        public string responseStatus { get; set; }
    }
 

}