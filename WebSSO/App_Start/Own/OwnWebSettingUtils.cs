using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSSO
{
    public class OwnWebSettingUtils
    {
        /// <summary>
        /// 获取登录的页面
        /// </summary>
        /// <returns></returns>
        public static string GetLoginPage()
        {
            return "/Home/Login";
        }

        /// <summary>
        /// 获取网站的名称
        /// </summary>
        /// <returns></returns>
        public static string GetWebName()
        {
            return "车险代理商平台";
        }

        /// <summary>
        /// 是否能查看错误日志的堆栈
        /// </summary>
        /// <returns></returns>
        public static bool CanViewErrorStackTrace()
        {
            string[] canViewIp = new string[] { "127.0.0.1", "::1" };


            string ip = CommonUtils.GetIP();

            if (canViewIp.Contains(ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}