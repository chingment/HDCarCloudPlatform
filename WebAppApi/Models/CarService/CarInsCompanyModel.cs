using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{
    public class CarInsCompanyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        //能投保
        public bool CanInsure { get; set; }
        //能理赔
        public bool CanClaims { get; set; }
    }
}