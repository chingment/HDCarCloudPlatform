using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("OrderToCarInsure")]
    public class OrderToCarInsure : Order
    {

        public int InsurePlanId { get; set; }

        public int InsuranceCompanyId { get; set; }

        [MaxLength(128)]
        public string InsuranceCompanyName { get; set; }

        [MaxLength(128)]
        public string InsuranceOrderId { get; set; }

        [MaxLength(1024)]
        public string InsureImgUrl { get; set; }

        [MaxLength(1024)]
        public string CZ_CL_XSZ_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string CZ_SFZ_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string CCSJM_WSZM_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string YCZ_CLDJZ_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ZJ1_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ZJ2_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ZJ3_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ZJ4_ImgUrl { get; set; }

        [MaxLength(1024)]
        public string ClientRequire { get; set; }

        [MaxLength(128)]
        public string CarOwner { get; set; }

        [MaxLength(128)]
        public string CarOwnerIdNumber { get; set; }

        [MaxLength(128)]
        public string CarPlateNo { get; set; }

        public Enumeration.CarVechicheType CarVechicheType { get; set; }

        [MaxLength(128)]
        public string CarModel { get; set; }

        public int CarSeat { get; set; }

        [MaxLength(128)]
        public string CarEngineNo { get; set; }

        [MaxLength(128)]
        public string CarRegisterDate { get; set; }

        [MaxLength(128)]
        public string CarIssueDate { get; set; }

        [MaxLength(128)]
        public string CarOwnerAddress { get; set; }

        public decimal? CarPurchasePrice { get; set; }

        public Enumeration.CarUserCharacter CarUserCharacter { get; set; }

        [MaxLength(128)]
        public string CarVin { get; set; }

        public decimal CommercialPrice { get; set; }

        public decimal TravelTaxPrice { get; set; }

        public decimal CompulsoryPrice { get; set; }

        public bool IsSameLastYear { get; set; }

        public bool IsLastYearNewCar { get; set; }

        public DateTime? StartOfferTime { get; set; }

        public DateTime? EndOfferTime { get; set; }

        public DateTime? PeriodStart { get; set; }

        public DateTime? PeriodEnd { get; set; }

        public int AutoCancelByHour { get; set; }

        public bool IsCarDamage { get; set; }

        public string CarModelName { get; set; }

    }
}
