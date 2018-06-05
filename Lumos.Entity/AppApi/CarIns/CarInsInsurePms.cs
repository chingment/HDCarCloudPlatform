using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class CarInsInsurePms
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int CarInfoOrderId { get; set; }

        public string InquirySeq { get; set; }

        public string OrderSeq { get; set; }

    }
}
