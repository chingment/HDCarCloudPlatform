﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("OrderToShopping")]
    public class OrderToShopping : Order
    {
        [MaxLength(128)]
        public string ExpressCompany { get; set; }
        [MaxLength(128)]
        public string ExpressOrderNo { get; set; }
    }
}