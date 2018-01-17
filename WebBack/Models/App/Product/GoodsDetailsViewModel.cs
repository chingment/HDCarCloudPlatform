using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.App.Product
{
    public class GoodsDetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Product _product = new Lumos.Entity.Product();
        private List<string> _elseImgs = new List<string>();
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

        public List<string> ElseImgs
        {
            get
            {
                return _elseImgs;
            }
            set
            {
                _elseImgs = value;
            }
        }

        public GoodsDetailsViewModel()
        {

        }

        public GoodsDetailsViewModel(int id)
        {
            var product = CurrentDb.Product.Where(m => m.Id == id).FirstOrDefault();
            if (product != null)
            {
                _product = product;

                if(_product.ElseImgUrls!=null)
                {
                    _elseImgs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(_product.ElseImgUrls);
                }
            }
        }
    }
}