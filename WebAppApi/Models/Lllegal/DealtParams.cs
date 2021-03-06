﻿using Lumos.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Lllegal
{
    public class DealtParams
    {
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int PosMachineId { get; set; }
        public string CarNo { get; set; }
        public bool IsGetConfirmInfo { get; set; }
        public List<LllegalRecord> LllegalRecord { get; set; }
    }
}