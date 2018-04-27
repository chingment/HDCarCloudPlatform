using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{

    public class OrderConfirmCouponModel
    {
        public string Tip { get; set; }

        public int CanUseQuantity { get; set; }

        public List<int> SelecedCouponId { get; set; }
    }
}
