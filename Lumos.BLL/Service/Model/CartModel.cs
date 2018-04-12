using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class CartModel
    {
        public CartModel()
        {
            this.List = new List<CartProcudtSkuListModel>();
        }

        public List<CartProcudtSkuListModel> List { get; set; }

        public int Count { get; set; }

        public decimal SumPrice { get; set; }

        public int CountBySelected { get; set; }

        public decimal SumPriceBySelected { get; set; }
    }
}
