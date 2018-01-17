using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Product
{
    public class RecommendProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public string LinkUrl { get; set; }

    }
}