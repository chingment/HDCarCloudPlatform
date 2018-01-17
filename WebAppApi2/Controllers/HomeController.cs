using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppApi.Controllers
{
    public class HomeController : Controller
    {
        private string host = "http://localhost:44051";

        public ActionResult Index()
        {
            HttpRequestClient.Send("" + host + "/api/values/1");


            return View();
        }

    }
}
