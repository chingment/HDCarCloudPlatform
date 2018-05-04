using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarIns
{

    public enum KeyWordType
    {
        [Remark("未知")]
        Unknow = 0,
        [Remark("行驶证图片")]
        LicenseImg = 1,
        [Remark("行驶证车牌号码")]
        LicensePlateNo = 2
    }


    public class CarInfoPms
    {
        public KeyWordType KeywordType { get; set; }

        public string Keyword { get; set; }

    }
}