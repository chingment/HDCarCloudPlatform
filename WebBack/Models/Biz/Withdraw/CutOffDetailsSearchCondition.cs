using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Withdraw
{
    public class CutOffDetailsSearchCondition:SearchCondition
    {
        public int WithdrawCutOffId { get; set; }
    }
}