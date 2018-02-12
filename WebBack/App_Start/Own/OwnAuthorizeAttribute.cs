using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using log4net;
using Lumos.Common;
using System.Globalization;
using Lumos.Mvc;
using Lumos.Session;

namespace WebBack
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OwnAuthorizeAttribute : ActionFilterAttribute
    {

        public OwnAuthorizeAttribute(params string[] permissions)
        {
            if (permissions != null)
            {
                if (permissions.Length > 0)
                {
                    this.Permissions = permissions;
                }
            }

        }

        public string[] Permissions { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
            {
                return;
            }

            var request = filterContext.RequestContext.HttpContext.Request;
            var response = filterContext.RequestContext.HttpContext.Response;
            bool isAjaxRequest = request.IsAjaxRequest();
            string userAgent = request.UserAgent;
            string returnUrl = isAjaxRequest == true ? request.UrlReferrer.AbsoluteUri : request.Url.AbsoluteUri;

            string token = request.QueryString["token"];
            if (token != null)
            {
                HttpCookie cookie_session = request.Cookies[OwnRequest.SESSION_NAME];
                if (cookie_session != null)
                {
                    cookie_session.Value = token;
                    response.AppendCookie(cookie_session);
                }
                else
                {
                    response.Cookies.Add(new HttpCookie(OwnRequest.SESSION_NAME, token));
                }
            }

            var userInfo = OwnRequest.GetUserInfo();

            if (userInfo == null)
            {
                if (token == null)
                {
                    MessageBoxModel messageBox = new MessageBoxModel();
                    messageBox.No = Guid.NewGuid().ToString();
                    messageBox.Type = MessageBoxTip.Exception;
                    messageBox.Title = "您没有权限访问,可能链接超时";
                    messageBox.Content = "请重新<a href=\"javascript:void(0)\" onclick=\"window.top.location.href='" + OwnWebSettingUtils.GetLoginPage(returnUrl) + "'\">登录</a>后打开";
                    string masterName = "_Layout";

                    filterContext.Result = new ViewResult { ViewName = "MessageBox", MasterName = masterName, ViewData = new ViewDataDictionary { Model = messageBox } };
                }
                else
                {
                    filterContext.Result = new RedirectResult(OwnWebSettingUtils.GetLoginPage(returnUrl));
                }

                return;
            }

            if (Permissions != null)
            {
                MessageBoxModel messageBox = new MessageBoxModel();
                messageBox.No = Guid.NewGuid().ToString();
                messageBox.Type = MessageBoxTip.Exception;
                messageBox.Title = "您没有权限访问,可能链接超时";

                if (!filterContext.HttpContext.Request.IsAuthenticated)
                {
                    messageBox.Content = "请重新<a href=\"javascript:void(0)\" onclick=\"window.top.location.href='" + OwnWebSettingUtils.GetLoginPage(returnUrl) + "'\">登录</a>后打开";
                }

                bool IsHasPermission = OwnRequest.IsInPermission(Permissions);

                if (!IsHasPermission)
                {
                    if (isAjaxRequest)
                    {
                        CustomJsonResult jsonResult = new CustomJsonResult(ResultType.Exception, ResultCode.Exception, messageBox.Title, messageBox);
                        jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                        filterContext.Result = jsonResult;
                        filterContext.Result.ExecuteResult(filterContext);
                        filterContext.HttpContext.Response.End();
                        return;
                    }
                    else
                    {
                        string masterName = "_Layout";
                        filterContext.Result = new ViewResult { ViewName = "MessageBox", MasterName = masterName, ViewData = new ViewDataDictionary { Model = messageBox } };
                        return;
                    }
                }
            }

            OwnRequest.Postpone();

            base.OnActionExecuting(filterContext);
        }
    }

}