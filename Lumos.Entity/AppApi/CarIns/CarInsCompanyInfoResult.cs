using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{

    public class Area
    {
        public int AreaId { get; set; }

        public string AreaName { get; set; }
    }

    public class CarInsCompanyInfoResult
    {
        public CarInsCompanyInfoResult()
        {
            this.Comanys = new List<CarInsComanyModel>();
            this.Areas = new List<Area>();
        }

        public int AreaId { get; set; }

        public string LicensePicKey { get; set; }

        public List<CarInsComanyModel> Comanys { get; set; }

        public List<Area> Areas { get; set; }
    }
}