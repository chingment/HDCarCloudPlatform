using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{
    public class InsureKindModel
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string Details { get; set; }

        public bool IsWaiverDeductible { get; set; }
    }
}