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
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System.IO;
using Newtonsoft.Json.Converters;
using Lumos.Mvc;

namespace WebBack
{

    /// <summary>
    /// BaseController用来扩展Controller,凡是在都该继承BaseController
    /// </summary>
    [OwnException]
    [OwnAuthorize]
    [ValidateInput(false)]
    public abstract class OwnBaseController : BaseController
    {
        #region 公共的方法
        public string ConvertToZTreeJson(object obj, string idField, string pIdField, string nameField, string IconSkinField, params int[] isCheckedIds)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            Type t = obj.GetType();
            foreach (var model in (object[])obj)
            {
                Type t1 = model.GetType();
                Json.Append("{");
                foreach (PropertyInfo p in t1.GetProperties())
                {
                    string name = p.Name.Trim().ToLower();
                    object value = p.GetValue(model, null);
                    if (name == idField.ToLower())
                    {
                        Json.Append("\"id\":" + JsonConvert.SerializeObject(value) + ",\"open\":true,");
                        int v = int.Parse(value.ToString());
                        if (isCheckedIds.Contains(v))
                        {
                            Json.Append("\"checked\":true,");
                        }
                    }
                    else if (name == pIdField.Trim().ToLower())
                    {
                        Json.Append("\"pId\":" + JsonConvert.SerializeObject(value) + ",");

                        if (value == null || value.ToString() == "")
                        {
                            Json.Append("\"iconSkin\":\"" + IconSkinField + "\" ");
                            Json.Append(",");
                        }
                        else
                        {
                            Json.Append("\"iconSkin\":\"" + IconSkinField + "s\" ");
                            Json.Append(",");
                        }

                    }
                    else if (name == nameField.Trim().ToLower())
                    {
                        Json.Append("\"name\":" + JsonConvert.SerializeObject(value) + ",");

                    }
                    else
                    {
                        Json.Append("\"" + p.Name + "\":" + JsonConvert.SerializeObject(value) + ",");
                    }
                }
                if (Json.Length > 2)
                {
                    Json.Remove(Json.Length - 1, 1);
                }
                Json.Append("},");
            }
            if (Json.Length > 2)
            {
                Json.Remove(Json.Length - 1, 1);
            }
            Json.Append("]");
            return Json.ToString();
        }

        public string ConvertToZTreeJson2(object obj, string idField, string pIdField, string nameField, string IconSkinField, params int[] isCheckedIds)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            Type t = obj.GetType();
            foreach (var model in (object[])obj)
            {
                Type t1 = model.GetType();
                Json.Append("{");
                foreach (PropertyInfo p in t1.GetProperties())
                {
                    string name = p.Name.Trim().ToLower();
                    object value = p.GetValue(model, null);
                    if (name == idField.ToLower())
                    {
                        Json.Append("\"id\":" + JsonConvert.SerializeObject(value) + ",");
                        int v = int.Parse(value.ToString());
                        if (isCheckedIds.Contains(v))
                        {
                            Json.Append("\"checked\":true,");
                        }
                    }
                    else if (name == pIdField.Trim().ToLower())
                    {
                        Json.Append("\"pId\":0,");

                        if (value == null || value.ToString() == "")
                        {
                            Json.Append("\"iconSkin\":\"" + IconSkinField + "\" ");
                            Json.Append(",");
                        }
                        else
                        {
                            Json.Append("\"iconSkin\":\"" + IconSkinField + "s\" ");
                            Json.Append(",");
                        }

                    }
                    else if (name == nameField.Trim().ToLower())
                    {
                        Json.Append("\"name\":" + JsonConvert.SerializeObject(value) + ",");

                    }
                    else
                    {
                        Json.Append("\"" + p.Name + "\":" + JsonConvert.SerializeObject(value) + ",");
                    }
                }
                if (Json.Length > 2)
                {
                    Json.Remove(Json.Length - 1, 1);
                }
                Json.Append("},");
            }
            if (Json.Length > 2)
            {
                Json.Remove(Json.Length - 1, 1);
            }
            Json.Append("]");
            return Json.ToString();
        }


        #endregion 公共的方法

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
            //return LogWebBack.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); //有问题，子类调用，返回的还是父类的logger

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
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            SetTrackID();

            ILog log = LogManager.GetLogger(CommonSetting.LoggerAccessWeb);
            log.Info(FormatUtils.AccessWeb(this.CurrentUserId, ""));

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                CurrentDb.SysPageAccessRecord.Add(new SysPageAccessRecord() { UserId = this.CurrentUserId, AccessTime = DateTime.Now, PageUrl = filterContext.HttpContext.Request.Url.AbsolutePath, Ip = CommonUtils.GetIP() });
                CurrentDb.SaveChanges();
            }


        }

        public int CurrentUserId
        {
            get
            {
                return OwnRequest.GetCurrentUserId();
            }
        }

    }
}