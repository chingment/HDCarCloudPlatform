using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsEditBaseInfoResult
    {
        public string Auto { get; set; }
        public CarInfoModel Car { get; set; }
        public List<CarInsCustomerModel> Customers { get; set; }
        public int CarInfoOrderId { get; set; }
    }
}