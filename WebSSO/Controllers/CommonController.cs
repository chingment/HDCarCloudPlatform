﻿using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Lumos.Entity;
using Lumos.Mvc;
using log4net;
using System.Text;

namespace WebSSO.Controllers
{

    public class CommonController : WebBackController
    {
        /// <summary>
        /// 获取验证码的图片 使用方式 请求url:/Common/GetImgVerifyCode?name=sessionname
        /// </summary>
        /// <param name="name">代表后台session的名称</param>
        /// <returns>返回一张带数字的图片</returns>
        [AllowAnonymous]
        public ActionResult GetImgVerifyCode(string name)
        {
            VerifyCodeHelper v = new VerifyCodeHelper();
            v.CodeSerial = "0,1,2,3,4,5,6,7,8,9";
            string code = v.CreateVerifyCode(); //取随机码 
            v.CreateImageOnPage(code, ControllerContext.HttpContext);   //输出图片
            Session[name] = code;   //Session 取出验证码
            Response.End();
            return null;
        }
    }
}