using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Resource
{

    public class DetailsViewModelByBanner : BaseViewModel
    {
        private Lumos.Entity.SysBanner _sysBanner = new Lumos.Entity.SysBanner();

        public Lumos.Entity.SysBanner SysBanner
        {
            get
            {
                return _sysBanner;
            }
            set
            {
                _sysBanner = value;
            }
        }

        public DetailsViewModelByBanner()
        {

        }

        public DetailsViewModelByBanner(int id)
        {
            var sysBanner = CurrentDb.SysBanner.Where(m => m.Id == id).FirstOrDefault();
            if (sysBanner != null)
            {
                sysBanner.ReadCount += 1;
                CurrentDb.SaveChanges();
                _sysBanner = sysBanner;
            }
        }
    }
}