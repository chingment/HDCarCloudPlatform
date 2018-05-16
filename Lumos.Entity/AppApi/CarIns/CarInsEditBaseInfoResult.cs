using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsEditBaseInfoResult
    {
        public string OrderSeq { get; set; }
        public string Auto { get; set; }
        public CarInfoModel Car { get; set; }
        public List<CarInsCustomerModel> Customers { get; set; }
    }
}