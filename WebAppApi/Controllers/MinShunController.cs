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
            receiveNotifyLog.ResultCodeName = SdkFactory.MinShunPay.GetResultCodeName(model.result_code);
            receiveNotifyLog.ResultMsg = model.result_msg;
            receiveNotifyLog.Sign = model.sign;
            receiveNotifyLog.MwebUrl = null;
            receiveNotifyLog.NotifyParty = Enumeration.PayResultNotifyParty.MinShunNotifyUrl;
            receiveNotifyLog.NotifyPartyName = Enumeration.PayResultNotifyParty.MinShunNotifyUrl.GetCnName();
            receiveNotifyLog.Creator = 0;
            receiveNotifyLog.CreateTime = DateTime.Now;

            IResult result = null;


            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("orderId", model.orderId);
            dic.Add("mercid", model.mercid);
            dic.Add("termid", model.termid);
            dic.Add("txnamt", model.txnamt);
            dic.Add("result_code", model.result_code);
            dic.Add("result_msg", model.result_msg);

            if (SdkFactory.MinShunPay.CheckSign(dic, model.sign))
            {
                result = BizFactory.Pay.ResultNotify(0, Enumeration.PayResultNotifyParty.MinShunNotifyUrl, receiveNotifyLog);

                if (result.Result == ResultType.Success)
                {
                    Log.Info("ReceiveNotify->success,通知成功");

                    result = new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "success");
                }
                else
                {
                    Log.Warn("ReceiveNotify->fail, 通知失败");

                    result = new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "fail,通知失败");
                }
            }
            else
            {
                Log.Error("ReceiveNotify->fail,验证签名失败");
                result = new CustomJsonResult(ResultType.Failure, ResultCode.Failure, " fail,验证签名失败");
            }

            return new APIResponse(result);
        }


    }
}