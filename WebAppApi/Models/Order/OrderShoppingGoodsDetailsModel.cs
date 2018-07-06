using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderShoppingGoodsDetailsModel
    {
        public string SkuName { get; set; }
        public string MainImg { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SumPrice { get; set; }
    }
}