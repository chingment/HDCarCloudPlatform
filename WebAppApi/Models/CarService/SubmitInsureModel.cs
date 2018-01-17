using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{



    public class SubmitInsureModel
    {
        public SubmitInsureModel()
        {

        }
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int InsurePlanId { get; set; }

        public int[] InsuranceCompanyId { get; set; }

        public List<InsureKindModel> InsureKind { get; set; }

        public Enumeration.ProductType Type { get; set; }

        public Dictionary<string, ImageModel> ImgData { get; set; }

    }
}