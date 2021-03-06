﻿using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using MySDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SmsProvider : BaseProvider
    {

        public bool Send(string smsPhone, string smsContent)
        {

            return true;

        }

        public string BuildValidCode()
        {
            VerifyCodeHelper v = new VerifyCodeHelper();
            v.CodeSerial = "0,1,2,3,4,5,6,7,8,9";
            v.Length = 6;
            string code = v.CreateVerifyCode(); //取随机码 

            return code;
        }

        public CustomJsonResult SendCarInsureOfferComplete(int userId, string phone, string orderSn, string carOwner, string carPlateno)
        {
            string token = null;
            CustomJsonResult result = SmsHelper.Send("SMS_129757235", "{\"ordersn\":\"" + orderSn + "\",\"carowner\":\"" + carOwner + "\",\"carplateno\":\"" + carPlateno + "\"}", phone, out token);
            return result;
        }

        public CustomJsonResult SendCarInsureOfferFollow(int userId, string phone, string orderSn, string carOwner, string carPlateno)
        {
            string token = null;
            CustomJsonResult result = SmsHelper.Send("SMS_129757237", "{\"ordersn\":\"" + orderSn + "\",\"carowner\":\"" + carOwner + "\",\"carplateno\":\"" + carPlateno + "\"}", phone, out token);
            return result;
        }

        public CustomJsonResult SendGetForgetPwdCode(int userId, string phone, out string validcode, out string token, out int seconds)
        {
            validcode = BuildValidCode();
            seconds = 600;
            CustomJsonResult result = SmsHelper.Send("SMS_129757696", "{\"code\":\"" + validcode + "\"}", phone, out token, validcode, seconds);
            return result;
        }


        public CustomJsonResult SendCreateAccountCode(int operater, string phone, out string validcode, out string token, out int seconds)
        {
            validcode = BuildValidCode();
            seconds = 600;
            CustomJsonResult result = SmsHelper.Send("SMS_129747717", "{\"code\":\"" + validcode + "\"}", phone, out token, validcode, seconds);
            return result;
        }

    }
}
