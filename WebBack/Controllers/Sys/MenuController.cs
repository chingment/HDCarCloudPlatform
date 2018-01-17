using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebBack.Models.Sys.Menu;

namespace WebBack.Controllers.Sys
{
    [OwnAuthorize(PermissionCode.菜单管理)]
    public class MenuController : WebBackController
    {

        public ActionResult List()
        {
            ListViewModel mode = new ListViewModel();
            return View(mode);
        }

        public ActionResult Add()
        {
            AddViewModel mode = new AddViewModel();
            return View(mode);
        }

        public ActionResult Sort()
        {
            return View();
        }



        public JsonResult GetDetails(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return Json(ResultType.Success, model, "");
        }

        public JsonResult GetTree(int pId)
        {
            SysMenu[] arr;
            if (pId == 0)
            {
                arr = CurrentDb.SysMenu.OrderByDescending(m => m.Priority).ToArray();
            }
            else
            {
                arr = CurrentDb.SysMenu.Where(m => m.PId == pId).OrderByDescending(m => m.Priority).ToArray();
            }

            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);
        }


        [HttpPost]
        [OwnNoResubmit]
        [ValidateAntiForgeryToken]
        public JsonResult Add(AddViewModel model)
        {
            var identity = new AspNetIdentiyAuthorizeRelay<SysUser>();
            identity.CreateMenu(this.CurrentUserId, model.SysMenu, model.SysMenu.Permission);
            return Json(ResultType.Success, OwnOperateTipUtils.ADD_SUCCESS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EditViewModel model)
        {
            int menuId = model.SysMenu.Id;
            SysMenu menu = CurrentDb.SysMenu.Find(menuId);
            menu.Name = model.SysMenu.Name;
            menu.Url = model.SysMenu.Url;
            menu.Description = model.SysMenu.Description;
            var identityWebBack = new AspNetIdentiyAuthorizeRelay<SysUser>();
            identityWebBack.UpdateMenu(this.CurrentUserId, menu, model.SysMenu.Permission);
            return Json(ResultType.Success, OwnOperateTipUtils.UPDATE_SUCCESS);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int[] ids)
        {
            var identity = new AspNetIdentiyAuthorizeRelay<SysUser>();

            identity.DeleteMenu(this.CurrentUserId, ids);

            return Json(ResultType.Success, OwnOperateTipUtils.DELETE_SUCCESS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditSort()
        {

            for (int i = 0; i < Request.Form.Count; i++)
            {
                string name = Request.Form.AllKeys[i].ToString();
                if (name.IndexOf("menuId") > -1)
                {
                    int id = int.Parse(name.Split('_')[1].Trim());
                    int priority = int.Parse(Request.Form[i].Trim());
                    SysMenu model = CurrentDb.SysMenu.Where(m => m.Id == id).FirstOrDefault();
                    if (model != null)
                    {
                        model.Priority = priority;
                        CurrentDb.SaveChanges();
                    }
                }
            }
            return Json(ResultType.Success, OwnOperateTipUtils.UPDATE_SUCCESS);

        }

    }
}