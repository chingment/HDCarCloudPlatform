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

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

    }
}
