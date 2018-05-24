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
using WebAppApi.Models.CarService;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class AccountController : OwnBaseApiController
    {

        public APIResponse Create(CreateModel model)
        {
            IResult result = BizFactory.Merchant.CreateAccount(0, model.Token, model.ValidCode, model.UserName, model.Password, model.DeviceId);

            return new APIResponse(result);

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
            if (model.UserName.ToLower().IndexOf("ag") > -1)
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
            string testAccount = "15989287032";
            string testDeviceId = "000000000000000";

            var clientUser = CurrentDb.SysClientUser.Where(m => m.UserName == model.UserName).FirstOrDefault();
            if (clientUser == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户名不存在");
            }


            if (!PassWordHelper.VerifyHashedPassword(clientUser.PasswordHash, model.Password))
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户密码错误");
            }


            var posMachine = CurrentDb.PosMachine.Where(m => m.DeviceId == model.DeviceId).FirstOrDefault();

            if (model.UserName != testAccount)
            {
                if (posMachine == null)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，设备没有注册");
                }
            }
            else
            {
                posMachine = CurrentDb.PosMachine.Where(m => m.DeviceId == testDeviceId).FirstOrDefault();
            }

            var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.UserId == clientUser.Id && m.MerchantId == clientUser.MerchantId).FirstOrDefault();

            if (merchantPosMachine == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，设备与用户不匹配");
            }

            if (merchantPosMachine.PosMachineId != posMachine.Id)
            {
                //内测账号，不验证设备ID
                if (model.UserName != testAccount)
                {
                    return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，设备与用户不匹配");
                }
            }

            if (merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.Unknow)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户状态异常");
            }

            LoginResultModel resultModel = new LoginResultModel();

            if (merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.Normal)
            {
                resultModel.Status = ClientLoginStatus.Normal;
            }
            else if (merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.NoActive)
            {
                resultModel.Status = ClientLoginStatus.NoActive;
            }
            else if (merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.Expiry)
            {
                resultModel.Status = ClientLoginStatus.Expiry;
            }
            else if (merchantPosMachine.ExpiryTime < DateTime.Now)
            {
                resultModel.Status = ClientLoginStatus.Expiry;
                merchantPosMachine.Status = Enumeration.MerchantPosMachineStatus.Expiry;
                CurrentDb.SaveChanges();
            }

            resultModel.UserId = clientUser.Id;
            resultModel.UserName = clientUser.UserName;
            resultModel.MerchantId = clientUser.MerchantId;
            resultModel.MerchantCode = clientUser.ClientCode;
            resultModel.IsTestAccount = clientUser.IsTestAccount;
            resultModel.PosMachineId = posMachine.Id;

            var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.UserId == clientUser.Id && m.Status == Enumeration.OrderStatus.WaitPay).FirstOrDefault();
            if (orderToServiceFee != null)
            {
                resultModel.OrderInfo = BizFactory.Merchant.GetOrderConfirmInfoByServiceFee(orderToServiceFee);
            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "登录成功", resultModel);
        }

        private APIResponse SalesmanLogin(LoginModel model)
        {
            var salesman = CurrentDb.SysUser.Where(m => m.UserName == model.UserName).FirstOrDefault();
            if (salesman == null)
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户名不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(salesman.PasswordHash, model.Password))
            {
                return ResponseResult(ResultType.Failure, ResultCode.FailureSignIn, "登录失败，用户密码错误");
            }

            LoginResultModel resultModel = new LoginResultModel();


            //var agent = CurrentDb.SysAgentUser.Where(m => m.Id == salesman.AgentId).FirstOrDefault();


            DateTime nowDate = DateTime.Now;
            resultModel.UserId = salesman.Id;
            resultModel.MerchantId = 0;
            resultModel.PosMachineId = 0;
            resultModel.UserName = salesman.UserName;
            resultModel.MerchantCode = "88888888";
            resultModel.IsTestAccount = true;
            resultModel.Status = ClientLoginStatus.Normal;

            return ResponseResult(ResultType.Success, ResultCode.Success, "登录成功", resultModel);
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
        public APIResponse Home(int userId, int merchantId, int posMachineId, DateTime? datetime)
        {

            HomeModel model = new HomeModel();

            DateTime? lastUpdateTime;
            if (!SysFactory.SysItemCacheUpdateTime.CanGetData(userId, datetime, out lastUpdateTime))
            {
                return ResponseResult(ResultType.Failure, ResultCode.Failure, "没有最新的数据");
            }

            model.ServiceTelphone = "服务热线：020-36824118 （9：00~18：00）";
            model.LastUpdateTime = lastUpdateTime.Value;

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
                                  join r in CurrentDb.Company on u.InsuranceCompanyId equals r.Id
                                  where u.Status == Enumeration.CarInsuranceCompanyStatus.Normal
                                  select new { r.Id, r.Name, u.InsuranceCompanyImgUrl, u.CanClaims, u.CanInsure, u.CanApplyLossAssess, u.Priority }).Distinct().OrderByDescending(m => m.Priority);

            List<CarInsCompanyModel> carInsCompanyModels = new List<CarInsCompanyModel>();

            foreach (var carInsCompany in carInsCompanys)
            {
                CarInsCompanyModel carInsCompanyModel = new CarInsCompanyModel();
                carInsCompanyModel.Id = carInsCompany.Id;
                carInsCompanyModel.Name = carInsCompany.Name;
                carInsCompanyModel.ImgUrl = carInsCompany.InsuranceCompanyImgUrl;
                carInsCompanyModel.CanClaims = carInsCompany.CanClaims;
                carInsCompanyModel.CanInsure = carInsCompany.CanInsure;
                carInsCompanyModel.CanApplyLossAssess = carInsCompany.CanApplyLossAssess;
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

                var carInsurePlanKindParentKindIds = carKinds.Where(m => carKindIds.Contains(m.Id) && m.PId == 0).Select(m => m.Id).ToList();

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

            #region 人才输送工种

            List<TalentDemandWorkJobModel> talentDemandWorkJobModel = new List<TalentDemandWorkJobModel>();
            foreach (Enumeration.WorkJob t in Enum.GetValues(typeof(Enumeration.WorkJob)))
            {
                int id = Convert.ToInt32(t);
                if (id != 0)
                {
                    Enum en = (Enum)Enum.Parse(t.GetType(), id.ToString());
                    string name = en.GetCnName();

                    talentDemandWorkJobModel.Add(new TalentDemandWorkJobModel { Id = id, Name = name });
                }
            }
            model.TalentDemandWorkJob = talentDemandWorkJobModel;
            #endregion

            #region 检查流量费 是否到期后，需支付的订单

            if (!IsSaleman(userId))
            {
                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.UserId == userId && m.MerchantId == merchantId && m.PosMachineId == posMachineId && m.Status == Enumeration.OrderStatus.WaitPay).FirstOrDefault();
                if (orderToServiceFee != null)
                {
                    model.OrderInfo = BizFactory.Merchant.GetOrderConfirmInfoByServiceFee(orderToServiceFee);
                }
            }

            #endregion

            #region 第三方服务
            var extendedApps = CurrentDb.ExtendedApp.Where(m => m.IsDisplay == true).ToList();

            List<ExtendedAppModel> extendedAppModel = new List<ExtendedAppModel>();

            foreach (var m in extendedApps)
            {
                ExtendedAppModel appModel = new ExtendedAppModel();
                appModel.Id = m.Id;
                appModel.Name = m.Name;
                appModel.ImgUrl = m.ImgUrl;
                appModel.LinkUrl = m.LinkUrl;
                appModel.AppKey = m.AppKey;
                appModel.AppSecret = m.AppSecret;
                appModel.Type = m.Type;
                extendedAppModel.Add(appModel);
            }

            model.ExtendedApp = extendedAppModel;
            #endregion

            #region 查询违章积分
            var lllegalQueryScore = CurrentDb.LllegalQueryScore.Where(m => m.UserId == userId).FirstOrDefault();
            if (lllegalQueryScore != null)
            {
                model.LllegalQueryScore = lllegalQueryScore.Score;
            }
            #endregion

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
            return new APIResponse(result);
        }

        public APIResponse BaseInfo(int userId, int merchantId, int posMachineId)
        {
            BaseInfoResultModel model = new BaseInfoResultModel();

            #region 查询违章积分
            var lllegalQueryScore = CurrentDb.LllegalQueryScore.Where(m => m.UserId == userId).FirstOrDefault();
            if (lllegalQueryScore != null)
            {
                model.LllegalQueryScore = lllegalQueryScore.Score;
            }
            #endregion

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
            return new APIResponse(result);
        }

    }
}