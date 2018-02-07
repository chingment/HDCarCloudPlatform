using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppApi.Controllers
{
    public class ExtendedAppController : Controller
    {
        // GET: ExtendedApp
        public ActionResult GoTo(int userId, int merchantId, int posMachineId, int appId)
        {
            return Redirect("http://www.baidu.com");
        }
    }
}