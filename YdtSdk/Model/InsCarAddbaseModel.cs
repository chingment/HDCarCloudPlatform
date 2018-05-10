using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class InscarAddbaseModel
    {
        public InscarAddbaseModel()
        {
            this.car = new InsCarInfoModel();
            this.pic = new InsPicModel();
            this.customers = new List<InsCustomers>();
        }
        public int auto { get; set; }
        public int carType { get; set; }
        public int belong { get; set; }
        public InsCarInfoModel car { get; set; }
        public InsPicModel pic { get; set; }
        public List<InsCustomers> customers { get; set; }

    }

    public class InscarEditbaseModel: InscarAddbaseModel
    {
        public string orderSeq { get; set; }

    }
}
