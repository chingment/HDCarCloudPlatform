using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppApi.Models.Resource;

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

        public ActionResult PrdDetails(int id)
        {
            DetailsViewModelByProduct model = new DetailsViewModelByProduct(id);
            return View(model);
        }

        public ViewResult BannerDetails(int id)
        {
            DetailsViewModelByBanner model = new DetailsViewModelByBanner(id);
            return View(model);
        }



    }
}