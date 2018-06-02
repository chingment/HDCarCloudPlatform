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
        public int InsCompanyId { get; set; }
        [MaxLength(128)]
        public string InsCompanyName { get; set; }
        public int ProductId { get; set; }
        [MaxLength(128)]
        public string ProductName { get; set; }
        public int ProductSkuId { get; set; }
        [MaxLength(128)]
        public string ProductSkuName { get; set; }
        public string ProductSkuAttrItems { get; set; }
        public string CredentialsImgs { get; set; }
    }
}
