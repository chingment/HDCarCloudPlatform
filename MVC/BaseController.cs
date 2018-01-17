using Lumos.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lumos.Mvc
{
    public abstract class BaseController : Controller
    {
        #region JsonResult 扩展

        protected internal JsonResult Json(ResultType type)
        {
            return Json(type, null, null);
        }

        protected internal JsonResult Json(ResultType type, string message)
        {
            return Json(type, null, message);
        }

        protected internal JsonResult Json(ResultType type, object content)
        {
            return Json(type, content, null);
        }

        protected JsonResult Json(ResultType type, object content, string message, params JsonConverter[] converters)
        {
            return Json(null, type, ResultCode.Unknown, message, content, null, converters);
        }

        protected JsonResult Json(ResultType type, object content, string message, JsonSerializerSettings settings)
        {
            return Json(null, type, ResultCode.Unknown, message, content, settings);
        }

        protected internal JsonResult Json(string contenttype, ResultType type, string message)
        {
            return Json(contenttype, type, null, message);
        }

        protected JsonResult Json(string contenttype, ResultType type, object content, string message, params JsonConverter[] converters)
        {
            return Json(contenttype, type, ResultCode.Unknown, message, content, null);
        }

        protected JsonResult Json(string contenttype, ResultType type, ResultCode code, string message, object content, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            return new CustomJsonResult(contenttype, type, code, message, content, settings, converters);
        }
        #endregion
    }
}
