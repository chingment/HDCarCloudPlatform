using Lumos.BLL;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models.Sms;


namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class SmsController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse GetForgetPwdCode(GetForgetPwdCodeModel model)
        {
            var clientUser = CurrentDb.SysClientUser.Where(m => m.UserName == model.Phone).FirstOrDefault();
            if (clientUser == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "用户手机不正确");
            }
            string token = "";
            string validCode = "";

            IResult iResult = BizFactory.Sms.SendResetPasswordValidCode(clientUser.Id, clientUser.PhoneNumber, out validCode, out token);

            if (iResult.Result != ResultType.Success)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "获取短信失败");
            }

            ResetPwdResultModel resultModel = new ResetPwdResultModel();
            resultModel.UserName = model.Phone;
            resultModel.ValidCode = validCode;
            resultModel.Token = token;

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", resultModel);
        }

        [HttpPost]
        public APIResponse GetCreateAccountCode(GetCreateAccountCodeModel model)
        {
            var clientUser = CurrentDb.SysClientUser.Where(m => m.UserName == model.Phone).FirstOrDefault();
            if (clientUser != null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该手机号已经存在");
            }
            string token = "";
            string validCode = "";
            int seconds = 0;
            IResult isSuccess = BizFactory.Sms.SendCreateAccountCode(0, model.Phone, out validCode, out token,out seconds);
            if (isSuccess.Result != ResultType.Success)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "获取短信失败");
            }

            GetCreateAccountCodeResultModel resultModel = new GetCreateAccountCodeResultModel();
            resultModel.Phone = model.Phone;
            resultModel.ValidCode = validCode;
            resultModel.Token = token;
            resultModel.Seconds = seconds;

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", resultModel);
        }
    }
}