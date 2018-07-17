using log4net;
using Lumos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

    public class YdtResult
    {
        public string msg { get; set; }
        public int code { get; set; }
    }

    public interface IYdtApi
    {
        YdtApiBaseResult<T> DoGet<T>(IYdtApiGetRequest<T> request);

        YdtApiBaseResult<T> DoPost<T>(IYdtApiPostRequest<T> request);

        YdtApiBaseResult<T> DoPostFile<T>(IYdtApiPostRequest<T> request, string filenName);
    }

    public class YdtApi : IYdtApi
    {

        public string serverUrl = "https://open.hybao.cc:443";


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

            var rsp = new YdtApiBaseResult<T>();

            try
            {
                LogUtil.Info("Ydt->result:" + body);
                YdtResult ydtResult = new YdtResult();


                if (body.IndexOf("无效会话") > -1)
                {

                }
                else if (body.IndexOf("msg") < 0 && body.IndexOf("extmsg") < 0)
                {
                    body = "{\"code\":0,\"msg\":\"成功\",\"data\":" + body + "}";
                }

                rsp = JsonConvert.DeserializeObject<YdtApiBaseResult<T>>(body);
            }
            catch (Exception ex)
            {
                LogUtil.Error("解释 Ydt->result 错误:" + body);
            }
            return rsp;
        }


        public YdtApiBaseResult<T> DoPost<T>(IYdtApiPostRequest<T> request)
        {
            try
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


                if (body.IndexOf("无效会话") > -1)
                {

                }
                if (body.IndexOf("msg") < 0 && body.IndexOf("extmsg") < 0)
                {
                    body = "{\"code\":0,\"msg\":\"成功\",\"data\":" + body + "}";
                }



                var rsp = JsonConvert.DeserializeObject<YdtApiBaseResult<T>>(body);

                return rsp;
            }
            catch (Exception ex)
            {
                LogUtil.Error("解释异常", ex);
                return null;
            }
        }


        public YdtApiBaseResult<T> DoPostFile<T>(IYdtApiPostRequest<T> request, string filename)
        {

            string realServerUrl = GetServerUrl(this.serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();

            LogUtil.Info("Ydt-request-filename>>>>" + filename);

            string body = webUtils.DoPostFile(realServerUrl, request.GetUrlParameters(), filename, (Stream)request.PostData);

            LogUtil.Info("Ydt-request-filename:result>>>>" + body);

            if (body.IndexOf("\"code\":") == -1)
            {
                body = "{\"code\":0,\"msg\":\"成功\",\"data\":" + body + "}";
            }
            var rsp = JsonConvert.DeserializeObject<YdtApiBaseResult<T>>(body);

            return rsp;
        }

    }
}
