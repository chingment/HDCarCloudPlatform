using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{
    public class StarPayBaseResult
    {
        public string tradeNo { get; set; }
        public string returnCode { get; set; }
        public string sysTime { get; set; }
        public string message { get; set; }
        public string mercId { get; set; }
        public string signValue { get; set; }
        public string addField { get; set; }

    }
}
