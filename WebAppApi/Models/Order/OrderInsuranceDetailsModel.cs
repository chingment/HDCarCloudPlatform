using Lumos.Entity.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderInsuranceDetailsModel : OrderBaseDetailsViewModel
    {
        public OrderInsuranceDetailsModel()
        {
            this.ProductSkuAttrItems = new List<ItemField>();
            this.CredentialsImgs = new List<ZjModel>();
        }
        public string InsCompanyName { get; set; }

        public string ProductSkuName { get; set; }
        public List<ItemField> ProductSkuAttrItems { get; set; }

        public List<ZjModel> CredentialsImgs { get; set; }
    }
}