using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.ServiceFee;

namespace WebBack.Controllers.Biz
{
    public class ServiceFeeController : OwnBaseController
    {
        public ViewResult Details(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return View(model);
        }
    }
}