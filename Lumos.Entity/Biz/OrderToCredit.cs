using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToCredit")]
    public class OrderToCredit : Order
    {
        [MaxLength(128)]
        public decimal Creditline { get; set; }

        public string CreditClass { get; set; }
    }
}
