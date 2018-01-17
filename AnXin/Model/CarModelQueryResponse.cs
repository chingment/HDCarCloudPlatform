using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarModelQueryResponse
    {
       public  ResponseHead ResponseHead;
       public CarModelQueryResponseMain CarModelQueryResponseMain;

        public CarModelQueryResponse()
        {
            ResponseHead = new ResponseHead();
            CarModelQueryResponseMain = new CarModelQueryResponseMain();
        }
    }
}
