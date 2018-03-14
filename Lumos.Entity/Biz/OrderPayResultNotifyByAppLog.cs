using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderPayResultNotifyByAppLog")]
    public class OrderPayResultNotifyByAppLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public int PosMachineId { get; set; }
        public int UserId { get; set; }
        [MaxLength(128)]
        public string TransResult { get; set; }
        [MaxLength(128)]
        public string TransType { get; set; }
        [MaxLength(128)]
        public string Amount { get; set; }
        [MaxLength(128)]
        public string CardNo { get; set; }
        [MaxLength(128)]
        public string TraceNo { get; set; }
        [MaxLength(128)]
        public string ShopId { get; set; }
        [MaxLength(128)]
        public string ShopName { get; set; }
        [MaxLength(128)]
        public string BatchNo { get; set; }
        [MaxLength(128)]
        public string RefNo { get; set; }
        [MaxLength(128)]
        public string AuthCode { get; set; }
        [MaxLength(128)]
        public string TransDate { get; set; }
        [MaxLength(128)]
        public string TransTime { get; set; }
        [MaxLength(128)]
        public string PosId { get; set; }
        [MaxLength(128)]
        public string OrderSn { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
