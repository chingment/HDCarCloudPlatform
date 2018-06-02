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

            model.Index = ServiceFactory.Index.GetData(userId);
            model.ProductKind = ServiceFactory.Product.GetKinds();
            model.Cart = ServiceFactory.Cart.GetData(userId);
            model.Personal = ServiceFactory.Personal.GetData(userId);
            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
            return new APIResponse(result);
        }

        [HttpPost]
        public APIResponse UploadLogTrace(UploadLogTracePms pms)
        {
            Log.Info("UploadLogTrace");
            if (!string.IsNullOrEmpty(pms.Trace))
            {
                Log.Error(pms.Trace);
            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "上传成功");
        }

        [HttpPost]
        public APIResponse UploadLogFile()
        {
            Log.Info("UploadLogFile");
            //if (!string.IsNullOrEmpty(pms.Trace))
            //{
            //    Log.Error(pms.Trace);
            //}

            return ResponseResult(ResultType.Success, ResultCode.Success, "上传成功");
        }
    }
}