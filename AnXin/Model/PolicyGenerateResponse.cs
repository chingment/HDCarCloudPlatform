using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public  class PolicyGenerateResponse
    {
        public ResponseHead ResponseHead { set; get; }
        public PolicyGenerateResponseMain PolicyGenerateResponseMain { set; get; }
        public PolicyGenerateResponse()
        {
            ResponseHead = new ResponseHead();
            PolicyGenerateResponseMain = new PolicyGenerateResponseMain();
        }
    }
}
