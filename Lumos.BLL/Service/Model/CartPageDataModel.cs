using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{

    public class CartPageDataModel
    {
        public CartPageDataModel()
        {
            this.ShoppingData = new CartShoppingDataModel();
        }

        public CartShoppingDataModel ShoppingData { get; set; }
    }
}
