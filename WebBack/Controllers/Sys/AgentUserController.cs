using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Sys.AgentUser;

namespace WebBack.Controllers.Sys
{
    public class AgentUserController : OwnBaseController
    {
        #region 视图

        public ViewResult List()
        {
            return View();
        }

        public ViewResult SelectList()
        {
            return View();
        }

        public ViewResult Add()
        {
            AddViewModel model = new AddViewModel();
            return View();
        }

        public ViewResult Edit(int id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }


        #endregion

        public JsonResult GetList(AgentUserSearchCondition condition)
        {
            var list = (from u in CurrentDb.SysAgentUser
                        where (condition.UserName == null || u.UserName.Contains(condition.UserName)) &&
                        (condition.FullName == null || u.FullName.Contains(condition.FullName)) &&
                        u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        public JsonResult GetSelectList(AgentUserSearchCondition condition)
        {
            var list = (from u in CurrentDb.SysAgentUser
                        where (condition.UserName == null || u.UserName.Contains(condition.UserName)) &&
                        (condition.FullName == null || u.FullName.Contains(condition.FullName)) &&
                        u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Add(AddViewModel model)
        {
            SysAgentUser user = new SysAgentUser();
            user.UserName = string.Format("AG{0}", model.SysAgentUser.UserName);
            user.FullName = model.SysAgentUser.FullName;
            user.PasswordHash = "888888";
            user.Email = model.SysAgentUser.Email;
            user.PhoneNumber = model.SysAgentUser.PhoneNumber;
            user.IsModifyDefaultPwd = false;
            user.IsDelete = false;
            user.Status = Enumeration.UserStatus.Normal;
            user.Creator = this.CurrentUserId;
            user.CreateTime = DateTime.Now;
            user.Type = Enumeration.UserType.Agent;
            var identiy = new AspNetIdentiyAuthorizeRelay<SysAgentUser>();


            if (identiy.UserExists(user.UserName.Trim()))
                return Json(ResultType.Failure, OwnOperateTipUtils.USER_EXISTS);


            bool r = identiy.CreateUser(this.CurrentUserId, user, null);
            if (!r)
                return Json(ResultType.Failure, OwnOperateTipUtils.ADD_FAILURE);



            return Json(ResultType.Success, OwnOperateTipUtils.ADD_SUCCESS);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EditViewModel model)
        {

            var identiy = new AspNetIdentiyAuthorizeRelay<SysAgentUser>();
            SysAgentUser user = identiy.GetUser(model.SysAgentUser.Id);

            user.FullName = model.SysAgentUser.FullName;
            user.Email = model.SysAgentUser.Email;
            user.PhoneNumber = model.SysAgentUser.PhoneNumber;
            user.Mender = this.CurrentUserId;
            user.LastUpdateTime = DateTime.Now;


            bool r = identiy.UpdateUser(this.CurrentUserId, user, model.SysAgentUser.PasswordHash);
            if (!r)
            {
                return Json(ResultType.Failure, OwnOperateTipUtils.UPDATE_FAILURE);
            }
            return Json(ResultType.Success, OwnOperateTipUtils.UPDATE_SUCCESS);
        }
    }
}