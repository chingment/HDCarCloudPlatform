using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz.Back
{
    class Provider111
    {
        //                            case Enumeration.ProductType.InsureForCarForInsure:
        //                        #region  投保
        //                        var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Sn == model.OrderSn).FirstOrDefault();

        //        var payCarInsureConfirmParams = Newtonsoft.Json.JsonConvert.DeserializeObject<PayCarInsureConfirmParams>(model.Params.ToString());


        //        var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.Id == payCarInsureConfirmParams.OfferId).FirstOrDefault();


        //        orderToCarInsure.InsuranceCompanyId = orderToCarInsureOfferCompany.InsuranceCompanyId;
        //                        orderToCarInsure.InsureImgUrl = orderToCarInsureOfferCompany.InsureImgUrl;
        //                        orderToCarInsure.InsuranceCompanyName = orderToCarInsureOfferCompany.InsuranceCompanyName;
        //                        orderToCarInsure.InsuranceOrderId = orderToCarInsureOfferCompany.InsuranceOrderId;
        //                        orderToCarInsure.CommercialPrice = orderToCarInsureOfferCompany.CommercialPrice == null ? 0 : orderToCarInsureOfferCompany.CommercialPrice.Value;
        //                        orderToCarInsure.TravelTaxPrice = orderToCarInsureOfferCompany.TravelTaxPrice == null ? 0 : orderToCarInsureOfferCompany.TravelTaxPrice.Value;
        //                        orderToCarInsure.CompulsoryPrice = orderToCarInsureOfferCompany.CompulsoryPrice == null ? 0 : orderToCarInsureOfferCompany.CompulsoryPrice.Value;


        //                        orderToCarInsure.Recipient = payCarInsureConfirmParams.Recipient;
        //                        orderToCarInsure.RecipientPhoneNumber = payCarInsureConfirmParams.RecipientPhoneNumber;//收件人电话
        //                        orderToCarInsure.RecipientAddress = payCarInsureConfirmParams.RecipientAddress;
        //                        orderToCarInsure.Price = orderToCarInsureOfferCompany.InsureTotalPrice.Value;

        //                        CurrentDb.SaveChanges();

        //                        var insuranceCompany = CurrentDb.InsuranceCompany.Where(m => m.Id == orderToCarInsureOfferCompany.InsuranceCompanyId).FirstOrDefault();
        //        var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == orderToCarInsureOfferCompany.InsuranceCompanyId).FirstOrDefault();

        //        yOrder.productName = orderToCarInsure.ProductName;
        //                        yOrder.transName = "消费";
        //                        yOrder.amount = orderToCarInsure.Price.ToF2Price().Replace(".", "").PadLeft(12, '0');

        //        yOrder.confirmField.Add(new OrderField("订单编号", orderToCarInsure.Sn.NullToEmpty()));
        //                        yOrder.confirmField.Add(new OrderField("保险公司", orderToCarInsure.InsuranceCompanyName.NullToEmpty()));
        //                        yOrder.confirmField.Add(new OrderField("车主姓名", orderToCarInsure.CarOwner.NullToEmpty()));
        //                        yOrder.confirmField.Add(new OrderField("车牌号码", orderToCarInsure.CarPlateNo.NullToEmpty()));
        //                        yOrder.confirmField.Add(new OrderField("支付金额", string.Format("{0}元", orderToCarInsure.Price.ToF2Price())));


        //                        yOrderInfo.order_no = orderToCarInsure.Sn.NullToEmpty();

        //                        yOrderInfo.teacct = orderToCarInsure.InsuranceOrderId.NullToEmpty();
        //                        yOrderInfo.company = orderToCarInsure.InsuranceCompanyName.NullToEmpty();
        //                        yOrderInfo.product_type = "";
        //                        yOrderInfo.customer_id_type = "01";
        //                        yOrderInfo.customer_id = orderToCarInsure.CarOwnerIdNumber.NullToEmpty();
        //                        yOrderInfo.customer_sex = "";
        //                        yOrderInfo.customer_name = orderToCarInsure.CarOwner.NullToEmpty();
        //                        yOrderInfo.customer_mobile_no = "";
        //                        yOrderInfo.customer_birthdate = "";
        //                        yOrderInfo.teacct = orderToCarInsure.InsuranceOrderId.NullToEmpty();
        //                        yOrderInfo.car_type = orderToCarInsure.CarVechicheType.NullToEmpty();
        //                        yOrderInfo.car_license = orderToCarInsure.CarPlateNo.NullToEmpty();
        //                        yOrderInfo.car_frame_no = "";
        //                        yOrderInfo.payer_id_type = "";
        //                        yOrderInfo.payer_id = "";
        //                        yOrderInfo.payer_name = "";
        //                        yOrderInfo.payer_mobile_no = "";
        //                        yOrderInfo.payer_address = "";
        //                        yOrderInfo.ci_amt = orderToCarInsure.CommercialPrice.ToF2Price();
        //                        yOrderInfo.tci_amt = orderToCarInsure.CompulsoryPrice.ToF2Price();
        //                        yOrderInfo.vvt_amt = orderToCarInsure.TravelTaxPrice.ToF2Price();
        //                        //yOrderInfo.qxt_account = merchant.ClientCode;
        //                        yOrderInfo.teller_id = merchant.ClientCode;
        //                        yOrderInfo.pay_type = "车险";



        //                        ybs_mer = BizFactory.Ybs.GetCarInsureMerchantInfo(insuranceCompany.Id, "", "", "", "");

        //                        yOrderInfo.ybs_mer_code = ybs_mer.ybs_mer_code;
        //                        yOrderInfo.merchant_id = ybs_mer.merchant_id;
        //                        yOrderInfo.merchant_name = ybs_mer.merchant_name;
        //                        yOrderInfo.biz_code = ybs_mer.biz_code;
        //                        yOrderInfo.phone_no = "";
        //                        yOrderInfo.cashier_id = "";

        //                        yOrder.orderInfo = yOrderInfo;




        //                        #region 支持的支付方式
        //                        payMethods = Array.ConvertAll<string, int>(carInsuranceCompany.PayWays.Split(','), s => int.Parse(s));

        //                        foreach (var payMethodId in payMethods)
        //                        {
        //                            var payWay = new PayWay();
        //        payWay.id = payMethodId;

        //                            switch (payMethodId)
        //                            {
        //                                case 1:
        //                                    yOrder.payMethod.Add(payWay);
        //                                    break;
        //                                case 2:

        //                                    if (!string.IsNullOrEmpty(orderToCarInsureOfferCompany.PayUrl))
        //                                    {
        //                                        payWay.param = string.Format("{0}/app/order/InsOfferPay?offerid={1}", BizFactory.AppSettings.WebAppServerUrl, payCarInsureConfirmParams.OfferId); //orderToCarInsureOfferCompany.PayUrl;

        //        yOrder.payMethod.Add(payWay);
        //                                    }


        //                                    break;
        //                            }
        //                        }
        //                        #endregion 


        //                        result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "确认成功", yOrder);
        //                        #endregion
        //                        break;
        //                    case Enumeration.ProductType.InsureForCarForClaim:
        //                        #region 理赔
        //                        var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Sn == model.OrderSn).FirstOrDefault();


        //yOrder.productName = orderToCarClaim.ProductName;
        //                        yOrder.transName = "消费";

        //                        yOrder.amount = orderToCarClaim.Price.ToF2Price().Replace(".", "").PadLeft(12, '0');

        //yOrder.confirmField.Add(new OrderField("订单编号", orderToCarClaim.Sn.NullToEmpty()));
        //                        yOrder.confirmField.Add(new OrderField("保险公司", orderToCarClaim.InsuranceCompanyName));
        //                        yOrder.confirmField.Add(new OrderField("车牌号码", orderToCarClaim.CarPlateNo));
        //                        yOrder.confirmField.Add(new OrderField("支付金额", string.Format("{0}元", orderToCarClaim.Price.NullToEmpty())));


        //                        yOrderInfo.order_no = orderToCarClaim.Sn;
        //                        yOrderInfo.customer_id_type = "";
        //                        yOrderInfo.customer_id = "";
        //                        yOrderInfo.customer_sex = "";
        //                        yOrderInfo.customer_name = "";
        //                        yOrderInfo.customer_mobile_no = "";
        //                        yOrderInfo.customer_birthdate = "";
        //                        yOrderInfo.payer_id_type = "";
        //                        yOrderInfo.payer_id = "";
        //                        yOrderInfo.payer_name = "";
        //                        yOrderInfo.payer_mobile_no = "";
        //                        yOrderInfo.payer_address = "";
        //                        // yOrderInfo.qxt_account = merchant.ClientCode;
        //                        yOrderInfo.teller_id = merchant.ClientCode;
        //                        yOrderInfo.pay_type = "理赔服务";

        //                        ybs_mer = BizFactory.Ybs.GetCarClaimMerchantInfo();
        //                        yOrderInfo.ybs_mer_code = ybs_mer.ybs_mer_code;
        //                        yOrderInfo.merchant_id = ybs_mer.merchant_id;
        //                        yOrderInfo.merchant_name = ybs_mer.merchant_name;
        //                        yOrderInfo.biz_code = ybs_mer.biz_code;
        //                        yOrderInfo.phone_no = "";
        //                        yOrderInfo.cashier_id = "";


        //                        yOrder.orderInfo = yOrderInfo;


        //                        #region 支持的支付方式
        //                        payMethods = new int[1] { 1 };

        //                        foreach (var payWayId in payMethods)
        //                        {
        //                            var payWay = new PayWay();
        //payWay.id = payWayId;
        //                            yOrder.payMethod.Add(payWay);
        //                        }
        //                        #endregion 


        //                        result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "确认成功", yOrder);
        //                        #endregion
        //                        break;
    }
}


