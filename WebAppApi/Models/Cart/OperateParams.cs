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
        public Lumos.Entity.Enumeration.CartOperateType Operate { get; set; }

        public List<CartProcudtSkuListByOperateModel> List { get; set; }
    }
}