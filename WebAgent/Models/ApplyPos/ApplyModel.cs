using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.ApplyPos
{
    public class ApplyModel
    {
        public int SalesmanId { get; set; }

        public int[] PosMachineIds { get; set; }
    }
}