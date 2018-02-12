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
    [Table("MerchantPosMachineChangeHistory")]
    public class MerchantPosMachineChangeHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int MerchantPosMachineId { get; set; }

        public int OldPosMachineId { get; set; }

        public int NewPosMachineId { get; set; }

        [MaxLength(2048)]
        public string Reason { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
