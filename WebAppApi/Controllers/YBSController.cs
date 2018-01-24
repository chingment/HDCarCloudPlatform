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



    public class CrossoffAccountRequestBody
    {

        public string serialnumber { get; set; }

        public string transactioncode { get; set; }

        public string datetime { get; set; }

        public string merchantcode { get; set; }

        public string money { get; set; }

        public string paymentnumber { get; set; }

        public string state { get; set; }

        public string channel { get; set; }

        public string terminalnumber { get; set; }

        public string bankcardnumber { get; set; }

    }


    public class CrossoffAccountRequest
    {
        public CrossoffAccountRequestBody request { get; set; }
    }

    public class YBSController : BaseApiController
    {

        [HttpPost]
        public APIResponse ReceiveNotify(CrossoffAccountRequest model)
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();

            Log.Info("测试数据YBS_ReceiveNotify：" + postData);

            CrossoffAccountRequest request = Newtonsoft.Json.JsonConvert.DeserializeObject<CrossoffAccountRequest>(postData);
            CrossoffAccountRequestBody requestBody = request.request;

            YBS_ReceiveNotifyLog receiveNotifyLog = new YBS_ReceiveNotifyLog();

            receiveNotifyLog.Bankcardnumber = requestBody.bankcardnumber;
            receiveNotifyLog.Channel = requestBody.channel;
            receiveNotifyLog.Datetime = requestBody.datetime;
            receiveNotifyLog.Merchantcode = requestBody.merchantcode;
            receiveNotifyLog.Money = requestBody.money;
            receiveNotifyLog.Paymentnumber = requestBody.paymentnumber;
            receiveNotifyLog.Serialnumber = requestBody.serialnumber;
            receiveNotifyLog.State = requestBody.state;
            receiveNotifyLog.Terminalnumber = requestBody.terminalnumber;
            receiveNotifyLog.Transactioncode = requestBody.transactioncode;

            IResult result = BizFactory.Pay.ResultNotify(0, ResultNotifyParty.Ybs, receiveNotifyLog);

            return new APIResponse(result);
        }


    }
}