using Lumos.Entity;
using WebBack.Models.Biz.Shopping;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.BLL;

namespace WebBack.Controllers.Biz
{
    public class ShoppingController : OwnBaseController
    {

        public ActionResult ListByDealtWaitSend()
        {
            return View();
        }

        public ActionResult ListBySended()
        {
            return View();
        }

        public ActionResult DealtWaitSend(int id)
        {
            DealtWaitSendViewModel model = new DealtWaitSendViewModel();
            model.LoadData(id);
            return View(model);
        }

        public ActionResult Details(int id)
        {
            DetailsViewModel model = new DetailsViewModel();
            model.LoadData(id);
            return View(model);
        }

        public CustomJsonResult GetListByDealtWaitSend(SearchCondition condition)
        {

            string clientCode = condition.ClientCode.ToSearchString();
            string yYZZ_Name = condition.YYZZ_Name.ToSearchString();
            string sn = condition.Sn.ToSearchString();

            var query = (from o in CurrentDb.OrderToShopping
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where o.Status == Enumeration.OrderStatus.Completed

                         && o.FollowStatus == (int)Enumeration.ShoppingFollowStatus.WaitSend
                         &&
                               (clientCode.Length == 0 || m.ClientCode.Contains(clientCode)) &&
                                 (yYZZ_Name.Length == 0 || m.YYZZ_Name.Contains(yYZZ_Name)) &&
                                 (sn.Length == 0 || o.Sn.Contains(sn))
                         select new { o.Id, m.ClientCode, o.Sn, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, o.TypeName, o.Price, o.SubmitTime, o.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                list.Add(new
                {
                    item.Id,
                    item.ClientCode,
                    item.Sn,
                    item.YYZZ_Name,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    item.Price,
                    item.TypeName,
                    item.SubmitTime
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        public CustomJsonResult GetListBySended(SearchCondition condition)
        {

            string clientCode = condition.ClientCode.ToSearchString();
            string yYZZ_Name = condition.YYZZ_Name.ToSearchString();
            string sn = condition.Sn.ToSearchString();

            var query = (from o in CurrentDb.OrderToShopping
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where o.Status == Enumeration.OrderStatus.Completed

                         && o.FollowStatus == (int)Enumeration.ShoppingFollowStatus.Sended
                         &&
                               (clientCode.Length == 0 || m.ClientCode.Contains(clientCode)) &&
                                 (yYZZ_Name.Length == 0 || m.YYZZ_Name.Contains(yYZZ_Name)) &&
                                 (sn.Length == 0 || o.Sn.Contains(sn))
                         select new { o.Id, m.ClientCode, o.Sn, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, o.TypeName, o.Price, o.SubmitTime, o.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                list.Add(new
                {
                    item.Id,
                    item.ClientCode,
                    item.Sn,
                    item.YYZZ_Name,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    item.Price,
                    item.TypeName,
                    item.SubmitTime
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult DealtWaitSend(DealtWaitSendViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.OrderToShopping.DealtWaitSend(this.CurrentUserId, model.OrderToShopping);

            return reuslt;
        }

    }
}