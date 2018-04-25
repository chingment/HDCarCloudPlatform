using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class OrderConfirmModel
    {
        public int UserId { get; set; }
        public List<OrderConfirmSkuModel> Skus { get; set; }

        public List<CouponModel> Coupon { get; set; }

    }
}
