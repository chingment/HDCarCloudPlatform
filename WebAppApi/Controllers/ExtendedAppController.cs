using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppApi.Models.ExtendedApp;

namespace WebAppApi.Controllers
{
    public class ExtendedAppController : Controller
    {
        // GET: ExtendedApp
        public ActionResult GoTo(int userId, int merchantId, int posMachineId, int appId)
        {
            string server = System.Configuration.ConfigurationManager.AppSettings["custom:WebAppServerUrl"];
            string url = "";
            switch (appId)
            {
                case 2:
                    url = string.Format("{0}{1}?userId={2}&merchantId={3}&posMachineId={4}", server, "/ExtendedApp/PosCredit", userId, merchantId, posMachineId);
                    break;
                case 6:
                    url = "http://www.omlife.com.cn/xinbox/toApplyCredit_inst?operCode=00059%E5%A5%BD%E6%98%93%E8%81%94_APP&orgno=00059&platform=%E5%A5%BD%E6%98%93%E8%81%94_APP&sign=F13EECFEF9DDCD63B7ECB49FD32A7DF0";
                    //url = string.Format("{0}{1}?userId={2}&merchantId={3}&posMachineId={4}", server, "/ExtendedApp/PosCredit", userId, merchantId, posMachineId);
                    break;
                default:
                    url = string.Format("{0}{1}?userId={2}&merchantId={3}&posMachineId={4}", server, "/ExtendedApp/ComeInSoon", userId, merchantId, posMachineId);
                    break;
            }

            return Redirect(url);
        }

        public ActionResult ComeInSoon()
        {
            return View();
        }

        public ActionResult PosCredit(int userId, int merchantId, int posMachineId)
        {
            PosCreditViewModel model = new PosCreditViewModel();
            model.UserId = userId;
            model.MerchantId = merchantId;
            model.PosMachineId = posMachineId;
            return View(model);
        }

        [HttpPost]
        public CustomJsonResult SubmitPosCredit(PosCreditViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            var orderToCredit = new OrderToCredit();
            orderToCredit.UserId = model.UserId; ;
            orderToCredit.MerchantId = model.MerchantId;
            orderToCredit.PosMachineId = model.PosMachineId;
            orderToCredit.CreditClass = "POS贷";
            orderToCredit.Creditline = 50000;
            reuslt = BizFactory.OrderToCredit.Submit(model.UserId, orderToCredit);
            return reuslt;
        }

    }
}