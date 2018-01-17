using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Product
{
    public class AddGoodsViewModel
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

        public AddGoodsViewModel()
        {

        }

    }
}