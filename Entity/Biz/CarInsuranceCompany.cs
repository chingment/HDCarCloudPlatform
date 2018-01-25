using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("CarInsuranceCompany")]
    public class CarInsuranceCompany
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InsuranceCompanyId { get; set; }

        [MaxLength(128)]
        public string InsuranceCompanyName { get; set; }

        [MaxLength(1024)]
        public string InsuranceCompanyImgUrl { get; set; }

        public Enumeration.CarInsuranceCompanyStatus Status { get; set; }

        public bool CanInsure { get; set; }

        public bool CanClaims { get; set; }

        public bool CanApplyLossAssess { get; set; }

        public string PayWays { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int Priority { get; set; }

    }
}
