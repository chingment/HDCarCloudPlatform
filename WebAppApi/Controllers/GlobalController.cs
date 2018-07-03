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
        [HttpGet]
        public APIResponse DataSet(int userId, int merchantId, int posMachineId, DateTime? datetime)
        {

            var model = new DataSetModel();

            model.Index = ServiceFactory.Index.GetPageData(userId);
            model.ProductKind = ServiceFactory.Product.GetKindPageData();
            model.Cart = ServiceFactory.Cart.GetPageData(userId, merchantId, posMachineId);
            model.Personal = ServiceFactory.Personal.GetPageData(userId);
            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
            return new APIResponse(result);
        }

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