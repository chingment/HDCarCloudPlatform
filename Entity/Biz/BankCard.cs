using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("BankCard")]
    public class BankCard
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int BankId { get; set; }

        [MaxLength(128)]
        public string BankCode { get; set; }

        [MaxLength(128)]
        public string BankName { get; set; }

        [MaxLength(128)]
        public string BankAccountName { get; set; }

        [MaxLength(128)]
        public string BankAccountNo { get; set; }

        [MaxLength(128)]
        public string BankAccountPhone { get; set; }

        public bool IsDelete { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
