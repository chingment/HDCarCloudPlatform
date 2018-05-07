using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models.Order;
using Lumos.Entity.AppApi;
using WebAppApi.Models;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class OrderController : OwnBaseApiController
    {
        [HttpGet]
        public APIResponse GetList(int userId, int merchantId, int posMachineId, int pageIndex, Enumeration.OrderStatus status, Enumeration.OrderType type)
        {
            var order = (from o in CurrentDb.Order
                         where o.MerchantId == merchantId

                         select new { o.Id, o.Sn, o.Type, o.Price, o.Status, o.Remarks, o.SubmitTime, o.CompleteTime, o.CancleTime, o.FollowStatus }
                         );


            if (status != Enumeration.OrderStatus.Unknow)
            {
                if (status == Enumeration.OrderStatus.Completed)
                {
                    order = order.Where(m => m.Status == status);
                }
                else
                {
                    order = order.Where(m => m.Status == status && (m.Type != Enumeration.OrderType.LllegalQueryRecharge && m.Type != Enumeration.OrderType.LllegalDealt));
                }
            }
            else
            {
                order = order.Where(m =>
                (m.Status == Enumeration.OrderStatus.Submitted && (m.Type != Enumeration.OrderType.LllegalQueryRecharge && m.Type != Enumeration.OrderType.LllegalDealt))
                || (m.Status == Enumeration.OrderStatus.Follow && (m.Type != Enumeration.OrderType.LllegalQueryRecharge && m.Type != Enumeration.OrderType.LllegalDealt))
                || (m.Status == Enumeration.OrderStatus.WaitPay && (m.Type != Enumeration.OrderType.LllegalQueryRecharge && m.Type != Enumeration.OrderType.LllegalDealt))
                || (m.Status == Enumeration.OrderStatus.Completed)
                || (m.Status == Enumeration.OrderStatus.Cancled && (m.Type != Enumeration.OrderType.LllegalQueryRecharge && m.Type != Enumeration.OrderType.LllegalDealt))
                );
            }

            if (type != Enumeration.OrderType.Unknow)
            {
                order = order.Where(m => m.Type == type);
            }


            int pageSize = 10;

            order = order.OrderByDescending(r => r.SubmitTime).Skip(pageSize * (pageIndex)).Take(pageSize);



            var orderlist = order.ToList();
            List<OrderModel> model = new List<OrderModel>();
            foreach (var m in orderlist)
            {
                OrderModel orderModel = new OrderModel();
                orderModel.Id = m.Id;
                orderModel.Sn = m.Sn;
                orderModel.typeName = m.Type.GetCnName();
                orderModel.type = m.Type;
                orderModel.Status = m.Status;
                orderModel.Price = m.Price;
                switch (m.Status)
                {
                    case Enumeration.OrderStatus.Submitted:

                        #region 已提交
                        orderModel.Remarks = m.SubmitTime.ToUnifiedFormatDateTime();//备注提交时间
                        orderModel.StatusName = "已提交";

                        switch (m.Type)
                        {
                            case Enumeration.OrderType.InsureForCarForInsure:
                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("身份证号码", orderToCarInsure.CarOwnerIdNumber.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("状态", "请稍侯，报价中"));

                                break;
                            case Enumeration.OrderType.InsureForCarForClaim:

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToCarClaim.InsuranceCompanyName.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("对接人", string.Format("{0},{1}", orderToCarClaim.HandPerson.NullToEmpty(), orderToCarClaim.HandPersonPhone.NullToEmpty())));
                                orderModel.OrderField.Add(new OrderField("状态", "请稍侯，理赔呼叫中"));
                                break;
                            case Enumeration.OrderType.TalentDemand:

                                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("工种", orderToTalentDemand.WorkJob.GetCnName().NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("人数", orderToTalentDemand.Quantity.ToString()));
                                orderModel.OrderField.Add(new OrderField("状态", "核实需求中,请留意电话"));
                                break;
                            case Enumeration.OrderType.PosMachineServiceFee:

                                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(c => c.Id == m.Id).FirstOrDefault();
                                if (orderToServiceFee.Deposit > 0)
                                {
                                    orderModel.OrderField.Add(new OrderField("押金", orderToServiceFee.Deposit.ToF2Price()));
                                }

                                orderModel.OrderField.Add(new OrderField("流量费", orderToServiceFee.MobileTrafficFee.ToF2Price()));

                                break;
                            case Enumeration.OrderType.ApplyLossAssess:

                                var orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("保险公司", orderToApplyLossAssess.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("申请时间", orderToApplyLossAssess.ApplyTime.ToUnifiedFormatDateTime()));
                                orderModel.OrderField.Add(new OrderField("状态", "核实需求中,请留意电话"));

                                break;
                            case Enumeration.OrderType.LllegalQueryRecharge:

                                var orderToLllegalQueryRecharge = CurrentDb.OrderToLllegalQueryRecharge.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("充值", string.Format("{0}元", orderToLllegalQueryRecharge.Price.ToF2Price())));
                                orderModel.OrderField.Add(new OrderField("积分", orderToLllegalQueryRecharge.Score.ToString()));

                                break;
                            case Enumeration.OrderType.Credit:

                                var orderToCredit = CurrentDb.OrderToCredit.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("贷款类别", orderToCredit.CreditClass));
                                //orderModel.OrderField.Add(new OrderField("申请金额", orderToCredit.Creditline.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("状态", "核实需求中,请留意电话"));

                                break;
                            case Enumeration.OrderType.Insure:

                                var orderToInsurance = CurrentDb.OrderToInsurance.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToInsurance.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("产品名称", orderToInsurance.ProductSkuName));
                                orderModel.OrderField.Add(new OrderField("状态", "核实需求中,请留意电话"));

                                break;

                        }
                        #endregion

                        break;
                    case Enumeration.OrderStatus.Follow:

                        #region 跟进中

                        orderModel.Remarks = GetRemarks(m.Remarks, 30);//备注 订单备注

                        orderModel.StatusName = "跟进中";
                        orderModel.FollowStatus = m.FollowStatus;
                        switch (m.Type)
                        {
                            case Enumeration.OrderType.InsureForCarForInsure:

                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));

                                var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(c => c.OrderId == m.Id).ToList();
                                foreach (var c in orderToCarInsureOfferCompany)
                                {
                                    orderModel.OrderField.Add(new OrderField(c.InsuranceCompanyName, string.Format("{0}元", c.InsureTotalPrice.ToF2Price())));
                                }

                                break;
                            case Enumeration.OrderType.InsureForCarForClaim:

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToCarClaim.InsuranceCompanyName.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));

                                var merchant = CurrentDb.Merchant.Where(q => q.Id == orderToCarClaim.HandMerchantId).FirstOrDefault();
                                if (orderToCarClaim.HandMerchantType == Enumeration.HandMerchantType.Supply)
                                {
                                    orderModel.OrderField.Add(new OrderField("对接维修厂", merchant.YYZZ_Name.NullToEmpty()));
                                }
                                else
                                {
                                    orderModel.OrderField.Add(new OrderField("对接商户", merchant.YYZZ_Name.NullToEmpty()));
                                }

                                var followStatus = (Enumeration.OrderToCarClaimFollowStatus)orderToCarClaim.FollowStatus;

                                if (followStatus == Enumeration.OrderToCarClaimFollowStatus.VerifyEstimateAmount)
                                {
                                    orderModel.Remarks = "定损单已经上传，正在核实";

                                }

                                orderModel.OrderField.Add(new OrderField("进度", followStatus.GetCnName()));
                                break;
                            case Enumeration.OrderType.TalentDemand:

                                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("工种", orderToTalentDemand.WorkJob.GetCnName().NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("人数", orderToTalentDemand.Quantity.ToString()));
                                orderModel.OrderField.Add(new OrderField("状态", "核实需求中"));
                                break;
                        }

                        #endregion

                        break;
                    case Enumeration.OrderStatus.WaitPay:

                        #region 待支付
                        switch (m.Type)
                        {
                            case Enumeration.OrderType.InsureForCarForInsure:
                                orderModel.Remarks = "";

                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));

                                var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(c => c.OrderId == m.Id).ToList();
                                foreach (var c in orderToCarInsureOfferCompany)
                                {
                                    orderModel.OrderField.Add(new OrderField(c.InsuranceCompanyName, string.Format("{0}元", c.InsureTotalPrice.ToF2Price())));
                                }

                                break;
                            case Enumeration.OrderType.InsureForCarForClaim:
                                orderModel.Remarks = string.Format("应付金额:{0}元", m.Price.ToF2Price());//理赔 理赔金额

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToCarClaim.InsuranceCompanyName.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("对接人", string.Format("{0},{1}", orderToCarClaim.HandPerson.NullToEmpty(), orderToCarClaim.HandPersonPhone.NullToEmpty())));
                                orderModel.OrderField.Add(new OrderField("定损单总价", string.Format("{0}元", orderToCarClaim.EstimatePrice.ToF2Price())));

                                break;
                            case Enumeration.OrderType.PosMachineServiceFee:

                                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(c => c.Id == m.Id).FirstOrDefault();
                                if (orderToServiceFee.Deposit > 0)
                                {
                                    orderModel.OrderField.Add(new OrderField("押金", orderToServiceFee.Deposit.ToF2Price()));
                                }

                                orderModel.OrderField.Add(new OrderField("流量费", orderToServiceFee.MobileTrafficFee.ToF2Price()));


                                break;
                            case Enumeration.OrderType.LllegalQueryRecharge:

                                var orderToLllegalQueryRecharge = CurrentDb.OrderToLllegalQueryRecharge.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("充值", string.Format("{0}元", orderToLllegalQueryRecharge.Price.ToF2Price())));
                                orderModel.OrderField.Add(new OrderField("积分", orderToLllegalQueryRecharge.Score.ToString()));

                                break;
                        }
                        orderModel.StatusName = "待支付";

                        #endregion

                        break;
                    case Enumeration.OrderStatus.Completed:

                        #region 已完成
                        orderModel.Remarks = m.CompleteTime.ToUnifiedFormatDateTime();//备注完成时间
                        orderModel.StatusName = "已完成";

                        switch (m.Type)
                        {
                            case Enumeration.OrderType.InsureForCarForInsure:


                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("身份证号码", orderToCarInsure.CarOwnerIdNumber.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField(orderToCarInsure.InsuranceCompanyName, string.Format("{0}元", orderToCarInsure.Price.ToF2Price())));


                                break;
                            case Enumeration.OrderType.InsureForCarForClaim:
                                orderModel.Remarks = string.Format("合计:{0}", m.Price);//理赔 理赔金额

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("工时费", orderToCarClaim.WorkingHoursPrice.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("配件费", orderToCarClaim.AccessoriesPrice.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("合计", orderToCarClaim.Price.ToF2Price()));

                                break;
                            case Enumeration.OrderType.TalentDemand:

                                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("工种", orderToTalentDemand.WorkJob.GetCnName().NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("人数", orderToTalentDemand.Quantity.ToString()));

                                break;
                            case Enumeration.OrderType.PosMachineServiceFee:

                                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(c => c.Id == m.Id).FirstOrDefault();
                                if (orderToServiceFee.Deposit > 0)
                                {
                                    orderModel.OrderField.Add(new OrderField("押金", orderToServiceFee.Deposit.ToF2Price()));
                                }

                                orderModel.OrderField.Add(new OrderField("流量费", orderToServiceFee.MobileTrafficFee.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("到期时间", orderToServiceFee.ExpiryTime.ToUnifiedFormatDate()));
                                break;
                            case Enumeration.OrderType.ApplyLossAssess:

                                var orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("保险公司", orderToApplyLossAssess.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("申请时间", orderToApplyLossAssess.ApplyTime.ToUnifiedFormatDateTime()));

                                break;
                            case Enumeration.OrderType.LllegalQueryRecharge:

                                var orderToLllegalQueryRecharge = CurrentDb.OrderToLllegalQueryRecharge.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("充值", string.Format("{0}元", orderToLllegalQueryRecharge.Price.ToF2Price())));
                                orderModel.OrderField.Add(new OrderField("积分", orderToLllegalQueryRecharge.Score.ToString()));

                                break;
                            case Enumeration.OrderType.LllegalDealt:

                                var orderToLllegalDealt = CurrentDb.OrderToLllegalDealt.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToLllegalDealt.CarNo));
                                orderModel.OrderField.Add(new OrderField("违章", string.Format("{0}次", orderToLllegalDealt.SumCount)));
                                orderModel.OrderField.Add(new OrderField("扣分", orderToLllegalDealt.SumPoint.ToString()));
                                orderModel.OrderField.Add(new OrderField("罚款", orderToLllegalDealt.SumFine.ToF2Price()));

                                var orderToLllegalDealtDetails = CurrentDb.OrderToLllegalDealtDetails.Where(c => c.OrderId == m.Id).ToList();
                                var dealtcount = orderToLllegalDealtDetails.Where(c => c.Status == Enumeration.OrderToLllegalDealtDetailsStatus.InDealt).Count();
                                if (dealtcount > 0)
                                {
                                    orderModel.StatusName = "已付，处理中";
                                }
                                else {
                                    orderModel.StatusName = "完成";
                                }

                                break;
                            case Enumeration.OrderType.Credit:

                                var orderToCredit = CurrentDb.OrderToCredit.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("贷款类别", orderToCredit.CreditClass));
                                //orderModel.OrderField.Add(new OrderField("申请金额", orderToCredit.Creditline.ToF2Price()));


                                break;
                            case Enumeration.OrderType.Insure:

                                var orderToInsurance = CurrentDb.OrderToInsurance.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToInsurance.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("产品名称", orderToInsurance.ProductSkuName));

                                break;
                        }
                        #endregion

                        break;
                    case Enumeration.OrderStatus.Cancled:

                        #region
                        orderModel.Remarks = m.CancleTime.ToUnifiedFormatDateTime();//备注完成时间
                        orderModel.StatusName = "已取消";


                        switch (m.Type)
                        {
                            case Enumeration.OrderType.InsureForCarForInsure:

                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("身份证号码", orderToCarInsure.CarOwnerIdNumber.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));
                                break;
                            case Enumeration.OrderType.InsureForCarForClaim:

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToCarClaim.InsuranceCompanyName.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("对接人", orderToCarClaim.HandPerson.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));

                                break;
                            case Enumeration.OrderType.TalentDemand:
                                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("工种", orderToTalentDemand.WorkJob.GetCnName().NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("人数", orderToTalentDemand.Quantity.ToString()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));
                                break;
                            case Enumeration.OrderType.ApplyLossAssess:

                                var orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("保险公司", orderToApplyLossAssess.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("申请时间", orderToApplyLossAssess.ApplyTime.ToUnifiedFormatDateTime()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));
                                break;
                            case Enumeration.OrderType.LllegalQueryRecharge:

                                var orderToLllegalQueryRecharge = CurrentDb.OrderToLllegalQueryRecharge.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("充值", string.Format("{0}元", orderToLllegalQueryRecharge.Price.ToF2Price())));
                                orderModel.OrderField.Add(new OrderField("积分", orderToLllegalQueryRecharge.Score.ToString()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));

                                break;
                            case Enumeration.OrderType.Credit:

                                var orderToCredit = CurrentDb.OrderToCredit.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("贷款类别", orderToCredit.CreditClass));
                                //orderModel.OrderField.Add(new OrderField("申请金额", orderToCredit.Creditline.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));

                                break;
                            case Enumeration.OrderType.Insure:

                                var orderToInsurance = CurrentDb.OrderToInsurance.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToInsurance.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("产品名称", orderToInsurance.ProductSkuName));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));
                                break;
                        }

                        #endregion

                        break;
                }


                model.Add(orderModel);
            }



            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }

        [HttpGet]
        public APIResponse GetDetails(int userId, int merchantId, int posMachineId, int orderId, Enumeration.OrderType type)
        {
            APIResult result = null;
            switch (type)
            {
                case Enumeration.OrderType.InsureForCarForInsure:
                    #region 投保
                    OrderCarInsureDetailsModel orderCarInsureDetailsModel = new OrderCarInsureDetailsModel();
                    var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToCarInsure != null)
                    {

                        orderCarInsureDetailsModel.Id = orderToCarInsure.Id;
                        orderCarInsureDetailsModel.Sn = orderToCarInsure.Sn;

                        #region 报价公司
                        var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == orderToCarInsure.Id).ToList();
                        foreach (var m in orderToCarInsureOfferCompany)
                        {
                            OrderCarInsureOfferCompanyModel orderCarInsureOfferCompanyModel = new OrderCarInsureOfferCompanyModel();
                            orderCarInsureOfferCompanyModel.InsuranceOfferId = m.Id;
                            orderCarInsureOfferCompanyModel.InsuranceCompanyId = m.InsuranceCompanyId;
                            orderCarInsureOfferCompanyModel.InsuranceCompanyName = m.InsuranceCompanyName;


                            if (m.InsuranceCompanyId == orderToCarInsure.InsuranceCompanyId)
                            {
                                orderCarInsureOfferCompanyModel.IsCheck = true;
                            }

                            if (m.CompulsoryPrice != null)
                            {
                                orderCarInsureOfferCompanyModel.CompulsoryPrice = m.CompulsoryPrice.Value;
                            }

                            if (m.TravelTaxPrice != null)
                            {
                                orderCarInsureOfferCompanyModel.TravelTaxPrice = m.TravelTaxPrice.Value;
                            }

                            if (m.CommercialPrice != null)
                            {
                                orderCarInsureOfferCompanyModel.CompulsoryPrice = m.CommercialPrice.Value;
                            }

                            if (m.InsureTotalPrice != null)
                            {
                                orderCarInsureOfferCompanyModel.InsureTotalPrice = m.InsureTotalPrice.Value;
                            }

                            switch (orderToCarInsure.Status)
                            {
                                case Enumeration.OrderStatus.Submitted:
                                    orderCarInsureOfferCompanyModel.description = "正在报价中";
                                    orderCarInsureOfferCompanyModel.InsureImgUrl = m.InsuranceCompanyImgUrl;
                                    break;
                                case Enumeration.OrderStatus.Follow:
                                    orderCarInsureOfferCompanyModel.description = "正在报价中";
                                    orderCarInsureOfferCompanyModel.InsureImgUrl = m.InsuranceCompanyImgUrl;
                                    break;
                                case Enumeration.OrderStatus.WaitPay:
                                    orderCarInsureOfferCompanyModel.InsureImgUrl = m.InsureImgUrl;
                                    orderCarInsureOfferCompanyModel.description = string.Format(
                                        "交强险：{0}，车船税：{1}，商业险：{2}，合计：{3}",
                                        orderCarInsureOfferCompanyModel.CompulsoryPrice,
                                        orderCarInsureOfferCompanyModel.TravelTaxPrice,
                                          orderCarInsureOfferCompanyModel.CompulsoryPrice,
                                           orderCarInsureOfferCompanyModel.InsureTotalPrice
                                        );
                                    break;
                                case Enumeration.OrderStatus.Completed:
                                    orderCarInsureOfferCompanyModel.InsureImgUrl = m.InsureImgUrl;
                                    orderCarInsureOfferCompanyModel.description = string.Format(
                             "交强险：{0}，车船税：{1}，商业险：{2}，合计：{3}",
                             orderCarInsureOfferCompanyModel.CompulsoryPrice,
                             orderCarInsureOfferCompanyModel.TravelTaxPrice,
                               orderCarInsureOfferCompanyModel.CompulsoryPrice,
                                orderCarInsureOfferCompanyModel.InsureTotalPrice
                             );
                                    break;
                                case Enumeration.OrderStatus.Cancled:
                                    orderCarInsureOfferCompanyModel.InsureImgUrl = m.InsureImgUrl == null ? m.InsuranceCompanyImgUrl : m.InsureImgUrl;
                                    orderCarInsureOfferCompanyModel.description = "";
                                    break;
                            }

                            orderCarInsureDetailsModel.OfferCompany.Add(orderCarInsureOfferCompanyModel);
                        }
                        #endregion

                        #region 险种
                        var orderToCarInsureOfferKind = CurrentDb.OrderToCarInsureOfferKind.Where(m => m.OrderId == orderToCarInsure.Id).ToList();
                        var carKinds = CurrentDb.CarKind.ToList();
                        if (orderToCarInsureOfferKind != null)
                        {
                            foreach (var m in orderToCarInsureOfferKind)
                            {
                                var carKind = carKinds.Where(q => q.Id == m.KindId).FirstOrDefault();
                                if (carKind != null)
                                {
                                    OrderToCarInsureOfferKindModel orderToCarInsureOfferKindModel = new OrderToCarInsureOfferKindModel();
                                    orderToCarInsureOfferKindModel.Field = carKind.Name;
                                    orderToCarInsureOfferKindModel.Value = m.KindValue + carKind.InputUnit;
                                    orderCarInsureDetailsModel.OfferKind.Add(orderToCarInsureOfferKindModel);
                                }
                            }
                        }
                        #endregion

                        #region 车主
                        orderCarInsureDetailsModel.CarOwner = orderToCarInsure.CarOwner.NullToEmpty();
                        orderCarInsureDetailsModel.CarPlateNo = orderToCarInsure.CarPlateNo.NullToEmpty();
                        orderCarInsureDetailsModel.CarOwnerIdNumber = orderToCarInsure.CarOwnerIdNumber.NullToEmpty();
                        #endregion

                        orderCarInsureDetailsModel.InsuranceCompanyId = orderToCarInsure.InsuranceCompanyId;
                        orderCarInsureDetailsModel.InsuranceCompanyName = orderToCarInsure.InsuranceCompanyName;
                        orderCarInsureDetailsModel.InsureImgUrl = orderToCarInsure.InsureImgUrl;

                        orderCarInsureDetailsModel.Recipient = orderToCarInsure.Recipient.NullToEmpty();
                        orderCarInsureDetailsModel.RecipientAddress = orderToCarInsure.RecipientAddress.NullToEmpty();
                        orderCarInsureDetailsModel.RecipientPhoneNumber = orderToCarInsure.RecipientPhoneNumber.NullToEmpty();

                        orderCarInsureDetailsModel.CommercialPrice = orderToCarInsure.CommercialPrice.ToF2Price();
                        orderCarInsureDetailsModel.TravelTaxPrice = orderToCarInsure.TravelTaxPrice.ToF2Price();
                        orderCarInsureDetailsModel.CompulsoryPrice = orderToCarInsure.CompulsoryPrice.ToF2Price();
                        orderCarInsureDetailsModel.Price = orderToCarInsure.Price.ToF2Price();

                        #region 证件

                        if (orderToCarInsure.CZ_CL_XSZ_ImgUrl != null)
                        {
                            orderCarInsureDetailsModel.ZJ.Add(new ZjModel("行驶证", orderToCarInsure.CZ_CL_XSZ_ImgUrl));
                        }

                        if (orderToCarInsure.CZ_SFZ_ImgUrl != null)
                        {
                            orderCarInsureDetailsModel.ZJ.Add(new ZjModel("身份证", orderToCarInsure.CZ_SFZ_ImgUrl));
                        }

                        if (orderToCarInsure.CCSJM_WSZM_ImgUrl != null)
                        {
                            orderCarInsureDetailsModel.ZJ.Add(new ZjModel("车船税减免/完税证明", orderToCarInsure.CCSJM_WSZM_ImgUrl));
                        }

                        if (orderToCarInsure.YCZ_CLDJZ_ImgUrl != null)
                        {
                            orderCarInsureDetailsModel.ZJ.Add(new ZjModel("验车照/车辆登记证", orderToCarInsure.YCZ_CLDJZ_ImgUrl));
                        }

                        if (orderToCarInsure.ZJ1_ImgUrl != null)
                        {
                            orderCarInsureDetailsModel.ZJ.Add(new ZjModel("其它", orderToCarInsure.ZJ1_ImgUrl));
                        }

                        if (orderToCarInsure.ZJ2_ImgUrl != null)
                        {
                            orderCarInsureDetailsModel.ZJ.Add(new ZjModel("其它", orderToCarInsure.ZJ2_ImgUrl));
                        }

                        if (orderToCarInsure.ZJ3_ImgUrl != null)
                        {
                            orderCarInsureDetailsModel.ZJ.Add(new ZjModel("其它", orderToCarInsure.ZJ3_ImgUrl));
                        }

                        if (orderToCarInsure.ZJ4_ImgUrl != null)
                        {
                            orderCarInsureDetailsModel.ZJ.Add(new ZjModel("其它", orderToCarInsure.ZJ4_ImgUrl));
                        }
                        #endregion

                        orderCarInsureDetailsModel.SubmitTime = orderToCarInsure.SubmitTime.ToUnifiedFormatDateTime();
                        orderCarInsureDetailsModel.CompleteTime = orderToCarInsure.CompleteTime.ToUnifiedFormatDateTime();
                        orderCarInsureDetailsModel.PayTime = orderToCarInsure.PayTime.ToUnifiedFormatDateTime();
                        orderCarInsureDetailsModel.CancleTime = orderToCarInsure.CancleTime.ToUnifiedFormatDateTime();
                        orderCarInsureDetailsModel.Status = orderToCarInsure.Status;
                        orderCarInsureDetailsModel.StatusName = orderToCarInsure.Status.GetCnName();
                        orderCarInsureDetailsModel.FollowStatus = orderToCarInsure.FollowStatus;
                        orderCarInsureDetailsModel.Remarks = orderToCarInsure.Remarks.NullToEmpty();

                        orderCarInsureDetailsModel.RecipientAddressList.Add("地址1");
                        orderCarInsureDetailsModel.RecipientAddressList.Add("地址2");

                    }

                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderCarInsureDetailsModel };
                    return new APIResponse(result);
                #endregion
                case Enumeration.OrderType.InsureForCarForClaim:
                    #region 理赔
                    OrderCarClaimDetailsModel orderCarClaimDetailsModel = new OrderCarClaimDetailsModel();
                    var orderToCarEstimate = CurrentDb.OrderToCarClaim.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToCarEstimate != null)
                    {
                        orderCarClaimDetailsModel.Id = orderToCarEstimate.Id;
                        orderCarClaimDetailsModel.Sn = orderToCarEstimate.Sn;
                        orderCarClaimDetailsModel.CarOwner = orderToCarEstimate.CarOwner;
                        orderCarClaimDetailsModel.CarPlateNo = orderToCarEstimate.CarPlateNo;
                        orderCarClaimDetailsModel.RepairsType = orderToCarEstimate.RepairsType.GetCnName();
                        orderCarClaimDetailsModel.HandPerson = orderToCarEstimate.HandPerson;
                        orderCarClaimDetailsModel.HandPersonPhone = orderToCarEstimate.HandPersonPhone;
                        orderCarClaimDetailsModel.InsuranceCompanyName = orderToCarEstimate.InsuranceCompanyName;
                        orderCarClaimDetailsModel.InsuranceCompanyId = orderToCarEstimate.InsuranceCompanyId;
                        orderCarClaimDetailsModel.EstimateListImgUrl = orderToCarEstimate.EstimateListImgUrl;
                        orderCarClaimDetailsModel.SubmitTime = orderToCarEstimate.SubmitTime.ToUnifiedFormatDateTime();
                        orderCarClaimDetailsModel.CompleteTime = orderToCarEstimate.CompleteTime.ToUnifiedFormatDateTime();
                        orderCarClaimDetailsModel.PayTime = orderToCarEstimate.PayTime.ToUnifiedFormatDateTime();
                        orderCarClaimDetailsModel.CancleTime = orderToCarEstimate.CancleTime.ToUnifiedFormatDateTime();
                        orderCarClaimDetailsModel.AccessoriesPrice = orderToCarEstimate.AccessoriesPrice.ToF2Price();
                        orderCarClaimDetailsModel.WorkingHoursPrice = orderToCarEstimate.WorkingHoursPrice.ToF2Price();
                        orderCarClaimDetailsModel.EstimatePrice = orderToCarEstimate.EstimatePrice.ToF2Price();
                        orderCarClaimDetailsModel.Price = orderToCarEstimate.Price.ToF2Price();
                        orderCarClaimDetailsModel.Status = orderToCarEstimate.Status;
                        orderCarClaimDetailsModel.FollowStatus = orderToCarEstimate.FollowStatus;
                        orderCarClaimDetailsModel.StatusName = orderToCarEstimate.Status.GetCnName();
                        orderCarClaimDetailsModel.Remarks = orderToCarEstimate.Remarks;

                        if (orderToCarEstimate.HandMerchantId != null)
                        {
                            var handMerchant = CurrentDb.Merchant.Where(m => m.Id == orderToCarEstimate.HandMerchantId.Value).FirstOrDefault();
                            if (handMerchant != null)
                            {
                                MerchantModel merchantModel = new MerchantModel();
                                merchantModel.Id = handMerchant.Id;
                                merchantModel.Name = handMerchant.YYZZ_Name;
                                merchantModel.Contact = handMerchant.ContactName;
                                merchantModel.ContactAddress = handMerchant.YYZZ_Address;
                                merchantModel.ContactPhone = handMerchant.ContactPhoneNumber;

                                string headTitle = "";
                                if (orderToCarEstimate.HandMerchantType == Enumeration.HandMerchantType.Demand)
                                {
                                    headTitle = "对接商家";
                                }
                                else if (orderToCarEstimate.HandMerchantType == Enumeration.HandMerchantType.Supply)
                                {
                                    headTitle = "维修厂";
                                }

                                merchantModel.HeadTitle = headTitle;
                                orderCarClaimDetailsModel.HandMerchant = merchantModel;

                            }

                        }
                    }

                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderCarClaimDetailsModel };
                    return new APIResponse(result);
                #endregion
                case Enumeration.OrderType.PosMachineServiceFee:
                    #region  PosMachineDepositRent
                    OrderServiceFeeDetailsModel orderServiceFeeDetailsModel = new OrderServiceFeeDetailsModel();
                    var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToServiceFee != null)
                    {
                        orderServiceFeeDetailsModel.Id = orderToServiceFee.Id;
                        orderServiceFeeDetailsModel.Sn = orderToServiceFee.Sn;
                        orderServiceFeeDetailsModel.Status = orderToServiceFee.Status;
                        orderServiceFeeDetailsModel.StatusName = orderToServiceFee.Status.GetCnName();
                        orderServiceFeeDetailsModel.Remarks = orderToServiceFee.Remarks;
                        orderServiceFeeDetailsModel.SubmitTime = orderToServiceFee.SubmitTime.ToUnifiedFormatDateTime();
                        orderServiceFeeDetailsModel.CompleteTime = orderToServiceFee.CompleteTime.ToUnifiedFormatDateTime();
                        orderServiceFeeDetailsModel.PayTime = orderToServiceFee.PayTime.ToUnifiedFormatDateTime();
                        orderServiceFeeDetailsModel.CancleTime = orderToServiceFee.CancleTime.ToUnifiedFormatDateTime();
                        orderServiceFeeDetailsModel.Price = orderToServiceFee.Price.ToF2Price();

                        if (orderToServiceFee.Deposit > 0)
                        {
                            orderServiceFeeDetailsModel.Deposit = orderToServiceFee.Deposit.ToF2Price();
                        }

                        orderServiceFeeDetailsModel.MobileTrafficFee = orderToServiceFee.MobileTrafficFee.ToF2Price();
                        orderServiceFeeDetailsModel.ExpiryTime = orderToServiceFee.ExpiryTime.ToUnifiedFormatDate();

                        PrintDataModel printData = new PrintDataModel();

                        //printData.MerchantName = "好易联";
                        //printData.MerchantCode = "354422";
                        //printData.ProductName = orderToServiceFee.ProductName;
                        //printData.TradeType = "消费";
                        //printData.TradeNo = orderToServiceFee.Sn;
                        //printData.TradePayMethod = orderToServiceFee.PayWay.GetCnName();
                        //printData.TradeAmount = orderToServiceFee.Price.ToF2Price();
                        //printData.TradeDateTime = orderToServiceFee.PayTime.ToUnifiedFormatDateTime();
                        //printData.ServiceHotline = "4400000000";

                        orderServiceFeeDetailsModel.PrintData = printData;
                    }


                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderServiceFeeDetailsModel };
                    return new APIResponse(result);
                #endregion
                case Enumeration.OrderType.TalentDemand:
                    #region TalentDemand
                    OrderTalentDemandDetailsModel orderTalentDemandDetailsModel = new OrderTalentDemandDetailsModel();
                    var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToTalentDemand != null)
                    {
                        orderTalentDemandDetailsModel.Id = orderToTalentDemand.Id;
                        orderTalentDemandDetailsModel.Sn = orderToTalentDemand.Sn;
                        orderTalentDemandDetailsModel.SubmitTime = orderToTalentDemand.SubmitTime.ToUnifiedFormatDateTime();
                        orderTalentDemandDetailsModel.CompleteTime = orderToTalentDemand.CompleteTime.ToUnifiedFormatDateTime();
                        orderTalentDemandDetailsModel.PayTime = orderToTalentDemand.PayTime.ToUnifiedFormatDateTime();
                        orderTalentDemandDetailsModel.CancleTime = orderToTalentDemand.CancleTime.ToUnifiedFormatDateTime();
                        orderTalentDemandDetailsModel.Status = orderToTalentDemand.Status;
                        orderTalentDemandDetailsModel.StatusName = orderToTalentDemand.Status.GetCnName();
                        orderTalentDemandDetailsModel.FollowStatus = orderToTalentDemand.FollowStatus;
                        orderTalentDemandDetailsModel.Remarks = orderToTalentDemand.Remarks.NullToEmpty();

                        orderTalentDemandDetailsModel.Quantity = orderToTalentDemand.Quantity;
                        orderTalentDemandDetailsModel.WorkJob = orderToTalentDemand.WorkJob.GetCnName();
                        orderTalentDemandDetailsModel.UseStartTime = orderToTalentDemand.UseStartTime.ToUnifiedFormatDateTime();
                        orderTalentDemandDetailsModel.UseEndTime = orderToTalentDemand.UseEndTime.ToUnifiedFormatDateTime();
                    }

                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderTalentDemandDetailsModel };
                    return new APIResponse(result);
                #endregion
                case Enumeration.OrderType.ApplyLossAssess:
                    #region ApplyLossAssess
                    OrderApplyLossAssessDetailsModel orderApplyLossAssessDetailsModel = new OrderApplyLossAssessDetailsModel();
                    var orderApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderApplyLossAssess != null)
                    {
                        orderApplyLossAssessDetailsModel.Id = orderApplyLossAssess.Id;
                        orderApplyLossAssessDetailsModel.Sn = orderApplyLossAssess.Sn;
                        orderApplyLossAssessDetailsModel.SubmitTime = orderApplyLossAssess.SubmitTime.ToUnifiedFormatDateTime();
                        orderApplyLossAssessDetailsModel.CompleteTime = orderApplyLossAssess.CompleteTime.ToUnifiedFormatDateTime();
                        orderApplyLossAssessDetailsModel.PayTime = orderApplyLossAssess.PayTime.ToUnifiedFormatDateTime();
                        orderApplyLossAssessDetailsModel.CancleTime = orderApplyLossAssess.CancleTime.ToUnifiedFormatDateTime();
                        orderApplyLossAssessDetailsModel.Status = orderApplyLossAssess.Status;
                        orderApplyLossAssessDetailsModel.StatusName = orderApplyLossAssess.Status.GetCnName();
                        orderApplyLossAssessDetailsModel.FollowStatus = orderApplyLossAssess.FollowStatus;
                        orderApplyLossAssessDetailsModel.Remarks = orderApplyLossAssess.Remarks.NullToEmpty();

                        orderApplyLossAssessDetailsModel.InsuranceCompanyName = orderApplyLossAssess.InsuranceCompanyName;
                        orderApplyLossAssessDetailsModel.ApplyTime = orderApplyLossAssess.ApplyTime.ToUnifiedFormatDateTime();
                    }

                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderApplyLossAssessDetailsModel };
                    return new APIResponse(result);
                #endregion
                case Enumeration.OrderType.LllegalQueryRecharge:
                    #region LllegalQueryRecharge
                    OrderLllegalQueryRechargeDetailsModel orderLllegalQueryRechargeDetailsModel = new OrderLllegalQueryRechargeDetailsModel();
                    var orderToLllegalQueryRecharge = CurrentDb.OrderToLllegalQueryRecharge.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToLllegalQueryRecharge != null)
                    {
                        orderLllegalQueryRechargeDetailsModel.Id = orderToLllegalQueryRecharge.Id;
                        orderLllegalQueryRechargeDetailsModel.Sn = orderToLllegalQueryRecharge.Sn;
                        orderLllegalQueryRechargeDetailsModel.SubmitTime = orderToLllegalQueryRecharge.SubmitTime.ToUnifiedFormatDateTime();
                        orderLllegalQueryRechargeDetailsModel.CompleteTime = orderToLllegalQueryRecharge.CompleteTime.ToUnifiedFormatDateTime();
                        orderLllegalQueryRechargeDetailsModel.PayTime = orderToLllegalQueryRecharge.PayTime.ToUnifiedFormatDateTime();
                        orderLllegalQueryRechargeDetailsModel.CancleTime = orderToLllegalQueryRecharge.CancleTime.ToUnifiedFormatDateTime();
                        orderLllegalQueryRechargeDetailsModel.Status = orderToLllegalQueryRecharge.Status;
                        orderLllegalQueryRechargeDetailsModel.StatusName = orderToLllegalQueryRecharge.Status.GetCnName();
                        orderLllegalQueryRechargeDetailsModel.FollowStatus = orderToLllegalQueryRecharge.FollowStatus;
                        orderLllegalQueryRechargeDetailsModel.Remarks = orderToLllegalQueryRecharge.Remarks.NullToEmpty();

                        orderLllegalQueryRechargeDetailsModel.Score = orderToLllegalQueryRecharge.Score;
                    }

                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderLllegalQueryRechargeDetailsModel };
                    return new APIResponse(result);
                #endregion
                case Enumeration.OrderType.LllegalDealt:
                    #region LllegalDealt
                    OrderLllegalDealtDetailsModel orderLllegalDealtDetailsModel = new OrderLllegalDealtDetailsModel();
                    var orderToLllegalDealt = CurrentDb.OrderToLllegalDealt.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToLllegalDealt != null)
                    {
                        orderLllegalDealtDetailsModel.Id = orderToLllegalDealt.Id;
                        orderLllegalDealtDetailsModel.Sn = orderToLllegalDealt.Sn;
                        orderLllegalDealtDetailsModel.SubmitTime = orderToLllegalDealt.SubmitTime.ToUnifiedFormatDateTime();
                        orderLllegalDealtDetailsModel.CompleteTime = orderToLllegalDealt.CompleteTime.ToUnifiedFormatDateTime();
                        orderLllegalDealtDetailsModel.PayTime = orderToLllegalDealt.PayTime.ToUnifiedFormatDateTime();
                        orderLllegalDealtDetailsModel.CancleTime = orderToLllegalDealt.CancleTime.ToUnifiedFormatDateTime();

                        orderLllegalDealtDetailsModel.StatusName = orderToLllegalDealt.Status.GetCnName();
                        orderLllegalDealtDetailsModel.FollowStatus = orderToLllegalDealt.FollowStatus;
                        orderLllegalDealtDetailsModel.Remarks = orderToLllegalDealt.Remarks.NullToEmpty();

                        orderLllegalDealtDetailsModel.CarNo = orderToLllegalDealt.CarNo;
                        orderLllegalDealtDetailsModel.SumCount = orderToLllegalDealt.SumCount;
                        orderLllegalDealtDetailsModel.SumFine = orderToLllegalDealt.SumFine.ToF2Price();
                        orderLllegalDealtDetailsModel.SumPoint = orderToLllegalDealt.SumPoint.ToString();
                        orderLllegalDealtDetailsModel.SumLateFees = orderToLllegalDealt.SumLateFees.ToF2Price();
                        orderLllegalDealtDetailsModel.SumServiceFees = orderToLllegalDealt.SumServiceFees.ToF2Price();
                        orderLllegalDealtDetailsModel.Price = orderToLllegalDealt.Price.ToF2Price();
                        var orderToLllegalDealtDetails = CurrentDb.OrderToLllegalDealtDetails.Where(m => m.OrderId == orderId).ToList();

                        foreach (var item in orderToLllegalDealtDetails)
                        {
                            var record = new LllegalRecord();

                            record.bookNo = item.BookNo;
                            record.bookType = item.BookType;
                            record.bookTypeName = item.BookTypeName;
                            record.lllegalCode = item.LllegalCode;
                            record.cityCode = item.CityCode;
                            record.lllegalTime = item.LllegalTime;
                            record.point = (int)item.Point;
                            record.offerType = item.OfferType;
                            record.ofserTypeName = item.OfserTypeName;
                            record.fine = item.Fine;
                            record.serviceFee = item.ServiceFee;
                            record.late_fees = item.Late_fees;
                            record.content = item.Content;
                            record.lllegalDesc = item.LllegalDesc;
                            record.lllegalCity = item.LllegalCity;
                            record.address = item.Address;
                            record.status = item.Status.GetCnName();
                            orderLllegalDealtDetailsModel.LllegalRecord.Add(record);
                        }

                        var dealtcount = orderToLllegalDealtDetails.Where(m => m.Status == Enumeration.OrderToLllegalDealtDetailsStatus.InDealt).Count();
                        if (dealtcount > 0)
                        {
                            orderLllegalDealtDetailsModel.StatusName = "已付，处理中";
                        }
                        else {
                            orderLllegalDealtDetailsModel.StatusName = "完成";
                        }

                    }

                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderLllegalDealtDetailsModel };
                    return new APIResponse(result);
                #endregion
                case Enumeration.OrderType.Credit:
                    #region Credit
                    OrderCreditDetailsModel orderCreditDetailsModel = new OrderCreditDetailsModel();
                    var orderToCredit = CurrentDb.OrderToCredit.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToCredit != null)
                    {
                        orderCreditDetailsModel.Id = orderToCredit.Id;
                        orderCreditDetailsModel.Sn = orderToCredit.Sn;
                        orderCreditDetailsModel.SubmitTime = orderToCredit.SubmitTime.ToUnifiedFormatDateTime();
                        orderCreditDetailsModel.CompleteTime = orderToCredit.CompleteTime.ToUnifiedFormatDateTime();
                        orderCreditDetailsModel.PayTime = orderToCredit.PayTime.ToUnifiedFormatDateTime();
                        orderCreditDetailsModel.CancleTime = orderToCredit.CancleTime.ToUnifiedFormatDateTime();
                        orderCreditDetailsModel.Status = orderToCredit.Status;
                        orderCreditDetailsModel.StatusName = orderToCredit.Status.GetCnName();
                        orderCreditDetailsModel.FollowStatus = orderToCredit.FollowStatus;
                        orderCreditDetailsModel.Remarks = orderToCredit.Remarks.NullToEmpty();
                        orderCreditDetailsModel.CreditClass = orderToCredit.CreditClass;
                        orderCreditDetailsModel.Creditline = orderToCredit.Creditline;
                    }

                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderCreditDetailsModel };
                    return new APIResponse(result);
                #endregion
                case Enumeration.OrderType.Insure:
                    #region Credit
                    OrderInsuranceDetailsModel orderInsuranceDetailsModel = new OrderInsuranceDetailsModel();
                    var orderToInsurance = CurrentDb.OrderToInsurance.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToInsurance != null)
                    {
                        orderInsuranceDetailsModel.Id = orderToInsurance.Id;
                        orderInsuranceDetailsModel.Sn = orderToInsurance.Sn;
                        orderInsuranceDetailsModel.SubmitTime = orderToInsurance.SubmitTime.ToUnifiedFormatDateTime();
                        orderInsuranceDetailsModel.CompleteTime = orderToInsurance.CompleteTime.ToUnifiedFormatDateTime();
                        orderInsuranceDetailsModel.PayTime = orderToInsurance.PayTime.ToUnifiedFormatDateTime();
                        orderInsuranceDetailsModel.CancleTime = orderToInsurance.CancleTime.ToUnifiedFormatDateTime();
                        orderInsuranceDetailsModel.Status = orderToInsurance.Status;
                        orderInsuranceDetailsModel.StatusName = orderToInsurance.Status.GetCnName();
                        orderInsuranceDetailsModel.FollowStatus = orderToInsurance.FollowStatus;
                        orderInsuranceDetailsModel.Remarks = orderToInsurance.Remarks.NullToEmpty();
                        orderInsuranceDetailsModel.InsuranceCompanyName = orderToInsurance.InsuranceCompanyName;
                        orderInsuranceDetailsModel.ProductSkuName = orderToInsurance.ProductSkuName;
                        orderInsuranceDetailsModel.ProductTypeName = orderToInsurance.ProductType.GetCnName();
                    }

                    result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = orderInsuranceDetailsModel };
                    return new APIResponse(result);
                #endregion
                default:
                    result = new APIResult() { Result = ResultType.Failure, Code = ResultCode.Failure, Message = "未知产品类型" };
                    return new APIResponse(result);
            }
        }

        [HttpPost]
        public APIResponse Cancle(CancleModel model)
        {

            IResult result = BizFactory.Order.Cancle(model.UserId, model.OrderSn);
            return new APIResponse(result);
        }

        [HttpPost]
        public APIResponse PayConfirm(PayConfirmModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            IResult result = BizFactory.Pay.Confirm(model.UserId, model);

            return new APIResponse(result);
        }

        [HttpGet]
        public APIResponse PayResultQuery(int userId, int merchantId, int posMachineId, string orderSn)
        {
            PayQueryParams pms = new PayQueryParams();
            pms.UserId = userId;
            pms.MerchantId = merchantId;
            pms.PosMachineId = posMachineId;
            pms.OrderSn = orderSn;

            IResult result = BizFactory.Pay.ResultQuery(pms.UserId, pms);
            return new APIResponse(result);
        }

        [HttpPost]
        public APIResponse SubmitTalentDemand(SubmitTalentDemandModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToTalentDemand orderToTalentDemand = new OrderToTalentDemand();
            orderToTalentDemand.UserId = model.UserId;
            orderToTalentDemand.MerchantId = model.MerchantId;
            orderToTalentDemand.PosMachineId = model.PosMachineId;
            orderToTalentDemand.WorkJob = model.WorkJob;
            orderToTalentDemand.Quantity = model.Quantity;
            orderToTalentDemand.UseEndTime = model.UseEndTime;
            orderToTalentDemand.UseStartTime = model.UseStartTime;
            IResult result = BizFactory.OrderToTalentDemand.Submit(model.UserId, orderToTalentDemand);
            return new APIResponse(result);

        }

        [HttpPost]
        public APIResponse SubmitApplyLossAssess(SubmitApplyLossAssessModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToApplyLossAssess orderToApplyLossAssess = new OrderToApplyLossAssess();
            orderToApplyLossAssess.UserId = model.UserId;
            orderToApplyLossAssess.MerchantId = model.MerchantId;
            orderToApplyLossAssess.PosMachineId = model.PosMachineId;
            orderToApplyLossAssess.InsuranceCompanyId = model.InsuranceCompanyId;
            orderToApplyLossAssess.IsAgreeService = model.IsAgreeService;
            IResult result = BizFactory.OrderToApplyLossAssess.Submit(model.UserId, orderToApplyLossAssess);
            return new APIResponse(result);

        }

        [HttpPost]
        public APIResponse SubmitLllegalQueryScoreRecharge(SubmitLllegalQueryScoreRechargeModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToLllegalQueryRecharge orderToLllegalQueryRecharge = new OrderToLllegalQueryRecharge();
            orderToLllegalQueryRecharge.UserId = model.UserId;
            orderToLllegalQueryRecharge.MerchantId = model.MerchantId;
            orderToLllegalQueryRecharge.PosMachineId = model.PosMachineId;
            orderToLllegalQueryRecharge.Price = 7.5m;
            orderToLllegalQueryRecharge.Score = 50;
            orderToLllegalQueryRecharge.PriceVersion = "2018.05.07";

            IResult result = BizFactory.Order.SubmitLllegalQueryScoreRecharge(model.UserId, orderToLllegalQueryRecharge);
            return new APIResponse(result);

        }

        [HttpPost]
        public APIResponse SubmitCredit(SubmitCreditModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToCredit orderToCredit = new OrderToCredit();
            orderToCredit.UserId = model.UserId;
            orderToCredit.MerchantId = model.MerchantId;
            orderToCredit.Creditline = model.Creditline;
            orderToCredit.CreditClass = model.CreditClass;
            IResult result = BizFactory.OrderToCredit.Submit(model.UserId, orderToCredit);
            return new APIResponse(result);

        }

        [HttpPost]
        public APIResponse SubmitInsurance(SubmitInsuranceModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToInsurance orderToInsurance = new OrderToInsurance();
            orderToInsurance.UserId = model.UserId;
            orderToInsurance.MerchantId = model.MerchantId;
            orderToInsurance.PosMachineId = model.PosMachineId;
            orderToInsurance.ProductSkuId = model.ProductSkuId;
            IResult result = BizFactory.OrderToInsurance.Submit(model.UserId, orderToInsurance);
            return new APIResponse(result);

        }

        [HttpPost]
        public APIResponse QrCodeDownload(QrCodeDownloadParams pms)
        {
            IResult result = SdkFactory.MinShunPay.QrCodeDownload(pms.UserId, pms);
            return new APIResponse(result);
        }

        [HttpPost]
        public APIResponse PayResultNotify(OrderPayResultNotifyByAppLog model)
        {
            IResult result = BizFactory.Pay.ResultNotify(model.UserId, Enumeration.PayResultNotifyParty.AppNotify, model);

            return new APIResponse(result);
        }


        [HttpPost]
        public APIResponse GetPayTranSn(GetPayTranSnParams pms)
        {
            var orderPayTrans = new OrderPayTrans();

            orderPayTrans.UserId = pms.UserId;
            orderPayTrans.MerchantId = pms.MerchantId;
            orderPayTrans.PosMachineId = pms.PosMachineId;
            orderPayTrans.OrderId = pms.OrderId;
            orderPayTrans.OrderSn = pms.OrderSn;
            orderPayTrans.TransType = pms.TransType;
            orderPayTrans.CreateTime = DateTime.Now;
            orderPayTrans.Creator = pms.UserId;

            IResult result = BizFactory.Order.GetPayTranSn(pms.UserId, orderPayTrans);

            return new APIResponse(result);

        }
    }
}