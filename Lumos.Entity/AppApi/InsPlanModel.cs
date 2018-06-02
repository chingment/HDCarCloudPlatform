using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class InsPlanModel
    {
        public InsPlanModel()
        {
            this.ProductSkus = new List<InsPlanProductSkuModel>();
        }

        public string BannerImgUrl { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<InsPlanProductSkuModel> ProductSkus { get; set; }

    }
}