//private CustomJsonResult PayCarInsureCompleted(int operater, string orderSn)
//{
//    CustomJsonResult result = new CustomJsonResult();

//    using (TransactionScope ts = new TransactionScope())
//    {
//        var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Sn == orderSn).FirstOrDefault();


//        if (orderToCarInsure.Status == Enumeration.OrderStatus.Completed)
//        {
//            ts.Complete();
//            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经支付完成");
//        }

//        if (orderToCarInsure.Status != Enumeration.OrderStatus.WaitPay)
//        {
//            ts.Complete();
//            return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单未在就绪支付状态");
//        }


//        orderToCarInsure.Status = Enumeration.OrderStatus.Completed;
//        orderToCarInsure.PayTime = this.DateTime;
//        orderToCarInsure.CompleteTime = this.DateTime;
//        orderToCarInsure.LastUpdateTime = this.DateTime;
//        orderToCarInsure.Mender = operater;


//        var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.AduitType == Enumeration.BizProcessesAuditType.CarInsure && m.AduitReferenceId == orderToCarInsure.Id).FirstOrDefault();
//        if (bizProcessesAudit != null)
//        {
//            BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarInsureOfferDealtStep.Complete, bizProcessesAudit.Id, orderToCarInsure.Creator, null, "支付完成", this.DateTime);
//            BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(orderToCarInsure.Creator, bizProcessesAudit.Id, Enumeration.CarInsureOfferDealtStatus.Complete);
//        }

