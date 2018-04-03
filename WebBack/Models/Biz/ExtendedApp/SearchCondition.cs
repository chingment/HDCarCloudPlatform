using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ExtendedApp
{
    public class SearchCondition:BaseSearchCondition
    {
        public Enumeration.ExtendedAppStatus Status { get; set; }
    }
}