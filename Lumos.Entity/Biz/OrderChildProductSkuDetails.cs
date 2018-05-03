using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderChildProductSkuDetails")]
    public class OrderChildProductSkuDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(128)]
        public string Sn { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        [MaxLength(128)]
        public string OrderSn { get; set; }
        public int OrderChildDetailsId { get; set; }
        public int ProductSkuId { get; set; }
        [MaxLength(128)]
        public string ProductSkuName { get; set; }
        [MaxLength(256)]
        public string ProductSkuMainImg { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? CancledTime { get; set; }
        public DateTime? CompletedTime { get; set; }

    }
}
