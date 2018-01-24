using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToServiceFee")]
    public class OrderToServiceFee : Order
    {
        
        public decimal Deposit { get; set; }

        public decimal MobileTrafficFee { get; set; }

        public DateTime? ExpiryTime { get; set; }


    }
}
