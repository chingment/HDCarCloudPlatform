using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsInquiryResult
    {
        public CarInsInquiryResult()
        {
            this.Car = new CarInfoModel();
            this.CommercialCoverageInfo = new CarInsCommercialCoverageInfo();
            this.CompulsoryInfo = new CarInsCompulsoryInfo();
        }

        public CarInfoModel Car { get; set; }

        public string Company { get; set; }

        public CarInsCommercialCoverageInfo CommercialCoverageInfo { get; set; }

        public CarInsCompulsoryInfo CompulsoryInfo { get; set; }

        public decimal? TravelTax { get; set; }

        public decimal? SumPremium { get; set; }

        public string OrderSeq { get; set; }

        public string InquirySeq { get; set; }
    }
}