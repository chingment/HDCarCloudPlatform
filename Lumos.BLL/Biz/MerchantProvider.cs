using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class MerchantProvider : BaseProvider
    {
        public CustomJsonResult CreateAccount(int operater, string token, string validCode, string userName, string password, string deviceId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                //var code = CurrentDb.SysSmsSendHistory.Where(m => m.Token == token && m.ValidCode == validCode && m.IsUse == false && m.ExpireTime >= DateTime.Now).FirstOrDefault();
                //if (code == null)
                //{
                //    return new CustomJsonResult(ResultType.Failure, "验证码错误");
                //}

                //code.IsUse = true;

                var isExists = CurrentDb.Users.Where(m => m.UserName == userName).FirstOrDefault();

                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, "账号已经存在");
                }



                var sysClientUser = new SysClientUser();
                sysClientUser.UserName = userName;
                sysClientUser.PasswordHash = PassWordHelper.HashPassword(password);
                sysClientUser.SecurityStamp = Guid.NewGuid().ToString();
                sysClientUser.RegisterTime = this.DateTime;
                sysClientUser.CreateTime = this.DateTime;
                sysClientUser.Creator = operater;
                sysClientUser.ClientAccountType = Enumeration.ClientAccountType.MasterAccount;
                sysClientUser.Status = Enumeration.UserStatus.Normal;

                CurrentDb.SysClientUser.Add(sysClientUser);
                CurrentDb.SaveChanges();

                var clientCode = CurrentDb.SysClientCode.Where(m => m.Id == sysClientUser.Id).FirstOrDefault();

                var merchant = new Merchant();
                merchant.ClientCode = clientCode.Code;
                merchant.UserId = sysClientUser.Id;
                merchant.CreateTime = this.DateTime;
                merchant.Creator = operater;
                merchant.Status = Enumeration.MerchantStatus.WaitFill;
                CurrentDb.Merchant.Add(merchant);
                CurrentDb.SaveChanges();

                sysClientUser.ClientCode = clientCode.Code;
                sysClientUser.MerchantId = merchant.Id;

                var posMachine = CurrentDb.PosMachine.Where(m => m.DeviceId == deviceId).FirstOrDefault();
                if (posMachine != null)
                {
                    if (posMachine.IsUse)
                    {
                        return new CustomJsonResult(ResultType.Failure, "当前POS已被注册");
                    }

                    posMachine.IsUse = false;
                    posMachine.Mender = operater;
                    posMachine.LastUpdateTime = this.DateTime;
                    CurrentDb.SaveChanges();
                }
                else
                {
                    posMachine = new PosMachine();
                    posMachine.DeviceId = deviceId;
                    posMachine.Creator = operater;
                    posMachine.CreateTime = this.DateTime;
                    CurrentDb.PosMachine.Add(posMachine);
                    CurrentDb.SaveChanges();
                }


                var bankCard = new BankCard();
                bankCard.MerchantId = merchant.Id;
                bankCard.UserId = merchant.UserId;
                bankCard.CreateTime = this.DateTime;
                bankCard.Creator = operater;
                CurrentDb.BankCard.Add(bankCard);
                CurrentDb.SaveChanges();

                CalculateServiceFee calculateServiceFee = new CalculateServiceFee();

                var merchantPosMachine = new MerchantPosMachine();
                merchantPosMachine.BankCardId = bankCard.Id;
                merchantPosMachine.UserId = sysClientUser.Id;
                merchantPosMachine.MerchantId = merchant.Id;
                merchantPosMachine.PosMachineId = posMachine.Id;
                merchantPosMachine.Deposit = calculateServiceFee.Deposit;
                merchantPosMachine.MobileTrafficFee = calculateServiceFee.MobileTrafficFee;
                merchantPosMachine.Status = Enumeration.MerchantPosMachineStatus.NoActive;
                merchantPosMachine.CreateTime = this.DateTime;
                merchantPosMachine.Creator = operater;
                CurrentDb.MerchantPosMachine.Add(merchantPosMachine);
                CurrentDb.SaveChanges();

                var orderToServiceFee = new OrderToServiceFee();
                orderToServiceFee.MerchantId = merchant.Id;
                orderToServiceFee.PosMachineId = posMachine.Id;
                orderToServiceFee.UserId = sysClientUser.Id;
                orderToServiceFee.SubmitTime = this.DateTime;
                orderToServiceFee.ProductType = Enumeration.ProductType.PosMachineServiceFee;
                orderToServiceFee.ProductName = Enumeration.ProductType.PosMachineServiceFee.GetCnName();
                orderToServiceFee.ProductId = (int)Enumeration.ProductType.PosMachineServiceFee;
                orderToServiceFee.Deposit = calculateServiceFee.Deposit;
                orderToServiceFee.MobileTrafficFee = calculateServiceFee.MobileTrafficFee;
                orderToServiceFee.PriceVersion = calculateServiceFee.Version;
                orderToServiceFee.Price = calculateServiceFee.Deposit + calculateServiceFee.MobileTrafficFee;
                orderToServiceFee.Status = Enumeration.OrderStatus.WaitPay;
                orderToServiceFee.CreateTime = this.DateTime;
                orderToServiceFee.Creator = operater;
                CurrentDb.OrderToServiceFee.Add(orderToServiceFee);
                CurrentDb.SaveChanges();

                SnModel snModel = Sn.Build(SnType.ServiceFee, orderToServiceFee.Id);
                orderToServiceFee.Sn = snModel.Sn;
                orderToServiceFee.TradeSnByWechat = snModel.TradeSnByWechat;
                orderToServiceFee.TradeSnByAlipay = snModel.TradeSnByAlipay;


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "注册成功");
            }

            return result;

        }

        public CustomJsonResult Edit(int operater, Merchant merchant, int[] estimateInsuranceCompanyIds, List<MerchantPosMachine> merchantPosMachine, List<BankCard> bankCard)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var l_Merchant = CurrentDb.Merchant.Where(m => m.Id == merchant.Id).FirstOrDefault();
                if (l_Merchant == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "不存在该商户");
                }

                l_Merchant.SalesmanId = merchant.SalesmanId;
                l_Merchant.Type = merchant.Type;
                if (l_Merchant.Type == Enumeration.MerchantType.CarRepair)
                {
                    l_Merchant.RepairCapacity = merchant.RepairCapacity;
                }
                else
                {
                    l_Merchant.RepairCapacity = Enumeration.RepairCapacity.NoRepair;
                }

                l_Merchant.ContactName = merchant.ContactName;
                l_Merchant.ContactPhoneNumber = merchant.ContactPhoneNumber;
                l_Merchant.ContactAddress = merchant.ContactAddress;
                l_Merchant.YYZZ_Name = merchant.YYZZ_Name;
                l_Merchant.YYZZ_RegisterNo = merchant.YYZZ_RegisterNo;
                l_Merchant.YYZZ_Type = merchant.YYZZ_Type;
                l_Merchant.YYZZ_Address = merchant.YYZZ_Address;
                l_Merchant.YYZZ_OperatingPeriodStart = merchant.YYZZ_OperatingPeriodStart;
                l_Merchant.YYZZ_OperatingPeriodEnd = merchant.YYZZ_OperatingPeriodEnd;
                l_Merchant.FR_Name = merchant.FR_Name;
                l_Merchant.FR_IdCardNo = merchant.FR_IdCardNo;
                l_Merchant.FR_Birthdate = merchant.FR_Birthdate;
                l_Merchant.FR_Address = merchant.FR_Address;
                l_Merchant.FR_IssuingAuthority = merchant.FR_IssuingAuthority;
                l_Merchant.FR_ValidPeriodStart = merchant.FR_ValidPeriodStart;
                l_Merchant.FR_ValidPeriodEnd = merchant.FR_ValidPeriodEnd;

                if (string.IsNullOrEmpty(merchant.Area))
                {
                    l_Merchant.Area = null;
                    l_Merchant.AreaCode = null;
                }
                else
                {
                    l_Merchant.Area = merchant.Area;
                    l_Merchant.AreaCode = merchant.AreaCode;
                }


                l_Merchant.LastUpdateTime = this.DateTime;
                l_Merchant.Mender = operater;


                var removeMerchantEstimateCompany = CurrentDb.MerchantEstimateCompany.Where(m => m.MerchantId == l_Merchant.Id).ToList();
                foreach (var m in removeMerchantEstimateCompany)
                {
                    CurrentDb.MerchantEstimateCompany.Remove(m);
                }

                if (estimateInsuranceCompanyIds != null)
                {
                    foreach (var m in estimateInsuranceCompanyIds)
                    {
                        MerchantEstimateCompany merchantEstimateCompany = new MerchantEstimateCompany();
                        merchantEstimateCompany.InsuranceCompanyId = m;
                        merchantEstimateCompany.MerchantId = l_Merchant.Id;
                        CurrentDb.MerchantEstimateCompany.Add(merchantEstimateCompany);
                    }
                }

                //foreach (var m in merchantPosMachine)
                //{
                //    var l_MerchantPosMachine = CurrentDb.MerchantPosMachine.Where(q => q.Id == q.Id && q.MerchantId == l_Merchant.Id).FirstOrDefault();
                //    if (l_MerchantPosMachine != null)
                //    {
                //        l_MerchantPosMachine.Deposit = m.Deposit;
                //        l_MerchantPosMachine.Rent = m.Rent;
                //        l_MerchantPosMachine.Mender = operater;
                //        l_MerchantPosMachine.LastUpdateTime = this.DateTime;
                //        CurrentDb.SaveChanges();
                //    }
                //}


                foreach (var m in bankCard)
                {
                    var l_BankCard = CurrentDb.BankCard.Where(q => q.Id == q.Id && q.MerchantId == l_Merchant.Id).FirstOrDefault();
                    if (l_BankCard != null)
                    {
                        l_BankCard.BankAccountName = m.BankAccountName;
                        l_BankCard.BankAccountNo = m.BankAccountNo;
                        l_BankCard.BankName = m.BankName;
                        l_BankCard.Mender = operater;
                        l_BankCard.LastUpdateTime = this.DateTime;
                        CurrentDb.SaveChanges();
                    }
                }


                var user = CurrentDb.SysClientUser.Where(m => m.Id == l_Merchant.UserId).FirstOrDefault();
                user.PhoneNumber = merchant.ContactPhoneNumber;

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "保存成功");
            }

            return result;

        }

        public CustomJsonResult PrimaryAudit(int operater, Enumeration.OperateType operate, Merchant merchant, int[] estimateInsuranceCompanyIds, List<MerchantPosMachine> merchantPosMachine, List<BankCard> bankCard, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var l_Merchant = CurrentDb.Merchant.Where(m => m.Id == merchant.Id).FirstOrDefault();
                if (l_Merchant == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "不存在该商户");
                }

                l_Merchant.SalesmanId = merchant.SalesmanId;
                l_Merchant.Type = merchant.Type;
                if (l_Merchant.Type == Enumeration.MerchantType.CarRepair)
                {
                    l_Merchant.RepairCapacity = merchant.RepairCapacity;
                }
                else
                {
                    l_Merchant.RepairCapacity = Enumeration.RepairCapacity.NoRepair;
                }

                l_Merchant.ContactName = merchant.ContactName;
                l_Merchant.ContactPhoneNumber = merchant.ContactPhoneNumber;
                l_Merchant.ContactAddress = merchant.ContactAddress;
                l_Merchant.YYZZ_Name = merchant.YYZZ_Name;
                l_Merchant.YYZZ_RegisterNo = merchant.YYZZ_RegisterNo;
                l_Merchant.YYZZ_Type = merchant.YYZZ_Type;
                l_Merchant.YYZZ_Address = merchant.YYZZ_Address;
                l_Merchant.YYZZ_OperatingPeriodStart = merchant.YYZZ_OperatingPeriodStart;
                l_Merchant.YYZZ_OperatingPeriodEnd = merchant.YYZZ_OperatingPeriodEnd;

                l_Merchant.FR_Name = merchant.FR_Name;
                l_Merchant.FR_IdCardNo = merchant.FR_IdCardNo;
                l_Merchant.FR_Birthdate = merchant.FR_Birthdate;
                l_Merchant.FR_Address = merchant.FR_Address;
                l_Merchant.FR_IssuingAuthority = merchant.FR_IssuingAuthority;
                l_Merchant.FR_ValidPeriodStart = merchant.FR_ValidPeriodEnd;
                l_Merchant.FR_ValidPeriodEnd = merchant.FR_ValidPeriodEnd;

                if (string.IsNullOrEmpty(merchant.AreaCode))
                {
                    l_Merchant.Area = null;
                    l_Merchant.AreaCode = null;
                }
                else
                {
                    l_Merchant.Area = merchant.Area;
                    l_Merchant.AreaCode = merchant.AreaCode;
                }

                l_Merchant.LastUpdateTime = this.DateTime;
                l_Merchant.Mender = operater;



                var removeMerchantEstimateCompany = CurrentDb.MerchantEstimateCompany.Where(m => m.MerchantId == l_Merchant.Id).ToList();
                foreach (var m in removeMerchantEstimateCompany)
                {
                    CurrentDb.MerchantEstimateCompany.Remove(m);
                }

                if (estimateInsuranceCompanyIds != null)
                {
                    foreach (var m in estimateInsuranceCompanyIds)
                    {
                        MerchantEstimateCompany merchantEstimateCompany = new MerchantEstimateCompany();
                        merchantEstimateCompany.InsuranceCompanyId = m;
                        merchantEstimateCompany.MerchantId = l_Merchant.Id;
                        CurrentDb.MerchantEstimateCompany.Add(merchantEstimateCompany);
                    }
                }


                //foreach (var m in merchantPosMachine)
                //{
                //    var l_MerchantPosMachine = CurrentDb.MerchantPosMachine.Where(q => q.Id == q.Id && q.MerchantId == l_Merchant.Id).FirstOrDefault();
                //    if (l_MerchantPosMachine != null)
                //    {
                //        l_MerchantPosMachine.Deposit = m.Deposit;
                //        l_MerchantPosMachine.Rent = m.Rent;
                //        l_MerchantPosMachine.Mender = operater;
                //        l_MerchantPosMachine.LastUpdateTime = this.DateTime;
                //        CurrentDb.SaveChanges();
                //    }
                //}


                foreach (var m in bankCard)
                {
                    var l_BankCard = CurrentDb.BankCard.Where(q => q.Id == q.Id && q.MerchantId == l_Merchant.Id).FirstOrDefault();
                    if (l_BankCard != null)
                    {
                        l_BankCard.BankAccountName = m.BankAccountName;
                        l_BankCard.BankAccountNo = m.BankAccountNo;
                        l_BankCard.BankName = m.BankName;
                        l_BankCard.Mender = operater;
                        l_BankCard.LastUpdateTime = this.DateTime;
                        CurrentDb.SaveChanges();
                    }
                }

                switch (operate)
                {
                    case Enumeration.OperateType.Save:

                        BizFactory.BizProcessesAudit.ChangeAuditDetailsAuditComments(operater, bizProcessesAudit.CurrentDetails.Id, bizProcessesAudit.CurrentDetails.AuditComments, null);

                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Submit:

                        BizFactory.BizProcessesAudit.ChangeAuditDetailsAuditComments(operater, bizProcessesAudit.CurrentDetails.Id, bizProcessesAudit.CurrentDetails.AuditComments, null, this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeMerchantAuditStatus(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, Enumeration.MerchantAuditStatus.WaitSeniorAudit);

                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                }


                CurrentDb.SaveChanges();
                ts.Complete();

            }

            return result;

        }

        public CustomJsonResult SeniorAudit(int operater, Enumeration.OperateType operate, int merchantId, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {



                switch (operate)
                {
                    case Enumeration.OperateType.Save:

                        BizFactory.BizProcessesAudit.ChangeAuditDetailsAuditComments(operater, bizProcessesAudit.CurrentDetails.Id, bizProcessesAudit.CurrentDetails.AuditComments, null);

                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Reject:

                        BizFactory.BizProcessesAudit.ChangeAuditDetailsAuditComments(operater, bizProcessesAudit.CurrentDetails.Id, bizProcessesAudit.CurrentDetails.AuditComments, null, this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeMerchantAuditStatus(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, Enumeration.MerchantAuditStatus.SeniorAuditReject, null);

                        result = new CustomJsonResult(ResultType.Success, "驳回成功");
                        break;
                    case Enumeration.OperateType.Submit:

                        var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantId).FirstOrDefault();

                        merchant.Status = Enumeration.MerchantStatus.Filled;

                        var user = CurrentDb.SysClientUser.Where(m => m.Id == merchant.UserId).FirstOrDefault();

                        user.PhoneNumber = merchant.ContactPhoneNumber;

                        CurrentDb.SaveChanges();

                        BizFactory.BizProcessesAudit.ChangeAuditDetailsAuditComments(operater, bizProcessesAudit.CurrentDetails.Id, bizProcessesAudit.CurrentDetails.AuditComments, null, this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeMerchantAuditStatus(operater, bizProcessesAudit.CurrentDetails.BizProcessesAuditId, Enumeration.MerchantAuditStatus.SeniorAuditPass, this.DateTime);

                        result = new CustomJsonResult(ResultType.Success, "归档成功");
                        break;
                }


                CurrentDb.SaveChanges();
                ts.Complete();

            }

            return result;

        }

        public CustomJsonResult ReturnPosMachine(int operater, MerchantPosMachine pMerchantPosMachine)
        {
            CustomJsonResult result = new CustomJsonResult();

            //todo 什么条件允许注销
            var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.Id == pMerchantPosMachine.Id).FirstOrDefault();
            if (merchantPosMachine != null)
                return new CustomJsonResult(ResultType.Failure, "找不到要注销的POS机");

            if (merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.Normal || merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.Expiry)
                return new CustomJsonResult(ResultType.Failure, "该POS的状态不允许注销");


            //posMachine.CreateTime = this.DateTime;
            //posMachine.Creator = operater;
            //posMachine.IsUse = false;
            //CurrentDb.PosMachine.Add(posMachine);
            //CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "登记成功");
        }

        public OrderConfirmInfo GetOrderConfirmInfoByServiceFee(OrderToServiceFee orderToServiceFee)
        {
            OrderConfirmInfo yOrder = new OrderConfirmInfo();


            orderToServiceFee.ExpiryTime = this.DateTime.AddYears(1);

            yOrder.OrderId = orderToServiceFee.Id;
            yOrder.OrderSn = orderToServiceFee.Sn;
            yOrder.remarks = orderToServiceFee.Remarks;
            yOrder.transName = "消费";
            yOrder.productType = orderToServiceFee.ProductType;
            yOrder.productName = orderToServiceFee.ProductName;
            yOrder.amount = orderToServiceFee.Price.ToF2Price().Replace(".", "").PadLeft(12, '0');

            yOrder.confirmField.Add(new OrderField("订单编号", orderToServiceFee.Sn.NullToEmpty()));
            if (orderToServiceFee.Deposit > 0)
            {
                yOrder.confirmField.Add(new OrderField("押金", string.Format("{0}元", orderToServiceFee.Deposit.ToF2Price())));
            }
            yOrder.confirmField.Add(new OrderField("流量费", string.Format("{0}元", orderToServiceFee.MobileTrafficFee.ToF2Price())));
            yOrder.confirmField.Add(new OrderField("到期时间", orderToServiceFee.ExpiryTime.ToUnifiedFormatDate()));
            yOrder.confirmField.Add(new OrderField("支付金额", string.Format("{0}元", orderToServiceFee.Price.NullToEmpty())));


            #region 支持的支付方式
            int[] payMethods = new int[1] { 1 };

            foreach (var payWayId in payMethods)
            {
                var payWay = new PayWay();
                payWay.id = payWayId;
                yOrder.payMethod.Add(payWay);
            }
            #endregion

            return yOrder;
        }

    }
}
