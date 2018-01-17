using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class OfferImgCarInfo
    {
        public string CarOwner { get; set; }

        public string CarPlateNo { get; set; }

        public string CarEngineNo { get; set; }

        public string CarVin { get; set; }

        public string CarModelName { get; set; }

    }


    public class OfferImgCommercialInfo
    {
        public OfferImgCommercialInfo()
        {
            this.Coverages = new List<OfferImgCoverage>();
        }

        public DateTime? PeriodStart { get; set; }

        public DateTime? PeriodEnd { get; set; }

        public List<OfferImgCoverage> Coverages { get; set; }

        public decimal? SumPremium { get; set; }
    }


    public class OfferImgCoverage
    {
        public string Name { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Premium { get; set; }

        public string Coverage { get; set; }

    }

    public class OfferImgCompulsoryInfo
    {
        public DateTime? PeriodStart { get; set; }

        public DateTime? PeriodEnd { get; set; }

        public decimal? Premium { get; set; }

    }

    public class OfferImgModel
    {
        public OfferImgModel()
        {
            this.CarInfo = new OfferImgCarInfo();
            this.CommercialCoverageInfo = new OfferImgCommercialInfo();
            this.CompulsoryInfo = new OfferImgCompulsoryInfo();
        }

        public OfferImgCarInfo CarInfo { get; set; }

        public string Company { get; set; }

        public string Offerer { get; set; }

        public DateTime OfferTime { get; set; }

        public OfferImgCommercialInfo CommercialCoverageInfo { get; set; }

        public OfferImgCompulsoryInfo CompulsoryInfo { get; set; }

        public decimal? TravelTax { get; set; }

        public decimal? SumPremium { get; set; }

    }
}
