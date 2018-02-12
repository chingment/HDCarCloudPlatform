using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("Merchant")]
    public class Merchant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public Enumeration.MerchantType Type { get; set; }

        public Enumeration.RepairCapacity RepairCapacity { get; set; }

        [MaxLength(128)]
        public string ClientCode { get; set; }

        [MaxLength(128)]
        public string YYZZ_RegisterNo { get; set; }

        [MaxLength(128)]
        public string YYZZ_Type { get; set; }

        [MaxLength(128)]
        public string YYZZ_Name { get; set; }

        [MaxLength(512)]
        public string YYZZ_Address { get; set; }

        public DateTime? YYZZ_OperatingPeriodStart { get; set; }

        public DateTime? YYZZ_OperatingPeriodEnd { get; set; }

        [MaxLength(128)]
        public string FR_IdCardNo { get; set; }

        [MaxLength(128)]
        public string FR_Name { get; set; }

        public DateTime? FR_Birthdate { get; set; }

        [MaxLength(512)]
        public string FR_Address { get; set; }

        [MaxLength(128)]
        public string FR_IssuingAuthority { get; set; }

        public DateTime? FR_ValidPeriodStart { get; set; }

        public DateTime? FR_ValidPeriodEnd { get; set; }

        //经度
        public double Longitude { get; set; }

        //纬度
        public double Latitude { get; set; }

        [MaxLength(128)]
        public string ContactName { get; set; }

        [MaxLength(128)]
        public string ContactPhoneNumber { get; set; }

        [MaxLength(512)]
        public string ContactAddress { get; set; }

        [MaxLength(128)]
        public string AreaCode { get; set; }

        [MaxLength(256)]
        public string Area { get; set; }

        public Enumeration.MerchantStatus Status { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? SalesmanId { get; set; }

        public int? AgentId { get;set; }

        public int? FollowUserId { get; set; }
    }
}
