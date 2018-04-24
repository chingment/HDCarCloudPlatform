using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class OrderConfirmResultModel
    {
        public OrderConfirmResultModel()
        {
            this.Skus = new List<OrderConfirmSkuModel>();
        }

        public List<ShippingAddressModel> ShippingAddress { get; set; }

        public List<OrderConfirmSkuModel> Skus { get; set; }
    }
}