//        CurrentDb.SaveChanges();
//        ts.Complete();


//        result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功");

//    }

//    return result;
//}

//private CustomJsonResult PayCarClaimCompleted(int operater, string orderSn)
//{
//    CustomJsonResult result = new CustomJsonResult();

//    using (TransactionScope ts = new TransactionScope())
//    {
//        var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Sn == orderSn).FirstOrDefault();


//        if (orderToCarClaim.Status == Enumeration.OrderStatus.Completed)
//        {
//            ts.Complete();
//            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经支付完成");
//        }


//        if (orderToCarClaim.Status != Enumeration.OrderStatus.WaitPay)
//        {
//            ts.Complete();
//            return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单未在就绪支付状态");
//        }

//        orderToCarClaim.Status = Enumeration.OrderStatus.Completed;
//        orderToCarClaim.PayTime = this.DateTime;
//        orderToCarClaim.CompleteTime = this.DateTime;
//        orderToCarClaim.LastUpdateTime = this.DateTime;
//        orderToCarClaim.Mender = operater;




//        if (orderToCarClaim.HandOrderId != null)
//        {
//            var handOrder = CurrentDb.OrderToCarClaim.Where(m => m.Id == orderToCarClaim.HandOrderId.Value).FirstOrDefault();

//            handOrder.Status = Enumeration.OrderStatus.Completed;
//            handOrder.PayTime = this.DateTime;
//            handOrder.CompleteTime = this.DateTime;
//            handOrder.LastUpdateTime = this.DateTime;
//            handOrder.Mender = operater;
//            CurrentDb.SaveChanges();

//            var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.AduitType == Enumeration.BizProcessesAuditType.CarClaim && m.AduitReferenceId == orderToCarClaim.HandOrderId.Value).FirstOrDefault();
//            if (bizProcessesAudit != null)
//            {
//                BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarClaimDealtStep.Complete, bizProcessesAudit.Id, operater, null, "支付完成", this.DateTime);
//                BizFactory.BizProcessesAudit.ChangeCarClaimDealtStatus(operater, bizProcessesAudit.Id, Enumeration.CarClaimDealtStatus.Complete);
//            }

//        }

//        CurrentDb.SaveChanges();
//        ts.Complete();

//        result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功");
//    }

//    return result;
//}