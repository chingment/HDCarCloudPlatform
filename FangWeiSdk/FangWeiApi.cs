using Lumos.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FangWeiSdk
{
    public enum YdtPostDataType
    {
        Unknow = 0,
        Json = 1,
        Xml = 2,
        Form = 3,
        Text = 4,
        Byte = 5,
        Stream = 6
    }

    public interface IFangWeiApi
    {
        CustomJsonResult DoGet<T>(IFangWeiApiGetRequest<T> request);

    }

    public class FangWeiApi : IFangWeiApi
    {
        public string serverUrl = "http://113.107.6.178:25192/api";


        public FangWeiApi()
        {

        }

        public string GetServerUrl(string serverurl, string apiname)
        {
            return serverurl + "/" + apiname;
        }


        public CustomJsonResult DoGet<T>(IFangWeiApiGetRequest<T> request)
        {
            CustomJsonResult result = new CustomJsonResult();

            string realServerUrl = GetServerUrl(this.serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "qxt");


            string body = webUtils.DoGet(realServerUrl, request.GetUrlParameters(), headers);

            JObject jo = JObject.Parse(body);

            if (jo.GetValue("error") != null)
            {
                result.Result = ResultType.Failure;
                result.Message = jo.GetValue("error").ToString();
                result.Code = ResultCode.Failure;
            }
            else
            {
                result.Result = ResultType.Success;
                result.Message = "成功";
                result.Code = ResultCode.Success;
                result.Data = JsonConvert.DeserializeObject<T>(body);


            }

            return result;
        }

    }
}
