using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz.Model
{
   public class ProductDetailInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal MarketPrice { get; set; }

        public decimal SalePrice { get; set; }

        public string SpecsJson { get; set; }

        public string MainImg { get; set; }

        public string DisplayImgs { get; set; }

        public string ProductKindIds { get; set; }
    }
}
