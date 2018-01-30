using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinShunPaySdk
{
    public class MinShunPayOrderInfo
    {
        public string TranType { get; set; }
        public string OrderId { get; set; }
        public decimal Price { get; set; }
        public string TermId { get; set; }
        public string SpbillIp { get; set; }
        public string Remark { get; set; }
        public DateTime SubmitTime { get; set; }
    }

    public static class MinShunPayUtil
    {
        public readonly static string notify_url = "http://112.74.179.185/Api/MinShun/ReceiveNotify";
        public readonly static string partnerId = "160010";
        public readonly static string mercid = "894440155416002";
        public readonly static string signkey = "36B4D6A3FBF116B5D740AFC1C39FC314";
        public readonly static string serverUrl = "http://14.29.111.142:8092";

        public static CodeDownload_Result CodeDownload(MinShunPayOrderInfo order)
        {
            MinShunPayApi api = new MinShunPayApi(serverUrl, signkey);

            CodeDownload_Params param = new CodeDownload_Params();
            param.partnerId = partnerId;
            param.tranCod = "0700";
            param.mercid = mercid;
            param.tranType = order.TranType;//微信：180000 ,支付宝：280000
            param.txnamt = ((int)(order.Price * 100)).ToString();
            param.orderId = order.OrderId;
            param.termid = order.TermId;
            param.spbill_ip = order.SpbillIp;
            param.notify_url = notify_url;
            param.remark = order.Remark;
            param.orderDate = order.SubmitTime.ToString("yyyyMMdd");
            param.orderTime = order.SubmitTime.ToString("HHmmdd");

            CodeDownload_Request rquest = new CodeDownload_Request(param);

            var b = api.DoPost(rquest);

            return b;
        }


        public static CodeDownload_Result PayQuery(MinShunPayOrderInfo order)
        {
            MinShunPayApi api = new MinShunPayApi(serverUrl, signkey);

            CodeDownload_Params param = new CodeDownload_Params();
            param.partnerId = partnerId;
            param.tranCod = "0700";
            param.mercid = mercid;
            param.tranType = order.TranType;//微信：180000 ,支付宝：280000
            param.txnamt = ((int)(order.Price * 100)).ToString();
            param.orderId = order.OrderId;
            param.termid = order.TermId;
            param.spbill_ip = order.SpbillIp;
            param.notify_url = notify_url;
            param.remark = order.Remark;
            param.orderDate = order.SubmitTime.ToString("yyyyMMdd");
            param.orderTime = order.SubmitTime.ToString("HHmmdd");

            CodeDownload_Request rquest = new CodeDownload_Request(param);

            var b = api.DoPost(rquest);

            return b;

        }



        public static bool CheckSign(Dictionary<string, string> dic, string sign)
        {

            string signdata = TdsPayUtil.GetSignData(dic);


            string _sign = TdsPayUtil.GetShaSign(signdata + signkey);

            if (_sign == sign)
            {
                return true;
            }

            return false;
        }

    }
}
