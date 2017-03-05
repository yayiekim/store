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

        public List<string> GetGenders()
        {
            List<string> list = new List<string>();

            list.Add("Male");
            list.Add("Female");

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

      

        public List<CheckBoxModel> GetProductGenders(List<ProductGenders> productGenders)
        {
            List<string> genders = new List<string>();

            genders.Add("Shoes");
            genders.Add("Top");
            genders.Add("Bottom");
            
            List<CheckBoxModel> list = new List<CheckBoxModel>();

            foreach (var x in genders)
            {
                if (productGenders.Any(i => i.Gender == x))
                {
                    CheckBoxModel gc = new CheckBoxModel()
                    {
                        Id = x,
                        IsSelected = true
                    };

                    list.Add(gc);
                }
                else
                {
                    CheckBoxModel gc = new CheckBoxModel()
                    {
                        Id = x,
                        IsSelected = false
                    };

                    list.Add(gc);
                }
            }

            return list;

        }

    }

    public class CheckBoxModel
    {
        public string Id { get; set; }
        public bool IsSelected { get; set; }
        public string Label { get; set; }
    }

    
}