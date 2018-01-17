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
    [Table("PosMachine")]
    public class PosMachine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string DeviceId { get; set; }

        [MaxLength(128)]
        public string FuselageNumber { get; set; }

        [MaxLength(128)]
        public string TerminalNumber { get; set; }

        [MaxLength(128)]
        public string Version { get; set; }

        public bool IsUse { get; set; }

        public decimal Deposit { get; set; }

        public decimal Rent { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public bool IsSpare { get; set; }

        //是否能生成商户账号，当是IsSpare 为 false,则为true,当更换流程 旧机器 为false
        public bool IsAutoBuildAccount { get; set; }
    }
}
