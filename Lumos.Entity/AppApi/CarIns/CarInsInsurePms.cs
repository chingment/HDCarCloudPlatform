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

        public int OfferId { get; set; }

        public CarInfoModel Car { get; set; }
        public List<CarInsCustomerModel> Customers { get; set; }

        public CarInsInsurePms()
        {
            this.Car = new CarInfoModel();
            this.Customers = new List<CarInsCustomerModel>();
        }

    }
}
