using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service
{
    public class ServiceFactory : BaseFactory
    {
        public static ProductService Product
        {
            get
            {
                return new ProductService();
            }
        }
    }
}
