using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("OrderToCarInsure")]
    public class OrderToCarInsure : Order
    {
        public int CarInfoOrderId { get; set; }
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
        public decimal? CarReplacementValue { get; set; }
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
        public string CarownerName { get; set; }
        [MaxLength(128)]
        public string CarownerCertNo { get; set; }
        [MaxLength(128)]
        public string CarownerMobile { get; set; }
        [MaxLength(128)]
        public string CarownerAddress { get; set; }
        [MaxLength(128)]
        public string CarownerIdentityFacePicKey { get; set; }
        [MaxLength(128)]
        public string CarownerIdentityBackPicKey { get; set; }
        [MaxLength(128)]
        public string CarownerOrgPicKey { get; set; }
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
        public int InsCompanyId { get; set; }
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

        public string InsOrderId { get; set; }

        public string InsImgUrl { get; set; }

        public int InsPlanId { get; set; }

        [MaxLength(1024)]
        public string CZ_CL_XSZ_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string CZ_SFZ_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string CCSJM_WSZM_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string YCZ_CLDJZ_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ZJ1_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ZJ2_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ZJ3_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ZJ4_ImgUrl { get; set; }

        public int AutoCancelByHour { get; set; }

        public Enumeration.CarVechicheType CarVechicheType { get; set; }
        [MaxLength(128)]
        public string CarIssueDate { get; set; }

        public DateTime? StartOfferTime { get; set; }

        public DateTime? EndOfferTime { get; set; }

        public Enumeration.CarUserCharacter CarUserCharacter { get; set; }
    }
}
