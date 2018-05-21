using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class UnifiedOrderResult
    {
        public string OrderSn { get; set; }

        public string MwebUrl { get; set; }

        public Enumeration.OrderPayWay PayWay { get; set; }
    }
}
