using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{
    public class StarPayBaseParams
    {
        public string opSys { get; set; }
        public string characterSet { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string orgNo { get; set; }
        public string mercId { get; set; }
        public string trmNo { get; set; }
        public string oprId { get; set; }
        public string trmTyp { get; set; }
        public string tradeNo { get; set; }
        public string txnTime { get; set; }
        public string signType { get; set; }
        public string signValue { get; set; }
        public string addField { get; set; }
        public string version { get; set; }
    }
}
