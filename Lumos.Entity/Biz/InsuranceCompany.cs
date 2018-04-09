using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("Company")]
    public class Company
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string ImgUrl { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }

        [MaxLength(256)]
        public string PhoneNumber { get; set; }
        public Enumeration.CompanyStatus Status { get; set; }

        public Enumeration.CompanyType Type { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
