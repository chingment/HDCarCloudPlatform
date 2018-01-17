using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class CarInsurePlanProvider : BaseProvider
    {
        public string GetPlanName(int id)
        {
            var m = CurrentDb.CarInsurePlan.Where(q => q.Id == id).FirstOrDefault();
            if (m == null)
                return "";

            return m.Name;
        }
    }
}
