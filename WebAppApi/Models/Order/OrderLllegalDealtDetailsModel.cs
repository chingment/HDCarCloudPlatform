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

        public string SumPoint { get; set; }
        public string SumFine { get; set; }
        public string CarNo { get; set; }
        public int SumCount { get; set; }

        public string SumLateFees { get; set; }
        public string SumServiceFees { get; set; }
    }
}