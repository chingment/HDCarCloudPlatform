using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class VehicleInformationQueryResponseMain
    {
        public List<VehicleVO> VehicleList { set; get; }
        public VehicleInformationQueryResponseMain()
        {
            VehicleList = new List<VehicleVO>();
        }
    }
}
