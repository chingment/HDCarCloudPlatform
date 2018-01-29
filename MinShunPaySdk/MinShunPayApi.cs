using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinShunPaySdk
{
    public enum MinShunPayPostDataType
    {
        Unknow = 0,
        Json = 1,
        Xml = 2,
        Form = 3,
        Text = 4,
        Byte = 5,
        Stream = 6
    }

    public interface IMinShunPayApi
    {
        T DoPost<T>(IMinShunPayApiPostRequest<T> request) where T : MinShunPayApiBaseResult;
    }

    public class MinShunPayApi : IMinShunPayApi
    {
        public string _serverUrl = "http://14.29.111.142:8092";
        public string _signkey = "";

        public MinShunPayApi(string serverUrl,string signkey)
        {
            _serverUrl = serverUrl;
            _signkey = signkey;
        }

        public string GetServerUrl(string serverurl, string apiname)
        {
            return serverurl + "/" + apiname;
        }

        public T  DoPost<T>(IMinShunPayApiPostRequest<T> request) where T : MinShunPayApiBaseResult
        {

            string realServerUrl = GetServerUrl(this._serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();


            string signdata = TdsPayUtil.GetSignData(request.UrlParameters);


            string sign = TdsPayUtil.GetShaSign(signdata + _signkey);

            request.UrlParameters.Add("sign", sign);

            string postData = null;
        


            string body = webUtils.DoPost(realServerUrl, request.UrlParameters, postData, null);
            T rsp = JsonConvert.DeserializeObject<T>(body);

            return rsp;
        }

    }
}
