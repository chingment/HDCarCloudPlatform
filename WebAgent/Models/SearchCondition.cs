using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models
{
    public class SearchCondition
    {
        public SearchCondition()
        {
            this.PageSize = 10;
        }

        public string Sn { get; set; }

        public string ClientCode { get; set; }

        public string ClientName { get; set; }

        public string Name { get; set; }

        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}