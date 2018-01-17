using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public  class VehicleInformationQueryRequestMain
    {
        public Channel Channel { set; get; }
        public string CarNo { set; get; }
        public string OwnerName { set; get; }
        public VehicleInformationQueryRequestMain()
        {
            Channel = new Channel();
            CarNo = "";
            OwnerName = "";
        }
    }
}
