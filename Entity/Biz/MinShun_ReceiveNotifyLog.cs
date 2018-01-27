using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Lumos.Entity
{
    [Table("MinShun_ReceiveNotifyLog")]
    public class MinShun_ReceiveNotifyLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string Mercid { get; set; }
        public string Termid { get; set; }
        public string Txnamt { get; set; }
        public string ResultCode { get; set; }
        public string ResultCodeName { get; set; }
        public string ResultMsg { get; set; }
        public string Sign { get; set; }
        public string MwebUrl { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }

        public Enumeration.PayResultNotifyParty NotifyParty { get; set; }
    }
}
