using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Lumos.Entity
{
    [Table("OrderToCarClaim")]
    public class OrderToCarClaim : Order
    {

        public Enumeration.RepairsType RepairsType { get; set; }

        public int InsuranceCompanyId { get; set; }

        public Enumeration.MerchantType HandMerchantType { get; set; }

        public int? HandMerchantId { get; set; }

        public int? HandOrderId { get; set; }

        [MaxLength(1024)]
        public string EstimateListImgUrl { get; set; }

        [MaxLength(128)]
        public string InsuranceCompanyName { get; set; }

        [MaxLength(128)]
        public string HandPerson { get; set; }

        [MaxLength(128)]
        public string HandPersonPhone { get; set; }

        [MaxLength(128)]
        public string CarOwner { get; set; }

        [MaxLength(128)]
        public string CarOwnerIdNumber { get; set; }

        [MaxLength(128)]
        public string CarPlateNo { get; set; }

        public decimal AccessoriesPrice { get; set; }

        public decimal WorkingHoursPrice { get; set; }

        public decimal EstimatePrice { get; set; }

        [MaxLength(1024)]
        public string ClientRequire { get; set; }


    }
}
