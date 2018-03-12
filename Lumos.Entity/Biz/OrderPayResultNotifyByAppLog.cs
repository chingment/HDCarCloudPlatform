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
        public string TransResult { get; set; }
        public string Amount { get; set; }
        public string CardNo { get; set; }
        public string BatchNo { get; set; }
        public string TraceNo { get; set; }
        public string IssBankNo { get; set; }
        public string AcqCode { get; set; }
        public string RefNo { get; set; }
        public string AuthCode { get; set; }
        public string TransDate { get; set; }
        public string TransTime { get; set; }
        public string posSN { get; set; }
        public string Version { get; set; }
        public string PosModel { get; set; }
        public string Order { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
