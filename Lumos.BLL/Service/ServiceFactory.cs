using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service
{
    public class ServiceFactory : BaseFactory
    {
        public static IndexService Index
        {
            get
            {
                return new IndexService();
            }
        }

        public static ProductService Product
        {
            get
            {
                return new ProductService();
            }
        }

        public static CartService Cart
        {
            get
            {
                return new CartService();
            }
        }

        public static PersonalService Personal
        {
            get
            {
                return new PersonalService();
            }
        }

        public static ShippingAddressService ShippingAddress
        {
            get
            {
                return new ShippingAddressService();
            }
        }

        public static OrderService Order
        {
            get
            {
                return new OrderService();
            }
        }
    }
}
