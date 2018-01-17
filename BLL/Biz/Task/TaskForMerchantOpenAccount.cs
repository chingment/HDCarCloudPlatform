using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Biz.Task
{

    public class TaskForMerchantOpenAccount : BaseProvider, ITask
    {
        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();


            int operater = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                var posMachines = CurrentDb.PosMachine.Where(m => m.IsUse == false && m.IsSpare == false&& m.IsAutoBuildAccount==true).ToList();

                Log.InfoFormat("准备生成商户账号{0}个", posMachines.Count());

                for (int i = 0; i < posMachines.Count; i++)
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
                        Log.WarnFormat("生成商户账号{0}个", i + 1);
                        throw new Exception("客户代码已经用完，请马上生成客户代码");
                    }

                    sysClientUser.ClientCode = clientCode.Code;
                    sysClientUser.UserName = clientCode.Code;

                    var merchant = new Merchant();
                    merchant.ClientCode = clientCode.Code;
                    merchant.UserId = sysClientUser.Id;
                    merchant.CreateTime = this.DateTime;
                    merchant.Creator = operater;
                    merchant.Status = Enumeration.MerchantStatus.WaitFill;
                    CurrentDb.Merchant.Add(merchant);
                    CurrentDb.SaveChanges();

                    sysClientUser.MerchantId = merchant.Id;


                    posMachines[i].IsUse = true;
                    posMachines[i].LastUpdateTime = this.DateTime;
                    posMachines[i].Mender = operater;
                    CurrentDb.SaveChanges();

                    var bankCard = new BankCard();
                    bankCard.MerchantId = merchant.Id;
                    bankCard.UserId = merchant.UserId;
                    bankCard.CreateTime = this.DateTime;
                    bankCard.Creator = operater;
                    CurrentDb.BankCard.Add(bankCard);
                    CurrentDb.SaveChanges();

                    var merchantPosMachine = new MerchantPosMachine();
                    merchantPosMachine.PosMachineId = posMachines[i].Id;
                    merchantPosMachine.BankCardId = bankCard.Id;
                    merchantPosMachine.UserId = sysClientUser.Id;
                    merchantPosMachine.MerchantId = merchant.Id;
                    merchantPosMachine.Status = Enumeration.MerchantPosMachineStatus.NoActive;
                    merchantPosMachine.CreateTime = this.DateTime;
                    merchantPosMachine.Creator = operater;
                    merchantPosMachine.Deposit = posMachines[i].Deposit;
                    merchantPosMachine.Rent = posMachines[i].Rent;
                    CurrentDb.MerchantPosMachine.Add(merchantPosMachine);
                    CurrentDb.SaveChanges();

                    var fund = new Fund();
                    fund.UserId = sysClientUser.Id;
                    fund.Arrearage = 0;
                    fund.Balance = 0;
                    fund.CreateTime = this.DateTime;
                    fund.Creator = operater;
                    fund.MerchantId = merchant.Id;
                    CurrentDb.Fund.Add(fund);
                    CurrentDb.SaveChanges();



                    var orderToDepositRent = new OrderToDepositRent();
                    orderToDepositRent.MerchantId = merchant.Id;
                    orderToDepositRent.MerchantPosMachineId = merchantPosMachine.Id;
                    orderToDepositRent.UserId = sysClientUser.Id;
                    orderToDepositRent.CreateTime = this.DateTime;
                    orderToDepositRent.Creator = 0;
                    orderToDepositRent.SubmitTime = this.DateTime;
                    orderToDepositRent.ProductType = Enumeration.ProductType.PosMachineDepositRent;
                    orderToDepositRent.ProductName = Enumeration.ProductType.PosMachineDepositRent.GetCnName();
                    orderToDepositRent.ProductId = (int)Enumeration.ProductType.PosMachineDepositRent;

                    orderToDepositRent.Deposit = posMachines[i].Deposit;

                    CalculateRent calculateRent = new CalculateRent(merchantPosMachine.Rent);

                    orderToDepositRent.RentMonths = 3;
                    orderToDepositRent.MonthlyRent = calculateRent.MonthlyRent;
                    orderToDepositRent.RentVersion = calculateRent.Version;
                    orderToDepositRent.RentTotal = calculateRent.GetRent(orderToDepositRent.RentMonths);

                    orderToDepositRent.Price = posMachines[i].Deposit + orderToDepositRent.RentTotal;

                    orderToDepositRent.Status = Enumeration.OrderStatus.WaitPay;
                    CurrentDb.OrderToDepositRent.Add(orderToDepositRent);
                    CurrentDb.SaveChanges();
                    orderToDepositRent.Sn = Sn.Build(SnType.DepositRent, orderToDepositRent.Id);


                    CurrentDb.SaveChanges();

                    Log.InfoFormat("生成商户账号:{0},对应POS机DeviceId:{1}", clientCode.Code, posMachines[i].DeviceId);
                }

                ts.Complete();

                Log.InfoFormat("生成商户账号{0}个", posMachines.Count());
            }

            return result;
        }
    }
}
