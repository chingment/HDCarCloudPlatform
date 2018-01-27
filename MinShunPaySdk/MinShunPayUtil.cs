﻿using System;
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

        public static CodeDownload_Result CodeDownload(MinShunPayOrderInfo order)
        {
            MinShunPayApi api = new MinShunPayApi();

            CodeDownload_Params param = new CodeDownload_Params();
            param.partnerId = partnerId;
            param.tranCod = "0700";
            param.mercid = mercid;
            param.tranType = order.TranType;//微信：180000 ,支付宝：280000
            param.txnamt = (order.Price * 100).ToString();
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


        public static void PayQuery(MinShunPayOrderInfo order)
        {
            MinShunPayApi api = new MinShunPayApi();

            CodeDownload_Params param = new CodeDownload_Params();
            param.partnerId = partnerId;
            param.tranCod = "0700";
            param.mercid = mercid;
            param.tranType = order.TranType;//微信：180000 ,支付宝：280000
            param.txnamt = (order.Price * 100).ToString();
            param.orderId = order.OrderId;
            param.termid = order.TermId;
            param.spbill_ip = order.SpbillIp;
            param.notify_url = notify_url;
            param.remark = order.Remark;
            param.orderDate = order.SubmitTime.ToString("yyyyMMdd");
            param.orderTime = order.SubmitTime.ToString("HHmmdd");

        }
    }
}
