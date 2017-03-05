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
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public List<string> Images { get; set; }

    }

    public class NewProductModel
    {
        public string Id { get; set; }
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
        public List<CheckBoxModel> Categories { get; set; }
        [Required]
        public List<CheckBoxModel> Genders { get; set; }
        public List<string> Images { get; set; }

    }




}