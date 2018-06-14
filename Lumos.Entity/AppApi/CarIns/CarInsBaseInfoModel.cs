using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Lumos.Entity
{
    public class CarInsBaseInfoModel
    {
        public CarInsBaseInfoModel()
        {
            this.Car = new CarInfoModel();
            this.Customers = new List<CarInsCustomerModel>();
        }

        public CarInfoModel Car { get; set; }
        public List<CarInsCustomerModel> Customers { get; set; }


    }
}
