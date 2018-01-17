using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Order
{
    public class OrderSearchCondition : SearchCondition
    {
        public Enumeration.OrderStatus Status { get; set; }

        public string YYZZ_Name { get; set; }

    }
}