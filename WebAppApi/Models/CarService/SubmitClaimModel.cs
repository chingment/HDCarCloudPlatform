using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{

    public class SubmitClaimModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int InsuranceCompanyId { get; set; }

        public Enumeration.RepairsType RepairsType { get; set; }

        public string HandPerson { get; set; }

        public string HandPersonPhone { get; set; }

        public string CarLicenseNumber { get; set; }

        public string ClientRequire { get; set; }
    }
}