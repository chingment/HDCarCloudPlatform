using AnXinSdk;
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
    public class CarServiceController : BaseApiController
    {

        //提交投保单
        [HttpPost]
        public APIResponse SubmitInsure(SubmitInsureModel model)
        {

            OrderToCarInsure orderToCarInsure = new OrderToCarInsure();

            orderToCarInsure.UserId = model.UserId;
            orderToCarInsure.MerchantId = model.MerchantId;
            orderToCarInsure.PosMachineId = model.PosMachineId;
            orderToCarInsure.InsurePlanId = model.InsurePlanId;
            orderToCarInsure.ProductType = model.Type;


            if (model.ImgData != null)
            {
                var key_CZ_CL_XSZ_Img = "CZ_CL_XSZ_Img";
                if (model.ImgData.ContainsKey(key_CZ_CL_XSZ_Img))
                {
                    orderToCarInsure.CZ_CL_XSZ_ImgUrl = GetUploadImageUrl(model.ImgData[key_CZ_CL_XSZ_Img], "CarInsure");

                    //DrivingLicenceVO drivingLicence = AnXin.GetDrivingLicenceByImageBase64(model.ImgData[key_CZ_CL_XSZ_Img].Data);

                    //if (drivingLicence != null)
                    //{
                    //    orderToCarInsure.CarOwner = drivingLicence.owner;
                    //    orderToCarInsure.CarOwnerAddress = drivingLicence.address;
                    //    orderToCarInsure.CarModel = drivingLicence.model;
                    //    orderToCarInsure.CarPlateNo = drivingLicence.plateNum;
                    //    orderToCarInsure.CarEngineNo = drivingLicence.engineNo;
                    //    orderToCarInsure.CarVin = drivingLicence.vin;
                    //    //orderToCarInsure.CarVechicheType = drivingLicence.vehicleType;
                    //    orderToCarInsure.CarIssueDate = drivingLicence.issueDate;
                    //    //orderToCarInsure.CarUserCharacter = drivingLicence.userCharacter;
                    //    orderToCarInsure.CarRegisterDate = drivingLicence.registerDate;
                    //}

                }

                var key_CZ_SFZ_Img = "CZ_SFZ_Img";
                if (model.ImgData.ContainsKey(key_CZ_SFZ_Img))
                {
                    orderToCarInsure.CZ_SFZ_ImgUrl = GetUploadImageUrl(model.ImgData[key_CZ_SFZ_Img], "CarInsure");

                    //IdentityCardVO identityCardVO = AnXin.GetIdentityCardByImageBase64(model.ImgData[key_CZ_SFZ_Img].Data);

                    //if (identityCardVO != null)
                    //{
                    //    orderToCarInsure.CarOwnerIdNumber = identityCardVO.idNumber;
                    //}

                }

                var key_CCSJM_WSZM_Img = "CCSJM_WSZM_Img";
                if (model.ImgData.ContainsKey(key_CCSJM_WSZM_Img))
                {
                    orderToCarInsure.CCSJM_WSZM_ImgUrl = GetUploadImageUrl(model.ImgData[key_CCSJM_WSZM_Img], "CarInsure");
                }

                var key_YCZ_CLDJZ_Img = "YCZ_CLDJZ_Img";
                if (model.ImgData.ContainsKey(key_YCZ_CLDJZ_Img))
                {
                    orderToCarInsure.YCZ_CLDJZ_ImgUrl = GetUploadImageUrl(model.ImgData[key_YCZ_CLDJZ_Img], "CarInsure");
                }
            }

            List<OrderToCarInsureOfferCompany> orderToCarInsureOfferCompanys = new List<OrderToCarInsureOfferCompany>();

            var insureOfferCompanys = CurrentDb.InsuranceCompany.ToList();
            foreach (var m in model.InsuranceCompanyId)
            {
                var insureOfferCompany = insureOfferCompanys.Where(q => q.Id == m).FirstOrDefault();
                OrderToCarInsureOfferCompany orderToCarInsureOfferCompany = new OrderToCarInsureOfferCompany();
                orderToCarInsureOfferCompany.InsuranceCompanyId = insureOfferCompany.Id;
                orderToCarInsureOfferCompany.InsuranceCompanyName = insureOfferCompany.Name;
                orderToCarInsureOfferCompany.InsuranceCompanyImgUrl = insureOfferCompany.ImgUrl;
                orderToCarInsureOfferCompanys.Add(orderToCarInsureOfferCompany);
            }

            List<OrderToCarInsureOfferKind> orderToCarInsureOfferKinds = new List<OrderToCarInsureOfferKind>();

            foreach (var m in model.InsureKind)
            {
                OrderToCarInsureOfferKind orderToCarInsureOfferKind = new OrderToCarInsureOfferKind();
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
            //业务人员模拟数据
            if (model.MerchantId == this.SalesmanMerchantId)
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