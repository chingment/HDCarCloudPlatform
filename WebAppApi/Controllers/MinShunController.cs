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

            IResult result = BizFactory.Pay.ResultNotify(0, Enumeration.PayResultNotifyParty.MinShunNotifyUrl, receiveNotifyLog);

            return new APIResponse(result);
        }


    }
}