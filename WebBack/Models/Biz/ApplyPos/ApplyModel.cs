using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ApplyPos
{
    public class ApplyModel
    {
        public int AgentId { get; set; }

        public int SalesmanId { get; set; }

        public int[] MerchantPosMachineIds { get; set; }
    }
}