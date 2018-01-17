using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppApi.Models;

namespace WebAppApi.Controllers
{
    public enum ExtendedAppType
    {
        Unknow = 0,
        All = 1,
        MainHomeCarService = 2,
        MainHomeRecommend = 3
    }

    public class ExtendedAppController : BaseApiController
    {
        [HttpGet]
        public APIResponse GetList(int userId,ExtendedAppType type)
        {
            List<ExtendedApp> extendedApps = new List<ExtendedApp>();

            if (type == ExtendedAppType.All)
            {
                extendedApps = CurrentDb.ExtendedApp.Where(m => m.Type != Enumeration.ExtendedAppType.CarService && m.IsDisplay==true).ToList();
            }
            else if (type == ExtendedAppType.MainHomeCarService)
            {
                extendedApps = CurrentDb.ExtendedApp.Where(m => m.Type == Enumeration.ExtendedAppType.CarService && m.IsDisplay == true).Take(4).ToList();
            }
            else if (type == ExtendedAppType.MainHomeRecommend)
            {
                extendedApps = CurrentDb.ExtendedApp.Where(m => m.Type != Enumeration.ExtendedAppType.CarService && m.IsDisplay == true).Take(4).ToList();
            }


            //获取商户信息
            var merchant = CurrentDb.Merchant.Where(m => m.UserId == userId).FirstOrDefault();

            string clientCode = "88888";
            if (merchant != null)
            {
                clientCode = merchant.ClientCode;
            }

            List<ExtendedAppModel> model = new List<ExtendedAppModel>();

            foreach (var m in extendedApps)
            {
                ExtendedAppModel imageModel = new ExtendedAppModel();
                imageModel.Id = m.Id;
                imageModel.Name = m.Name;
                imageModel.ImgUrl = m.ImgUrl;
                imageModel.LinkUrl = GetLinkUrl(clientCode, m.LinkUrl); ;
                imageModel.AppKey = m.AppKey;
                imageModel.AppSecret = m.AppSecret;
                model.Add(imageModel);
            }

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }
    }
}