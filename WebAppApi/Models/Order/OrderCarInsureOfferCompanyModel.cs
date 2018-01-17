using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderCarInsureOfferCompanyModel
    {
        public int InsuranceOfferId { get; set; }

        public int InsuranceCompanyId { get; set; }

        public string InsuranceCompanyName { get; set; }

        public string InsureImgUrl { get; set; }

        public decimal CommercialAndTravelTaxPrice { get; set; }

        public decimal CompulsoryPrice { get; set; }

        public decimal InsureTotalPrice { get; set; }


    }
}