using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("YBS_ReceiveNotifyLog")]
    public class YBS_ReceiveNotifyLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string Serialnumber { get; set; }

        [MaxLength(128)]
        public string Transactioncode { get; set; }

        [MaxLength(128)]
        public string Datetime { get; set; }

        [MaxLength(128)]
        public string Merchantcode { get; set; }

        [MaxLength(128)]
        public string Money { get; set; }

        [MaxLength(128)]
        public string Paymentnumber { get; set; }

        [MaxLength(128)]
        public string State { get; set; }

        [MaxLength(128)]
        public string Channel { get; set; }

        [MaxLength(128)]
        public string Terminalnumber { get; set; }

        [MaxLength(128)]
        public string Bankcardnumber { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
