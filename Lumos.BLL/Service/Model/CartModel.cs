﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{

    public class CartBlock
    {

        public string TagName { get; set; }

        public List<CartProcudtSkuListModel> Skus { get; set; }

        public int ChannelId { get; set; }

        public Entity.Enumeration.ChannelType ChannelType { get; set; }
    }

    public class CartModel
    {
        public CartModel()
        {
            this.Block = new List<CartBlock>();
        }

        public List<CartBlock> Block { get; set; }

        public int Count { get; set; }

        public decimal SumPrice { get; set; }

        public int CountBySelected { get; set; }

        public decimal SumPriceBySelected { get; set; }
    }
}