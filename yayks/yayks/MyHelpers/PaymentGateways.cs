using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TwoCheckout;
using yayks.Models;

namespace yayks.MyHelpers
{
    public class PaymentGateways
    {

        public PaymentGatewayResponse CheckOutTwoCheckOut(ChargeAuthorizeServiceOptions charge)
        {
            TwoCheckoutConfig.SellerID = ConfigurationManager.AppSettings["TwoCheckOutSellerId"];
            TwoCheckoutConfig.PrivateKey = ConfigurationManager.AppSettings["TwoCheckOutPrivateKey"]; ;
            TwoCheckoutConfig.Sandbox = true;


            var _res = new PaymentGatewayResponse();


            try
            {

                var Charge = new ChargeService();
                var result = Charge.Authorize(charge);

                _res.amount = result.total;
                _res.refNo = result.transactionId.ToString();
                _res.responseStatus = result.responseCode;
                _res.message = result.responseMsg;

                if (charge.total == result.total)
                {

                    _res.PaymentStatus = "paid";

                }
                else if (charge.total > result.total)
                {
                    _res.PaymentStatus = "partial";

                }

                return _res;

            }
            catch (Exception ex)
            {
                _res.responseStatus = "failed";
                _res.message = ex.Message;

                return _res;
            }

        }

    }
}