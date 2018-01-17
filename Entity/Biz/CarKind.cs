using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("CarKind")]
    public class CarKind
    {
        [Key]
        public int Id { get; set; }
        public int PId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string AliasName { get; set; }

        public bool CanWaiverDeductible { get; set; }

        public bool IsWaiverDeductible { get; set; }

        public Enumeration.CarKindType Type { get; set; }

        public Enumeration.CarKindInputType InputType { get; set; }

        [MaxLength(128)]
        public string InputUnit { get; set; }

        [MaxLength(2048)]
        public string InputValue { get; set; }

        [MaxLength(2048)]
        public string InputOption { get; set; }

        public bool IsHasDetails { get; set; }

        public int Priority { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

    }
}
