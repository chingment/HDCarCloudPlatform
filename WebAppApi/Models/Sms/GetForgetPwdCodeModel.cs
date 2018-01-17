using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Sms
{
    public class GetForgetPwdCodeModel
    {
        public string UserName { get; set; }

        public string Phone { get; set; }

    }
}