using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
namespace Lumos.DAL.AuthorizeRelay
{
    /// <summary>
    /// 扩展RoleManager
    /// </summary>
    public class SysRoleManager : RoleManager<SysRole,int>
    {
        public SysRoleManager(IRoleStore<SysRole, int> store)
            : base(store)
        {

        }
    }
}
