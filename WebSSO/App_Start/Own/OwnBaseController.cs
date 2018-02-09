using Lumos.Common;
using Lumos.DAL;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System.IO;
using Newtonsoft.Json.Converters;
using Lumos.Mvc;

namespace WebSSO
{

    /// <summary>
    /// BaseController用来扩展Controller,凡是在都该继承BaseController
    /// </summary>
    [OwnException]
    [ValidateInput(false)]
    public abstract class OwnBaseController : BaseController
    {
 
        private LumosDbContext _currentDb;

        public LumosDbContext CurrentDb
        {
            get
            {
                return _currentDb;
            }
        }

        protected ILog Log
        {
            get
            {
                return LogManager.GetLogger(this.GetType());
            }
        }

        protected static ILog GetLog(Type t)
        {
            return LogManager.GetLogger(t);
        }

        protected static ILog GetLog()
        {
            var trace = new System.Diagnostics.StackTrace();
            Type baseType = typeof(BaseController);
            for (int i = 0; i < trace.FrameCount; i++)
            {
                var frame = trace.GetFrame(i);
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                if (type.IsSubclassOf(baseType)) return GetLog(type);
            }
            return LogManager.GetLogger(baseType);
        }

        protected void SetTrackID()
        {
            if (LogicalThreadContext.Properties["trackid"] == null)
                LogicalThreadContext.Properties["trackid"] = this.Session.SessionID;
        }

        public OwnBaseController()
        {
            _currentDb = new LumosDbContext();
            var identity = new AspNetIdentiyAuthorizeRelay<SysUser>();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            SetTrackID();

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                CurrentDb.SysPageAccessRecord.Add(new SysPageAccessRecord() { UserId = User.Identity.GetUserId<int>(), AccessTime = DateTime.Now, PageUrl = filterContext.HttpContext.Request.Url.AbsolutePath, Ip = CommonUtils.GetIP() });
                CurrentDb.SaveChanges();
            }

            ILog log = LogManager.GetLogger(CommonSetting.LoggerAccessWeb);
            log.Info(FormatUtils.AccessWeb(User.Identity.GetUserId<int>(), User.Identity.GetUserName()));

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (!skipAuthorization)
            {
                if (filterContext.HttpContext.Request.Url.AbsolutePath.IndexOf(OwnWebSettingUtils.GetLoginPage()) == -1)
                {
                    if (Request.IsAuthenticated)
                    {
                        var userId = User.Identity.GetUserId<int>();
                        var user = CurrentDb.SysAgentUser.Where(m => m.Id == userId).FirstOrDefault();
                        if (user == null)
                        {
                            Response.Redirect(OwnWebSettingUtils.GetLoginPage() + "?out=0");
                        }
                    }
                }
            }

        }

        public int CurrentUserId
        {
            get
            {
                return User.Identity.GetUserId<int>();
            }
        }

    }
}