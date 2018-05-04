using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarIns
{
    //车型信息
    public class CarModelInfoModel
    {
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public string Displacement { get; set; }
        public string MarketYear { get; set; }
        public string RatedPassengerCapacity { get; set; }
        public string ReplacementValue { get; set; }
    }
}