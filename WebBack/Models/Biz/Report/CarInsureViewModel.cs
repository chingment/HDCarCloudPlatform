using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Report
{
    public class CarInsureViewModel:ReportModel
    {
        public Enumeration.OrderStatus Status { get; set; }
    }
}