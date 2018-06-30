using Lumos.BLL.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service
{
    public class PersonalService : BaseProvider
    {
        public PersonalModel GetPageData(int userId)
        {
            var model = new PersonalModel();

            var user = CurrentDb.SysUser.Where(m => m.Id == userId).FirstOrDefault();
            if (user != null)
            {
                var userInfo = new UserInfoModel();
                userInfo.UserId = user.Id;
                userInfo.NickName = user.UserName;
                userInfo.Phone = user.PhoneNumber;
                userInfo.HeadImg = "http://thirdwx.qlogo.cn/mmopen/vi_32/6zcicmSoM5yjdWG9MoHydE6suFUGaHsKATFUPU7yU4d7PhLcsKWj51NhxA4PichkuY5uWvbEvXZWBGBpJSd48GNA/132";


                model.UserInfo = userInfo;
            }


            return model;
        }
    }
}
