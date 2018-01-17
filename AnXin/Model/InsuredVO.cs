using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class InsuredVO
    {
        public string cInsuredCde { set; get; }//被保人名称
        public string cInsuredNme { set; get; }//被保人名称
        public string cCertfCls { set; get; }//证件类型
        public string cCertfCde { set; get; }//证件号码
        public string cResidAddr { set; get; }//地址/住址
        public string cZipCde { set; get; }//邮编
        public string cMobile { set; get; }//移动电话
        public string cWorkDpt { set; get; }//工作单位,单位性质
        public string cCustRiskRank { set; get; }//反洗钱客户风险等级
        public string cResvTxt20 { set; get; }// 被保人类型
        public string cEmail { set; get; }//电子邮箱
        public string cClntMrk { set; get; }//电子邮箱
        public string cSex { set; get; }//电子邮箱
        public string cClntAddr { set; get; }//电子邮箱

        public InsuredVO()
        {
            cInsuredCde = "";
            cInsuredNme = "";
            cCertfCls = "";
            cCertfCde = "";
            cResidAddr = "";
            cZipCde = "";
            cMobile = "";
            cWorkDpt = "";
            cCustRiskRank = "";
            cResvTxt20 = "";
            cEmail = "";
            cClntMrk = "";
            cSex = "";
            cClntAddr = "";
        }
    }
}
