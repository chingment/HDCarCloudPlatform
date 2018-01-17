using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.App.Order;

namespace WebBack.Controllers.App
{
    public class OrderController : Controller
    {
        // GET: Order
        public ViewResult InsOfferPay(int offerId)
        {
            InsOfferPayViewModel model = new InsOfferPayViewModel(offerId); 
            return View(model);
        }
    }
}