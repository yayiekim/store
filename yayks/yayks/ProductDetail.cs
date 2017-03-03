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
    
    public partial class ProductDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductDetail()
        {
            this.ProductDetailImages = new HashSet<ProductDetailImage>();
        }
    
        public string Id { get; set; }
        public string ProductId { get; set; }
        public Nullable<int> ProductColorId { get; set; }
        public Nullable<int> ProductMeasurementId { get; set; }
        public Nullable<decimal> Length { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Height { get; set; }
    
        public virtual ProductColor ProductColor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductDetailImage> ProductDetailImages { get; set; }
        public virtual ProductMeasurement ProductMeasurement { get; set; }
        public virtual Product Product { get; set; }
    }
}