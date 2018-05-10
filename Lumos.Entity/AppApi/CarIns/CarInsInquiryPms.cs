using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsInquiryPms
    {
        public CarInsInquiryPms()
        {
            this.InsureKind = new List<CarInsInsureKindModel>();
            this.Car = new CarInfoModel();
        }
        public int Auto { get; set; }

        public string OrderSeq { get; set; }

        public int ChannelId { get; set; }

        public string CompanyCode { get; set; }

        public List<CarInsInsureKindModel> InsureKind { get; set; }

        public CarInfoModel Car { get; set; }

        public string CiStartDate { get; set; }

        public string BiStartDate { get; set; }
    }
}