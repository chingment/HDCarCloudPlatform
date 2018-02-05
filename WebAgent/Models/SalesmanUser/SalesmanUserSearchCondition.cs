using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.SalesmanUser
{
    public class SalesmanUserSearchCondition : SearchCondition
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}