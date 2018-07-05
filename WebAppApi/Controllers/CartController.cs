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
        public APIResponse Operate(OperatePms pms)
        {
            IResult result = ServiceFactory.Cart.Operate(pms.UserId, pms.Operate, pms.UserId, pms.MerchantId, pms.PosMachineId, pms.Skus);

            return new APIResponse(result);

        }

        [HttpGet]
        public APIResponse GetPageData(int userId, int merchantId, int posMachineId)
        {
            var model = ServiceFactory.Cart.GetPageData(userId, merchantId, posMachineId);

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", model);

        }

        [HttpGet]
        public APIResponse GetShoppingData(int userId, int merchantId, int posMachineId)
        {
            var model = ServiceFactory.Cart.GetShoppingData(userId, merchantId, posMachineId);

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", model);

        }

        [HttpPost]
        public APIResponse GetComfirmOrderData(GetComfirmOrderDataPms pms)
        {

            var result = ServiceFactory.Cart.GetComfirmOrderData(pms.UserId, pms.UserId, pms.MerchantId, pms.PosMachineId, pms.Skus);

            return new APIResponse(result);
        }
    }
}