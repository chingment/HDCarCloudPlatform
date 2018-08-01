using Lumos;
using Lumos.BLL;
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
using System.Text;
using System.Web;
using System.Web.Http;
using WebAppApi.Models;
using WebAppApi.Models.Account;
using YdtSdk;

namespace WebAppApi.Controllers
{

    public class CarInsUploadImgPms
    {
        public string Type { get; set; }

        public Dictionary<string, ImageModel> ImgData { get; set; }

        public CarInsUploadImgPms()
        {
            this.ImgData = new Dictionary<string, ImageModel>();
        }
    }


    [BaseAuthorizeAttribute]
    public class CarInsController : OwnBaseApiController
    {

        public static string nullName = "某某某";
        public static string nullAddress = "null";
        public static string nullCerno = "440182198804141552";
        public static string nullMobile = "13800138000";


        [HttpPost]
        public APIResponse UploadImg(CarInsUploadImgPms pms)
        {
            CarInsUploadImgResult result = new CarInsUploadImgResult();
            string imgurl = "";

            if (pms.ImgData == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "上传图片的内容为空");
            }


            if (!pms.ImgData.ContainsKey("certPic"))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "必须上传的键名为certPic的图片内容");
            }

            LogUtil.Info("开始上传");
            imgurl = GetUploadImageUrl(pms.ImgData["certPic"], "CarInsure");
            LogUtil.Info("上传结束");

            if (string.IsNullOrEmpty(imgurl))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "上传图片失败");
            }
            LogUtil.Info("imgurl:" + imgurl);
            LogUtil.Info("开始解释图片");
            switch (pms.Type)
            {
                case "1":
                    // imgurl = "http://120.79.233.231:8087/Upload/CarInsure/56f84750-294f-4a40-a702-f30d3ecf2aa3_O.jpg";
                    YdtUploadResultData model1 = YdtUtils.UploadImg(imgurl);
                    LogUtil.Info("解释图片结束");
                    if (model1 != null)
                    {
                        LogUtil.Info("解释对象不为空");
                        result.Url = imgurl;

                        if (model1.file != null)
                        {

                            LogUtil.Info("解释KEy不为空");
                            result.Key = model1.file.key;
                        }
                    }
                    break;
                case "10":
                    //imgurl = "http://file.gzhaoyilian.com/Upload/d2.jpg";
                    YdtLicenseInfo model2 = YdtUtils.GetLicenseInfoByUrl(imgurl);
                    if (model2 != null)
                    {
                        result.Url = imgurl;
                        if (model2.fileKey != null)
                        {
                            result.Key = model2.fileKey;
                        }

                        result.Info = model2;
                    }
                    break;
                case "11":
                    //imgurl = "http://file.gzhaoyilian.com/Upload/d1.jpg";
                    YdtIdentityInfo model3 = YdtUtils.GetIdentityInfoByUrl(imgurl);
                    if (model3 != null)
                    {
                        result.Url = imgurl;
                        if (model3.fileKey != null)
                        {
                            result.Key = model3.fileKey;
                        }

                        result.Info = model3;
                    }
                    break;
            }

            if (string.IsNullOrEmpty(imgurl))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "图片上传失败，请重新选择", result);
            }


            if (string.IsNullOrEmpty(result.Key) || string.IsNullOrEmpty(result.Url))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "图片解释失败，相片类型错误或模糊，请重新选择或者拍照", result);
            }


            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", result);
        }

        [HttpPost]
        public APIResponse GetCarInfo(CarInfoPms pms)
        {

            var carInfoResult = new CarInfoResult();

            var carInfo = new CarInfoModel();
            var customers = new List<CarInsCustomerModel>();
            string licensePlateNo = "";
            string imgurl = null;
            DrivingLicenceInfo drivingLicenceInfo = null; ;
            switch (pms.KeywordType)
            {
                case KeyWordType.LicenseImg:

                    string[] arr_Keyword = pms.Keyword.Split('@');
                    ImageModel imgModel = new ImageModel();
                    imgModel.Data = arr_Keyword[0];
                    imgModel.Type = arr_Keyword[1];
                    imgurl = GetUploadImageUrl(imgModel, "CarInsure");

                    LogUtil.Info("IMGURL:" + imgurl);

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
            if (drivingLicenceInfo != null)
            {
                carInfo.LicensePicKey = drivingLicenceInfo.fileKey;
            }

            carInfo.LicensePicUrl = imgurl;




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
                        carInfo.BiEndDate = ydtInsCarApiSearchResultData.BiEndDate;
                        carInfo.BiStartDate = ydtInsCarApiSearchResultData.BiStartDate;
                        carInfo.CiEndDate = ydtInsCarApiSearchResultData.CiEndDate;
                        carInfo.CiStartDate = ydtInsCarApiSearchResultData.CiStartDate;
                    }

                }
                else
                {
                    carInfo.Belong = "1";//车辆归属     1：私人，2：公司
                    carInfo.ChgownerType = "0";//是否过户              0：否，1：是
                }

                if (drivingLicenceInfo != null)
                {
                    carInfo.Vin = carInfo.Vin ?? drivingLicenceInfo.vin.NullStringToNullObject();
                    carInfo.EngineNo = carInfo.EngineNo ?? drivingLicenceInfo.engineNo.NullStringToNullObject();
                    carInfo.FirstRegisterDate = carInfo.FirstRegisterDate ?? drivingLicenceInfo.registerDate.NullStringToNullObject();
                    carInfo.ModelName = carInfo.ModelName ?? drivingLicenceInfo.model.NullStringToNullObject();
                    carInfo.ChgownerType = carInfo.ChgownerType ?? "0";//是否过户，0：否，1：是
                }

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

                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "3", Name = insCarInfo.InsuredName, Mobile = insCarInfo.InsuredMobile, Address = insCarInfo.InsuredAddress, CertNo = insCarInfo.InsuredCertNo, IdentityBackPicKey = insCarInfo.InsuredIdentityBackPicKey, IdentityFacePicKey = insCarInfo.InsuredIdentityFacePicKey, OrgPicKey = insCarInfo.InsuredOrgPicKey });
                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "1", Name = insCarInfo.CarownerName, Mobile = insCarInfo.CarownerMobile, Address = insCarInfo.CarownerAddress, CertNo = insCarInfo.CarownerCertNo, IdentityBackPicKey = insCarInfo.CarownerIdentityBackPicKey, IdentityFacePicKey = insCarInfo.CarownerIdentityFacePicKey, OrgPicKey = insCarInfo.CarownerOrgPicKey });
                carInfoResult.Customers.Add(new CarInsCustomerModel { InsuredFlag = "2", Name = insCarInfo.PolicyholderName, Mobile = insCarInfo.PolicyholderMobile, Address = insCarInfo.PolicyholderAddress, CertNo = insCarInfo.PolicyholderCertNo, IdentityBackPicKey = insCarInfo.PolicyholderIdentityBackPicKey, IdentityFacePicKey = insCarInfo.PolicyholderIdentityFacePicKey, OrgPicKey = insCarInfo.PolicyholderOrgPicKey });

            }


            carInfoResult.Auto = "0";
            carInfoResult.Car = carInfo;
            carInfoResult.Car.Belong = carInfoResult.Car.Belong ?? "1";
            carInfoResult.Car.ChgownerType = carInfoResult.Car.ChgownerType ?? "0";

            carInfoResult.Customers[0].Name = "测试";
            carInfoResult.Customers[1].Name = "测试";
            carInfoResult.Customers[2].Name = "测试";

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", carInfoResult);
        }

        public APIResponse GetCarModelInfo(string keyword, string vin, string firstRegisterDate)
        {
            var carModelInfoResult = new CarModelInfoResult();


            var ydtCarModelQueryResultData = YdtUtils.CarModelQuery(vin, vin, firstRegisterDate);

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
                for (var i = 0; i < pms.Customers.Count; i++)
                {


                    pms.Customers[i].Name = string.IsNullOrEmpty(pms.Customers[i].Name) == true ? nullName : pms.Customers[i].Name;
                    pms.Customers[i].CertNo = string.IsNullOrEmpty(pms.Customers[i].CertNo) == true ? nullCerno : pms.Customers[i].CertNo;
                    pms.Customers[i].Mobile = string.IsNullOrEmpty(pms.Customers[i].Mobile) == true ? nullMobile : pms.Customers[i].Mobile;
                    pms.Customers[i].Address = string.IsNullOrEmpty(pms.Customers[i].Address) == true ? nullAddress : pms.Customers[i].Address;

                    //1是私人车，2为公司车
                    if (pms.Car.Belong == "1")
                    {
                        pms.Customers[i].IdentityFacePicKey = pms.Customers[i].IdentityFacePicKey;
                        pms.Customers[i].IdentityBackPicKey = pms.Customers[i].IdentityBackPicKey;
                        pms.Customers[i].OrgPicKey = null;
                    }
                    else
                    {
                        pms.Customers[i].IdentityFacePicKey = null;
                        pms.Customers[i].IdentityBackPicKey = null;
                        pms.Customers[i].OrgPicKey = pms.Customers[i].OrgPicKey;
                    }

                }

                var carOwnerInfo = pms.Customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                if (carOwnerInfo != null)
                {

                    YdtInscarCustomerModel carOwner = new YdtInscarCustomerModel();
                    carOwner.insuredFlag = "3";//车主
                    carOwner.name = carOwnerInfo.Name;
                    carOwner.certNo = carOwnerInfo.CertNo;
                    carOwner.mobile = carOwnerInfo.Mobile;
                    carOwner.address = carOwnerInfo.Address;
                    carOwner.identityFacePic = carOwnerInfo.IdentityFacePicKey;
                    carOwner.identityBackPic = carOwnerInfo.IdentityBackPicKey;
                    carOwner.orgPic = carOwnerInfo.OrgPicKey;

                    YdtInscarCustomerModel insureds = new YdtInscarCustomerModel();
                    insureds.insuredFlag = "1";//被保人
                    insureds.name = carOwner.name;
                    insureds.certNo = carOwner.certNo;
                    insureds.mobile = carOwner.mobile;
                    insureds.address = carOwner.address;
                    insureds.identityFacePic = carOwner.identityFacePic;
                    insureds.identityBackPic = carOwner.identityBackPic;
                    insureds.orgPic = carOwner.orgPic;


                    YdtInscarCustomerModel holder = new YdtInscarCustomerModel();
                    holder.insuredFlag = "2";//投保人
                    holder.name = carOwner.name;
                    holder.certNo = carOwner.certNo;
                    holder.mobile = carOwner.mobile;
                    holder.address = carOwner.address;
                    holder.identityFacePic = carOwner.identityFacePic;
                    holder.identityBackPic = carOwner.identityBackPic;
                    holder.orgPic = carOwner.orgPic;

                    customers.Add(carOwner);
                    customers.Add(insureds);
                    customers.Add(holder);

                    baseInfoModel.customers = customers;
                }


            }
            #endregion

            #region  图片
            YdtInscarPicModel insPic = new YdtInscarPicModel();



            insPic.licensePic = pms.Car.LicensePicKey;
            insPic.licenseOtherPic = pms.Car.LicenseOtherPicKey;
            insPic.carCertPic = pms.Car.CarCertPicKey;
            insPic.carInvoicePic = pms.Car.CarInvoicePicKey;



            baseInfoModel.pic = insPic;
            #endregion


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

        [HttpGet]
        public APIResponse GetBaseInfo(int userId, int merchantId, int posMachineId, int offerId)
        {
            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.Id == offerId).FirstOrDefault();

            if (orderToCarInsureOfferCompany == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "未找到报价结果");
            }

            var order = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderToCarInsureOfferCompany.OrderId).FirstOrDefault();

            if (order == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "未找到订单信息");
            }

            var carInsBaseInfoModel = new CarInsBaseInfoModel();

            carInsBaseInfoModel.Car.CarType = order.CarType;
            carInsBaseInfoModel.Car.Belong = order.CarBelong;
            carInsBaseInfoModel.Car.LicensePlateNo = order.CarLicensePlateNo;
            carInsBaseInfoModel.Car.Vin = order.CarVin;
            carInsBaseInfoModel.Car.EngineNo = order.CarEngineNo;
            carInsBaseInfoModel.Car.FirstRegisterDate = order.CarFirstRegisterDate;
            carInsBaseInfoModel.Car.ModelCode = order.CarModelCode;
            carInsBaseInfoModel.Car.ModelName = order.CarModelName;
            carInsBaseInfoModel.Car.Displacement = order.CarDisplacement;
            carInsBaseInfoModel.Car.MarketYear = order.CarMarketYear;
            carInsBaseInfoModel.Car.RatedPassengerCapacity = order.CarRatedPassengerCapacity;
            carInsBaseInfoModel.Car.ReplacementValue = order.CarReplacementValue == null ? 0 : order.CarReplacementValue.Value;
            carInsBaseInfoModel.Car.ChgownerType = order.CarChgownerType;
            carInsBaseInfoModel.Car.ChgownerDate = order.CarChgownerDate;
            carInsBaseInfoModel.Car.Tonnage = order.CarTonnage;
            carInsBaseInfoModel.Car.WholeWeight = order.CarWholeWeight;
            carInsBaseInfoModel.Car.LicensePicKey = order.CarLicensePicKey;
            carInsBaseInfoModel.Car.LicensePicUrl = order.CarLicensePicUrl;
            carInsBaseInfoModel.Car.LicenseOtherPicKey = order.CarLicenseOtherPicKey;
            carInsBaseInfoModel.Car.LicenseOtherPicUrl = order.CarLicenseOtherPicUrl;
            carInsBaseInfoModel.Car.CarInvoicePicKey = order.CarInvoicePicKey;
            carInsBaseInfoModel.Car.CarInvoicePicUrl = order.CarInvoicePicUrl;
            carInsBaseInfoModel.Car.CarCertPicKey = order.CarCertPicKey;
            carInsBaseInfoModel.Car.CarCertPicUrl = order.CarCertPicUrl;



            carInsBaseInfoModel.Customers.Add(new CarInsCustomerModel
            {
                InsuredFlag = "3",
                Name = (order.CarownerName == nullName ? "" : order.CarownerName),
                Mobile = (order.CarownerMobile == nullMobile ? "" : order.CarownerMobile),
                Address = (order.CarownerAddress == nullAddress ? "" : order.CarownerAddress),
                CertNo = (order.CarownerCertNo == nullCerno ? "" : order.CarownerCertNo),
                IdentityFacePicUrl = order.CarownerIdentityFacePicUrl,
                IdentityFacePicKey = order.CarownerIdentityFacePicKey,
                IdentityBackPicKey = order.CarownerIdentityBackPicKey,
                IdentityBackPicUrl = order.CarownerIdentityBackPicUrl,
                OrgPicKey = order.CarownerOrgPicKey,
                OrgPicUrl = order.CarownerOrgPicUrl
            });
            carInsBaseInfoModel.Customers.Add(new CarInsCustomerModel
            {
                InsuredFlag = "1",
                Name = (order.PolicyholderName == nullName ? "" : order.PolicyholderName),
                Mobile = (order.PolicyholderMobile == nullMobile ? "" : order.PolicyholderMobile),
                Address = (order.PolicyholderAddress == nullAddress ? "" : order.PolicyholderAddress),
                CertNo = (order.PolicyholderCertNo == nullCerno ? "" : order.PolicyholderCertNo),
                IdentityFacePicUrl = order.PolicyholderIdentityFacePicUrl,
                IdentityFacePicKey = order.PolicyholderIdentityFacePicKey,
                IdentityBackPicKey = order.PolicyholderIdentityFacePicKey,
                IdentityBackPicUrl = order.PolicyholderIdentityFacePicUrl,
                OrgPicKey = order.PolicyholderOrgPicKey,
                OrgPicUrl = order.PolicyholderOrgPicUrl
            });


            carInsBaseInfoModel.Customers.Add(new CarInsCustomerModel
            {
                InsuredFlag = "2",
                Name = (order.InsuredName == nullName ? "" : order.InsuredName),
                Mobile = (order.InsuredMobile == nullMobile ? "" : order.InsuredMobile),
                Address = (order.InsuredAddress == nullAddress ? "" : order.InsuredAddress),
                CertNo = (order.InsuredCertNo == nullCerno ? "" : order.InsuredCertNo),
                IdentityFacePicKey = order.InsuredIdentityFacePicKey,
                IdentityFacePicUrl = order.InsuredIdentityFacePicUrl,
                IdentityBackPicKey = order.InsuredIdentityBackPicKey,
                IdentityBackPicUrl = order.InsuredIdentityBackPicUrl,
                OrgPicKey = order.InsuredOrgPicKey,
                OrgPicUrl = order.InsuredOrgPicUrl
            });


            return ResponseResult(ResultType.Success, ResultCode.Success, "", carInsBaseInfoModel);
        }

        [HttpPost]
        public APIResponse InsComanyInfo(CarInsCompanyInfoPms pms)
        {
            var insComanyInfoResult = new CarInsCompanyInfoResult();

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
                        var insCompany = YdtDataMap.GetCompanyByCode(item.code);
                        if (insCompany != null)
                        {
                            var company = carInsuranceCompanys.Where(m => m.InsuranceCompanyId == insCompany.UpLinkCode).FirstOrDefault();
                            if (company != null)
                            {
                                var carInsComanyModel = new CarInsComanyModel();

                                carInsComanyModel.Id = company.InsuranceCompanyId;
                                carInsComanyModel.ImgUrl = company.InsuranceCompanyImgUrl;
                                carInsComanyModel.PartnerChannelId = item.channelId;
                                carInsComanyModel.PartnerCode = item.code;
                                carInsComanyModel.Descp = item.descp;
                                carInsComanyModel.Name = item.name;
                                insComanyInfoResult.Companys.Add(carInsComanyModel);
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
            insCarAdvicevalueModel.registDate = insCarInfoOrder.FirstRegisterDate;
            insCarAdvicevalueModel.replacementValue = insCarInfoOrder.ReplacementValue;

            var ydtGetAdviceValue = YdtUtils.GetAdviceValue(insCarAdvicevalueModel.startDate, insCarAdvicevalueModel.registDate, insCarAdvicevalueModel.replacementValue);
            if (ydtGetAdviceValue.Result != ResultType.Success)
            {
                if (pms.Auto == 1)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "折旧价计算失败", ydtGetAdviceValue.Message);
                }
            }

            decimal actualPrice = ydtGetAdviceValue.Data;

            model.coverages = GetCoverages(pms.InsureKind, actualPrice, insCarInfoOrder.RatedPassengerCapacity);
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
                model.notifyUrl = string.Format("{0}/Api/CarIns/InquiryNotify", BizFactory.AppSettings.WebApiServerUrl);
                model.openNotifyUrl = string.Format("{0}/Api/CarIns/InquiryNotify", BizFactory.AppSettings.WebApiServerUrl);

                offerResult = YdtUtils.GetInsInquiryByArtificial(model);

                if (offerResult.Result != ResultType.Success)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "提交人工报价失败，请咨询客服");
                }

                updateOrderOfferPms.OfferResult = Enumeration.OfferResult.SumbitArtificialOfferSuccess;

            }
            else
            {
                offerResult = YdtUtils.GetInsInquiryByAuto(model);

                if (offerResult.Result != ResultType.Success)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, offerResult.Message);
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
                    updateOrderOfferPms.PartnerInquiryId = offerResultData.inquirySeq;
                    updateOrderOfferPms.Inquirys = offerResultData.inquirys;
                    if (offerResultData.coverages != null)
                    {
                        updateOrderOfferPms.Coverages = offerResultData.coverages;
                    }
                }
            }

            var result_UpdateOfferByAfter = BizFactory.InsCar.UpdateOfferByAfter(0, updateOrderOfferPms);


            if (pms.Auto == 0)
            {
                return ResponseResult(ResultType.Success, ResultCode.Success, "提交人工报价成功", null);
            }
            else
            {
                var carInsCompanyInfoModel = new CarInsComanyModel();
                var insCompany = YdtDataMap.GetCompanyByCode(pms.CompanyCode);
                if (insCompany != null)
                {
                    var company = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == insCompany.UpLinkCode).FirstOrDefault();
                    if (company != null)
                    {
                        carInsCompanyInfoModel.Id = company.InsuranceCompanyId;
                        carInsCompanyInfoModel.ImgUrl = company.InsuranceCompanyImgUrl;
                        carInsCompanyInfoModel.Name = company.InsuranceCompanyName;
                        carInsCompanyInfoModel.PartnerChannelId = pms.ChannelId;
                        carInsCompanyInfoModel.PartnerCode = pms.CompanyCode;
                    }
                }

                carInsCompanyInfoModel.OfferId = result_UpdateOfferByAfter.Data.CarInsureOfferCompany.Id;
                carInsCompanyInfoModel.OfferInquirys = GetInsureItem(result_UpdateOfferByAfter.Data.CarInsure, result_UpdateOfferByAfter.Data.CarInsureOfferCompany, result_UpdateOfferByAfter.Data.CarInsureOfferCompanyKinds);
                carInsCompanyInfoModel.OfferSumPremium = result_UpdateOfferByAfter.Data.CarInsureOfferCompany.InsureTotalPrice.Value.ToF2Price();

                return ResponseResult(ResultType.Success, ResultCode.Success, "自动报价成功", carInsCompanyInfoModel);
            }

            #endregion

        }

        [HttpGet]
        public APIResponse GetInsInquiryInfo(int userId, int merchantId, int posMachineId, int orderId)
        {

            var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderId).FirstOrDefault();
            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == orderToCarInsure.Id).FirstOrDefault();
            var orderToCarInsureOfferCompanyKinds = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == orderToCarInsure.Id).ToList();




            var carInsCompanyInfoModel = new CarInsComanyModel();
            var insCompany = YdtDataMap.GetCompanyByCode(orderToCarInsureOfferCompany.PartnerCompanyId);
            if (insCompany != null)
            {
                var company = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == insCompany.UpLinkCode).FirstOrDefault();
                if (company != null)
                {
                    carInsCompanyInfoModel.Id = company.InsuranceCompanyId;
                    carInsCompanyInfoModel.ImgUrl = company.InsuranceCompanyImgUrl;
                    carInsCompanyInfoModel.Name = company.InsuranceCompanyName;
                    carInsCompanyInfoModel.PartnerChannelId = int.Parse(orderToCarInsureOfferCompany.PartnerChannelId);
                    carInsCompanyInfoModel.PartnerCode = orderToCarInsureOfferCompany.PartnerCompanyId;
                }
            }
            carInsCompanyInfoModel.Auto = orderToCarInsure.IsAuto == false ? 0 : 1;

            carInsCompanyInfoModel.OfferId = orderToCarInsureOfferCompany.Id;
            carInsCompanyInfoModel.OfferInquirys = GetInsureItem(orderToCarInsure, orderToCarInsureOfferCompany, orderToCarInsureOfferCompanyKinds);
            carInsCompanyInfoModel.OfferSumPremium = orderToCarInsureOfferCompany.InsureTotalPrice == null ? "正在报价中" : orderToCarInsureOfferCompany.InsureTotalPrice.Value.ToF2Price();

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", carInsCompanyInfoModel);
        }

        [HttpPost]
        public APIResponse Insure(CarInsInsurePms pms)
        {
            CarInsInsureResult result = new CarInsInsureResult();


            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.Id == pms.OfferId).FirstOrDefault();

            var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderToCarInsureOfferCompany.OrderId).FirstOrDefault();




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


            YdtInscarEditbasePms ydtInscarEditbasePms = new YdtInscarEditbasePms();
            ydtInscarEditbasePms.orderSeq = orderToCarInsureOfferCompany.PartnerOrderId;

            ydtInscarEditbasePms.belong = int.Parse(pms.Car.Belong);
            ydtInscarEditbasePms.carType = int.Parse(pms.Car.CarType);

            #region  车辆信息
            ydtInscarEditbasePms.car.licensePlateNo = pms.Car.LicensePlateNo;
            ydtInscarEditbasePms.car.vin = pms.Car.Vin;
            ydtInscarEditbasePms.car.engineNo = pms.Car.EngineNo;
            ydtInscarEditbasePms.car.firstRegisterDate = pms.Car.FirstRegisterDate;
            ydtInscarEditbasePms.car.modelCode = pms.Car.ModelCode;
            ydtInscarEditbasePms.car.modelName = pms.Car.ModelName;
            ydtInscarEditbasePms.car.displacement = pms.Car.Displacement;
            ydtInscarEditbasePms.car.marketYear = pms.Car.MarketYear;
            ydtInscarEditbasePms.car.ratedPassengerCapacity = pms.Car.RatedPassengerCapacity;
            ydtInscarEditbasePms.car.replacementValue = pms.Car.ReplacementValue;
            ydtInscarEditbasePms.car.chgownerType = pms.Car.ChgownerType;
            ydtInscarEditbasePms.car.chgownerDate = pms.Car.ChgownerDate;
            ydtInscarEditbasePms.car.tonnage = pms.Car.Tonnage;
            ydtInscarEditbasePms.car.wholeWeight = pms.Car.WholeWeight;

            #endregion


            #region 被保人，投保人，车主
            List<YdtInscarCustomerModel> customers = new List<YdtInscarCustomerModel>();


            if (pms.Customers != null)
            {
                var carOwnerInfo = pms.Customers.Where(m => m.InsuredFlag == "3").FirstOrDefault();
                if (carOwnerInfo != null)
                {


                    YdtInscarCustomerModel carOwner = new YdtInscarCustomerModel();
                    carOwner.insuredFlag = "3";//车主
                    carOwner.name = carOwnerInfo.Name;
                    carOwner.certNo = string.IsNullOrEmpty(carOwnerInfo.CertNo) == true ? nullCerno : carOwnerInfo.CertNo;
                    carOwner.mobile = string.IsNullOrEmpty(carOwnerInfo.Mobile) == true ? nullMobile : carOwnerInfo.Mobile;
                    carOwner.address = string.IsNullOrEmpty(carOwnerInfo.Address) == true ? nullAddress : carOwnerInfo.Address;
                    if (pms.Car.Belong == "1")
                    {
                        carOwner.identityFacePic = carOwnerInfo.IdentityFacePicKey;
                        carOwner.identityBackPic = carOwnerInfo.IdentityBackPicKey;
                        carOwner.orgPic = null;
                    }
                    else
                    {
                        carOwner.identityFacePic = null;
                        carOwner.identityBackPic = null;
                        carOwner.orgPic = carOwnerInfo.OrgPicKey;
                    }


                    YdtInscarCustomerModel insureds = new YdtInscarCustomerModel();
                    insureds.insuredFlag = "1";//被保人
                    insureds.name = carOwner.name;
                    insureds.certNo = carOwner.certNo;
                    insureds.mobile = carOwner.mobile;
                    insureds.address = carOwner.address;
                    insureds.identityFacePic = carOwner.identityFacePic;
                    insureds.identityBackPic = carOwner.identityBackPic;
                    insureds.orgPic = carOwner.orgPic;


                    YdtInscarCustomerModel holder = new YdtInscarCustomerModel();
                    holder.insuredFlag = "2";//投保人
                    holder.name = carOwner.name;
                    holder.certNo = carOwner.certNo;
                    holder.mobile = carOwner.mobile;
                    holder.address = carOwner.address;
                    holder.identityFacePic = carOwner.identityFacePic;
                    holder.identityBackPic = carOwner.identityBackPic;
                    holder.orgPic = carOwner.orgPic;

                    customers.Add(insureds);
                    customers.Add(holder);
                    customers.Add(carOwner);

                    ydtInscarEditbasePms.customers = customers;
                }
            }
            #endregion

            #region  图片
            YdtInscarPicModel insPic = new YdtInscarPicModel();


            insPic.licensePic = pms.Car.LicensePicKey;
            insPic.licenseOtherPic = pms.Car.LicenseOtherPicKey;
            insPic.carCertPic = pms.Car.CarCertPicKey;
            insPic.carInvoicePic = pms.Car.CarInvoicePicKey;


            ydtInscarEditbasePms.pic = insPic;
            #endregion


            IResult<string> editBaseInfo_Result = YdtUtils.EditBaseInfo(ydtInscarEditbasePms);


            if (editBaseInfo_Result.Result != ResultType.Success)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "保存资料到保险公司失败");
            }

            var updateOrder_Result = BizFactory.InsCar.UpdateOrder(pms.UserId, orderToCarInsureOfferCompany.OrderId, pms.Car, pms.Customers);


            if (updateOrder_Result.Result != ResultType.Success)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "保存资料到本系统失败");
            }


            YdtInscarInsurePms ydtInscarInsurePms = new YdtInscarInsurePms();

            ydtInscarInsurePms.inquirySeq = orderToCarInsureOfferCompany.PartnerInquiryId;
            ydtInscarInsurePms.notifyUrl = string.Format("{0}/Api/CarIns/InsureNotify", BizFactory.AppSettings.WebApiServerUrl);
            ydtInscarInsurePms.openNotifyUrl = string.Format("{0}/Api/CarIns/InsureNotify", BizFactory.AppSettings.WebApiServerUrl);
            ydtInscarInsurePms.orderSeq = orderToCarInsureOfferCompany.PartnerOrderId;


            //0 人工报价，1 自动报价
            if (!orderToCarInsure.IsAuto)
            {
                #region 提交人工核保
                YdtInscarInsureByArtificialPms ydtInscarInsureByArtificialPms = new YdtInscarInsureByArtificialPms();

                ydtInscarInsureByArtificialPms.belong = int.Parse(pms.Car.Belong);
                ydtInscarInsureByArtificialPms.inquirySeq = orderToCarInsureOfferCompany.PartnerInquiryId;
                ydtInscarInsureByArtificialPms.orderSeq = orderToCarInsureOfferCompany.PartnerOrderId;
                ydtInscarInsureByArtificialPms.loanFlag = "false";
                ydtInscarInsureByArtificialPms.licensePic = pms.Car.LicensePicKey;
                ydtInscarInsureByArtificialPms.customers = ydtInscarEditbasePms.customers;
                ydtInscarInsureByArtificialPms.notifyUrl = ydtInscarInsurePms.notifyUrl;
                ydtInscarInsureByArtificialPms.openNotifyUrl = ydtInscarInsurePms.openNotifyUrl;
                var result_Insure = YdtUtils.InsureByArtificial(ydtInscarInsureByArtificialPms);

                if (result_Insure.Result != ResultType.Success)
                {

                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "人工核保提交失败，请联系客服", result);
                }

                orderToCarInsureOfferCompany.PartnerInsureId = result_Insure.Data.insureSeq;
                orderToCarInsure.IsInvisiable = false;
                orderToCarInsure.PartnerInsureId = result_Insure.Data.insureSeq;
                orderToCarInsure.IsAuto = pms.Auto == 0 ? false : true;
                orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialInsure;
                CurrentDb.SaveChanges();


                #endregion
            }
            else
            {
                #region 自动核保

                var result_Insure = YdtUtils.InsureByAuto(ydtInscarInsurePms);

                if (result_Insure.Result != ResultType.Success)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, result_Insure.Message, result);
                }

                orderToCarInsureOfferCompany.PartnerInsureId = result_Insure.Data.insureSeq;

                orderToCarInsureOfferCompany.BiProposalNo = result_Insure.Data.biProposalNo;
                orderToCarInsureOfferCompany.CiProposalNo = result_Insure.Data.ciProposalNo;

                orderToCarInsure.PartnerInsureId = result_Insure.Data.insureSeq;
                orderToCarInsure.BiProposalNo = result_Insure.Data.biProposalNo;
                orderToCarInsure.CiProposalNo = result_Insure.Data.ciProposalNo;
                orderToCarInsure.Status = Enumeration.OrderStatus.WaitPay;
                orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitPay;
                orderToCarInsure.IsAuto = pms.Auto == 0 ? false : true;
                CurrentDb.SaveChanges();

                #endregion
            }



            if (pms.Auto == 0)
            {

                return ResponseResult(ResultType.Success, ResultCode.Success, "提交成功", result);
            }
            else
            {
                var merchant = CurrentDb.Merchant.Where(m => m.Id == pms.MerchantId).FirstOrDefault();

                result.ReceiptAddress.Address = merchant.ContactAddress;
                result.ReceiptAddress.Consignee = merchant.ContactName;
                result.ReceiptAddress.Mobile = merchant.ContactPhoneNumber;
                result.ReceiptAddress.Email = "";
                result.ReceiptAddress.AreaId = merchant.AreaCode;
                result.ReceiptAddress.AreaName = merchant.Area;

                var orderInfo = new ItemParentField("投保单信息", "");

                orderInfo.Child.Add(new ItemChildField("交强险单号", orderToCarInsureOfferCompany.CiProposalNo));
                orderInfo.Child.Add(new ItemChildField("商业险单号", orderToCarInsureOfferCompany.BiProposalNo));
                orderInfo.Child.Add(new ItemChildField("投保单号", orderToCarInsureOfferCompany.PartnerInsureId));
                orderInfo.Child.Add(new ItemChildField("商业险", orderToCarInsureOfferCompany.CommercialPrice.ToF2Price()));
                orderInfo.Child.Add(new ItemChildField("交强险", orderToCarInsureOfferCompany.CompulsoryPrice.ToF2Price()));
                orderInfo.Child.Add(new ItemChildField("车船税", orderToCarInsureOfferCompany.TravelTaxPrice.ToF2Price()));

                result.InfoItems.Add(orderInfo);

                return ResponseResult(ResultType.Success, ResultCode.Success, "核保成功", result);

            }

        }

        [HttpGet]
        public APIResponse GetInsureInfo(int userId, int merchantId, int posMachineId, int orderId)
        {
            CarInsInsureInfo info = new CarInsInsureInfo();

            var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderId).FirstOrDefault();
            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == orderId).FirstOrDefault();
            var orderToCarInsureOfferCompanyKinds = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == orderId).ToList();
            var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantId).FirstOrDefault();

            var carInsCompanyInfoModel = new CarInsComanyModel();

            carInsCompanyInfoModel.Id = orderToCarInsure.InsCompanyId;
            carInsCompanyInfoModel.ImgUrl = orderToCarInsure.InsCompanyImgUrl;
            carInsCompanyInfoModel.Name = orderToCarInsure.InsCompanyName;
            carInsCompanyInfoModel.PartnerChannelId = int.Parse(orderToCarInsureOfferCompany.PartnerChannelId);
            carInsCompanyInfoModel.PartnerCode = orderToCarInsureOfferCompany.PartnerCompanyId;

            carInsCompanyInfoModel.OfferId = orderToCarInsureOfferCompany.Id;
            carInsCompanyInfoModel.OfferInquirys = GetInsureItem(orderToCarInsure, orderToCarInsureOfferCompany, orderToCarInsureOfferCompanyKinds);
            carInsCompanyInfoModel.OfferSumPremium = orderToCarInsureOfferCompany.InsureTotalPrice.Value.ToF2Price();

            info.Auto = orderToCarInsure.IsAuto == false ? 0 : 1;
            info.OfferInfo = carInsCompanyInfoModel;

            CarInsInsureResult insureInfo = new CarInsInsureResult();

            insureInfo.ReceiptAddress.Address = merchant.ContactAddress;
            insureInfo.ReceiptAddress.Consignee = merchant.ContactName;
            insureInfo.ReceiptAddress.Mobile = merchant.ContactPhoneNumber;
            insureInfo.ReceiptAddress.Email = "";
            insureInfo.ReceiptAddress.AreaId = merchant.AreaCode;
            insureInfo.ReceiptAddress.AreaName = merchant.Area;

            var orderInfo = new ItemParentField("投保单信息", "");


            orderInfo.Child.Add(new ItemChildField("交强险单号", orderToCarInsureOfferCompany.CiProposalNo));
            orderInfo.Child.Add(new ItemChildField("商业险单号", orderToCarInsureOfferCompany.BiProposalNo));
            orderInfo.Child.Add(new ItemChildField("投保单号", orderToCarInsureOfferCompany.PartnerInsureId));
            orderInfo.Child.Add(new ItemChildField("商业险", orderToCarInsureOfferCompany.CommercialPrice.ToF2Price()));
            orderInfo.Child.Add(new ItemChildField("交强险", orderToCarInsureOfferCompany.CompulsoryPrice.ToF2Price()));
            orderInfo.Child.Add(new ItemChildField("车船税", orderToCarInsureOfferCompany.TravelTaxPrice.ToF2Price()));



            insureInfo.InfoItems.Add(orderInfo);


            info.InsureInfo = insureInfo;

            if (!string.IsNullOrEmpty(orderToCarInsureOfferCompany.PayUrl))
            {
                CarInsPayResult payInfo = new CarInsPayResult();

                payInfo.OfferId = orderToCarInsureOfferCompany.Id;
                payInfo.OrderSn = orderToCarInsure.Sn;
                payInfo.payUrl = orderToCarInsureOfferCompany.PayUrl;

                info.PayInfo = payInfo;

            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "核保成功", info);

        }


        [HttpPost]
        public APIResponse Pay(CarInsPayPms pms)
        {
            CarInsPayResult result = new CarInsPayResult();


            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.Id == pms.OfferId).FirstOrDefault();
            var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderToCarInsureOfferCompany.OrderId).FirstOrDefault();
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
            ydtInscarPayPms.notifyUrl = string.Format("{0}/Api/CarIns/PayResultNotify", BizFactory.AppSettings.WebApiServerUrl);
            ydtInscarPayPms.openNotifyUrl = string.Format("{0}/Api/CarIns/PayResultNotify", BizFactory.AppSettings.WebApiServerUrl);
            ydtInscarPayPms.address.consignee = pms.ReceiptAddress.Consignee;
            ydtInscarPayPms.address.address = pms.ReceiptAddress.Address;
            ydtInscarPayPms.address.mobile = pms.ReceiptAddress.Mobile;
            ydtInscarPayPms.address.email = pms.ReceiptAddress.Email;

            string areaId = pms.ReceiptAddress.AreaId;

            //if (areaId.Length > 4)
            //{
            //    areaId = areaId.Substring(0, 4);
            //}

            ydtInscarPayPms.address.areaId = areaId;

            if (orderToCarInsure.IsAuto)
            {

                ydtInscarPayPms.notifyUrl = string.Format("{0}/Api/CarIns/PayResultNotify", BizFactory.AppSettings.WebApiServerUrl);
                ydtInscarPayPms.openNotifyUrl = string.Format("{0}/Api/CarIns/PayResultNotify", BizFactory.AppSettings.WebApiServerUrl);

                var result_Pay = YdtUtils.PayByAuto(ydtInscarPayPms);

                if (result_Pay.Result != ResultType.Success)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "生成支付申请失败，请联系客服", result);
                }

                orderToCarInsure.Recipient = pms.ReceiptAddress.Consignee;
                orderToCarInsure.RecipientAddress = pms.ReceiptAddress.Address;
                orderToCarInsure.RecipientPhoneNumber = pms.ReceiptAddress.Mobile;
                orderToCarInsure.RecipientAreaName = pms.ReceiptAddress.AreaName;
                orderToCarInsure.RecipientAreaCode = pms.ReceiptAddress.AreaId;
                orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitPay;
                orderToCarInsure.Status = Enumeration.OrderStatus.WaitPay;
                orderToCarInsure.PartnerPayId = result_Pay.Data.paySeq;


                orderToCarInsureOfferCompany.PayUrl = result_Pay.Data.payUrl;
                orderToCarInsureOfferCompany.PartnerPayId = result_Pay.Data.paySeq;


                CurrentDb.SaveChanges();

                result.OrderSn = orderToCarInsure.Sn;
                result.OfferId = pms.OfferId;
                result.payUrl = orderToCarInsureOfferCompany.PayUrl;
            }
            else
            {

                YdtInscarPayByArtificialPms ydtInscarPayByArtificialPms = new YdtInscarPayByArtificialPms();

                ydtInscarPayByArtificialPms.insureSeq = orderToCarInsureOfferCompany.PartnerInsureId;
                ydtInscarPayByArtificialPms.inquirySeq = orderToCarInsureOfferCompany.PartnerInquiryId;
                ydtInscarPayByArtificialPms.orderSeq = orderToCarInsureOfferCompany.PartnerOrderId;
                ydtInscarPayByArtificialPms.notifyUrl = string.Format("{0}/Api/CarIns/PayResultNotify", BizFactory.AppSettings.WebApiServerUrl);
                ydtInscarPayByArtificialPms.openNotifyUrl = string.Format("{0}/Api/CarIns/PayResultNotify", BizFactory.AppSettings.WebApiServerUrl);
                ydtInscarPayByArtificialPms.address.consignee = pms.ReceiptAddress.Consignee;
                ydtInscarPayByArtificialPms.address.address = pms.ReceiptAddress.Address;
                ydtInscarPayByArtificialPms.address.mobile = pms.ReceiptAddress.Mobile;
                ydtInscarPayByArtificialPms.address.email = pms.ReceiptAddress.Email;
                ydtInscarPayByArtificialPms.address.areaId = areaId;

                var result_PayByArtificial = YdtUtils.PayByArtificial(ydtInscarPayByArtificialPms);

                if (result_PayByArtificial.Result != ResultType.Success)
                {

                    return ResponseResult(ResultType.Failure, ResultCode.Failure, "生成支付失败，请联系客服", result);
                }
                else
                {
                    orderToCarInsure.Status = Enumeration.OrderStatus.WaitPay;
                    orderToCarInsure.PartnerPayId = result_PayByArtificial.Data.paySeq;
                    orderToCarInsure.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitPay;
                    orderToCarInsure.Recipient = pms.ReceiptAddress.Consignee;
                    orderToCarInsure.RecipientAddress = pms.ReceiptAddress.Address;
                    orderToCarInsure.RecipientPhoneNumber = pms.ReceiptAddress.Mobile;
                    orderToCarInsure.RecipientAreaName = pms.ReceiptAddress.AreaName;
                    orderToCarInsure.RecipientAreaCode = pms.ReceiptAddress.AreaId;

                    orderToCarInsureOfferCompany.PayUrl = result_PayByArtificial.Data.payUrl;
                    orderToCarInsureOfferCompany.PartnerPayId = result_PayByArtificial.Data.paySeq;


                    CurrentDb.SaveChanges();

                    result.OrderSn = orderToCarInsure.Sn;
                    result.OfferId = pms.OfferId;
                    result.payUrl = orderToCarInsureOfferCompany.PayUrl;

                    return ResponseResult(ResultType.Success, ResultCode.Success, "申请支付成功", result);
                }
            }


            return ResponseResult(ResultType.Success, ResultCode.Success, "生成支付成功", result);

        }


        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage InquiryNotify()
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();
            LogUtil.Info("GetIP：" + CommonUtils.GetIP());
            LogUtil.Info("报价结果异步通知InquiryNotify：" + postData);
            SdkFactory.Ydt.NotifyLog("报价", "", postData);

            YdtInscarInquiryResultData pms = null;

            try
            {
                try
                {
                    pms = Newtonsoft.Json.JsonConvert.DeserializeObject<YdtInscarInquiryResultData>(postData);
                }
                catch
                {
                    LogUtil.Error("JSON数据解释不成功");
                }


                if (pms == null)
                {
                    LogUtil.Info("JSON数据解释 为空");
                }
                else
                {

                    var orderToCarInsures = CurrentDb.OrderToCarInsure.Where(m => m.PartnerOrderId == pms.orderSeq && m.PartnerInquiryId == pms.inquirySeq).ToList();

                    if (orderToCarInsures.Count == 0)
                    {
                        LogUtil.Info("OrderToCarInsure找不到订单:" + pms.orderSeq);
                    }
                    else
                    {

                        foreach (var item in orderToCarInsures)
                        {

                            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == item.Id).FirstOrDefault();

                            if (orderToCarInsureOfferCompany == null)
                            {
                                LogUtil.Info("orderToCarInsureOfferCompany 为空");
                            }
                            else
                            {

                                var updateOrderOfferPms = new UpdateOrderOfferPms();
                                updateOrderOfferPms.Auto = 0;
                                updateOrderOfferPms.UserId = item.UserId;
                                updateOrderOfferPms.MerchantId = item.MerchantId;
                                updateOrderOfferPms.PosMachineId = item.PosMachineId;
                                updateOrderOfferPms.CarInfoOrderId = item.CarInfoOrderId;
                                updateOrderOfferPms.PartnerOrderId = item.PartnerOrderId;
                                updateOrderOfferPms.PartnerInquiryId = pms.inquirySeq;
                                if (CommonUtils.IsInt(orderToCarInsureOfferCompany.PartnerChannelId))
                                {
                                    updateOrderOfferPms.PartnerChannelId = int.Parse(orderToCarInsureOfferCompany.PartnerChannelId);
                                }
                                updateOrderOfferPms.PartnerCompanyId = orderToCarInsureOfferCompany.PartnerCompanyId;

                                if (CommonUtils.IsInt(item.PartnerRisk))
                                {
                                    updateOrderOfferPms.PartnerRisk = int.Parse(item.PartnerRisk);
                                }
                                updateOrderOfferPms.BiStartDate = pms.biStartDate;
                                updateOrderOfferPms.CiStartDate = pms.ciStartDate;
                                updateOrderOfferPms.Coverages = pms.coverages;
                                updateOrderOfferPms.OfferResult = Enumeration.OfferResult.ArtificialOfferSuccess;
                                updateOrderOfferPms.Inquirys = pms.inquirys;

                                BizFactory.InsCar.UpdateOfferByAfter(0, updateOrderOfferPms);

                            }
                        }


                        LogUtil.Info("人工报价成功");

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("数据解释不成功", ex);
            }

            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent("success", Encoding.GetEncoding("UTF-8"), "text/plain") };
            return result;
        }


        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage InsureNotify()
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();
            LogUtil.Info("核保结果异步通知InsureNotify：" + postData);
            SdkFactory.Ydt.NotifyLog("核保", "", postData);

            YdtInscarInsureResultData pms = null;
            try
            {
                try
                {
                    pms = Newtonsoft.Json.JsonConvert.DeserializeObject<YdtInscarInsureResultData>(postData);
                }
                catch
                {
                    LogUtil.Error("JSON数据解释不成功");
                }

                if (pms == null)
                {
                    LogUtil.Error("JSON数据解释 为空");

                }
                else
                {

                    var orderToCarInsures = CurrentDb.OrderToCarInsure.Where(m => m.PartnerOrderId == pms.orderSeq && m.PartnerInsureId == pms.insureSeq).ToList();

                    foreach (var item in orderToCarInsures)
                    {

                        var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == item.Id).FirstOrDefault();

                        if (orderToCarInsureOfferCompany == null)
                        {
                            LogUtil.Info("orderToCarInsureOfferCompany 为空");
                        }
                        else
                        {

                            orderToCarInsureOfferCompany.PartnerInsureId = pms.insureSeq;
                            orderToCarInsureOfferCompany.BiProposalNo = pms.biProposalNo;
                            orderToCarInsureOfferCompany.CiProposalNo = pms.ciProposalNo;

                        }

                        item.PartnerInsureId = pms.insureSeq;
                        item.BiProposalNo = pms.biProposalNo;
                        item.CiProposalNo = pms.ciProposalNo;


                        item.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitPay;

                        item.Status = Enumeration.OrderStatus.WaitPay;

                        CurrentDb.SaveChanges();

                        LogUtil.Info("人工核保成功");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("数据解释不成功", ex);
            }


            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent("success", Encoding.GetEncoding("UTF-8"), "text/plain") };
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage PayResultNotify()
        {
            Stream stream = HttpContext.Current.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string postData = new StreamReader(stream).ReadToEnd();

            LogUtil.Info("支付结果异步通知InsureNotify：" + postData);
            SdkFactory.Ydt.NotifyLog("支付结果通知", "", postData);

            YdtInscarPayResultNotifyData pms = null;
            try
            {
                pms = Newtonsoft.Json.JsonConvert.DeserializeObject<YdtInscarPayResultNotifyData>(postData);
            }
            catch
            {
                LogUtil.Error("JSON数据解释不成功");
            }



            if (pms == null)
            {
                LogUtil.Error("JSON数据解释 为空");

            }
            else
            {

                if (!string.IsNullOrEmpty(pms.orderSeq))
                {
                    var orderToCarInsures = CurrentDb.OrderToCarInsure.Where(m => m.PartnerOrderId == pms.orderSeq && m.PartnerPayId == pms.paySeq).ToList();

                    foreach (var item in orderToCarInsures)
                    {
                        BizFactory.Pay.ResultNotify(0, item.Sn, true, Enumeration.PayResultNotifyType.PartnerPayOrgOrderQueryApi, "易点通", postData);
                    }
                }
            }


            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent("success", Encoding.GetEncoding("UTF-8"), "text/plain") };
            return result;
        }


        [HttpGet]
        public APIResponse GetFollowStatus(int userId, int merchantId, int posMachineId, int orderId)
        {
            CarInsOrderFollowStatus info = new CarInsOrderFollowStatus();

            var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == orderId).FirstOrDefault();


            info.FollowStatus = orderToCarInsure.FollowStatus;

            info.PartnerOrderId = orderToCarInsure.PartnerOrderId;

            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == orderToCarInsure.Id).FirstOrDefault();
            var orderToCarInsureOfferCompanyKinds = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == orderToCarInsure.Id).ToList();

            var carInsCompanyInfoModel = new CarInsComanyModel();
            var insCompany = YdtDataMap.GetCompanyByCode(orderToCarInsureOfferCompany.PartnerCompanyId);
            if (insCompany != null)
            {
                var company = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == insCompany.UpLinkCode).FirstOrDefault();
                if (company != null)
                {
                    carInsCompanyInfoModel.Id = company.InsuranceCompanyId;
                    carInsCompanyInfoModel.ImgUrl = company.InsuranceCompanyImgUrl;
                    carInsCompanyInfoModel.Name = company.InsuranceCompanyName;
                    carInsCompanyInfoModel.PartnerChannelId = int.Parse(orderToCarInsureOfferCompany.PartnerChannelId);
                    carInsCompanyInfoModel.PartnerCode = orderToCarInsureOfferCompany.PartnerCompanyId;
                }
            }

            carInsCompanyInfoModel.OfferId = orderToCarInsureOfferCompany.Id;
            carInsCompanyInfoModel.OfferInquirys = GetInsureItem(orderToCarInsure, orderToCarInsureOfferCompany, orderToCarInsureOfferCompanyKinds);
            carInsCompanyInfoModel.OfferSumPremium = orderToCarInsureOfferCompany.InsureTotalPrice == null ? "正在报价中" : orderToCarInsureOfferCompany.InsureTotalPrice.Value.ToF2Price();

            info.Remark = ((Enumeration.OrderToCarClaimFollowStatus)info.FollowStatus).GetCnName();
            info.OrderInfo = carInsCompanyInfoModel;

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", info);

        }

        private static int GetRisk(List<CarInsInsureKindModel> kinds)
        {
            if (kinds == null)
                return 0;

            if (kinds.Count == 0)
                return 0;


            var lists = kinds.Where(m => m.Id == 1).ToList();
            var listc = kinds.Where(m => m.Id >= 3).ToList();

            if (lists.Count == 1 && listc.Count == 0)
            {
                LogUtil.Info("交强险");
                return 2;
            }


            if (lists.Count == 0 && listc.Count > 0)
            {
                LogUtil.Info("商业险");
                return 1;

            }

            LogUtil.Info("商业险+交强险");
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
                        default:
                            model.compensation = 0;
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
                //大于等于8，说明已经报价成功
                if (carInsure.FollowStatus >= 8)
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
                        if (carInsureOfferCompany.TravelTaxPrice.Value > 0)
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
                            var commercialKinds = carInsureOfferCompanyKinds.Where(m => m.KindId >= 3).ToList();

                            var parentsByCommercial = new ItemParentField();
                            parentsByCommercial.Field = "商业险";
                            parentsByCommercial.Value = carInsureOfferCompany.CommercialPrice.ToF2Price();

                            var carInsureOfferCompanyKinds1 = commercialKinds.Where(m => m.IsWaiverDeductibleKind == false).OrderBy(m => m.Priority).ToList();
                            foreach (var kind in carInsureOfferCompanyKinds1)
                            {
                                parentsByCommercial.Child.Add(new ItemChildField(kind.KindName, kind.StandardPremium.ToF2Price()));
                            }

                            var carInsureOfferCompanyKinds2 = commercialKinds.Where(m => m.IsWaiverDeductibleKind == true).OrderBy(m => m.Priority).ToList();

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
                else
                {
                    var compulsoryKind = carInsureOfferCompanyKinds.Where(m => m.KindId == 1).FirstOrDefault();

                    if (compulsoryKind != null)
                    {
                        var parentsByCompulsory = new ItemParentField();
                        parentsByCompulsory.Field = "交强险";
                        parentsByCompulsory.Value = "已投";
                        parents.Add(parentsByCompulsory);
                    }



                    var travelTaxPrice = carInsureOfferCompanyKinds.Where(m => m.KindId == 2).FirstOrDefault();

                    if (travelTaxPrice != null)
                    {
                        var parentsByTravelTax = new ItemParentField();
                        parentsByTravelTax.Field = "车船稅";
                        parentsByTravelTax.Value = "已投";
                        parents.Add(parentsByTravelTax);
                    }

                    var commercialKinds = carInsureOfferCompanyKinds.Where(m => m.KindId >= 3).ToList();

                    if (commercialKinds.Count > 0)
                    {
                        var parentsByCommercial = new ItemParentField();
                        parentsByCommercial.Field = "商业险";
                        parentsByCommercial.Value = "";


                        var carInsureOfferCompanyKinds1 = commercialKinds.Where(m => m.IsWaiverDeductible == false).OrderBy(m => m.Priority).ToList();
                        foreach (var kind in carInsureOfferCompanyKinds1)
                        {
                            parentsByCommercial.Child.Add(new ItemChildField(kind.KindName, "已投"));
                        }

                        var carInsureOfferCompanyKinds2 = commercialKinds.Where(m => m.IsWaiverDeductible == true).OrderBy(m => m.Priority).ToList();

                        if (carInsureOfferCompanyKinds2.Count > 0)
                        {
                            decimal sumCompensation = 0;
                            foreach (var kind in carInsureOfferCompanyKinds2)
                            {
                                sumCompensation += kind.StandardPremium;
                                parentsByCommercial.Child.Add(new ItemChildField(kind.KindName, "已投"));
                            }
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


            if (carInsure.Status == Enumeration.OrderStatus.Completed)
            {
                var parentsByInusreInfo = new ItemParentField("投保单信息", "");

                parentsByInusreInfo.Child.Add(new ItemChildField("交强险单号", carInsure.CiProposalNo));
                parentsByInusreInfo.Child.Add(new ItemChildField("商业险单号", carInsure.BiProposalNo));
                parentsByInusreInfo.Child.Add(new ItemChildField("投保单号", carInsure.PartnerInsureId));
                parentsByInusreInfo.Child.Add(new ItemChildField("商业险", carInsure.InsCommercialPrice.ToF2Price()));
                parentsByInusreInfo.Child.Add(new ItemChildField("交强险", carInsure.InsCompulsoryPrice.ToF2Price()));
                parentsByInusreInfo.Child.Add(new ItemChildField("车船税", carInsure.InsTravelTaxPrice.ToF2Price()));

                parents.Add(parentsByInusreInfo);

                var parentsByReceiptAddress = new ItemParentField("快递信息", "");

                parentsByReceiptAddress.Child.Add(new ItemChildField("联系人", carInsure.Recipient));
                parentsByReceiptAddress.Child.Add(new ItemChildField("联系电话", carInsure.RecipientPhoneNumber));
                parentsByReceiptAddress.Child.Add(new ItemChildField("联系地址", carInsure.RecipientAddress));

                parents.Add(parentsByReceiptAddress);

            }


            return parents;
        }
    }
}