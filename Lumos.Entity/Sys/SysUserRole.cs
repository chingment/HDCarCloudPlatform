using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{

    [Table("SysUserRole")]
    public  class SysUserRole : IdentityUserRole<int>
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Key, Column(Order = 0)]
        public override int RoleId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Key, Column(Order = 1)]
        public override int UserId { get; set; }

    }
}
