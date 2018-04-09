using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ProductCategory
{
    public class ListViewModel:BaseViewModel
    {
       public Lumos.Entity.ProductCategory ProductCategory { set;get;}
       
        public ListViewModel()
        {
        }
        public ListViewModel( int id)
        {
            ProductCategory = CurrentDb.ProductCategory.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}