using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    //商品种类集合
    [Table("ProductKind")]
    public class ProductKind
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public int PId { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }

        [MaxLength(512)]
        public string IconImg { get; set; }

        [MaxLength(512)]
        public string MainImg { get; set; }

        public bool IsDelete { get; set; }

        public Lumos.Entity.Enumeration.ProductKindStatus Status { get; set; }

        public int Priority { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

    }
}
