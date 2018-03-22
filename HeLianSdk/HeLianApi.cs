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
        private static string _openid = "1066b451c67536c3e5c99d5d08a";
        private static string _appkey = "259cfe89f9dfb3fd1d514a52i";


        private static long GetTimespan()
        {
            return (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;
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
                string value = HttpUtility.UrlEncode(item.Value, UTF8Encoding.UTF8).ToUpper();
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }


            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);

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

            string body = DoPost("http://www.hl2016.com/mainoa/api/carquery/dataList.jhtml", postData);

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
            postData.Add("dataLllegal", pams.dataLllegal);

            string body = DoPost("http://www.hl2016.com/mainoa/api/carquery/getLllegalPrice.jhtml", postData);

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
