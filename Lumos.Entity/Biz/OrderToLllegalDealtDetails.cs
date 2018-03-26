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
        public int OrderId { get; set; }
        public string BookNo { get; set; }
        public string BookType { get; set; }
        public string BookTypeName { get; set; }
        public string LllegalCode { get; set; }
        public string CityCode { get; set; }
        public string LllegalTime { get; set; }
        public decimal Point { get; set; }
        public string OfferType { get; set; }
        public string OfserTypeName { get; set; }
        public decimal Fine { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Late_fees { get; set; }
        public string Content { get; set; }
        public string LllegalDesc { get; set; }
        public string LllegalCity { get; set; }
        public string Address { get; set; }
        public Enumeration.OrderToLllegalDealtDetailsStatus Status { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
