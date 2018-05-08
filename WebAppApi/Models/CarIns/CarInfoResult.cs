using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarIns
{
    public class CarInfoResult
    {
        public CarInfoResult()
        {
            this.Car = new CarInfoModel();
            this.Customers = new List<CustomerModel>();
        }

        public string Auto { get; set; }
        public CarInfoModel Car { get; set; }
        public List<CustomerModel> Customers { get; set; }

    }
}