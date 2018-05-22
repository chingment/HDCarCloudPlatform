using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{
    public interface IStarPayApi
    {
        T DoPost<T>(IStarPayPostRequest<T> request) where T : StarPayBaseResult;
    }

    public class StarPayApi : IStarPayApi
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string _serverUrl = "";
        public string _signkey = "";

        public StarPayApi(string serverUrl, string signkey)
        {
            _serverUrl = serverUrl;
            _signkey = signkey;
        }

        public string GetServerUrl(string serverurl, string apiname)
        {
            return serverurl + "/" + apiname;
        }

        public string _result = "";


        public string Result
        {
            get
            {
                return _result;
            }
        }

        public T DoPost<T>(IStarPayPostRequest<T> request) where T : StarPayBaseResult
        {

            string realServerUrl = GetServerUrl(this._serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();

            string postData = Newtonsoft.Json.JsonConvert.SerializeObject(request.PostData);

            string body = webUtils.DoPost(realServerUrl, null, postData, null);

            _result = body;

            if (string.IsNullOrEmpty(body))
                return null;

            T rsp = JsonConvert.DeserializeObject<T>(body);

            if (rsp != null)
            {
                if (!string.IsNullOrEmpty(rsp.message))
                {
                    rsp.message = System.Web.HttpUtility.UrlDecode(rsp.message);

                    log.InfoFormat("StarPay->message:{0}", rsp.message);

                }
            }

            return rsp;
        }

    }
}
