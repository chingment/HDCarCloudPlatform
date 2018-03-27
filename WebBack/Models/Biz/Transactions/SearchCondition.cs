using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Transactions
{
    public class SearchCondition:BaseSearchCondition
    {
        public int UserId { get; set; }
    }
}