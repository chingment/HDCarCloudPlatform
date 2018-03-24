using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Lllegal
{
    public class QueryParams
    {
        public string CarNo { get; set; }
        public string CarType { get; set; }
        public string RackNo { get; set; }
        public string EnginNo { get; set; }
        public string IsCompany { get; set; }
        //public List<DataLllegal> DataLllegal { get; set; }
        public bool IsOffer { get; set; }
    }
}