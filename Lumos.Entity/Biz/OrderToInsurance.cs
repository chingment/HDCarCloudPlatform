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
        //public int InsuranceCompanyId { get; set; }
        //[MaxLength(128)]
        //public string InsuranceCompanyName { get; set; }

        //public int ProductSkuId { get; set; }

        //public string ProductSkuName { get; set; }

        //public int PeopleNumber { get; set; }

    }
}
