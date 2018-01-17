using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebAppApi.Controllers.Common
{

    public class AppHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>

        /// 异常

        /// </summary>

        /// <param name="filterContext"></param>

        public override void OnException(ExceptionContext filterContext)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            //使用log4net或其他记录错误消息

            Exception ex = filterContext.Exception;

            log.Error("API应用程序错误", ex);


        }
    }
}