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
    [Table("MerchantPosMachine")]
    public class MerchantPosMachine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public decimal Deposit { get; set; }

        public DateTime? ActiveTime { get; set; }

        public decimal? ReturnDeposit { get; set; }

        public DateTime? ReturnTime { get; set; }

        public decimal MobileTrafficFee { get; set; }

        public DateTime? ExpiryTime  { get; set; }

        public Enumeration.MerchantPosMachineStatus Status { get; set; }

        public int BankCardId { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        [MaxLength(128)]
        public string PosMerchantNumber { get; set; }

        [NotMapped]
        public PosMachine PosMachine { get; set; }


    }
}
