using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service
{
    public class ShippingAddressService : BaseProvider
    {
        public CustomJsonResult Edit(int operater, ShippingAddress shippingAddress)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {


            }


            return result;
        }
    }
}
