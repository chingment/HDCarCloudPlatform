using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppApi.Controllers
{
    public class ResourceController : Controller
    {
        //用工协议
        public ActionResult yonggongxieyi()
        {
            return View();
        }

        //理赔协议
        public ActionResult lipeixieyi()
        {
            return View();
        }


    }
}