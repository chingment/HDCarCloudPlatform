using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ExtendedApp
{
    public class ExtendedAppSearchCondition:BaseSearchCondition
    {
        public Enumeration.ExtendedAppAuditStatus AuditStatus { get; set; }
    }
}