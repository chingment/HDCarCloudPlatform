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

        public int Quantity { get; set; }

    }
}
