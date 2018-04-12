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
            IResult result = ServiceFactory.Cart.Operate(model.UserId, model.Operate, model.UserId,model.List);

            return new APIResponse(result);

        }
    }
}