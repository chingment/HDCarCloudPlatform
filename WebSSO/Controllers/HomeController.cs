using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using WebSSO.Models.Home;
using System.Web.Mvc;
using Lumos.Session;
using System;

namespace WebSSO.Controllers
{

    public class HomeController : OwnBaseController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Session["WebSSOLoginVerifyCode"] = null;

            LoginModel model = new LoginModel();
            model.ReturnUrl = returnUrl;

            return View(model);
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

            var result = SysFactory.AuthorizeRelay.SignIn(model.UserName, model.Password, CommonUtils.GetIP(), Enumeration.LoginType.Website);

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

            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                switch (result.User.Type)
                {
                    case Enumeration.UserType.Staff:
                        model.ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["custom:WebBackUrl"];
                        break;
                    case Enumeration.UserType.Client:
                        break;
                    case Enumeration.UserType.Agent:
                        model.ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["custom:WebAgentUrl"];
                        break;
                    case Enumeration.UserType.Salesman:
                        break;
                }
            }

            if (model.ReturnUrl.IndexOf("?") < 0)
            {
                model.ReturnUrl += "?";
            }
            else
            {
                model.ReturnUrl += "&";
            }

            UserInfo userInfo = new UserInfo();
            userInfo.UserId = result.User.Id;
            userInfo.UserName = result.User.UserName;
            userInfo.Token = Guid.NewGuid().ToString().Replace("-", "");

            SSOUtil.SetUserInfo(userInfo);

            gotoViewModel.Url = string.Format("{0}token={1}", model.ReturnUrl, userInfo.Token);

            return Json(ResultType.Success, gotoViewModel, OwnOperateTipUtils.LOGIN_SUCCESS);

        }
    }
}