using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class ProductModel
    {
        public int Id { get; set; }

        public int SkuId { get; set; }

        public string Name { get; set; }

        public string MainImg { get; set; }

        public decimal Price { get; set; }

        public string ShowPrice { get; set; }

        public string BriefIntro { get; set; }

        public bool IsHot { get; set; }

        public string DetailsUrl { get; set; }


        public List<ImgSet> DispalyImgs { get; set; }
    }
}
