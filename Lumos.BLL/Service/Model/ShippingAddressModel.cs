using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class ShippingAddressModel
    {
        public int Id { get; set; }
        public string Receiver { get; set; }
        public string PhoneNumber { get; set; }
        public string Area { get; set; }
        public string AreaCode { get; set; }
        public string Address { get; set; }
    }
}
