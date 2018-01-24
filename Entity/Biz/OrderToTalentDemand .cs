using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToTalentDemand")]
    public class OrderToTalentDemand : Order
    {
        public Enumeration.WorkJob WorkJob { get; set; }

        [MaxLength(128)]
        public String WorkJobName { get; set; }

        public int Quantity { get; set; }

        public DateTime? UseStartTime { get; set; }

        public DateTime? UseEndTime { get; set; }

    }
}
