using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using log4net;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using WebAgent.Models.Home;
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

namespace WebAgent.Controllers
{

    public class HomeController : WebBackController
    {
        public static string ToHexString(byte[] bytes)
        {
            var hex = new StringBuilder();
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] bytes;
            if (password == null)
                return null;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(32);

            }
            string a = ToHexString(salt);
            Guid a1 = Guid.Parse(a);
            string b = ToHexString(bytes);
            byte[] numArray = new byte[49];
            Buffer.BlockCopy(salt, 0, numArray, 1, 16);
            Buffer.BlockCopy(bytes, 0, numArray, 17, 32);
            return Convert.ToBase64String(numArray);
        }

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Main()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            Session["WebBackLoginVerifyCode"] = null;
            if (Request.IsAuthenticated)
            {
                if (Request.QueryString["out"] == null)
                {
                    return Redirect(OwnWebSettingUtils.GetHomePage());
                }
            }

            return View();
        }


        public ViewResult ChangePassword()
        {
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
        [CheckVerifyCode("WebBackLoginVerifyCode")]
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

            gotoViewModel.Url = OwnWebSettingUtils.GetHomePage();
            return Json(ResultType.Success, gotoViewModel, OwnOperateTipUtils.LOGIN_SUCCESS);

        }

        /// <summary>
        /// 退出方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            ILog log = LogManager.GetLogger(CommonSetting.LoggerLoginWeb);
            log.Info(FormatUtils.LoginOffWeb(this.CurrentUserId, User.Identity.GetUserName()));
            var identity = new AspNetIdentiyAuthorizeRelay<SysUser>();
            identity.SignOut();
            return Redirect(OwnWebSettingUtils.GetLoginPage());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(ChangePasswordModel model)
        {
            string oldPassword = model.OldPassword;
            string newPassword = model.NewPassword;
            var authorizeRelay = new AspNetIdentiyAuthorizeRelay<SysUser>();
            bool result = authorizeRelay.ChangePassword(this.CurrentUserId, this.CurrentUserId, oldPassword, newPassword);

            if (!result)
            {
                return Json(ResultType.Failure, OwnOperateTipUtils.CHANGEPASSWORD_OLDPASSWORDINCORRECT);
            }



            if (Request.IsAuthenticated)
            {
                authorizeRelay.SignOut();
            }


            return Json(ResultType.Success, "点击<a href=\"" + OwnWebSettingUtils.GetLoginPage() + "\">登录</a>");

        }

    }
}