using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YdtSdk;

namespace WebBack.Controllers.App
{
    public class CarInsureOfferController : Controller
    {
        [AllowAnonymous]
        public ViewResult OfferImg(OfferImgModel model)
        {
            //var a = Request;
            //OfferImgModel model = new OfferImgModel();

            //if (Session["offerImgModel"] != null)
            //{
            //    model = (OfferImgModel)Session["offerImgModel"];
            //}
            return View(model);
        }
    }
}