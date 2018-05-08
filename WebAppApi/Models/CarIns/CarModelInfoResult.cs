using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarIns
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