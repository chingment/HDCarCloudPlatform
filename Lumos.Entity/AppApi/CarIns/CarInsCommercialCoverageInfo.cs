using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsCommercialCoverageInfo
    {
        public CarInsCommercialCoverageInfo()
        {
            this.Coverages = new List<CarInsCoverageModel>();
        }

        public string PeriodStart { get; set; }

        public string PeriodEnd { get; set; }

        public List<CarInsCoverageModel> Coverages { get; set; }

        public decimal? SumPremium { get; set; }
    }
}