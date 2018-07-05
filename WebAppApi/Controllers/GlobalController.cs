using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models;
using WebAppApi.Models.Account;
using WebAppApi.Models.CarService;
using WebAppApi.Models.Global;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class GlobalController : OwnBaseApiController
    {

        [HttpPost]
        public APIResponse UploadLogTrace(UploadLogTracePms pms)
        {
            Log.Info("AppVersion:" + GetAppVersion());

            log4net.ILog log = log4net.LogManager.GetLogger("AppErrorLogFileAppender");

            log.Info("UploadLogFile");
            if (!string.IsNullOrEmpty(pms.Trace))
            {
                log.Error(pms.Trace);
            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "上传成功");
        }

        [HttpPost]
        public APIResponse UploadLogFile()
        {
            log4net.ILog log = log4net.LogManager.GetLogger("AppErrorLogFileAppender");

            log.Info("UploadLogFile");
            //if (!string.IsNullOrEmpty(pms.Trace))
            //{
            //    Log.Error(pms.Trace);
            //}

            return ResponseResult(ResultType.Success, ResultCode.Success, "上传成功");
        }
    }
}