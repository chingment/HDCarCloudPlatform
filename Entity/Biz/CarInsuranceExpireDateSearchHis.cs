using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("CarInsuranceExpireDateSearchHis")]
    public class CarInsuranceExpireDateSearchHis
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

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
