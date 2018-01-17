using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PackageVOPG
    {
        public string cAppNo { set; get; }// 申请单号
        public string cProdNo { set; get; }// 产品代码 
        public string cQryCde { set; get; }// 平台查询码
        public string cPlyTyp { set; get; }// 保单形式
        public string cAppValidateNo { set; get; }// 承保验证码
        public PackageVOPG()
        {
            cAppNo = "";
            cProdNo = "";
            cQryCde = "";
            cPlyTyp = "";
            cAppValidateNo = "";
        }

    }
}
