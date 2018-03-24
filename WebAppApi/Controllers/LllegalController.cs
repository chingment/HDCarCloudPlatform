using Lumos.BLL;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class LllegalController : OwnBaseApiController
    {
        // GET: Lllegal
        public APIResponse<List<LllegalRecord>> Query(LllegalQueryParams model)
        {
            var result = SdkFactory.HeLian.Query(model.UserId, model);

            return new APIResponse<List<LllegalRecord>>(result);

        }
    }
}