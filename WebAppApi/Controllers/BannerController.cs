using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models.Banner;
using Lumos.Entity;
using Lumos.Mvc;
using System.Web.Mvc;

namespace WebAppApi.Controllers
{
    public class BannerController : Controller
    {
        public ViewResult Details(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return View(model);
        }
    }
}