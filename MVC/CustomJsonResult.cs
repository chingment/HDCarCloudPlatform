using Lumos.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lumos.Mvc
{
    #region CustomJsonResult 自定义JsonResult
    /// <summary>
    /// 扩展JsonResult
    /// </summary>
    public class CustomJsonResult : JsonResult, IResult
    {

        private ResultType _Result = ResultType.Unknown;
        private ResultCode _Code = ResultCode.Unknown;
        private string _Message = "";
        private object _Data = null;

        /// <summary>
        /// 结果状态默认为Unknown
        /// </summary>
        public ResultType Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
            }
        }

        /// <summary>
        /// 信息默认返回空字符串
        /// </summary>
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }

        /// <summary>
        /// 信息默认返回空字符串
        /// </summary>
        public ResultCode Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
            }
        }

        /// <summary>
        /// 内容默认为null
        /// </summary>
        public new object Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
            }
        }

        public JsonConverter[] JsonConverter
        {
            get;
            set;
        }

        public JsonSerializerSettings JsonSerializerSettings
        {
            get;
            set;
        }


        public CustomJsonResult()
        {
            this.JsonSerializerSettings = new JsonSerializerSettings();
        }

        private void SetCustomJsonResult(string contenttype, ResultType type, ResultCode code, string message, object data, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            this.ContentType = contenttype;
            this._Result = type;
            this._Code = code;
            this._Message = message;
            this._Data = data;
            this.JsonSerializerSettings = settings;
            this.JsonConverter = converters;
        }

        public CustomJsonResult(string contenttype, ResultType type, ResultCode code, string message, object content, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            SetCustomJsonResult(contenttype, type, code, message, content, settings, converters);
        }


        public CustomJsonResult(string contenttype, ResultType type, string message, object content, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            SetCustomJsonResult(contenttype, type, ResultCode.Unknown, message, content, settings, converters);
        }


        public CustomJsonResult(string contenttype, ResultType type, string message, object content, params JsonConverter[] converters)
        {
            SetCustomJsonResult(contenttype, type, ResultCode.Unknown, message, content, null, converters);
        }


        public CustomJsonResult(string contenttype, ResultType type, string message, object content, JsonSerializerSettings settings)
        {
            SetCustomJsonResult(contenttype, type, ResultCode.Unknown, message, content, settings);
        }


        public CustomJsonResult(ResultType type, string message)
        {
            SetCustomJsonResult(null, type, ResultCode.Unknown, message, null, null);
        }


        public CustomJsonResult(ResultType type, ResultCode code, string message)
        {
            SetCustomJsonResult(null, type, code, message, null, null);
        }

        public CustomJsonResult(Exception ex, string message)
        {
            MessageBoxModel messageBox = new MessageBoxModel();
            messageBox.No = Guid.NewGuid().ToString();
            messageBox.Type = MessageBoxTip.Exception;
            messageBox.Title = message;
            //messageBox.Content = "<a href=\"javascript:void(0)\" onclick=\"window.top.location.href='" + OwnWebSettingUtils.GetHomePage() + "'\">返回主页</a>";
            messageBox.IsTop = true;
            messageBox.ErrorStackTrace = CommonUtils.ToHtml(ex.Message + "\r\n" + ex.StackTrace);

            SetCustomJsonResult(null, ResultType.Exception, ResultCode.Exception, message, messageBox, null);
        }


        public CustomJsonResult(ResultType type, ResultCode code, string message, object content, params JsonConverter[] converters)
        {
            SetCustomJsonResult(null, type, code, message, content, null, converters);
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            response.Write(GetResultJson());

        }

        public override string ToString()
        {
            return GetResultJson();
        }


        private string GetResultJson()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");

            try
            {
                if (this._Data != null)
                {
                    if (this._Data is string)
                    {
                        if (!string.IsNullOrWhiteSpace(this._Data.ToString()))
                        {
                            var obj = JsonConvert.DeserializeObject(this._Data.ToString());
                            json.Append("\"data\":" + this._Data + ",");
                        }
                    }
                    else
                    {

                        if (this.JsonSerializerSettings == null)
                        {
                            this.JsonSerializerSettings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

                        }

                        JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                        {
                            //日期类型默认格式化处理
                            this.JsonSerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                            this.JsonSerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                            return this.JsonSerializerSettings;
                        });
                        this.JsonSerializerSettings.Converters = this.JsonConverter;
                        this.JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        json.Append("\"data\":" + JsonConvert.SerializeObject(this._Data, Formatting.None, this.JsonSerializerSettings) + ",");
                    }
                }
                json.Append("\"result\": " + (int)this._Result + ",");


                if (this._Code != ResultCode.Unknown)
                {
                    json.Append("\"code\": \"" + ((int)this._Code).ToString() + "\",");
                }


                json.Append("\"message\":" + JsonConvert.SerializeObject(this._Message) + "");


            }
            catch (Exception ex)
            {

                json.Append("\"result\": " + (int)ResultType.Exception + ",");
                json.Append("\"message\":\"Does not conform to the Json format, the conversion of Json format exception\"");
                //转换失败记录日志
            }
            json.Append("}");

            string s = json.ToString();

            return s;
        }

    }
    #endregion
}
