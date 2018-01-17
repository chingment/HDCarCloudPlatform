using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Remoting.Messaging;
using Lumos.Entity;
using System.Reflection;
using System.Security.Cryptography;
namespace Lumos.DAL.AuthorizeRelay
{
    public class LoginResult<TUser> where TUser : SysUser
    {

        private Enumeration.LoginResult _ResultType;
        private Enumeration.LoginResultTip _ResultTip;
        private TUser _User;

        public Enumeration.LoginResult ResultType
        {
            get
            {
                return _ResultType;
            }
        }


        public Enumeration.LoginResultTip ResultTip
        {
            get
            {
                return _ResultTip;
            }
        }


        public TUser User
        {
            get
            {
                return _User;
            }
        }

        public LoginResult()
        {

        }

        public LoginResult(Enumeration.LoginResult type, Enumeration.LoginResultTip tip)
        {
            this._ResultType = type;
            this._ResultTip = tip;
        }

        public LoginResult(Enumeration.LoginResult type, Enumeration.LoginResultTip tip, TUser user)
        {
            this._ResultType = type;
            this._ResultTip = tip;
            this._User = user;
        }

    }

    public class AspNetIdentiyAuthorizeRelay<TUser> where TUser : SysUser
    {

        #region 属性和变量
        private static SysRoleManager _roleManager;
        private static SysUserManager<TUser> _userManager;


        public SysRoleManager RoleManager
        {
            get
            {
                return _roleManager;
            }
        }

        public SysUserManager<TUser> UserManager
        {
            get
            {
                return _userManager;
            }
        }


        private AuthorizeRelayDbContext _db;

        public AuthorizeRelayDbContext Db
        {
            get
            {
                return _db;
            }
        }
        #endregion

        public AspNetIdentiyAuthorizeRelay()
        {

            _db = new AuthorizeRelayDbContext();
            _roleManager = new SysRoleManager(new RoleStore<SysRole, int, SysUserRole>(_db));
            _userManager = new SysUserManager<TUser>(new UserStore<TUser, SysRole, int, SysUserLoginProvider, SysUserRole, SysUserClaim>(_db));
        }

        public AspNetIdentiyAuthorizeRelay(AuthorizeRelayDbContext db)
        {
            _db = db;
            _roleManager = new SysRoleManager(new RoleStore<SysRole, int, SysUserRole>(_db));
            _userManager = new SysUserManager<TUser>(new UserStore<TUser, SysRole, int, SysUserLoginProvider, SysUserRole, SysUserClaim>(_db));
        }

        private void AddOperateHistory(int operater, Enumeration.OperateType operateType, int referenceId, string content)
        {
            SysOperateHistory operateHistory = new SysOperateHistory();
            operateHistory.UserId = operater;
            operateHistory.ReferenceId = referenceId;
            operateHistory.Ip = "";
            operateHistory.Type = operateType;
            operateHistory.Content = content;
            operateHistory.CreateTime = DateTime.Now;
            operateHistory.Creator = operater;
            _db.SysOperateHistory.Add(operateHistory);
            _db.SaveChanges();
        }

        #region 登录和注销
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        private object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();
            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
        }

        public LoginResult<TUser> Login(string userName, string password, DateTime loginTime, string loginIp)
        {
            LoginResult<TUser> result = new LoginResult<TUser>();

            userName = userName.Trim();
            var user = _userManager.FindByName<TUser, int>(userName);

            if (user == null)
            {
                result = new LoginResult<TUser>(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserNotExist);
            }

            else
            {
                var lastUserInfo = CloneObject(user) as TUser;
                user = _userManager.Find<TUser, int>(userName, password);
                if (user == null)
                {
                    result = new LoginResult<TUser>(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserPasswordIncorrect, lastUserInfo);
                }
                else
                {

                    if (user.Status == Enumeration.UserStatus.Disable)
                    {
                        result = new LoginResult<TUser>(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserDisabled, lastUserInfo);
                    }
                    else
                    {
                        if (user.IsDelete)
                        {
                            result = new LoginResult<TUser>(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserDeleted, lastUserInfo);
                        }
                        else
                        {
                            user.LastLoginTime = loginTime;
                            user.LastLoginIp = loginIp;
                            _db.SaveChanges();

                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);


                            result = new LoginResult<TUser>(Enumeration.LoginResult.Success, Enumeration.LoginResultTip.VerifyPass, lastUserInfo);

                        }
                    }
                }
            }


