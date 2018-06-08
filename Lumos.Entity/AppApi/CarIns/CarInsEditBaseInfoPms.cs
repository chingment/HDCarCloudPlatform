using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsEditBaseInfoPms
    {
        public CarInsEditBaseInfoPms()
        {
            this.Car = new CarInfoModel();
            this.Customers = new List<CarInsCustomerModel>();
        }
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int CarInfoOrderId { get; set; }
        public string Auto { get; set; }
        public CarInfoModel Car { get; set; }
        public List<CarInsCustomerModel> Customers { get; set; }
    }
}