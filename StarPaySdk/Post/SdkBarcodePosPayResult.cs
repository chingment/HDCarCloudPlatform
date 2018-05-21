using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{
    public class SdkBarcodePosPayResult : StarPayBaseResult
    {
        public string logNo { get; set; }
        public string result { get; set; }
        public string payCode { get; set; }
        public string orderNo { get; set; }
        public string amount { get; set; }
        public string total_amount { get; set; }
        public string subject { get; set; }
        public string selOrderNo { get; set; }
        public string goodsTag { get; set; }
        public string attach { get; set; }
    }
}
