using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{

    public class CoveragesModel
    {
        public string code { get; set; }
        public int compensation { get; set; }
        public decimal amount { get; set; }
        public decimal unitAmount { get; set; }
        public int quantity { get; set; }
        public int glassType { get; set; }

    }

    public class InsCarInquiryModel
    {
        public int auto { get; set; }
        public string orderSeq { get; set; }
        public string companyCode { get; set; }
        public int risk { get; set; }
        public string biStartDate { get; set; }
        public string ciStartDate { get; set; }
        public List<CoveragesModel> coverages { get; set; }
    }
}
