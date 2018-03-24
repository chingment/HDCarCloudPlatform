using Lumos.BLL;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgent.Models.SalesmanUser;

namespace WebAgent.Controllers
{
    public class SalesmanUserController : OwnBaseController
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
            model.LoadData(this.CurrentUserId);
            return View(model);
        }

        public ViewResult Edit(int id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }


        #endregion

        public CustomJsonResult GetList(SalesmanUserSearchCondition condition)
        {
            var list = (from u in CurrentDb.SysSalesmanUser
                        where (condition.UserName == null || u.UserName.Contains(condition.UserName)) &&
                        (condition.FullName == null || u.FullName.Contains(condition.FullName)) &&
                        u.IsDelete == false
                          && u.AgentId == this.CurrentUserId
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        public CustomJsonResult GetSelectList(SalesmanUserSearchCondition condition)
        {
            var list = (from u in CurrentDb.SysSalesmanUser
                        where (condition.UserName == null || u.UserName.Contains(condition.UserName)) &&
                        (condition.FullName == null || u.FullName.Contains(condition.FullName)) &&
                        u.IsDelete == false
                        && u.AgentId == this.CurrentUserId
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
        public CustomJsonResult Add(AddViewModel model)
        {

            SysSalesmanUser user = new SysSalesmanUser();

            var agent = CurrentDb.SysAgentUser.Where(m => m.Id == this.CurrentUserId).FirstOrDefault();

            user.UserName = string.Format("{0}{1}", agent.UserName, model.SysSalesmanUser.UserName);
            user.AgentId = agent.Id;
            user.FullName = model.SysSalesmanUser.FullName;
            user.PasswordHash = PassWordHelper.HashPassword("888888");
            user.Email = model.SysSalesmanUser.Email;
            user.PhoneNumber = model.SysSalesmanUser.PhoneNumber;
            user.IsDelete = false;
            user.Status = Enumeration.UserStatus.Normal;
            user.Type = Enumeration.UserType.Salesman;

            return SysFactory.AuthorizeRelay.CreateUser<SysSalesmanUser>(this.CurrentUserId, user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Edit(EditViewModel model)
        {

            var user = new SysSalesmanUser();

            user.Password = model.SysSalesmanUser.Password;
            user.Id = model.SysSalesmanUser.Id;
            user.FullName = model.SysSalesmanUser.FullName;
            user.Email = model.SysSalesmanUser.Email;
            user.PhoneNumber = model.SysSalesmanUser.PhoneNumber;

            return SysFactory.AuthorizeRelay.UpdateUser<SysSalesmanUser>(this.CurrentUserId, user);

        }

    }
}