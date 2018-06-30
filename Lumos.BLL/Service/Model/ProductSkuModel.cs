using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class ProductSkuModel
    {
        public int SkuId { get; set; }

        public string Name { get; set; }

        public string MainImg { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal ShowPrice { get; set; }

        public string BriefIntro { get; set; }

        public bool IsHot { get; set; }

        public string DetailsDesc { get; set; }

        public List<Lumos.Entity.ImgSet> DispalyImgs { get; set; }

        public string ServiceDesc { get; set; }

        public List<SpecModel> Specs { get; set; }
    }
}
