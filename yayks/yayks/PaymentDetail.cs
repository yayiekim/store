//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace yayks
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentDetail
    {
        public string Id { get; set; }
        public string PaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentRef { get; set; }
        public string PayerName { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Remarks { get; set; }
    
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
