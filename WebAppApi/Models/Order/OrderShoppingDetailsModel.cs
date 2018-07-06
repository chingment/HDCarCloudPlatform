using Lumos.BLL.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderShoppingDetailsModel : OrderBaseDetailsViewModel
    {
        public RecipientAddressModel RecipientAddress { get; set; }

        public OrderShoppingDetailsModel()
        {
            this.RecipientAddress = new RecipientAddressModel();
            this.Skus = new List<OrderShoppingGoodsDetailsModel>();
        }

        public List<OrderShoppingGoodsDetailsModel> Skus { get; set; }
    }
}