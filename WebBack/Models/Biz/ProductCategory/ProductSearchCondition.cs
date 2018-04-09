using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ProductCategory
{
    public class ProductSearchCondition: BaseSearchCondition
    {
        public int CategoryId { get; set; }
    }
}