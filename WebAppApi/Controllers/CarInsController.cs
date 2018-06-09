﻿using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.Common;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAppApi.Models;
using WebAppApi.Models.Account;
using WebAppApi.Models.CarService;
using YdtSdk;

namespace WebAppApi.Controllers
{

    public class CarInsUploadImgPms
    {
        public string Type { get; set; }
        public ImageModel ImgData { get; set; }
    }


    [BaseAuthorizeAttribute]
    public class CarInsController : OwnBaseApiController
    {

        [HttpPost]
        public APIResponse UploadImg(CarInsUploadImgPms pms)
        {

            string imgurl = "";

            //string imgurl = GetUploadImageUrl(pms.ImgData, "CarInsure");
            object model = null;
            switch (pms.Type)
            {
                case "1":
                    imgurl = "http://file.gzhaoyilian.com/Upload/c1.jpg";
                    model = YdtUtils.UploadImg(imgurl);
                    break;
                case "10":
                    imgurl = "http://file.gzhaoyilian.com/Upload/c2.jpg";
                    model = YdtUtils.GetLicenseInfoByUrl(imgurl);
                    break;
                case "11":
                    imgurl = "http://file.gzhaoyilian.com/Upload/c1.jpg";
                    model = YdtUtils.GetIdentityInfoByUrl(imgurl);
                    break;
            }


            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", model);
        }

        [HttpPost]
        public APIResponse GetCarInfo(CarInfoPms pms)
        {

            var carInfoResult = new CarInfoResult();

            var carInfo = new CarInfoModel();
            var customers = new List<CarInsCustomerModel>();
            string licensePlateNo = "";
            DrivingLicenceInfo drivingLicenceInfo = null; ;
            switch (pms.KeywordType)
            {
                case KeyWordType.LicenseImg:

                    string[] arr_Keyword = pms.Keyword.Split('@');
                    ImageModel imgModel = new ImageModel();
                    imgModel.Data = arr_Keyword[0];
                    imgModel.Type = arr_Keyword[1];
                    string imgurl = GetUploadImageUrl(imgModel, "CarInsure");
                    drivingLicenceInfo = BizFactory.CarInsureOffer.GetDrivingLicenceInfoFromImgUrl(imgurl);
                    if (drivingLicenceInfo == null)
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "无法识别图片或请输入车牌号码查询");
                    }

