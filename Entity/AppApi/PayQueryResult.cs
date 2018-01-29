using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class PayQueryResult
    {
        public string OrderSn { get; set; }

        public int Status { get; set; }

        public string Remarks { get; set; }
    }
}
