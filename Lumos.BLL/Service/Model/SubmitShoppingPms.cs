using Lumos.BLL.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.BLL.Service.Model
{
    public class SubmitShoppingPms
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int OrderId { get; set; }

        public RecipientAddressModel RecipientAddress { get; set; }

        public List<CartProcudtSkuByOperateModel> Skus { get; set; }
    }
}