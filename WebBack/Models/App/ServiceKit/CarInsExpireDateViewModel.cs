using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.App.ServiceKit
{
    public class CarInsExpireDateViewModel
    {
        public string ClientCode { get; set; }

        public string CarPlateNo { get; set; }

        public string CarVinLast6Num { get; set; }
    }
}