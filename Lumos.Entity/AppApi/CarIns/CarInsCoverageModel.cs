using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsCoverageModel
    {
        public string Name { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Premium { get; set; }

        public string Coverage { get; set; }
    }
}