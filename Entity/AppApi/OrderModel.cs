using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class OrderModel
    {
        public OrderModel()
        {
            this.OrderField = new List<OrderField>();
        }
        public int Id { get; set; }

        public string Sn { get; set; }

        public string Product { get; set; }

        public Enumeration.ProductType ProductType { get; set; }

        public Enumeration.OrderStatus Status { get; set; }

        public string StatusName { get; set; }

        public string Remarks { get; set; }

        public List<OrderField> OrderField { get; set; }

        public int FollowStatus { get; set; }

        public decimal Price { get; set; }

    }
}
