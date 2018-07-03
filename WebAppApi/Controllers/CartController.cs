using Lumos.BLL.Service;
using Lumos.Entity;
using Lumos.Mvc;
using System.Web.Http;
using WebAppApi.Models.Cart;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class CartController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse Operate(OperateParams model)
        {
            IResult result = ServiceFactory.Cart.Operate(model.UserId, model.Operate, model.UserId, model.MerchantId, model.PosMachineId, model.Skus);

            return new APIResponse(result);

        }

        [HttpGet]
        public APIResponse GetPageData(int userId, int merchantId, int posMachineId)
        {
            var model = ServiceFactory.Cart.GetPageData(userId, merchantId, posMachineId);

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", model);

        }
    }
}