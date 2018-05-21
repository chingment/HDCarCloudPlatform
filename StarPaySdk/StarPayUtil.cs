﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{

    public class StarPayOrderInfo
    {
        public string OrderId { get; set; }
        public string PayWay { get; set; }
        public string Amount { get; set; }
        public DateTime TransTime { get; set; }
        public string TermId { get; set; }
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

        public static SdkBarcodePosPayResult CodeDownload(StarPayOrderInfo order)
        {
            StarPayApi api = new StarPayApi(serverUrl, signkey);

            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("opSys", "3");
            sParams.Add("characterSet", "00");
            sParams.Add("orgNo", "489");
            sParams.Add("mercId", mercid);
            sParams.Add("trmNo", "95006888");
            sParams.Add("tradeNo", "20170804103115431251");
            sParams.Add("txnTime", "20170804103115");
            sParams.Add("signType", "MD5");
            sParams.Add("version", "V1.0.0");


            sParams.Add("amount", "1");
            sParams.Add("authCode", "130219205322723729");
            sParams.Add("payChannel", "WXPAY");
            sParams.Add("total_amount", "1");


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

        public static SdkQryBarcodePayResult PayQuery(StarPayOrderInfo order)
        {
            StarPayApi api = new StarPayApi(serverUrl, signkey);
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("opSys", "3");
            sParams.Add("characterSet", "00");
            sParams.Add("orgNo", "489");
            sParams.Add("mercId", mercid);
            sParams.Add("trmNo", "95006888");
            sParams.Add("tradeNo", "20170804103115431251");
            sParams.Add("txnTime", "20170804103115");
            sParams.Add("signType", "MD5");
            sParams.Add("version", "V1.0.0");

            sParams.Add("qryNo ", "");
            string sign = CommonUtil.MakeMd5Sign(sParams, signkey);
            sParams.Add("signValue", sign);
            SdkQryBarcodePayRequest rquest = new SdkQryBarcodePayRequest(sParams);

            var result = api.DoPost(rquest);

            return result;
        }
    }
}