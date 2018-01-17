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
    /// 扩展ApplicationUser
    /// </summary>
    public class SysUserManager<TUser> : UserManager<TUser,int> where TUser : SysUser
    {
        public SysUserManager(IUserStore<TUser,int> store)
            : base(store)
        {

        }
    }
}
