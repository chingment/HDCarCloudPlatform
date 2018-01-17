using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Report
{
    public class ReportModel
    {
        public string UserName { get; set; }

        public Enumeration.OperateType Operate { get; set; }

        public string ClientCode { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }


        public DateTime? StartPayTime { get; set; }

        public DateTime? EndPayTime { get; set; }

        public string TableHtml { get; set; }
    }
}