using MinShunPaySdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinShunPaySdkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> dics = new Dictionary<string, string>();

            dics.Add("partnerId", "160010");
            dics.Add("tranCod", "0700");
            dics.Add("tranType", "180000");//微信：180000 ,支付宝：280000
            dics.Add("txnamt", "1");
            dics.Add("orderId", "600000000A0000000002");
            dics.Add("mercid", "894440155416002");
            dics.Add("termid", "90117998");
            dics.Add("spbill_ip", "127.0.0.1");
            dics.Add("notify_url", "http://14.29.111.142/posm/wft_notify.tran");
            dics.Add("remark", "1");
            dics.Add("orderDate", "20170810");
            dics.Add("orderTime", "121212");

            string signdata = TdsPayUtil.GetSignData(dics);
            Console.WriteLine("拼接的代签名数据:{0}", signdata);

            string signkey = "36B4D6A3FBF116B5D740AFC1C39FC314";
            string sign = TdsPayUtil.GetShaSign(signdata + signkey);
            Console.WriteLine("拼接的代签名sign数据:{0}", sign);


            MinShunPayOrderInfo orderInfo = new MinShunPayOrderInfo();

            orderInfo.OrderId = "D1705311427000002891";
            orderInfo.Price = 0.01m;
            orderInfo.Remark = "测试";
            orderInfo.SubmitTime = DateTime.Now;
            orderInfo.TranType = "180000";
            orderInfo.TermId = "90117998";
            orderInfo.SpbillIp = "127.0.0.1";



            var result = MinShunPayUtil.CodeDownload(orderInfo);


            //MinShunPayApi api = new MinShunPayApi();

            //CodeDownload_Params param = new CodeDownload_Params();

            //param.partnerId = "160010";
            //param.tranCod = "0700";
            //param.tranType = "180000";//微信：180000 ,支付宝：280000
            //param.txnamt = "1";
            //param.orderId = "D170531142700000281";
            //param.mercid = "894440155416002";
            //param.termid = "90117998";
            //param.spbill_ip = "127.0.0.1";
            //param.notify_url = "http://14.29.111.142/posm/wft_notify.tran";
            //param.remark = "1";
            //param.orderDate = "20170810";
            //param.orderTime = "121212";

            //CodeDownload_Request rquest = new CodeDownload_Request(param);

            //var b = api.DoPost(rquest);




        }
    }
}
