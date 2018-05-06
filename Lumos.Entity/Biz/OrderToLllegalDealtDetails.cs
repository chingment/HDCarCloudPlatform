using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("OrderToLllegalDealtDetails")]
    public class OrderToLllegalDealtDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        [MaxLength(128)]
        public string BookNo { get; set; }
        [MaxLength(128)]
        public string BookType { get; set; }
        [MaxLength(128)]
        public string BookTypeName { get; set; }
        [MaxLength(128)]
        public string LllegalCode { get; set; }
        [MaxLength(128)]
        public string CityCode { get; set; }
        [MaxLength(128)]
        public string LllegalTime { get; set; }
        public decimal Point { get; set; }
        [MaxLength(128)]
        public string OfferType { get; set; }
        [MaxLength(128)]
        public string OfserTypeName { get; set; }
        public decimal Fine { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Late_fees { get; set; }
        [MaxLength(2048)]
        public string Content { get; set; }
        [MaxLength(2048)]
        public string LllegalDesc { get; set; }
        [MaxLength(128)]
        public string LllegalCity { get; set; }
        [MaxLength(1024)]
        public string Address { get; set; }
        public Enumeration.OrderToLllegalDealtDetailsStatus Status { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }

        public bool NeedUrgent { get; set; }

        public decimal UrgentFee { get; set; }
    }
}
