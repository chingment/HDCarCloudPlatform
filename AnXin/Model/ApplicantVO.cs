using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class ApplicantVO
    {
         public string cAppCde { set; get; }//投保人名称
        public string cAppNme { set; get; }//投保人名称
        public string cCertfCls { set; get; }//证件类型
        public string cCertfCde { set; get; }// 证件号码
        public string cWorkArea { set; get; }//地址/住址 
        public string cZipCde { set; get; }// 邮编
        public string cMobile { set; get; }//移动电话
        public string cWorkDpt { set; get; }//工作单位,单位性质
        public string cCustRiskRank { set; get; }//反洗钱客户风险等级
        public string cAppCate { set; get; }//投保人类型
        public string cEmail { set; get; }//电子邮箱
        public string cClntAddr { set; get; }//
         public string cSex { set; get; }//
        public string cClntMrk { set; get; }//
        public ApplicantVO()
        {
            cAppCde = "";
            cAppNme = "";
            cCertfCls = "";
            cCertfCde = "";
            cWorkArea = "";
            cZipCde = "";
            cMobile = "";
            cWorkDpt = "";
            cCustRiskRank = "";
            cAppCate = "";
            cEmail = "";
            cClntAddr = "";
            cSex = "";
            cClntMrk = "";
        }
    }
}
