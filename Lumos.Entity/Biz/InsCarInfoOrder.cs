﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Lumos.Entity
{
    [Table("InsCarInfoOrder")]
    public class InsCarInfoOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(128)]
        public string PartnerOrderId { get; set; }
        public string Belong { get; set; }
        [MaxLength(128)]
        public string CarType { get; set; }
        [MaxLength(128)]
        public string LicensePlateNo { get; set; }
        [MaxLength(128)]
        public string Vin { get; set; }
        [MaxLength(128)]
        public string EngineNo { get; set; }
        [MaxLength(128)]
        public string FirstRegisterDate { get; set; }
        [MaxLength(128)]
        public string ModelCode { get; set; }
        [MaxLength(128)]
        public string ModelName { get; set; }
        [MaxLength(128)]
        public string Displacement { get; set; }
        [MaxLength(128)]
        public string MarketYear { get; set; }
        public int RatedPassengerCapacity { get; set; }
        public decimal ReplacementValue { get; set; }
        [MaxLength(128)]
        public string ChgownerType { get; set; }
        [MaxLength(128)]
        public string ChgownerDate { get; set; }
        [MaxLength(128)]
        public string Tonnage { get; set; }
        [MaxLength(128)]
        public string WholeWeight { get; set; }
        [MaxLength(128)]
        public string LicensePicKey { get; set; }
        [MaxLength(1024)]
        public string LicensePicUrl { get; set; }
        [MaxLength(128)]
        public string LicenseOtherPicKey { get; set; }
        [MaxLength(1024)]
        public string LicenseOtherPicUrl { get; set; }
        [MaxLength(128)]
        public string CarCertPicKey { get; set; }
        [MaxLength(1024)]
        public string CarCertPicUrl { get; set; }
        [MaxLength(128)]
        public string CarInvoicePicKey { get; set; }
        [MaxLength(1024)]
        public string CarInvoicePicUrl { get; set; }
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
        [MaxLength(1024)]
        public string CarownerIdentityFacePicUrl { get; set; }
        [MaxLength(128)]
        public string CarownerIdentityBackPicKey { get; set; }
        [MaxLength(1024)]
        public string CarownerIdentityBackPicUrl { get; set; }
        [MaxLength(128)]
        public string CarownerOrgPicKey { get; set; }
        [MaxLength(1024)]
        public string CarownerOrgPicUrl { get; set; }
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
        [MaxLength(1024)]
        public string PolicyholderIdentityFacePicUrl { get; set; }
        [MaxLength(128)]
        public string PolicyholderIdentityBackPicKey { get; set; }
        [MaxLength(1024)]
        public string PolicyholderIdentityBackPicUrl { get; set; }
        [MaxLength(128)]
        public string PolicyholderOrgPicKey { get; set; }
        [MaxLength(1024)]
        public string PolicyholderOrgPicUrl { get; set; }
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
        [MaxLength(1024)]
        public string InsuredIdentityFacePicUrl { get; set; }
        [MaxLength(128)]
        public string InsuredIdentityBackPicKey { get; set; }
        [MaxLength(1024)]
        public string InsuredIdentityBackPicUrl { get; set; }
        [MaxLength(128)]
        public string InsuredOrgPicKey { get; set; }
        [MaxLength(1024)]
        public string InsuredOrgPicUrl { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }


    }
}
