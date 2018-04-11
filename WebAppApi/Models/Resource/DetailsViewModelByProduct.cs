using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Resource
{
    public class DetailsViewModelByProduct : BaseViewModel
    {
        private Lumos.Entity.Product _product = new Lumos.Entity.Product();

        public Lumos.Entity.Product Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        public DetailsViewModelByProduct()
        {

        }

        public DetailsViewModelByProduct(int id)
        {
            var product = CurrentDb.Product.Where(m => m.Id == id).FirstOrDefault();
            if (product != null)
            {
                _product = product;
            }
        }
    }
}