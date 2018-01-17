using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models
{
    public class ExtendedAppModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LinkUrl { get; set; }

        public string ImgUrl { get; set; }

        public string AppKey { get; set; }

        public string AppSecret { get; set; }
    }
}