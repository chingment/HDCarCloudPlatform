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
            string server = System.Configuration.ConfigurationManager.AppSettings["custom:WebAppServerUrl"];
            string url = string.Format("{0}{1}", server, "/ExtendedApp/ComeInSoon");
            return Redirect(url);
        }

        public ActionResult ComeInSoon()
        {
            return View();
        }
    }
}