using Lumos.BLL;
using Lumos.Common;
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
    public class PayController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse ReceiveNotifyByStar()
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();



            string key = "test";
            string secret = "6ZB97cdVz211O08EKZ6yriAYrHXFBowC";
            long timespan = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;
            string signStr = Signature.Compute(key, secret, timespan, postData);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("http://120.79.233.231/api/Pay/ReceiveNotifyByStar", postData, headers);


            Log.Info("ReceiveNotify：" + postData);

            return ResponseResult(ResultType.Success, ResultCode.Success, "接收成功");
        }
    }
}