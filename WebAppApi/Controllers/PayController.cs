using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppApi.Controllers
{
    public class PayController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse ReceiveNotifyByStar()
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();

            Log.Info("ReceiveNotify：" + postData);

            return ResponseResult(ResultType.Success, ResultCode.Success, "接收成功");
        }
    }
}