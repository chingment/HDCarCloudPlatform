using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderCreditDetailsModel : OrderBaseDetailsViewModel
    {
        public decimal Creditline { get; set; }

        public string CreditClass { get; set; }
    }
}