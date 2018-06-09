using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarInsureByArtificialPms
    {
        public int belong { get; set; }
        public List<YdtInscarCustomerModel> customers { get; set; }
        public string inquirySeq { get; set; }
        public string licensePic { get; set; }
        public string loanFlag { get; set; }
        public string orderSeq { get; set; }
    }
}
