﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.SalesmanUser
{
    public class SearchCondition : BaseSearchCondition
    {
        public string UserName { get; set; }
        public string FullName { get; set; }

        public int AgentId { get; set; }
    }
}