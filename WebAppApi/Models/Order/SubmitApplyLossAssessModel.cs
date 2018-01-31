using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class SubmitApplyLossAssessModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int InsuranceCompanyId { get; set; }
    }
}