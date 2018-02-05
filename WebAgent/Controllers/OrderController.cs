using Lumos.BLL;
using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgent.Models.Order;

namespace WebAgent.Controllers
{

    public class OrderController : WebBackController
    {



        public ViewResult List(Enumeration.OrderStatus status)
        {
            ListViewModel model = new ListViewModel();
            model.Status = status;
            return View(model);
        }


        public JsonResult GetList(OrderSearchCondition condition)
        {


            string clientCode = condition.ClientCode.ToSearchString();
            string yYZZ_Name = condition.YYZZ_Name.ToSearchString();
            string sn = condition.Sn.ToSearchString();


            Enumeration.OrderStatus status = condition.Status;

            var query = (from o in CurrentDb.Order
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where o.PId == null
                         &&
                         (clientCode.Length == 0 || m.ClientCode.Contains(clientCode)) &&
                                 (yYZZ_Name.Length == 0 || m.YYZZ_Name.Contains(yYZZ_Name)) &&
                                 (sn.Length == 0 || o.Sn.Contains(sn))
                                 &&
                                 o.AgentId == this.CurrentUserId

                         select new { o.Id, m.ClientCode, m.YYZZ_Name, o.Sn, o.ProductType, o.ProductName, o.Price, o.Status, o.Remarks, o.SubmitTime, o.CompleteTime, o.CancleTime, o.FollowStatus, o.ContactPhoneNumber, o.Contact }
                        );


            if (status != Enumeration.OrderStatus.Unknow)
            {
                //&& (((int)m.ProductType).ToString().StartsWith("201"))
                query = query.Where(m => m.Status == status);
            }

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;

            query = query.OrderByDescending(r => r.SubmitTime).Skip(pageSize * (pageIndex)).Take(pageSize);

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
                    item.ProductType,
                    item.SubmitTime,
                    Status = item.Status.GetCnName()
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");

        }


    }
}