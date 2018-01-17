using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{
    public class CarInsPlanModel
    {

        public CarInsPlanModel()
        {
            this.KindParent = new List<CarInsPlanKindParentModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public List<CarInsPlanKindParentModel> KindParent { get; set; }

    }
}