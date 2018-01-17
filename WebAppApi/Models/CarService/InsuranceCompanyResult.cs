using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{
    public class InsuranceCompanyResult
    {
        public int CanInsureCount { get; set; }
        public List<CarInsCompanyModel> InsuranceCompany { get; set; }
    }
}