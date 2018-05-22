using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderPayTrans")]
    public class OrderPayTrans
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int UserId { get; set; }

        [MaxLength(128)]
        public string Sn { get; set; }

        [MaxLength(128)]
        public string OrderSn { get; set; }

        public int OrderId { get; set; }

        public string Amount { get; set; }

        public Enumeration.OrderPayWay PayWay { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime TransTime { get; set; }

        public string PartnerLogNo { get; set; }

        public string PartnerOrderNo { get; set; }
    }
}
