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
        public decimal Creditline { get; set; }

        [MaxLength(128)]
        public string CreditClass { get; set; }
    }
}
