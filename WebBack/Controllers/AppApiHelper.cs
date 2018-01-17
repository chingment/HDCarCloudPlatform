using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Controllers
{
    public class AppApiHelper
    {
        private string key = "test";
        private string secret = "6ZB97cdVz211O08EKZ6yriAYrHXFBowC";
        private long timespan = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;
        private string host = "http://localhost:16665";

        public string GetOrderList(int userId, int merchantId, int pageIndex, int status)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());
            parames.Add("status", status.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Order/GetList?userId=" + userId.ToString() + "&merchantId=" + merchantId + "&pageIndex=" + pageIndex + "&status=" + status, headers);

            return result;
        }

    }
}