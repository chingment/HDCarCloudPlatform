using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{

    [Table("Withdraw")]
    public class Withdraw
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string Sn { get; set; }

        public int MerchantId { get; set; }

        public int UserId { get; set; }

        public int BankCardId { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountByAfterFee { get; set; }

        public decimal Fee { get; set; }

        [MaxLength(128)]
        public string FeeRateRule { get; set; }

        public DateTime ExpectArriveTime { get; set; }

        public DateTime SettlementStartTime { get; set; }

        public DateTime? WithdrawCutoffTime { get; set; }

        public int? WithdrawCutoffId { get; set; }

        public DateTime? SettlementEndTime { get; set; }

        [MaxLength(1024)]
        public string FailureReason { get; set; }

        public Enumeration.WithdrawStatus Status { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }


    }
}
