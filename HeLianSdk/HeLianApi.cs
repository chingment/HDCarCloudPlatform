using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace HeLianSdk
{

    public static class HeLianApi
    {
        private static string _openid = "85237fd7e5af4fe499c92c08d58dfb44";
        private static string _appkey = "81d31e6521c2481693399e5da4204cfa";
        private static string _md5key = "2a8a199309774415b5098ead2a2031dd";

        private static long GetTimespan()
        {
            return (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        private static string GetSign(Dictionary<string, string> dic)
        {


            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (dic == null || dic.Count == 0)
                return "";

            foreach (var item in dic)
            {
                string key = item.Key;
                string value = item.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }


            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);
            s += _md5key;
            s = s.ToLower();
            s = FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5").ToLower();

            return s;

        }

        public static HeLianApiBaseResult<List<CarQueryDataList_Request>> CarQueryDataList(CarQueryDataList_Params pams)
        {

            HeLianApiBaseResult<List<CarQueryDataList_Request>> result = new HeLianApiBaseResult<List<CarQueryDataList_Request>>();

            Dictionary<string, string> postData = new Dictionary<string, string>();

            postData.Add("openid", _openid);
            postData.Add("appkey", _appkey);
            postData.Add("timestamp", GetTimespan().ToString());
            postData.Add("carNo", pams.carNo);
            postData.Add("carType", pams.carType);
            postData.Add("rackNo", pams.rackNo);
            postData.Add("enginNo", pams.enginNo);
            string sign = GetSign(postData);
            postData.Add("sign", sign);
            postData.Add("isCompany", pams.isCompany);


            WebUtils webUtils = new WebUtils();
            string postDataStr = "data=" + JsonConvert.SerializeObject(postData);
            string body = webUtils.DoPost("http://www.hl2016.com/mainoa/api/carquery/dataList.jhtml", null, postDataStr, null);


            result = JsonConvert.DeserializeObject<HeLianApiBaseResult<List<CarQueryDataList_Request>>>(body);

            return result;
        }

        public static HeLianApiBaseResult<List<CarQueryGetLllegalPrice_Result>> CarQueryGetLllegalPrice(CarQueryGetLllegalPrice_Params pams)
        {
            HeLianApiBaseResult<List<CarQueryGetLllegalPrice_Result>> result = new HeLianApiBaseResult<List<CarQueryGetLllegalPrice_Result>>();

            Dictionary<string, string> postData = new Dictionary<string, string>();

            postData.Add("openid", _openid);
            postData.Add("appkey", _appkey);
            postData.Add("timestamp", GetTimespan().ToString());
            postData.Add("carNo", pams.carNo);
            postData.Add("carType", pams.carType);
            postData.Add("rackNo", pams.rackNo);
            postData.Add("enginNo", pams.enginNo);
            string sign = GetSign(postData);

            postData.Add("sign", sign);
            postData.Add("isCompany", pams.isCompany);

            string v = JsonConvert.SerializeObject(pams.dataLllegal);

            postData.Add("dataLllegal", v);

            WebUtils webUtils = new WebUtils();

            string postDataStr = "";
            foreach (var m in postData)
            {
                postDataStr += m.Key + "=" + m.Value + "&";
            }

            postDataStr = postDataStr.Substring(0, postDataStr.Length - 1);

            string body = webUtils.DoPost("http://www.hl2016.com/mainoa/api/carquery/getLllegalPrice.jhtml", null, postDataStr, null);


            result = JsonConvert.DeserializeObject<HeLianApiBaseResult<List<CarQueryGetLllegalPrice_Result>>>(body);

            return result;
        }

        private static string DoPost(string url, Dictionary<string, string> postData)
        {
            WebUtils webUtils = new WebUtils();
            string postDataStr = "data=" + JsonConvert.SerializeObject(postData);
            string body = webUtils.DoPost(url, null, postDataStr, null);
            return body;
        }


    }
}
