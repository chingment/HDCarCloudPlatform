using Lumos.BLL.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Global
{
    public class DataSetModel
    {
        public ProductKindModel ProductKind { get; set; }

        public CartModel Cart { get; set; }
    }
}