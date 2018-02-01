using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class OrderField
    {
        public OrderField(string field, string value)
        {
            this.field = field;
            this.value = value;
        }

        public string field { get; set; }

        public string value { get; set; }
    }
}
