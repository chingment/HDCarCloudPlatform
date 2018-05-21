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
                    insCarInfoOrder.LicenseOtherPicKey = carInfo.LicenseOtherPicKey;
                    insCarInfoOrder.CarCertPicKey = carInfo.CarCertPicKey;
                    insCarInfoOrder.CarInvoicePicKey = carInfo.CarInvoicePicKey;
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
                            insCarInfoOrder.CarownerIdentityBackPicKey = carowner.IdentityFacePicKey;
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
                            insCarInfoOrder.PolicyholderIdentityBackPicKey = policyholder.IdentityFacePicKey;
                            insCarInfoOrder.PolicyholderOrgPicKey = policyholder.OrgPicKey;
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
                            insCarInfoOrder.InsuredIdentityBackPicKey = insured.IdentityFacePicKey;
                            insCarInfoOrder.InsuredOrgPicKey = insured.OrgPicKey;

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
                    var merchant = CurrentDb.Merchant.Where(m => m.Id == pms.MerchantId).FirstOrDefault();
                    var insCarInfoOrder = CurrentDb.InsCarInfoOrder.Where(m => m.Id == pms.CarInfoOrderId).FirstOrDefault();
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
                    orderToCarInsure.CarLicenseOtherPicKey = insCarInfoOrder.LicenseOtherPicKey;
                    orderToCarInsure.CarCertPicKey = insCarInfoOrder.CarCertPicKey;
                    orderToCarInsure.CarInvoicePicKey = insCarInfoOrder.CarInvoicePicKey;
                    orderToCarInsure.CarownerInsuredFlag = insCarInfoOrder.CarownerInsuredFlag;
                    orderToCarInsure.CarownerName = insCarInfoOrder.CarownerName;
                    orderToCarInsure.CarownerCertNo = insCarInfoOrder.CarownerCertNo;
                    orderToCarInsure.CarownerMobile = insCarInfoOrder.CarownerMobile;
                    orderToCarInsure.CarownerAddress = insCarInfoOrder.CarownerAddress;
                    orderToCarInsure.CarownerIdentityFacePicKey = insCarInfoOrder.CarownerIdentityFacePicKey;
                    orderToCarInsure.CarownerIdentityBackPicKey = insCarInfoOrder.CarownerIdentityBackPicKey;
                    orderToCarInsure.CarownerOrgPicKey = insCarInfoOrder.CarownerOrgPicKey;
                    orderToCarInsure.PolicyholderInsuredFlag = insCarInfoOrder.PolicyholderInsuredFlag;
                    orderToCarInsure.PolicyholderName = insCarInfoOrder.PolicyholderName;
                    orderToCarInsure.PolicyholderCertNo = insCarInfoOrder.PolicyholderCertNo;
                    orderToCarInsure.PolicyholderMobile = insCarInfoOrder.PolicyholderMobile;
                    orderToCarInsure.PolicyholderAddress = insCarInfoOrder.PolicyholderAddress;
                    orderToCarInsure.PolicyholderIdentityFacePicKey = insCarInfoOrder.PolicyholderIdentityFacePicKey;
                    orderToCarInsure.PolicyholderIdentityBackPicKey = insCarInfoOrder.PolicyholderIdentityBackPicKey;
                    orderToCarInsure.PolicyholderOrgPicKey = insCarInfoOrder.PolicyholderOrgPicKey;
                    orderToCarInsure.InsuredInsuredFlag = insCarInfoOrder.InsuredInsuredFlag;
                    orderToCarInsure.InsuredName = insCarInfoOrder.InsuredName;
                    orderToCarInsure.InsuredCertNo = insCarInfoOrder.InsuredCertNo;
                    orderToCarInsure.InsuredMobile = insCarInfoOrder.InsuredMobile;
                    orderToCarInsure.InsuredAddress = insCarInfoOrder.InsuredAddress;
                    orderToCarInsure.InsuredIdentityFacePicKey = insCarInfoOrder.InsuredIdentityFacePicKey;
                    orderToCarInsure.InsuredIdentityBackPicKey = insCarInfoOrder.InsuredIdentityBackPicKey;
                    orderToCarInsure.InsuredOrgPicKey = insCarInfoOrder.InsuredOrgPicKey;

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
            insCarInfo.LicenseOtherPicKey = carInfo.LicenseOtherPicKey;
            insCarInfo.CarCertPicKey = carInfo.CarCertPicKey;
            insCarInfo.CarInvoicePicKey = carInfo.CarInvoicePicKey;

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
                    insCarInfo.CarownerIdentityBackPicKey = carowner.IdentityFacePicKey;
                    insCarInfo.CarownerOrgPicKey = carowner.OrgPicKey;
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

            return insCarInfo;

        }
    }
}
