﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderInsuranceDetailsModel : OrderBaseDetailsViewModel
    {
        public string InsCompanyName { get; set; }

        public string InsPlanName { get; set; }
    }
}