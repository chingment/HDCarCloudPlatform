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
        public int MerchantId { get; set; }
        public int PosMachineId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string OrderSn { get; set; }
        public bool IsPaySuccess { get; set; }
        public Enumeration.PayResultNotifyType NotifyType { get; set; }
        public string NotifyFromName { get; set; }
        public string NotifyFromResult { get; set; }
        public int OperatorId { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
