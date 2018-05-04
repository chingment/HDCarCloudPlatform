using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models;
using WebAppApi.Models.Account;
using WebAppApi.Models.CarIns;
using WebAppApi.Models.CarService;
using YdtSdk;

namespace WebAppApi.Controllers
{

    [BaseAuthorizeAttribute]
    public class CarInsController : OwnBaseApiController
    {
        [BaseAuthorizeAttribute]
        [HttpPost]
        public APIResponse GetCarInfo(CarInfoPms pms)
        {
            var carInfoResult = new CarInfoResult();


            var carInfo = new CarInfoModel();
            var customers = new List<CustomerModel>();
            string licensePlateNo = "";
            DrivingLicenceInfo drivingLicenceInfo = null; ;
            switch (pms.KeywordType)
            {
                case KeyWordType.LicenseImg:

                    ImageModel imgModel = new ImageModel();
                    imgModel.Type = ".jpg";
                    imgModel.Data = pms.Keyword;
                    string imgurl = GetUploadImageUrl(imgModel, "CarInsure");

                    //string imgurl = "http://file.gzhaoyilian.com/Upload/demo_jsz.jpg";
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

            var ydtInsCarApiSearchResultData = YdtUtils.GetInsCarInfo(licensePlateNo);

            if (ydtInsCarApiSearchResultData != null)
            {
                carInfo.Belong = ydtInsCarApiSearchResultData.Belong;
                carInfo.CarType = "1";
                if (ydtInsCarApiSearchResultData.Car != null)
                {
                    carInfo.Vin = ydtInsCarApiSearchResultData.Car.vin;
                    carInfo.EngineNo = ydtInsCarApiSearchResultData.Car.engineNo;
                    carInfo.FirstRegisterDate = ydtInsCarApiSearchResultData.Car.firstRegisterDate;
                    carInfo.ModelCode = ydtInsCarApiSearchResultData.Car.modelCode;
                    carInfo.ModelName = ydtInsCarApiSearchResultData.Car.modelName;
                    carInfo.Displacement = ydtInsCarApiSearchResultData.Car.displacement;
                    carInfo.MarketYear = ydtInsCarApiSearchResultData.Car.marketYear;
                    carInfo.RatedPassengerCapacity = ydtInsCarApiSearchResultData.Car.ratedPassengerCapacity;
                    carInfo.ReplacementValue = ydtInsCarApiSearchResultData.Car.replacementValue;
                    carInfo.ChgownerType = ydtInsCarApiSearchResultData.Car.chgownerType;
                    carInfo.ChgownerDate = ydtInsCarApiSearchResultData.Car.chgownerDate;
                    carInfo.Tonnage = ydtInsCarApiSearchResultData.Car.tonnage;
                    carInfo.WholeWeight = ydtInsCarApiSearchResultData.Car.wholeWeight;
                }
            }

            if (drivingLicenceInfo != null)
            {
                carInfo.Vin = carInfo.Vin ?? drivingLicenceInfo.vin;
                carInfo.EngineNo = carInfo.EngineNo ?? drivingLicenceInfo.engineNo;
                carInfo.FirstRegisterDate = carInfo.FirstRegisterDate ?? drivingLicenceInfo.registerDate;
                carInfo.ModelName = carInfo.ModelName ?? drivingLicenceInfo.model;
                carInfo.ChgownerType = carInfo.ChgownerType ?? "0";//是否过户，0：否，1：是
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

        public APIResponse EditBaseInfo(EditBaseInfoPms pms)
        {
            var editBaseInfoResult = new EditBaseInfoResult();


            #region 检查必要选项
            if (pms.Car.CarType == "1" || pms.Car.CarType == "3")
            {
                if (string.IsNullOrEmpty(pms.Car.LicensePicKey))
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "行驶证正面图片必须上传");
                }

                if (string.IsNullOrEmpty(pms.Car.LicensePlateNo))
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "9座以下桥车、车货的车牌号码必须填写");
                }

                if (pms.Car.CarType == "3")
                {
                    if (string.IsNullOrEmpty(pms.Car.Tonnage))
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "车货的核定载质量必须填写,单位：千克");
                    }

                    if (string.IsNullOrEmpty(pms.Car.WholeWeight))
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "车货的整备质量必须填写,单位：千克");
                    }

                    if (string.IsNullOrEmpty(pms.Car.LicenseOtherPicKey))
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "行驶证副面图片必须上传");
                    }
                }
            }
            else if (pms.Car.CarType == "2")
            {
                if (string.IsNullOrEmpty(pms.Car.CarCertPicKey))
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "车辆合格证图片必须上传");
                }

                if (string.IsNullOrEmpty(pms.Car.CarInvoicePicKey))
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "车辆发票图片必须上传");
                }
            }

            if (pms.Car.ChgownerType == "1")
            {
                if (string.IsNullOrEmpty(pms.Car.ChgownerDate))
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "选择是过户，过户时间必须填写");
                }
            }

            foreach (var item in pms.Customers)
            {

                if (item.InsuredFlag == "1")
                {
                    if (string.IsNullOrEmpty(item.IdentityFacePicKey))
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "身份证正面图片必须上传");
                    }
                    if (string.IsNullOrEmpty(item.IdentityBackPicKey))
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "身份证反面图片必须上传");
                    }
                }
                else if (item.InsuredFlag == "2")
                {
                    if (string.IsNullOrEmpty(item.OrgPicKey))
                    {
                        return ResponseResult(ResultType.Failure, ResultCode.Failure, "组织机构图片必须上传");
                    }
                }
            }
            #endregion 


            return ResponseResult(ResultType.Success, ResultCode.Success, "提交成功", editBaseInfoResult);
        }


        public APIResponse InsComInquiry(EditBaseInfoPms pms)
        {


            return null;
        }

        [HttpPost]
        public APIResponse InsComanyInfo(InsComanyInfoPms pms)
        {
            var insComanyInfoResult = new InsComanyInfoResult();


            var ydtCarModelQueryResultData = YdtUtils.GetInquiryInfo(pms.OrderSeq, pms.AreaId);

            if (ydtCarModelQueryResultData != null)
            {
                insComanyInfoResult.AreaId = ydtCarModelQueryResultData.areaId;
                insComanyInfoResult.LicensePicKey = ydtCarModelQueryResultData.licensePic;

                if (ydtCarModelQueryResultData.channelList != null)
                {
                    foreach (var item in ydtCarModelQueryResultData.channelList)
                    {
                        var channel = new Channel();


                        channel.ChannelId = item.channelId;
                        channel.Code = item.code;
                        channel.Descp = item.descp;
                        channel.Inquiry = item.inquiry;
                        channel.Message = item.message;
                        channel.Name = item.name;
                        channel.OpType = item.opType;
                        channel.Remote = item.remote;
                        channel.Sort = item.sort;

                        insComanyInfoResult.Channels.Add(channel);
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
        public APIResponse InsInquiry(InsInquiryPms pms)
        {




            return null;
        }
    }
}