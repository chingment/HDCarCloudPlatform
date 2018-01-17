using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("WithdrawFeedbackRecord")]
    public class WithdrawFeedbackRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CutOffDetailId { get; set; }

        public int WithdrawId { get; set; }

        public Enumeration.WithdrawStatus Status { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
