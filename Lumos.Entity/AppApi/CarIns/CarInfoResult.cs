using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInfoResult
    {
        public CarInfoResult()
        {
            this.Car = new CarInfoModel();
            this.Customers = new List<CarInsCustomerModel>();
        }
        public string Auto { get; set; }
        public CarInfoModel Car { get; set; }
        public List<CarInsCustomerModel> Customers { get; set; }

    }
}