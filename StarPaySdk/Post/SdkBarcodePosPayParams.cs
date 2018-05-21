using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{
    public class SdkBarcodePosPayParams : StarPayBaseParams
    {
        public string amount { get; set; }
        public string total_amount { get; set; }
        public string authCode { get; set; }
        public string payChannel { get; set; }
        public string subject { get; set; }
        public string selOrderNo { get; set; }
        public string goods_tag { get; set; }
        public string attach { get; set; }
    }
}
