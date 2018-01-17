using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Sms
{
    public class GetAddChildAccountCodeModel
    {
        public int UserId { get; set; }

        public string AccountPhone { get; set; }
    }

}