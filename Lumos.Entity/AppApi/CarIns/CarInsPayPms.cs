using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class CarInsPayPms
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int OfferId { get; set; }

        public int Auto { get; set; }

        public CarInsAddressModel ReceiptAddress { get; set; }

        public CarInsPayPms()
        {
            this.ReceiptAddress = new CarInsAddressModel();
        }

    }
}

