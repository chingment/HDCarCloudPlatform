using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgent.Models.CarInsureOffer;

namespace WebAgent.Controllers
{
    public class CarInsureOfferController : OwnBaseController
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



        public CustomJsonResult GetList(CarInsureOfferSearchCondition condition)
        {

            string sn = condition.Sn.ToSearchString();
            string clientCode = condition.ClientCode.ToSearchString();
            string clientName = condition.ClientName.ToSearchString();


            var query = (from o in CurrentDb.OrderToCarInsure
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where o.PId == null &&
                               (o.Status == condition.Status || condition.Status == Enumeration.OrderStatus.Unknow) &&
                        (sn.Length == 0 || o.Sn.Contains(sn)) &&
                            (clientCode.Length == 0 || m.ClientCode.Contains(clientCode)) &&
                                (clientCode.Length == 0 || m.YYZZ_Name.Contains(clientCode))


                         select new { o.Id, m.ClientCode, o.Sn, m.YYZZ_Name, m.ContactPhoneNumber, o.ProductName, o.ProductType, o.SubmitTime, o.Status, o.CreateTime });



            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                list.Add(new
                {
                    item.Id,
                    item.ClientCode,
                    item.Sn,
                    item.YYZZ_Name,
                    item.ContactPhoneNumber,
                    item.ProductName,
                    item.SubmitTime,
                    Status = item.Status.GetCnName()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

    }
}