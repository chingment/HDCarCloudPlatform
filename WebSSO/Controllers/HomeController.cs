using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using WebSSO.Models.Home;
using System.Web.Mvc;
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
            gotoViewModel.Url = OwnWebSettingUtils.GetLoginPage();


            LoginManager<SysUser> loginManager = new LoginManager<SysUser>();

            var result = loginManager.SignIn(model.UserName, model.Password, CommonUtils.GetIP(), Enumeration.LoginType.Website);

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
                        model.ReturnUrl = "http://localhost:12060?token=";
                        break;
                    case Enumeration.UserType.Client:
                        break;
                    case Enumeration.UserType.Agent:
                        model.ReturnUrl = "http://localhost:12068/";
                        break;
                    case Enumeration.UserType.Salesman:
                        break;
                }
            }

            gotoViewModel.Url = model.ReturnUrl;
            return Json(ResultType.Success, gotoViewModel, OwnOperateTipUtils.LOGIN_SUCCESS);

        }
    }
}