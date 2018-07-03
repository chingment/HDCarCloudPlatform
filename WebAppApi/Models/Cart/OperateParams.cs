using Lumos.BLL.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Cart
{
    public class OperateParams
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public Lumos.Entity.Enumeration.CartOperateType Operate { get; set; }

        public List<CartProcudtSkuByOperateModel> Skus { get; set; }

        public OperateParams()
        {
            this.Skus = new List<CartProcudtSkuByOperateModel>();
        }
    }
}