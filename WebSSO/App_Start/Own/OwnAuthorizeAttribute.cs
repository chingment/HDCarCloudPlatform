using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using System.Reflection;
using log4net;
using Lumos.Common;
using System.Globalization;
using Lumos.Mvc;

namespace WebSSO
{
    #region 授权过滤器
    // 摘要:
    //     继承Authorize属性
    //     扩展Permission权限代码,用来控制用户是否拥有该类或方法的权限
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OwnAuthorizeAttribute : AuthorizeAttribute
    {
    
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);


            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
            {
                return;
            }

            #endregion
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            filterContext.Result = new RedirectResult(OwnWebSettingUtils.GetLoginPage());

        }
    }
   
}