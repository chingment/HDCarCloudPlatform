
using log4net;
using Lumos.BLL;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppApi.Models.CarService;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class CarServiceController : OwnBaseApiController
    {

        //提交投保单
        [HttpPost]
        public APIResponse SubmitInsure(SubmitInsureModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToCarInsure orderToCarInsure = new OrderToCarInsure();

            orderToCarInsure.UserId = model.UserId;
            orderToCarInsure.MerchantId = model.MerchantId;
            orderToCarInsure.PosMachineId = model.PosMachineId;
            orderToCarInsure.InsPlanId = model.InsurePlanId;
            orderToCarInsure.Type = Enumeration.OrderType.InsureForCarForInsure;
            orderToCarInsure.TypeName = Enumeration.OrderType.InsureForCarForInsure.GetCnName();

            if (model.ImgData != null)
            {
                var key_CZ_CL_XSZ_Img = "CZ_CL_XSZ_Img";
                if (model.ImgData.ContainsKey(key_CZ_CL_XSZ_Img))
                {
                    orderToCarInsure.CarLicensePicUrl = GetUploadImageUrl(model.ImgData[key_CZ_CL_XSZ_Img], "CarInsure");
                    //orderToCarInsure.CZ_CL_XSZ_ImgUrl = "http://file.gzhaoyilian.com/Upload/demo_jsz.jpg";
                    var drivingLicenceInfo = BizFactory.CarInsureOffer.GetDrivingLicenceInfoFromImgUrl(orderToCarInsure.CarLicensePicUrl);
                    if (drivingLicenceInfo != null)
                    {
                        orderToCarInsure.CarownerName = drivingLicenceInfo.owner;
                        orderToCarInsure.CarownerAddress = drivingLicenceInfo.address;
                        orderToCarInsure.CarModelCode = drivingLicenceInfo.model;
                        orderToCarInsure.CarLicensePlateNo = drivingLicenceInfo.plateNum;
                        orderToCarInsure.CarEngineNo = drivingLicenceInfo.engineNo;
                        orderToCarInsure.CarVin = drivingLicenceInfo.vin;
                        //orderToCarInsure.CarVechicheType = drivingLicence.vehicleType;
                        //orderToCarInsure.CarUserCharacter = drivingLicence.userCharacter;
                        if (CommonUtils.IsDateTime(drivingLicenceInfo.issueDate))
                        {
                            orderToCarInsure.CarIssueDate =DateTime.Parse(drivingLicenceInfo.issueDate).ToUnifiedFormatDate();
                        }
                        if (CommonUtils.IsDateTime(drivingLicenceInfo.registerDate))
                        {
                            orderToCarInsure.CarFirstRegisterDate = DateTime.Parse(drivingLicenceInfo.registerDate).ToUnifiedFormatDate();
                        }
                        orderToCarInsure.CarLicensePicKey = drivingLicenceInfo.fileKey;
                    }

                }

                var key_CZ_SFZ_Img = "CZ_SFZ_Img";
                if (model.ImgData.ContainsKey(key_CZ_SFZ_Img))
                {
                    //orderToCarInsure.CZ_SFZ_ImgUrl = "http://file.gzhaoyilian.com/Upload/demo_sfz.jpg";
                    orderToCarInsure.CarownerIdentityFacePicUrl = GetUploadImageUrl(model.ImgData[key_CZ_SFZ_Img], "CarInsure");
                    var identityCardInfo = BizFactory.CarInsureOffer.GetIdentityCardInfoFromImgUrl(orderToCarInsure.CarownerIdentityFacePicUrl);
                    if (identityCardInfo != null)
                    {
                        orderToCarInsure.CarownerCertNo = identityCardInfo.idNumber;
                        orderToCarInsure.CarownerIdentityFacePicKey = identityCardInfo.fileKey;
                    }
                }

            }

            List<OrderToCarInsureOfferCompany> orderToCarInsureOfferCompanys = new List<OrderToCarInsureOfferCompany>();

            var insureOfferCompanys = CurrentDb.Company.ToList();
            foreach (var m in model.InsuranceCompanyId)
            {
                var insureOfferCompany = insureOfferCompanys.Where(q => q.Id == m).FirstOrDefault();
                OrderToCarInsureOfferCompany orderToCarInsureOfferCompany = new OrderToCarInsureOfferCompany();
                orderToCarInsureOfferCompany.InsuranceCompanyId = insureOfferCompany.Id;
                orderToCarInsureOfferCompany.InsuranceCompanyName = insureOfferCompany.Name;
                orderToCarInsureOfferCompany.InsuranceCompanyImgUrl = insureOfferCompany.ImgUrl;
                orderToCarInsureOfferCompanys.Add(orderToCarInsureOfferCompany);
            }

            List<OrderToCarInsureOfferCompanyKind> orderToCarInsureOfferKinds = new List<OrderToCarInsureOfferCompanyKind>();

            foreach (var m in model.InsureKind)
            {
                OrderToCarInsureOfferCompanyKind orderToCarInsureOfferKind = new OrderToCarInsureOfferCompanyKind();
                orderToCarInsureOfferKind.KindId = m.Id;
                orderToCarInsureOfferKind.KindValue = m.Value;
                orderToCarInsureOfferKind.KindDetails = m.Details;
                orderToCarInsureOfferKind.IsWaiverDeductible = m.IsWaiverDeductible;
                orderToCarInsureOfferKinds.Add(orderToCarInsureOfferKind);
            }

            IResult result = BizFactory.Order.SubmitCarInsure(model.UserId, orderToCarInsure, orderToCarInsureOfferCompanys, orderToCarInsureOfferKinds);

            return new APIResponse(result);

        }

        //提交跟进的投保单
        [HttpPost]
        public APIResponse SubmitFollowInsure(SubmitFollowInsureModel model)
        {
            OrderToCarInsure orderToCarInsure = new OrderToCarInsure();

            orderToCarInsure.Id = model.OrderId;

            if (model.ImgData != null)
            {
                var key_ZJ1_Img = "ZJ1_Img";
                if (model.ImgData.ContainsKey(key_ZJ1_Img))
                {
                    orderToCarInsure.ZJ1_ImgUrl = GetUploadImageUrl(model.ImgData[key_ZJ1_Img], "CarInsure");
                }

                var key_ZJ2_Img = "ZJ2_Img";
                if (model.ImgData.ContainsKey(key_ZJ2_Img))
                {
                    orderToCarInsure.ZJ2_ImgUrl = GetUploadImageUrl(model.ImgData[key_ZJ2_Img], "CarInsure");
                }

                var key_ZJ3_Img = "ZJ3_Img";
                if (model.ImgData.ContainsKey(key_ZJ3_Img))
                {
                    orderToCarInsure.ZJ3_ImgUrl = GetUploadImageUrl(model.ImgData[key_ZJ3_Img], "CarInsure");
                }


                var key_ZJ4_Img = "ZJ4_Img";
                if (model.ImgData.ContainsKey(key_ZJ4_Img))
                {
                    orderToCarInsure.ZJ4_ImgUrl = GetUploadImageUrl(model.ImgData[key_ZJ4_Img], "CarInsure");
                }

            }



            IResult result = BizFactory.Order.SubmitFollowInsure(model.UserId, orderToCarInsure);
            return new APIResponse(result);
        }

        //提交理赔需求
        [HttpPost]
        public APIResponse SubmitClaim(SubmitClaimModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            OrderToCarClaim orderToCarClaim = new OrderToCarClaim();
            orderToCarClaim.PosMachineId = model.PosMachineId;
            orderToCarClaim.RepairsType = model.RepairsType;
            orderToCarClaim.CarPlateNo = model.CarLicenseNumber;
            orderToCarClaim.HandPerson = model.HandPerson;
            orderToCarClaim.HandPersonPhone = model.HandPersonPhone;
            orderToCarClaim.InsuranceCompanyId = model.InsuranceCompanyId;
            orderToCarClaim.ClientRequire = model.ClientRequire;
            IResult result = BizFactory.Order.SubmitClaim(model.UserId, model.UserId, orderToCarClaim);

            return new APIResponse(result);
        }

        //提交定损单
        [HttpPost]
        public APIResponse SubmitEstimateList(SubmitEstimateListModel model)
        {
            if (IsSaleman(model.UserId))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "该用户为业务员，不能提交订单");
            }

            string estimateListImg = "";
            if (model.ImgData != null)
            {
                var key_EstimateListImg = "estimateListImg";
                if (model.ImgData.ContainsKey(key_EstimateListImg))
                {
                    estimateListImg = GetUploadImageUrl(model.ImgData[key_EstimateListImg], "CarInsure");
                }
            }

            IResult result = BizFactory.Order.SubmitEstimateList(model.UserId, model.UserId, model.OrderId, estimateListImg);

            return new APIResponse(result);
        }

    }
}