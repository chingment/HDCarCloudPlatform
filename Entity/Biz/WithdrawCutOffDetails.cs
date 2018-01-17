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
    [Table("WithdrawCutOffDetails")]
    public class WithdrawCutOffDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MerchantId { get; set; }

        public int UserId { get; set; }

        public int WithdrawCutOffId { get; set; }

        public int WithdrawId { get; set; }

        [MaxLength(128)]
        public string WithdrawSn { get; set; }

        public decimal WithdrawAmount { get; set; }

        public decimal WithdrawAmountByAfterFee { get; set; }

        public decimal WithdrawFee { get; set; }

        [MaxLength(128)]
        public string WithdrawFeeRateRule { get; set; }

        public DateTime WithdrawExpectArriveTime { get; set; }

        public DateTime WithdrawStartTime { get; set; }

        public DateTime WithdrawSettlementStartTime { get; set; }

        public DateTime? WithdrawSettlementEndTime { get; set; }

        [MaxLength(1024)]
        public string WithdrawFailureReason { get; set; }

        public Enumeration.WithdrawStatus WithdrawStatus { get; set; }

        public int WithdrawBankCardId { get; set; }

        [MaxLength(128)]
        public string WithdrawBankName { get; set; }

        [MaxLength(128)]
        public string WithdrawBankAccountName { get; set; }

        [MaxLength(128)]
        public string WithdrawBankAccountNo { get; set; }

        public DateTime WithdrawCutoffTime { get; set; }


        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
