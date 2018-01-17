using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.App.Product;

namespace WebBack.Controllers.App
{
    public class ProductController : Controller
    {
        // GET: Product
        public ViewResult GoodsDetails(int id,string clientcode)
        {
            GoodsDetailsViewModel model = new GoodsDetailsViewModel(id);
            return View(model);
        }
    }
}