using Lumos.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppApi.Models.Banner;
using WebAppApi.Models.CarService;

namespace WebAppApi.Models.Account
{
    public class HomeModel
    {
        public string ServiceTelphone { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public List<BannerImageModel> Banner { get; set; }

        public List<CarInsPlanModel> CarInsPlan { get; set; }

        public List<CarInsKindModel> CarInsKind { get; set; }

        public List<CarInsCompanyModel> CarInsCompany { get; set; }

        public List<TalentDemandWorkJobModel> TalentDemandWorkJob { get; set; }

        public OrderConfirmInfo OrderInfo { get; set; }

        public List<ExtendedAppModel> ExtendedApp { get; set; }

        public int LllegalQueryScore { get; set; }
    }
}