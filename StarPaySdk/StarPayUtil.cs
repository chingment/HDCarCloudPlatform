using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{

    public class StarPayOrderInfo
    {
        public string OrderId { get; set; }
        public string TransType { get; set; }
        public string Amount { get; set; }
        public DateTime TransTime { get; set; }
    }

    public class StarPayUtil
    {
        //public readonly static string mercid = "800290000000441";
        //public readonly static string trmNo = "95006888";
        //public readonly static string signkey = "Pz5meIuUxRKcF7rlvPglliyfwvC9vhjGcBy61WHM00Qfwz0E6yTTLwgxJsFYE9IQ";
        public readonly static string mercid = "800581000010155";
        public readonly static string trmNo = "95234555";
        public readonly static string signkey = "0319B673D3D851EFF2B35BE564AB7DC4";
        public readonly static string serverUrl = "http://gateway.starpos.com.cn";

        public static void CodeDownload(StarPayOrderInfo order)
        {
            StarPayApi api = new StarPayApi(serverUrl, signkey);

            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("amount", "1");
            sParams.Add("authCode", "130219205322723729");
            sParams.Add("characterSet", "00");
            sParams.Add("mercId", mercid);
            sParams.Add("opSys", "3");
            sParams.Add("orgNo", "489");
            sParams.Add("payChannel", "WXPAY");
            sParams.Add("signType", "MD5");
            sParams.Add("total_amount", "1");
            sParams.Add("tradeNo", "20170804103115431251");
            sParams.Add("trmNo", "95006888");
            sParams.Add("txnTime", "20170804103115");
            sParams.Add("version", "V1.0.0");
            string sign = CommonUtil.MakeMd5Sign(sParams, signkey);
            sParams.Add("signValue", sign);//签名

            //SdkBarcodePosPayParams param = new SdkBarcodePosPayParams();
            //param.opSys = "3";
            //param.characterSet = "00-GBK";
            //param.orgNo = "489";
            //param.mercId = mercid;
            //param.trmNo = "95006888";
            //param.tradeNo = "20170804103115431251";
            //param.txnTime = "20170804103115";
            //param.signType = "MD5";
            //param.version = "V1.0.0";
            //param.amount = "1";
            //param.total_amount = "1";
            //param.authCode = "130219205322723729";
            //param.characterSet = "00";
            //param.payChannel = "WXPAY";

            SdkBarcodePosPayRequest rquest = new SdkBarcodePosPayRequest(sParams);

            var b = api.DoPost(rquest);


        }

        public static void PayQuery(StarPayOrderInfo order)
        {
            //MinShunPayApi api = new MinShunPayApi(serverUrl, signkey);

            //CodeDownload_Params param = new CodeDownload_Params();
            //param.partnerId = partnerId;
            //param.tranCod = "0700";
            //param.mercid = mercid;
            //param.tranType = order.TranType;//微信：180000 ,支付宝：280000
            //param.txnamt = ((int)(order.Price * 100)).ToString();
            //param.orderId = order.OrderId;
            //param.termid = order.TermId;
            //param.spbill_ip = order.SpbillIp;
            //param.notify_url = notify_url;
            //param.remark = order.Remark;
            //param.orderDate = order.SubmitTime.ToString("yyyyMMdd");
            //param.orderTime = order.SubmitTime.ToString("HHmmdd");

            //CodeDownload_Request rquest = new CodeDownload_Request(param);

            //var b = api.DoPost(rquest);

            //return b;

        }
    }
}
