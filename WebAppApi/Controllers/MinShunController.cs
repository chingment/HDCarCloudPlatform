using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppApi.Controllers
{



    public class ReceiveNotifyModel
    {
        public string orderId { get; set; }

        public string mercid { get; set; }

        public string termid { get; set; }

        public string txnamt { get; set; }

        public string result_code { get; set; }

        public string result_msg { get; set; }

        public string sign { get; set; }
    }


    public class MinShunController : BaseApiController
    {

        [HttpPost]
        public APIResponse ReceiveNotify(ReceiveNotifyModel model)
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();

            Log.Info("ReceiveNotify：" + postData);


            OrderPayResultNotifyByMinShunLog receiveNotifyLog = new OrderPayResultNotifyByMinShunLog();

            receiveNotifyLog.OrderId = model.orderId;
            receiveNotifyLog.Mercid = model.mercid;
            receiveNotifyLog.Termid = model.termid;
            receiveNotifyLog.Txnamt = model.txnamt;
            receiveNotifyLog.ResultCode = model.result_code;
            receiveNotifyLog.ResultCodeName = GetResultCodeName(model.result_code);
            receiveNotifyLog.ResultMsg = model.result_msg;
            receiveNotifyLog.Sign = model.sign;
            receiveNotifyLog.MwebUrl = null;
            receiveNotifyLog.NotifyParty = Enumeration.PayResultNotifyParty.MinShunNotifyUrl;
            receiveNotifyLog.NotifyPartyName = Enumeration.PayResultNotifyParty.MinShunNotifyUrl.GetCnName();
            receiveNotifyLog.Creator = 0;
            receiveNotifyLog.CreateTime = DateTime.Now;

            IResult result = BizFactory.Pay.ResultNotify(0, Enumeration.PayResultNotifyParty.MinShunNotifyUrl, receiveNotifyLog);

            return new APIResponse(result);
        }


        public string GetResultCodeName(string resultcode)
        {
            string name = "";
            switch (resultcode)
            {
                case "00":
                    name = "成功";
                    break;
                case "S1":
                    name = "交易已发起退货";
                    break;
                case "S2":
                    name = "未支付";
                    break;
                case "S3":
                    name = "交易关闭";
                    break;
                case "S4":
                    name = "交易已冲正 ";
                    break;
                case "S5":
                    name = "正在支付";
                    break;
                case "S6":
                    name = "支付失败";
                    break;
                case "T1":
                    name = "终端未入库";
                    break;
                case "T2":
                    name = "终端未绑定";
                    break;
                case "T3":
                    name = "商户无此接口权限";
                    break;
                case "T4":
                    name = "用户余额不足";
                    break;
                case "T5":
                    name = "订单已支付";
                    break;
                case "T6":
                    name = "订单已关闭";
                    break;
                case "T7":
                    name = "系统错误";
                    break;
                case "T8":
                    name = "缺少参数";
                    break;
            }
            return name;
        }

    }
}