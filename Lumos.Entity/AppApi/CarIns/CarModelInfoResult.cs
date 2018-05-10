using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarModelInfoResult
    {
        public CarModelInfoResult()
        {
            this.models = new List<CarModelInfoModel>();
        }

        public List<CarModelInfoModel> models { get; set; }
    }
}