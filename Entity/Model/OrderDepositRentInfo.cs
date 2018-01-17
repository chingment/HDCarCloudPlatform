using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class OrderDepositRentInfo
    {
        public OrderDepositRentInfo()
        {

        }
        public int Id { get; set; }

        public string Sn { get; set; }

        public string Product { get; set; }

        public Enumeration.ProductType ProductType { get; set; }

        public Enumeration.OrderStatus Status { get; set; }

        public string StatusName { get; set; }

        public decimal Deposit { get; set; }

        public decimal MonthlyRent { get; set; }

        public string Remarks { get; set; }

        public string MerchantCode { get; set; }

        public string RentDueDate { get; set; }

    }
}
