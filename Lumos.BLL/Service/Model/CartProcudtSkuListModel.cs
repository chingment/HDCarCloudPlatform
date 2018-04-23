using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class CartProcudtSkuListModel
    {
        public int CartId { get; set; }
        public int SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuMainImg { get; set; }
        public int Quantity { get; set; }

        public bool Selected { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal ShowPrice { get; set; }

        public decimal SumPrice { get; set; }

    }
}
