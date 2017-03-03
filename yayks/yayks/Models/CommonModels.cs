using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace yayks.Models
{
    public class CommonModels
    {
        Entities data = new Entities();
        public List<string> GetMeasurementTypes()
        {
            List<string> list = new List<string>();

            list.Add("Shoes");
            list.Add("Shirts");

            return list;

        }

        public async Task<List<ProductCategory>> GetProductCategories()
        {
            var res = await (from c in data.ProductCategories select c).ToListAsync();

            return res;

        }

        public async Task<List<ProductColor>> GetProductColors()
        {
            var res = await (from c in data.ProductColors select c).ToListAsync();

            return res;

        }

        public List<GenderCheckBox> GetGenders()
        {
            List<string> genders = new List<string>();

            genders.Add("Shoes");
            genders.Add("Top");
            genders.Add("Bottom");

            List<GenderCheckBox> list = new List<GenderCheckBox>();

            foreach (var x in genders)
            {
               
                    GenderCheckBox gc = new GenderCheckBox()
                    {
                        Name = x,
                        IsSelected = false
                    };

                    list.Add(gc);

            }

            return list;

        }

        public List<GenderCheckBox> GetProductGenders(List<ProductGenders> productGenders)
        {
            List<string> genders = new List<string>();

            genders.Add("Shoes");
            genders.Add("Top");
            genders.Add("Bottom");
            
            List<GenderCheckBox> list = new List<GenderCheckBox>();

            foreach (var x in genders)
            {
                if (productGenders.Any(i => i.Gender == x))
                {
                    GenderCheckBox gc = new GenderCheckBox()
                    {
                        Name = x,
                        IsSelected = true
                    };

                    list.Add(gc);
                }
                else
                {
                    GenderCheckBox gc = new GenderCheckBox()
                    {
                        Name = x,
                        IsSelected = false
                    };

                    list.Add(gc);
                }
            }

            return list;

        }

    }

    public class GenderCheckBox
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}