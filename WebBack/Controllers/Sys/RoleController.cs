using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebBack.Models.Sys.Role;

namespace WebBack.Controllers.Sys
{
    [OwnAuthorize(PermissionCode.角色管理)]
    public class RoleController : OwnBaseController
    {
        #region 视图
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult AddUserToRole()
        {
            return View();
        }

        #endregion

        #region 方法
        public JsonResult GetRoleTree()
        {
            object json = ConvertToZTreeJson2(CurrentDb.Roles.ToArray(), "id", "pid", "name", "role");
            return Json(ResultType.Success, json);
        }


        public JsonResult GetDetails(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return Json(ResultType.Success, model);
        }

        public JsonResult GetRoleMenuTreeList(int roleId)
        {

            var identity = new AspNetIdentiyAuthorizeRelay<SysUser>();
            var roleMenus = identity.GetRoleMenus(roleId);
            var isCheckedIds = from p in roleMenus select p.Id;

            object json = ConvertToZTreeJson(CurrentDb.SysMenu.OrderByDescending(m => m.Priority).ToArray(), "id", "pid", "name", "menu", isCheckedIds.ToArray());

            return Json(ResultType.Success, json);
        }

        public JsonResult GetRoleUserList(RoleUserSearchCondition condition)
        {


            string userName = "";
            if (condition.UserName != null)
            {
                userName = condition.UserName.Trim();
            }

            string fullName = "";
            if (condition.FullName != null)
            {
                fullName = condition.FullName.Trim();
            }


            var list = (from ur in CurrentDb.SysUserRole
                        join r in CurrentDb.Roles on ur.RoleId equals r.Id
                        join u in CurrentDb.SysStaffUser on ur.UserId equals u.Id
                        where ur.RoleId == condition.RoleId &&
                            (userName.Length == 0 || u.UserName.Contains(userName)) &&
                               (fullName.Length == 0 || u.FullName.Contains(fullName)) &&
                              u.IsDelete == false
                        select new { ur.UserId, ur.RoleId, u.FullName, u.UserName }).Distinct();

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.UserId).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }

        public JsonResult GetNotBindUsers(RoleUserSearchCondition condition)
        {

            string userName = "";
            if (condition.UserName != null)
            {
                userName = condition.UserName.Trim();
            }

            string fullName = "";
            if (condition.FullName != null)
            {
                fullName = condition.FullName.Trim();
            }


            var list = (from u in CurrentDb.SysStaffUser
                        where !(from d in CurrentDb.SysUserRole

                                where d.RoleId == condition.RoleId
                                select d.UserId).Contains(u.Id)

                        where
                                              (userName.Length == 0 || u.UserName.Contains(userName)) &&
                               (fullName.Length == 0 || u.FullName.Contains(fullName)) &&
                                                u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName }).Distinct();

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddUserToRole(int roleId, int[] userIds)
        {
            var identityWebBack = new AspNetIdentiyAuthorizeRelay<SysUser>();
            foreach (int userId in userIds)
            {
                identityWebBack.AddUserToRole(this.CurrentUserId, userId, roleId);
            }

            return Json(ResultType.Success, OwnOperateTipUtils.SELECT_SUCCESS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RemoveUserFromRole(int roleId, int[] userIds)
        {

            var identityWebBack = new AspNetIdentiyAuthorizeRelay<SysUser>();

            foreach (int userId in userIds)
            {
                identityWebBack.RemoveUserFromRole(this.CurrentUserId, roleId, userId);
            }

            return Json(ResultType.Success, OwnOperateTipUtils.REMOVE_SUCCESS);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRoleMenu(int roleId, int[] menuIds)
        {
            var identity = new AspNetIdentiyAuthorizeRelay<SysUser>();
            identity.SaveRoleMenu(this.CurrentUserId, roleId, menuIds);

            return Json(ResultType.Success, OwnOperateTipUtils.SAVE_SUCCESS);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Add(AddViewModel model)
        {
            var identityWebBack = new AspNetIdentiyAuthorizeRelay<SysUser>();
            if (!identityWebBack.RoleExists(model.SysRole.Name))
            {
                identityWebBack.CreateRole(this.CurrentUserId, model.SysRole);
                return Json(ResultType.Success, OwnOperateTipUtils.ADD_SUCCESS);
            }
            else
            {
                return Json(ResultType.Failure, OwnOperateTipUtils.ROLE_EXISTS);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EditViewModel model)
        {

            var identityWebBack = new AspNetIdentiyAuthorizeRelay<SysUser>();

            var isExxistRoleName = CurrentDb.Roles.Where(m => m.Name == model.SysRole.Name && m.Id != model.SysRole.Id).FirstOrDefault();
            if (isExxistRoleName != null)
                return Json(ResultType.Failure, OwnOperateTipUtils.ROLE_EXISTS);
            identityWebBack.UpdateRole(this.CurrentUserId, model.SysRole);

            return Json(ResultType.Success, OwnOperateTipUtils.UPDATE_SUCCESS);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int[] ids)
        {

            var identityWebBack = new AspNetIdentiyAuthorizeRelay<SysUser>();

            identityWebBack.DeleteRole(this.CurrentUserId, ids);

            return Json(ResultType.Success, OwnOperateTipUtils.DELETE_SUCCESS);
        }

        #endregion
    }
}