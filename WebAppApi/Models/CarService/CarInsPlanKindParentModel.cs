using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{
    public class CarInsPlanKindParentModel
    {
        public CarInsPlanKindParentModel()
        {
            this.Child = new List<int>();
        }

        public int Id { get; set; }

        public List<int> Child { get; set; }
    }
}