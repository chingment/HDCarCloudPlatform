using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Report
{
    public class WithdrawViewModel : ReportModel
    {
        public Enumeration.WithdrawStatus Status { get; set; }
    }
}