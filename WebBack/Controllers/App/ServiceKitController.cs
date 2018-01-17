using Lumos.BLL;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.App.ServiceKit;

namespace WebBack.Controllers.App
{
    public class ServiceKitController : Controller
    {
        // GET: ServiceKit
        public ViewResult CarInsExpireDate()
        {

            return View();
        }


        [HttpPost]
        public JsonResult GetCarInsExpireDate(CarInsExpireDateViewModel model)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = SdkFactory.FangWei.GetVehiclesInfo(0,model.ClientCode, model.CarPlateNo, model.CarVinLast6Num);

            return result;
        }

        [HttpPost]
        public JsonResult GetCarInsExpireDateSearchHis(CarInsExpireDateViewModel model)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = SdkFactory.FangWei.GetCarInsuranceExpireDateSearchHis(0, model.ClientCode);

            return result;
        }
    }
}