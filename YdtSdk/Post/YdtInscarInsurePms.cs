using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarInsureAddress
    {
        public string consignee { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }

        public string areaId { get; set; }
    }

    public class YdtInscarInsurePms
    {
        public YdtInscarInsurePms()
        {
            
        }
        public string inquirySeq { get; set; }
        public string orderSeq { get; set; }
        public string notifyUrl { get; set; }

        public string openNotifyUrl { get; set; }
    }
}
