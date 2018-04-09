using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("InventoryOperateHis")]
    public class InventoryOperateHis
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ProductSkuId { get; set; }

        public int Quantity { get; set; }

        public int LockQuantity { get; set; }

        public int SellQuantity { get; set; }

        public Enumeration.InventoryOperateType OperateType { get; set; }

        public int ChangQuantity { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }
        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

       

    }
}
