using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarAddbasePms
    {
        public YdtInscarAddbasePms()
        {
            this.car = new YdtInscarInfoModel();
            this.pic = new YdtInscarPicModel();
            this.customers = new List<YdtInscarCustomerModel>();
        }
        public int auto { get; set; }
        public int carType { get; set; }
        public int belong { get; set; }
        public YdtInscarInfoModel car { get; set; }
        public YdtInscarPicModel pic { get; set; }
        public List<YdtInscarCustomerModel> customers { get; set; }

    }

    public class YdtInscarEditbasePms : YdtInscarAddbasePms
    {
        public string orderSeq { get; set; }

    }
}
