using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.PosMachine
{
    public class ChangeModel
    {
        public int MerchantId { get; set; }
        public int OldPosMachineId { get; set; }
        public int NewPosMachineId { get; set; }

    }
}