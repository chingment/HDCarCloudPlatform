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
        private static List<YdtInsComanyModel> YdtInsComanyList()
        {
            List<YdtInsComanyModel> list = new List<YdtInsComanyModel>();
            list.Add(new YdtInsComanyModel { UpLinkCode = 1, YdtCode = "002000", Name = "平安保险",PrintName= "平安保险有限公司", ChannelId= 19 });
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

        private static List<YdtInsCoverageModel> YdtInsCoverageList()
        {
            List<YdtInsCoverageModel> list = new List<YdtInsCoverageModel>();
            list.Add(new YdtInsCoverageModel { UpLinkCode = 3, YdtCode = "001", Name = "车损险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 4, YdtCode = "002", Name = "三者险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 5, YdtCode = "003", Name = "司机险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 6, YdtCode = "004", Name = "乘客险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 7, YdtCode = "005", Name = "盗抢险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 8, YdtCode = "006", Name = "玻璃险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 9, YdtCode = "007", Name = "划痕险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 12, YdtCode = "008", Name = "自燃险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 11, YdtCode = "009", Name = "涉水险" });
            list.Add(new YdtInsCoverageModel { UpLinkCode = 17, YdtCode = "010", Name = "指定维修厂" });

            return list;
        }

        public static YdtInsComanyModel GetCompanyCode(int uplinkInsCarCompanyId)
        {
            var comany = YdtInsComanyList().Where(m => m.UpLinkCode == uplinkInsCarCompanyId).FirstOrDefault();

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

        public static List<CoveragesModel> GetCoverages(List<OrderToCarInsureOfferKind> kinds, decimal oldAmount, int carSeat)
        {
            List<CoveragesModel> list = new List<CoveragesModel>();
            var ydtInsCoverageList = YdtInsCoverageList();
            foreach (var kind in kinds)
            {
                var coverage = ydtInsCoverageList.Where(m => m.UpLinkCode == kind.KindId).FirstOrDefault();
                if (coverage != null)
                {
                    CoveragesModel model = new CoveragesModel();
                    model.code = coverage.YdtCode;

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
                        model.amount = model.unitAmount * model.quantity;
                    }

                    #endregion

                    #region  乘客险
                    if (kind.KindId == 6)
                    {
                        var sCarSeat = carSeat - 1;

                        model.unitAmount = GetCoverageAmount(kind.KindValue);
                        model.quantity = sCarSeat;
                        model.amount = model.unitAmount * model.quantity;
                    }
                    #endregion

                    #region 划损险

                    if (kind.KindId == 9)
                    {
                        model.unitAmount = GetCoverageAmount(kind.KindValue);
                        model.quantity =1;
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

        public static int GetRisk(List<OrderToCarInsureOfferKind> kinds)
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
