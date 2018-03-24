using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("LllegalQueryLog")]
    public class LllegalQueryLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        [MaxLength(128)]
        public string CarNo { get; set; }
        [MaxLength(128)]
        public string CarType { get; set; }
        [MaxLength(128)]
        public string RackNo { get; set; }
        [MaxLength(128)]
        public string EnginNo { get; set; }
        [MaxLength(128)]
        public string IsCompany { get; set; }
        public string QueryResult{ get; set; }
        public string OfferResult { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
