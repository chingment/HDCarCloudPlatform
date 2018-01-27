using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinShunPaySdk
{
    public class PayQuery_Params
    {
        public string partnerId { get; set; }
        public string tranCod { get; set; }
        public string tranType { get; set; }
        public string txnamt { get; set; }
        public string orderId { get; set; }
        public string mercid { get; set; }
        public string termid { get; set; }
        public string spbill_ip { get; set; }
        public string notify_url { get; set; }
        public string remark { get; set; }
        public string orderDate { get; set; }
        public string orderTime { get; set; }
    }
}
