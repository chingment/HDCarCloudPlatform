using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("OrderToInsurance")]
    public class OrderToInsurance : Order
    {
        public int ProductId { get; set; }

        public Enumeration.ProductType ProductType { get; set; }

        [MaxLength(256)]
        public string ProductName { get; set; }

        //public int InsuranceCompanyId { get; set; }
        //[MaxLength(128)]
        //public string InsuranceCompanyName { get; set; }

        //public int ProductSkuId { get; set; }

        //public string ProductSkuName { get; set; }

        //public int PeopleNumber { get; set; }

    }
}
