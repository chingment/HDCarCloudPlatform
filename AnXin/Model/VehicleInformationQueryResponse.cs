using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class VehicleInformationQueryResponse
    {
        public ResponseHead ResponseHead { set; get; }
        public VehicleInformationQueryResponseMain VehicleInformationQueryResponseMain { set; get; }
        public VehicleInformationQueryResponse()
        {
            ResponseHead = new ResponseHead();
            VehicleInformationQueryResponseMain = new VehicleInformationQueryResponseMain();
        }
    }
}
