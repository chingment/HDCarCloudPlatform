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


        public static YdtUploadResultData UploadImg(string url)
        {

            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            var ydtUpload = new YdtUpload(au.token, au.session, "1", url);
            var ydtUploadReuslt = ydtApi.DoPostFile(ydtUpload, Path.GetFileName(url));

            if (ydtUploadReuslt.code != 0)
            {
                return null;
            }

            if (ydtUploadReuslt == null)
            {
                return null;
            }

            return ydtUploadReuslt.data;
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
                    return new CustomJsonResult<string>(ResultType.Failure, ResultCode.Failure, ydtInscarCarResult.extmsg, null);
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
                    return new CustomJsonResult<string>(ResultType.Failure, ResultCode.Failure, ydtInscarCarResult.extmsg, null);
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
                return new CustomJsonResult<YdtInscarInquiryResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInquiryResult.extmsg, null);
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
                return new CustomJsonResult<YdtInscarInquiryResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInquiryResult.extmsg, null);
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
                return new CustomJsonResult<YdtInscarInsureResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInsureResult.extmsg, null);
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
                return new CustomJsonResult<YdtInscarPayResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInsureResult.extmsg, null);
            }

            return new CustomJsonResult<YdtInscarPayResultData>(ResultType.Success, ResultCode.Success, ydtInscarInsureResult.msg, ydtInscarInsureResult.data);
        }


        public static CustomJsonResult<YdtInscarPayQueryResultData> PayQuery(YdtInscarPayQueryPms model)
        {
            var result = new CustomJsonResult<YdtInscarPayQueryResultData>();
            var au = YdtUtils.GetToken();
            YdtApi ydtApi = new YdtApi();

            var ydtInscarInsure = new YdtInscarPayQuery(au.token, au.session, model.orderSeq, model.inquirySeq, model.insureSeq, model.paySeq);
            var ydtInscarInsureResult = ydtApi.DoGet(ydtInscarInsure);

            if (ydtInscarInsureResult.code != 0)
            {
                return new CustomJsonResult<YdtInscarPayQueryResultData>(ResultType.Failure, ResultCode.Failure, ydtInscarInsureResult.extmsg, null);
            }

            return new CustomJsonResult<YdtInscarPayQueryResultData>(ResultType.Success, ResultCode.Success, ydtInscarInsureResult.msg, ydtInscarInsureResult.data);
        }
    }
}
