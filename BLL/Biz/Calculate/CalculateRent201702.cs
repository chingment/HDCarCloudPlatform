using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{

    public class RentList
    {
        public RentList(string text, int months, decimal price)
        {
            this.Text = text;
            this.Months = months;
            this.Price = price;

        }
        public string Text { get; set; }

        public int Months { get; set; }

        public decimal Price { get; set; }
    }

    public class CalculateRent
    {
        private string _version = "2017.02.20";
        private List<RentList> _rentList = new List<RentList>();
        private string _remark = "公告：在优惠活动期间激活的亲，如果选择租期6个月可获得100元的减免优惠，即1100元；选择12个月的租期可获得200元的减免优惠，即2200元！";
        private decimal _monthlyRent = 200;


        public string Version
        {
            get
            {
                return _version;
            }

        }

        public string Remark
        {
            get
            {
                return _remark;
            }

        }

        public decimal MonthlyRent
        {
            get
            {
                return _monthlyRent;
            }
        }

        public List<RentList> RentList
        {
            get
            {
                return _rentList;
            }
        }

        public CalculateRent(decimal monthlyRent)
        {
            _monthlyRent = monthlyRent;

            this.RentList.Add(new BLL.RentList("3个月", 3, Math.Round(3 * monthlyRent, 2)));
            this.RentList.Add(new BLL.RentList("6个月", 6, Math.Round(6 * monthlyRent * 0.91666667m, 2)));
            this.RentList.Add(new BLL.RentList("12个月", 12, Math.Round(12 * monthlyRent * 0.91666667m, 2)));
        }

        public decimal GetRent(int months)
        {
            var rent = this.RentList.Where(m => m.Months == months).FirstOrDefault();
            return rent.Price;
        }

    }
}
