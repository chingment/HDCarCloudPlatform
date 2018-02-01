using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderPayResultNotifyLog")]
    public class OrderPayResultNotifyLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SysOrderId { get; set; }

        [MaxLength(128)]
        public string SysOrderSn { get; set; }

        public string OrderNo { get; set; }

        [MaxLength(128)]
        public string MerchantId { get; set; }

        [MaxLength(128)]
        public string Amount { get; set; }

        [MaxLength(128)]
        public string TerminalId { get; set; }

        [MaxLength(128)]
        public string MerchantNo { get; set; }

        [MaxLength(128)]
        public string BatchNo { get; set; }

        [MaxLength(128)]
        public string MerchantName { get; set; }

        [MaxLength(128)]
        public string Issue { get; set; }

        [MaxLength(128)]
        public string TraceNo { get; set; }

        [MaxLength(1024)]
        public string FailureReason { get; set; }

        [MaxLength(128)]
        public string ReferenceNo { get; set; }

        [MaxLength(128)]
        public string Type { get; set; }

        [MaxLength(128)]
        public string Result { get; set; }

        [MaxLength(128)]
        public string CardNo { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
