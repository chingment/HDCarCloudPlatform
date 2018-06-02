using Lumos.Entity.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class InsPlan
    {
        public InsPlan()
        {
            this.DetailsItems = new List<ItemField>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public bool IsTeam { get; set; }
        public List<ItemField> DetailsItems { get; set; }
        public decimal Price { get; set; }
    }

    public class InsPlanUntil
    {
        private static List<InsPlan> GetAll()
        {
            List<InsPlan> insPlans = new List<InsPlan>();

            #region InsPrd1
            var insPlan1 = new InsPlan();
            insPlan1.PlanId = 1;
            insPlan1.PlanName = "泰康“呵护一生”方案一";
            insPlan1.CompanyId = 0;
            insPlan1.CompanyName = "泰康保险";
            insPlan1.IsTeam = false;
            insPlan1.Price = 60;
            insPlan1.DetailsItems.Add(new ItemField { field = "被保险人年龄", value = "0-65岁" });
            insPlan1.DetailsItems.Add(new ItemField { field = "意外身故/伤残", value = "10万" });
            insPlan1.DetailsItems.Add(new ItemField { field = "意外医疗", value = "1W" });
            insPlan1.DetailsItems.Add(new ItemField { field = "保费", value = "60元" });
            insPlans.Add(insPlan1);

            var insPlan2 = new InsPlan();
            insPlan2.PlanId = 2;
            insPlan2.PlanName = "泰康“呵护一生”方案二";
            insPlan2.CompanyId = 0;
            insPlan2.CompanyName = "泰康保险";
            insPlan2.IsTeam = false;
            insPlan2.Price = 60;
            insPlan2.DetailsItems.Add(new ItemField { field = "被保险人年龄", value = "10-65岁" });
            insPlan2.DetailsItems.Add(new ItemField { field = "意外身故/伤残", value = "20万" });
            insPlan2.DetailsItems.Add(new ItemField { field = "意外医疗", value = "2W" });
            insPlan2.DetailsItems.Add(new ItemField { field = "保费", value = "120元" });
            insPlans.Add(insPlan2);

            var insPlan3 = new InsPlan();
            insPlan3.PlanId = 3;
            insPlan3.PlanName = "泰康“呵护一生”方案三";
            insPlan3.CompanyId = 0;
            insPlan3.CompanyName = "泰康保险";
            insPlan3.IsTeam = false;
            insPlan3.Price = 60;
            insPlan3.DetailsItems.Add(new ItemField { field = "被保险人年龄", value = "18-65岁" });
            insPlan3.DetailsItems.Add(new ItemField { field = "意外身故/伤残", value = "30万" });
            insPlan3.DetailsItems.Add(new ItemField { field = "意外医疗", value = "3W" });
            insPlan3.DetailsItems.Add(new ItemField { field = "保费", value = "180元" });
            insPlans.Add(insPlan3);

            #endregion

            return insPlans;
        }

        public static InsPlan Get(int planId)
        {
            InsPlan insPlan = GetAll().Where(m => m.PlanId == planId).FirstOrDefault();

            return insPlan;
        }
    }
}
