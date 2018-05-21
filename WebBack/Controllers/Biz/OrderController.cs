﻿using Lumos.BLL;
using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.Order;

namespace WebBack.Controllers.Biz
{
    [OwnAuthorize(PermissionCode.订单查询)]
    public class OrderController : OwnBaseController
    {
        public ViewResult CarInsureFollow(int id)
        {
            CarInsureFollowViewModel model = new CarInsureFollowViewModel(id);
            return View(model);
        }

        public ViewResult CarClaimFollow(int id)
        {
            CarClaimFollowViewModel model = new CarClaimFollowViewModel(id);
            return View(model);
        }

        public ViewResult CarInsurePay(int id)
        {
            CarInsurePayViewModel model = new CarInsurePayViewModel(id);
            return View(model);
        }

        public ViewResult List(Enumeration.OrderStatus status)
        {
            ListViewModel model = new ListViewModel();
            model.Status = status;
            return View(model);
        }


        [OwnAuthorize(PermissionCode.订单支付确认)]
        public ViewResult ConfirmPayList()
        {
            return View();
        }

        public ViewResult PayTransList()
        {
            return View();
        }

        public CustomJsonResult GetList(SearchCondition condition)
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


                         select new { o.Id, m.ClientCode, m.YYZZ_Name, o.Sn, o.Type, o.TypeName, o.Price, o.Status, o.Remarks, o.SubmitTime, o.CompleteTime, o.CancleTime, o.FollowStatus, m.ContactPhoneNumber}
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
                    item.TypeName,
                    item.Type,
                    item.SubmitTime,
                    Status = item.Status.GetCnName()
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");

        }


        [OwnAuthorize(PermissionCode.订单支付确认)]
        public CustomJsonResult GetConfirmPayList(Models.Biz.Order.SearchCondition condition)
        {


            string clientCode = condition.ClientCode.ToSearchString();
            string yYZZ_Name = condition.YYZZ_Name.ToSearchString();
            string sn = condition.Sn.ToSearchString();


            Enumeration.OrderStatus status = condition.Status;

            var query = (from o in CurrentDb.Order
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         join u in CurrentDb.SysClientUser on o.UserId equals u.Id
                         where

                         o.Status == Enumeration.OrderStatus.WaitPay
                         &&
                         (clientCode.Length == 0 || m.ClientCode.Contains(clientCode)) &&
                                 (yYZZ_Name.Length == 0 || m.YYZZ_Name.Contains(yYZZ_Name)) &&
                                 (sn.Length == 0 || o.Sn.Contains(sn))


                         select new { o.Id, m.ClientCode,u.UserName, m.YYZZ_Name, o.Sn, o.Type, o.TypeName, o.Price, o.Status, o.Remarks, o.SubmitTime, o.CompleteTime, o.CancleTime, o.FollowStatus }
                        );


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
                    item.UserName,
                    item.Sn,
                    item.YYZZ_Name,
                    item.TypeName,
                    item.Type,
                    item.SubmitTime,
                    Status = item.Status.GetCnName(),
                    item.Price
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");

        }


        public CustomJsonResult Cancle(Order model)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = BizFactory.Order.Cancle(this.CurrentUserId, model.Sn);

            return result;
        }

        public CustomJsonResult SubmitCarInsureFollow(CarInsureFollowViewModel model)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = BizFactory.Order.SubmitFollowInsure(this.CurrentUserId, model.OrderToCarInsure);

            return result;
        }

        public CustomJsonResult SubmitEstimateList(CarClaimFollowViewModel model)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = BizFactory.Order.SubmitEstimateList(this.CurrentUserId, model.OrderToCarClaim.UserId, model.OrderToCarClaim.Id, model.OrderToCarClaim.EstimateListImgUrl);

            return result;
        }


        [OwnAuthorize(PermissionCode.订单支付确认)]
        [HttpPost]
        public CustomJsonResult ConfirmPay(int orderId)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = BizFactory.Pay.ConfirmPayByStaff(this.CurrentUserId, orderId);

            return result;
        }

    }
}