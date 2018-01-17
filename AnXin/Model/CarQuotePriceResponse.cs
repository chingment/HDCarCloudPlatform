using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarQuotePriceResponse
    {
        public ResponseHead ResponseHead { set; get; }
        public CarQuotePriceResponseMain CarQuotePriceResponseMain { set; get; }
        public CarQuotePriceResponse()
        {
            ResponseHead = new ResponseHead();
            CarQuotePriceResponseMain = new CarQuotePriceResponseMain();
        }
    }
}
