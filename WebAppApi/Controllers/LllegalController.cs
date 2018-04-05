﻿using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAppApi.Models.Lllegal;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class LllegalController : OwnBaseApiController
    {

        [HttpPost]
        public APIResponse<LllegalQueryResult> Query(LllegalQueryParams model)
        {
            var result = SdkFactory.HeLian.Query(model.UserId, model);

            return new APIResponse<LllegalQueryResult>(result);

        }

        public APIResponse Dealt(DealtParams model)
        {
            //业务人员模拟数据
            if (model.MerchantId == this.SalesmanMerchantId)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToLllegalDealt orderToLllegalDealt = new OrderToLllegalDealt();
            orderToLllegalDealt.UserId = model.UserId;
            orderToLllegalDealt.MerchantId = model.MerchantId;
            orderToLllegalDealt.PosMachineId = model.PosMachineId;
            orderToLllegalDealt.CarNo = model.CarNo.ToUpper();

            if (model.LllegalRecord == null || model.LllegalRecord.Count == 0)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "请选择要处理的违章");
            }

            List<OrderToLllegalDealtDetails> orderToLllegalDealtDetails = new List<OrderToLllegalDealtDetails>();

            if (model.LllegalRecord != null)
            {
                foreach (var item in model.LllegalRecord)
                {
                    var orderToLllegalDealtDetail = new OrderToLllegalDealtDetails();

                    orderToLllegalDealtDetail.BookNo = item.bookNo;
                    orderToLllegalDealtDetail.BookType = item.bookType;
                    orderToLllegalDealtDetail.BookTypeName = item.bookTypeName;
                    orderToLllegalDealtDetail.Address = item.address;
                    orderToLllegalDealtDetail.CityCode = item.cityCode;
                    orderToLllegalDealtDetail.Content = item.content;
                    orderToLllegalDealtDetail.Fine = item.fine;
                    orderToLllegalDealtDetail.Late_fees = item.late_fees;
                    orderToLllegalDealtDetail.LllegalCity = item.lllegalCity;
                    orderToLllegalDealtDetail.LllegalCode = item.lllegalCode;
                    orderToLllegalDealtDetail.LllegalDesc = item.lllegalDesc;
                    orderToLllegalDealtDetail.LllegalTime = item.lllegalTime;
                    orderToLllegalDealtDetail.OfferType = item.offerType;
                    orderToLllegalDealtDetail.OfserTypeName = item.ofserTypeName;
                    orderToLllegalDealtDetail.Point = item.point;
                    orderToLllegalDealtDetail.ServiceFee = item.serviceFee;

                    orderToLllegalDealtDetails.Add(orderToLllegalDealtDetail);
                }
            }

            IResult result = BizFactory.OrderToLllegalDealt.Submit(model.UserId, model.IsGetConfirmInfo, orderToLllegalDealt, orderToLllegalDealtDetails);
            return new APIResponse(result);

        }

        [HttpGet]
        public APIResponse<List<LllegalQueryRecord>> QueryLog(int userId, int merchantId, int posMachineId)
        {

            var result = SdkFactory.HeLian.QueryLog(userId, userId, merchantId, posMachineId);

            return new APIResponse<List<LllegalQueryRecord>>(result);

        }
    }
}