using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ProductCategory
{
    public class DetailsViewModel:BaseViewModel
    {
        public Lumos.Entity.ProductCategory ProductCategory { set; get; }
        public DetailsViewModel()
        {

        }
        public DetailsViewModel(int id)
        {
            ProductCategory = CurrentDb.ProductCategory.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}