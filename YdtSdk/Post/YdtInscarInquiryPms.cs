using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarInquiryPms
    {
        public int auto { get; set; }
        public int channelId { get; set; }
        public string orderSeq { get; set; }
        public string companyCode { get; set; }
        public int risk { get; set; }
        public string biStartDate { get; set; }
        public string ciStartDate { get; set; }
        public List<CoverageModel> coverages { get; set; }
        public string notifyUrl { get; set; }
        public string openNotifyUrl { get; set; }
        public List<InquiryModel> inquirys { get; set; }
    }
}
