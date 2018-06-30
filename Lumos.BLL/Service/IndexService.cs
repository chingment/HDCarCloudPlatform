using Lumos.BLL.Service.Model;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service
{
    public class IndexService : BaseProvider
    {
        public IndexModel GetPageData(int userId)
        {
            var model = new IndexModel();


            //var banner = CurrentDb.SysBanner.Where(m => m.Type == Enumeration.BannerType.MainHomeTop && m.Status == Enumeration.SysBannerStatus.Release).ToList();

            //List<BannerModel> bannerModels = new List<BannerModel>();

            //foreach (var m in banner)
            //{
            //    var bannerModel = new BannerModel();
            //    bannerModel.Id = m.Id;
            //    bannerModel.Title = m.Title;
            //    bannerModel.LinkUrl = SysFactory.Banner.GetLinkUrl(m.Id);
            //    bannerModel.ImgUrl = m.ImgUrl;
            //    bannerModels.Add(bannerModel);
            //}

            //model.Banner = bannerModels;


            return model;
        }
    }
}
