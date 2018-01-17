using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToDepositRent")]
    public class OrderToDepositRent : Order
    {
        public decimal Deposit { get; set; }

        public decimal RentTotal { get; set; }

        public int RentMonths { get; set; }

        public DateTime? RentDueDate { get; set; }

        [MaxLength(128)]
        public string RentVersion { get; set; }

        public decimal MonthlyRent { get; set; }

    }
}
