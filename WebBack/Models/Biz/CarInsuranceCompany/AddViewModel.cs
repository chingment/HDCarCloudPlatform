using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.CarInsuranceCompany
{
    public class AddViewModel
    {
        public int InsuranceCompanyId { get; set; }

        public string InsuranceCompanyName { get; set; }

        public string InsuranceCompanyImgUrl { get; set; }

        public decimal CommercialRate { get; set; }

        public decimal CompulsoryRate { get; set; }

    }
}