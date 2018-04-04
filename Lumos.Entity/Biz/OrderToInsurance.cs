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

        [MaxLength(128)]
        public string InsuranceCompanyName { get; set; }
    }
}
