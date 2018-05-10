﻿using Lumos.Entity;
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
    }

    public class InsCarProvider : BaseProvider
    {
        public void UpdateOrder(int operater, string partnerOrderId, CarInfoModel carInfo, List<CarInsCustomerModel> customers)
        {
            OrderToCarInsureAuto l_orderToCarInsureAuto = null;

            l_orderToCarInsureAuto = CurrentDb.OrderToCarInsureAuto.Where(m => m.PartnerOrderId == partnerOrderId).FirstOrDefault();

            if (l_orderToCarInsureAuto == null)
            {
                l_orderToCarInsureAuto = new OrderToCarInsureAuto();

                l_orderToCarInsureAuto.PartnerOrderId = partnerOrderId;
                l_orderToCarInsureAuto.SubmitTime = this.DateTime;
                l_orderToCarInsureAuto.CreateTime = this.DateTime;
                l_orderToCarInsureAuto.Creator = operater;
            }
            else
            {
                l_orderToCarInsureAuto.LastUpdateTime = this.DateTime;
                l_orderToCarInsureAuto.Mender = operater;
            }

            l_orderToCarInsureAuto.Belong = carInfo.Belong;
            l_orderToCarInsureAuto.CarType = carInfo.CarType;
            l_orderToCarInsureAuto.LicensePlateNo = carInfo.LicensePlateNo;
            l_orderToCarInsureAuto.Vin = carInfo.Vin;
            l_orderToCarInsureAuto.EngineNo = carInfo.EngineNo;
            l_orderToCarInsureAuto.FirstRegisterDate = carInfo.FirstRegisterDate;
            l_orderToCarInsureAuto.ModelCode = carInfo.ModelCode;
            l_orderToCarInsureAuto.ModelName = carInfo.ModelName;
            l_orderToCarInsureAuto.Displacement = carInfo.Displacement;
            l_orderToCarInsureAuto.MarketYear = carInfo.MarketYear;
            l_orderToCarInsureAuto.RatedPassengerCapacity = carInfo.RatedPassengerCapacity;
            l_orderToCarInsureAuto.ReplacementValue = carInfo.ReplacementValue;
            l_orderToCarInsureAuto.ChgownerType = carInfo.ChgownerType;
            l_orderToCarInsureAuto.ChgownerDate = carInfo.ChgownerDate;
            l_orderToCarInsureAuto.Tonnage = carInfo.Tonnage;
            l_orderToCarInsureAuto.WholeWeight = carInfo.WholeWeight;
            l_orderToCarInsureAuto.LicensePicKey = carInfo.LicensePicKey;
            l_orderToCarInsureAuto.LicenseOtherPicKey = carInfo.LicenseOtherPicKey;
            l_orderToCarInsureAuto.CarCertPicKey = carInfo.CarCertPicKey;
            l_orderToCarInsureAuto.CarInvoicePicKey = carInfo.CarInvoicePicKey;

            if (customers != null)
            {
                var carowner = customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                if (carowner != null)
                {
                    l_orderToCarInsureAuto.CarownerInsuredFlag = carowner.InsuredFlag;
                    l_orderToCarInsureAuto.CarowneName = carowner.Name;
                    l_orderToCarInsureAuto.CarowneCertNo = carowner.CertNo;
                    l_orderToCarInsureAuto.CarowneMobile = carowner.Mobile;
                    l_orderToCarInsureAuto.CarowneAddress = carowner.Address;
                    l_orderToCarInsureAuto.CarowneIdentityFacePicKey = carowner.IdentityFacePicKey;
                    l_orderToCarInsureAuto.CarowneIdentityBackPicKey = carowner.IdentityFacePicKey;
                    l_orderToCarInsureAuto.CarowneOrgPicKey = carowner.OrgPicKey;
                }

                var policyholder = customers.Where(m => m.InsuredFlag == "1").FirstOrDefault();
                if (policyholder != null)
                {
                    l_orderToCarInsureAuto.PolicyholderInsuredFlag = policyholder.InsuredFlag;
                    l_orderToCarInsureAuto.PolicyholderName = policyholder.Name;
                    l_orderToCarInsureAuto.PolicyholderCertNo = policyholder.CertNo;
                    l_orderToCarInsureAuto.PolicyholderMobile = policyholder.Mobile;
                    l_orderToCarInsureAuto.PolicyholderAddress = policyholder.Address;
                    l_orderToCarInsureAuto.PolicyholderIdentityFacePicKey = policyholder.IdentityFacePicKey;
                    l_orderToCarInsureAuto.PolicyholderIdentityBackPicKey = policyholder.IdentityFacePicKey;
                    l_orderToCarInsureAuto.PolicyholderOrgPicKey = policyholder.OrgPicKey;
                }


                var insured = customers.Where(m => m.InsuredFlag == "2").FirstOrDefault();
                if (insured != null)
                {
                    l_orderToCarInsureAuto.InsuredInsuredFlag = insured.InsuredFlag;
                    l_orderToCarInsureAuto.InsuredName = insured.Name;
                    l_orderToCarInsureAuto.InsuredCertNo = insured.CertNo;
                    l_orderToCarInsureAuto.InsuredMobile = insured.Mobile;
                    l_orderToCarInsureAuto.InsuredAddress = insured.Address;
                    l_orderToCarInsureAuto.InsuredIdentityFacePicKey = insured.IdentityFacePicKey;
                    l_orderToCarInsureAuto.InsuredIdentityBackPicKey = insured.IdentityFacePicKey;
                    l_orderToCarInsureAuto.InsuredOrgPicKey = insured.OrgPicKey;

                }
            }


            if (l_orderToCarInsureAuto.Id == 0)
            {
                CurrentDb.OrderToCarInsureAuto.Add(l_orderToCarInsureAuto);
            }

            CurrentDb.SaveChanges();

        }



        public void UpdateOrderOffer(int operater, UpdateOrderOfferPms pms)
        {

            if (pms == null)
                return;

            if (string.IsNullOrEmpty(pms.PartnerOrderId))
                return;

            using (TransactionScope ts = new TransactionScope())
            {
                var l_OrderToCarInsureAuto = CurrentDb.OrderToCarInsureAuto.Where(m => m.PartnerOrderId == pms.PartnerOrderId).FirstOrDefault();



                var partnerCompany = YdtDataMap.YdtInsComanyList().Where(m => m.YdtCode == pms.PartnerCompanyId).FirstOrDefault();

                if (partnerCompany == null)
                    return;

                var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == partnerCompany.UpLinkCode).FirstOrDefault();
                if (carInsuranceCompany == null)
                    return;


                var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == l_OrderToCarInsureAuto.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).FirstOrDefault();

                if (orderToCarInsureOfferCompany == null)
                {
                    orderToCarInsureOfferCompany = new OrderToCarInsureOfferCompany();
                    orderToCarInsureOfferCompany.OrderId = l_OrderToCarInsureAuto.Id;
                    orderToCarInsureOfferCompany.CreateTime = this.DateTime;
                    orderToCarInsureOfferCompany.Creator = operater;
                }
                else
                {
                    orderToCarInsureOfferCompany.LastUpdateTime = this.DateTime;
                    orderToCarInsureOfferCompany.Mender = operater;
                }

                orderToCarInsureOfferCompany.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                orderToCarInsureOfferCompany.InsuranceCompanyName = carInsuranceCompany.InsuranceCompanyName;
                orderToCarInsureOfferCompany.InsuranceCompanyImgUrl = carInsuranceCompany.InsuranceCompanyImgUrl;
                orderToCarInsureOfferCompany.BiStartDate = pms.BiStartDate;
                orderToCarInsureOfferCompany.CiStartDate = pms.CiStartDate;
                orderToCarInsureOfferCompany.PartnerOrderId = l_OrderToCarInsureAuto.PartnerOrderId;
                orderToCarInsureOfferCompany.PartnerInquiryId = pms.PartnerInquirySeq;
                orderToCarInsureOfferCompany.PartnerCompanyId = pms.PartnerCompanyId;
                orderToCarInsureOfferCompany.PartnerChannelId = pms.PartnerChannelId.ToString();
                orderToCarInsureOfferCompany.PartnerRisk = pms.PartnerRisk.ToString();


                if (orderToCarInsureOfferCompany.Id == 0)
                {
                    CurrentDb.OrderToCarInsureOfferCompany.Add(orderToCarInsureOfferCompany);
                }

                CurrentDb.SaveChanges();

                var orderToCarInsureOfferCompanyKinds = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == l_OrderToCarInsureAuto.Id && m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).ToList();

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
                            orderToCarInsureOfferCompanyKind.OrderId = l_OrderToCarInsureAuto.Id;
                            orderToCarInsureOfferCompanyKind.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                            orderToCarInsureOfferCompanyKind.KindId = partnerKind.UpLinkCode;
                            orderToCarInsureOfferCompanyKind.PartnerKindId = partnerKind.Code;
                            orderToCarInsureOfferCompanyKind.Compensation = item.compensation;
                            orderToCarInsureOfferCompanyKind.KindName = partnerKind.Name;
                            orderToCarInsureOfferCompanyKind.Quantity = item.quantity;
                            orderToCarInsureOfferCompanyKind.GlassType = item.glassType;
                            orderToCarInsureOfferCompanyKind.BasicPremium = item.basicPremium;
                            orderToCarInsureOfferCompanyKind.Discount = item.discount;
                            orderToCarInsureOfferCompanyKind.StandardPremium = item.standardPremium;
                            orderToCarInsureOfferCompanyKind.Amount = item.amount;
                            orderToCarInsureOfferCompanyKind.Premium = item.premium;
                            orderToCarInsureOfferCompanyKind.Creator = operater;
                            orderToCarInsureOfferCompanyKind.CreateTime = this.DateTime;
                            CurrentDb.OrderToCarInsureOfferCompanyKind.Add(orderToCarInsureOfferCompanyKind);
                            CurrentDb.SaveChanges();
                        }
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();
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
