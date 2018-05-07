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
        public decimal SumPoint { get; set; }

        public decimal SumFine { get; set; }

        public decimal SumUrgentFee { get; set; }

        public int SumCount { get; set; }

        public decimal SumLateFees { get; set; }
        public decimal SumServiceFees { get; set; }

        [MaxLength(128)]
        public string CarNo { get; set; }


    }
}
