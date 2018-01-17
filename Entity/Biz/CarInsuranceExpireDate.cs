using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("CarInsuranceExpireDate")]
    public class CarInsuranceExpireDate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string CarPlateNo { get; set; }

        [MaxLength(128)]
        public string CarVinLast6Num { get; set; }

        public DateTime ValidityEndDate { get; set; }

        public DateTime InsuranceEndDate { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
