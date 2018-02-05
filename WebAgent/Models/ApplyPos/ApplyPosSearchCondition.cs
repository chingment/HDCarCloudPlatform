using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.ApplyPos
{
    public class ApplyPosSearchCondition : SearchCondition
    {
        public string DeviceId { get; set; }

        public string UserName { get; set; }
    }
}