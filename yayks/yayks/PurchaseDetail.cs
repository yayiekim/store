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
    
    public partial class PurchaseDetail
    {
        public string Id { get; set; }
        public string PurchaseId { get; set; }
        public string ProductId { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public string PurchaseDetailStatus { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}
