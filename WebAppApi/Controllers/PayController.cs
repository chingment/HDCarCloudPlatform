﻿using Lumos.BLL;
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

    public class ReceiveNotifyByStarResult
    {
        public string BalDate { get; set; }
        public string AgentId { get; set; }
        public string TxnLogId { get; set; }
        public string BusinessId { get; set; }
        public string SDTermNo { get; set; }
        public string TxnCode { get; set; }
        public string PayChannel { get; set; }
        public string TxnDate { get; set; }
        public string TxnTime { get; set; }
        public string TxnAmt { get; set; }
        public string TxnStatus { get; set; }
        //public string BankType { get; set; }
        public string OfficeId { get; set; }
        public string ChannelId { get; set; }
       // public string UserId { get; set; }
        public string logNo { get; set; }
        //public string attach { get; set; }
        //public string CrdFlg { get; set; }
    }

    public class PayController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse ReceiveNotifyByStar()
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();

            Log.Info("ReceiveNotify：" + postData);

            if (string.IsNullOrEmpty(postData))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }

            ReceiveNotifyByStarResult result = null;
            bool isSuccessPay = false;
            string orderSn = "";
            string notifyFromResult = postData;

            try
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceiveNotifyByStarResult>(postData);
                if (result != null)
                {
                    orderSn = result.ChannelId;
                    if (result.TxnStatus == "1")
                    {
                        isSuccessPay = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("解析支付结果发生异常", ex);
            }

            BizFactory.Pay.ResultNotify(0, orderSn, isSuccessPay, Enumeration.PayResultNotifyType.PartnerPayOrgNotifyUrl, "星大陆", notifyFromResult);

            return ResponseResult(ResultType.Success, ResultCode.Success, "接收成功");
        }
    }
}