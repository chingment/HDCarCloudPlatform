using Lumos.BLL;
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
using WebAppApi.Models.Banner;
using WebAppApi.Models.CarService;
using WebAppApi.Models.Product;

namespace WebAppApi.Controllers
{
    public class AccountController : BaseApiController
    {
        [HttpGet]
        public APIResponse CheckUserName(string userName)
        {
            var clientUser = CurrentDb.SysClientUser.Where(m => m.UserName == userName).FirstOrDefault();
            if (clientUser == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureUserNameNotExists, "用户名不存在");
            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "有效用户名");
        }

        [HttpPost]
        public APIResponse ResetPassword(ResetPasswordModel model)
        {

            var clientUser = CurrentDb.SysClientUser.Where(m => m.UserName == model.UserName).FirstOrDefault();
            if (clientUser == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureUserNameNotExists, "用户名不存在");
            }

            var token = CurrentDb.SysSmsSendHistory.Where(m => m.Token == model.Token && m.ValidCode == model.ValidCode && m.IsUse == false && m.ExpireTime >= DateTime.Now).FirstOrDefault();
            if (token == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "验证码错误");
            }

            token.IsUse = true;


            clientUser.PasswordHash = PassWordHelper.HashPassword(model.Password);
            clientUser.LastUpdateTime = DateTime.Now;
            clientUser.Mender = clientUser.Id;
            CurrentDb.SaveChanges();

            return ResponseResult(ResultType.Success, ResultCode.Success, "重置成功");

        }

        [HttpPost]
        public APIResponse Login(LoginModel model)
        {
            if (model.UserName.IndexOf("YW") > -1)
            {
                return SalesmanLogin(model);
            }
            else
            {
                return ClientLogin(model);
            }

        }

        private APIResponse ClientLogin(LoginModel model)
        {
            var clientUser = CurrentDb.SysClientUser.Where(m => m.UserName == model.UserName).FirstOrDefault();
            if (clientUser == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户名不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(clientUser.PasswordHash, model.Password))
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户密码错误");
            }

            ClientLoginResultModel resultModel = new ClientLoginResultModel(clientUser, model.DeviceId);
            if (resultModel == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，设备与用户不匹配");
            }

            if (resultModel.PosMachineStatus == Enumeration.MerchantPosMachineStatus.NotMatch)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，设备与用户不匹配");
            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "登录成功", resultModel);
        }

        private APIResponse SalesmanLogin(LoginModel model)
        {
            var salesman = CurrentDb.SysSalesmanUser.Where(m => m.UserName == model.UserName).FirstOrDefault();
            if (salesman == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户名不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(salesman.PasswordHash, model.Password))
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户密码错误");
            }

            SalesmanLoginResultModel resultModel = new SalesmanLoginResultModel(salesman, model.DeviceId, this.SalesmanMerchantId);

            return ResponseResult(ResultType.Success, ResultCode.Success, "登录成功", resultModel);
        }

        public APIResponse Edit()
        {
            APIResult result = new APIResult() { Result = ResultType.Exception, Code = ResultCode.Failure, Message = "NULL" };
            return new APIResponse(result);
        }

        [HttpPost]
        public APIResponse ChangePassword(ChangePasswordModel model)
        {

            var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == model.UserId).FirstOrDefault();
            if (clientUser == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureUserNameNotExists, "用户名不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(clientUser.PasswordHash, model.OldPassword))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "修改失败，旧密码错误");
            }

            clientUser.PasswordHash = PassWordHelper.HashPassword(model.NewPassword);
            clientUser.Mender = model.UserId;
            clientUser.LastUpdateTime = DateTime.Now;
            CurrentDb.SaveChanges();

            return ResponseResult(ResultType.Success, ResultCode.Success, "修改成功");
        }

        [HttpGet]
        public APIResponse Home(int userId, int merchantId)
        {

            HomeModel model = new HomeModel();

            #region 获取bannder
            var banner = CurrentDb.SysBanner.Where(m => m.Type == Enumeration.BannerType.MainHomeTop && m.Status == Enumeration.SysBannerStatus.Release).ToList();

            List<BannerImageModel> bannerImageModel = new List<BannerImageModel>();

            foreach (var m in banner)
            {
                BannerImageModel imageModel = new BannerImageModel();
                imageModel.Id = m.Id;
                imageModel.Title = m.Title;
                imageModel.LinkUrl = SysFactory.Banner.GetLinkUrl(m.Id);
                imageModel.ImgUrl = m.ImgUrl;
                bannerImageModel.Add(imageModel);
            }

            model.Banner = bannerImageModel;

            #endregion

            #region 获取 投保公司,理赔公司

            var carInsCompanys = (from u in CurrentDb.CarInsuranceCompany
                                  join r in CurrentDb.InsuranceCompany on u.InsuranceCompanyId equals r.Id
                                  where u.Status == Enumeration.CarInsuranceCompanyStatus.Normal
                                  select new { r.Id, r.Name, u.InsuranceCompanyImgUrl,u.CanClaims,u.CanInsure, u.Priority }).Distinct().OrderByDescending(m => m.Priority);

            List<CarInsCompanyModel> carInsCompanyModels = new List<CarInsCompanyModel>();

            foreach (var carInsCompany in carInsCompanys)
            {
                CarInsCompanyModel carInsCompanyModel = new CarInsCompanyModel();
                carInsCompanyModel.Id = carInsCompany.Id;
                carInsCompanyModel.Name = carInsCompany.Name;
                carInsCompanyModel.ImgUrl = carInsCompany.InsuranceCompanyImgUrl;
                carInsCompanyModel.CanClaims = carInsCompany.CanClaims;
                carInsCompanyModel.CanInsure = carInsCompany.CanInsure;
                carInsCompanyModels.Add(carInsCompanyModel);
            }

            model.CarInsCompany = carInsCompanyModels;
            #endregion

            #region 获取车险险种

            List<CarInsKindModel> carInsKindModels = new List<CarInsKindModel>();

            var carKinds = CurrentDb.CarKind.OrderByDescending(m => m.Priority).ToList();

            foreach (var carKind in carKinds)
            {
                CarInsKindModel carInsKindModel = new CarInsKindModel();
                carInsKindModel.Id = carKind.Id;
                carInsKindModel.PId = carKind.PId;
                carInsKindModel.Name = carKind.Name;
                carInsKindModel.AliasName = carKind.AliasName;
                carInsKindModel.Type = carKind.Type;
                carInsKindModel.CanWaiverDeductible = carKind.CanWaiverDeductible;
                carInsKindModel.IsWaiverDeductible = carKind.IsWaiverDeductible;
                carInsKindModel.InputType = carKind.InputType;
                carInsKindModel.InputUnit = carKind.InputUnit;
                carInsKindModel.InputValue = carKind.InputValue;
                if (!string.IsNullOrEmpty(carKind.InputOption))
                {
                    carInsKindModel.InputOption = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(carKind.InputOption);
                }
            
                carInsKindModel.IsHasDetails = carKind.IsHasDetails;
                carInsKindModel.IsCheck = carKind.IsCheck;
                carInsKindModels.Add(carInsKindModel);
            }



            model.CarInsKind = carInsKindModels;

            #endregion

            #region 获取车险的投保方案

            List<CarInsPlanModel> carInsPlanModels = new List<CarInsPlanModel>();

            var carInsurePlans = CurrentDb.CarInsurePlan.Where(m => m.IsDelete == false).ToList();

            var carInsurePlanKinds = CurrentDb.CarInsurePlanKind.ToList();

            foreach (var carInsurePlan in carInsurePlans)
            {
                CarInsPlanModel carInsPlanModel = new CarInsPlanModel();

                carInsPlanModel.Id = carInsurePlan.Id;
                carInsPlanModel.Name = carInsurePlan.Name;
                carInsPlanModel.ImgUrl = carInsurePlan.ImgUrl;

                var carKindIds = carInsurePlanKinds.Where(m => m.CarInsurePlanId == carInsurePlan.Id).Select(m => m.CarKindId).ToArray();

                var carInsurePlanKindParentKindIds = carKinds.Where(m => carKindIds.Contains(m.Id)&&m.PId==0).Select(m=>m.Id).ToList();

                List<CarInsPlanKindParentModel> carInsPlanKindParentModels = new List<CarInsPlanKindParentModel>();

                foreach (var carInsurePlanKindParentKindId in carInsurePlanKindParentKindIds)
                {
                    CarInsPlanKindParentModel carInsPlanKindParentModel = new CarInsPlanKindParentModel();

                    carInsPlanKindParentModel.Id = carInsurePlanKindParentKindId;

                    var carInsurePlanKindChildKindIds = carKinds.Where(m => carKindIds.Contains(m.Id) && m.PId == carInsurePlanKindParentKindId).Select(m => m.Id).ToList();

                    carInsPlanKindParentModel.Child = carInsurePlanKindChildKindIds;

                    carInsPlanKindParentModels.Add(carInsPlanKindParentModel);
                }

                carInsPlanModel.KindParent = carInsPlanKindParentModels;

                carInsPlanModels.Add(carInsPlanModel);
            }

            model.CarInsPlan = carInsPlanModels;

            #endregion

 

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
            return new APIResponse(result);
        }

    }
}