﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class CouponModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Discount { get; set; }

        public string DiscountUnit { get; set; }

        public string ValidDate { get; set; }

        public string Description { get; set; }

        public bool CanSelected { get; set; }

        public string DiscountTip { get; set; }
    }
}
