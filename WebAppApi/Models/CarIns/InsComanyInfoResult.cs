using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarIns
{
    public class Channel
    {
        public int ChannelId { get; set; }
        public string Code { get; set; }
        public string Descp { get; set; }
        public string Inquiry { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public int OpType { get; set; }
        public int Remote { get; set; }
        public int Sort { get; set; }

    }

    public class Area
    {
        public int AreaId { get; set; }

        public string AreaName { get; set; }
    }

    public class InsComanyInfoResult
    {
        public int AreaId { get; set; }

        public string LicensePicKey { get; set; }

        public List<Channel> Channels { get; set; }

        public List<Area> Areas { get; set; }
    }
}