using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class CartComfirmOrderDataModel
    {
        public CartComfirmOrderDataModel()
        {
            this.Skus = new List<CartProcudtSkuModel>();
            this.RecipientAddress = new RecipientAddressModel();
        }

        public List<CartProcudtSkuModel> Skus { get; set; }

        public string ActualAmount { get; set; }

        public RecipientAddressModel RecipientAddress { get; set; }
    }
}
