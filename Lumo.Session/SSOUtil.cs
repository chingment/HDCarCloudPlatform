using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Session
{
    public static class SSOUtil
    {
        public static void SetUserInfo(UserInfo userInfo)
        {
            var session = new Session();
            session.Set(userInfo.Token, userInfo);
        }

        public static UserInfo GetUserInfo(string key)
        {
            var session = new Session();
            return session.Get<UserInfo>(key);
        }

        public static void Postpone(string key)
        {
            var session = new Session();

            session.Postpone(key);
        }

        public static void Quit(string key)
        {
            var session = new Session();

            session.Quit(key);
        }
    }
}
