using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToCarInsureOfferKind")]
    public class OrderToCarInsureOfferKind
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int KindId { get; set; }


        [MaxLength(1024)]
        public string PartnerKindId { get; set; }
        [MaxLength(128)]
        public string KindValue { get; set; }
        [MaxLength(1024)]
        public string KindDetails { get; set; }
        public bool IsWaiverDeductible { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        [MaxLength(128)]
        public string KindName { get; set; }
        [MaxLength(128)]
        public string KindUnit { get; set; }
        public int KindQuantity { get; set; }
        public int KindGlassType { get; set; }
    }
}
