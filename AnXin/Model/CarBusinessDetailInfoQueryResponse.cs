using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarBusinessDetailInfoQueryResponse
    {
        public ResponseHead ResponseHead { set; get; }
        public CarBusinessDetailInfoQueryResponseMain CarBusinessDetailInfoQueryResponseMain { set; get; }
        public CarBusinessDetailInfoQueryResponse()
        {
            ResponseHead = new ResponseHead();
            CarBusinessDetailInfoQueryResponseMain = new CarBusinessDetailInfoQueryResponseMain();
        }
    }
}
