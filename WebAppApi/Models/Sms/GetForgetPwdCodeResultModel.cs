﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Sms
{
    public class GetForgetPwdCodeResultModel
    {
        public string UserName { get; set; }

        public string ValidCode { get; set; }

        public string Token {get; set; }

        public int Seconds { get; set; }
    }
}