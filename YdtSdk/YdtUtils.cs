using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
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
            TokenAu au = new TokenAu();
            YdtApi c = new YdtApi();
            YdtToken ydtToken = new YdtToken("hylian", "hylian_2018");
            var ydtTokenResult = c.DoGet(ydtToken);

            // ydtTokenResult


            YdtEmLogin ydtEmLogin = new YdtEmLogin(ydtTokenResult.data.token, "15012405333", "7c4a8d09ca3762af61e59520943dc26494f8941b");
            var ydtEmLoginResult = c.DoGet(ydtEmLogin);

            au.token = ydtTokenResult.data.token;
            au.session = ydtEmLoginResult.data.session;
            return au;

        }


        public static CustomJsonResult GetCarInsOffer(OrderToCarInsure order, List<OrderToCarInsureOfferCompany> offerCompany, List<OrderToCarInsureOfferKind> kinds)
        {

            CustomJsonResult result = new CustomJsonResult();

            List<YdtInscarInquiryOffer> offerList = new List<YdtInscarInquiryOffer>();

            order.RecipientAddress = "测试地址";
            order.RecipientPhoneNumber = "13800138000";


            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            InscarAddbaseModel model = new InscarAddbaseModel();

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
            InsCarInfoModel carInfo = new InsCarInfoModel();
            carInfo.licensePlateNo = order.CarPlateNo;
            carInfo.vin = order.CarVin;
            carInfo.engineNo = order.CarEngineNo;
            carInfo.modelCode = order.CarModel;
            carInfo.modelName = order.CarModelName;
            carInfo.firstRegisterDate = order.CarRegisterDate;
            // carInfo.displacement = ;
            // carInfo.marketYear = ;

            //switch (order.CarSeat)
            //{
            //    case Enumeration.CarSeat.S6:
            carInfo.ratedPassengerCapacity = order.CarSeat;
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

            carInfo.ratedPassengerCapacity = order.CarSeat;
            carInfo.replacementValue = order.CarPurchasePrice.Value;
            carInfo.licensePlateNo = order.CarPlateNo;

            //todo
            carInfo.replacementValue = 0;
            carInfo.chgownerType = "0";




            model.car = carInfo;

            #endregion



            #region 被保人，投保人，车主
            List<InsCustomers> customers = new List<InsCustomers>();

            InsCustomers insured = new InsCustomers();
            insured.insuredFlag = 1;
            insured.name = order.CarOwner;
            insured.certNo = order.CarOwnerIdNumber;
            insured.mobile = order.RecipientPhoneNumber;
            insured.address = order.RecipientAddress;

            YdtUpload ydtUpdate_SFZ = new YdtUpload(au.token, au.session, "1", order.CZ_SFZ_ImgUrl);
            var ydtUpdateResult_SFZ = ydtApi.DoPostFile(ydtUpdate_SFZ, Path.GetFileName(order.CZ_SFZ_ImgUrl));

            if (ydtUpdateResult_SFZ.code != 0)
            {
                return new CustomJsonResult(ResultType.Failure, ydtUpdateResult_SFZ.msg);
            }

            insured.identityFacePic = ydtUpdateResult_SFZ.data.file.key;
            insured.identityBackPic = ydtUpdateResult_SFZ.data.file.key;

            InsCustomers holder = new InsCustomers();
            holder.insuredFlag = 2;
            holder.name = insured.name;
            holder.certNo = insured.certNo;
            holder.mobile = insured.mobile;
            holder.address = insured.address;
            holder.identityFacePic = insured.identityFacePic;
            holder.identityBackPic = insured.identityBackPic;

            InsCustomers carOwner = new InsCustomers();
            carOwner.insuredFlag = 3;
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


            var ydtUpdate = new YdtUpload(au.token, au.session, "1", order.CZ_SFZ_ImgUrl);
            var ydtUpdateResult = ydtApi.DoPostFile(ydtUpdate, Path.GetFileName(order.CZ_SFZ_ImgUrl));
            if (ydtUpdateResult.code != 0)
            {
                return new CustomJsonResult(ResultType.Failure, ydtUpdateResult.msg);
            }

            #region pic
            InsPicModel insPic = new InsPicModel();
            insPic.licensePic = ydtUpdateResult.data.file.key;
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
            insCarAdvicevalueModel.startDate = order.PeriodStart.Value.ToString("yyyy-MM-dd");
            insCarAdvicevalueModel.registDate = order.CarRegisterDate;
            insCarAdvicevalueModel.replacementValue = order.CarPurchasePrice.Value;
            var ydtInscarAdvicevalue = new YdtInscarAdvicevalue(au.token, au.session, YdtPostDataType.Json, insCarAdvicevalueModel);
            var ydtInscarAdvicevalueResult = ydtApi.DoPost(ydtInscarAdvicevalue);

            if (ydtInscarAdvicevalueResult.code != 0)
            {
                return new CustomJsonResult(ResultType.Failure, ydtInscarAdvicevalueResult.msg);
            }

            InsCarInquiryModel insCarInquiryModel = new InsCarInquiryModel();
            insCarInquiryModel.auto = 1;
            insCarInquiryModel.orderSeq = ydtInscarCarResult.data.orderSeq;
            insCarInquiryModel.risk = YdtDataMap.GetRisk(kinds);

            if (insCarInquiryModel.risk == 2 || insCarInquiryModel.risk == 3)
            {
                insCarInquiryModel.ciStartDate = order.PeriodStart.Value.ToString("yyyy-MM-dd");
            }

            if (insCarInquiryModel.risk == 1 || insCarInquiryModel.risk == 3)
            {
                insCarInquiryModel.biStartDate = order.PeriodStart.Value.ToString("yyyy-MM-dd");
            }


            insCarInquiryModel.coverages = YdtDataMap.GetCoverages(kinds, ydtInscarAdvicevalueResult.data.actualPrice, order.CarSeat);


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
                    offerImgCarInfo.CarOwner = order.CarOwner;
                    offerImgCarInfo.CarPlateNo = order.CarPlateNo;
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
                                    else if(coverage.code == "004")
                                    {
                                        offerImgCoverage.Coverage = coverage.unitAmount.ToF2Price();
                                    }
                                    else if (coverage.name.IndexOf("不计免赔") > -1)
                                    {
                                        offerImgCoverage.Coverage = "";
                                    }
                                    else
                                    {
                                        if (coverage.amount != null)
                                        {
                                            offerImgCoverage.Coverage = coverage.amount.Value.ToF2Price();
                                        }
                                    }

                                    offerImgModel.CommercialCoverageInfo.Coverages.Add(offerImgCoverage);
                                }
                                offerImgModel.CommercialCoverageInfo.PeriodStart = order.PeriodStart;
                                offerImgModel.CommercialCoverageInfo.PeriodEnd = order.PeriodEnd;
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
                                    offerImgModel.CompulsoryInfo.PeriodStart = order.PeriodStart;
                                    offerImgModel.CompulsoryInfo.PeriodEnd = order.PeriodEnd;
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
    }
}
