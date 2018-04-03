using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class PayConfirmModel
    {

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int OrderId { get; set; }

        public string OrderSn { get; set; }

        public Enumeration.ProductType ProductType { get; set; }

        public object Params { get; set; }
    }
}
