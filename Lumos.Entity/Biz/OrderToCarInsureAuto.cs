using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("OrderToCarInsureAuto")]
    public class OrderToCarInsureAuto : Order
    {
        public string CarBelong { get; set; }
        [MaxLength(128)]
        public string CarType { get; set; }
        [MaxLength(128)]
        public string CarLicensePlateNo { get; set; }
        [MaxLength(128)]
        public string CarVin { get; set; }
        [MaxLength(128)]
        public string CarEngineNo { get; set; }
        [MaxLength(128)]
        public string CarFirstRegisterDate { get; set; }
        [MaxLength(128)]
        public string CarModelCode { get; set; }
        [MaxLength(128)]
        public string CarModelName { get; set; }
        [MaxLength(128)]
        public string CarDisplacement { get; set; }
        [MaxLength(128)]
        public string CarMarketYear { get; set; }
        public int CarRatedPassengerCapacity { get; set; }
        public decimal CarReplacementValue { get; set; }
        [MaxLength(128)]
        public string CarChgownerType { get; set; }
        [MaxLength(128)]
        public string CarChgownerDate { get; set; }
        [MaxLength(128)]
        public string CarTonnage { get; set; }
        [MaxLength(128)]
        public string CarWholeWeight { get; set; }
        [MaxLength(128)]
        public string CarLicensePicKey { get; set; }
        [MaxLength(128)]
        public string CarLicenseOtherPicKey { get; set; }
        [MaxLength(128)]
        public string CarCertPicKey { get; set; }
        [MaxLength(128)]
        public string CarInvoicePicKey { get; set; }
        [MaxLength(128)]
        public string CarownerInsuredFlag { get; set; }
        [MaxLength(128)]
        public string CarowneName { get; set; }
        [MaxLength(128)]
        public string CarowneCertNo { get; set; }
        [MaxLength(128)]
        public string CarowneMobile { get; set; }
        [MaxLength(128)]
        public string CarowneAddress { get; set; }
        [MaxLength(128)]
        public string CarowneIdentityFacePicKey { get; set; }
        [MaxLength(128)]
        public string CarowneIdentityBackPicKey { get; set; }
        [MaxLength(128)]
        public string CarowneOrgPicKey { get; set; }
        [MaxLength(128)]
        public string PolicyholderInsuredFlag { get; set; }
        [MaxLength(128)]
        public string PolicyholderName { get; set; }
        [MaxLength(128)]
        public string PolicyholderCertNo { get; set; }
        [MaxLength(128)]
        public string PolicyholderMobile { get; set; }
        [MaxLength(128)]
        public string PolicyholderAddress { get; set; }
        [MaxLength(128)]
        public string PolicyholderIdentityFacePicKey { get; set; }
        [MaxLength(128)]
        public string PolicyholderIdentityBackPicKey { get; set; }
        [MaxLength(128)]
        public string PolicyholderOrgPicKey { get; set; }
        [MaxLength(128)]
        public string InsuredInsuredFlag { get; set; }
        [MaxLength(128)]
        public string InsuredName { get; set; }
        [MaxLength(128)]
        public string InsuredCertNo { get; set; }
        [MaxLength(128)]
        public string InsuredMobile { get; set; }
        [MaxLength(128)]
        public string InsuredAddress { get; set; }
        [MaxLength(128)]
        public string InsuredIdentityFacePicKey { get; set; }
        [MaxLength(128)]
        public string InsuredIdentityBackPicKey { get; set; }
        [MaxLength(128)]
        public string InsuredOrgPicKey { get; set; }
        [MaxLength(128)]
        public string PartnerOrderId { get; set; }
        [MaxLength(128)]
        public string PartnerInquiryId { get; set; }
        [MaxLength(128)]
        public string PartnerChannelId { get; set; }
        [MaxLength(128)]
        public string PartnerCompanyId { get; set; }
        [MaxLength(128)]
        public string PartnerRisk { get; set; }
        public int? InsCompanyId { get; set; }
        [MaxLength(256)]
        public string InsCompanyImgUrl { get; set; }
        [MaxLength(128)]
        public string InsCompanyName { get; set; }
        [MaxLength(128)]
        public string InsBiStartDate { get; set; }
        [MaxLength(128)]
        public string InsCiStartDate { get; set; }
        public decimal? InsCommercialPrice { get; set; }
        public decimal? InsTravelTaxPrice { get; set; }
        public decimal? InsCompulsoryPrice { get; set; }
        public decimal? InsTotalPrice { get; set; }
        public Enumeration.OfferResult OfferResult { get; set; }
        public int OfferTryCount { get; set; }

        public bool OfferIsAuto { get; set; }
    }
}
