using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("Product")]
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Enumeration.ProductType Type { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string MainImgUrl { get; set; }

        [MaxLength(2048)]
        public string ElseImgUrls { get; set; }

        public string Details { get; set; }

        public Enumeration.ProductStatus Status { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
