using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgent.Models.CarClaim;

namespace WebAgent.Controllers.Biz
{
    public class CarClaimController : OwnBaseController
    {

        public ViewResult List()
        {
            return View();
        }

        public ViewResult Details(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return View(model);
        }

    }
}