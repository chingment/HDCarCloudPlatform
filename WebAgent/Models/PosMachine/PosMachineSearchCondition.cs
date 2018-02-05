using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.PosMachine
{
    public class PosMachineSearchCondition : SearchCondition
    {
        public string UserName { get; set; }

        public string DeviceId { get; set; }

        public string FuselageNumber { get; set; }

        public string TerminalNumber { get; set; }

        public string NoInDeviceIds { get; set; }
}
}