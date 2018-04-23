using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class OrderProvider : BaseProvider
    {
        public CustomJsonResult SubmitCarInsure(int operater, OrderToCarInsure orderToCarInsure, List<OrderToCarInsureOfferCompany> orderToCarInsureOfferCompany, List<OrderToCarInsureOfferKind> orderToCarInsureOfferKind)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                OrderToCarInsure order = new OrderToCarInsure();

                //用户信息
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == orderToCarInsure.UserId).FirstOrDefault();
                //商户信息
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();



                order.MerchantId = merchant.Id;
                order.PosMachineId = orderToCarInsure.PosMachineId;
                order.UserId = merchant.UserId;
                order.SalesmanId = merchant.SalesmanId ?? 0;
                order.AgentId = merchant.AgentId ?? 0;
                order.Type = Enumeration.OrderType.InsureForCarForInsure;
                order.TypeName = Enumeration.OrderType.InsureForCarForInsure.GetCnName();
                order.ClientRequire = orderToCarInsure.ClientRequire;
                order.InsuranceCompanyId = orderToCarInsure.InsuranceCompanyId;
                order.InsuranceCompanyName = orderToCarInsure.InsuranceCompanyName;
                order.CarOwner = orderToCarInsure.CarOwner;
                order.CarOwnerIdNumber = orderToCarInsure.CarOwnerIdNumber;
                order.CarOwnerAddress = orderToCarInsure.CarOwnerAddress;
                order.CarModel = orderToCarInsure.CarModel;
                order.CarOwner = orderToCarInsure.CarOwner;
                order.CarPlateNo = orderToCarInsure.CarPlateNo;
                order.CarEngineNo = orderToCarInsure.CarEngineNo;
                order.CarVin = orderToCarInsure.CarVin;
                order.CarVechicheType = orderToCarInsure.CarVechicheType;
                order.CarRegisterDate = orderToCarInsure.CarRegisterDate;
                order.CarIssueDate = orderToCarInsure.CarIssueDate;
                order.InsurePlanId = orderToCarInsure.InsurePlanId;
                order.CZ_CL_XSZ_ImgUrl = orderToCarInsure.CZ_CL_XSZ_ImgUrl;
                order.CZ_SFZ_ImgUrl = orderToCarInsure.CZ_SFZ_ImgUrl;
                order.YCZ_CLDJZ_ImgUrl = orderToCarInsure.YCZ_CLDJZ_ImgUrl;
                order.CCSJM_WSZM_ImgUrl = orderToCarInsure.CCSJM_WSZM_ImgUrl;
                order.Status = Enumeration.OrderStatus.Submitted;
                order.StartOfferTime = this.DateTime;
                order.SubmitTime = this.DateTime;
                order.CreateTime = this.DateTime;
                order.Creator = operater;
                order.IsLastYearNewCar = orderToCarInsure.IsLastYearNewCar;
                order.IsSameLastYear = orderToCarInsure.IsSameLastYear;
                order.AutoCancelByHour = 24;
                CurrentDb.OrderToCarInsure.Add(order);
                CurrentDb.SaveChanges();

                SnModel snModel = Sn.Build(SnType.OrderToCarInsure, order.Id);

                order.Sn = snModel.Sn;
                order.TradeSnByWechat = snModel.TradeSnByWechat;
                order.TradeSnByAlipay = snModel.TradeSnByAlipay;


                if (orderToCarInsureOfferCompany != null)
                {
                    foreach (var m in orderToCarInsureOfferCompany)
                    {
                        m.OrderId = order.Id;
                        m.CreateTime = this.DateTime;
                        m.Creator = operater;
                        CurrentDb.OrderToCarInsureOfferCompany.Add(m);
                    }
                }

                if (orderToCarInsureOfferKind != null)
                {
                    foreach (var m in orderToCarInsureOfferKind)
                    {
                        m.OrderId = order.Id;
                        m.CreateTime = this.DateTime;
                        m.Creator = operater;
                        CurrentDb.OrderToCarInsureOfferKind.Add(m);
                    }
                }


                BizProcessesAudit bizProcessesAudit = BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.OrderToCarInsure, order.Id, Enumeration.CarInsureOfferDealtStatus.WaitOffer);

                BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarInsureOfferDealtStep.Submit, bizProcessesAudit.Id, operater, orderToCarInsure.ClientRequire, "商户提交投保订单，等待报价", this.DateTime);


                order.BizProcessesAuditId = bizProcessesAudit.Id;

                CurrentDb.SaveChanges();
                ts.Complete();


                result = new CustomJsonResult(ResultType.Success, "提交成功");
            }

            return result;
        }

        public CustomJsonResult SubmitFollowInsure(int operater, OrderToCarInsure orderToCarInsure)
        {
            CustomJsonResult result = new CustomJsonResult();

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var l_orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderToCarInsure.Id).FirstOrDefault();
                    l_orderToCarInsure.ZJ1_ImgUrl = orderToCarInsure.ZJ1_ImgUrl;
                    l_orderToCarInsure.ZJ2_ImgUrl = orderToCarInsure.ZJ2_ImgUrl;
                    l_orderToCarInsure.ZJ3_ImgUrl = orderToCarInsure.ZJ3_ImgUrl;
                    l_orderToCarInsure.ZJ4_ImgUrl = orderToCarInsure.ZJ4_ImgUrl;
                    l_orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.Submitted;
                    l_orderToCarInsure.LastUpdateTime = this.DateTime;
                    l_orderToCarInsure.Mender = operater;



                    BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarInsureOfferDealtStep.Fllow, l_orderToCarInsure.BizProcessesAuditId, operater, orderToCarInsure.ClientRequire, "商户再次提交投保订单", this.DateTime);

                    BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(operater, l_orderToCarInsure.BizProcessesAuditId, Enumeration.CarInsureOfferDealtStatus.WaitOffer);


                    CurrentDb.SaveChanges();
                    ts.Complete();
                    result = new CustomJsonResult(ResultType.Success, "提交成功");
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("出错" + orderToCarInsure.Id, ex);

                return result;
            }



        }

        public CustomJsonResult SubmitCarInsureOffer(int operater, Enumeration.OperateType operate, OrderToCarInsure orderToCarInsure, List<OrderToCarInsureOfferCompany> orderToCarInsureOfferCompany, List<OrderToCarInsureOfferKind> orderToCarInsureOfferKind, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {


                var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAudit.CurrentDetails.BizProcessesAuditId && (m.Status == (int)Enumeration.CarInsureOfferDealtStatus.WaitOffer || m.Status == (int)Enumeration.CarInsureOfferDealtStatus.InOffer)).FirstOrDefault();

                if (l_bizProcessesAudit == null)
                {
                    return new CustomJsonResult(ResultType.Success, "该订单已经处理完成");
                }

                if (l_bizProcessesAudit.Auditor != null)
                {
                    if (l_bizProcessesAudit.Auditor.Value != operater)
                    {
                        return new CustomJsonResult(ResultType.Failure, "该订单其他用户正在处理");
                    }
                }

                var l_orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderToCarInsure.Id).FirstOrDefault();

                if (l_orderToCarInsure.Status == Enumeration.OrderStatus.Cancled)
                {
                    return new CustomJsonResult(ResultType.Failure, "该订单已经被取消");
                }



                l_orderToCarInsure.CarOwner = orderToCarInsure.CarOwner;
                l_orderToCarInsure.CarOwnerIdNumber = orderToCarInsure.CarOwnerIdNumber;
                l_orderToCarInsure.CarOwnerAddress = orderToCarInsure.CarOwnerAddress;
                l_orderToCarInsure.CarPlateNo = orderToCarInsure.CarPlateNo;
                l_orderToCarInsure.CarRegisterDate = orderToCarInsure.CarRegisterDate;
                l_orderToCarInsure.CarIssueDate = orderToCarInsure.CarIssueDate;
                l_orderToCarInsure.CarSeat = orderToCarInsure.CarSeat;
                l_orderToCarInsure.CarUserCharacter = orderToCarInsure.CarUserCharacter;
                l_orderToCarInsure.CarVin = orderToCarInsure.CarVin;
                l_orderToCarInsure.CarVechicheType = orderToCarInsure.CarVechicheType;
                l_orderToCarInsure.CarModel = orderToCarInsure.CarModel;
                l_orderToCarInsure.CarModelName = orderToCarInsure.CarModelName;
                l_orderToCarInsure.CarEngineNo = orderToCarInsure.CarEngineNo;
                l_orderToCarInsure.CarPurchasePrice = orderToCarInsure.CarPurchasePrice;
                l_orderToCarInsure.IsCarDamage = orderToCarInsure.IsCarDamage;

                l_orderToCarInsure.PeriodStart = orderToCarInsure.PeriodStart;
                if (orderToCarInsure.PeriodStart != null)
                {
                    l_orderToCarInsure.PeriodEnd = orderToCarInsure.PeriodStart.Value.AddYears(1);
                }
                l_orderToCarInsure.Remarks = orderToCarInsure.Remarks;
                l_orderToCarInsure.LastUpdateTime = this.DateTime;
                l_orderToCarInsure.Mender = operater;



                bizProcessesAudit.CurrentDetails.AuditComments = orderToCarInsure.Remarks;




                foreach (var m in orderToCarInsureOfferCompany)
                {
                    var l_orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(q => q.Id == m.Id).FirstOrDefault();
                    if (l_orderToCarInsureOfferCompany != null)
                    {
                        l_orderToCarInsureOfferCompany.InsuranceOrderId = m.InsuranceOrderId;
                        l_orderToCarInsureOfferCompany.InsureImgUrl = m.InsureImgUrl;
                        l_orderToCarInsureOfferCompany.CommercialPrice = m.CommercialPrice;
                        l_orderToCarInsureOfferCompany.CompulsoryPrice = m.CompulsoryPrice;
                        l_orderToCarInsureOfferCompany.TravelTaxPrice = m.TravelTaxPrice;
                        l_orderToCarInsureOfferCompany.PayUrl = m.PayUrl;

                        decimal insureTotalPrice = 0;
                        if (l_orderToCarInsureOfferCompany.CommercialPrice != null)
                        {
                            insureTotalPrice += l_orderToCarInsureOfferCompany.CommercialPrice.Value;
                        }

                        if (l_orderToCarInsureOfferCompany.CompulsoryPrice != null)
                        {
                            insureTotalPrice += l_orderToCarInsureOfferCompany.CompulsoryPrice.Value;
                        }

                        if (l_orderToCarInsureOfferCompany.TravelTaxPrice != null)
                        {
                            insureTotalPrice += l_orderToCarInsureOfferCompany.TravelTaxPrice.Value;
                        }

                        l_orderToCarInsureOfferCompany.InsureTotalPrice = insureTotalPrice;

                        l_orderToCarInsureOfferCompany.LastUpdateTime = this.DateTime;
                        l_orderToCarInsureOfferCompany.Mender = operater;
                    }

                }

                var client = CurrentDb.SysUser.Where(m => m.Id == l_orderToCarInsure.UserId).FirstOrDefault();

                switch (operate)
                {
                    case Enumeration.OperateType.Save:

                        result = new CustomJsonResult(ResultType.Success, "保存成功");

                        BizFactory.BizProcessesAudit.ChangeAuditDetailsAuditComments(operater, bizProcessesAudit.CurrentDetails.Id, bizProcessesAudit.CurrentDetails.AuditComments, null);

                        break;
                    case Enumeration.OperateType.Reject:

                        l_orderToCarInsure.Status = Enumeration.OrderStatus.Follow;
                        l_orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitSubmit;

                        BizFactory.BizProcessesAudit.ChangeAuditDetailsAuditComments(operater, bizProcessesAudit.CurrentDetails.Id, bizProcessesAudit.CurrentDetails.AuditComments, "后台人员转给商户跟进", this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, Enumeration.CarInsureOfferDealtStatus.ClientFllow, "商户正在跟进");

                        BizFactory.Sms.SendCarInsureOfferFollow(client.Id, client.PhoneNumber, l_orderToCarInsure.Sn, l_orderToCarInsure.CarOwner, l_orderToCarInsure.CarPlateNo);

                        result = new CustomJsonResult(ResultType.Success, "转给客户跟进成功");

                        break;
                    case Enumeration.OperateType.Cancle:

                        l_orderToCarInsure.CancleTime = this.DateTime;
                        l_orderToCarInsure.EndOfferTime = this.DateTime;
                        l_orderToCarInsure.Status = Enumeration.OrderStatus.Cancled;


                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.CarInsureOfferDealtStep.Cancle, l_bizProcessesAudit.Id, operater, bizProcessesAudit.CurrentDetails.AuditComments, "后台人员撤销订单", this.DateTime);


                        BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(operater, l_bizProcessesAudit.Id, Enumeration.CarInsureOfferDealtStatus.StaffCancle);

                        result = new CustomJsonResult(ResultType.Success, "撤销成功");

                        break;
                    case Enumeration.OperateType.Submit:

                        l_orderToCarInsure.EndOfferTime = this.DateTime;
                        l_orderToCarInsure.Status = Enumeration.OrderStatus.WaitPay;
                        l_orderToCarInsure.AutoCancelByHour = orderToCarInsure.AutoCancelByHour;

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.CarInsureOfferDealtStep.Offer, l_bizProcessesAudit.Id, operater, bizProcessesAudit.CurrentDetails.AuditComments, "报价完成", this.DateTime);
                        BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(operater, l_bizProcessesAudit.Id, Enumeration.CarInsureOfferDealtStatus.OfferComplete);

                        BizFactory.Sms.SendCarInsureOfferComplete(client.Id, client.PhoneNumber, l_orderToCarInsure.Sn, l_orderToCarInsure.CarOwner, l_orderToCarInsure.CarPlateNo);

                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;

                }


                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;

        }

        public CustomJsonResult SubmitClaim(int operater, int userId, OrderToCarClaim orderToCarClaim)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                //用户信息
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == userId).FirstOrDefault();
                //商户信息
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();

                var insuranceCompany = CurrentDb.Company.Where(m => m.Id == orderToCarClaim.InsuranceCompanyId).FirstOrDefault();


                orderToCarClaim.SalesmanId = merchant.SalesmanId ?? 0;
                orderToCarClaim.AgentId = merchant.AgentId ?? 0;

                orderToCarClaim.Type = Enumeration.OrderType.InsureForCarForClaim;
                orderToCarClaim.TypeName = Enumeration.OrderType.InsureForCarForClaim.GetCnName();
                orderToCarClaim.MerchantId = merchant.Id;
                orderToCarClaim.PosMachineId = orderToCarClaim.PosMachineId;
                orderToCarClaim.UserId = merchant.UserId;
                orderToCarClaim.InsuranceCompanyName = insuranceCompany.Name;
                orderToCarClaim.Status = Enumeration.OrderStatus.Submitted;
                orderToCarClaim.SubmitTime = this.DateTime;
                orderToCarClaim.CreateTime = this.DateTime;
                orderToCarClaim.Creator = operater;
                CurrentDb.OrderToCarClaim.Add(orderToCarClaim);
                CurrentDb.SaveChanges();

                SnModel snModel = Sn.Build(SnType.OrderToCarClaim, orderToCarClaim.Id);

                orderToCarClaim.Sn = snModel.Sn;
                orderToCarClaim.TradeSnByWechat = snModel.TradeSnByWechat;
                orderToCarClaim.TradeSnByAlipay = snModel.TradeSnByAlipay;


                //状态改为待核实
                BizProcessesAudit bizProcessesAudit = BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.OrderToCarClaim, orderToCarClaim.Id, Enumeration.CarClaimDealtStatus.WaitVerifyOrder);
                BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarClaimDealtStep.Submit, bizProcessesAudit.Id, operater, orderToCarClaim.ClientRequire, "商户提交理赔需求", this.DateTime);

                orderToCarClaim.BizProcessesAuditId = bizProcessesAudit.Id;

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "提交成功");
            }


            return result;
        }

        public CustomJsonResult SubmitEstimateList(int operater, int userId, int orderId, string estimateListImgUrl)
        {
            CustomJsonResult result = new CustomJsonResult();

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var estimateOrderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Id == orderId).FirstOrDefault();

                    estimateOrderToCarClaim.EstimateListImgUrl = estimateListImgUrl;
                    estimateOrderToCarClaim.FollowStatus = (int)Enumeration.OrderToCarClaimFollowStatus.VerifyEstimateAmount;
                    // estimateOrderToCarClaim.Remarks = "定损单已经上传，正在核实";
                    estimateOrderToCarClaim.LastUpdateTime = this.DateTime;
                    estimateOrderToCarClaim.Mender = operater;


                    var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Id == estimateOrderToCarClaim.PId).FirstOrDefault();
                    orderToCarClaim.EstimateListImgUrl = estimateListImgUrl;
                    orderToCarClaim.FollowStatus = (int)Enumeration.OrderToCarClaimFollowStatus.VerifyEstimateAmount;
                    //orderToCarClaim.Remarks = "定损单已经上传，正在核实";
                    orderToCarClaim.LastUpdateTime = this.DateTime;
                    orderToCarClaim.Mender = operater;

                    var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.AduitReferenceId == orderToCarClaim.Id && m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarClaim).FirstOrDefault();

                    BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarClaimDealtStep.UploadEstimateListImg, bizProcessesAudit.Id, operater, "定损单已经上传，正在核实", "商户提交定损单", this.DateTime);

                    BizFactory.BizProcessesAudit.ChangeCarClaimDealtStatus(operater, bizProcessesAudit.Id, Enumeration.CarClaimDealtStatus.WaitVerifyAmount);

                    orderToCarClaim.BizProcessesAuditId = bizProcessesAudit.Id;

                    CurrentDb.SaveChanges();
                    ts.Complete();

                    result = new CustomJsonResult(ResultType.Success, "提交成功");
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("出错" + orderId, ex);

                return result;
            }

        }

        public CustomJsonResult VerifyClaimOrder(int operater, Enumeration.OperateType operate, OrderToCarClaim orderToCarClaim, int estimateMerchantId, string estimateMerchantRemarks, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAudit.CurrentDetails.BizProcessesAuditId && (m.Status == (int)Enumeration.CarClaimDealtStatus.WaitVerifyOrder || m.Status == (int)Enumeration.CarClaimDealtStatus.InVerifyOrder)).FirstOrDefault();

                if (bizProcessesAudit == null)
                {
                    return new CustomJsonResult(ResultType.Success, "该订单已经处理完成");
                }

                if (bizProcessesAudit.Auditor != null)
                {
                    if (bizProcessesAudit.Auditor.Value != operater)
                    {
                        return new CustomJsonResult(ResultType.Failure, "该订单其他用户正在处理");
                    }
                }


                var l_orderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Id == orderToCarClaim.Id).FirstOrDefault();

                l_orderToCarClaim.HandMerchantId = estimateMerchantId;
                l_orderToCarClaim.HandMerchantType = Enumeration.HandMerchantType.Supply;

                l_orderToCarClaim.Remarks = orderToCarClaim.Remarks;

                bizProcessesAudit.CurrentDetails.AuditComments = orderToCarClaim.Remarks;

                switch (operate)
                {
                    case Enumeration.OperateType.Save:

                        result = new CustomJsonResult(ResultType.Success, "保存成功");

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.CarClaimDealtStep.VerifyOrder, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, operater, bizProcessesAudit.CurrentDetails.AuditComments, null);

                        break;
                    case Enumeration.OperateType.Cancle:

                        l_orderToCarClaim.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToCarClaim.CancleTime = this.DateTime;

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.CarClaimDealtStep.VerifyOrder, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, operater, bizProcessesAudit.CurrentDetails.AuditComments, "后台人员撤销订单", this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, Enumeration.CarInsureOfferDealtStatus.StaffCancle);

                        result = new CustomJsonResult(ResultType.Success, "撤销成功");

                        break;
                    case Enumeration.OperateType.Submit:

                        l_orderToCarClaim.Status = Enumeration.OrderStatus.Follow;

                        l_orderToCarClaim.FollowStatus = (int)Enumeration.OrderToCarClaimFollowStatus.WaitEstimate;


                        var merchant = CurrentDb.Merchant.Where(m => m.Id == l_orderToCarClaim.HandMerchantId).FirstOrDefault();

                        var estimateOrderToCarClaim = new OrderToCarClaim();
                        estimateOrderToCarClaim.RepairsType = l_orderToCarClaim.RepairsType;
                        estimateOrderToCarClaim.SalesmanId = merchant.SalesmanId ?? 0;
                        estimateOrderToCarClaim.AgentId = merchant.AgentId ?? 0;
                        estimateOrderToCarClaim.MerchantId = merchant.Id;
                        estimateOrderToCarClaim.PosMachineId = l_orderToCarClaim.PosMachineId;
                        estimateOrderToCarClaim.UserId = merchant.UserId;
                        estimateOrderToCarClaim.HandPerson = l_orderToCarClaim.HandPerson;
                        estimateOrderToCarClaim.HandPersonPhone = l_orderToCarClaim.HandPersonPhone;
                        estimateOrderToCarClaim.InsuranceCompanyId = l_orderToCarClaim.InsuranceCompanyId;
                        estimateOrderToCarClaim.InsuranceCompanyName = l_orderToCarClaim.InsuranceCompanyName;
                        estimateOrderToCarClaim.CarPlateNo = l_orderToCarClaim.CarPlateNo;
                        estimateOrderToCarClaim.Status = Enumeration.OrderStatus.Follow;
                        estimateOrderToCarClaim.FollowStatus = (int)Enumeration.OrderToCarClaimFollowStatus.WaitUploadEstimateList;
                        estimateOrderToCarClaim.SubmitTime = this.DateTime;
                        estimateOrderToCarClaim.Creator = operater;
                        estimateOrderToCarClaim.CreateTime = this.DateTime;

                        estimateOrderToCarClaim.HandMerchantId = l_orderToCarClaim.MerchantId;
                        estimateOrderToCarClaim.HandMerchantType = Enumeration.HandMerchantType.Demand;


                        estimateOrderToCarClaim.Remarks = estimateMerchantRemarks;//告知维修厂备注


                        estimateOrderToCarClaim.Type = l_orderToCarClaim.Type;
                        estimateOrderToCarClaim.TypeName = l_orderToCarClaim.TypeName;
                        estimateOrderToCarClaim.PId = l_orderToCarClaim.Id;
                        estimateOrderToCarClaim.ClientRequire = l_orderToCarClaim.ClientRequire;


                        estimateOrderToCarClaim.HandOrderId = l_orderToCarClaim.Id;

                        CurrentDb.OrderToCarClaim.Add(estimateOrderToCarClaim);
                        CurrentDb.SaveChanges();

                        SnModel snModel = Sn.Build(SnType.OrderToCarClaim, estimateOrderToCarClaim.Id);

                        estimateOrderToCarClaim.Sn = snModel.Sn;
                        estimateOrderToCarClaim.TradeSnByWechat = snModel.TradeSnByWechat;
                        estimateOrderToCarClaim.TradeSnByAlipay = snModel.TradeSnByAlipay;



                        l_orderToCarClaim.HandOrderId = estimateOrderToCarClaim.Id;

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.CarClaimDealtStep.VerifyOrder, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, operater, bizProcessesAudit.CurrentDetails.AuditComments, "后台人员派单完成", this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeCarClaimDealtStatus(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, Enumeration.CarClaimDealtStatus.FllowUploadEstimateListImg, "等待商户上传定损单");

                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;

                }



                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;

        }

        public CustomJsonResult VerifyClaimAmount(int operater, Enumeration.OperateType operate, OrderToCarClaim orderToCarClaim, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                //var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAudit.CurrentDetails.BizProcessesAuditId && (m.Status == (int)Enumeration.CarInsureOfferDealtStatus.WaitOffer || m.Status == (int)Enumeration.CarInsureOfferDealtStatus.InOffer)).FirstOrDefault();

                //if (bizProcessesAudit == null)
                //{
                //    return new CustomJsonResult(ResultType.Success, "该订单已经处理完成");
                //}

                //if (bizProcessesAudit.Auditor != null)
                //{
                //    if (bizProcessesAudit.Auditor.Value != operater)
                //    {
                //        return new CustomJsonResult(ResultType.Failure, "该订单其他用户正在处理");
                //    }
                //}


                var l_orderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Id == orderToCarClaim.Id).FirstOrDefault();
                l_orderToCarClaim.WorkingHoursPrice = orderToCarClaim.WorkingHoursPrice;
                l_orderToCarClaim.AccessoriesPrice = orderToCarClaim.AccessoriesPrice;
                l_orderToCarClaim.EstimatePrice = orderToCarClaim.WorkingHoursPrice + orderToCarClaim.AccessoriesPrice;
                l_orderToCarClaim.Remarks = orderToCarClaim.Remarks;


                //bizProcessesAudit.CurrentDetails.AuditComments = orderToCarClaim.Remarks;
                var estimateOrderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.PId == orderToCarClaim.Id).FirstOrDefault();
                switch (operate)
                {
                    case Enumeration.OperateType.Save:

                        result = new CustomJsonResult(ResultType.Success, "保存成功");

                        BizFactory.BizProcessesAudit.ChangeAuditDetailsAuditComments(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, bizProcessesAudit.CurrentDetails.AuditComments, null);

                        break;
                    case Enumeration.OperateType.Cancle:

                        l_orderToCarClaim.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToCarClaim.CancleTime = this.DateTime;

                        estimateOrderToCarClaim.Status = Enumeration.OrderStatus.Cancled;
                        estimateOrderToCarClaim.CancleTime = this.DateTime;

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.CarClaimDealtStep.VerifyAmount, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, operater, bizProcessesAudit.CurrentDetails.AuditComments, "后台人员撤销订单", this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeCarClaimDealtStatus(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, Enumeration.CarClaimDealtStatus.StaffCancle);

                        result = new CustomJsonResult(ResultType.Success, "撤销成功");

                        break;
                    case Enumeration.OperateType.Submit:


                        estimateOrderToCarClaim.WorkingHoursPrice = l_orderToCarClaim.WorkingHoursPrice;
                        estimateOrderToCarClaim.AccessoriesPrice = l_orderToCarClaim.AccessoriesPrice;


                        estimateOrderToCarClaim.EstimatePrice = l_orderToCarClaim.EstimatePrice;
                        estimateOrderToCarClaim.Remarks = orderToCarClaim.Remarks;


                        if (l_orderToCarClaim.RepairsType == Enumeration.RepairsType.EstimateRepair)
                        {
                            l_orderToCarClaim.Status = Enumeration.OrderStatus.Follow;
                            l_orderToCarClaim.FollowStatus = (int)Enumeration.OrderToCarClaimFollowStatus.WaitPayCommission;
                            l_orderToCarClaim.Price = 0;


                            estimateOrderToCarClaim.Status = Enumeration.OrderStatus.WaitPay;
                            estimateOrderToCarClaim.FollowStatus = (int)Enumeration.OrderToCarClaimFollowStatus.WaitPayCommission;
                            estimateOrderToCarClaim.Price = 0;//应付金额

                        }
                        else if (l_orderToCarClaim.RepairsType == Enumeration.RepairsType.Estimate)
                        {
                            l_orderToCarClaim.Status = Enumeration.OrderStatus.WaitPay;
                            l_orderToCarClaim.FollowStatus = (int)Enumeration.OrderToCarClaimFollowStatus.WaitPayCommission;
                            l_orderToCarClaim.Price = 0;//应付金额

                            estimateOrderToCarClaim.Status = Enumeration.OrderStatus.Follow;
                            estimateOrderToCarClaim.FollowStatus = (int)Enumeration.OrderToCarClaimFollowStatus.WaitPayCommission;
                            estimateOrderToCarClaim.Price = 0;
                        }

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.CarClaimDealtStep.VerifyAmount, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, operater, bizProcessesAudit.CurrentDetails.AuditComments, "复核金额完成，提交给商户支付", this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeCarClaimDealtStatus(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, Enumeration.CarClaimDealtStatus.Complete, "复核金额完成，提交给商户支付");


                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;

                }



                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;

        }

        public CustomJsonResult Cancle(int operater, string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var order = CurrentDb.Order.Where(m => m.Sn == orderSn).FirstOrDefault();
                if (order == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "找不到该订单");
                }

                if (order.Status == Enumeration.OrderStatus.Completed)
                {
                    return new CustomJsonResult(ResultType.Failure, "该已经完成，不能取消");
                }

                if (order.Status == Enumeration.OrderStatus.Cancled)
                {
                    return new CustomJsonResult(ResultType.Failure, "该已经被取消");
                }

                order.Remarks = "商户取消";
                order.CancleTime = this.DateTime;
                order.Status = Enumeration.OrderStatus.Cancled;



                switch (order.Type)
                {
                    case Enumeration.OrderType.InsureForCarForInsure:
                        BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarInsureOfferDealtStep.Complete, order.BizProcessesAuditId, operater, null, "取消订单", this.DateTime);
                        BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(operater, order.BizProcessesAuditId, Enumeration.CarInsureOfferDealtStatus.ClientCancle);
                        break;
                }
                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "该订单取消成功");
            }


            return result;

        }

        public CustomJsonResult ReCarInsureOffer(int operater, int userId, int merchantId, int orderId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var oldOrder = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderId).FirstOrDefault();

                var newOrder = new OrderToCarInsure();
                newOrder.MerchantId = oldOrder.MerchantId;
                newOrder.PosMachineId = oldOrder.PosMachineId;
                newOrder.UserId = oldOrder.UserId;

                //newOrder.ProductName = oldOrder.ProductName;
                //newOrder.ProductType = oldOrder.ProductType;
                newOrder.ClientRequire = oldOrder.ClientRequire;
                newOrder.InsuranceCompanyId = oldOrder.InsuranceCompanyId;
                newOrder.InsuranceCompanyName = oldOrder.InsuranceCompanyName;
                newOrder.CarOwner = oldOrder.CarOwner;
                newOrder.CarOwnerIdNumber = oldOrder.CarOwnerIdNumber;
                newOrder.CarOwnerAddress = oldOrder.CarOwnerAddress;
                newOrder.CarModel = oldOrder.CarModel;
                newOrder.CarModelName = oldOrder.CarModelName;
                newOrder.CarOwner = oldOrder.CarOwner;
                newOrder.CarPlateNo = oldOrder.CarPlateNo;
                newOrder.CarEngineNo = oldOrder.CarEngineNo;
                newOrder.CarVin = oldOrder.CarVin;
                newOrder.CarVechicheType = oldOrder.CarVechicheType;
                newOrder.CarRegisterDate = oldOrder.CarRegisterDate;
                newOrder.CarIssueDate = oldOrder.CarIssueDate;
                newOrder.CarPurchasePrice = oldOrder.CarPurchasePrice;
                newOrder.InsurePlanId = oldOrder.InsurePlanId;
                newOrder.CZ_CL_XSZ_ImgUrl = oldOrder.CZ_CL_XSZ_ImgUrl;
                newOrder.CZ_SFZ_ImgUrl = oldOrder.CZ_SFZ_ImgUrl;
                newOrder.YCZ_CLDJZ_ImgUrl = oldOrder.YCZ_CLDJZ_ImgUrl;
                newOrder.CCSJM_WSZM_ImgUrl = oldOrder.CCSJM_WSZM_ImgUrl;
                newOrder.ZJ1_ImgUrl = oldOrder.ZJ1_ImgUrl;
                newOrder.ZJ2_ImgUrl = oldOrder.ZJ2_ImgUrl;
                newOrder.ZJ3_ImgUrl = oldOrder.ZJ3_ImgUrl;
                newOrder.ZJ4_ImgUrl = oldOrder.ZJ4_ImgUrl;

                newOrder.Status = Enumeration.OrderStatus.Submitted;

                newOrder.SubmitTime = this.DateTime;
                newOrder.CreateTime = this.DateTime;
                newOrder.Creator = operater;
                newOrder.IsLastYearNewCar = oldOrder.IsLastYearNewCar;
                newOrder.IsSameLastYear = oldOrder.IsSameLastYear;
                newOrder.AutoCancelByHour = 24;
                CurrentDb.OrderToCarInsure.Add(newOrder);
                CurrentDb.SaveChanges();

                SnModel snModel = Sn.Build(SnType.OrderToCarInsure, newOrder.Id);

                newOrder.Sn = snModel.Sn;
                newOrder.TradeSnByWechat = snModel.TradeSnByWechat;
                newOrder.TradeSnByAlipay = snModel.TradeSnByAlipay;




                var oldOrderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == oldOrder.Id).ToList();
                if (oldOrderToCarInsureOfferCompany != null)
                {
                    foreach (var m in oldOrderToCarInsureOfferCompany)
                    {
                        var newOrderToCarInsureOfferCompany = new OrderToCarInsureOfferCompany();

                        newOrderToCarInsureOfferCompany.OrderId = newOrder.Id;
                        newOrderToCarInsureOfferCompany.InsuranceCompanyId = m.InsuranceCompanyId;
                        newOrderToCarInsureOfferCompany.InsuranceCompanyName = m.InsuranceCompanyName;
                        newOrderToCarInsureOfferCompany.CreateTime = this.DateTime;
                        newOrderToCarInsureOfferCompany.Creator = operater;
                        CurrentDb.OrderToCarInsureOfferCompany.Add(newOrderToCarInsureOfferCompany);
                    }
                }


                var oldKinds = CurrentDb.OrderToCarInsureOfferKind.Where(m => m.OrderId == oldOrder.Id).ToList();

                if (oldKinds != null)
                {
                    foreach (var m in oldKinds)
                    {
                        var newKinds = new OrderToCarInsureOfferKind();
                        newKinds.OrderId = newOrder.Id;
                        newKinds.KindId = m.KindId;
                        newKinds.KindValue = m.KindValue;
                        newKinds.KindDetails = m.KindDetails;
                        newKinds.IsWaiverDeductible = m.IsWaiverDeductible;
                        newKinds.CreateTime = this.DateTime;
                        newKinds.Creator = operater;
                        CurrentDb.OrderToCarInsureOfferKind.Add(newKinds);
                    }
                }

                BizProcessesAudit bizProcessesAudit = BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.OrderToCarInsure, newOrder.Id, Enumeration.CarInsureOfferDealtStatus.WaitOffer);

                BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarInsureOfferDealtStep.Submit, bizProcessesAudit.Id, operater, newOrder.ClientRequire, "商户重新报价，等待报价", this.DateTime);

                CurrentDb.SaveChanges();
                ts.Complete();


                result = new CustomJsonResult(ResultType.Success, "重新报价");

            }

            return result;
        }


        public CustomJsonResult SubmitLllegalQueryScoreRecharge(int operater, OrderToLllegalQueryRecharge orderToLllegalQueryRecharge)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == orderToLllegalQueryRecharge.UserId).FirstOrDefault();
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();

                orderToLllegalQueryRecharge.SalesmanId = merchant.SalesmanId ?? 0;
                orderToLllegalQueryRecharge.AgentId = merchant.AgentId ?? 0;

                orderToLllegalQueryRecharge.Type = Enumeration.OrderType.LllegalQueryRecharge;
                orderToLllegalQueryRecharge.TypeName = Enumeration.OrderType.LllegalQueryRecharge.GetCnName();
                orderToLllegalQueryRecharge.Status = Enumeration.OrderStatus.WaitPay;
                orderToLllegalQueryRecharge.SubmitTime = this.DateTime;
                orderToLllegalQueryRecharge.CreateTime = this.DateTime;
                orderToLllegalQueryRecharge.Creator = operater;
                CurrentDb.OrderToLllegalQueryRecharge.Add(orderToLllegalQueryRecharge);
                CurrentDb.SaveChanges();



                SnModel snModel = Sn.Build(SnType.OrderToLllegalQueryRecharge, orderToLllegalQueryRecharge.Id);

                orderToLllegalQueryRecharge.Sn = snModel.Sn;
                orderToLllegalQueryRecharge.TradeSnByWechat = snModel.TradeSnByWechat;
                orderToLllegalQueryRecharge.TradeSnByAlipay = snModel.TradeSnByAlipay;
                CurrentDb.SaveChanges();



                OrderConfirmInfo yOrder = new OrderConfirmInfo();


                yOrder.OrderId = orderToLllegalQueryRecharge.Id;
                yOrder.OrderSn = orderToLllegalQueryRecharge.Sn;
                yOrder.remarks = orderToLllegalQueryRecharge.Remarks;
                yOrder.orderType = orderToLllegalQueryRecharge.Type;
                yOrder.orderTypeName = orderToLllegalQueryRecharge.TypeName;

                // yOrder.confirmField.Add(new Entity.AppApi.OrderField("订单编号", orderToLllegalQueryRecharge.Sn.NullToEmpty()));
                yOrder.confirmField.Add(new Entity.AppApi.OrderField("积分", string.Format("{0}", orderToLllegalQueryRecharge.Score)));
                yOrder.confirmField.Add(new Entity.AppApi.OrderField("支付金额", string.Format("{0}元", orderToLllegalQueryRecharge.Price.NullToEmpty())));


                #region 支持的支付方式
                int[] payMethods = new int[1] { 1 };

                foreach (var payWayId in payMethods)
                {
                    var payWay = new PayWay();
                    payWay.id = payWayId;
                    yOrder.payMethod.Add(payWay);
                }
                #endregion


                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "提交成功", yOrder);
            }


            return result;
        }


        public CustomJsonResult GetPayTranSn(int operater, OrderPayTrans orderPayTrans)
        {
            CustomJsonResult result = new CustomJsonResult();

            return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "暂时没有开通该支付方式");

            using (TransactionScope ts = new TransactionScope())
            {


                var order = CurrentDb.Order.Where(m => m.UserId == orderPayTrans.UserId && m.Id == orderPayTrans.OrderId && m.Sn == orderPayTrans.OrderSn).FirstOrDefault();
                if (order == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "跳转支付错误");
                }

                var l_orderPayTrans = new OrderPayTrans();

                l_orderPayTrans.UserId = orderPayTrans.UserId;
                l_orderPayTrans.MerchantId = orderPayTrans.MerchantId;
                l_orderPayTrans.PosMachineId = orderPayTrans.PosMachineId;
                l_orderPayTrans.OrderId = orderPayTrans.OrderId;
                l_orderPayTrans.OrderSn = orderPayTrans.OrderSn;
                l_orderPayTrans.TransType = orderPayTrans.TransType;

                string orderPrice = "";
                if (BizFactory.AppSettings.IsTest)
                {

                    orderPrice = "0.01";
                    l_orderPayTrans.Amount = "1";
                }
                else
                {

                    orderPrice = order.Price.ToF2Price();
                    l_orderPayTrans.Amount = Convert.ToInt32((order.Price * 100)).ToString();
                }

                l_orderPayTrans.CreateTime = DateTime.Now;
                l_orderPayTrans.Creator = operater;
                CurrentDb.OrderPayTrans.Add(l_orderPayTrans);
                CurrentDb.SaveChanges();

                SnModel snModel = Sn.Build(SnType.OrderPayTrans, l_orderPayTrans.Id);

                l_orderPayTrans.Sn = snModel.Sn;

                CurrentDb.SaveChanges();
                ts.Complete();


                var model = new { orderId = orderPayTrans.OrderId, orderSn = orderPayTrans.OrderSn, payTransSn = l_orderPayTrans.Sn, transType = l_orderPayTrans.TransType, amount = l_orderPayTrans.Amount, orderPrice = orderPrice };

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", model);

            }

            return result;
        }

    }
}
