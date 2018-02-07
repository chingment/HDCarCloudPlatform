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

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class OrderController : BaseApiController
    {
        [HttpGet]
        public APIResponse GetList(int userId, int merchantId, int posMachineId, int pageIndex, Enumeration.OrderStatus status)
        {
            var order = (from o in CurrentDb.Order
                         where o.MerchantId == merchantId

                         select new { o.Id, o.Sn, o.ProductType, o.Price, o.Status, o.Remarks, o.SubmitTime, o.CompleteTime, o.CancleTime, o.FollowStatus }
                         );


            if (status != Enumeration.OrderStatus.Unknow)
            {
                order = order.Where(m => m.Status == status);
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
                orderModel.Product = m.ProductType.GetCnName();
                orderModel.ProductType = m.ProductType;
                orderModel.Status = m.Status;
                orderModel.Price = m.Price;
                switch (m.Status)
                {
                    case Enumeration.OrderStatus.Submitted:

                        #region 已提交
                        orderModel.Remarks = m.SubmitTime.ToUnifiedFormatDateTime();//备注提交时间
                        orderModel.StatusName = "已提交";

                        switch (m.ProductType)
                        {
                            case Enumeration.ProductType.InsureForCarForInsure:
                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("身份证号码", orderToCarInsure.CarOwnerIdNumber.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("状态", "请稍侯，报价中"));

                                break;
                            case Enumeration.ProductType.InsureForCarForClaim:

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToCarClaim.InsuranceCompanyName.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("对接人", string.Format("{0},{1}", orderToCarClaim.HandPerson.NullToEmpty(), orderToCarClaim.HandPersonPhone.NullToEmpty())));
                                orderModel.OrderField.Add(new OrderField("状态", "请稍侯，理赔呼叫中"));
                                break;
                            case Enumeration.ProductType.TalentDemand:

                                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("工种", orderToTalentDemand.WorkJob.GetCnName().NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("人数", orderToTalentDemand.Quantity.ToString()));
                                orderModel.OrderField.Add(new OrderField("状态", "核实需求中"));
                                break;
                            case Enumeration.ProductType.PosMachineServiceFee:

                                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(c => c.Id == m.Id).FirstOrDefault();
                                if (orderToServiceFee.Deposit > 0)
                                {
                                    orderModel.OrderField.Add(new OrderField("押金", orderToServiceFee.Deposit.ToF2Price()));
                                }

                                orderModel.OrderField.Add(new OrderField("流量费", orderToServiceFee.MobileTrafficFee.ToF2Price()));

                                break;
                            case Enumeration.ProductType.ApplyLossAssess:

                                var orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("保险公司", orderToApplyLossAssess.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("申请时间", orderToApplyLossAssess.ApplyTime.ToUnifiedFormatDateTime()));
                                orderModel.OrderField.Add(new OrderField("状态", "核实需求中"));

                                break;
                        }
                        #endregion

                        break;
                    case Enumeration.OrderStatus.Follow:

                        #region 跟进中

                        orderModel.Remarks = GetRemarks(m.Remarks, 30);//备注 订单备注

                        orderModel.StatusName = "跟进中";
                        orderModel.FollowStatus = m.FollowStatus;
                        switch (m.ProductType)
                        {
                            case Enumeration.ProductType.InsureForCarForInsure:

                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));

                                var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(c => c.OrderId == m.Id).ToList();
                                foreach (var c in orderToCarInsureOfferCompany)
                                {
                                    orderModel.OrderField.Add(new OrderField(c.InsuranceCompanyName, string.Format("{0}元", c.InsureTotalPrice.ToF2Price())));
                                }

                                break;
                            case Enumeration.ProductType.InsureForCarForClaim:

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
                            case Enumeration.ProductType.TalentDemand:

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
                        switch (m.ProductType)
                        {
                            case Enumeration.ProductType.InsureForCarForInsure:
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
                            case Enumeration.ProductType.InsureForCarForClaim:
                                orderModel.Remarks = string.Format("应付金额:{0}元", m.Price.ToF2Price());//理赔 理赔金额

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToCarClaim.InsuranceCompanyName.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("对接人", string.Format("{0},{1}", orderToCarClaim.HandPerson.NullToEmpty(), orderToCarClaim.HandPersonPhone.NullToEmpty())));
                                orderModel.OrderField.Add(new OrderField("定损单总价", string.Format("{0}元", orderToCarClaim.EstimatePrice.ToF2Price())));

                                break;
                            case Enumeration.ProductType.PosMachineServiceFee:

                                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(c => c.Id == m.Id).FirstOrDefault();
                                if (orderToServiceFee.Deposit > 0)
                                {
                                    orderModel.OrderField.Add(new OrderField("押金", orderToServiceFee.Deposit.ToF2Price()));
                                }

                                orderModel.OrderField.Add(new OrderField("流量费", orderToServiceFee.MobileTrafficFee.ToF2Price()));


                                break;
                        }
                        orderModel.StatusName = "待支付";

                        #endregion

                        break;
                    case Enumeration.OrderStatus.Completed:

                        #region 已完成
                        orderModel.Remarks = m.CompleteTime.ToUnifiedFormatDateTime();//备注完成时间
                        orderModel.StatusName = "已完成";

                        switch (m.ProductType)
                        {
                            case Enumeration.ProductType.InsureForCarForInsure:


                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("身份证号码", orderToCarInsure.CarOwnerIdNumber.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField(orderToCarInsure.InsuranceCompanyName, string.Format("{0}元", orderToCarInsure.Price.ToF2Price())));


                                break;
                            case Enumeration.ProductType.InsureForCarForClaim:
                                orderModel.Remarks = string.Format("合计:{0}", m.Price);//理赔 理赔金额

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("工时费", orderToCarClaim.WorkingHoursPrice.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("配件费", orderToCarClaim.AccessoriesPrice.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("合计", orderToCarClaim.Price.ToF2Price()));

                                break;
                            case Enumeration.ProductType.TalentDemand:

                                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("工种", orderToTalentDemand.WorkJob.GetCnName().NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("人数", orderToTalentDemand.Quantity.ToString()));

                                break;
                            case Enumeration.ProductType.PosMachineServiceFee:

                                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(c => c.Id == m.Id).FirstOrDefault();
                                if (orderToServiceFee.Deposit > 0)
                                {
                                    orderModel.OrderField.Add(new OrderField("押金", orderToServiceFee.Deposit.ToF2Price()));
                                }

                                orderModel.OrderField.Add(new OrderField("流量费", orderToServiceFee.MobileTrafficFee.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("到期时间", orderToServiceFee.ExpiryTime.ToUnifiedFormatDate()));
                                break;
                            case Enumeration.ProductType.ApplyLossAssess:

                                var orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("保险公司", orderToApplyLossAssess.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("申请时间", orderToApplyLossAssess.ApplyTime.ToUnifiedFormatDateTime()));

                                break;
                        }
                        #endregion

                        break;
                    case Enumeration.OrderStatus.Cancled:

                        #region
                        orderModel.Remarks = m.CancleTime.ToUnifiedFormatDateTime();//备注完成时间
                        orderModel.StatusName = "已取消";


                        switch (m.ProductType)
                        {
                            case Enumeration.ProductType.InsureForCarForInsure:

                                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("身份证号码", orderToCarInsure.CarOwnerIdNumber.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));
                                break;
                            case Enumeration.ProductType.InsureForCarForClaim:

                                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("保险公司", orderToCarClaim.InsuranceCompanyName.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("对接人", orderToCarClaim.HandPerson.NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));

                                break;
                            case Enumeration.ProductType.TalentDemand:
                                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(c => c.Id == m.Id).FirstOrDefault();
                                orderModel.OrderField.Add(new OrderField("工种", orderToTalentDemand.WorkJob.GetCnName().NullToEmpty()));
                                orderModel.OrderField.Add(new OrderField("人数", orderToTalentDemand.Quantity.ToString()));
                                orderModel.OrderField.Add(new OrderField("取消原因", GetRemarks(m.Remarks, 20)));
                                break;
                            case Enumeration.ProductType.ApplyLossAssess:

                                var orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("保险公司", orderToApplyLossAssess.InsuranceCompanyName));
                                orderModel.OrderField.Add(new OrderField("申请时间", orderToApplyLossAssess.ApplyTime.ToUnifiedFormatDateTime()));
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
        public APIResponse GetDetails(int userId, int merchantId, int posMachineId, int orderId, Enumeration.ProductType productType)
        {
            if (productType == Enumeration.ProductType.InsureForCarForInsure)
            {
                #region 投保
                OrderCarInsureDetailsModel model = new OrderCarInsureDetailsModel();
                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderId).FirstOrDefault();
                if (orderToCarInsure != null)
                {

                    model.Id = orderToCarInsure.Id;
                    model.Sn = orderToCarInsure.Sn;

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

                        model.OfferCompany.Add(orderCarInsureOfferCompanyModel);
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
                                model.OfferKind.Add(orderToCarInsureOfferKindModel);
                            }
                        }
                    }
                    #endregion

                    #region 车主
                    model.CarOwner = orderToCarInsure.CarOwner.NullToEmpty();
                    model.CarPlateNo = orderToCarInsure.CarPlateNo.NullToEmpty();
                    model.CarOwnerIdNumber = orderToCarInsure.CarOwnerIdNumber.NullToEmpty();
                    #endregion

                    model.InsuranceCompanyId = orderToCarInsure.InsuranceCompanyId;
                    model.InsuranceCompanyName = orderToCarInsure.InsuranceCompanyName;
                    model.InsureImgUrl = orderToCarInsure.InsureImgUrl;

                    model.Recipient = orderToCarInsure.Recipient.NullToEmpty();
                    model.RecipientAddress = orderToCarInsure.RecipientAddress.NullToEmpty();
                    model.RecipientPhoneNumber = orderToCarInsure.RecipientPhoneNumber.NullToEmpty();

                    model.CommercialPrice = orderToCarInsure.CommercialPrice.ToF2Price();
                    model.TravelTaxPrice = orderToCarInsure.TravelTaxPrice.ToF2Price();
                    model.CompulsoryPrice = orderToCarInsure.CompulsoryPrice.ToF2Price();
                    model.Price = orderToCarInsure.Price.ToF2Price();

                    #region 证件

                    if (orderToCarInsure.CZ_CL_XSZ_ImgUrl != null)
                    {
                        model.ZJ.Add(new ZjModel("行驶证", orderToCarInsure.CZ_CL_XSZ_ImgUrl));
                    }

                    if (orderToCarInsure.CZ_SFZ_ImgUrl != null)
                    {
                        model.ZJ.Add(new ZjModel("身份证", orderToCarInsure.CZ_SFZ_ImgUrl));
                    }

                    if (orderToCarInsure.CCSJM_WSZM_ImgUrl != null)
                    {
                        model.ZJ.Add(new ZjModel("车船税减免/完税证明", orderToCarInsure.CCSJM_WSZM_ImgUrl));
                    }

                    if (orderToCarInsure.YCZ_CLDJZ_ImgUrl != null)
                    {
                        model.ZJ.Add(new ZjModel("验车照/车辆登记证", orderToCarInsure.YCZ_CLDJZ_ImgUrl));
                    }

                    if (orderToCarInsure.ZJ1_ImgUrl != null)
                    {
                        model.ZJ.Add(new ZjModel("其它", orderToCarInsure.ZJ1_ImgUrl));
                    }

                    if (orderToCarInsure.ZJ2_ImgUrl != null)
                    {
                        model.ZJ.Add(new ZjModel("其它", orderToCarInsure.ZJ2_ImgUrl));
                    }

                    if (orderToCarInsure.ZJ3_ImgUrl != null)
                    {
                        model.ZJ.Add(new ZjModel("其它", orderToCarInsure.ZJ3_ImgUrl));
                    }

                    if (orderToCarInsure.ZJ4_ImgUrl != null)
                    {
                        model.ZJ.Add(new ZjModel("其它", orderToCarInsure.ZJ4_ImgUrl));
                    }
                    #endregion

                    model.SubmitTime = orderToCarInsure.SubmitTime.ToUnifiedFormatDateTime(); 
                    model.CompleteTime = orderToCarInsure.CompleteTime.ToUnifiedFormatDateTime();
                    model.PayTime = orderToCarInsure.PayTime.ToUnifiedFormatDateTime();
                    model.CancleTime = orderToCarInsure.CancleTime.ToUnifiedFormatDateTime();
                    model.Status = orderToCarInsure.Status;
                    model.StatusName = orderToCarInsure.Status.GetCnName();
                    model.FollowStatus = orderToCarInsure.FollowStatus;
                    model.Remarks = orderToCarInsure.Remarks.NullToEmpty();

                    model.RecipientAddressList.Add("地址1");
                    model.RecipientAddressList.Add("地址2");

                }

                APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
                return new APIResponse(result);
                #endregion
            }
            else if (productType == Enumeration.ProductType.InsureForCarForClaim)
            {
                #region 理赔
                OrderCarClaimDetailsModel model = new OrderCarClaimDetailsModel();
                var orderToCarEstimate = CurrentDb.OrderToCarClaim.Where(m => m.Id == orderId).FirstOrDefault();
                if (orderToCarEstimate != null)
                {
                    model.Id = orderToCarEstimate.Id;
                    model.Sn = orderToCarEstimate.Sn;
                    model.CarOwner = orderToCarEstimate.CarOwner;
                    model.CarPlateNo = orderToCarEstimate.CarPlateNo;
                    model.RepairsType = orderToCarEstimate.RepairsType.GetCnName();
                    model.HandPerson = orderToCarEstimate.HandPerson;
                    model.HandPersonPhone = orderToCarEstimate.HandPersonPhone;
                    model.InsuranceCompanyName = orderToCarEstimate.InsuranceCompanyName;
                    model.InsuranceCompanyId = orderToCarEstimate.InsuranceCompanyId;
                    model.EstimateListImgUrl = orderToCarEstimate.EstimateListImgUrl;
                    model.SubmitTime = orderToCarEstimate.SubmitTime.ToUnifiedFormatDateTime();
                    model.CompleteTime = orderToCarEstimate.CompleteTime.ToUnifiedFormatDateTime();
                    model.PayTime = orderToCarEstimate.PayTime.ToUnifiedFormatDateTime();
                    model.CancleTime = orderToCarEstimate.CancleTime.ToUnifiedFormatDateTime();
                    model.AccessoriesPrice = orderToCarEstimate.AccessoriesPrice.ToF2Price();
                    model.WorkingHoursPrice = orderToCarEstimate.WorkingHoursPrice.ToF2Price();
                    model.EstimatePrice = orderToCarEstimate.EstimatePrice.ToF2Price();
                    model.Price = orderToCarEstimate.Price.ToF2Price();
                    model.Status = orderToCarEstimate.Status;
                    model.FollowStatus = orderToCarEstimate.FollowStatus;
                    model.StatusName = orderToCarEstimate.Status.GetCnName();
                    model.Remarks = orderToCarEstimate.Remarks;

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
                            model.HandMerchant = merchantModel;

                        }

                    }
                }

                APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
                return new APIResponse(result);
                #endregion
            }
            else if (productType == Enumeration.ProductType.PosMachineServiceFee)
            {
                #region  PosMachineDepositRent
                OrderServiceFeeDetailsModel model = new OrderServiceFeeDetailsModel();
                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.Id == orderId).FirstOrDefault();
                if (orderToServiceFee != null)
                {
                    model.Id = orderToServiceFee.Id;
                    model.Sn = orderToServiceFee.Sn;
                    model.Status = orderToServiceFee.Status;
                    model.StatusName = orderToServiceFee.Status.GetCnName();
                    model.Remarks = orderToServiceFee.Remarks;
                    model.SubmitTime = orderToServiceFee.SubmitTime.ToUnifiedFormatDateTime();
                    model.CompleteTime = orderToServiceFee.CompleteTime.ToUnifiedFormatDateTime();
                    model.PayTime = orderToServiceFee.PayTime.ToUnifiedFormatDateTime();
                    model.CancleTime = orderToServiceFee.CancleTime.ToUnifiedFormatDateTime();
                    model.Price = orderToServiceFee.Price.ToF2Price();

                    if (orderToServiceFee.Deposit > 0)
                    {
                        model.Deposit = orderToServiceFee.Deposit.ToF2Price();
                    }

                    model.MobileTrafficFee = orderToServiceFee.MobileTrafficFee.ToF2Price();
                    model.ExpiryTime = orderToServiceFee.ExpiryTime.ToUnifiedFormatDate();

                }
                APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
                return new APIResponse(result);
                #endregion 
            }
            else if (productType == Enumeration.ProductType.TalentDemand)
            {
                #region TalentDemand
                OrderTalentDemandDetailsModel model = new OrderTalentDemandDetailsModel();
                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(m => m.Id == orderId).FirstOrDefault();
                if (orderToTalentDemand != null)
                {
                    model.Id = orderToTalentDemand.Id;
                    model.Sn = orderToTalentDemand.Sn;
                    model.SubmitTime = orderToTalentDemand.SubmitTime.ToUnifiedFormatDateTime();
                    model.CompleteTime = orderToTalentDemand.CompleteTime.ToUnifiedFormatDateTime();
                    model.PayTime = orderToTalentDemand.PayTime.ToUnifiedFormatDateTime();
                    model.CancleTime = orderToTalentDemand.CancleTime.ToUnifiedFormatDateTime();
                    model.Status = orderToTalentDemand.Status;
                    model.StatusName = orderToTalentDemand.Status.GetCnName();
                    model.FollowStatus = orderToTalentDemand.FollowStatus;
                    model.Remarks = orderToTalentDemand.Remarks.NullToEmpty();

                    model.Quantity = orderToTalentDemand.Quantity;
                    model.WorkJob = orderToTalentDemand.WorkJob.GetCnName();
                    model.UseStartTime = orderToTalentDemand.UseStartTime.ToUnifiedFormatDateTime();
                    model.UseEndTime = orderToTalentDemand.UseEndTime.ToUnifiedFormatDateTime();
                }

                APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
                return new APIResponse(result);
                #endregion
            }
            else if (productType == Enumeration.ProductType.ApplyLossAssess)
            {
                #region ApplyLossAssess
                OrderApplyLossAssessDetailsModel model = new OrderApplyLossAssessDetailsModel();
                var orderApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(m => m.Id == orderId).FirstOrDefault();
                if (orderApplyLossAssess != null)
                {
                    model.Id = orderApplyLossAssess.Id;
                    model.Sn = orderApplyLossAssess.Sn;
                    model.SubmitTime = orderApplyLossAssess.SubmitTime.ToUnifiedFormatDateTime();
                    model.CompleteTime = orderApplyLossAssess.CompleteTime.ToUnifiedFormatDateTime();
                    model.PayTime = orderApplyLossAssess.PayTime.ToUnifiedFormatDateTime();
                    model.CancleTime = orderApplyLossAssess.CancleTime.ToUnifiedFormatDateTime();
                    model.Status = orderApplyLossAssess.Status;
                    model.StatusName = orderApplyLossAssess.Status.GetCnName();
                    model.FollowStatus = orderApplyLossAssess.FollowStatus;
                    model.Remarks = orderApplyLossAssess.Remarks.NullToEmpty();

                    model.InsuranceCompanyName = orderApplyLossAssess.InsuranceCompanyName;
                    model.ApplyTime = orderApplyLossAssess.ApplyTime.ToUnifiedFormatDateTime();
                }

                APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
                return new APIResponse(result);
                #endregion
            }
            else
            {
                APIResult result = new APIResult() { Result = ResultType.Failure, Code = ResultCode.Failure, Message = "未知产品类型" };
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
            //业务人员模拟数据
            if (model.MerchantId == this.SalesmanMerchantId)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能支付");
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
            //业务人员模拟数据
            if (model.MerchantId == this.SalesmanMerchantId)
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
            IResult result = BizFactory.Order.SubmitTalentDemand(model.UserId, orderToTalentDemand);
            return new APIResponse(result);

        }

        [HttpPost]
        public APIResponse SubmitApplyLossAssess(SubmitApplyLossAssessModel model)
        {
            //业务人员模拟数据
            if (model.MerchantId == this.SalesmanMerchantId)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToApplyLossAssess orderToApplyLossAssess = new OrderToApplyLossAssess();
            orderToApplyLossAssess.UserId = model.UserId;
            orderToApplyLossAssess.MerchantId = model.MerchantId;
            orderToApplyLossAssess.PosMachineId = model.PosMachineId;
            orderToApplyLossAssess.InsuranceCompanyId = model.InsuranceCompanyId;
            orderToApplyLossAssess.IsAgreeService = model.IsAgreeService;
            IResult result = BizFactory.Order.SubmitApplyLossAssess(model.UserId, orderToApplyLossAssess);
            return new APIResponse(result);

        }

        [HttpPost]
        public APIResponse QrCodeDownload(QrCodeDownloadParams pms)
        {
            IResult result = SdkFactory.MinShunPay.QrCodeDownload(pms.UserId, pms);
            return new APIResponse(result);
        }
    }
}