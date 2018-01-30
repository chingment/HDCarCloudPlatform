using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Order")]
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? PId { get; set; }

        [MaxLength(128)]
        public string Sn { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public Enumeration.ProductType ProductType { get; set; }

        [MaxLength(256)]
        public string ProductName { get; set; }

        [MaxLength(128)]
        public string Contact { get; set; }

        [MaxLength(128)]
        public string ContactPhoneNumber { get; set; }

        [MaxLength(512)]
        public string ContactAddress { get; set; }

        public decimal Price { get; set; }

        [MaxLength(1024)]
        public string Remarks { get; set; }

        public Enumeration.OrderStatus Status { get; set; }

        public int FollowStatus { get; set; }

        public DateTime SubmitTime { get; set; }

        public Enumeration.OrderPayWay PayWay { get; set; }

        public DateTime? PayTime { get; set; }

        public DateTime? CancleTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }


        [MaxLength(128)]
        public string Recipient { get; set; }

        [MaxLength(512)]
        public string RecipientAddress { get; set; }

        [MaxLength(128)]
        public string RecipientPhoneNumber { get; set; }

        [MaxLength(1024)]
        public string ClientRequire { get; set; }


        [MaxLength(128)]
        public string PriceVersion { get; set; }

        public string TermId { get; set; }

        public string SpbillIp { get; set; }

        //public bool IsPayQuery { get; set; }
    }
}
