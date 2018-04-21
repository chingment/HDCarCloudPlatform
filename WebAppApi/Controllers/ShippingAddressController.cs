using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;



namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class ShippingAddressController : OwnBaseApiController
    {

        [HttpGet]
        public APIResponse GetList(int userId, int merchantId, int posMachineId, int pageIndex, Enumeration.ProductType type, int categoryId, int kindId, string name)
        {
            return null;
        }

        [HttpPost]
        public APIResponse Edit(EditModel model)
        {
            return null;
        }
    }
}