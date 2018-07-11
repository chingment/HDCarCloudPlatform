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
        //座位数
        public int? Quantity { get; set; }
        //玻璃类型
        public int? GlassType { get; set; }
        //unitAmount
        public decimal? UnitAmount { get; set; }
        //basicPremium
        public decimal BasicPremium { get; set; }
        //discount
        public decimal Discount { get; set; }
        //standardPremium
        public decimal StandardPremium { get; set; }
        //总保额
        public decimal Amount { get; set; }
        public decimal? Premium { get; set; }
        //public int Compensation { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public int Priority { get; set; }
        public string KindValue { get; set; }
        [MaxLength(1024)]
        public string KindDetails { get; set; }
        //是否不计免赔
        public bool IsWaiverDeductible { get; set; }
        [MaxLength(128)]
        public string KindUnit { get; set; }
    }
}
