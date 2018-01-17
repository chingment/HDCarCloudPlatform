using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace Lumos.BLL
{
    public class ApplyResult
    {
        public string bankAccount { get; set; }

        public decimal amount { get; set; }

        public decimal fee { get; set; }

        public string remark { get; set; }
    }

    public class WithdrawProvider : BaseProvider
    {
        public string GetWithrawRuleUrl()
        {
            string linkUrl = string.Format("{0}/App/Help/WithdrawRule", BizFactory.AppSettings.WebAppServerUrl);

            return linkUrl;
        }

        public CustomJsonResult Apply(int operater, int userId, bool confirm, int bankCardId, decimal withdrawAmount)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var bankCard = CurrentDb.BankCard.Where(m => m.Id == bankCardId).FirstOrDefault();

                if (string.IsNullOrEmpty(bankCard.BankAccountNo))
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "您的银行卡正在配置中，如有疑问，请联系客服人员");
                }

                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == userId).FirstOrDefault();

                CalculateWithdrawFee calculateWithdrawFee = new CalculateWithdrawFee(withdrawAmount);

                if (calculateWithdrawFee.Fee > 0)
                {
                    if (!confirm)
                    {
                        return new CustomJsonResult(ResultType.Success, ResultCode.WithdrawConfirm, "您的提现手续费为" + calculateWithdrawFee.Fee + ",确定要提现？");
                    }
                }
                else
                {
                    if (!confirm)
                    {
                        return new CustomJsonResult(ResultType.Success, ResultCode.WithdrawConfirm, "确定要提现？");
                    }
                }

                var withdraw = new Withdraw();
                withdraw.MerchantId = clientUser.MerchantId;
                withdraw.UserId = clientUser.Id;
                withdraw.Amount = calculateWithdrawFee.Amount;
                withdraw.AmountByAfterFee = calculateWithdrawFee.AmountByAfterFee;
                withdraw.Fee = calculateWithdrawFee.Fee;
                withdraw.FeeRateRule = calculateWithdrawFee.FeeRateRule;
                withdraw.BankCardId = bankCardId;
                withdraw.SettlementStartTime = this.DateTime;
                withdraw.Creator = operater;
                withdraw.CreateTime = this.DateTime;
                withdraw.ExpectArriveTime = this.DateTime.AddDays(1);
                withdraw.Status = Enumeration.WithdrawStatus.SendRequest;
                CurrentDb.Withdraw.Add(withdraw);
                CurrentDb.SaveChanges();
                withdraw.Sn = Sn.Build(SnType.Withdraw, withdraw.Id);
                CurrentDb.SaveChanges();

                //商户流水
                var clienfund = CurrentDb.Fund.Where(m => m.UserId == clientUser.Id).FirstOrDefault();
                if (clienfund.Balance < withdraw.Amount)
                {
                    return new CustomJsonResult(ResultType.Failure, "余额不够");
                }

                clienfund.Balance -= withdraw.Amount;//可用金额

                var clientTrans = new Transactions();
                clientTrans.UserId = clientUser.Id;
                clientTrans.Type = Enumeration.TransactionsType.Withdraw;
                clientTrans.ChangeAmount = -withdraw.Amount;
                clientTrans.Balance = clienfund.Balance;
                clientTrans.Creator = operater;
                clientTrans.CreateTime = this.DateTime;
                CurrentDb.Transactions.Add(clientTrans);
                CurrentDb.SaveChanges();
                clientTrans.Sn = Sn.Build(SnType.Transactions, clientTrans.Id);
                clientTrans.Description = string.Format("您在{0}发起申请一笔提现，金额：{1}元", this.DateTime.ToUnifiedFormatDateTime(), withdraw.Amount);
                CurrentDb.SaveChanges();


                //手续费流水
                if (calculateWithdrawFee.Fee > 0)
                {
                    var withdrawFeeFund = CurrentDb.Fund.Where(m => m.UserId == (int)Enumeration.UserAccount.WithdrawFee).FirstOrDefault();
                    withdrawFeeFund.Balance += withdraw.Fee;
                    withdrawFeeFund.Mender = operater;
                    withdrawFeeFund.LastUpdateTime = this.DateTime;

                    var withdrawFeeTrans = new Transactions();
                    withdrawFeeTrans.UserId = withdrawFeeFund.UserId;
                    withdrawFeeTrans.ChangeAmount = withdraw.Fee;
                    withdrawFeeTrans.Balance = withdrawFeeFund.Balance;
                    withdrawFeeTrans.Type = Enumeration.TransactionsType.ChargeFee;
                    withdrawFeeTrans.Description = string.Format("在{0}来自商户({1})的提现手续费，金额：{2}元", this.DateTime.ToUnifiedFormatDateTime(), clientUser.ClientCode, withdraw.Fee);
                    withdrawFeeTrans.Creator = operater;
                    withdrawFeeTrans.CreateTime = this.DateTime;
                    CurrentDb.Transactions.Add(withdrawFeeTrans);
                    CurrentDb.SaveChanges();
                    withdrawFeeTrans.Sn = Sn.Build(SnType.Transactions, withdrawFeeTrans.Id);
                    CurrentDb.SaveChanges();
                }

                //资金池流水
                var withdrawFundPoolFund = CurrentDb.Fund.Where(m => m.UserId == (int)Enumeration.UserAccount.WithdrawFundPool).FirstOrDefault();
                withdrawFundPoolFund.Balance += calculateWithdrawFee.AmountByAfterFee;
                withdrawFundPoolFund.Mender = operater;
                withdrawFundPoolFund.LastUpdateTime = this.DateTime;


                var withdrawFundPoolTrans = new Transactions();
                withdrawFundPoolTrans.UserId = withdrawFundPoolFund.UserId;
                withdrawFundPoolTrans.ChangeAmount = calculateWithdrawFee.AmountByAfterFee;
                withdrawFundPoolTrans.Balance = withdrawFundPoolFund.Balance;
                withdrawFundPoolTrans.Type = Enumeration.TransactionsType.TurnsInWithdrawFund;
                withdrawFundPoolTrans.Description = string.Format("在{0}来自商户({1})的实际提现金额，金额：{2}元", this.DateTime.ToUnifiedFormatDateTime(), clientUser.ClientCode, withdraw.AmountByAfterFee);
                withdrawFundPoolTrans.Creator = operater;
                withdrawFundPoolTrans.CreateTime = this.DateTime;
                CurrentDb.Transactions.Add(withdrawFundPoolTrans);
                CurrentDb.SaveChanges();
                withdrawFundPoolTrans.Sn = Sn.Build(SnType.Transactions, withdrawFundPoolTrans.Id);
                CurrentDb.SaveChanges();

                CurrentDb.SaveChanges();
                ts.Complete();


                ApplyResult applyResult = new ApplyResult();
                applyResult.bankAccount = string.Format("{0} 尾号{1}", bankCard.BankName, CommonUtils.GetBankAccountTail(bankCard.BankAccountNo));
                applyResult.amount = calculateWithdrawFee.Amount;
                applyResult.fee = calculateWithdrawFee.Fee;
                applyResult.remark = "预计1个工作日到账";


                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "提现申请成功", applyResult);
            }

            return result;
        }

        public CustomJsonResult BuildCutOffData(int operater, DateTime startTime, DateTime endTime)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {

                var withdrawList = CurrentDb.Withdraw.Where(m => m.Status == Enumeration.WithdrawStatus.SendRequest && m.UserId > 0 & m.SettlementStartTime >= startTime && m.SettlementStartTime <= endTime).ToList();

                if (withdrawList == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "没有可截单的数据");
                }

                if (withdrawList.Count == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, "没有可截单的数据");
                }

                var sumAmount = (from s in withdrawList select s.Amount).Sum();
                var sumAmountByAfterFee = (from s in withdrawList select s.AmountByAfterFee).Sum();

                DateTime dateNow = DateTime.Now;
                DateTime? d1 = CommonUtils.ConverToStartTime(dateNow.ToString("yyyy-MM-dd"));
                DateTime? d2 = CommonUtils.ConverToEndTime(dateNow.ToString("yyyy-MM-dd"));

                var cutOffCountByDay = (from s in CurrentDb.WithdrawCutOff where s.CreateTime >= d1 && s.CreateTime <= d2 select s.Id).Distinct().Count();

                WithdrawCutOff withdrawCutOff = new WithdrawCutOff();
                withdrawCutOff.BatchNo = dateNow.ToString("yyyy-MM-dd") + "-" + (cutOffCountByDay + 1).ToString("d4");
                withdrawCutOff.CreateTime = dateNow;
                withdrawCutOff.Creator = operater;
                withdrawCutOff.Amount = sumAmount;
                withdrawCutOff.AmountByAfterFee = sumAmountByAfterFee;
                withdrawCutOff.CutOffTime = endTime;
                CurrentDb.WithdrawCutOff.Add(withdrawCutOff);
                CurrentDb.SaveChanges();



                foreach (var m in withdrawList)
                {

                    var withdrawAccount = CurrentDb.BankCard.Where(q => q.Id == m.BankCardId).FirstOrDefault();


                    WithdrawCutOffDetails cutOffDetail = new WithdrawCutOffDetails();
                    cutOffDetail.UserId = m.UserId;
                    cutOffDetail.WithdrawCutOffId = withdrawCutOff.Id;
                    cutOffDetail.MerchantId = m.MerchantId;
                    cutOffDetail.WithdrawBankCardId = m.BankCardId;
                    cutOffDetail.WithdrawId = m.Id;
                    cutOffDetail.WithdrawBankAccountName = withdrawAccount.BankAccountName;
                    cutOffDetail.WithdrawBankAccountNo = withdrawAccount.BankAccountNo;
                    cutOffDetail.WithdrawBankName = withdrawAccount.BankName;
                    cutOffDetail.WithdrawSn = m.Sn;
                    cutOffDetail.WithdrawAmount = m.Amount;
                    cutOffDetail.WithdrawFeeRateRule = m.FeeRateRule;
                    cutOffDetail.WithdrawFee = m.Fee;
                    cutOffDetail.WithdrawAmountByAfterFee = m.AmountByAfterFee;
                    cutOffDetail.WithdrawStartTime = m.SettlementStartTime;
                    cutOffDetail.WithdrawExpectArriveTime = m.ExpectArriveTime;
                    cutOffDetail.WithdrawSettlementStartTime = dateNow;
                    cutOffDetail.WithdrawStatus = Enumeration.WithdrawStatus.Handing;
                    cutOffDetail.WithdrawCutoffTime = this.DateTime;
                    cutOffDetail.CreateTime = dateNow;
                    cutOffDetail.Creator = operater;
                    cutOffDetail.WithdrawCutoffTime = this.DateTime;

                    CurrentDb.WithdrawCutOffDetails.Add(cutOffDetail);

                    m.WithdrawCutoffTime = this.DateTime;
                    m.WithdrawCutoffId = withdrawCutOff.Id;
                    m.Status = Enumeration.WithdrawStatus.Handing;
                    m.LastUpdateTime = dateNow;
                    m.Mender = operater;

                }

                CurrentDb.SaveChanges();

                ts.Complete();


                result = new CustomJsonResult(ResultType.Success, "截单成功");
            }

            return result;
        }

        public CustomJsonResult Feedback(int operater, int cutoffid, List<WithdrawCutOffDetails> updateCutOffDetails)
        {
            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                int handingCount = 0;
                int successCount = 0;
                int failureCount = 0;
                foreach (var m in updateCutOffDetails)
                {

                    DateTime dateNow = DateTime.Now;
                    var cutOffDetail = CurrentDb.WithdrawCutOffDetails.Where(q => q.WithdrawSn == m.WithdrawSn).FirstOrDefault();
                    if (cutOffDetail != null)
                    {
                        var withdraw = CurrentDb.Withdraw.Where(q => q.Id == cutOffDetail.WithdrawId && q.Status == Enumeration.WithdrawStatus.Handing).FirstOrDefault();
                        if (withdraw != null)
                        {
                            if (m.WithdrawStatus == Enumeration.WithdrawStatus.Handing)
                            {
                                handingCount++;

                                #region 处理中
                                cutOffDetail.WithdrawStatus = Enumeration.WithdrawStatus.Handing;
                                cutOffDetail.LastUpdateTime = dateNow;
                                cutOffDetail.Mender = operater;

                                withdraw.Status = Enumeration.WithdrawStatus.Handing;
                                withdraw.LastUpdateTime = dateNow;
                                withdraw.Mender = operater;
                                #endregion

                            }
                            else if (m.WithdrawStatus == Enumeration.WithdrawStatus.Success)
                            {
                                successCount++;

                                #region 成功

                                var client = CurrentDb.SysClientUser.Find(withdraw.UserId);

                                cutOffDetail.WithdrawSettlementEndTime = dateNow;
                                cutOffDetail.WithdrawStatus = Enumeration.WithdrawStatus.Success;
                                cutOffDetail.LastUpdateTime = dateNow;
                                cutOffDetail.Mender = operater;

                                withdraw.SettlementEndTime = dateNow;
                                withdraw.Status = Enumeration.WithdrawStatus.Success;
                                withdraw.LastUpdateTime = dateNow;
                                withdraw.Mender = operater;

                                var withdrawFundPoolFund = CurrentDb.Fund.Where(c => c.UserId == (int)Enumeration.UserAccount.WithdrawFundPool).FirstOrDefault();
                                withdrawFundPoolFund.Balance -= withdraw.AmountByAfterFee;
                                withdrawFundPoolFund.Mender = operater;
                                withdrawFundPoolFund.LastUpdateTime = dateNow;


                                var withdrawFundPoolTrans = new Transactions();
                                withdrawFundPoolTrans.UserId = withdrawFundPoolFund.UserId;
                                withdrawFundPoolTrans.Type = Enumeration.TransactionsType.TurnsOutWithdrawFund;
                                withdrawFundPoolTrans.ChangeAmount = -withdraw.AmountByAfterFee;
                                withdrawFundPoolTrans.Balance = withdrawFundPoolFund.Balance;
                                withdrawFundPoolTrans.Description = string.Format("提现到帐成功，转出提现资金给商户({0})，金额：{1}", client.ClientCode, withdraw.AmountByAfterFee);
                                withdrawFundPoolTrans.Creator = operater;
                                withdrawFundPoolTrans.CreateTime = dateNow;
                                CurrentDb.Transactions.Add(withdrawFundPoolTrans);
                                CurrentDb.SaveChanges();
                                withdrawFundPoolTrans.Sn = Sn.Build(SnType.Transactions, withdrawFundPoolTrans.Id);
                                CurrentDb.SaveChanges();



                                var uplinkFund = CurrentDb.Fund.Where(c => c.UserId == (int)Enumeration.UserAccount.Uplink).FirstOrDefault();
                                uplinkFund.Balance -= withdraw.AmountByAfterFee;
                                uplinkFund.Mender = operater;
                                uplinkFund.LastUpdateTime = dateNow;


                                var uplinkFundTrans = new Transactions();
                                uplinkFundTrans.UserId = uplinkFund.UserId;
                                uplinkFundTrans.Type = Enumeration.TransactionsType.Advance;
                                uplinkFundTrans.ChangeAmount = -withdraw.AmountByAfterFee;
                                uplinkFundTrans.Balance = uplinkFund.Balance;
                                uplinkFundTrans.Description = string.Format("商户({0})提现到帐成功，全线通垫付资金，金额：{1}", client.ClientCode, withdraw.AmountByAfterFee);
                                uplinkFundTrans.Creator = operater;
                                uplinkFundTrans.CreateTime = dateNow;
                                CurrentDb.Transactions.Add(uplinkFundTrans);
                                CurrentDb.SaveChanges();
                                uplinkFundTrans.Sn = Sn.Build(SnType.Transactions, uplinkFundTrans.Id);
                                CurrentDb.SaveChanges();



                                #endregion
                            }
                            else if (m.WithdrawStatus == Enumeration.WithdrawStatus.Failure)
                            {
                                failureCount++;

                                #region 失败

                                cutOffDetail.WithdrawSettlementEndTime = dateNow;
                                cutOffDetail.WithdrawStatus = Enumeration.WithdrawStatus.Failure;
                                cutOffDetail.WithdrawFailureReason = m.WithdrawFailureReason;
                                cutOffDetail.LastUpdateTime = dateNow;
                                cutOffDetail.Mender = operater;

                                withdraw.SettlementEndTime = dateNow;
                                withdraw.Status = Enumeration.WithdrawStatus.Failure;
                                withdraw.FailureReason = m.WithdrawFailureReason;
                                withdraw.LastUpdateTime = dateNow;
                                withdraw.Mender = operater;

                                var client = CurrentDb.SysClientUser.Find(withdraw.UserId);

                                var clientStoreFund = CurrentDb.Fund.Where(c => c.UserId == withdraw.UserId).FirstOrDefault();


                                #region 客户虚拟帐户中扣除对应的金额

                                clientStoreFund.Balance += withdraw.Amount;//可用金额

                                CurrentDb.SaveChanges();

                                var clientTrans = new Transactions();
                                clientTrans.UserId = client.Id;
                                clientTrans.Type = Enumeration.TransactionsType.WithdrawRefund;
                                clientTrans.ChangeAmount = withdraw.Amount;
                                clientTrans.Balance = clientStoreFund.Balance;
                                clientTrans.Creator = operater;
                                clientTrans.CreateTime = dateNow;
                                CurrentDb.Transactions.Add(clientTrans);
                                CurrentDb.SaveChanges();
                                clientTrans.Sn = Sn.Build(SnType.Transactions, clientTrans.Id);
                                string description = "";
                                clientTrans.Description = description;
                                CurrentDb.SaveChanges();


                                #endregion


                                #region 平台虚拟账户中更改对应的金额


                                if (withdraw.Fee > 0)
                                {
                                    var withdrawFeeStoreFund = CurrentDb.Fund.Where(c => c.UserId == (int)Enumeration.UserAccount.WithdrawFee).FirstOrDefault();
                                    withdrawFeeStoreFund.Balance -= withdraw.Fee;
                                    withdrawFeeStoreFund.Mender = operater;
                                    withdrawFeeStoreFund.LastUpdateTime = dateNow;


                                    var withdrawFeeTrans = new Transactions();
                                    withdrawFeeTrans.UserId = withdrawFeeStoreFund.UserId;
                                    withdrawFeeTrans.Type = Enumeration.TransactionsType.ReturnFee;
                                    withdrawFeeTrans.ChangeAmount = -withdraw.Fee;
                                    withdrawFeeTrans.Balance = withdrawFeeStoreFund.Balance;
                                    withdrawFeeTrans.Description = "";
                                    withdrawFeeTrans.Creator = operater;
                                    withdrawFeeTrans.CreateTime = dateNow;
                                    CurrentDb.Transactions.Add(withdrawFeeTrans);
                                    CurrentDb.SaveChanges();
                                    withdrawFeeTrans.Sn = Sn.Build(SnType.Transactions, withdrawFeeTrans.Id);
                                    CurrentDb.SaveChanges();
                                }


                                var withdrawFundPoolStoreFund = CurrentDb.Fund.Where(c => c.UserId == (int)Enumeration.UserAccount.WithdrawFundPool).FirstOrDefault();
                                withdrawFundPoolStoreFund.Balance -= withdraw.AmountByAfterFee;
                                withdrawFundPoolStoreFund.Mender = operater;
                                withdrawFundPoolStoreFund.LastUpdateTime = dateNow;


                                var withdrawFundPoolTrans = new Transactions();
                                withdrawFundPoolTrans.UserId = withdrawFundPoolStoreFund.UserId;
                                withdrawFundPoolTrans.Type = Enumeration.TransactionsType.ReturnWithdrawFund;
                                withdrawFundPoolTrans.ChangeAmount = -withdraw.AmountByAfterFee;
                                withdrawFundPoolTrans.Balance = withdrawFundPoolStoreFund.Balance;
                                withdrawFundPoolTrans.Description = "";
                                withdrawFundPoolTrans.Creator = operater;
                                withdrawFundPoolTrans.CreateTime = dateNow;
                                CurrentDb.Transactions.Add(withdrawFundPoolTrans);
                                CurrentDb.SaveChanges();
                                withdrawFundPoolTrans.Sn = Sn.Build(SnType.Transactions, withdrawFundPoolTrans.Id);
                                CurrentDb.SaveChanges();
                                #endregion

                                #endregion
                            }
                        }
                    }
                }

                //操作历史
                // SysFactory.SysOperateHistory.Add(Enumeration.OperateType.WithdrawFeedback, operater, cutoffid, string.Format("提现反馈:处理中({0}),成功({1}),失败:({2})", handingCount, successCount, failureCount));

                CurrentDb.SaveChanges();

                ts.Complete();


                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "截单反馈成功");
            }

            return result;

        }
    }
}
