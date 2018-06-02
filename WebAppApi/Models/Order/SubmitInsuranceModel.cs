using Lumos.Entity.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class SubmitInsuranceModel
    {
        public SubmitInsuranceModel()
        {
     
        }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int ProductSkuId { get; set; }

        public Dictionary<string, ImageModel> ImgData { get; set; }

    }
}