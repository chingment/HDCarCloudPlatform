using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using log4net;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using WebSSO.Models.Home;
using Lumos.Mvc;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Lumos.BLL;
using System.Security.Cryptography;
using System.Text;
using MySDK;
using System.Linq;
using System.Data.Entity.Core.Objects;

namespace WebSSO.Controllers
{

    public class HomeController : WebBackController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            Session["WebSSOLoginVerifyCode"] = null;
            return View();
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CheckVerifyCode("WebSSOLoginVerifyCode")]
        public JsonResult Login(LoginModel model)
        {
            GoToViewModel gotoViewModel = new GoToViewModel();
            gotoViewModel.Url = OwnWebSettingUtils.GetLoginPage();


            LoginManager<SysAgentUser> loginWebBack = new LoginManager<SysAgentUser>();

            var result = loginWebBack.SignIn(model.UserName, model.Password, CommonUtils.GetIP(), Enumeration.LoginType.Website);


            if (result.ResultType == Enumeration.LoginResult.Failure)
            {

                if (result.ResultTip == Enumeration.LoginResultTip.UserNotExist || result.ResultTip == Enumeration.LoginResultTip.UserPasswordIncorrect)
                {
                    return Json(ResultType.Failure, gotoViewModel, OwnOperateTipUtils.LOGIN_USERNAMEORPASSWORDINCORRECT);
                }

                if (result.ResultTip == Enumeration.LoginResultTip.UserDisabled)
                {
                    return Json(ResultType.Failure, gotoViewModel, OwnOperateTipUtils.LOGIN_ACCOUNT_DISABLED);
                }

                if (result.ResultTip == Enumeration.LoginResultTip.UserDeleted)
                {
                    return Json(ResultType.Failure, gotoViewModel, OwnOperateTipUtils.LOGIN_ACCOUNT_DELETE);
                }
            }

            gotoViewModel.Url = model.ReturnUrl;
            return Json(ResultType.Success, gotoViewModel, OwnOperateTipUtils.LOGIN_SUCCESS);

        }
    }
}