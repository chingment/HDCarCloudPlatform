using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    //注意 当StoreId和ProductSkuId 为0 时 代表 的快递商品
    [Table("Inventory")]
    public class Inventory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ProductSkuId { get; set; }

        public int Quantity { get; set; }

        public int LockQuantity { get; set; }

        public int SellQuantity { get; set; }

        public bool IsOffSell { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

    }
}
