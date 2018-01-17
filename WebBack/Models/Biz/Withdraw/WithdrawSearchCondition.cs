using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Withdraw
{
    public class WithdrawSearchCondition:SearchCondition
    {
        public Enumeration.WithdrawStatus Status { get; set; }

        public int UserId { get; set; }
    }
}