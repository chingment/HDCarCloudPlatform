﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeLianSdk
{
    public class CarQueryDataList_Request
    {
        public string bookNo { get; set; }
        public string bookType { get; set; }
        public string lllegalDesc { get; set; }
        public string address { get; set; }
        public string lllegalTime { get; set; }
        public int point { get; set; }
        public string carNo { get; set; }
        public decimal fine { get; set; }
        public decimal lateFee { get; set; }
        public string lllegalCode { get; set; }
        public string lllegalCity { get; set; }
        public string cityCode { get; set; }
        public string offerType { get; set; }
        public decimal serviceFee { get; set; }
        public decimal late_fees { get; set; }
        public string content { get; set; }
    }
}
