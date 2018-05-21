using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderPayResultNotifyByPartnerPayOrgLog")]
    public class OrderPayResultNotifyByPartnerPayOrgLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string Mercid { get; set; }
        public string Termid { get; set; }
        public string Amount { get; set; }
        public string TotalAmount { get; set; }
        public string PayType { get; set; }
        public string ResultCode { get; set; }
        public string ResultCodeName { get; set; }
        public string ResultMsg { get; set; }
        public Enumeration.PayResultNotifyType NotifyType { get; set; }
        public string PartnerPayOrgName { get; set; }
        public string PartnerPayOrgResult { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
