﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToCarInsureOfferCompany")]
    public class OrderToCarInsureOfferCompany
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int InsuranceCompanyId { get; set; }

        [MaxLength(128)]
        public string InsuranceOrderId { get; set; }

        [MaxLength(1024)]
        public string InsureImgUrl { get; set; }

        public decimal? CommercialPrice { get; set; }

        public decimal? TravelTaxPrice { get; set; }

        public decimal? CompulsoryPrice { get; set; }

        public decimal? InsureTotalPrice { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public string PayUrl { get; set; }

        [MaxLength(128)]
        public string InsuranceCompanyName { get; set; }

        [MaxLength(512)]
        public string InsuranceCompanyImgUrl { get; set; }
        [MaxLength(128)]
        public string BiStartDate { get; set; }
        [MaxLength(128)]
        public string CiStartDate { get; set; }
        [MaxLength(128)]
        public string PartnerInquiryId { get; set; }
        [MaxLength(128)]
        public string PartnerOrderId { get; set; }
        [MaxLength(128)]
        public string PartnerChannelId { get; set; }
        [MaxLength(128)]
        public string PartnerCompanyId { get; set; }

        public string PartnerRisk { get; set; }

    }
}
