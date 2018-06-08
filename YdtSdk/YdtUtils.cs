using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{

    public class TokenAu
    {
        public string token { get; set; }

        public string session { get; set; }
    }

    public class YdtUtils
    {
        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        public static TokenAu GetToken()
        {
            var redisClient = new RedisClient<TokenAu>();

            string key = "ydt_toekn";
            TokenAu au = redisClient.KGet(key);
            if (au == null)
            {
                YdtApi c = new YdtApi();
                YdtToken ydtToken = new YdtToken("hylian", "hylian_2018");
                var ydtTokenResult = c.DoGet(ydtToken);

                YdtEmLogin ydtEmLogin = new YdtEmLogin(ydtTokenResult.data.token, "15012405333", "7c4a8d09ca3762af61e59520943dc26494f8941b");
                var ydtEmLoginResult = c.DoGet(ydtEmLogin);

                au = new TokenAu();
                au.token = ydtTokenResult.data.token;
                au.session = ydtEmLoginResult.data.session;

                redisClient.KSet(key, au, new TimeSpan(1, 0, 0));
            }


            return au;

        }

        public static CustomJsonResult GetCarInsOffer(OrderToCarInsure order, List<OrderToCarInsureOfferCompany> offerCompany, List<OrderToCarInsureOfferCompanyKind> kinds)
        {

            CustomJsonResult result = new CustomJsonResult();

            List<YdtInscarInquiryOffer> offerList = new List<YdtInscarInquiryOffer>();

            order.RecipientAddress = "测试地址";
            order.RecipientPhoneNumber = "13800138000";


            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            YdtInscarAddbasePms model = new YdtInscarAddbasePms();

            model.auto = 1;

            //todo 新车怎么标识
            switch (order.CarVechicheType)
            {
                case Enumeration.CarVechicheType.HC:
                    model.carType = 3;
                    break;
                default:
                    model.carType = 1;
                    break;
            }


            switch (order.CarUserCharacter)
            {
                case Enumeration.CarUserCharacter.JTZYQC:
                    model.belong = 1;
                    break;
                default:
                    model.belong = 2;
                    break;
            }

            #region 车辆信息
            YdtInscarInfoModel carInfo = new YdtInscarInfoModel();
            carInfo.licensePlateNo = order.CarLicensePlateNo;
            carInfo.vin = order.CarVin;
            carInfo.engineNo = order.CarEngineNo;
            carInfo.modelCode = order.CarModelCode;
            carInfo.modelName = order.CarModelName;
            carInfo.firstRegisterDate = order.CarFirstRegisterDate;
            // carInfo.displacement = ;
            // carInfo.marketYear = ;

            //switch (order.CarSeat)
            //{
            //    case Enumeration.CarSeat.S6:
            carInfo.ratedPassengerCapacity = order.CarRatedPassengerCapacity;
            //        break;
            //    case Enumeration.CarSeat.S10:
            //        carInfo.ratedPassengerCapacity = 10;
            //        break;
            //    case Enumeration.CarSeat.S12:
            //        carInfo.ratedPassengerCapacity = 12;
            //        break;
            //    case Enumeration.CarSeat.S30:
            //        carInfo.ratedPassengerCapacity = 30;
            //        break;
            //    case Enumeration.CarSeat.S36:
            //        carInfo.ratedPassengerCapacity = 36;
            //        break;
            //    default:
            //        carInfo.ratedPassengerCapacity = 9;
            //        break;
            //}

            carInfo.replacementValue = order.CarReplacementValue.Value;

            //todo
            carInfo.replacementValue = 0;
            carInfo.chgownerType = "0";




            model.car = carInfo;

            #endregion



            #region 被保人，投保人，车主
            List<YdtInscarCustomerModel> customers = new List<YdtInscarCustomerModel>();

            YdtInscarCustomerModel insured = new YdtInscarCustomerModel();
            insured.insuredFlag = "1";
            insured.name = order.CarownerName;
            insured.certNo = order.CarownerCertNo;
            insured.mobile = order.RecipientPhoneNumber;
            insured.address = order.RecipientAddress;

            if (string.IsNullOrEmpty(order.CarownerIdentityFacePicKey))
            {
                YdtUpload ydtUpdate_SFZ = new YdtUpload(au.token, au.session, "1", order.CZ_SFZ_ImgUrl);
                var ydtUpdateResult_SFZ = ydtApi.DoPostFile(ydtUpdate_SFZ, Path.GetFileName(order.CZ_SFZ_ImgUrl));

                if (ydtUpdateResult_SFZ.code != 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ydtUpdateResult_SFZ.msg);
                }
                insured.identityFacePic = ydtUpdateResult_SFZ.data.file.key;
                insured.identityBackPic = ydtUpdateResult_SFZ.data.file.key;
            }
            else
            {
                insured.identityFacePic = order.CarownerIdentityFacePicKey;
                insured.identityBackPic = order.CarownerIdentityFacePicKey;
            }

            YdtInscarCustomerModel holder = new YdtInscarCustomerModel();
            holder.insuredFlag = "2";
            holder.name = insured.name;
            holder.certNo = insured.certNo;
            holder.mobile = insured.mobile;
            holder.address = insured.address;
            holder.identityFacePic = insured.identityFacePic;
            holder.identityBackPic = insured.identityBackPic;

            YdtInscarCustomerModel carOwner = new YdtInscarCustomerModel();
            carOwner.insuredFlag = "3";
            carOwner.name = insured.name;
            carOwner.certNo = insured.certNo;
            carOwner.mobile = insured.mobile;
            carOwner.address = insured.address;
            carOwner.identityFacePic = insured.identityFacePic;
            carOwner.identityBackPic = insured.identityBackPic;


            customers.Add(insured);
            customers.Add(holder);
            customers.Add(carOwner);
            #endregion


            model.customers = customers;

            YdtInscarPicModel insPic = new YdtInscarPicModel();

            //if (string.IsNullOrEmpty(order.CarIdentityCardFaceImgKey))
            //{
            //    var ydtUpdate = new YdtUpload(au.token, au.session, "1", order.CZ_CL_XSZ_ImgUrl);
            //    var ydtUpdateResult = ydtApi.DoPostFile(ydtUpdate, Path.GetFileName(order.CZ_CL_XSZ_ImgUrl));
            //    if (ydtUpdateResult.code != 0)
            //    {
            //        return new CustomJsonResult(ResultType.Failure, ydtUpdateResult.msg);
            //    }
            //    insPic.licensePic = ydtUpdateResult.data.file.key;
            //}
            //else
            //{
            //    insPic.licensePic = order.DrivingLicenceFaceImgKey;
            //}

            #region pic

            insPic.licenseOtherPic = "";
            insPic.carCertPic = "";
            insPic.carInvoicePic = "";

            model.pic = insPic;
            #endregion



            YdtInscarAddbase ydtInscarAddbase = new YdtInscarAddbase(au.token, au.session, YdtPostDataType.Json, model);
            var ydtInscarCarResult = ydtApi.DoPost(ydtInscarAddbase);

            if (ydtInscarCarResult.code != 0)
            {
                return new CustomJsonResult(ResultType.Failure, ydtInscarCarResult.msg);
            }

            var insCarAdvicevalueModel = new InsCarAdvicevalueModel();
            //insCarAdvicevalueModel.startDate = order.PeriodStart.Value.ToString("yyyy-MM-dd");
            //insCarAdvicevalueModel.registDate = order.CarRegisterDate;
            //insCarAdvicevalueModel.replacementValue = order.CarPurchasePrice.Value;
            var ydtInscarAdvicevalue = new YdtInscarAdvicevalue(au.token, au.session, YdtPostDataType.Json, insCarAdvicevalueModel);
            var ydtInscarAdvicevalueResult = ydtApi.DoPost(ydtInscarAdvicevalue);

            if (ydtInscarAdvicevalueResult.code != 0)
            {
                return new CustomJsonResult(ResultType.Failure, ydtInscarAdvicevalueResult.msg);
            }

            YdtInscarInquiryPms insCarInquiryModel = new YdtInscarInquiryPms();
            insCarInquiryModel.auto = 1;
            insCarInquiryModel.orderSeq = ydtInscarCarResult.data.orderSeq;
            insCarInquiryModel.risk = YdtDataMap.GetRisk(kinds);

            if (insCarInquiryModel.risk == 2 || insCarInquiryModel.risk == 3)
            {
                //insCarInquiryModel.ciStartDate = order.PeriodStart.Value.ToString("yyyy-MM-dd");
            }

            if (insCarInquiryModel.risk == 1 || insCarInquiryModel.risk == 3)
            {
                // insCarInquiryModel.biStartDate = order.PeriodStart.Value.ToString("yyyy-MM-dd");
            }


            insCarInquiryModel.coverages = YdtDataMap.GetCoverages(kinds, ydtInscarAdvicevalueResult.data.actualPrice, order.CarRatedPassengerCapacity);


            var YdtInscarGetInquiryInfoModel = new YdtInscarGetInquiryInfoModel();
            YdtInscarGetInquiryInfoModel.orderSeq = ydtInscarCarResult.data.orderSeq;

            var ydtInscarGetInquiryInfo = new YdtInscarGetInquiryInfo(au.token, au.session, YdtPostDataType.Json, YdtInscarGetInquiryInfoModel);
            var ydtInscarGetInquiryInfoResult = ydtApi.DoPost(ydtInscarGetInquiryInfo);


            foreach (var company in offerCompany)
            {
                var insCompany = YdtDataMap.GetCompanyCode(company.InsuranceCompanyId);
                insCarInquiryModel.companyCode = insCompany.YdtCode;//"006000";
                insCarInquiryModel.channelId = insCompany.ChannelId;
                YdtInscarInquiry ydtInscarInquiry = new YdtInscarInquiry(au.token, au.session, YdtPostDataType.Json, insCarInquiryModel);
                var ydtInscarInquiryResult = ydtApi.DoPost(ydtInscarInquiry);

                YdtInscarInquiryOffer offer = new YdtInscarInquiryOffer();



                offer.UplinkInsCompanyId = company.InsuranceCompanyId;
                offer.YdtInsCompanyId = insCarInquiryModel.companyCode;
                offer.Inquiry = ydtInscarInquiryResult;

                if (ydtInscarInquiryResult.code == 0)
                {

                    OfferImgModel offerImgModel = new OfferImgModel();
                    offerImgModel.Company = insCompany.PrintName;
                    offerImgModel.OfferTime = DateTime.Now;
                    offerImgModel.Offerer = "";

                    OfferImgCarInfo offerImgCarInfo = new OfferImgCarInfo();
                    offerImgCarInfo.CarOwner = order.CarownerName;
                    offerImgCarInfo.CarPlateNo = order.CarLicensePlateNo;
                    offerImgCarInfo.CarEngineNo = order.CarEngineNo;
                    offerImgCarInfo.CarVin = order.CarVin;
                    offerImgCarInfo.CarModelName = order.CarModelName;

                    offerImgModel.CarInfo = offerImgCarInfo;

                    string postData = "";
                    if (offer.Inquiry != null)
                    {
                        if (offer.Inquiry.data != null)
                        {
                            if (offer.Inquiry.data.coverages != null)
                            {

                                var coverages = offer.Inquiry.data.coverages;
                                foreach (var coverage in coverages)
                                {
                                    OfferImgCoverage offerImgCoverage = new OfferImgCoverage();
                                    offerImgCoverage.Name = coverage.name;
                                    offerImgCoverage.Discount = coverage.discount;
                                    offerImgCoverage.Premium = coverage.standardPremium;

                                    if (coverage.code == "006")
                                    {
                                        if (coverage.glassType != null)
                                        {
                                            if (coverage.glassType.Value == 1)
                                            {
                                                offerImgCoverage.Coverage = "国产";
                                            }
                                            else
                                            {
                                                offerImgCoverage.Coverage = "进口";
                                            }
                                        }
                                    }
                                    else if (coverage.code == "004")
                                    {
                                        offerImgCoverage.Coverage = coverage.unitAmount.ToF2Price();
                                    }
                                    else if (coverage.name.IndexOf("不计免赔") > -1)
                                    {
                                        offerImgCoverage.Coverage = "";
                                    }
                                    else
                                    {
                                        offerImgCoverage.Coverage = coverage.amount.ToF2Price();
                                    }

                                    offerImgModel.CommercialCoverageInfo.Coverages.Add(offerImgCoverage);
                                }
                                //offerImgModel.CommercialCoverageInfo.PeriodStart = order.PeriodStart;
                                //offerImgModel.CommercialCoverageInfo.PeriodEnd = order.PeriodEnd;
                            }

                            if (offer.Inquiry.data.inquirys != null)
                            {
                                var commercial = offer.Inquiry.data.inquirys.Where(m => m.risk == 1).FirstOrDefault();
                                if (commercial != null)
                                {
                                    offerImgModel.CommercialCoverageInfo.SumPremium = commercial.standardPremium;
                                }

                                var compulsory = offer.Inquiry.data.inquirys.Where(m => m.risk == 2).FirstOrDefault();

                                if (compulsory != null)
                                {
                                    //offerImgModel.CompulsoryInfo.PeriodStart = order.PeriodStart;
                                    //offerImgModel.CompulsoryInfo.PeriodEnd = order.PeriodEnd;
                                    offerImgModel.CompulsoryInfo.Premium = compulsory.standardPremium - compulsory.sumPayTax;
                                    offerImgModel.TravelTax = compulsory.sumPayTax;
                                }
                            }

                            offerImgModel.SumPremium = offer.Inquiry.data.inquirys.Sum(m => m.standardPremium);

                        }
                    }

                    postData = Newtonsoft.Json.JsonConvert.SerializeObject(offerImgModel);

                    int height = 840;
                    if (offer.Inquiry.data.coverages != null)
                    {
                        if (offer.Inquiry.data.coverages.Count > 5)
                        {
                            height = height + (offer.Inquiry.data.coverages.Count - 5) * 40;
                        }
                    }



                    Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail("http://localhost:12060/Biz/CarInsureOffer/OfferImg", 1280, height, 1280, height, postData); //宽高根据要获取快照的网页决定

                    byte[] bytes = Bitmap2Byte(m_Bitmap);

                    string fileExt = ".jpg";
                    UploadFileEntity entity = new UploadFileEntity();
                    entity.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;//自定义文件名称，这里以当前时间为例
                    entity.FileData = bytes;
                    entity.UploadFolder = "offerImg";
                    entity.GenerateSize = false;
                    var rm = HttpClientOperate.Post<CustomJsonResult>(entity.UploadFolder, ConfigurationManager.AppSettings["custom:UploadServerUrl"], entity);//封装的POST提交方
                    if (rm.Result == ResultType.Success)
                    {
                        ImageUpload imageUpload = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageUpload>(rm.Data.ToString());
                        offer.OfferImgUrl = imageUpload.OriginalPath;
                    }
                }


                offerList.Add(offer);
            }

            result.Result = ResultType.Success;
            result.Code = ResultCode.Success;
            result.Data = offerList;

            return result;
        }

        public static YdtIdentityInfo GetIdentityInfoByUrl(string url)
        {
            YdtIdentityInfo identityInfo = null;

            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            var ydtUploadByIdentity = new YdtUploadByIdentity(au.token, au.session, url);
            var ydtUploadByIdentityResult = ydtApi.DoPostFile(ydtUploadByIdentity, Path.GetFileName(url));

            if (ydtUploadByIdentityResult.code == 0)
            {
                if (ydtUploadByIdentityResult.data != null)
                {
                    identityInfo = new YdtIdentityInfo();

                    identityInfo.num = ydtUploadByIdentityResult.data.identity.num;
                    identityInfo.name = ydtUploadByIdentityResult.data.identity.name;
                    identityInfo.sex = ydtUploadByIdentityResult.data.identity.sex;
                    identityInfo.birthday = ydtUploadByIdentityResult.data.identity.birthday;
                    identityInfo.nationality = ydtUploadByIdentityResult.data.identity.nationality;
                    identityInfo.address = ydtUploadByIdentityResult.data.identity.address;
                    identityInfo.fileKey = ydtUploadByIdentityResult.data.file.key;
                }
            }


            return identityInfo;
        }

        public static YdtLicenseInfo GetLicenseInfoByUrl(string url)
        {
            YdtLicenseInfo licenseInfo = null;

            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            var ydtUploadByLicense = new YdtUploadByLicense(au.token, au.session, url);
            var ydtUploadByLicenseResult = ydtApi.DoPostFile(ydtUploadByLicense, Path.GetFileName(url));


            if (ydtUploadByLicenseResult.code == 0)
            {
                if (ydtUploadByLicenseResult.data != null)
                {
                    licenseInfo = new YdtLicenseInfo();

                    licenseInfo.owner = ydtUploadByLicenseResult.data.license.owner;
                    licenseInfo.plateNum = ydtUploadByLicenseResult.data.license.plateNum;
                    licenseInfo.vehicleType = ydtUploadByLicenseResult.data.license.vehicleType;
                    licenseInfo.model = ydtUploadByLicenseResult.data.license.model;
                    licenseInfo.vin = ydtUploadByLicenseResult.data.license.vin;
                    licenseInfo.engineNum = ydtUploadByLicenseResult.data.license.engineNum;
                    licenseInfo.registerDate = ydtUploadByLicenseResult.data.license.registerDate;
                    licenseInfo.issueDate = ydtUploadByLicenseResult.data.license.issueDate;
                    licenseInfo.fileKey = ydtUploadByLicenseResult.data.file.key;


                }
            }

            return licenseInfo;
        }

        public static YdtInsCarApiSearchResultData GetInsCarInfo(string licensePlateNo)
        {
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            var ydtInsCarApiSearch = new YdtInsCarApiSearch(au.token, au.session, licensePlateNo);
            var ydtInsCarApiSearchResult = ydtApi.DoGet(ydtInsCarApiSearch);

            return ydtInsCarApiSearchResult.data;
        }

        public static List<YdtInscarCarResultData> CarModelQuery(string keyword, string vin, string firstRegisterDate)
        {
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();
            YdtInscarCar ydtInscarCar = new YdtInscarCar(au.token, au.session, keyword, vin, firstRegisterDate);
            var ydtInscarCarResult = ydtApi.DoGet(ydtInscarCar);

            return ydtInscarCarResult.data;
        }

        public static YdtInscarGetInquiryInfoResultData GetInquiryInfo(string orderSeq, int areaId)
        {
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();
            var ydtInscarGetInquiryInfoModel = new YdtInscarGetInquiryInfoModel();
            ydtInscarGetInquiryInfoModel.orderSeq = orderSeq;
            ydtInscarGetInquiryInfoModel.areaId = areaId;
            var ydtInscarGetInquiryInfo = new YdtInscarGetInquiryInfo(au.token, au.session, YdtPostDataType.Json, ydtInscarGetInquiryInfoModel);
            var ydtInscarGetInquiryInfoResult = ydtApi.DoPost(ydtInscarGetInquiryInfo);

            return ydtInscarGetInquiryInfoResult.data;
        }

        public static CustomJsonResult<string> EditBaseInfo(YdtInscarEditbasePms model)
        {
            CustomJsonResult<string> result = new CustomJsonResult<string>();

            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            if (string.IsNullOrEmpty(model.orderSeq))
            {
                YdtInscarAddbasePms addModel = new YdtInscarAddbasePms();
                addModel.auto = model.auto;
                addModel.belong = model.belong;
                addModel.carType = model.carType;
                addModel.car = model.car;
                addModel.customers = model.customers;
                addModel.pic = model.pic;
                YdtInscarAddbase ydtInscarAddbase = new YdtInscarAddbase(au.token, au.session, YdtPostDataType.Json, addModel);
                var ydtInscarCarResult = ydtApi.DoPost(ydtInscarAddbase);

                if (ydtInscarCarResult.code != 0)
                {
                    return new CustomJsonResult<string>(ResultType.Failure, ResultCode.Failure, string.Format("{0}({1})", ydtInscarCarResult.msg, ydtInscarCarResult.extmsg), null);
                }

                result.Result = ResultType.Success;
                result.Code = ResultCode.Success;
                result.Data = ydtInscarCarResult.data.orderSeq;
            }
            else
            {
                YdtInscarUpatebase ydtInscarUpdatebase = new YdtInscarUpatebase(au.token, au.session, YdtPostDataType.Json, model);
                var ydtInscarCarResult = ydtApi.DoPost(ydtInscarUpdatebase);

                if (ydtInscarCarResult.code != 0)
                {
                    return new CustomJsonResult<string>(ResultType.Failure, ResultCode.Failure, ydtInscarCarResult.msg, null);
                }

                result.Result = ResultType.Success;
                result.Code = ResultCode.Success;
                result.Data = ydtInscarCarResult.data.orderSeq;
            }

            return result;
        }

        public static CustomJsonResult<YdtInscarInquiryResultData> GetInsInquiryByAuto(YdtInscarInquiryPms model)
        {
            var result = new CustomJsonResult<YdtInscarInquiryResultData>();
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            YdtInscarInquiry ydtInscarInquiry = new YdtInscarInquiry(au.token, au.session, YdtPostDataType.Json, model);
            var ydtInscarInquiryResult = ydtApi.DoPost(ydtInscarInquiry);

            if (ydtInscarInquiryResult.code != 0)
            {
                return new CustomJsonResult<YdtInscarInquiryResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInquiryResult.msg + "(" + ydtInscarInquiryResult.extmsg + ")", null);
            }

            return new CustomJsonResult<YdtInscarInquiryResultData>(ResultType.Success, ResultCode.Success, ydtInscarInquiryResult.msg, ydtInscarInquiryResult.data);
        }

        public static CustomJsonResult<YdtInscarInquiryResultData> GetInsInquiryByArtificial(YdtInscarInquiryPms model)
        {
            var result = new CustomJsonResult<YdtInscarInquiryResultData>();
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            YdtInscarInquiryByArtificial ydtInscarInquiry = new YdtInscarInquiryByArtificial(au.token, au.session, YdtPostDataType.Json, model);
            var ydtInscarInquiryResult = ydtApi.DoPost(ydtInscarInquiry);

            if (ydtInscarInquiryResult.code != 0)
            {
                return new CustomJsonResult<YdtInscarInquiryResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInquiryResult.msg + "(" + ydtInscarInquiryResult.extmsg + ")", null);
            }

            return new CustomJsonResult<YdtInscarInquiryResultData>(ResultType.Success, ResultCode.Success, ydtInscarInquiryResult.msg, ydtInscarInquiryResult.data);
        }

        public static CustomJsonResult<decimal> GetAdviceValue(string startDate, string registDate, decimal replacementValue)
        {
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            var insCarAdvicevalueModel = new InsCarAdvicevalueModel();
            insCarAdvicevalueModel.startDate = startDate;
            insCarAdvicevalueModel.registDate = registDate;
            insCarAdvicevalueModel.replacementValue = replacementValue;

            var ydtInscarAdvicevalue = new YdtInscarAdvicevalue(au.token, au.session, YdtPostDataType.Json, insCarAdvicevalueModel);
            var ydtInscarAdvicevalueResult = ydtApi.DoPost(ydtInscarAdvicevalue);

            if (ydtInscarAdvicevalueResult.code != 0)
            {
                return new CustomJsonResult<decimal>(ResultType.Failure, ResultCode.Failure, ydtInscarAdvicevalueResult.msg, 0);
            }

            return new CustomJsonResult<decimal>(ResultType.Success, ResultCode.Success, ydtInscarAdvicevalueResult.msg, ydtInscarAdvicevalueResult.data.actualPrice);
        }


        public static CustomJsonResult<YdtInsCarQueryInquiryResultData> QueryInquiry(string orderSeq, string inquirySeq)
        {
            var result = new CustomJsonResult();
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();


            var ydtInsCarQueryInquiry = new YdtInsCarQueryInquiry(au.token, au.session, inquirySeq, orderSeq);
            var ydtInsCarQueryInquiryResult = ydtApi.DoGet(ydtInsCarQueryInquiry);

            if (ydtInsCarQueryInquiryResult.code != 0)
            {
                return new CustomJsonResult<YdtInsCarQueryInquiryResultData>(ResultType.Failure, ResultCode.Failure, ydtInsCarQueryInquiryResult.msg, null);
            }

            return new CustomJsonResult<YdtInsCarQueryInquiryResultData>(ResultType.Success, ResultCode.Success, ydtInsCarQueryInquiryResult.msg, ydtInsCarQueryInquiryResult.data);

        }


        public static CustomJsonResult<YdtInscarInsureResultData> Insure(YdtInscarInsurePms model)
        {
            var result = new CustomJsonResult<YdtInscarInsureResultData>();
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            var ydtInscarInsure = new YdtInscarInsure(au.token, au.session, YdtPostDataType.Json, model);
            var ydtInscarInsureResult = ydtApi.DoPost(ydtInscarInsure);

            if (ydtInscarInsureResult.code != 0)
            {
                return new CustomJsonResult<YdtInscarInsureResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInsureResult.msg + "(" + ydtInscarInsureResult.extmsg + ")", null);
            }

            return new CustomJsonResult<YdtInscarInsureResultData>(ResultType.Success, ResultCode.Success, ydtInscarInsureResult.msg, ydtInscarInsureResult.data);
        }

        public static CustomJsonResult<YdtInscarPayResultData> Pay(YdtInscarPayPms model)
        {
            var result = new CustomJsonResult<YdtInscarInsureResultData>();
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            var ydtInscarInsure = new YdtInscarPay(au.token, au.session, YdtPostDataType.Json, model);
            var ydtInscarInsureResult = ydtApi.DoPost(ydtInscarInsure);

            if (ydtInscarInsureResult.code != 0)
            {
                return new CustomJsonResult<YdtInscarPayResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInsureResult.msg + "(" + ydtInscarInsureResult.extmsg + ")", null);
            }

            return new CustomJsonResult<YdtInscarPayResultData>(ResultType.Success, ResultCode.Success, ydtInscarInsureResult.msg, ydtInscarInsureResult.data);
        }
    }
}
