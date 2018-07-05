using Lumos.BLL.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class SubmitShoppingModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int OrderId { get; set; }

        public ShippingAddressModel ShippingAddress { get; set; }

        List<CartProcudtSkuByOperateModel> Skus { get; set; }
    }
}