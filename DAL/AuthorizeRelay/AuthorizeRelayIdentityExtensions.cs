using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Microsoft.AspNet.Identity
{


    public static class AuthorizeRelayIdentityExtensions
    {
        private static AspNetIdentiyAuthorizeRelay<SysUser> Manager
        {
            get
            {
                return new AspNetIdentiyAuthorizeRelay<SysUser>();
            }
        }

        public static string GetHiddenUserName(this IIdentity identity)
        {
            if (identity == null)
                return null;
            if (!identity.IsAuthenticated)
                return null;
            var username = identity.GetUserName();
            char[] c = username.ToCharArray();
            if (username.Length > 2 & username.Length <= 5)
            {
                c[1] = '*';
            }
            else if (username.Length > 5 && username.Length <= 9)
            {
                c[3] = '*';
                c[4] = '*';
            }
            else if (username.Length > 9)
            {
                c[3] = '*';
                c[4] = '*';
                c[5] = '*';
            }
            username = new string(c);

            return username;
        }

        /// <summary>
        /// 获取用户所在的角色
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static List<SysUserRole> GetUserRoles(this IIdentity identity)
        {
            if (identity == null)
                return null;
            if (!identity.IsAuthenticated)
                return null;
            var userId = int.Parse(identity.GetUserId());
            var model = new List<SysUserRole>();
            var identityUserRole = Manager.UserManager.FindById(userId);
            model = identityUserRole.Roles.ToList();

            return model;
        }

        /// <summary>
        /// 获取用户的所有菜单
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static List<SysMenu> GetUserMenus(this IIdentity identity)
        {
            if (identity == null)
                return null;
            if (!identity.IsAuthenticated)
                return null;

            int userId = int.Parse(identity.GetUserId());


            List<SysMenu> list = new List<SysMenu>();


            list = Manager.GetUserMenus(userId);

            return list;
        }

        public static bool UserHasMenu(this IIdentity identity, List<SysMenu> menus, string url)
        {

            if (identity == null)
                return false;
            if (!identity.IsAuthenticated)
                return false;

            if (menus == null)
                return false;

            int count = menus.Where(m => m.Url == url).Count();

            if (count == 0)
                return false;


            return true;

        }

        /// <summary>
        /// 获取用户的所有权限
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static List<string> GetUserPermissions(this IIdentity identity)
        {
            if (identity == null)
                return null;
            if (!identity.IsAuthenticated)
                return null;

            var listsPermission = new List<string>();

            var userId = int.Parse(identity.GetUserId());

            List<string> list = new List<string>();
            list = Manager.GetUserPermissions(userId);

            return list;
        }

        /// <summary>
        /// 判断用户是否拥有该权限
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="permission">权限代码</param>
        /// <returns></returns>
        public static bool IsInPermission(this IIdentity identity, params string[] permissions)
        {
            if (identity == null)
                return false;
            if (!identity.IsAuthenticated)
                return false;

            List<string> listPermissions = GetUserPermissions(identity);
            if (listPermissions == null)
                return false;
            if (listPermissions.Count < 1)
                return false;

            bool isHas = false;
            foreach (var permission in listPermissions)
            {
                foreach (var m in permissions)
                {
                    if (permission.Trim() == m.Trim())
                    {
                        isHas = true;
                        break;
                    }
                }
                if (isHas)
                {
                    break;
                }
            }

            return isHas;
        }

        /// <summary>
        /// 判断用户是否该页面的访问权限
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="permission">权限代码</param>
        /// <returns></returns>
        public static bool IsInMenu(this IIdentity identity, string menuUrl)
        {
            if (identity == null)
                return false;
            if (!identity.IsAuthenticated)
                return false;


            List<SysMenu> list = GetUserMenus(identity);
            if (list == null)
                return false;
            if (list.Count < 1)
                return false;

            bool isHas = false;
            foreach (var p in list)
            {
                if (p.Url.ToUpper().IndexOf(menuUrl.ToUpper()) > -1)
                {
                    isHas = true;
                    break;
                }
            }

            return isHas;
        }
    }
}