                    if (string.IsNullOrEmpty(drivingLicenceInfo.plateNum))
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "无法识别图片或请输入车牌号码查询");
                    }

                    licensePlateNo = drivingLicenceInfo.plateNum;

                    break;
                case KeyWordType.LicensePlateNo:

                    if (string.IsNullOrEmpty(pms.Keyword))
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "请输入车牌号码");
                    }

                    licensePlateNo = pms.Keyword;
                    break;
                default:
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "未知关键字类型");
            }

            carInfo.LicensePlateNo = licensePlateNo;


            var insCarInfo = CurrentDb.InsCarInfo.Where(m => m.LicensePlateNo == licensePlateNo).FirstOrDefault();
            if (insCarInfo == null)
            {
                var ydtInsCarApiSearchResultData = YdtUtils.GetInsCarInfo(licensePlateNo);

                if (ydtInsCarApiSearchResultData != null)
                {
                    carInfo.Belong = ydtInsCarApiSearchResultData.Belong ?? "1";
                    carInfo.CarType = "1";
                    if (ydtInsCarApiSearchResultData.Car != null)
                    {
                        carInfo.Vin = ydtInsCarApiSearchResultData.Car.vin.NullStringToNullObject();
                        carInfo.EngineNo = ydtInsCarApiSearchResultData.Car.engineNo.NullStringToNullObject();
                        if (CommonUtils.IsDateTime(ydtInsCarApiSearchResultData.Car.firstRegisterDate))
                        {
                            carInfo.FirstRegisterDate = DateTime.Parse(ydtInsCarApiSearchResultData.Car.firstRegisterDate).ToUnifiedFormatDate();
                        }
                        carInfo.ModelCode = ydtInsCarApiSearchResultData.Car.modelCode.NullStringToNullObject();
                        carInfo.ModelName = ydtInsCarApiSearchResultData.Car.modelName.NullStringToNullObject();
                        carInfo.Displacement = ydtInsCarApiSearchResultData.Car.displacement.NullStringToNullObject();
                        carInfo.MarketYear = ydtInsCarApiSearchResultData.Car.marketYear.NullStringToNullObject();
                        carInfo.RatedPassengerCapacity = ydtInsCarApiSearchResultData.Car.ratedPassengerCapacity;
                        carInfo.ReplacementValue = ydtInsCarApiSearchResultData.Car.replacementValue;
                        carInfo.ChgownerType = ydtInsCarApiSearchResultData.Car.chgownerType.NullStringToNullObject();
                        if (CommonUtils.IsDateTime(ydtInsCarApiSearchResultData.Car.chgownerDate))
                        {
                            carInfo.ChgownerDate = DateTime.Parse(ydtInsCarApiSearchResultData.Car.chgownerDate).ToUnifiedFormatDate();
                        }
                        carInfo.Tonnage = ydtInsCarApiSearchResultData.Car.tonnage.NullStringToNullObject();
                        carInfo.WholeWeight = ydtInsCarApiSearchResultData.Car.wholeWeight.NullStringToNullObject();
                    }
                }

                if (drivingLicenceInfo != null)
                {
                    carInfo.Vin = carInfo.Vin ?? drivingLicenceInfo.vin.NullStringToNullObject();
                    carInfo.EngineNo = carInfo.EngineNo ?? drivingLicenceInfo.engineNo.NullStringToNullObject();
                    carInfo.FirstRegisterDate = carInfo.FirstRegisterDate ?? drivingLicenceInfo.registerDate.NullStringToNullObject();
                    carInfo.ModelName = carInfo.ModelName ?? drivingLicenceInfo.model.NullStringToNullObject();
                    carInfo.ChgownerType = carInfo.ChgownerType ?? "0";//是否过户，0：否，1：是
                }

                carInfo.Belong = ydtInsCarApiSearchResultData.Belong ?? "1";//车辆归属     1：私人，2：公司
                carInfo.ChgownerType = carInfo.ChgownerType ?? "0";//是否过户              0：否，1：是

                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "1" });
                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "2" });
                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "3" });
            }
            else
            {
                carInfo.Belong = insCarInfo.Belong;
                carInfo.CarType = insCarInfo.CarType;
                carInfo.FirstRegisterDate = insCarInfo.FirstRegisterDate;
                carInfo.ModelCode = insCarInfo.ModelCode;
                carInfo.ModelName = insCarInfo.ModelName;
                carInfo.Displacement = insCarInfo.Displacement;
                carInfo.MarketYear = insCarInfo.MarketYear;
                carInfo.RatedPassengerCapacity = insCarInfo.RatedPassengerCapacity;
                carInfo.ReplacementValue = insCarInfo.ReplacementValue;
                carInfo.ChgownerType = insCarInfo.ChgownerType;
                carInfo.ChgownerDate = insCarInfo.ChgownerDate;
                carInfo.Tonnage = insCarInfo.Tonnage;
                carInfo.WholeWeight = insCarInfo.WholeWeight;
                carInfo.Vin = insCarInfo.Vin;
                carInfo.EngineNo = insCarInfo.EngineNo;
                carInfo.FirstRegisterDate = insCarInfo.FirstRegisterDate;
                carInfo.ModelName = insCarInfo.ModelName;
                carInfo.ChgownerType = insCarInfo.ChgownerType;


                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "1", Name = insCarInfo.CarownerName, Mobile = insCarInfo.CarownerMobile, Address = insCarInfo.CarownerAddress, CertNo = insCarInfo.CarownerCertNo, IdentityBackPicKey = insCarInfo.CarownerIdentityBackPicKey, IdentityFacePicKey = insCarInfo.CarownerIdentityFacePicKey, OrgPicKey = insCarInfo.CarownerOrgPicKey });
                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "2", Name = insCarInfo.PolicyholderName, Mobile = insCarInfo.PolicyholderMobile, Address = insCarInfo.PolicyholderAddress, CertNo = insCarInfo.PolicyholderCertNo, IdentityBackPicKey = insCarInfo.PolicyholderIdentityBackPicKey, IdentityFacePicKey = insCarInfo.PolicyholderIdentityFacePicKey, OrgPicKey = insCarInfo.PolicyholderOrgPicKey });
                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "3", Name = insCarInfo.InsuredName, Mobile = insCarInfo.InsuredMobile, Address = insCarInfo.InsuredAddress, CertNo = insCarInfo.InsuredCertNo, IdentityBackPicKey = insCarInfo.InsuredIdentityBackPicKey, IdentityFacePicKey = insCarInfo.InsuredIdentityFacePicKey, OrgPicKey = insCarInfo.InsuredOrgPicKey });
            }




            carInfoResult.Car = carInfo;






            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", carInfoResult);
        }

        public APIResponse GetCarModelInfo(string keyword, string vin, string firstRegisterDate)
        {
            var carModelInfoResult = new CarModelInfoResult();


            var ydtCarModelQueryResultData = YdtUtils.CarModelQuery(keyword, vin, firstRegisterDate);

            if (ydtCarModelQueryResultData != null)
            {
                foreach (var item in ydtCarModelQueryResultData)
                {
                    var carModelInfoModel = new CarModelInfoModel();

                    carModelInfoModel.ModelCode = item.modelCode;
                    carModelInfoModel.ModelName = item.modelName;
                    carModelInfoModel.Displacement = item.displacement;
                    carModelInfoModel.MarketYear = item.marketYear;
                    carModelInfoModel.RatedPassengerCapacity = item.ratedPassengerCapacity;
                    carModelInfoModel.ReplacementValue = item.replacementValue;

                    carModelInfoResult.models.Add(carModelInfoModel);
                }
            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", carModelInfoResult);


        }

        [HttpPost]
        public APIResponse EditBaseInfo(CarInsEditBaseInfoPms pms)
        {
            var editBaseInfoResult = new CarInsEditBaseInfoResult();


            #region 检查必要选项
            //if (pms.Car.CarType == "1" || pms.Car.CarType == "3")
            //{
            //    if (string.IsNullOrEmpty(pms.Car.LicensePicKey))
            //    {
            //        return ResponseResult(ResultType.Failure, ResultCode.Failure, "行驶证正面图片必须上传");
            //    }

            //    if (string.IsNullOrEmpty(pms.Car.LicensePlateNo))
            //    {
            //        return ResponseResult(ResultType.Failure, ResultCode.Failure, "9座以下桥车、车货的车牌号码必须填写");
            //    }

            //    if (pms.Car.CarType == "3")
            //    {
            //        if (string.IsNullOrEmpty(pms.Car.Tonnage))
            //        {
            //            return ResponseResult(ResultType.Failure, ResultCode.Failure, "车货的核定载质量必须填写,单位：千克");
            //        }

            //        if (string.IsNullOrEmpty(pms.Car.WholeWeight))
            //        {
            //            return ResponseResult(ResultType.Failure, ResultCode.Failure, "车货的整备质量必须填写,单位：千克");
            //        }

            //        if (string.IsNullOrEmpty(pms.Car.LicenseOtherPicKey))
            //        {
            //            return ResponseResult(ResultType.Failure, ResultCode.Failure, "行驶证副面图片必须上传");
            //        }
            //    }
            //}
            //else if (pms.Car.CarType == "2")
            //{
            //    if (string.IsNullOrEmpty(pms.Car.CarCertPicKey))
            //    {
            //        return ResponseResult(ResultType.Failure, ResultCode.Failure, "车辆合格证图片必须上传");
            //    }

            //    if (string.IsNullOrEmpty(pms.Car.CarInvoicePicKey))
            //    {
            //        return ResponseResult(ResultType.Failure, ResultCode.Failure, "车辆发票图片必须上传");
            //    }
            //}

            //if (pms.Car.ChgownerType == "1")
            //{
            //    if (string.IsNullOrEmpty(pms.Car.ChgownerDate))
            //    {
            //        return ResponseResult(ResultType.Failure, ResultCode.Failure, "选择是过户，过户时间必须填写");
            //    }
            //}

            //foreach (var item in pms.Customers)
            //{

            //    if (item.InsuredFlag == "1")
            //    {
            //        if (string.IsNullOrEmpty(item.IdentityFacePicKey))
            //        {
            //            return ResponseResult(ResultType.Failure, ResultCode.Failure, "身份证正面图片必须上传");
            //        }
            //        if (string.IsNullOrEmpty(item.IdentityBackPicKey))
            //        {
            //            return ResponseResult(ResultType.Failure, ResultCode.Failure, "身份证反面图片必须上传");
            //        }
            //    }
            //    else if (item.InsuredFlag == "2")
            //    {
            //        if (string.IsNullOrEmpty(item.OrgPicKey))
            //        {
            //            return ResponseResult(ResultType.Failure, ResultCode.Failure, "组织机构图片必须上传");
            //        }
            //    }
            //}
            #endregion

            YdtInscarEditbasePms baseInfoModel = new YdtInscarEditbasePms();

            baseInfoModel.auto = int.Parse(pms.Auto);
            baseInfoModel.belong = int.Parse(pms.Car.Belong);
            baseInfoModel.carType = 1;

            if (pms.CarInfoOrderId > 0)
            {
                var insCarInfoOrder = CurrentDb.InsCarInfoOrder.Where(m => m.Id == pms.CarInfoOrderId).FirstOrDefault();
                if (insCarInfoOrder != null)
                {
                    if (!string.IsNullOrEmpty(insCarInfoOrder.PartnerOrderId))
                    {
                        baseInfoModel.orderSeq = insCarInfoOrder.PartnerOrderId;


                    }

                }
            }

            #region  车辆信息
            baseInfoModel.car.licensePlateNo = pms.Car.LicensePlateNo;
            baseInfoModel.car.vin = pms.Car.Vin;
            baseInfoModel.car.engineNo = pms.Car.EngineNo;
            baseInfoModel.car.firstRegisterDate = pms.Car.FirstRegisterDate;
            baseInfoModel.car.modelCode = pms.Car.ModelCode;
            baseInfoModel.car.modelName = pms.Car.ModelName;
            baseInfoModel.car.displacement = pms.Car.Displacement;
            baseInfoModel.car.marketYear = pms.Car.MarketYear;
            baseInfoModel.car.ratedPassengerCapacity = pms.Car.RatedPassengerCapacity;
            baseInfoModel.car.replacementValue = pms.Car.ReplacementValue;
            baseInfoModel.car.chgownerType = pms.Car.ChgownerType;
            baseInfoModel.car.chgownerDate = pms.Car.ChgownerDate;
            baseInfoModel.car.tonnage = pms.Car.Tonnage;
            baseInfoModel.car.wholeWeight = pms.Car.WholeWeight;

            #endregion 

            #region 被保人，投保人，车主
            List<YdtInscarCustomerModel> customers = new List<YdtInscarCustomerModel>();


            if (pms.Customers != null)
            {
                var carOwnerInfo = pms.Customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                if (carOwnerInfo != null)
                {
                    YdtInscarCustomerModel insureds = new YdtInscarCustomerModel();
                    insureds.insuredFlag = "1";//被保人
                    insureds.name = carOwnerInfo.Name;
                    insureds.certNo = carOwnerInfo.CertNo;
                    insureds.mobile = carOwnerInfo.Mobile;
                    insureds.address = carOwnerInfo.Address;
                    insureds.identityFacePic = "0a1e00f463d3f3a50163e0199eed0012.jpg";
                    insureds.identityBackPic = "0a1e00f463d3f3a50163e0199eed0012.jpg";

                    YdtInscarCustomerModel holder = new YdtInscarCustomerModel();
                    holder.insuredFlag = "2";//投保人
                    holder.name = insureds.name;
                    holder.certNo = insureds.certNo;
                    holder.mobile = insureds.mobile;
                    holder.address = insureds.address;
                    holder.identityFacePic = insureds.identityFacePic;
                    holder.identityBackPic = insureds.identityBackPic;

                    YdtInscarCustomerModel carOwner = new YdtInscarCustomerModel();
                    carOwner.insuredFlag = "3";//车主
                    carOwner.name = insureds.name;
                    carOwner.certNo = insureds.certNo;
                    carOwner.mobile = insureds.mobile;
                    carOwner.address = insureds.address;
                    carOwner.identityFacePic = insureds.identityFacePic;
                    carOwner.identityBackPic = insureds.identityBackPic;
                    if (pms.Car.Belong == "2")
                    {
                        carOwner.orgPic = null;
                    }

                    customers.Add(insureds);
                    customers.Add(holder);
                    customers.Add(carOwner);

                    baseInfoModel.customers = customers;
                }
            }
            #endregion

            #region  图片
            YdtInscarPicModel insPic = new YdtInscarPicModel();



            insPic.licensePic = "0a1e00f463d3f3a50163e0199eed0012.jpg";
            insPic.licenseOtherPic = "0a1e00f463d3f3a50163e0199eed0012.jpg";
            insPic.carCertPic = "0a1e00f463d3f3a50163e0199eed0012.jpg";
            insPic.carInvoicePic = "0a1e00f463d3f3a50163e0199eed0012.jpg";

            //insPic.licensePic = pms.Car.LicensePicKey;
            //insPic.licenseOtherPic = pms.Car.LicenseOtherPicKey;
            //insPic.carCertPic = pms.Car.CarCertPicKey;
            //insPic.carInvoicePic = pms.Car.CarInvoicePicKey;

            baseInfoModel.pic = insPic;
            #endregion


            //BizFactory.InsCar.UpdateCarInfo(0, pms.Car, pms.Customers);

            IResult<string> result = YdtUtils.EditBaseInfo(baseInfoModel);

            if (result.Result == ResultType.Success)
            {
                editBaseInfoResult.Auto = "1";//默认自动报价
                editBaseInfoResult.Car = pms.Car;
                editBaseInfoResult.Customers = pms.Customers;
                editBaseInfoResult.CarInfoOrderId = BizFactory.InsCar.UpdateCarInfoOrder(pms.UserId, pms.UserId, result.Data.ToString(), editBaseInfoResult.Car, editBaseInfoResult.Customers);


                return ResponseResult(ResultType.Success, ResultCode.Success, result.Message, editBaseInfoResult);
            }
            else
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, result.Message);
            }
        }

        [HttpPost]
        public APIResponse InsComanyInfo(CarInsComanyInfoPms pms)
        {
            var insComanyInfoResult = new CarInsComanyInfoResult();

            var carInsuranceCompanys = CurrentDb.CarInsuranceCompany.ToList();

            var insCarInfoOrder = CurrentDb.InsCarInfoOrder.Where(m => m.UserId == pms.UserId && m.Id == pms.CarInfoOrdeId).FirstOrDefault();

            if (insCarInfoOrder == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "找不到订单信息");
            }

            if (string.IsNullOrEmpty(insCarInfoOrder.PartnerOrderId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "基础信息未添加");
            }

            var ydtCarModelQueryResultData = YdtUtils.GetInquiryInfo(insCarInfoOrder.PartnerOrderId, pms.AreaId);

            if (ydtCarModelQueryResultData != null)
            {
                insComanyInfoResult.AreaId = ydtCarModelQueryResultData.areaId;
                insComanyInfoResult.LicensePicKey = ydtCarModelQueryResultData.licensePic;

                if (ydtCarModelQueryResultData.channelList != null)
                {
                    foreach (var item in ydtCarModelQueryResultData.channelList)
                    {
                        var channel = new Channel();

                        var insCompany = YdtDataMap.GetCompanyByCode(item.code);
                        if (insCompany != null)
                        {
                            var company = carInsuranceCompanys.Where(m => m.InsuranceCompanyId == insCompany.UpLinkCode).FirstOrDefault();
                            if (company != null)
                            {
                                channel.ChannelId = item.channelId;
                                channel.Code = item.code;
                                channel.Descp = item.descp;
                                channel.Inquiry = item.inquiry;
                                channel.Message = item.message;
                                channel.Name = item.name;
                                channel.OpType = item.opType;
                                channel.Remote = item.remote;
                                channel.Sort = item.sort;
                                channel.CompanyId = company.InsuranceCompanyId;
                                channel.CompanyImg = company.InsuranceCompanyImgUrl;
                                insComanyInfoResult.Channels.Add(channel);
                            }
                        }
                    }
                }

                if (ydtCarModelQueryResultData.planList != null)
                {
                    foreach (var item in ydtCarModelQueryResultData.planList)
                    {
                        var area = new Area();
                        area.AreaId = item.areaId;
                        area.AreaName = item.areaName;
                        insComanyInfoResult.Areas.Add(area);
                    }
                }

            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", insComanyInfoResult);
        }

        [HttpPost]
        public APIResponse InsInquiry(CarInsInquiryPms pms)
        {
            YdtInscarInquiryPms model = new YdtInscarInquiryPms();


            var insCarInfoOrder = CurrentDb.InsCarInfoOrder.Where(m => m.UserId == pms.UserId && m.Id == pms.CarInfoOrderId).FirstOrDefault();


            if (insCarInfoOrder == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "找不到订单信息");
            }

            if (string.IsNullOrEmpty(insCarInfoOrder.PartnerOrderId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "基础信息未添加");
            }

            model.auto = pms.Auto;
            model.channelId = pms.ChannelId;
            model.companyCode = pms.CompanyCode;
            model.risk = GetRisk(pms.InsureKind);

            if (model.risk == 2 || model.risk == 3)
            {
                model.ciStartDate = pms.CiStartDate;
            }

            if (model.risk == 1 || model.risk == 3)
            {
                model.biStartDate = pms.BiStartDate;
            }

            model.ciStartDate = pms.CiStartDate;
            model.biStartDate = pms.BiStartDate;

            string startDate = null;
            if (!string.IsNullOrEmpty(pms.CiStartDate))
            {
                startDate = pms.CiStartDate;
            }
            else
            {
                if (!string.IsNullOrEmpty(pms.BiStartDate))
                {
                    startDate = pms.BiStartDate;
                }
            }

            var insCarAdvicevalueModel = new InsCarAdvicevalueModel();
            insCarAdvicevalueModel.startDate = startDate;
            insCarAdvicevalueModel.registDate = pms.Car.FirstRegisterDate;
            insCarAdvicevalueModel.replacementValue = pms.Car.ReplacementValue;

            var ydtGetAdviceValue = YdtUtils.GetAdviceValue(insCarAdvicevalueModel.startDate, insCarAdvicevalueModel.registDate, insCarAdvicevalueModel.replacementValue);
            if (ydtGetAdviceValue.Result != ResultType.Success)
            {
                if (pms.Auto == 1)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "折旧价计算失败", ydtGetAdviceValue.Message);
                }
            }

            decimal actualPrice = ydtGetAdviceValue.Data;

            model.coverages = GetCoverages(pms.InsureKind, actualPrice, pms.Car.RatedPassengerCapacity);
            model.orderSeq = insCarInfoOrder.PartnerOrderId;

            var updateOrderOfferPms = new UpdateOrderOfferPms();
            updateOrderOfferPms.Auto = pms.Auto;
            updateOrderOfferPms.UserId = pms.UserId;
            updateOrderOfferPms.MerchantId = pms.MerchantId;
            updateOrderOfferPms.PosMachineId = pms.PosMachineId;
            updateOrderOfferPms.CarInfoOrderId = pms.CarInfoOrderId;
            updateOrderOfferPms.PartnerOrderId = insCarInfoOrder.PartnerOrderId;
            updateOrderOfferPms.PartnerChannelId = pms.ChannelId;
            updateOrderOfferPms.PartnerCompanyId = pms.CompanyCode;
            updateOrderOfferPms.PartnerRisk = model.risk;
            updateOrderOfferPms.BiStartDate = model.biStartDate;
            updateOrderOfferPms.CiStartDate = model.ciStartDate;
            updateOrderOfferPms.Coverages = model.coverages;
            updateOrderOfferPms.OfferResult = Enumeration.OfferResult.WaitAutoOffer;

            var result_UpdateOfferByBefore = BizFactory.InsCar.UpdateOfferByBefore(0, updateOrderOfferPms);

            if (result_UpdateOfferByBefore.Result != ResultType.Success)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, result_UpdateOfferByBefore.Message);
            }

            CustomJsonResult<YdtInscarInquiryResultData> offerResult = null;
            //0 人工报价，1 自动报价
            if (pms.Auto == 0)
            {
                model.notifyUrl = "http://api.gzhaoyilian.com/Api/CarIns/InquiryNotify";

                try
                {
                    offerResult = YdtUtils.GetInsInquiryByArtificial(model);
                }
                catch (Exception ex)
                {

                }

                updateOrderOfferPms.OfferResult = Enumeration.OfferResult.WaitArtificialOffer;

            }
            else
            {
                offerResult = YdtUtils.GetInsInquiryByAuto(model);

                if (offerResult.Result != ResultType.Success)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "获取自动报价失败");
                }

                updateOrderOfferPms.OfferResult = Enumeration.OfferResult.AutoOfferSuccess;
            }


            #region 构造结果


            YdtInscarInquiryResultData offerResultData = null;

            if (offerResult != null)
            {
                offerResultData = offerResult.Data;

                if (offerResultData != null)
                {
                    updateOrderOfferPms.PartnerInquirySeq = offerResultData.inquirySeq;
                    updateOrderOfferPms.Inquirys = offerResultData.inquirys;
                    updateOrderOfferPms.Coverages = offerResultData.coverages;
                }
            }

            var result_UpdateOfferByAfter = BizFactory.InsCar.UpdateOfferByAfter(0, updateOrderOfferPms);


            if (pms.Auto == 0)
            {
                return ResponseResult(ResultType.Success, ResultCode.Success, "提交人工报价成功", null);
            }
            else
            {
                CarInsInquiryResult result = new CarInsInquiryResult();
                var channel = new Channel();
                var insCompany = YdtDataMap.GetCompanyByCode(pms.CompanyCode);
                if (insCompany != null)
                {
                    var company = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == insCompany.UpLinkCode).FirstOrDefault();
                    if (company != null)
                    {
                        channel.Name = company.InsuranceCompanyName;
                        channel.ChannelId = pms.ChannelId;
                        channel.Code = pms.CompanyCode;
                        channel.CompanyId = company.InsuranceCompanyId;
                        channel.CompanyImg = company.InsuranceCompanyImgUrl;
                    }
                }

                result.Channel = channel;
                result.OfferId = result_UpdateOfferByAfter.Data.CarInsureOfferCompany.Id;
                result.InsureItem = GetInsureItem(result_UpdateOfferByAfter.Data.CarInsure, result_UpdateOfferByAfter.Data.CarInsureOfferCompany, result_UpdateOfferByAfter.Data.CarInsureOfferCompanyKinds);
                result.SumPremium = result_UpdateOfferByAfter.Data.CarInsureOfferCompany.InsureTotalPrice.Value;

                return ResponseResult(ResultType.Success, ResultCode.Success, "自动报价成功", result);
            }

            #endregion

        }

        [HttpPost]
        public APIResponse Insure(CarInsInsurePms pms)
        {
            CarInsInsureResult result = new CarInsInsureResult();


            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.Id == pms.OfferId).FirstOrDefault();

            if (orderToCarInsureOfferCompany == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "未找到报价结果");
            }


            if (string.IsNullOrEmpty(orderToCarInsureOfferCompany.PartnerOrderId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "基本信息未添加");
            }

            if (string.IsNullOrEmpty(orderToCarInsureOfferCompany.PartnerInquiryId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "报价未完成");
            }


            YdtInscarInsurePms ydtInscarInsurePms = new YdtInscarInsurePms();

            ydtInscarInsurePms.inquirySeq = orderToCarInsureOfferCompany.PartnerInquiryId;
            ydtInscarInsurePms.notifyUrl = "http://api.gzhaoyilian.com/Api/CarIns/InsureNotify";
            ydtInscarInsurePms.orderSeq = orderToCarInsureOfferCompany.PartnerOrderId;
            ydtInscarInsurePms.address.consignee = "朱长荣";
            ydtInscarInsurePms.address.email = "chingment@126.com";
            ydtInscarInsurePms.address.mobile = "15989287032";
            ydtInscarInsurePms.address.address = "广东省新丰县梅坑镇利坑村牛路坎组2-1号";

            var result_Insure = YdtUtils.Insure(ydtInscarInsurePms);

            if (result_Insure.Result != ResultType.Success)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "核保失败", result);
            }

            orderToCarInsureOfferCompany.PartnerInsureId = result_Insure.Data.insureSeq;
            orderToCarInsureOfferCompany.BiProposalNo = result_Insure.Data.biProposalNo;
            orderToCarInsureOfferCompany.CiProposalNo = result_Insure.Data.ciProposalNo;
            CurrentDb.SaveChanges();


            return ResponseResult(ResultType.Success, ResultCode.Success, "核保成功", result);

        }

        [HttpPost]
        public APIResponse Pay(CarInsPayPms pms)
        {
            CarInsPayResult result = new CarInsPayResult();


            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.Id == pms.OfferId).FirstOrDefault();

            if (orderToCarInsureOfferCompany == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "未找到报价结果");
            }


            if (string.IsNullOrEmpty(orderToCarInsureOfferCompany.PartnerOrderId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "基本信息未添加");
            }

            if (string.IsNullOrEmpty(orderToCarInsureOfferCompany.PartnerInquiryId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "报价未完成");
            }

            if (string.IsNullOrEmpty(orderToCarInsureOfferCompany.PartnerInsureId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "核保未完成");
            }


            YdtInscarPayPms ydtInscarPayPms = new YdtInscarPayPms();
            ydtInscarPayPms.insureSeq = orderToCarInsureOfferCompany.PartnerInsureId;
            ydtInscarPayPms.inquirySeq = orderToCarInsureOfferCompany.PartnerInquiryId;
            ydtInscarPayPms.orderSeq = orderToCarInsureOfferCompany.PartnerOrderId;
            ydtInscarPayPms.notifyUrl = "http://api.gzhaoyilian.com/Api/CarIns/PayNotify";

            var result_Insure = YdtUtils.Pay(ydtInscarPayPms);

            if (result_Insure.Result != ResultType.Success)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "生成支付失败", result);
            }

            orderToCarInsureOfferCompany.PartnerInsureId = result_Insure.Data.insureSeq;
            orderToCarInsureOfferCompany.PartnerPayId = result_Insure.Data.paySeq;
            orderToCarInsureOfferCompany.PayUrl = result_Insure.Data.payUrl;
            CurrentDb.SaveChanges();

            return ResponseResult(ResultType.Success, ResultCode.Success, "生成支付失败", result);

        }

        [HttpPost]
        [AllowAnonymous]
        public APIResponse InquiryNotify(YdtInscarInquiryResultData pms)
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();

            Log.Info("InquiryNotify：" + postData);

            var result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "success");
            return new APIResponse(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public APIResponse InsureNotify(YdtInscarInsureResultData pms)
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();

            Log.Info("InsureNotify：" + postData);

            var result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "success");
            return new APIResponse(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public APIResponse PayNotify(YdtInscarPayResultData pms)
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();

            Log.Info("PayNotify：" + postData);

            var result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "success");
            return new APIResponse(result);
        }



        private static int GetRisk(List<CarInsInsureKindModel> kinds)
        {
            if (kinds == null)
                return 2;

            if (kinds.Count == 0)
                return 2;


            var lists = kinds.Where(m => m.Id == 1 || m.Id == 2).ToList();
            var listc = kinds.Where(m => m.Id >= 3).ToList();

            if (lists.Count == 2 && listc.Count == 0)
                return 2;


            if (lists.Count == 0 && listc.Count > 0)
                return 1;

            return 3;

        }

        private static decimal GetCoverageAmount(string d)
        {
            if (string.IsNullOrEmpty(d))
                return 0;

            decimal amount = 0;
            d = d.ToLower();
            int of = d.IndexOf('w');
            if (of > -1)
            {
                d = d.Substring(0, of);

                amount = decimal.Parse(d) * 10000;

            }
            else
            {
                amount = decimal.Parse(d);
            }

            return amount;
        }

        private static List<CoverageModel> GetCoverages(List<CarInsInsureKindModel> kinds, decimal oldAmount, int carSeat)
        {
            List<CoverageModel> list = new List<CoverageModel>();
            var ydtInsCoverageList = YdtDataMap.YdtInsCoverageList();
            foreach (var kind in kinds)
            {
                var coverage = ydtInsCoverageList.Where(m => m.UpLinkCode == kind.Id).FirstOrDefault();
                if (coverage != null)
                {
                    CoverageModel model = new CoverageModel();
                    model.code = coverage.Code;

                    #region 是否免损
                    switch (kind.Id)
                    {
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 9:
                        case 11:
                        case 12:
                            if (kind.IsWaiverDeductible)
                            {
                                model.compensation = 1;
                            }
                            else
                            {
                                model.compensation = 0;
                            }
                            break;
                    }
                    #endregion

                    #region 需要折旧价
                    switch (kind.Id)
                    {
                        case 3:
                        case 7:
                        case 10:
                        case 11:
                        case 12:
                        case 17:
                            model.amount = oldAmount;
                            break;
                    }
                    #endregion

                    #region 玻璃险
                    if (kind.Id == 8)
                    {
                        if (kind.Value == "国产")
                        {
                            model.glassType = 1;
                        }
                        else
                        {
                            model.glassType = 2;
                        }
                    }
                    #endregion

                    #region 第三者责任

                    if (kind.Id == 4)
                    {
                        model.amount = GetCoverageAmount(kind.Value);
                    }

                    #endregion

                    #region 司乘险

                    if (kind.Id == 5)
                    {
                        model.unitAmount = GetCoverageAmount(kind.Value);
                        model.quantity = 1;
                        model.amount = model.unitAmount.Value * model.quantity.Value;
                    }

                    #endregion

                    #region  乘客险
                    if (kind.Id == 6)
                    {
                        var sCarSeat = carSeat - 1;

                        model.unitAmount = GetCoverageAmount(kind.Value);
                        model.quantity = sCarSeat;
                        model.amount = model.unitAmount.Value * model.quantity.Value;
                    }
                    #endregion

                    #region 划损险

                    if (kind.Id == 9)
                    {
                        model.unitAmount = GetCoverageAmount(kind.Value);
                        model.quantity = 1;
                        model.amount = GetCoverageAmount(kind.Value);
                    }

                    #endregion


                    list.Add(model);
                }
            }

            return list;
        }

        private static List<ItemParentField> GetInsureItem(OrderToCarInsure carInsure, OrderToCarInsureOfferCompany carInsureOfferCompany, List<OrderToCarInsureOfferCompanyKind> carInsureOfferCompanyKinds)
        {
            List<ItemParentField> parents = new List<ItemParentField>();


            if (carInsureOfferCompany != null)
            {

                if (carInsureOfferCompany.CompulsoryPrice != null)
                {
                    if (carInsureOfferCompany.CompulsoryPrice.Value > 0)
                    {
                        var parentsByCompulsory = new ItemParentField();
                        parentsByCompulsory.Field = "交强险";
                        parentsByCompulsory.Value = carInsureOfferCompany.CompulsoryPrice.ToF2Price();
                        parents.Add(parentsByCompulsory);
                    }
                }
                if (carInsureOfferCompany.TravelTaxPrice != null)
                {
                    if (carInsureOfferCompany.TravelTaxPrice != null)
                    {
                        var parentsByTravelTax = new ItemParentField();
                        parentsByTravelTax.Field = "车船稅";
                        parentsByTravelTax.Value = carInsureOfferCompany.TravelTaxPrice.ToF2Price();
                        parents.Add(parentsByTravelTax);
                    }
                }


                if (carInsureOfferCompany.CommercialPrice != null)
                {
                    if (carInsureOfferCompany.CommercialPrice != null)
                    {
                        var parentsByCommercial = new ItemParentField();
                        parentsByCommercial.Field = "商业险";
                        parentsByCommercial.Value = carInsureOfferCompany.CommercialPrice.ToF2Price();

                        var carInsureOfferCompanyKinds1 = carInsureOfferCompanyKinds.Where(m => m.IsCompensation == false).OrderBy(m => m.Priority).ToList();
                        foreach (var kind in carInsureOfferCompanyKinds1)
                        {
                            parentsByCommercial.Child.Add(new ItemChildField(kind.KindName, kind.StandardPremium.ToF2Price()));
                        }

                        var carInsureOfferCompanyKinds2 = carInsureOfferCompanyKinds.Where(m => m.IsCompensation == true).OrderBy(m => m.Priority).ToList();

                        if (carInsureOfferCompanyKinds2.Count > 0)
                        {
                            decimal sumCompensation = 0;
                            foreach (var kind in carInsureOfferCompanyKinds2)
                            {
                                sumCompensation += kind.StandardPremium;
                                parentsByCommercial.Child.Add(new ItemChildField(kind.KindName, "已投"));
                            }

                            parentsByCommercial.Child.Add(new ItemChildField("总计不免赔", sumCompensation.ToF2Price()));
                        }

                        parents.Add(parentsByCommercial);
                    }
                }
            }


            var parentsByCarInfo = new ItemParentField("车辆信息", "");

            parentsByCarInfo.Child.Add(new ItemChildField("车牌号码", carInsure.CarLicensePlateNo));
            parentsByCarInfo.Child.Add(new ItemChildField("品牌型号", carInsure.CarModelName));
            parentsByCarInfo.Child.Add(new ItemChildField("配置信息", "大众FV720FCDWG桥车 2012款 19884ML 5座"));
            parentsByCarInfo.Child.Add(new ItemChildField("注册日期", carInsure.CarFirstRegisterDate));
            parentsByCarInfo.Child.Add(new ItemChildField("车架号", carInsure.CarVin));
            parentsByCarInfo.Child.Add(new ItemChildField("发动机", carInsure.CarEngineNo));
            parentsByCarInfo.Child.Add(new ItemChildField("是否过户", carInsure.CarChgownerType == "0" ? "否" : "是"));
            if (carInsure.CarChgownerType == "1")
            {
                parents.Add(new ItemParentField("过户日期", carInsure.CarChgownerDate));
            }

            parents.Add(parentsByCarInfo);

            return parents;
        }
    }
}