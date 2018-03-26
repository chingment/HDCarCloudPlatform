using Lumos.BLL;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class LllegalController : OwnBaseApiController
    {
        
        [HttpPost]
        public APIResponse<LllegalQueryResult> Query(LllegalQueryParams model)
        {
            var result = SdkFactory.HeLian.Query(model.UserId, model);

            return new APIResponse<LllegalQueryResult>(result);

        }

        [HttpGet]
        public APIResponse<List<LllegalQueryRecord>> QueryLog(int userId, int merchantId, int posMachineId)
        {

            var result = SdkFactory.HeLian.QueryLog(userId, userId, merchantId, posMachineId);

            return new APIResponse<List<LllegalQueryRecord>>(result);

        }
    }
}