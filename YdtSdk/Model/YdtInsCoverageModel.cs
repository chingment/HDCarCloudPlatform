using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInsCoverageModel
    {
        public int UpLinkCode { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsCompensation { get; set; }

        public decimal Amount { get; set; }

        public decimal UnitAmount { get; set; }

        public string Quantity { get; set; }

        public string GlassType { get; set; }

        public int Priority { get; set; }
    }
}
