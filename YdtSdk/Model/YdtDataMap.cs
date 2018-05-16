using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtDataMap
    {
        public static List<YdtInsComanyModel> YdtInsComanyList()
        {
            List<YdtInsComanyModel> list = new List<YdtInsComanyModel>();
            list.Add(new YdtInsComanyModel { UpLinkCode = 1, YdtCode = "002000", Name = "平安保险", PrintName = "平安保险有限公司", ChannelId = 19 });
            list.Add(new YdtInsComanyModel { UpLinkCode = 2, YdtCode = "008000", Name = "太平洋保险", PrintName = "太平洋保险有限公司" });
            list.Add(new YdtInsComanyModel { UpLinkCode = 3, YdtCode = "003000", Name = "阳光保险", PrintName = "阳光保险有限公司", ChannelId = 12 });
            list.Add(new YdtInsComanyModel { UpLinkCode = 4, YdtCode = "", Name = "亚太保险", PrintName = "亚太保险有限公司" });
            list.Add(new YdtInsComanyModel { UpLinkCode = 5, YdtCode = "005000", Name = "人民保险", PrintName = "人民保险有限公司" });
            list.Add(new YdtInsComanyModel { UpLinkCode = 6, YdtCode = "", Name = "中华保险", PrintName = "中华保险有限公司", ChannelId = 16 });
            //list.Add(new YdtInsComanyModel { UpLinkCode = 7, YdtCode = "005000", Name = "人民保险(佛山)", PrintName = "人民保险有限公司" });
            list.Add(new YdtInsComanyModel { UpLinkCode = 8, YdtCode = "007000", Name = "大地保险", PrintName = "大地保险有限公司", ChannelId = 6 });
            list.Add(new YdtInsComanyModel { UpLinkCode = 9, YdtCode = "006000", Name = "太平保险", PrintName = "太平保险有限公司", ChannelId = 1 });

            return list;
        }

        public static List<YdtInsCoverageModel> YdtInsCoverageList()
        {

            List<YdtInsCoverageModel> list = new List<YdtInsCoverageModel>();
            list.Add(new YdtInsCoverageModel { UpLinkCode = 3, Code = "001", Name = "车损险", IsCompensation = false, Priority = 1 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 4, Code = "002", Name = "三者险", IsCompensation = false, Priority = 2 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 5, Code = "003", Name = "司机险", IsCompensation = false, Priority = 3 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 6, Code = "004", Name = "乘客险", IsCompensation = false, Priority = 4 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 7, Code = "005", Name = "盗抢险", IsCompensation = false, Priority = 5 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 8, Code = "006", Name = "玻璃险", IsCompensation = false, Priority = 6 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 9, Code = "007", Name = "划痕险", IsCompensation = false, Priority = 7 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 12, Code = "008", Name = "自燃险", IsCompensation = false, Priority = 8 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 11, Code = "009", Name = "涉水险", IsCompensation = false, Priority = 9 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 17, Code = "010", Name = "指定维修厂", IsCompensation = false, Priority = 10 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 10, Code = "011", Name = "无法找到第三方险", IsCompensation = false, Priority = 11 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 20, Code = "101", Name = "车损险不计免赔", IsCompensation = true, Priority = 12 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 21, Code = "102", Name = "三者险不计免赔", IsCompensation = true, Priority = 13 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 22, Code = "112", Name = "车上人员不计免赔", IsCompensation = true, Priority = 14 });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 23, Code = "110", Name = "附加险不计免赔", IsCompensation = true, Priority = 15 });
            return list;
        }

        public static YdtInsComanyModel GetCompanyCode(int uplinkInsCarCompanyId)
        {
            var comany = YdtInsComanyList().Where(m => m.UpLinkCode == uplinkInsCarCompanyId).FirstOrDefault();

            return comany;
        }

        public static YdtInsComanyModel GetCompanyByCode(string code)
        {
            var comany = YdtInsComanyList().Where(m => m.YdtCode == code).FirstOrDefault();

            return comany;
        }

        public static decimal GetCoverageAmount(string d)
        {
            if (string.IsNullOrEmpty(d))
                return 0;

            decimal amount = 0;
            d = d.ToLower();
            int of = d.IndexOf('w');
            if (of > -1)
            {
                d = d.Substring(0, of);

                amount = decimal.Parse(d) * 10000;

            }
            else
            {
                amount = decimal.Parse(d);
            }

            return amount;
        }

        public static List<CoverageModel> GetCoverages(List<OrderToCarInsureOfferCompanyKind> kinds, decimal oldAmount, int carSeat)
        {
            List<CoverageModel> list = new List<CoverageModel>();
            var ydtInsCoverageList = YdtInsCoverageList();
            foreach (var kind in kinds)
            {
                var coverage = ydtInsCoverageList.Where(m => m.UpLinkCode == kind.KindId).FirstOrDefault();
                if (coverage != null)
                {
                    CoverageModel model = new CoverageModel();
                    model.code = coverage.Code;

                    #region 是否免损
                    switch (kind.KindId)
                    {
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 9:
                        case 11:
                        case 12:
                            if (kind.IsWaiverDeductible)
                            {
                                model.compensation = 1;
                            }
                            else
                            {
                                model.compensation = 0;
                            }
                            break;
                    }
                    #endregion

                    #region 需要折旧价
                    switch (kind.KindId)
                    {
                        case 3:
                        case 7:
                        case 10:
                        case 11:
                        case 12:
                        case 17:
                            model.amount = oldAmount;
                            break;
                    }
                    #endregion

                    #region 玻璃险
                    if (kind.KindId == 8)
                    {
                        if (kind.KindValue == "国产")
                        {
                            model.glassType = 1;
                        }
                        else
                        {
                            model.glassType = 2;
                        }
                    }
                    #endregion

                    #region 第三者责任

                    if (kind.KindId == 4)
                    {
                        model.amount = GetCoverageAmount(kind.KindValue);
                    }

                    #endregion

                    #region 司乘险

                    if (kind.KindId == 5)
                    {
                        model.unitAmount = GetCoverageAmount(kind.KindValue);
                        model.quantity = 1;
                        model.amount = model.unitAmount.Value * model.quantity.Value;
                    }

                    #endregion

                    #region  乘客险
                    if (kind.KindId == 6)
                    {
                        var sCarSeat = carSeat - 1;

                        model.unitAmount = GetCoverageAmount(kind.KindValue);
                        model.quantity = sCarSeat;
                        model.amount = model.unitAmount.Value * model.quantity.Value;
                    }
                    #endregion

                    #region 划损险

                    if (kind.KindId == 9)
                    {
                        model.unitAmount = GetCoverageAmount(kind.KindValue);
                        model.quantity = 1;
                        model.amount = GetCoverageAmount(kind.KindValue);
                    }

                    #endregion


                    //model.amount = kind.KindValue;

                    // model.unitAmount = kind.KindValue;

                    //  model.quantity = kind.KindValue;


                    //  model.glassType = kind.KindValue;

                    list.Add(model);
                }
            }

            return list;
        }

        public static int GetRisk(List<OrderToCarInsureOfferCompanyKind> kinds)
        {
            if (kinds == null)
                return 2;

            if (kinds.Count == 0)
                return 2;

            var list = kinds.Where(m => (m.KindId != 1 || m.KindId != 2) && m.Id >= 3).ToList();
            if (list.Count > 0)
                return 1;


            int[] sId = new int[2] { 1, 2 };
            var list2 = kinds.Where(m => sId.Contains(m.KindId)).ToList();
            if (list2.Count == 2 && kinds.Count == 2)
                return 2;

            return 3;

        }


    }
}
