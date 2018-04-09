using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ProductKind
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.ProductKind _productKind = new Lumos.Entity.ProductKind();

        public Lumos.Entity.ProductKind ProductKind
        {
            get
            {
                return _productKind;
            }
            set
            {
                _productKind = value;
            }
        }

        public DetailsViewModel()
        {

        }
        public DetailsViewModel(int id)
        {
            var productKind = CurrentDb.ProductKind.Where(m => m.Id == id).FirstOrDefault();
            if (productKind != null)
            {
                _productKind = productKind;
            }


        }



    }
}