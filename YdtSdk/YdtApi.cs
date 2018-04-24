using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
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

    public interface IYdtApi
    {
        YdtApiBaseResult<T> DoGet<T>(IYdtApiGetRequest<T> request);

        YdtApiBaseResult<T> DoPost<T>(IYdtApiPostRequest<T> request);

        YdtApiBaseResult<T> DoPostFile<T>(IYdtApiPostRequest<T> request, string filenName);
    }

    public class YdtApi : IYdtApi
    {
        public string serverUrl = "http://test.hybao.cc:6200";


        public YdtApi()
        {

        }

        public string GetServerUrl(string serverurl, string apiname)
        {
            return serverurl + "/" + apiname;
        }


        public YdtApiBaseResult<T> DoGet<T>(IYdtApiGetRequest<T> request)
        {

            string realServerUrl = GetServerUrl(this.serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();
            string body = webUtils.DoGet(realServerUrl, request.GetUrlParameters(), null);



            if (body.IndexOf("\"code\":") == -1)
            {
                body = "{\"code\":0,\"msg\":\"成功\",\"data\":" + body + "}";
            }

            var rsp = JsonConvert.DeserializeObject<YdtApiBaseResult<T>>(body);






            return rsp;
        }


        public YdtApiBaseResult<T> DoPost<T>(IYdtApiPostRequest<T> request)
        {

            string realServerUrl = GetServerUrl(this.serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();

            string postData = null;
            if (request.PostDataTpye == YdtPostDataType.Text)
            {
                postData = request.PostData.ToString();
            }
            else if (request.PostDataTpye == YdtPostDataType.Json)
            {
                postData = JsonConvert.SerializeObject(request.PostData);
            }

            string body = webUtils.DoPost(realServerUrl, request.GetUrlParameters(), postData, null);

            var rsp1 = JsonConvert.DeserializeObject<YdtApiBaseResult<object>>(body);

            if (rsp1.code == 0)
            {
                body = "{\"code\":0,\"msg\":\"成功\",\"data\":" + body + "}";

            }

            var rsp = JsonConvert.DeserializeObject<YdtApiBaseResult<T>>(body);

            //if (body.IndexOf("\"code\":") == -1)
            //{
            //    if (body.IndexOf('{') == -1 && body.IndexOf('[') == -1)
            //    {
            //        body = "{\"code\":2,\"msg\":\"" + body + "\"}";
            //    }
            //    else
            //    {
            //        body = "{\"code\":0,\"msg\":\"成功\",\"data\":" + body + "}";
            //    }
            //}


            //  var rsp = JsonConvert.DeserializeObject<YdtApiBaseResult<T>>(body);


            return rsp;
        }


        public YdtApiBaseResult<T> DoPostFile<T>(IYdtApiPostRequest<T> request, string filename)
        {

            string realServerUrl = GetServerUrl(this.serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();

            string body = webUtils.DoPostFile(realServerUrl, request.GetUrlParameters(), filename, (Stream)request.PostData);
            if (body.IndexOf("\"code\":") == -1)
            {
                body = "{\"code\":0,\"msg\":\"成功\",\"data\":" + body + "}";
            }
            var rsp = JsonConvert.DeserializeObject<YdtApiBaseResult<T>>(body);

            return rsp;
        }

    }
}
