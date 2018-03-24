using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("LllegalQueryScoreTrans")]
   public class LllegalQueryScoreTrans
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string Sn { get; set; }

        public Enumeration.LllegalQueryScoreTransType Type { get; set; }

        public int UserId { get; set; }

        public decimal ChangeScore { get; set; }

        public decimal Score { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
