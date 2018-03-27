using Lumos.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderLllegalDealtDetailsModel:OrderBaseDetailsViewModel
    {
        public OrderLllegalDealtDetailsModel()
        {
            this.LllegalRecord = new List<Lumos.BLL.LllegalRecord>();
        }

        public List<LllegalRecord> LllegalRecord { get; set; }

        public decimal SumPoint { get; set; }
        public decimal SumFine { get; set; }
        public string CarNo { get; set; }
        public int SumCount { get; set; }
    }
}