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

    public class OrderCarClaimDetailsModel
    {
        public OrderCarClaimDetailsModel()
        {
            this.HandMerchant = new MerchantModel();
        }

        public int Id { get; set; }

        public string Sn { get; set; }

        public int InsuranceCompanyId { get; set; }

        public string InsuranceCompanyName { get; set; }

        public string RepairsType { get; set; }

        public string CarOwner { get; set; }

        public string CarPlateNo { get; set; }

        public string HandPerson { get; set; }

        public string HandPersonPhone { get; set; }

        public Enumeration.OrderStatus Status { get; set; }

        public int FollowStatus { get; set; }

        public string StatusName { get; set; }

        public DateTime SubmitTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public DateTime? CancleTime { get; set; }

        public DateTime? PayTime { get; set; }

        public string EstimateListImgUrl { get; set; }

        public MerchantModel HandMerchant { get; set; }


        public decimal AccessoriesPrice { get; set; }

        public decimal WorkingHoursPrice { get; set; }

        public decimal EstimatePrice { get; set; }

        public decimal Price { get; set; }

        public string Remark { get; set; }
    }
}