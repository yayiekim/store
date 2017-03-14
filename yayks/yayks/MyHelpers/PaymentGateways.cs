using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwoCheckout;

namespace yayks.MyHelpers
{
    public class PaymentGateways
    {

        public bool CheckOutTwoCheckOut(ChargeAuthorizeServiceOptions charge)
        {
            TwoCheckoutConfig.SellerID = "203205815";
            TwoCheckoutConfig.PrivateKey = "50870A0E-C0A1-4AE4-8FDD-590EBBE29F6F";


            try
            {
                var Charge = new ChargeService();
                Charge.Authorize(charge);
                return true;

            }
            catch
            {

                return false;
            }

        }

    }
}