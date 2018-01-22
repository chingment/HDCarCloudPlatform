using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppApi.Models.Banner;
using WebAppApi.Models.CarService;
using WebAppApi.Models.Product;

namespace WebAppApi.Models.Account
{
    public class HomeModel
    {
        public List<BannerImageModel> Banner { get; set; }

        public List<CarInsPlanModel> CarInsPlan { get; set; }

        public List<CarInsKindModel> CarInsKind { get; set; }

        public List<CarInsCompanyModel> CarInsCompany { get; set; }

        public List<TalentDemandWorkJobModel> TalentDemandWorkJob { get; set; }
    }
}