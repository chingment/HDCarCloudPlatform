﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class CarInsInsureResult
    {
        public CarInsInsureResult()
        {
            this.InfoItems = new List<ItemParentField>();
            this.ReceiptAddress = new CarInsAddressModel();
        }

        public List<ItemParentField> InfoItems { get; set; }

        public CarInsAddressModel ReceiptAddress { get; set; }

    }
}
