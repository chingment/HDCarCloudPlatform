﻿using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtDataMap
    {
        public static List<YdtInscarComanyModel> YdtInsComanyList()
        {
            List<YdtInscarComanyModel> list = new List<YdtInscarComanyModel>();
            list.Add(new YdtInscarComanyModel { UpLinkCode = 1, YdtCode = "002000", Name = "平安保险", PrintName = "平安保险", ChannelId = 19 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 2, YdtCode = "008000", Name = "太平洋保险", PrintName = "太平洋保险", ChannelId = 15 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 3, YdtCode = "003000", Name = "阳光保险", PrintName = "阳光保险", ChannelId = 12 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 5, YdtCode = "001400", Name = "广州中保", PrintName = "广州中保", ChannelId = 26 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 6, YdtCode = "004000", Name = "广州中华联合", PrintName = "广州中华联合", ChannelId = 16 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 8, YdtCode = "007000", Name = "大地保险", PrintName = "大地保险", ChannelId = 6 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 9, YdtCode = "006000", Name = "太平保险", PrintName = "太平保险", ChannelId = 1 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 10, YdtCode = "001000", Name = "广州众诚", PrintName = "广州众诚", ChannelId = 18 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 11, YdtCode = "001700", Name = "广州安心", PrintName = "广州安心", ChannelId = 78 });
            list.Add(new YdtInscarComanyModel { UpLinkCode = 13, YdtCode = "001600", Name = "广州国任(信达)", PrintName = "广州国任(信达)", ChannelId = 28 });
            return list;
        }

        public static List<YdtInscarCoveragesModel> YdtInsCoverageList()
        {

            List<YdtInscarCoveragesModel> list = new List<YdtInscarCoveragesModel>();
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 3, Code = "001", Name = "车损险", IsWaiverDeductibleKind = false, Priority = 1, WaiverDeductibleKindCode = "101" });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 4, Code = "002", Name = "三者险", IsWaiverDeductibleKind = false, Priority = 2, WaiverDeductibleKindCode = "112" });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 5, Code = "003", Name = "司机险", IsWaiverDeductibleKind = false, Priority = 3, WaiverDeductibleKindCode = "103" });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 6, Code = "004", Name = "乘客险", IsWaiverDeductibleKind = false, Priority = 4, WaiverDeductibleKindCode = "104" });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 7, Code = "005", Name = "盗抢险", IsWaiverDeductibleKind = false, Priority = 5, WaiverDeductibleKindCode = "105" });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 8, Code = "006", Name = "玻璃险", IsWaiverDeductibleKind = false, Priority = 6 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 9, Code = "007", Name = "划痕险", IsWaiverDeductibleKind = false, Priority = 7, WaiverDeductibleKindCode = "106" });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 12, Code = "008", Name = "自燃险", IsWaiverDeductibleKind = false, Priority = 8, WaiverDeductibleKindCode = "107" });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 11, Code = "009", Name = "涉水险", IsWaiverDeductibleKind = false, Priority = 9, WaiverDeductibleKindCode = "108" });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 17, Code = "010", Name = "指定维修厂", IsWaiverDeductibleKind = false, Priority = 10 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 10, Code = "011", Name = "无法找到第三方险", IsWaiverDeductibleKind = false, Priority = 11 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 31, Code = "101", Name = "车损险不计免赔", IsWaiverDeductibleKind = true, Priority = 12 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 32, Code = "102", Name = "三者险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 33, Code = "103", Name = "司机险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 34, Code = "104", Name = "乘客险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 35, Code = "105", Name = "盗抢险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 36, Code = "106", Name = "划痕险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 37, Code = "107", Name = "自燃险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 38, Code = "108", Name = "涉水险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 39, Code = "109", Name = "主险不计免赔", IsWaiverDeductibleKind = true, Priority = 13 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 40, Code = "110", Name = "附加险不计免赔", IsWaiverDeductibleKind = true, Priority = 15 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 41, Code = "111", Name = "总不计免赔", IsWaiverDeductibleKind = true, Priority = 16 });
            list.Add(new YdtInscarCoveragesModel { UpLinkCode = 42, Code = "112", Name = "车上人员不计免赔", IsWaiverDeductibleKind = true, Priority = 14 });
            return list;
        }

        public static YdtInscarComanyModel GetCompanyCode(int uplinkInsCarCompanyId)
        {
            var comany = YdtInsComanyList().Where(m => m.UpLinkCode == uplinkInsCarCompanyId).FirstOrDefault();

            return comany;
        }

        public static YdtInscarComanyModel GetCompanyByCode(string code)
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
