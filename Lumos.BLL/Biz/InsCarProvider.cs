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
        public string PartnerInquiryId { get; set; }
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
                    insCarInfoOrder.BiEndDate = carInfo.BiEndDate;
                    insCarInfoOrder.BiStartDate = carInfo.BiStartDate;
                    insCarInfoOrder.CiEndDate = carInfo.CiEndDate;
                    insCarInfoOrder.CiStartDate = carInfo.CiStartDate;
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
                LogUtil.Error("本系统基础数据保存失败1", ex);

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
                        orderToCarInsure.OrderFrom = Enumeration.OrderFrom.Ydt;
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
                        orderToCarInsure.InsCompanyId = carInsuranceCompany.InsuranceCompanyId;
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
                    orderToCarInsure.PartnerRisk = pms.PartnerRisk.ToString();
                    orderToCarInsure.IsInvisiable = true;
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
                    orderToCarInsureOfferCompany.PartnerInquiryId = pms.PartnerInquiryId;
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


                    if (pms.PartnerRisk == 2 || pms.PartnerRisk == 3)
                    {
                        var compulsory_kind = new OrderToCarInsureOfferCompanyKind();
                        compulsory_kind.OrderId = orderToCarInsure.Id;
                        compulsory_kind.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                        compulsory_kind.KindId = 1;
                        compulsory_kind.PartnerKindId = "0";
                        compulsory_kind.KindName = "交强险";
                        compulsory_kind.Quantity = 0;
                        compulsory_kind.GlassType = 0;
                        compulsory_kind.Amount = 0;
                        compulsory_kind.StandardPremium = 0;//保费
                        compulsory_kind.Priority = 0;
                        compulsory_kind.Creator = operater;
                        compulsory_kind.CreateTime = this.DateTime;
                        compulsory_kind.IsWaiverDeductible = false;
                        compulsory_kind.IsWaiverDeductibleKind = false;
                        compulsory_kind.KindValue = "";
                        CurrentDb.OrderToCarInsureOfferCompanyKind.Add(compulsory_kind);
                        CurrentDb.SaveChanges();

                        var travelTax_Kind = new OrderToCarInsureOfferCompanyKind();
                        travelTax_Kind.OrderId = orderToCarInsure.Id;
                        travelTax_Kind.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                        travelTax_Kind.KindId = 2;
                        travelTax_Kind.PartnerKindId = "0";
                        travelTax_Kind.KindName = "车船税";
                        travelTax_Kind.Quantity = 0;
                        travelTax_Kind.GlassType = 0;
                        travelTax_Kind.Amount = 0;
                        travelTax_Kind.StandardPremium = 0;
                        travelTax_Kind.Priority = 0;
                        travelTax_Kind.Creator = operater;
                        travelTax_Kind.CreateTime = this.DateTime;
                        travelTax_Kind.IsWaiverDeductible = false;
                        travelTax_Kind.IsWaiverDeductibleKind = false;
                        travelTax_Kind.KindValue = "";

                        CurrentDb.OrderToCarInsureOfferCompanyKind.Add(travelTax_Kind);
                        CurrentDb.SaveChanges();
                    }


                    if (pms.Coverages != null)
                    {
                        var mapCoverages = YdtDataMap.YdtInsCoverageList();
                        foreach (var item in pms.Coverages)
                        {
                            var partnerKind = mapCoverages.Where(m => m.Code == item.code).FirstOrDefault();
                            if (partnerKind != null)
                            {
                                var kind = new OrderToCarInsureOfferCompanyKind();
                                kind.OrderId = orderToCarInsure.Id;
                                kind.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                                kind.KindId = partnerKind.UpLinkCode;
                                kind.PartnerKindId = partnerKind.Code;
                                kind.KindName = partnerKind.Name;
                                kind.Quantity = item.quantity;
                                kind.GlassType = item.glassType;
                                kind.Amount = item.amount ?? 0;
                                kind.Creator = operater;
                                kind.CreateTime = this.DateTime;
                                kind.IsWaiverDeductible = item.compensation == 0 ? false : true;//是否有不计免费险
                                kind.IsWaiverDeductibleKind = partnerKind.IsWaiverDeductibleKind;

                                kind.Priority = partnerKind.Priority;
                                CurrentDb.OrderToCarInsureOfferCompanyKind.Add(kind);
                                CurrentDb.SaveChanges();

                                if (kind.IsWaiverDeductible)
                                {
                                    var parner_wd_kind = mapCoverages.Where(m => m.Code == partnerKind.WaiverDeductibleKindCode).FirstOrDefault();
                                    var parner_wd_kindOffer = pms.Coverages.Where(m => m.code == partnerKind.WaiverDeductibleKindCode).FirstOrDefault();
                                    if (parner_wd_kind != null && parner_wd_kindOffer != null)
                                    {
                                        var wd_kind = new OrderToCarInsureOfferCompanyKind();
                                        wd_kind.OrderId = orderToCarInsure.Id;
                                        wd_kind.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                                        wd_kind.KindId = parner_wd_kind.UpLinkCode;
                                        wd_kind.PartnerKindId = parner_wd_kind.Code;
                                        wd_kind.KindName = parner_wd_kind.Name;
                                        wd_kind.Quantity = parner_wd_kindOffer.quantity;
                                        wd_kind.GlassType = parner_wd_kindOffer.glassType;
                                        wd_kind.Amount = parner_wd_kindOffer.amount ?? 0;
                                        wd_kind.Creator = operater;
                                        wd_kind.CreateTime = this.DateTime;
                                        wd_kind.IsWaiverDeductible = false;
                                        wd_kind.IsWaiverDeductibleKind = partnerKind.IsWaiverDeductibleKind;
                                        wd_kind.Priority = parner_wd_kind.Priority;
                                        CurrentDb.OrderToCarInsureOfferCompanyKind.Add(wd_kind);
                                        CurrentDb.SaveChanges();
                                    }

                                }
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
                LogUtil.Error("本系统基础数据保存失败1", ex);
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


                    var partnerCompany = YdtDataMap.YdtInsComanyList().Where(m => m.YdtCode == pms.PartnerCompanyId).FirstOrDefault();

                    if (partnerCompany == null)
                    {
                        LogUtil.Info("partnerCompany 为空");
                    }

                    var l_orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.CarInfoOrderId == pms.CarInfoOrderId && m.PartnerOrderId == pms.PartnerOrderId && m.UserId == pms.UserId && m.InsCompanyId == partnerCompany.UpLinkCode).FirstOrDefault();

                    if (l_orderToCarInsure == null)
                    {
                        LogUtil.Info("l_orderToCarInsure 为空");
                    }

                    l_orderToCarInsure.PartnerInquiryId = pms.PartnerInquiryId;
                    l_orderToCarInsure.IsAuto = pms.Auto == 0 ? false : true;
                    if (pms.Auto == 0)
                    {
                        l_orderToCarInsure.IsInvisiable = false;
                    }



                    var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == partnerCompany.UpLinkCode).FirstOrDefault();

                    if (carInsuranceCompany == null)
                    {
                        LogUtil.Info("carInsuranceCompany 为空");
                    }

                    var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == l_orderToCarInsure.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).FirstOrDefault();

                    if (orderToCarInsureOfferCompany == null)
                    {
                        LogUtil.Info("orderToCarInsureOfferCompany 为空，partnerCompany.UpLinkCode:" + partnerCompany.UpLinkCode + ",OrderId:" + l_orderToCarInsure.Id + ",InsuranceCompanyId:" + carInsuranceCompany.InsuranceCompanyId);
                    }

                    orderToCarInsureOfferCompany.IsAuto = pms.Auto == 0 ? false : true;
                    orderToCarInsureOfferCompany.OfferResult = pms.OfferResult;
                    orderToCarInsureOfferCompany.PartnerInquiryId = pms.PartnerInquiryId;
                    orderToCarInsureOfferCompany.LastUpdateTime = this.DateTime;
                    orderToCarInsureOfferCompany.Mender = operater;

                    var orderToCarInsureOfferCompanyKinds = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == l_orderToCarInsure.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).ToList();

                    foreach (var item in orderToCarInsureOfferCompanyKinds)
                    {
                        CurrentDb.OrderToCarInsureOfferCompanyKind.Remove(item);
                        CurrentDb.SaveChanges();
                    }

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


                        decimal? compulsoryPrice = 0;
                        decimal? travelTaxPrice = 0;
                        if (compulsory != null)
                        {
                            if (compulsory.standardPremium != null)
                            {
                                if (compulsory.standardPremium.Value > 0)
                                {
                                    compulsoryPrice = compulsory.standardPremium;
                                    orderToCarInsureOfferCompany.CompulsoryPrice = compulsory.standardPremium;
                                    insureTotalPrice += compulsory.standardPremium.Value;

                                }
                            }

                            if (compulsory.sumPayTax != null)
                            {
                                if (compulsory.sumPayTax.Value > 0)
                                {
                                    travelTaxPrice = compulsory.sumPayTax;
                                    orderToCarInsureOfferCompany.TravelTaxPrice = compulsory.sumPayTax;
                                    insureTotalPrice += compulsory.sumPayTax.Value;
                                }
                            }

                        }


                        l_orderToCarInsure.Price = insureTotalPrice;
                        orderToCarInsureOfferCompany.InsureTotalPrice = insureTotalPrice;
                        CurrentDb.SaveChanges();

                        if (pms.PartnerRisk == 2 || pms.PartnerRisk == 3)
                        {
                            var kind_Compulsory = new OrderToCarInsureOfferCompanyKind();
                            kind_Compulsory.OrderId = l_orderToCarInsure.Id;
                            kind_Compulsory.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                            kind_Compulsory.KindId = 1;
                            kind_Compulsory.PartnerKindId = "0";
                            kind_Compulsory.KindName = "交强险";
                            kind_Compulsory.Quantity = 0;
                            kind_Compulsory.GlassType = 0;
                            kind_Compulsory.Amount = 0;
                            kind_Compulsory.StandardPremium = (compulsoryPrice == null ? 0 : compulsoryPrice.Value);//保费
                            kind_Compulsory.Priority = 0;
                            kind_Compulsory.Creator = operater;
                            kind_Compulsory.CreateTime = this.DateTime;
                            kind_Compulsory.IsWaiverDeductible = false;
                            kind_Compulsory.KindValue = "";
                            CurrentDb.OrderToCarInsureOfferCompanyKind.Add(kind_Compulsory);
                            CurrentDb.SaveChanges();



                            var kind_Tax = new OrderToCarInsureOfferCompanyKind();
                            kind_Tax.OrderId = l_orderToCarInsure.Id;
                            kind_Tax.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                            kind_Tax.KindId = 2;
                            kind_Tax.PartnerKindId = "0";
                            kind_Tax.KindName = "车船税";
                            kind_Tax.Quantity = 0;
                            kind_Tax.GlassType = 0;
                            kind_Tax.Amount = 0;
                            kind_Tax.StandardPremium = (travelTaxPrice == null ? 0 : travelTaxPrice.Value);//保费 
                            kind_Tax.Priority = 0;
                            kind_Tax.Creator = operater;
                            kind_Tax.CreateTime = this.DateTime;
                            kind_Tax.IsWaiverDeductible = false;
                            kind_Tax.KindValue = "";
                            CurrentDb.OrderToCarInsureOfferCompanyKind.Add(kind_Tax);
                            CurrentDb.SaveChanges();
                        }
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
                                orderToCarInsureOfferCompanyKind.KindName = partnerKind.Name;
                                switch (orderToCarInsureOfferCompanyKind.KindId)
                                {
                                    case 1:
                                        orderToCarInsureOfferCompanyKind.KindUnit = "";
                                        break;
                                    case 2:
                                        orderToCarInsureOfferCompanyKind.KindUnit = "";
                                        break;
                                    case 6:
                                        int quantity = item.quantity == null ? 0 : item.quantity.Value;
                                        decimal unitAmount = item.unitAmount == null ? 0 : item.unitAmount.Value;
                                        orderToCarInsureOfferCompanyKind.KindDetails = string.Format("{0}座，每座：{1}元，合计：{2}元", quantity, unitAmount, item.amount);
                                        orderToCarInsureOfferCompanyKind.KindValue = item.amount.ToF2Price();
                                        orderToCarInsureOfferCompanyKind.KindUnit = "元";
                                        break;
                                    case 8:
                                        orderToCarInsureOfferCompanyKind.GlassType = item.glassType;
                                        if (item.glassType == 1)
                                        {
                                            orderToCarInsureOfferCompanyKind.KindValue = "国产";
                                        }
                                        else if (item.glassType == 2)
                                        {
                                            orderToCarInsureOfferCompanyKind.KindValue = "进口";
                                        }

                                        orderToCarInsureOfferCompanyKind.KindUnit = "";

                                        break;
                                    default:
                                        orderToCarInsureOfferCompanyKind.KindValue = item.amount.ToF2Price();
                                        orderToCarInsureOfferCompanyKind.KindUnit = "元";
                                        break;
                                }
                                orderToCarInsureOfferCompanyKind.Quantity = item.quantity;
                                orderToCarInsureOfferCompanyKind.Amount = item.amount ?? 0;
                                orderToCarInsureOfferCompanyKind.StandardPremium = item.standardPremium ?? 0;
                                orderToCarInsureOfferCompanyKind.BasicPremium = item.basicPremium ?? 0;
                                orderToCarInsureOfferCompanyKind.Premium = item.premium;
                                orderToCarInsureOfferCompanyKind.UnitAmount = item.unitAmount;
                                orderToCarInsureOfferCompanyKind.Discount = item.discount ?? 0;
                                orderToCarInsureOfferCompanyKind.IsWaiverDeductibleKind = partnerKind.IsWaiverDeductibleKind;
                                if (partnerKind.IsWaiverDeductibleKind)
                                {
                                    orderToCarInsureOfferCompanyKind.IsWaiverDeductible = false;
                                }
                                else
                                {
                                    orderToCarInsureOfferCompanyKind.IsWaiverDeductible = GetIsWaiverDeductible(pms.Coverages, item.code);
                                }
                                orderToCarInsureOfferCompanyKind.Priority = partnerKind.Priority;
                                orderToCarInsureOfferCompanyKind.Creator = operater;
                                orderToCarInsureOfferCompanyKind.CreateTime = this.DateTime;



                                CurrentDb.OrderToCarInsureOfferCompanyKind.Add(orderToCarInsureOfferCompanyKind);
                                CurrentDb.SaveChanges();

                                out_carInsureOfferCompanyKinds.Add(orderToCarInsureOfferCompanyKind);
                            }
                        }
                    }


                    LogUtil.InfoFormat("pms.OfferResult:" + (int)pms.OfferResult);


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
                LogUtil.Error("本系统基础数据保存失败2", ex);

                string err = string.Empty;
                err += "错误日期:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n"; ;
                err += "异常信息：" + ex.Message + "\r\n";
                err += "Source:" + ex.Source + "\r\n";
                err += "StackTrace:" + ex.StackTrace + "\r\n\r\n";
                err += "-----------------------------------------------------------\r\n\r\n";

                LogUtil.Error(err);
                return new CustomJsonResult<UpdateOfferByAfterResult>(ResultType.Failure, ResultCode.Failure, "本系统基础数据保存失败2", null);
            }
        }

        public bool GetIsWaiverDeductible(List<CoverageModel> coverages, string code)
        {
            //List<YdtInscarCoveragesModel> list = new List<YdtInscarCoveragesModel>();
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 3, Code = "001", Name = "车损险", IsWaiverDeductibleKind = false, Priority = 1 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 4, Code = "002", Name = "三者险", IsWaiverDeductibleKind = false, Priority = 2 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 5, Code = "003", Name = "司机险", IsWaiverDeductibleKind = false, Priority = 3 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 6, Code = "004", Name = "乘客险", IsWaiverDeductibleKind = false, Priority = 4 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 7, Code = "005", Name = "盗抢险", IsWaiverDeductibleKind = false, Priority = 5 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 9, Code = "007", Name = "划痕险", IsWaiverDeductibleKind = false, Priority = 7 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 12, Code = "008", Name = "自燃险", IsWaiverDeductibleKind = false, Priority = 8 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 11, Code = "009", Name = "涉水险", IsWaiverDeductibleKind = false, Priority = 9 });

            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 20, Code = "101", Name = "车损险不计免赔", IsWaiverDeductibleKind = true, Priority = 12 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 21, Code = "102", Name = "三者险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            //list.Add(new YdtInscarCoveragesModel { UpLinkCode = 22, Code = "112", Name = "车上人员不计免赔", IsWaiverDeductibleKind = true, Priority = 14 });

            int count = 0;
            switch (code)
            {
                case "001":

                    count = coverages.Where(m => m.code == "101").Count();

                    break;
                case "002":
                    count = coverages.Where(m => m.code == "102").Count();
                    break;
                case "003":
                    count = coverages.Where(m => m.code == "112").Count();
                    break;
                case "004":
                    break;
                case "005":
                    break;
                case "007":
                    break;
                case "008":
                    break;
                case "009":
                    break;
            }
            //  List<CoverageModel> Coverages
            return true;
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
                LogUtil.Error("本系统基础数据保存失败1", ex);
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
            insCarInfo.BiEndDate = carInfo.BiEndDate;
            insCarInfo.BiStartDate = carInfo.BiStartDate;
            insCarInfo.CiEndDate = carInfo.CiEndDate;
            insCarInfo.CiStartDate = carInfo.CiStartDate;

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
