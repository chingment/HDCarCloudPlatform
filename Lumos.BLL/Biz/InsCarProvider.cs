using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using YdtSdk;

namespace Lumos.BLL
{

    public class UpdateOrderOfferPms
    {
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int PosMachineId { get; set; }
        public int Auto { get; set; }
        public string PartnerOrderId { get; set; }
        public string PartnerInquirySeq { get; set; }
        public int PartnerChannelId { get; set; }
        public string PartnerCompanyId { get; set; }
        public int PartnerRisk { get; set; }
        public List<InquiryModel> Inquirys { get; set; }
        public List<CoverageModel> Coverages { get; set; }
        public string CiStartDate { get; set; }
        public string BiStartDate { get; set; }
        public Enumeration.OfferResult OfferResult { get; set; }
    }

    public class UpdateOfferByAfterResult
    {
        public UpdateOfferByAfterResult()
        {
            this.CarInsure = new OrderToCarInsure();
            this.CarInsureOfferCompany = new OrderToCarInsureOfferCompany();
            this.CarInsureOfferCompanyKinds = new List<OrderToCarInsureOfferCompanyKind>();
        }
        public OrderToCarInsure CarInsure { get; set; }

        public OrderToCarInsureOfferCompany CarInsureOfferCompany { get; set; }

        public List<OrderToCarInsureOfferCompanyKind> CarInsureOfferCompanyKinds { get; set; }


    }

    public class InsCarProvider : BaseProvider
    {
        public void UpdateOrder(int operater, int userId, int merchantId, int posMachineId, string partnerOrderId, CarInfoModel carInfo, List<CarInsCustomerModel> customers)
        {
            var result = new CustomJsonResult();
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    OrderToCarInsure l_orderToCarInsure = null;

                    l_orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.PartnerOrderId == partnerOrderId && m.UserId == userId).FirstOrDefault();


