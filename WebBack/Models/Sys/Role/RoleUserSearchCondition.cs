﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Role
{
    public class RoleUserSearchCondition: BaseSearchCondition
    {
        public int RoleId { get; set; }

        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}