using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Lumos.Entity
{
    [Table("OrderToInsurance")]
    public class OrderToInsurance : Order
    {
        public int InsCompanyId { get; set; }
        [MaxLength(128)]
        public string InsCompanyName { get; set; }
        public int InsPlanId { get; set; }
        [MaxLength(128)]
        public string InsPlanName { get; set; }
        public string InsPlanDetailsItems { get; set; }
        public bool IsTeam { get; set; }
    }
}
