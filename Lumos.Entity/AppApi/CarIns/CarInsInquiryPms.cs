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
        }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int CarInfoOrderId { get; set; }

        public int Auto { get; set; }

        public int ChannelId { get; set; }

        public string CompanyCode { get; set; }

        public List<CarInsInsureKindModel> InsureKind { get; set; }

        public string CiStartDate { get; set; }

        public string BiStartDate { get; set; }
    }
}