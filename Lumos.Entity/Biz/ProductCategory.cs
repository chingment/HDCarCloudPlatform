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
    [Table("ProductCategory")]
    public class ProductCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }

        public int PId { get; set; }

        public int Priority { get; set; }

        [MaxLength(512)]
        public string IconImg { get; set; }

        [MaxLength(512)]
        public string MainImg { get; set; }

        public bool IsDelete { get; set; }

        public Lumos.Entity.Enumeration.ProductCategoryStatus Status { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime CreateTime { get; set; }

        public int Creator { get; set; }
    }
}
