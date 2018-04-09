using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("ProductSpecDataSetValue")]
    public class ProductSpecDataSetValue
    {
        public int Id { get; set; }

        public int ProductSpecDataSetId { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
