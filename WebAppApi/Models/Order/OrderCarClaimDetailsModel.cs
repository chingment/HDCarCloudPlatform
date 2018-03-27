using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class MerchantModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }

        public string ContactPhone { get; set; }

        public string ContactAddress { get; set; }

        public string HeadTitle { get; set; }

    }

    public class OrderCarClaimDetailsModel : OrderBaseDetailsViewModel
    {
        public OrderCarClaimDetailsModel()
        {
            this.HandMerchant = new MerchantModel();
        }


        public int InsuranceCompanyId { get; set; }

        public string InsuranceCompanyName { get; set; }

        public string RepairsType { get; set; }

        public string CarOwner { get; set; }

        public string CarPlateNo { get; set; }

        public string HandPerson { get; set; }

        public string HandPersonPhone { get; set; }

      

        public string EstimateListImgUrl { get; set; }

        public MerchantModel HandMerchant { get; set; }


        public string AccessoriesPrice { get; set; }

        public string WorkingHoursPrice { get; set; }

        public string EstimatePrice { get; set; }

    }
}