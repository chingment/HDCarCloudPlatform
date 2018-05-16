using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsInsureKindModel
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string Details { get; set; }

        public bool IsWaiverDeductible { get; set; }
    }
}