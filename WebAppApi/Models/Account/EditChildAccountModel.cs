using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Account
{
    public class EditChildAccountModel
    {

        public int UserId { get; set; }

        public int AccountId { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { get; set; }
    }
}