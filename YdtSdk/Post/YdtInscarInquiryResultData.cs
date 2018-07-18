using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{

    public class CoverageModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public int? compensation { get; set; }
        public decimal? amount { get; set; }
        public decimal? unitAmount { get; set; }
        public int? quantity { get; set; }
        public int? glassType { get; set; }
        public decimal? basicPremium { get; set; }
        public decimal? discount { get; set; }
        public decimal? standardPremium { get; set; }

        public decimal? premium { get; set; }
    }

    public class InquiryModel
    {
        public int risk { get; set; }
        public decimal? basicPremium { get; set; }
        public decimal? discount { get; set; }
        public decimal? standardPremium { get; set; }
        public decimal? taxActual { get; set; }
        public decimal? sumPayTax { get; set; }
    }

    public class YdtInscarInquiryResultData
    {

        public string biStartDate { get; set; }
        public string ciStartDate { get; set; }
        public string inquirySeq { get; set; }
        public string orderSeq { get; set; }
        public int? result { get; set; }
        public string message { get; set; }
        public float? commission { get; set; }
        public List<InquiryModel> inquirys { get; set; }
        public List<CoverageModel> coverages { get; set; }
    }
}
