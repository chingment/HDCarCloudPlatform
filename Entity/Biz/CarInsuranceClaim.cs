using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("BizProcessesAuditDetails")]
    public class CarInsuranceClaim
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InsuranceCompanyId { get; set; }

        [MaxLength(128)]
        public string CarLicenseNumber { get; set; }

        [MaxLength(128)]
        public string Contact { get; set; }

        [MaxLength(128)]
        public string ContactPhoneNumber { get; set; }

        [MaxLength(256)]
        public string ContactAddress { get; set; }

        public int MerchantId { get; set; }

        public int MaintenanceId { get; set; }

        public Enumeration.DealtStatus DealtStatus { get; set; }

        public Enumeration.CarInsuranceClaimResult DealtResult { get; set; }

        [MaxLength(1024)]
        public string CancelReason { get; set; }

        public DateTime SubmitTime { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public string PayPriceVersion { get; set; }

        public string CommissionVersion { get; set; }
    }
}
