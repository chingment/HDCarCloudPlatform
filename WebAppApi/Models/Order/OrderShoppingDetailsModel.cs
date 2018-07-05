using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderShoppingDetailsModel : OrderBaseDetailsViewModel
    {

        public OrderShoppingDetailsModel()
        {
            this.Skus = new List<OrderShoppingGoodsDetailsModel>();
        }

        public List<OrderShoppingGoodsDetailsModel> Skus { get; set; }
    }
}