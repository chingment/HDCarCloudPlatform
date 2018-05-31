using Lumos.Entity.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class SubmitInsuranceModel
    {
        public SubmitInsuranceModel()
        {
            this.InsPlanDetailsItems = new List<ItemField>();
        }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int InsCompanyId { get; set; }

        public string InsCompanyName { get; set; }
        public int InsPlanId { get; set; }
        public string InsPlanName { get; set; }
        public string InsPlanDetails { get; set; }
        public bool IsTeam { get; set; }

        public List<ItemField> InsPlanDetailsItems { get; set; }
    }
}