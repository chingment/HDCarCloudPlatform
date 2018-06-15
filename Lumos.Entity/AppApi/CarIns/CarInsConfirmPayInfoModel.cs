using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class CarInsConfirmPayInfoModel
    {
        public CarInsConfirmPayInfoModel()
        {
            this.InfoItems = new List<ItemParentField>();
            this.receiptAddress = new CarInsAddressModel();
        }

        public List<ItemParentField> InfoItems { get; set; }

        public CarInsAddressModel receiptAddress { get; set; }
    }
}
