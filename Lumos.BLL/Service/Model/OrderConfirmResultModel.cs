using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{

    public class OrderBlock
    {
        public OrderBlock()
        {
            this.Skus = new List<OrderConfirmSkuModel>();
        }

        public string TagName { get; set; }
        public ShippingAddressModel ShippingAddress { get; set; }

        public List<OrderConfirmSkuModel> Skus { get; set; }
    }

    public class OrderConfirmResultModel
    {
        public OrderConfirmResultModel()
        {
            this.Coupon = new List<CouponModel>();
            this.SubtotalItem = new List<SubtotalItem>();
        }

        //选择的优惠卷
        public List<CouponModel> Coupon { get; set; }
        //订单块
        public List<OrderBlock> OrderBlock { get; set; }
        //小计项目
        public List<SubtotalItem> SubtotalItem { get; set; }
        //实际支付金额
        public string ActualAmount { get; set; }
        //原金额
        public string OriginalAmount { get; set; }
    }
}
