using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderPayResultNotifyByStaffLog")]
    public class OrderPayResultNotifyByStaffLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        [MaxLength(128)]
        public string OrderSn { get; set; }

        public int MerchantId { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
