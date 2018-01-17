using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Biz.Task
{
    class TaskForMerchantTransactionsCheck : BaseProvider, ITask
    {

        public string BulidOpendId(string merchantCode, string key, string secret)
        {

            byte[] result = Encoding.Default.GetBytes(merchantCode + key + secret);    //tbPass为输入密码的文本框  
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string s_output = BitConverter.ToString(output).Replace("-", "");

            return s_output;
        }

        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();

            //处理提交之后24内没有支付的车险订单,以报价完成时间
            var canleList = CurrentDb.OrderToCarInsure.Where(m => SqlFunctions.DateDiff("hour", m.EndOfferTime, this.DateTime) >= m.AutoCancelByHour && m.Status == Enumeration.OrderStatus.WaitPay).ToList();

            foreach (var m in canleList)
            {
                m.CancleTime = this.DateTime;
                m.Status = Enumeration.OrderStatus.Cancled;

                var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(c => c.AduitReferenceId == m.Id && c.AduitType == Enumeration.BizProcessesAuditType.CarInsure).FirstOrDefault();

                BizFactory.BizProcessesAudit.ChangeAuditDetails(0, Enumeration.CarInsureOfferDealtStep.Cancle, l_bizProcessesAudit.Id, 0, null, "超过1天未支付，系统自动取消，请重新提交报价", this.DateTime);

                BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(0, l_bizProcessesAudit.Id, Enumeration.CarInsureOfferDealtStatus.StaffCancle);

                CurrentDb.SaveChanges();

                Log.InfoFormat("订单编号:{0}，在24小时内没有支付车险订单，系统自动取消", m.Sn);
            }

            //生成商户的对应OpenId
            //var merchant = CurrentDb.Merchant.ToList();

            //foreach (var m in merchant)
            //{
            //    var openIdInfo = CurrentDb.SysOpenIdInfo.Where(q => q.ClientCode == m.ClientCode && q.OpenIdType == Enumeration.OpenIdType.Cgj).FirstOrDefault();

            //    if (openIdInfo == null)
            //    {
            //        openIdInfo = new SysOpenIdInfo();
            //        openIdInfo.ClientCode = m.ClientCode;
            //        openIdInfo.OpenId = BulidOpendId(m.ClientCode, "cgjCarApp", "971251a0942548adb59c4c88501c7055");
            //        openIdInfo.OpenIdType = Enumeration.OpenIdType.Cgj;
            //        CurrentDb.SysOpenIdInfo.Add(openIdInfo);
            //        CurrentDb.SaveChanges();

            //        Log.InfoFormat("生成商户代码:{0},车管家对应的opendId:{1}", m.ClientCode, openIdInfo.OpenId);

            //    }
            //}


            #region 生成订单佣金
            //var orders = CurrentDb.OrderToCarInsure.Where(m => m.Status == Enumeration.OrderStatus.Completed).ToList();

            //foreach (var orderToCarInsure in orders)
            //{

            //    var commissionRate = CurrentDb.CarInsureCommissionRate.Where(m => (m.Type == Enumeration.CommissionRateType.Uplink && m.ReferenceId == orderToCarInsure.InsuranceCompanyId) || (m.Type == Enumeration.CommissionRateType.YiBanShi && m.ReferenceId == 1) || (m.Type == Enumeration.CommissionRateType.InsuranceCompany && m.ReferenceId == orderToCarInsure.InsuranceCompanyId)).ToList();


            //    var uplink_CommissionRate = commissionRate.Where(m => m.Type == Enumeration.CommissionRateType.Uplink).FirstOrDefault();


            //    var uplink_Commission_Commercial = orderToCarInsure.CommercialPrice * (uplink_CommissionRate.Commercial / 100);
            //    var uplink_Commission_Compulsory = orderToCarInsure.CompulsoryPrice * (uplink_CommissionRate.Compulsory / 100);
            //    var uplink_Commission = PayProvider.GetPayDecimal(uplink_Commission_Commercial + uplink_Commission_Compulsory);


            //    var yiBanShi_CommissionRate = commissionRate.Where(m => m.Type == Enumeration.CommissionRateType.YiBanShi).FirstOrDefault();



            //    var merchant_CommissionRate = commissionRate.Where(m => m.Type == Enumeration.CommissionRateType.InsuranceCompany).FirstOrDefault();


            //    var merchant_Commission_Commercial = orderToCarInsure.CommercialPrice * (merchant_CommissionRate.Commercial / 100);
            //    var merchant_Commission_Compulsory = orderToCarInsure.CompulsoryPrice * (merchant_CommissionRate.Compulsory / 100);
            //    var merchant_Commission = PayProvider.GetPayDecimal(merchant_Commission_Commercial + merchant_Commission_Compulsory);

            //    orderToCarInsure.MerchantCommercialCom = merchant_Commission_Commercial;
            //    orderToCarInsure.MerchantCompulsoryCom = merchant_Commission_Compulsory;
            //    orderToCarInsure.MerchantCommission = merchant_Commission;


            //    var yiBanShi_Commission_Commercial = orderToCarInsure.CommercialPrice * (yiBanShi_CommissionRate.Commercial / 100);
            //    var yiBanShi_Commission_Compulsory = orderToCarInsure.CompulsoryPrice * (yiBanShi_CommissionRate.Compulsory / 100);
            //    var yiBanShi_Commission = PayProvider.GetPayDecimal(yiBanShi_Commission_Commercial + yiBanShi_Commission_Compulsory);

            //    orderToCarInsure.YiBanShiCommercialCom = yiBanShi_Commission_Commercial;
            //    orderToCarInsure.YiBanShiCompulsoryCom = yiBanShi_Commission_Compulsory;
            //    orderToCarInsure.YiBanShiCommission = yiBanShi_Commission;

            //    orderToCarInsure.UplinkCommercialCom = uplink_Commission_Commercial;
            //    orderToCarInsure.UplinkCompulsoryCom = uplink_Commission_Compulsory;
            //    orderToCarInsure.UplinkCommission = uplink_Commission;

            //    CurrentDb.SaveChanges();
            //}
            #endregion


            return result;
        }
    }
}
