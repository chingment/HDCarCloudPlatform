using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    /// <summary>
    /// 配送信息
    /// </summary>
    public class DeliveryVO
    {
        public string cSignInCnm { set; get; }//签收人
        public string cSignInTel { set; get; }//签收人电话
        public string cSendOrderAddr { set; get; }//送单地址
        public DeliveryVO()
        {
            cSignInCnm = "";
            cSignInTel = "";
            cSendOrderAddr = "";
        }
    }
}
