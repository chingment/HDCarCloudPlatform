using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ProductKind
{
    public class ProductSearchCondition: BaseSearchCondition
    {
        public string KindId { get; set; }
    }
}