using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class InsPlanProductSkuModel
    {
        public int ProductSkuId { get; set; }
        public string ProductSkuName { get; set; }
        public string Price { get; set; }
        public List<ItemField> AttrItems { get; set; }
    }
}
