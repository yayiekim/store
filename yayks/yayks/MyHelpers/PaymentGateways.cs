using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TwoCheckout;

namespace yayks.MyHelpers
{
    public class PaymentGateways
    {

        public bool CheckOutTwoCheckOut(ChargeAuthorizeServiceOptions charge)
        {
            TwoCheckoutConfig.SellerID = ConfigurationManager.AppSettings["TwoCheckOutSellerId"];
            TwoCheckoutConfig.PrivateKey = ConfigurationManager.AppSettings["TwoCheckOutPrivateKey"]; ;
            TwoCheckoutConfig.Sandbox = true;


            try
            {
                var Charge = new ChargeService();
                Charge.Authorize(charge);
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }

        }

    }
}