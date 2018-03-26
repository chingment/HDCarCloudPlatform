using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToLllegalDealt")]
    public class OrderToLllegalDealt : Order
    {
        public int SumPoint { get; set; }

        public decimal SumFine { get; set; }


    }
}
