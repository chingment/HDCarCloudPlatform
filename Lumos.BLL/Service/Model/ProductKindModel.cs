﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class ProductKindModel
    {

        public ProductKindModel()
        {
            this.Child = new List<ProductChildKindModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public bool Selected { get; set; }

        public List<ProductChildKindModel> Child { get; set; }
    }
}
