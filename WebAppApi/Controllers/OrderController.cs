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
    public class OrderController : BaseApiController
    {
        [HttpGet]
        public APIResponse GetList(int userId, int merchantId, int pageIndex, Enumeration.OrderStatus status)
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
                            case Enumeration.ProductType.PosMachineDepositRent:
                                orderModel.Remarks = string.Format("合计:{0}", m.Price);//押金和租金

                                var orderToDepositRent = CurrentDb.OrderToDepositRent.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("押金", orderToDepositRent.Deposit.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("租金", orderToDepositRent.RentTotal.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("续期", orderToDepositRent.RentMonths + "个月"));
                                orderModel.OrderField.Add(new OrderField("到期日期", orderToDepositRent.RentDueDate.ToUnifiedFormatDate()));


                                break;
                            case Enumeration.ProductType.PosMachineRent:
                                orderModel.Remarks = string.Format("合计:{0}", m.Price);//押金和租金

                                var orderToRent = CurrentDb.OrderToDepositRent.Where(c => c.Id == m.Id).FirstOrDefault();

                                orderModel.OrderField.Add(new OrderField("租金", orderToRent.RentTotal.ToF2Price()));
                                orderModel.OrderField.Add(new OrderField("续期", orderToRent.RentMonths + "个月"));
                                orderModel.OrderField.Add(new OrderField("到期日期", orderToRent.RentDueDate.ToUnifiedFormatDate()));


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
        public APIResponse GetDetails(int userId, int merchantId, int orderId, Enumeration.ProductType productType)
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

                    model.CommercialPrice = orderToCarInsure.CommercialPrice;
                    model.TravelTaxPrice = orderToCarInsure.TravelTaxPrice;
                    model.CompulsoryPrice = orderToCarInsure.CompulsoryPrice;
                    model.Price = orderToCarInsure.Price;

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

                    model.SubmitTime = orderToCarInsure.SubmitTime;
                    model.CompleteTime = orderToCarInsure.CompleteTime;
                    model.PayTime = orderToCarInsure.PayTime;
                    model.CancleTime = orderToCarInsure.CancleTime;
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
                    model.SubmitTime = orderToCarEstimate.SubmitTime;
                    model.CompleteTime = orderToCarEstimate.CompleteTime;
                    model.PayTime = orderToCarEstimate.PayTime;
                    model.CancleTime = orderToCarEstimate.CancleTime;
                    model.AccessoriesPrice = orderToCarEstimate.AccessoriesPrice;
                    model.WorkingHoursPrice = orderToCarEstimate.WorkingHoursPrice;
                    model.EstimatePrice = orderToCarEstimate.EstimatePrice;
                    model.Price = orderToCarEstimate.Price;
                    model.Status = orderToCarEstimate.Status;
                    model.FollowStatus = orderToCarEstimate.FollowStatus;
                    model.StatusName = orderToCarEstimate.Status.GetCnName();
                    model.Remark = orderToCarEstimate.Remarks;

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
                            if (orderToCarEstimate.HandMerchantType== Enumeration.HandMerchantType.Demand)
                            {
                                headTitle = "维修厂";
                            }
                            else if(orderToCarEstimate.HandMerchantType == Enumeration.HandMerchantType.Supply)
                            {
                                headTitle = "对接商家";
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
            else if (productType == Enumeration.ProductType.PosMachineDepositRent)
            {
                OrderPosDepositRentDetailsModel model = new OrderPosDepositRentDetailsModel();
                var orderToDepositRent = CurrentDb.OrderToDepositRent.Where(m => m.Id == orderId).FirstOrDefault();
                if (orderToDepositRent != null)
                {
                    model.Id = orderToDepositRent.Id;
                    model.Sn = orderToDepositRent.Sn;
                    model.Deposit = string.Format("{0}元", orderToDepositRent.Deposit);
                    model.RentDueDate = orderToDepositRent.RentDueDate.ToUnifiedFormatDate();
                    model.RentMonths = string.Format("{0}个月", orderToDepositRent.RentMonths);
                    model.RentTotal = string.Format("{0}元", orderToDepositRent.RentTotal);
                    model.Price = string.Format("{0}元", orderToDepositRent.Price);
                }
                APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
                return new APIResponse(result);
            }
            else if (productType == Enumeration.ProductType.PosMachineRent)
            {
                OrderPosRentDetailsModel model = new OrderPosRentDetailsModel();
                var orderToRent = CurrentDb.OrderToDepositRent.Where(m => m.Id == orderId).FirstOrDefault();
                if (orderToRent != null)
                {
                    model.Id = orderToRent.Id;
                    model.Sn = orderToRent.Sn;
                    model.RentDueDate = orderToRent.RentDueDate.ToUnifiedFormatDate();
                    model.RentMonths = string.Format("{0}个月", orderToRent.RentMonths);
                    model.RentTotal = string.Format("{0}元", orderToRent.RentTotal);
                    model.Price = string.Format("{0}元", orderToRent.Price);
                }

                APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
                return new APIResponse(result);
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

        [HttpPost]
        public APIResponse PayResultNotify(PayResultModel model)
        {
            IResult result = BizFactory.Pay.ResultNotify(model.UserId, ResultNotifyParty.App, model);

            return new APIResponse(result);
        }


    }
}