                    var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == userId).FirstOrDefault();
                    var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantId).FirstOrDefault();

                    if (l_orderToCarInsure == null)
                    {
                        l_orderToCarInsure = new OrderToCarInsure();

                        l_orderToCarInsure.Type = Enumeration.OrderType.InsureForCarForInsure;
                        l_orderToCarInsure.TypeName = Enumeration.OrderType.InsureForCarForInsure.GetCnName();
                        l_orderToCarInsure.SalesmanId = merchant.SalesmanId ?? 0;
                        l_orderToCarInsure.AgentId = merchant.AgentId ?? 0;
                        l_orderToCarInsure.Status = Enumeration.OrderStatus.Submitted;
                        l_orderToCarInsure.SubmitTime = this.DateTime;
                        l_orderToCarInsure.UserId = userId;
                        l_orderToCarInsure.MerchantId = merchantId;
                        l_orderToCarInsure.PosMachineId = posMachineId;
                        l_orderToCarInsure.PartnerOrderId = partnerOrderId;
                        l_orderToCarInsure.SubmitTime = this.DateTime;
                        l_orderToCarInsure.CreateTime = this.DateTime;
                        l_orderToCarInsure.Creator = operater;
                    }
                    else
                    {
                        l_orderToCarInsure.LastUpdateTime = this.DateTime;
                        l_orderToCarInsure.Mender = operater;
                    }

                    l_orderToCarInsure.CarBelong = carInfo.Belong;
                    l_orderToCarInsure.CarType = carInfo.CarType;
                    l_orderToCarInsure.CarLicensePlateNo = carInfo.LicensePlateNo;
                    l_orderToCarInsure.CarVin = carInfo.Vin;
                    l_orderToCarInsure.CarEngineNo = carInfo.EngineNo;
                    l_orderToCarInsure.CarFirstRegisterDate = carInfo.FirstRegisterDate;
                    l_orderToCarInsure.CarModelCode = carInfo.ModelCode;
                    l_orderToCarInsure.CarModelName = carInfo.ModelName;
                    l_orderToCarInsure.CarDisplacement = carInfo.Displacement;
                    l_orderToCarInsure.CarMarketYear = carInfo.MarketYear;
                    l_orderToCarInsure.CarRatedPassengerCapacity = carInfo.RatedPassengerCapacity;
                    l_orderToCarInsure.CarReplacementValue = carInfo.ReplacementValue;
                    l_orderToCarInsure.CarChgownerType = carInfo.ChgownerType;
                    l_orderToCarInsure.CarChgownerDate = carInfo.ChgownerDate;
                    l_orderToCarInsure.CarTonnage = carInfo.Tonnage;
                    l_orderToCarInsure.CarWholeWeight = carInfo.WholeWeight;
                    l_orderToCarInsure.CarLicensePicKey = carInfo.LicensePicKey;
                    l_orderToCarInsure.CarLicenseOtherPicKey = carInfo.LicenseOtherPicKey;
                    l_orderToCarInsure.CarCertPicKey = carInfo.CarCertPicKey;
                    l_orderToCarInsure.CarInvoicePicKey = carInfo.CarInvoicePicKey;

                    if (customers != null)
                    {
                        var carowner = customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                        if (carowner != null)
                        {
                            l_orderToCarInsure.CarownerInsuredFlag = carowner.InsuredFlag;
                            l_orderToCarInsure.CarownerName = carowner.Name;
                            l_orderToCarInsure.CarownerCertNo = carowner.CertNo;
                            l_orderToCarInsure.CarownerMobile = carowner.Mobile;
                            l_orderToCarInsure.CarownerAddress = carowner.Address;
                            l_orderToCarInsure.CarownerIdentityFacePicKey = carowner.IdentityFacePicKey;
                            l_orderToCarInsure.CarownerIdentityBackPicKey = carowner.IdentityFacePicKey;
                            l_orderToCarInsure.CarownerOrgPicKey = carowner.OrgPicKey;
                        }

                        var policyholder = customers.Where(m => m.InsuredFlag == "1").FirstOrDefault();
                        if (policyholder != null)
                        {
                            l_orderToCarInsure.PolicyholderInsuredFlag = policyholder.InsuredFlag;
                            l_orderToCarInsure.PolicyholderName = policyholder.Name;
                            l_orderToCarInsure.PolicyholderCertNo = policyholder.CertNo;
                            l_orderToCarInsure.PolicyholderMobile = policyholder.Mobile;
                            l_orderToCarInsure.PolicyholderAddress = policyholder.Address;
                            l_orderToCarInsure.PolicyholderIdentityFacePicKey = policyholder.IdentityFacePicKey;
                            l_orderToCarInsure.PolicyholderIdentityBackPicKey = policyholder.IdentityFacePicKey;
                            l_orderToCarInsure.PolicyholderOrgPicKey = policyholder.OrgPicKey;
                        }


                        var insured = customers.Where(m => m.InsuredFlag == "2").FirstOrDefault();
                        if (insured != null)
                        {
                            l_orderToCarInsure.InsuredInsuredFlag = insured.InsuredFlag;
                            l_orderToCarInsure.InsuredName = insured.Name;
                            l_orderToCarInsure.InsuredCertNo = insured.CertNo;
                            l_orderToCarInsure.InsuredMobile = insured.Mobile;
                            l_orderToCarInsure.InsuredAddress = insured.Address;
                            l_orderToCarInsure.InsuredIdentityFacePicKey = insured.IdentityFacePicKey;
                            l_orderToCarInsure.InsuredIdentityBackPicKey = insured.IdentityFacePicKey;
                            l_orderToCarInsure.InsuredOrgPicKey = insured.OrgPicKey;

                        }
                    }


                    if (l_orderToCarInsure.Id == 0)
                    {
                        CurrentDb.OrderToCarInsure.Add(l_orderToCarInsure);
                        CurrentDb.SaveChanges();
                        SnModel snModel = Sn.Build(SnType.OrderToCarInsure, l_orderToCarInsure.Id);
                        l_orderToCarInsure.Sn = snModel.Sn;
                    }

                    CurrentDb.SaveChanges();
                    ts.Complete();
                }

            }
            catch (Exception ex)
            {
                Log.Error("本系统基础数据保存失败1", ex);

            }

        }


        public CustomJsonResult UpdateOfferByBefore(int operater, UpdateOrderOfferPms pms)
        {
            var result = new CustomJsonResult();
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var l_orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.PartnerOrderId == pms.PartnerOrderId && m.UserId == pms.UserId).FirstOrDefault();


                    var partnerCompany = YdtDataMap.YdtInsComanyList().Where(m => m.YdtCode == pms.PartnerCompanyId).FirstOrDefault();

                    if (partnerCompany == null)
                        return new CustomJsonResult(ResultType.Failure, "找不到第三方系统的保险公司，映射无结果");

                    var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == partnerCompany.UpLinkCode).FirstOrDefault();
                    if (carInsuranceCompany == null)
                        return new CustomJsonResult(ResultType.Failure, "找不到本系统的保险公司");


                    var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == l_orderToCarInsure.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).FirstOrDefault();

                    if (orderToCarInsureOfferCompany == null)
                    {
                        orderToCarInsureOfferCompany = new OrderToCarInsureOfferCompany();
                        orderToCarInsureOfferCompany.OrderId = l_orderToCarInsure.Id;
                        orderToCarInsureOfferCompany.CreateTime = this.DateTime;
                        orderToCarInsureOfferCompany.Creator = operater;
                    }
                    else
                    {
                        orderToCarInsureOfferCompany.LastUpdateTime = this.DateTime;
                        orderToCarInsureOfferCompany.Mender = operater;
                    }

                    orderToCarInsureOfferCompany.IsAuto = pms.Auto == 0 ? false : true;
                    orderToCarInsureOfferCompany.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                    orderToCarInsureOfferCompany.InsuranceCompanyName = carInsuranceCompany.InsuranceCompanyName;
                    orderToCarInsureOfferCompany.InsuranceCompanyImgUrl = carInsuranceCompany.InsuranceCompanyImgUrl;
                    orderToCarInsureOfferCompany.BiStartDate = pms.BiStartDate;
                    orderToCarInsureOfferCompany.CiStartDate = pms.CiStartDate;
                    orderToCarInsureOfferCompany.PartnerOrderId = l_orderToCarInsure.PartnerOrderId;
                    orderToCarInsureOfferCompany.PartnerInquiryId = pms.PartnerInquirySeq;
                    orderToCarInsureOfferCompany.PartnerCompanyId = pms.PartnerCompanyId;
                    orderToCarInsureOfferCompany.PartnerChannelId = pms.PartnerChannelId.ToString();
                    orderToCarInsureOfferCompany.PartnerRisk = pms.PartnerRisk.ToString();


                    if (orderToCarInsureOfferCompany.Id == 0)
                    {
                        CurrentDb.OrderToCarInsureOfferCompany.Add(orderToCarInsureOfferCompany);
                    }

                    CurrentDb.SaveChanges();

                    var orderToCarInsureOfferCompanyKinds = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == l_orderToCarInsure.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).ToList();

                    foreach (var item in orderToCarInsureOfferCompanyKinds)
                    {
                        CurrentDb.OrderToCarInsureOfferCompanyKind.Remove(item);
                        CurrentDb.SaveChanges();
                    }

                    if (pms.Coverages != null)
                    {
                        foreach (var item in pms.Coverages)
                        {
                            var partnerKind = YdtDataMap.YdtInsCoverageList().Where(m => m.Code == item.code).FirstOrDefault();
                            if (partnerKind != null)
                            {
                                var orderToCarInsureOfferCompanyKind = new OrderToCarInsureOfferCompanyKind();
                                orderToCarInsureOfferCompanyKind.OrderId = l_orderToCarInsure.Id;
                                orderToCarInsureOfferCompanyKind.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                                orderToCarInsureOfferCompanyKind.KindId = partnerKind.UpLinkCode;
                                orderToCarInsureOfferCompanyKind.PartnerKindId = partnerKind.Code;
                                orderToCarInsureOfferCompanyKind.Compensation = item.compensation;
                                orderToCarInsureOfferCompanyKind.KindName = partnerKind.Name;
                                orderToCarInsureOfferCompanyKind.Quantity = item.quantity;
                                orderToCarInsureOfferCompanyKind.GlassType = item.glassType;
                                orderToCarInsureOfferCompanyKind.Amount = item.amount;
                                orderToCarInsureOfferCompanyKind.Creator = operater;
                                orderToCarInsureOfferCompanyKind.CreateTime = this.DateTime;
                                orderToCarInsureOfferCompanyKind.IsCompensation = partnerKind.IsCompensation;
                                orderToCarInsureOfferCompanyKind.Priority = partnerKind.Priority;
                                CurrentDb.OrderToCarInsureOfferCompanyKind.Add(orderToCarInsureOfferCompanyKind);
                                CurrentDb.SaveChanges();
                            }
                        }
                    }

                    CurrentDb.SaveChanges();
                    ts.Complete();

                    return new CustomJsonResult(ResultType.Success, "提交成功");
                }
            }
            catch (Exception ex)
            {
                Log.Error("本系统基础数据保存失败1", ex);
                return new CustomJsonResult(ResultType.Failure, "本系统基础数据保存失败1");
            }
        }

        public CustomJsonResult<UpdateOfferByAfterResult> UpdateOfferByAfter(int operater, UpdateOrderOfferPms pms)
        {
            var result = new CustomJsonResult<UpdateOfferByAfterResult>();

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var resultData = new UpdateOfferByAfterResult();

                    var l_orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.PartnerOrderId == pms.PartnerOrderId && m.UserId == pms.UserId).FirstOrDefault();

                    var partnerCompany = YdtDataMap.YdtInsComanyList().Where(m => m.YdtCode == pms.PartnerCompanyId).FirstOrDefault();

                    var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == partnerCompany.UpLinkCode).FirstOrDefault();

                    var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == l_orderToCarInsure.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).FirstOrDefault();

                    orderToCarInsureOfferCompany.OfferResult = pms.OfferResult;
                    orderToCarInsureOfferCompany.PartnerInquiryId = pms.PartnerInquirySeq;
                    orderToCarInsureOfferCompany.LastUpdateTime = this.DateTime;
                    orderToCarInsureOfferCompany.Mender = operater;

                    decimal insureTotalPrice = 0;

                    if (pms.Inquirys != null)
                    {
                        var commercial = pms.Inquirys.Where(m => m.risk == 1).FirstOrDefault();
                        if (commercial != null)
                        {
                            if (commercial.standardPremium != null)
                            {
                                if (commercial.standardPremium.Value > 0)
                                {
                                    orderToCarInsureOfferCompany.CommercialPrice = commercial.standardPremium;
                                    insureTotalPrice += commercial.standardPremium.Value;
                                }
                            }
                        }

                        var compulsory = pms.Inquirys.Where(m => m.risk == 2).FirstOrDefault();

                        if (compulsory != null)
                        {
                            if (compulsory.standardPremium != null)
                            {
                                if (compulsory.standardPremium.Value > 0)
                                {
                                    orderToCarInsureOfferCompany.CompulsoryPrice = compulsory.standardPremium;
                                    insureTotalPrice += compulsory.standardPremium.Value;
                                }
                            }

                            if (compulsory.sumPayTax != null)
                            {
                                if (compulsory.sumPayTax.Value > 0)
                                {
                                    orderToCarInsureOfferCompany.TravelTaxPrice = compulsory.sumPayTax;
                                    insureTotalPrice += compulsory.sumPayTax.Value;
                                }
                            }

                        }
                    }

                    orderToCarInsureOfferCompany.InsureTotalPrice = insureTotalPrice;
                    CurrentDb.SaveChanges();

                    var orderToCarInsureOfferCompanyKinds = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == l_orderToCarInsure.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).ToList();



                    foreach (var item in orderToCarInsureOfferCompanyKinds)
                    {
                        CurrentDb.OrderToCarInsureOfferCompanyKind.Remove(item);
                        CurrentDb.SaveChanges();
                    }

                    var out_carInsureOfferCompanyKinds = new List<OrderToCarInsureOfferCompanyKind>();
                    if (pms.Coverages != null)
                    {
                        foreach (var item in pms.Coverages)
                        {
                            var partnerKind = YdtDataMap.YdtInsCoverageList().Where(m => m.Code == item.code).FirstOrDefault();
                            if (partnerKind != null)
                            {
                                var orderToCarInsureOfferCompanyKind = new OrderToCarInsureOfferCompanyKind();
                                orderToCarInsureOfferCompanyKind.OrderId = l_orderToCarInsure.Id;
                                orderToCarInsureOfferCompanyKind.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                                orderToCarInsureOfferCompanyKind.KindId = partnerKind.UpLinkCode;
                                orderToCarInsureOfferCompanyKind.PartnerKindId = partnerKind.Code;
                                orderToCarInsureOfferCompanyKind.Compensation = item.compensation;
                                orderToCarInsureOfferCompanyKind.KindName = partnerKind.Name;
                                orderToCarInsureOfferCompanyKind.Quantity = item.quantity;
                                orderToCarInsureOfferCompanyKind.GlassType = item.glassType;
                                orderToCarInsureOfferCompanyKind.Amount = item.amount;
                                orderToCarInsureOfferCompanyKind.StandardPremium = item.standardPremium;
                                orderToCarInsureOfferCompanyKind.BasicPremium = item.basicPremium;
                                orderToCarInsureOfferCompanyKind.Premium = item.premium;
                                orderToCarInsureOfferCompanyKind.UnitAmount = item.unitAmount;
                                orderToCarInsureOfferCompanyKind.Discount = item.discount;
                                orderToCarInsureOfferCompanyKind.IsCompensation = partnerKind.IsCompensation;
                                orderToCarInsureOfferCompanyKind.Priority = partnerKind.Priority;
                                orderToCarInsureOfferCompanyKind.Creator = operater;
                                orderToCarInsureOfferCompanyKind.CreateTime = this.DateTime;
                                CurrentDb.OrderToCarInsureOfferCompanyKind.Add(orderToCarInsureOfferCompanyKind);
                                CurrentDb.SaveChanges();

                                out_carInsureOfferCompanyKinds.Add(orderToCarInsureOfferCompanyKind);
                            }
                        }
                    }


                    switch (pms.OfferResult)
                    {
                        case Enumeration.OfferResult.WaitArtificialOffer:
                            var bizProcessesAudit = BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.OrderToCarInsure, l_orderToCarInsure.UserId, l_orderToCarInsure.MerchantId, l_orderToCarInsure.Id, Enumeration.AuditFlowV1Status.Submit);
                            l_orderToCarInsure.BizProcessesAuditId = bizProcessesAudit.Id;
                            break;

                    }


                    CurrentDb.SaveChanges();
                    ts.Complete();

                    resultData.CarInsure = l_orderToCarInsure;
                    resultData.CarInsureOfferCompany = orderToCarInsureOfferCompany;
                    resultData.CarInsureOfferCompanyKinds = out_carInsureOfferCompanyKinds;
                    return new CustomJsonResult<UpdateOfferByAfterResult>(ResultType.Success, ResultCode.Success, "", resultData);
                }


            }
            catch (Exception ex)
            {
                Log.Error("本系统基础数据保存失败2", ex);
                return new CustomJsonResult<UpdateOfferByAfterResult>(ResultType.Failure, ResultCode.Failure, "本系统基础数据保存失败2", null);
            }
        }


        public void UpdateCarInfo(int operater, CarInfoModel carInfo, List<CarInsCustomerModel> customers)
        {
            if (carInfo == null)
                return;

            if (string.IsNullOrEmpty(carInfo.LicensePlateNo))
                return;

            var insCarInfo = CurrentDb.InsCarInfo.Where(m => m.LicensePlateNo == carInfo.LicensePlateNo).FirstOrDefault();
            if (insCarInfo == null)
            {
                insCarInfo = new InsCarInfo();
                insCarInfo.Creator = operater;
                insCarInfo.CreateTime = this.DateTime;
            }
            else
            {
                insCarInfo.Mender = operater;
                insCarInfo.LastUpdateTime = this.DateTime;
            }

            insCarInfo.Belong = carInfo.Belong;
            insCarInfo.CarType = carInfo.CarType;
            insCarInfo.LicensePlateNo = carInfo.LicensePlateNo;
            insCarInfo.Vin = carInfo.Vin;
            insCarInfo.EngineNo = carInfo.EngineNo;
            insCarInfo.FirstRegisterDate = carInfo.FirstRegisterDate;
            insCarInfo.ModelCode = carInfo.ModelCode;
            insCarInfo.ModelName = carInfo.ModelName;
            insCarInfo.Displacement = carInfo.Displacement;
            insCarInfo.MarketYear = carInfo.MarketYear;
            insCarInfo.RatedPassengerCapacity = carInfo.RatedPassengerCapacity;
            insCarInfo.ReplacementValue = carInfo.ReplacementValue;
            insCarInfo.ChgownerType = carInfo.ChgownerType;
            insCarInfo.ChgownerDate = carInfo.ChgownerDate;
            insCarInfo.Tonnage = carInfo.Tonnage;
            insCarInfo.WholeWeight = carInfo.WholeWeight;
            insCarInfo.LicensePicKey = carInfo.LicensePicKey;
            insCarInfo.LicenseOtherPicKey = carInfo.LicenseOtherPicKey;
            insCarInfo.CarCertPicKey = carInfo.CarCertPicKey;
            insCarInfo.CarInvoicePicKey = carInfo.CarInvoicePicKey;

            if (customers != null)
            {
                var carowner = customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                if (carowner != null)
                {
                    insCarInfo.CarownerInsuredFlag = carowner.InsuredFlag;
                    insCarInfo.CarowneName = carowner.Name;
                    insCarInfo.CarowneCertNo = carowner.CertNo;
                    insCarInfo.CarowneMobile = carowner.Mobile;
                    insCarInfo.CarowneAddress = carowner.Address;
                    insCarInfo.CarowneIdentityFacePicKey = carowner.IdentityFacePicKey;
                    insCarInfo.CarowneIdentityBackPicKey = carowner.IdentityFacePicKey;
                    insCarInfo.CarowneOrgPicKey = carowner.OrgPicKey;
                }

                var policyholder = customers.Where(m => m.InsuredFlag == "1").FirstOrDefault();
                if (policyholder != null)
                {
                    insCarInfo.PolicyholderInsuredFlag = policyholder.InsuredFlag;
                    insCarInfo.PolicyholderName = policyholder.Name;
                    insCarInfo.PolicyholderCertNo = policyholder.CertNo;
                    insCarInfo.PolicyholderMobile = policyholder.Mobile;
                    insCarInfo.PolicyholderAddress = policyholder.Address;
                    insCarInfo.PolicyholderIdentityFacePicKey = policyholder.IdentityFacePicKey;
                    insCarInfo.PolicyholderIdentityBackPicKey = policyholder.IdentityFacePicKey;
                    insCarInfo.PolicyholderOrgPicKey = policyholder.OrgPicKey;
                }


                var insured = customers.Where(m => m.InsuredFlag == "2").FirstOrDefault();
                if (insured != null)
                {
                    insCarInfo.InsuredInsuredFlag = insured.InsuredFlag;
                    insCarInfo.InsuredName = insured.Name;
                    insCarInfo.InsuredCertNo = insured.CertNo;
                    insCarInfo.InsuredMobile = insured.Mobile;
                    insCarInfo.InsuredAddress = insured.Address;
                    insCarInfo.InsuredIdentityFacePicKey = insured.IdentityFacePicKey;
                    insCarInfo.InsuredIdentityBackPicKey = insured.IdentityFacePicKey;
                    insCarInfo.InsuredOrgPicKey = insured.OrgPicKey;

                }
            }

            if (insCarInfo.Id == 0)
            {
                CurrentDb.InsCarInfo.Add(insCarInfo);
            }

            CurrentDb.SaveChanges();
        }
    }
}
