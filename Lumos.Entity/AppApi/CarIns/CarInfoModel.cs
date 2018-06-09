using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInfoModel
    {
        public string Belong { get; set; }
        public string CarType { get; set; }
        public string LicensePlateNo { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string FirstRegisterDate { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public string Displacement { get; set; }
        public string MarketYear { get; set; }
        public int RatedPassengerCapacity { get; set; }
        public decimal ReplacementValue { get; set; }
        public string ChgownerType { get; set; }
        public string ChgownerDate { get; set; }
        public string Tonnage { get; set; }
        public string WholeWeight { get; set; }
        public string LicensePicKey { get; set; }
        public string LicensePicUrl { get; set; }
        public string LicenseOtherPicKey { get; set; }
        public string LicenseOtherPicUrl { get; set; }
        public string CarCertPicKey { get; set; }
        public string CarCertPicUrl { get; set; }
        public string CarInvoicePicKey { get; set; }
        public string CarInvoicePicUrl { get; set; }
    }
}