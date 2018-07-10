using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarPayByArtificialPms
    {
        public YdtInscarPayByArtificialPms()
        {
            this.address = new YdtInscarInsureAddress();
        }

        public string insureSeq { get; set; }
        public string inquirySeq { get; set; }
        public string orderSeq { get; set; }
        public YdtInscarInsureAddress address { get; set; }
        public string notifyUrl { get; set; }
        public string openNotifyUrl { get; set; }
    }
}
