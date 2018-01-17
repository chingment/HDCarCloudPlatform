using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public  class PolicyGenerateRequestMain
    {
        public string cBizConsultNo {set;get;}
        public string cPaySequence {set;get;}
        public string cPayTyp {set;get;}
        public string cTerNo {set;get;}
        public string tChargeTm { set; get; }
        public Channel Channel { set; get; }
        public List<PayConfirmInfoVO> PayConfirmInfoList { set; get; }

        public PolicyGenerateRequestMain()
        {
            cBizConsultNo = "";
            cPaySequence = "";
            cPayTyp = "";
            cTerNo = "";
            tChargeTm = "";
            Channel = new Channel();
            PayConfirmInfoList = new List<PayConfirmInfoVO>();
        }



    }
}
