using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("Cart")]
    public class Cart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductSkuId { get; set; }
        [MaxLength(128)]
        public string ProductSkuName { get; set; }
        [MaxLength(256)]
        public string ProductSkuMainImg { get; set; }
        public int Quantity { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }

        public bool Selected { get; set; }

        public Enumeration.CartStatus Status { get; set; }
    }
}
