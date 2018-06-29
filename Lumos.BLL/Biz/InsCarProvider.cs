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

        public int CarInfoOrderId { get; set; }
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
        public int UpdateCarInfoOrder(int operater, int userId, string partnerOrderId, CarInfoModel carInfo, List<CarInsCustomerModel> customers)
        {
            int carInfoOrderId = 0;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    UpdateCarInfo(operater, carInfo, customers);

                    var insCarInfoOrder = new InsCarInfoOrder();
                    insCarInfoOrder.UserId = userId;
                    insCarInfoOrder.PartnerOrderId = partnerOrderId;
                    insCarInfoOrder.Belong = carInfo.Belong;
                    insCarInfoOrder.CarType = carInfo.CarType;
                    insCarInfoOrder.LicensePlateNo = carInfo.LicensePlateNo;
                    insCarInfoOrder.Vin = carInfo.Vin;
                    insCarInfoOrder.EngineNo = carInfo.EngineNo;
                    insCarInfoOrder.FirstRegisterDate = carInfo.FirstRegisterDate;
                    insCarInfoOrder.ModelCode = carInfo.ModelCode;
                    insCarInfoOrder.ModelName = carInfo.ModelName;
                    insCarInfoOrder.Displacement = carInfo.Displacement;
                    insCarInfoOrder.MarketYear = carInfo.MarketYear;
                    insCarInfoOrder.RatedPassengerCapacity = carInfo.RatedPassengerCapacity;
                    insCarInfoOrder.ReplacementValue = carInfo.ReplacementValue;
                    insCarInfoOrder.ChgownerType = carInfo.ChgownerType;
                    insCarInfoOrder.ChgownerDate = carInfo.ChgownerDate;
                    insCarInfoOrder.Tonnage = carInfo.Tonnage;
                    insCarInfoOrder.WholeWeight = carInfo.WholeWeight;
                    insCarInfoOrder.LicensePicKey = carInfo.LicensePicKey;
                    insCarInfoOrder.LicensePicUrl = carInfo.LicensePicUrl;
                    insCarInfoOrder.LicenseOtherPicKey = carInfo.LicenseOtherPicKey;
                    insCarInfoOrder.LicenseOtherPicUrl = carInfo.LicenseOtherPicUrl;
                    insCarInfoOrder.CarCertPicKey = carInfo.CarCertPicKey;
                    insCarInfoOrder.CarCertPicUrl = carInfo.CarCertPicUrl;
                    insCarInfoOrder.CarInvoicePicKey = carInfo.CarInvoicePicKey;
                    insCarInfoOrder.CarInvoicePicUrl = carInfo.CarInvoicePicUrl;
                    insCarInfoOrder.CreateTime = this.DateTime;
                    insCarInfoOrder.Creator = operater;

                    if (customers != null)
                    {
                        var carowner = customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                        if (carowner != null)
                        {
                            insCarInfoOrder.CarownerInsuredFlag = carowner.InsuredFlag;
                            insCarInfoOrder.CarownerName = carowner.Name;
                            insCarInfoOrder.CarownerCertNo = carowner.CertNo;
                            insCarInfoOrder.CarownerMobile = carowner.Mobile;
                            insCarInfoOrder.CarownerAddress = carowner.Address;
                            insCarInfoOrder.CarownerIdentityFacePicKey = carowner.IdentityFacePicKey;
                            insCarInfoOrder.CarownerIdentityFacePicUrl = carowner.IdentityFacePicUrl;
                            insCarInfoOrder.CarownerIdentityBackPicKey = carowner.IdentityFacePicKey;
                            insCarInfoOrder.CarownerIdentityBackPicUrl = carowner.IdentityBackPicUrl;
                            insCarInfoOrder.CarownerOrgPicKey = carowner.OrgPicKey;
                        }

                        var policyholder = customers.Where(m => m.InsuredFlag == "1").FirstOrDefault();
                        if (policyholder != null)
                        {
                            insCarInfoOrder.PolicyholderInsuredFlag = policyholder.InsuredFlag;
                            insCarInfoOrder.PolicyholderName = policyholder.Name;
                            insCarInfoOrder.PolicyholderCertNo = policyholder.CertNo;
                            insCarInfoOrder.PolicyholderMobile = policyholder.Mobile;
                            insCarInfoOrder.PolicyholderAddress = policyholder.Address;
                            insCarInfoOrder.PolicyholderIdentityFacePicKey = policyholder.IdentityFacePicKey;
                            insCarInfoOrder.PolicyholderIdentityFacePicUrl = policyholder.IdentityFacePicUrl;
                            insCarInfoOrder.PolicyholderIdentityBackPicKey = policyholder.IdentityBackPicKey;
                            insCarInfoOrder.PolicyholderIdentityBackPicUrl = policyholder.IdentityBackPicUrl;
                            insCarInfoOrder.PolicyholderOrgPicKey = policyholder.OrgPicKey;
                        }
                        else
                        {
                            insCarInfoOrder.PolicyholderInsuredFlag = "1";
                            insCarInfoOrder.PolicyholderName = carowner.Name;
                            insCarInfoOrder.PolicyholderCertNo = carowner.CertNo;
                            insCarInfoOrder.PolicyholderMobile = carowner.Mobile;
                            insCarInfoOrder.PolicyholderAddress = carowner.Address;
                            insCarInfoOrder.PolicyholderIdentityFacePicKey = carowner.IdentityFacePicKey;
                            insCarInfoOrder.PolicyholderIdentityFacePicUrl = carowner.IdentityFacePicUrl;
                            insCarInfoOrder.PolicyholderIdentityBackPicKey = carowner.IdentityBackPicKey;
                            insCarInfoOrder.PolicyholderIdentityBackPicUrl = carowner.IdentityBackPicUrl;
                            insCarInfoOrder.PolicyholderOrgPicKey = carowner.OrgPicKey;
                        }


                        var insured = customers.Where(m => m.InsuredFlag == "2").FirstOrDefault();
                        if (insured != null)
                        {
                            insCarInfoOrder.InsuredInsuredFlag = insured.InsuredFlag;
                            insCarInfoOrder.InsuredName = insured.Name;
                            insCarInfoOrder.InsuredCertNo = insured.CertNo;
                            insCarInfoOrder.InsuredMobile = insured.Mobile;
                            insCarInfoOrder.InsuredAddress = insured.Address;
                            insCarInfoOrder.InsuredIdentityFacePicKey = insured.IdentityFacePicKey;
                            insCarInfoOrder.InsuredIdentityFacePicUrl = insured.IdentityFacePicUrl;
                            insCarInfoOrder.InsuredIdentityBackPicKey = insured.IdentityFacePicKey;
                            insCarInfoOrder.InsuredIdentityBackPicUrl = insured.IdentityBackPicUrl;
                            insCarInfoOrder.InsuredOrgPicKey = insured.OrgPicKey;

                        }
                        else
                        {
                            insCarInfoOrder.InsuredInsuredFlag = "2";
                            insCarInfoOrder.InsuredName = carowner.Name;
                            insCarInfoOrder.InsuredCertNo = carowner.CertNo;
                            insCarInfoOrder.InsuredMobile = carowner.Mobile;
                            insCarInfoOrder.InsuredAddress = carowner.Address;
                            insCarInfoOrder.InsuredIdentityFacePicKey = carowner.IdentityFacePicKey;
                            insCarInfoOrder.InsuredIdentityFacePicUrl = carowner.IdentityFacePicUrl;
                            insCarInfoOrder.InsuredIdentityBackPicKey = carowner.IdentityFacePicKey;
                            insCarInfoOrder.InsuredIdentityBackPicUrl = carowner.IdentityBackPicUrl;
                            insCarInfoOrder.InsuredOrgPicKey = carowner.OrgPicKey;
                        }

                    }

                    if (insCarInfoOrder.Id == 0)
                    {
                        CurrentDb.InsCarInfoOrder.Add(insCarInfoOrder);
                    }

                    CurrentDb.SaveChanges();

                    ts.Complete();

                    carInfoOrderId = insCarInfoOrder.Id;
                }

            }
            catch (Exception ex)
            {
                Log.Error("本系统基础数据保存失败1", ex);

            }

            return carInfoOrderId;

        }


        public CustomJsonResult UpdateOfferByBefore(int operater, UpdateOrderOfferPms pms)
        {
            var result = new CustomJsonResult();
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var partnerCompany = YdtDataMap.YdtInsComanyList().Where(m => m.YdtCode == pms.PartnerCompanyId).FirstOrDefault();
                    if (partnerCompany == null)
                        return new CustomJsonResult(ResultType.Failure, "找不到第三方系统的保险公司，映射无结果");

                    var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == partnerCompany.UpLinkCode).FirstOrDefault();
                    if (carInsuranceCompany == null)
                        return new CustomJsonResult(ResultType.Failure, "找不到本系统的保险公司");

                    var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == pms.UserId).FirstOrDefault();

                    if (clientUser == null)
                    {
                        return new CustomJsonResult(ResultType.Failure, "找不到本系统的用户");
                    }

                    var merchant = CurrentDb.Merchant.Where(m => m.Id == pms.MerchantId).FirstOrDefault();

                    if (merchant == null)
                    {
                        return new CustomJsonResult(ResultType.Failure, "找不到本系统的商户");
                    }

                    var insCarInfoOrder = CurrentDb.InsCarInfoOrder.Where(m => m.Id == pms.CarInfoOrderId).FirstOrDefault();
                    if (insCarInfoOrder == null)
                    {
                        return new CustomJsonResult(ResultType.Failure, "找不到本系统的订单");
                    }


                    var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.CarInfoOrderId == pms.CarInfoOrderId && m.InsCompanyId == carInsuranceCompany.Id && m.PartnerOrderId == pms.PartnerOrderId && m.UserId == pms.UserId).FirstOrDefault();

                    if (orderToCarInsure == null)
                    {
                        orderToCarInsure = new OrderToCarInsure();
                        orderToCarInsure.Type = Enumeration.OrderType.InsureForCarForInsure;
                        orderToCarInsure.TypeName = Enumeration.OrderType.InsureForCarForInsure.GetCnName();
                        orderToCarInsure.SalesmanId = merchant.SalesmanId ?? 0;
                        orderToCarInsure.AgentId = merchant.AgentId ?? 0;
                        orderToCarInsure.Status = Enumeration.OrderStatus.Submitted;
                        orderToCarInsure.SubmitTime = this.DateTime;
                        orderToCarInsure.UserId = pms.UserId;
                        orderToCarInsure.MerchantId = pms.MerchantId;
                        orderToCarInsure.PosMachineId = pms.PosMachineId;
                        orderToCarInsure.PartnerOrderId = pms.PartnerOrderId;
                        orderToCarInsure.CarInfoOrderId = pms.CarInfoOrderId;
                        orderToCarInsure.SubmitTime = this.DateTime;
                        orderToCarInsure.InsCompanyId = carInsuranceCompany.Id;
                        orderToCarInsure.InsCompanyName = carInsuranceCompany.InsuranceCompanyName;
                        orderToCarInsure.InsCompanyImgUrl = carInsuranceCompany.InsuranceCompanyImgUrl;
                        orderToCarInsure.CreateTime = this.DateTime;
                        orderToCarInsure.Creator = operater;
                    }
                    else
                    {
                        orderToCarInsure.LastUpdateTime = this.DateTime;
                        orderToCarInsure.Mender = operater;
                    }

                    orderToCarInsure.CarBelong = insCarInfoOrder.Belong;
                    orderToCarInsure.CarType = insCarInfoOrder.CarType;
                    orderToCarInsure.CarLicensePlateNo = insCarInfoOrder.LicensePlateNo;
                    orderToCarInsure.CarVin = insCarInfoOrder.Vin;
                    orderToCarInsure.CarEngineNo = insCarInfoOrder.EngineNo;
                    orderToCarInsure.CarFirstRegisterDate = insCarInfoOrder.FirstRegisterDate;
                    orderToCarInsure.CarModelCode = insCarInfoOrder.ModelCode;
                    orderToCarInsure.CarModelName = insCarInfoOrder.ModelName;
                    orderToCarInsure.CarDisplacement = insCarInfoOrder.Displacement;
                    orderToCarInsure.CarMarketYear = insCarInfoOrder.MarketYear;
                    orderToCarInsure.CarRatedPassengerCapacity = insCarInfoOrder.RatedPassengerCapacity;
                    orderToCarInsure.CarReplacementValue = insCarInfoOrder.ReplacementValue;
                    orderToCarInsure.CarChgownerType = insCarInfoOrder.ChgownerType;
                    orderToCarInsure.CarChgownerDate = insCarInfoOrder.ChgownerDate;
                    orderToCarInsure.CarTonnage = insCarInfoOrder.Tonnage;
                    orderToCarInsure.CarWholeWeight = insCarInfoOrder.WholeWeight;
                    orderToCarInsure.CarLicensePicKey = insCarInfoOrder.LicensePicKey;
                    orderToCarInsure.CarLicensePicUrl = insCarInfoOrder.LicensePicUrl;
                    orderToCarInsure.CarLicenseOtherPicKey = insCarInfoOrder.LicenseOtherPicKey;
                    orderToCarInsure.CarLicenseOtherPicUrl = insCarInfoOrder.LicenseOtherPicUrl;
                    orderToCarInsure.CarCertPicKey = insCarInfoOrder.CarCertPicKey;
                    orderToCarInsure.CarCertPicUrl = insCarInfoOrder.CarCertPicUrl;
                    orderToCarInsure.CarInvoicePicKey = insCarInfoOrder.CarInvoicePicKey;
                    orderToCarInsure.CarInvoicePicUrl = insCarInfoOrder.CarInvoicePicUrl;
                    orderToCarInsure.CarownerInsuredFlag = insCarInfoOrder.CarownerInsuredFlag;
                    orderToCarInsure.CarownerName = insCarInfoOrder.CarownerName;
                    orderToCarInsure.CarownerCertNo = insCarInfoOrder.CarownerCertNo;
                    orderToCarInsure.CarownerMobile = insCarInfoOrder.CarownerMobile;
                    orderToCarInsure.CarownerAddress = insCarInfoOrder.CarownerAddress;
                    orderToCarInsure.CarownerIdentityFacePicKey = insCarInfoOrder.CarownerIdentityFacePicKey;
                    orderToCarInsure.CarownerIdentityFacePicUrl = insCarInfoOrder.CarownerIdentityFacePicUrl;
                    orderToCarInsure.CarownerIdentityBackPicKey = insCarInfoOrder.CarownerIdentityBackPicKey;
                    orderToCarInsure.CarownerIdentityBackPicUrl = insCarInfoOrder.CarownerIdentityBackPicUrl;
                    orderToCarInsure.CarownerOrgPicKey = insCarInfoOrder.CarownerOrgPicKey;
                    orderToCarInsure.PolicyholderInsuredFlag = insCarInfoOrder.PolicyholderInsuredFlag;
                    orderToCarInsure.PolicyholderName = insCarInfoOrder.PolicyholderName;
                    orderToCarInsure.PolicyholderCertNo = insCarInfoOrder.PolicyholderCertNo;
                    orderToCarInsure.PolicyholderMobile = insCarInfoOrder.PolicyholderMobile;
                    orderToCarInsure.PolicyholderAddress = insCarInfoOrder.PolicyholderAddress;
                    orderToCarInsure.PolicyholderIdentityFacePicKey = insCarInfoOrder.PolicyholderIdentityFacePicKey;
                    orderToCarInsure.PolicyholderIdentityFacePicUrl = insCarInfoOrder.PolicyholderIdentityFacePicUrl;
                    orderToCarInsure.PolicyholderIdentityBackPicKey = insCarInfoOrder.PolicyholderIdentityBackPicKey;
                    orderToCarInsure.PolicyholderIdentityBackPicUrl = insCarInfoOrder.PolicyholderIdentityBackPicUrl;
                    orderToCarInsure.PolicyholderOrgPicKey = insCarInfoOrder.PolicyholderOrgPicKey;
                    orderToCarInsure.PolicyholderOrgPicUrl = insCarInfoOrder.PolicyholderOrgPicUrl;
                    orderToCarInsure.InsuredInsuredFlag = insCarInfoOrder.InsuredInsuredFlag;
                    orderToCarInsure.InsuredName = insCarInfoOrder.InsuredName;
                    orderToCarInsure.InsuredCertNo = insCarInfoOrder.InsuredCertNo;
                    orderToCarInsure.InsuredMobile = insCarInfoOrder.InsuredMobile;
                    orderToCarInsure.InsuredAddress = insCarInfoOrder.InsuredAddress;
                    orderToCarInsure.InsuredIdentityFacePicKey = insCarInfoOrder.InsuredIdentityFacePicKey;
                    orderToCarInsure.InsuredIdentityBackPicUrl = insCarInfoOrder.InsuredIdentityBackPicUrl;
                    orderToCarInsure.InsuredIdentityBackPicKey = insCarInfoOrder.InsuredIdentityBackPicKey;
                    orderToCarInsure.InsuredIdentityBackPicUrl = insCarInfoOrder.InsuredIdentityBackPicUrl;
                    orderToCarInsure.InsuredOrgPicKey = insCarInfoOrder.InsuredOrgPicKey;
                    orderToCarInsure.InsuredOrgPicUrl = insCarInfoOrder.InsuredOrgPicUrl;

                    if (orderToCarInsure.Id == 0)
                    {
                        CurrentDb.OrderToCarInsure.Add(orderToCarInsure);
                        CurrentDb.SaveChanges();
                        SnModel snModel = Sn.Build(SnType.OrderToCarInsure, orderToCarInsure.Id);
                        orderToCarInsure.Sn = snModel.Sn;
                        CurrentDb.SaveChanges();
                    }

                    var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == orderToCarInsure.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).FirstOrDefault();

                    if (orderToCarInsureOfferCompany == null)
                    {
                        orderToCarInsureOfferCompany = new OrderToCarInsureOfferCompany();
                        orderToCarInsureOfferCompany.OrderId = orderToCarInsure.Id;
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
                    orderToCarInsureOfferCompany.PartnerOrderId = orderToCarInsure.PartnerOrderId;
                    orderToCarInsureOfferCompany.PartnerInquiryId = pms.PartnerInquirySeq;
                    orderToCarInsureOfferCompany.PartnerCompanyId = pms.PartnerCompanyId;
                    orderToCarInsureOfferCompany.PartnerChannelId = pms.PartnerChannelId.ToString();
                    orderToCarInsureOfferCompany.PartnerRisk = pms.PartnerRisk.ToString();

                    if (orderToCarInsureOfferCompany.Id == 0)
                    {
                        CurrentDb.OrderToCarInsureOfferCompany.Add(orderToCarInsureOfferCompany);
                    }

                    CurrentDb.SaveChanges();


                    var orderToCarInsureOfferCompanyKinds = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == orderToCarInsure.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).ToList();

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
                                orderToCarInsureOfferCompanyKind.OrderId = orderToCarInsure.Id;
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

                    var l_orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.CarInfoOrderId == pms.CarInfoOrderId && m.PartnerOrderId == pms.PartnerOrderId && m.UserId == pms.UserId).FirstOrDefault();

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


                    Log.InfoFormat("(pms.OfferResult:" + (int)pms.OfferResult);


                    if (pms.OfferResult == Enumeration.OfferResult.SumbitArtificialOfferSuccess)
                    {
                        l_orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialOffer;//提交人工报价成功，等待人工报价
                    }
                    else if (pms.OfferResult == Enumeration.OfferResult.ArtificialOfferSuccess)
                    {
                        l_orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitAutoInsure;//人工报价成功，等待自动核保
                    }
                    else if (pms.OfferResult == Enumeration.OfferResult.ArtificialOfferFailure)
                    {
                        l_orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitSubmitQuotesheet;//人工报价失败，等待提交报价详细资料;
                    }
                    else if (pms.OfferResult == Enumeration.OfferResult.AutoOfferSuccess)
                    {
                        l_orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitAutoInsure;//自动报价成功，等待自动核保;
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


        public CustomJsonResult UpdateOrder(int operater, int orderId, CarInfoModel carInfo, List<CarInsCustomerModel> customers)
        {
            var result = new CustomJsonResult();
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderId).FirstOrDefault();
                    if (orderToCarInsure == null)
                    {
                        return new CustomJsonResult(ResultType.Failure, "找不到本系统的订单");
                    }


                    orderToCarInsure.CarBelong = carInfo.Belong;
                    orderToCarInsure.CarType = carInfo.CarType;
                    orderToCarInsure.CarLicensePlateNo = carInfo.LicensePlateNo;
                    orderToCarInsure.CarVin = carInfo.Vin;
                    orderToCarInsure.CarEngineNo = carInfo.EngineNo;
                    orderToCarInsure.CarFirstRegisterDate = carInfo.FirstRegisterDate;
                    orderToCarInsure.CarModelCode = carInfo.ModelCode;
                    orderToCarInsure.CarModelName = carInfo.ModelName;
                    orderToCarInsure.CarDisplacement = carInfo.Displacement;
                    orderToCarInsure.CarMarketYear = carInfo.MarketYear;
                    orderToCarInsure.CarRatedPassengerCapacity = carInfo.RatedPassengerCapacity;
                    orderToCarInsure.CarReplacementValue = carInfo.ReplacementValue;
                    orderToCarInsure.CarChgownerType = carInfo.ChgownerType;
                    orderToCarInsure.CarChgownerDate = carInfo.ChgownerDate;
                    orderToCarInsure.CarTonnage = carInfo.Tonnage;
                    orderToCarInsure.CarWholeWeight = carInfo.WholeWeight;
                    orderToCarInsure.CarLicensePicKey = carInfo.LicensePicKey;
                    orderToCarInsure.CarLicensePicUrl = carInfo.LicensePicUrl;
                    orderToCarInsure.CarLicenseOtherPicKey = carInfo.LicenseOtherPicKey;
                    orderToCarInsure.CarLicenseOtherPicUrl = carInfo.LicenseOtherPicUrl;
                    orderToCarInsure.CarCertPicKey = carInfo.CarCertPicKey;
                    orderToCarInsure.CarCertPicUrl = carInfo.CarCertPicUrl;
                    orderToCarInsure.CarInvoicePicKey = carInfo.CarInvoicePicKey;
                    orderToCarInsure.CarInvoicePicUrl = carInfo.CarInvoicePicUrl;

                    var carowner = customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                    if (carowner != null)
                    {
                        orderToCarInsure.CarownerInsuredFlag = carowner.InsuredFlag;
                        orderToCarInsure.CarownerName = carowner.Name;
                        orderToCarInsure.CarownerCertNo = carowner.CertNo;
                        orderToCarInsure.CarownerMobile = carowner.Mobile;
                        orderToCarInsure.CarownerAddress = carowner.Address;
                        orderToCarInsure.CarownerIdentityFacePicKey = carowner.IdentityFacePicKey;
                        orderToCarInsure.CarownerIdentityFacePicUrl = carowner.IdentityFacePicUrl;
                        orderToCarInsure.CarownerIdentityBackPicKey = carowner.IdentityBackPicKey;
                        orderToCarInsure.CarownerIdentityBackPicUrl = carowner.IdentityBackPicUrl;
                        orderToCarInsure.CarownerOrgPicKey = carowner.OrgPicKey;
                    }

                    var policyholder = customers.Where(m => m.InsuredFlag == "1").FirstOrDefault();
                    if (policyholder != null)
                    {
                        orderToCarInsure.PolicyholderInsuredFlag = policyholder.InsuredFlag;
                        orderToCarInsure.PolicyholderName = carowner.Name;
                        orderToCarInsure.PolicyholderCertNo = carowner.CertNo;
                        orderToCarInsure.PolicyholderMobile = carowner.Mobile;
                        orderToCarInsure.PolicyholderAddress = carowner.Address;
                        orderToCarInsure.PolicyholderIdentityFacePicKey = carowner.IdentityFacePicKey;
                        orderToCarInsure.PolicyholderIdentityFacePicUrl = carowner.IdentityFacePicUrl;
                        orderToCarInsure.PolicyholderIdentityBackPicKey = carowner.IdentityBackPicKey;
                        orderToCarInsure.PolicyholderIdentityBackPicUrl = carowner.IdentityBackPicUrl;
                        orderToCarInsure.PolicyholderOrgPicKey = carowner.OrgPicKey;
                        orderToCarInsure.PolicyholderOrgPicUrl = carowner.OrgPicUrl;
                    }

                    var insured = customers.Where(m => m.InsuredFlag == "1").FirstOrDefault();
                    if (insured != null)
                    {
                        orderToCarInsure.InsuredInsuredFlag = carowner.InsuredFlag;
                        orderToCarInsure.InsuredName = carowner.Name;
                        orderToCarInsure.InsuredCertNo = carowner.CertNo;
                        orderToCarInsure.InsuredMobile = carowner.Mobile;
                        orderToCarInsure.InsuredAddress = carowner.Address;
                        orderToCarInsure.InsuredIdentityFacePicKey = carowner.IdentityFacePicKey;
                        orderToCarInsure.InsuredIdentityBackPicUrl = carowner.IdentityBackPicUrl;
                        orderToCarInsure.InsuredIdentityBackPicKey = carowner.IdentityBackPicKey;
                        orderToCarInsure.InsuredIdentityBackPicUrl = carowner.IdentityBackPicUrl;
                        orderToCarInsure.InsuredOrgPicKey = carowner.OrgPicKey;
                        orderToCarInsure.InsuredOrgPicUrl = carowner.OrgPicUrl;
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

        public InsCarInfo UpdateCarInfo(int operater, CarInfoModel carInfo, List<CarInsCustomerModel> customers)
        {
            if (carInfo == null)
                return null;

            if (string.IsNullOrEmpty(carInfo.LicensePlateNo))
                return null;

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
            insCarInfo.LicensePicUrl = carInfo.LicensePicUrl;
            insCarInfo.LicenseOtherPicKey = carInfo.LicenseOtherPicKey;
            insCarInfo.LicenseOtherPicUrl = carInfo.LicenseOtherPicUrl;
            insCarInfo.CarCertPicKey = carInfo.CarCertPicKey;
            insCarInfo.CarCertPicUrl = carInfo.CarCertPicUrl;
            insCarInfo.CarInvoicePicKey = carInfo.CarInvoicePicKey;
            insCarInfo.CarInvoicePicUrl = carInfo.CarInvoicePicUrl;

            if (customers != null)
            {
                var carowner = customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                if (carowner != null)
                {
                    insCarInfo.CarownerInsuredFlag = carowner.InsuredFlag;
                    insCarInfo.CarownerName = carowner.Name;
                    insCarInfo.CarownerCertNo = carowner.CertNo;
                    insCarInfo.CarownerMobile = carowner.Mobile;
                    insCarInfo.CarownerAddress = carowner.Address;
                    insCarInfo.CarownerIdentityFacePicKey = carowner.IdentityFacePicKey;
                    insCarInfo.CarownerIdentityFacePicUrl = carowner.IdentityFacePicUrl;
                    insCarInfo.CarownerIdentityBackPicKey = carowner.IdentityBackPicKey;
                    insCarInfo.CarownerIdentityBackPicUrl = carowner.IdentityBackPicUrl;
                    insCarInfo.CarownerOrgPicKey = carowner.OrgPicKey;
                    insCarInfo.CarownerOrgPicUrl = carowner.OrgPicUrl;
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
                    insCarInfo.PolicyholderIdentityFacePicUrl = policyholder.IdentityFacePicUrl;
                    insCarInfo.PolicyholderIdentityBackPicKey = policyholder.IdentityFacePicKey;
                    insCarInfo.PolicyholderIdentityBackPicUrl = policyholder.IdentityFacePicUrl;
                    insCarInfo.PolicyholderOrgPicKey = policyholder.OrgPicKey;
                    insCarInfo.PolicyholderOrgPicUrl = policyholder.OrgPicUrl;
                }
                else
                {
                    insCarInfo.PolicyholderInsuredFlag = "1";
                    insCarInfo.PolicyholderName = carowner.Name;
                    insCarInfo.PolicyholderCertNo = carowner.CertNo;
                    insCarInfo.PolicyholderMobile = carowner.Mobile;
                    insCarInfo.PolicyholderAddress = carowner.Address;
                    insCarInfo.PolicyholderIdentityFacePicKey = carowner.IdentityFacePicKey;
                    insCarInfo.PolicyholderIdentityFacePicUrl = carowner.IdentityFacePicUrl;
                    insCarInfo.PolicyholderIdentityBackPicKey = carowner.IdentityFacePicKey;
                    insCarInfo.PolicyholderIdentityBackPicUrl = carowner.IdentityFacePicUrl;
                    insCarInfo.PolicyholderOrgPicKey = carowner.OrgPicKey;
                    insCarInfo.PolicyholderOrgPicUrl = carowner.OrgPicUrl;
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
                    insCarInfo.InsuredIdentityFacePicUrl = insured.IdentityFacePicUrl;
                    insCarInfo.InsuredIdentityBackPicKey = insured.IdentityFacePicKey;
                    insCarInfo.InsuredIdentityBackPicUrl = insured.IdentityFacePicUrl;
                    insCarInfo.InsuredOrgPicKey = insured.OrgPicKey;
                    insCarInfo.InsuredOrgPicUrl = insured.OrgPicUrl;

                }
                else
                {
                    insCarInfo.InsuredInsuredFlag = "2";
                    insCarInfo.InsuredName = carowner.Name;
                    insCarInfo.InsuredCertNo = carowner.CertNo;
                    insCarInfo.InsuredMobile = carowner.Mobile;
                    insCarInfo.InsuredAddress = carowner.Address;
                    insCarInfo.InsuredIdentityFacePicKey = carowner.IdentityFacePicKey;
                    insCarInfo.InsuredIdentityFacePicUrl = carowner.IdentityFacePicUrl;
                    insCarInfo.InsuredIdentityBackPicKey = carowner.IdentityFacePicKey;
                    insCarInfo.InsuredIdentityBackPicUrl = carowner.IdentityFacePicUrl;
                    insCarInfo.InsuredOrgPicKey = carowner.OrgPicKey;
                    insCarInfo.InsuredOrgPicUrl = carowner.OrgPicUrl;
                }
            }

            if (insCarInfo.Id == 0)
            {
                CurrentDb.InsCarInfo.Add(insCarInfo);
            }

            CurrentDb.SaveChanges();

            return insCarInfo;

        }
    }
}
