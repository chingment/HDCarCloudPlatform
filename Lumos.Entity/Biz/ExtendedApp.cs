using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("ExtendedApp")]
    public class ExtendedApp
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Enumeration.ExtendedAppType Type { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string LinkUrl { get; set; }

        [MaxLength(1024)]
        public string ImgUrl { get; set; }

        public string AccessCount { get; set; }

        [MaxLength(128)]
        public string AppKey { get; set; }

        [MaxLength(128)]
        public string AppSecret { get; set; }

        public Enumeration.ExtendedAppStatus Status { get; set; }

        public bool? IsDisplay { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        [NotMapped]
        public Enumeration.ExtendedAppApplyType ApplyType { get; set; }
    }
}
