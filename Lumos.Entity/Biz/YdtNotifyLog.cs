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
    [Table("YdtNotifyLog")]
    public class YdtNotifyLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(128)]
        public string OrderSeq { get; set; }
        [MaxLength(128)]
        public string Type { get; set; }
        public string NotifyContent { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
