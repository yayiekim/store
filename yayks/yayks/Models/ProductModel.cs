using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace yayks.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Measurement { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public List<string> Images { get; set; }
        public bool IsSelected { get; set; }
        public string CartId { get; set; }
        public string Status { get; set; }

    }

    public class NewProductModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public int ProductBrandId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public int MeasurementId { get; set; }
        [Required]
        public int ColordId { get; set; }
        [Required]
        public double Lenght { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public List<CheckBoxModel> Categories { get; set; }
        [Required]
        public List<CheckBoxModel> Genders { get; set; }
        public List<NewIMageModel> Images { get; set; }
        public string ForDeleteImages { get; set; }

    }




}