using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
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
        public CustomJsonResult OpenAccount(int operater, Merchant merchant, MerchantPosMachine merchantPosMachine, BankCard bankCard)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysClientUser = new SysClientUser();
                sysClientUser.UserName = Guid.NewGuid().ToString().Replace("-", "");
                sysClientUser.PasswordHash = PassWordHelper.HashPassword("888888");
                sysClientUser.SecurityStamp = Guid.NewGuid().ToString();
                sysClientUser.RegisterTime = this.DateTime;
                sysClientUser.CreateTime = this.DateTime;
                sysClientUser.Creator = operater;
                sysClientUser.ClientAccountType = Enumeration.ClientAccountType.MasterAccount;
                sysClientUser.Status = Enumeration.UserStatus.Normal;

                CurrentDb.SysClientUser.Add(sysClientUser);
                CurrentDb.SaveChanges();

                var clientCode = CurrentDb.SysClientCode.Where(m => m.Id == sysClientUser.Id).FirstOrDefault();
                if (clientCode == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "开户失败，原因获取不到商户代码 ");
                }

                sysClientUser.ClientCode = clientCode.Code;
                sysClientUser.UserName = clientCode.Code;

                merchant.ClientCode = clientCode.Code;
                merchant.UserId = sysClientUser.Id;

                if (string.IsNullOrEmpty(merchant.Area))
                {
                    merchant.Area = null;
                    merchant.AreaCode = null;
                }


                merchant.CreateTime = this.DateTime;
                merchant.Creator = operater;
                merchant.Status = Enumeration.MerchantStatus.WaitFill;
                CurrentDb.Merchant.Add(merchant);
                CurrentDb.SaveChanges();

                sysClientUser.MerchantId = merchant.Id;


                var posMachine = CurrentDb.PosMachine.Where(m => m.Id == merchantPosMachine.PosMachineId).FirstOrDefault();
                if (posMachine == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "开户失败，找不到POS机");
                }

                if (posMachine.IsUse)
                {
                    return new CustomJsonResult(ResultType.Failure, "开户失败，POS机已经被使用");
                }

                posMachine.IsUse = true;
                posMachine.LastUpdateTime = this.DateTime;
                posMachine.Mender = operater;
                CurrentDb.SaveChanges();

                bankCard.MerchantId = merchant.Id;
                bankCard.UserId = merchant.UserId;
                bankCard.CreateTime = this.DateTime;
                bankCard.Creator = operater;
                CurrentDb.BankCard.Add(bankCard);
                CurrentDb.SaveChanges();

                merchantPosMachine.BankCardId = bankCard.Id;
                merchantPosMachine.UserId = sysClientUser.Id;
                merchantPosMachine.MerchantId = merchant.Id;
                merchantPosMachine.Status = Enumeration.MerchantPosMachineStatus.NoActive;
                merchantPosMachine.CreateTime = this.DateTime;
                merchantPosMachine.Creator = operater;
                merchantPosMachine.Deposit = posMachine.Deposit;
                merchantPosMachine.Rent = posMachine.Rent;
                CurrentDb.MerchantPosMachine.Add(merchantPosMachine);


                Fund fund = new Fund();
                fund.UserId = sysClientUser.Id;
                fund.Arrearage = 0;
                fund.Balance = 0;
                fund.CreateTime = this.DateTime;
                fund.Creator = operater;
                fund.MerchantId = merchant.Id;
                CurrentDb.Fund.Add(fund);

                //暂定在这里开启
                BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.MerchantAudit, merchant.Id, Enumeration.MerchantAuditStatus.WaitPrimaryAudit, "");


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "开户成功");
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

        public CustomJsonResult AddChildAccount(int operater, int userId, string fullName, string phoneNumber)
        {

            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var masterAccount = CurrentDb.SysClientUser.Where(m => m.Id == userId).FirstOrDefault();
                var subAccount = new SysClientUser();
                subAccount.UserName = Guid.NewGuid().ToString().Replace("-", "");
                subAccount.PasswordHash = PassWordHelper.HashPassword("888888");
                subAccount.SecurityStamp = Guid.NewGuid().ToString();
                subAccount.RegisterTime = this.DateTime;
                subAccount.CreateTime = this.DateTime;
                subAccount.Creator = operater;
                subAccount.ClientAccountType = Enumeration.ClientAccountType.SubAccount;
                subAccount.MerchantId = masterAccount.MerchantId;
                subAccount.FullName = fullName;
                subAccount.PhoneNumber = phoneNumber;
                subAccount.Status = Enumeration.UserStatus.Normal;

                CurrentDb.SysClientUser.Add(subAccount);
                CurrentDb.SaveChanges();

                var clientCode = CurrentDb.SysClientCode.Where(m => m.Id == subAccount.Id).FirstOrDefault();
                if (clientCode == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "开户失败，原因获取不到商户代码 ");
                }

                subAccount.ClientCode = clientCode.Code;
                subAccount.UserName = clientCode.Code;


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "开户成功");
                result.Data = subAccount;
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

            if (merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.Normal || merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.Rentdue)
                return new CustomJsonResult(ResultType.Failure, "该POS的状态不允许注销");


            //posMachine.CreateTime = this.DateTime;
            //posMachine.Creator = operater;
            //posMachine.IsUse = false;
            //CurrentDb.PosMachine.Add(posMachine);
            //CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "登记成功");
        }

        public OrderDepositRentInfo GetDepositRentOrder(int merchantId, int merchantPosMachineId)
        {
            var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantId).FirstOrDefault();

            var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.MerchantId == merchantId && m.Id == merchantPosMachineId).FirstOrDefault();

            var orderToDepositRent = CurrentDb.OrderToDepositRent.Where(m => m.MerchantId == merchantId && m.MerchantPosMachineId == merchantPosMachineId && m.ProductType == Enumeration.ProductType.PosMachineDepositRent).FirstOrDefault();


            CalculateRent calculateRent = new CalculateRent(merchantPosMachine.Rent);

            if (orderToDepositRent == null)
            {
                orderToDepositRent = new OrderToDepositRent();
                orderToDepositRent.MerchantId = merchantId;
                orderToDepositRent.MerchantPosMachineId = merchantPosMachineId;
                orderToDepositRent.UserId = merchantPosMachine.UserId;
                orderToDepositRent.CreateTime = this.DateTime;
                orderToDepositRent.Creator = 0;
                orderToDepositRent.SubmitTime = this.DateTime;
                orderToDepositRent.ProductType = Enumeration.ProductType.PosMachineDepositRent;
                orderToDepositRent.ProductName = Enumeration.ProductType.PosMachineDepositRent.GetCnName();
                orderToDepositRent.ProductId = (int)Enumeration.ProductType.PosMachineDepositRent;

                orderToDepositRent.Deposit = merchantPosMachine.Deposit;

                orderToDepositRent.RentMonths = 3;
                orderToDepositRent.MonthlyRent = calculateRent.MonthlyRent;
                orderToDepositRent.RentTotal = calculateRent.GetRent(orderToDepositRent.RentMonths);
                orderToDepositRent.RentVersion = calculateRent.Version;

                orderToDepositRent.Price = merchantPosMachine.Deposit + orderToDepositRent.RentTotal;
                orderToDepositRent.Status = Enumeration.OrderStatus.WaitPay;
                CurrentDb.OrderToDepositRent.Add(orderToDepositRent);
                CurrentDb.SaveChanges();
                orderToDepositRent.Sn = Sn.Build(SnType.DepositRent, orderToDepositRent.Id);
                CurrentDb.SaveChanges();
            }

            OrderDepositRentInfo orderDepositRentInfo = new OrderDepositRentInfo();

            orderDepositRentInfo.Id = orderToDepositRent.Id;
            orderDepositRentInfo.Sn = orderToDepositRent.Sn;
            orderDepositRentInfo.Product = orderToDepositRent.ProductName;
            orderDepositRentInfo.ProductType = orderToDepositRent.ProductType;
            orderDepositRentInfo.Status = orderToDepositRent.Status;
            orderDepositRentInfo.StatusName = orderToDepositRent.Status.GetCnName();
            orderDepositRentInfo.Deposit = orderToDepositRent.Deposit;
            orderDepositRentInfo.MonthlyRent = calculateRent.MonthlyRent;
            orderDepositRentInfo.Remarks = calculateRent.Remark;
            orderDepositRentInfo.MerchantCode = merchant.ClientCode;


            return orderDepositRentInfo;
        }

        public OrderDepositRentInfo GetRentOrder(int merchantId, int merchantPosMachineId)
        {
            OrderDepositRentInfo orderDepositRentInfo = new OrderDepositRentInfo();

            var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantId).FirstOrDefault();

            var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.Id == merchantPosMachineId && m.MerchantId == merchantId).FirstOrDefault();

            var orderToRent = CurrentDb.OrderToDepositRent.Where(m => m.MerchantId == merchantId && m.MerchantPosMachineId == merchantPosMachineId && m.ProductType == Enumeration.ProductType.PosMachineRent && m.Status == Enumeration.OrderStatus.WaitPay).FirstOrDefault();

            CalculateRent calculateRent = new CalculateRent(merchantPosMachine.Rent);

            if (orderToRent == null)
            {
                orderToRent = new OrderToDepositRent();
                orderToRent.MerchantId = merchant.Id;
                orderToRent.MerchantPosMachineId = merchantPosMachineId;
                orderToRent.UserId = merchantPosMachine.UserId;
                orderToRent.CreateTime = this.DateTime;
                orderToRent.Creator = 0;
                orderToRent.SubmitTime = this.DateTime;
                orderToRent.ProductType = Enumeration.ProductType.PosMachineRent;
                orderToRent.ProductName = Enumeration.ProductType.PosMachineRent.GetCnName();
                orderToRent.ProductId = (int)Enumeration.ProductType.PosMachineRent;

                orderToRent.Deposit = 0;

                orderToRent.RentMonths = 3;
                orderToRent.MonthlyRent = calculateRent.MonthlyRent;
                orderToRent.RentTotal = calculateRent.GetRent(orderToRent.RentMonths);
                orderToRent.RentVersion = calculateRent.Version;

                orderToRent.Price = orderToRent.RentTotal;
                orderToRent.Status = Enumeration.OrderStatus.WaitPay;
                CurrentDb.OrderToDepositRent.Add(orderToRent);
                CurrentDb.SaveChanges();
                orderToRent.Sn = Sn.Build(SnType.DepositRent, orderToRent.Id);
                CurrentDb.SaveChanges();
            }




            orderDepositRentInfo.Id = orderToRent.Id;
            orderDepositRentInfo.Sn = orderToRent.Sn;
            orderDepositRentInfo.Product = orderToRent.ProductName;
            orderDepositRentInfo.ProductType = orderToRent.ProductType;
            orderDepositRentInfo.Status = orderToRent.Status;
            orderDepositRentInfo.StatusName = orderToRent.Status.GetCnName();
            orderDepositRentInfo.MonthlyRent = calculateRent.MonthlyRent;
            orderDepositRentInfo.Remarks = calculateRent.Remark;
            orderDepositRentInfo.MerchantCode = merchant.ClientCode;
            orderDepositRentInfo.RentDueDate = merchantPosMachine.RentDueDate.ToUnifiedFormatDate();


            return orderDepositRentInfo;
        }

    }
}
