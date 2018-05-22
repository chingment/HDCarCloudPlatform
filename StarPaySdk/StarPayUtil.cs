using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{

    public class StarPayOrderInfo
    {
        public string TransSn { get; set; }
        public string OrderSn { get; set; }
        public string PayWay { get; set; }
        public string Amount { get; set; }
        public DateTime TransTime { get; set; }
    }

    public class StarPayUtil
    {
        //public readonly static string mercid = "800290000007906";
        //public readonly static string trmNo = "XB006439";
        //public readonly static string orgNo = "11658";
        //public readonly static string signkey = "B1823ECCC7D7E4A2A1B06F57199C4276";
        //public readonly static string serverUrl = "http://139.196.77.69:8280";

        public readonly static string mercid = "800581000010155";
        public readonly static string trmNo = "95234555";
        public readonly static string orgNo = "719";
        public readonly static string signkey = "0319B673D3D851EFF2B35BE564AB7DC4";
        public readonly static string serverUrl = "http://gateway.starpos.com.cn";

        public static SdkBarcodePosPayResult CodeDownload(StarPayOrderInfo order)
        {
            StarPayApi api = new StarPayApi(serverUrl, signkey);

            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("opSys", "3");
            sParams.Add("characterSet", "00");
            sParams.Add("orgNo", orgNo);
            sParams.Add("mercId", mercid);
            sParams.Add("trmNo", trmNo);
            sParams.Add("tradeNo", order.TransSn);
            sParams.Add("txnTime", order.TransTime.ToString("yyyyMMddHHmmss"));
            sParams.Add("signType", "MD5");
            sParams.Add("version", "V1.0.0");


            sParams.Add("amount", order.Amount);
            sParams.Add("payChannel", order.PayWay);
            sParams.Add("total_amount", order.Amount);
            sParams.Add("selOrderNo", order.OrderSn);


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

            var result = api.DoPost(rquest);

            return result;
        }

        public static SdkQryBarcodePayResult PayQuery(StarPayOrderInfo order, out string apiResult)
        {
            StarPayApi api = new StarPayApi(serverUrl, signkey);
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("opSys", "3");
            sParams.Add("characterSet", "00");
            sParams.Add("orgNo", orgNo);
            sParams.Add("mercId", mercid);
            sParams.Add("trmNo", trmNo);
            sParams.Add("tradeNo", order.TransSn);
            sParams.Add("txnTime", order.TransTime.ToString("yyyyMMddHHmmss"));
            sParams.Add("signType", "MD5");
            sParams.Add("version", "V1.0.0");

            sParams.Add("qryNo", order.TransSn);

            string sign = CommonUtil.MakeMd5Sign(sParams, signkey);
            sParams.Add("signValue", sign);

            SdkQryBarcodePayRequest rquest = new SdkQryBarcodePayRequest(sParams);

            var result = api.DoPost(rquest);

            apiResult = api.Result;

            return result;
        }
    }
}
