using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ProductCategory
{
    public class EditViewModel:BaseViewModel
    {
        public Lumos.Entity.ProductCategory ProductCategory { set; get; }
    }
}