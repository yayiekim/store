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


        public class BlobResultForSaving
        {
            public string BaseUrl { get; set; }
            public string FileName { get; set; }
            public string URL { get; set; }
            public string ThumbUrl { get; set; }

        }


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
        
    }

    public class CheckBoxModel
    {
        public string Id { get; set; }
        public bool IsSelected { get; set; }
        public string Label { get; set; }
    }

    public class NewIMageModel
    {
        public string Id { get; set; }
        public string FileExtention { get; set; }
        public string Url { get; set; }
        public HttpPostedFileBase File { get; set; }
    }


}