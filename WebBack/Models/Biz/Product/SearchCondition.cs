using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lumos.Entity;

namespace WebBack.Models.Biz.Product
{
    public class SearchCondition : BaseSearchCondition
    {

        public string ProductName { get; set; }

    }
}