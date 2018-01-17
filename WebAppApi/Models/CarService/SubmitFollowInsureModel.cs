using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{
    public class SubmitFollowInsureModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int OrderId { get; set; }

        public Dictionary<string, ImageModel> ImgData { get; set; }
    }
}