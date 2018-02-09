using Lumos.Common;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebBack.Controllers.Sys
{

    public class PermissionController : OwnBaseController
    {
    
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取权限树形列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPermissionTree()
        {
            var identity = new AspNetIdentiyAuthorizeRelay<SysUser>();
            object json = ConvertToZTreeJson(identity.GetPermissionList(new PermissionCode()).ToArray(), "id", "pid", "name", "opfun");
            return Json(ResultType.Success, json);
        }

    }
}