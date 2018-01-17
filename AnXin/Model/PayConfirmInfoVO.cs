using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PayConfirmInfoVO
    {
        public string cAppNo { set; get; }
        public string cAppValidateNo { set; get; }
        public string cPlyNo { set; get; }
        public string flag { set; get; }
        public string cRstMsg { set; get; }
        public PayConfirmInfoVO()
        {
            cAppNo = "";
            cAppValidateNo = "";
            cPlyNo = "";
            flag = "";
            cRstMsg = "";
        }
    }
}
