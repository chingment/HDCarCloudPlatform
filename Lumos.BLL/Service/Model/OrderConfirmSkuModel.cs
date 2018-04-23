using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class OrderConfirmSkuModel
    {
        public int CartId  {  get;set;  }

        public int ProductSkuId { get; set; }

        public int Quantity { get; set; }

        public string ProductSkuName { get; set; }

        public string ProductSkuMainImg { get; set; }

        public string Price { get; set; }
    }
}
