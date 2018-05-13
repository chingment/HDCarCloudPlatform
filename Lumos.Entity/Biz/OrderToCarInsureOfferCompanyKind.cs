using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToCarInsureOfferCompanyKind")]
    public class OrderToCarInsureOfferCompanyKind
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int InsuranceCompanyId { get; set; }
        public int KindId { get; set; }
        [MaxLength(1024)]
        public string PartnerKindId { get; set; }
        [MaxLength(128)]
        public string KindName { get; set; }
        public int? Quantity { get; set; }
        public int? GlassType { get; set; }
        public decimal? UnitAmount { get; set; }
        public decimal BasicPremium { get; set; }
        public decimal Discount { get; set; }
        public decimal StandardPremium { get; set; }
        public decimal Amount { get; set; }
        public decimal? Premium { get; set; }
        public int Compensation { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public bool IsCompensation { get; set; }
        public int Priority { get; set; }
    }
}
