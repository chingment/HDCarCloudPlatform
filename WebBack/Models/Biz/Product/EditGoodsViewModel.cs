using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Product
{
    public class EditGoodsViewModel : BaseViewModel
    {
        private Lumos.Entity.Product _product = new Lumos.Entity.Product();

        private List<string> _elseImgUrls = new List<string>();

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

        public List<string> ElseImgUrls
        {
            get
            {
                return _elseImgUrls;
            }
            set
            {
                _elseImgUrls = value;
            }
        }

        public EditGoodsViewModel()
        {

        }

        public EditGoodsViewModel(int id)
        {
            _elseImgUrls.Add("");
            _elseImgUrls.Add("");
            _elseImgUrls.Add("");
            _elseImgUrls.Add("");

            var product = CurrentDb.Product.Where(m => m.Id == id).FirstOrDefault();
            if (product != null)
            {
                _product = product;

                if (!string.IsNullOrEmpty(_product.ElseImgUrls))
                {
                    var elseImgUrls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(_product.ElseImgUrls);
                    for (var i = 0; i < elseImgUrls.Count; i++)
                    {
                        _elseImgUrls[i] = elseImgUrls[i];
                    }
                }

            }
        }
    }
}