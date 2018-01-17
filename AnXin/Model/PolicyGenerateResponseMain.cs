using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PolicyGenerateResponseMain
    {
        public List<PayConfirmInfoVO> PayConfirmInfoList { set; get; }
        public PolicyGenerateResponseMain()
        {
            PayConfirmInfoList = new List<PayConfirmInfoVO>();
        }
    }
}
