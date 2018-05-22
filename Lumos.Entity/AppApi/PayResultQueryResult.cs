using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class PayResultQueryResult
    {
        public string OrderSn { get; set; }

        public int Status { get; set; }

        public string Remarks { get; set; }

        public Enumeration.OrderType OrderType { get; set; }

        public PrintDataModel PrintData { get; set; }
    }
}
