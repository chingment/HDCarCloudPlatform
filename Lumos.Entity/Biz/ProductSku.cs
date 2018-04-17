using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("ProductSku")]
    public class ProductSku
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal ShowPrice { get; set; }

        public bool IsMultiSpec { get; set; }

        [MaxLength(2048)]
        public string Spec { get; set; }

        [MaxLength(2048)]
        public string Spec2 { get; set; }
        [MaxLength(2048)]
        public string Spec3 { get; set; }

        public Enumeration.ProductStatus Status { get; set; }

        public bool IsHot { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        [NotMapped]
        public int Quantity { get; set; }
    }
}
