using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Withdraw
{
    public class CutOffAmountByTimePeriod
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountByAfterFee { get; set; }

    }
}