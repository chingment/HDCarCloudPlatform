using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppApi.Models;
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

        public ViewResult IdRecogInfo(string idnum)
        {
            //DetailsViewModelByBanner model = new DetailsViewModelByBanner(id);

            List<IdRecogModel> models = new List<IdRecogModel>();


            models.Add(new IdRecogModel { IdNum = "448837222031", Company = "广州永发皮具有限公司", ImgUrl = "~/Images/baobao2.jpg", Producer = "昌名代理生产", PuctionDate = "2018-05-27" });
            models.Add(new IdRecogModel { IdNum = "548837554026", Company = "广州永发皮具有限公司", ImgUrl = "~/Images/baobao2.jpg", Producer = "龙胜代理生产", PuctionDate = "2018-05-14" });
            models.Add(new IdRecogModel { IdNum = "844673554063", Company = "广州永发皮具有限公司", ImgUrl = "~/Images/baobao2.jpg", Producer = "明仔代理生产", PuctionDate = "2018-05-05" });



            IdRecogModel model = models.Where(m => m.IdNum == idnum).FirstOrDefault();
            if (model == null)
            {
                model = models[0];
            }

            return View(model);
        }

    }
}