            return result;
        }

        public void SignOut()
        {
            AuthenticationManager.SignOut();
        }
        #endregion

        #region 用户相关
        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TUser GetUser(int id)
        {

            return _userManager.FindById<TUser, int>(id);
        }

        /// <summary>
        /// 获取用户的所有菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SysMenu> GetUserMenus(int userId)
        {
            List<SysMenu> listMenu = new List<SysMenu>();
            var model =
                from menu in _db.SysMenu
                where
                (from rolemenu in _db.SysRoleMenu
                 where
                 (from userrole in _db.SysUserRole where rolemenu.RoleId == userrole.RoleId && userrole.UserId == userId select userrole.RoleId)
                 .Contains(rolemenu.RoleId)
                 select rolemenu.MenuId).Contains(menu.Id)
                select menu;


            if (model != null)
            {
                listMenu = model.OrderByDescending(m => m.Priority).ToList();
            }
            return listMenu;
        }

        /// <summary>
        /// 获取用户的所有权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> GetUserPermissions(int userId)
        {
            List<string> list = new List<string>();


            var model = (from sysMenuPermission in _db.SysMenuPermission
                         where
                             (from sysRoleMenu in _db.SysRoleMenu
                              where
                              (from userrole in _db.SysUserRole where sysRoleMenu.RoleId == userrole.RoleId && userrole.UserId == userId select userrole.RoleId)
                              .Contains(sysRoleMenu.RoleId)
                              select sysRoleMenu.MenuId).Contains(sysMenuPermission.MenuId)
                         select sysMenuPermission.PermissionId).Distinct();




            if (model != null)
            {
                list = model.ToList();
            }
            return list;
        }


        public bool UserExists(string username)
        {
            var idResult = _db.Users.Where(m => m.UserName == username).FirstOrDefault();
            if (idResult != null)
                return true;

            return false;
        }



        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CreateUser(int operater, TUser user, params int[] roleIds)
        {
            user.CreateTime = DateTime.Now;
            user.RegisterTime = DateTime.Now;
            user.Status = Enumeration.UserStatus.Normal;

            var result = _userManager.Create<TUser, int>(user, user.PasswordHash);

            if (result.Succeeded)
            {
                List<SysUserRole> userRoleList = _db.SysUserRole.Where(m => m.UserId == user.Id).ToList();
                foreach (var userRole in userRoleList)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                if (roleIds != null)
                {
                    if (roleIds.Length > 0)
                    {
                        foreach (int roleId in roleIds)
                        {
                            if (roleId != 0)
                            {
                                _db.SysUserRole.Add(new SysUserRole { UserId = user.Id, RoleId = roleId });
                            }
                        }
                    }
                }
                _db.SaveChanges();

                AddOperateHistory(operater, Enumeration.OperateType.New, user.Id, string.Format("新建用户{0}(ID:{1})", user.UserName, user.Id));
            }

            return result.Succeeded;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool UpdateUser(int operater, TUser user, string password, int[] roleIds)
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(password);
            }

            var result = _userManager.Update<TUser, int>(user);

            if (result.Succeeded)
            {
                List<SysUserRole> userRoleList = _db.SysUserRole.Where(m => m.UserId == user.Id).ToList();

                foreach (var userRole in userRoleList)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                if (roleIds != null)
                {
                    if (roleIds.Length > 0)
                    {
                        foreach (int roleId in roleIds)
                        {
                            if (roleId != 0)
                            {
                                _db.SysUserRole.Add(new SysUserRole { UserId = user.Id, RoleId = roleId });
                            }
                        }
                    }
                }
                _db.SaveChanges();

                AddOperateHistory(operater, Enumeration.OperateType.Update, user.Id, string.Format("修改用户{0}(ID:{1})", user.UserName, user.Id));

            }

            return result.Succeeded;
        }

        public bool UpdateUser(int operater, TUser user, string password)
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(password);
            }

            var result = _userManager.Update<TUser, int>(user);



            _db.SaveChanges();

            AddOperateHistory(operater, Enumeration.OperateType.Update, user.Id, string.Format("修改用户{0}(ID:{1})", user.UserName, user.Id));



            return result.Succeeded;
        }



        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool DeleteUser(int operater, int[] userIds)
        {
            if (userIds == null)
                return false;

            if (userIds.Length <= 0)
                return false;


            foreach (int userId in userIds)
            {
                SysUser user = _db.Users.Find(userId);
                user.IsDelete = true;
                user.Mender = operater;
                user.LastUpdateTime = DateTime.Now;

                var userRoles = _db.SysUserRole.Where(m => m.UserId == userId).ToList();
                foreach (var userRole in userRoles)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                _db.SaveChanges();

                AddOperateHistory(operater, Enumeration.OperateType.Delete, user.Id, string.Format("删除用户{0}(ID:{1})", user.UserName, user.Id));
            }

            return true;
        }

        /// <summary>
        /// 修改密码或重置密码 当oldpassword为null的时候为重置密码
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="oldpassword">旧密码</param>
        /// <param name="newpassword">新密码</param>
        /// <returns></returns>
        public bool ChangePassword(int operater, int userId, string oldpassword, string newpassword)
        {
            TUser user = GetUser(userId);

            var isFlag = _userManager.CheckPassword(user, oldpassword);
            if (!isFlag)
            {
                return false;
            }


            var result = _userManager.ChangePassword(userId, oldpassword, newpassword);

            AddOperateHistory(operater, Enumeration.OperateType.Delete, userId, string.Format("修改用户{0}(ID:{1})密码", user.UserName, user.Id));

            return result.Succeeded;

        }

        public bool ResetPassword(int operater, int userId, string password)
        {
            SysUser user = GetUser(userId);
            user.PasswordHash = null;
            user.Mender = operater;
            user.LastUpdateTime = DateTime.Now;
            _db.SaveChanges();

            IdentityResult result = _userManager.AddPassword(userId, password);
            return result.Succeeded;

        }




        #endregion 用户相关

        #region 角色相关

        /// <summary>
        /// 判断角色名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RoleExists(string name)
        {
            return _roleManager.RoleExists(name);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool CreateRole(int operater, SysRole role)
        {
            role.PId = 0;
            role.CreateTime = DateTime.Now;
            role.Creator = operater;
            var idResult = _roleManager.Create(role);
            AddOperateHistory(operater, Enumeration.OperateType.New, role.Id, string.Format("新建角色{0}(ID:{1})", role.Name, role.Id));
            return idResult.Succeeded;
        }

        /// <summary>
        /// 删除角色 删除角色菜单 删除角色用户 删除角色权限
        /// </summary>
        /// <param name="roleId"></param>
        public void DeleteRole(int operater, int[] ids)
        {

            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var roleUsers = _db.SysUserRole.Where(u => u.RoleId == id).Distinct();

                    var roleMenus = _db.SysRoleMenu.Where(u => u.RoleId == id).Distinct();


                    var role = _db.Roles.Find(id);

                    foreach (var user in roleUsers)
                    {
                        _db.SysUserRole.Remove(user);
                    }

                    foreach (var menu in roleMenus)
                    {
                        _db.SysRoleMenu.Remove(menu);
                    }


                    _db.Roles.Remove(role);
                    _db.SaveChanges();

                    AddOperateHistory(operater, Enumeration.OperateType.Delete, role.Id, string.Format("删除角色{0}(ID:{1})", role.Name, role.Id));
                }

            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleId"></param>
        public bool UpdateRole(int operater, SysRole sysRole)
        {
            bool isSucceed = false;
            var _sysRole = _roleManager.FindById(sysRole.Id);
            if (_sysRole != null)
            {
                _sysRole.Name = sysRole.Name;
                _sysRole.Description = sysRole.Description;
                _sysRole.LastUpdateTime = DateTime.Now;
                _sysRole.Mender = operater;
                isSucceed = _roleManager.Update(_sysRole).Succeeded;

                AddOperateHistory(operater, Enumeration.OperateType.Update, _sysRole.Id, string.Format("修改角色{0}(ID:{1})", _sysRole.Name, _sysRole.Id));

            }
            return isSucceed;
        }

        /// <summary>
        /// 添加用户到角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool AddUserToRole(int operater, int userId, int roleId)
        {
            _db.SysUserRole.Add(new SysUserRole { UserId = userId, RoleId = roleId });
            _db.SaveChanges();


            AddOperateHistory(operater, Enumeration.OperateType.Update, roleId, string.Format("添加用户(ID：{0})到角色(ID:{1})", userId, roleId));

            return true;
        }


        /// <summary>
        /// 清楚用户所在的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        public void RemoveUserFromRole(int operater, int roleId, int userId)
        {
            SysUserRole userRole = _db.SysUserRole.Find(roleId, userId);
            _db.SysUserRole.Remove(userRole);
            _db.SaveChanges();

            AddOperateHistory(operater, Enumeration.OperateType.Update, roleId, string.Format("移除用户(ID：{0})所在的角色(ID:{1})", userId, roleId));

        }


        /// <summary>
        /// 获取角色的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<SysMenu> GetRoleMenus(int roleId)
        {
            var model = from c in _db.SysMenu
                        where
                            (from o in _db.SysRoleMenu where o.RoleId == roleId select o.MenuId).Contains(c.Id)
                        select c;

            return model.ToList();
        }

        /// <summary>
        /// 添加菜单到角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool SaveRoleMenu(int operater, int roleId, int[] menuIds)
        {

            List<SysRoleMenu> roleMenuList = _db.SysRoleMenu.Where(r => r.RoleId == roleId).ToList();

            foreach (var roleMenu in roleMenuList)
            {
                _db.SysRoleMenu.Remove(roleMenu);
            }


            if (menuIds != null)
            {
                foreach (var menuId in menuIds)
                {
                    _db.SysRoleMenu.Add(new SysRoleMenu { RoleId = roleId, MenuId = menuId });
                }
            }

            _db.SaveChanges();

            AddOperateHistory(operater, Enumeration.OperateType.Update, roleId, string.Format("保存角色(ID:{0})菜单", roleId));

            return true;
        }

        #endregion 角色相关

        #region 菜单相关
        public int CreateMenu(int operater, SysMenu sysMenu, string[] perssionId)
        {
            sysMenu.Creator = operater;
            sysMenu.CreateTime = DateTime.Now;
            _db.SysMenu.Add(sysMenu);
            _db.SaveChanges();

            if (perssionId != null)
            {
                foreach (var id in perssionId)
                {
                    _db.SysMenuPermission.Add(new SysMenuPermission { MenuId = sysMenu.Id, PermissionId = id });
                }
            }
            _db.SaveChanges();

            AddOperateHistory(operater, Enumeration.OperateType.New, sysMenu.Id, string.Format("新建菜单(ID:{0})", sysMenu.Id));

            return sysMenu.Id;
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="id"></param>
        public void UpdateMenu(int operater, SysMenu sysMenu, string[] perssions)
        {
            sysMenu.Mender = operater;
            sysMenu.LastUpdateTime = DateTime.Now;
            _db.Entry(sysMenu).State = EntityState.Modified;



            var sysMenuPermission = _db.SysMenuPermission.Where(r => r.MenuId == sysMenu.Id).ToList();
            foreach (var m in sysMenuPermission)
            {
                _db.SysMenuPermission.Remove(m);
            }


            if (perssions != null)
            {
                foreach (var m in perssions)
                {
                    _db.SysMenuPermission.Add(new SysMenuPermission { MenuId = sysMenu.Id, PermissionId = m });
                }
            }

            AddOperateHistory(operater, Enumeration.OperateType.Update, sysMenu.Id, string.Format("修改菜单(ID:{0})", sysMenu.Id));

            _db.SaveChanges();

        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMenu(int operater, int[] ids)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var sysMenu = _db.SysMenu.Where(m => m.Id == id).FirstOrDefault();

                    _db.SysMenu.Remove(sysMenu);

                    var sysRoleMenus = _db.SysRoleMenu.Where(r => r.MenuId == id).ToList();
                    foreach (var sysRoleMenu in sysRoleMenus)
                    {
                        _db.SysRoleMenu.Remove(sysRoleMenu);
                    }

                    _db.SaveChanges();

                    AddOperateHistory(operater, Enumeration.OperateType.Delete, id, string.Format("删除菜单(ID:{0})", id));

                }
            }

        }

        #endregion 菜单相关

        #region 权限相关

        public List<SysPermission> GetPermissionList(PermissionCode permission)
        {
            Type t = permission.GetType();
            List<SysPermission> list = new List<SysPermission>();
            //SysPermission p = new SysPermission();
            //p.Id = "0";
            //p.Name = "权限集合";
            //p.PId = "";
            //list.Add(p);
            list = GetBasePermissionList(t, list);
            return list;
        }

        private List<SysPermission> GetBasePermissionList(Type t, List<SysPermission> list)
        {
            if (t.Name != "Object")
            {
                System.Reflection.FieldInfo[] properties = t.GetFields();
                foreach (System.Reflection.FieldInfo property in properties)
                {
                    string pId = "0";
                    object[] typeAttributes = property.GetCustomAttributes(false);
                    foreach (PermissionCodeAttribute attribute in typeAttributes)
                    {
                        pId = attribute.PId;
                    }
                    object id = property.GetValue(null);
                    string name = property.Name;
                    SysPermission model = new SysPermission();
                    model.Id = id.ToString();
                    model.Name = name;
                    list.Add(model);
                }
                list = GetBasePermissionList(t.BaseType, list);
            }
            return list;
        }

        #endregion
    }
}
