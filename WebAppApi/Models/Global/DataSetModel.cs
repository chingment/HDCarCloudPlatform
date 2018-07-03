﻿using Lumos.BLL.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Global
{
    public class DataSetModel
    {
        public IndexModel Index{ get; set; }

        public ProductKindPageModel ProductKind { get; set; }

        public CartPageDataModel Cart { get; set; }

        public PersonalModel Personal { get; set; }
    }
}