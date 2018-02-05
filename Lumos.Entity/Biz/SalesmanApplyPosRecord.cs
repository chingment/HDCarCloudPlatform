using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SalesmanApplyPosRecord")]
    public class SalesmanApplyPosRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SalesmanId { get; set; }

        [MaxLength(128)]
        public string SalesmanName { get; set; }

        public int AgentId { get; set; }

        [MaxLength(128)]
        public string AgentName { get; set; }

        public int PosMachineId { get; set; }

        [MaxLength(128)]
        public string PosMachineDeviceId { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        [MaxLength(1024)]
        public string Remarks { get; set; }
    }
}
