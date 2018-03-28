using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    /// <summary>
    /// 业务的枚举
    /// </summary>
    public partial class Enumeration
    {

        public enum MerchantPosMachineStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Normal = 1,
            [Remark("未激活")]
            NoActive = 2,
            [Remark("到期")]
            Expiry = 3
        }

        public enum ExtendedAppType
        {
            Unknow = 0,
            HaoYiLianService = 1,
            ThirdPartyApp = 2,
            CarInsService = 3
        }

        public enum ExtendedAppAuditStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待初审")]
            WaitAudit = 1,
            [Remark("初审中")]
            InAudit = 2,
            [Remark("等待复审")]
            WaitReview = 3,
            [Remark("复审中")]
            InReview = 4,
            [Remark("复审通过")]
            ReviewPass = 5,
            [Remark("复审驳回")]
            ReviewReject = 6,
            [Remark("复审拒绝")]
            ReviewRefuse = 7
        }

        public enum CarInsureOfferDealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待报价")]
            WaitOffer = 1,
            [Remark("报价中")]
            InOffer = 2,
            [Remark("报价完成")]
            OfferComplete = 3,
            [Remark("客户跟进")]
            ClientFllow = 3,
            [Remark("后台取消订单")]
            StaffCancle = 4,
            [Remark("客户取消订单")]
            ClientCancle = 5,
            [Remark("完成报价")]
            Complete = 6
        }

        public enum CarInsureOfferDealtStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交")]
            Submit = 1,
            [Remark("报价")]
            Offer = 2,
            [Remark("跟进")]
            Fllow = 3,
            [Remark("完成")]
            Complete = 4,
            [Remark("取消")]
            Cancle = 5
        }

        public enum CarClaimDealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待核实")]
            WaitVerifyOrder = 1,
            [Remark("核实需求")]
            InVerifyOrder = 2,
            [Remark("跟进待上传定损单")]
            FllowUploadEstimateListImg = 3,
            [Remark("核实金额")]
            WaitVerifyAmount = 4,
            [Remark("核实金额")]
            InVerifyAmount = 5,
            [Remark("后台取消订单")]
            StaffCancle = 6,
            [Remark("客户取消订单")]
            ClientCancle = 7,
            [Remark("完成")]
            Complete = 8
        }

        public enum CarClaimDealtStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Submit = 1,
            [Remark("核实需求")]
            VerifyOrder = 2,
            [Remark("上传定损单")]
            UploadEstimateListImg = 3,
            [Remark("核实金额")]
            VerifyAmount = 4,
            [Remark("支付完成")]
            Complete = 5

        }

        public enum TalentDemandDealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待处理")]
            WaitDealt = 1,
            [Remark("处理中")]
            InDealt = 2,
            [Remark("后台取消订单")]
            StaffCancle = 3,
            [Remark("客户取消订单")]
            ClientCancle = 4,
            [Remark("完成")]
            Complete = 5
        }

        public enum TalentDemandDealtStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Submit = 1,
            [Remark("处理订单")]
            Dealt = 2,
            [Remark("完成")]
            Complete = 3

        }

        public enum LllegalDealtStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Submit = 1,
            [Remark("处理订单")]
            Dealt = 2,
            [Remark("完成")]
            Complete = 3

        }

        public enum LllegalDealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待核实")]
            WaitDealt = 1,
            [Remark("核实需求")]
            InDealt = 2,
            [Remark("后台取消订单")]
            StaffCancle = 3,
            [Remark("客户取消订单")]
            ClientCancle = 4,
            [Remark("完成")]
            Complete = 5
        }


        public enum ApplyLossAssessDealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待处理")]
            WaitDealt = 1,
            [Remark("处理中")]
            InDealt = 2,
            [Remark("后台取消订单")]
            StaffCancle = 3,
            [Remark("客户取消订单")]
            ClientCancle = 4,
            [Remark("完成")]
            Complete = 5
        }


        public enum ApplyLossAssessDealtStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Submit = 1,
            [Remark("处理中")]
            Dealt = 2,
            [Remark("完成")]
            Complete = 3

        }


        public enum MerchantAuditStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待初审")]
            WaitPrimaryAudit = 1,
            [Remark("初审中")]
            InPrimaryAudit = 2,
            [Remark("等待复审")]
            WaitSeniorAudit = 3,
            [Remark("复审中")]
            InSeniorAudit = 4,
            [Remark("复审通过")]
            SeniorAuditPass = 5,
            [Remark("复审驳回")]
            SeniorAuditReject = 6
        }

        public enum MerchantAuditStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("初审")]
            PrimaryAudit = 1,
            [Remark("复审")]
            SeniorAudit = 2
        }


        public enum ExtendedAppAuditStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("初审")]
            PrimaryAudit = 1,
            [Remark("复审")]
            SeniorAudit = 2,
        }

        public enum ExtendedAppApplyType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("上架")]
            On = 1,
            [Remark("下架")]
            Off = 2,
            [Remark("恢复")]
            Recovery = 3
        }

        public enum ExtendedAppStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("上架审核中")]
            AuditOn = 1,
            [Remark("上架审核通过")]
            AuditOnPass = 2,
            [Remark("上架审核拒绝")]
            AuditOnRefuse = 3,
            [Remark("下架审核中")]
            AuditOff = 4,
            [Remark("下架审核通过")]
            AuditOffPass = 5,
            [Remark("下架审核拒绝")]
            AuditOffRefuse = 6,
            [Remark("恢复审核中")]
            AuditRecovery = 7,
            [Remark("恢复审核通过")]
            AuditRecoveryPass = 8,
            [Remark("恢复审核拒绝")]
            AuditRecoveryRefuse = 9
        }

        public enum CarInsurePlanStatus
        {
            Unknow = 0,
            CarService = 1
        }

        public enum CarKindInputType
        {
            Unknow = 0,
            None = 1,
            Text = 2,
            DropDownList = 3
        }
        public enum CarKindType
        {
            Unknow = 0,
            Compulsory = 1,
            Commercial = 2,
            AdditionalRisk = 3
        }

        public enum ProductType
        {
            Unknow = 0,
            [Remark("商品")]
            Goods = 1,
            //[Remark("汽车用品")]
            //GoodsForCar = 101,
            //[Remark("机油")]
            //GoodsForCarForMachineOil = 1011,
            //[Remark("轮胎")]
            //GoodsForCarForTyre = 1012,
            //[Remark("座垫")]
            //GoodsForCarForCushion = 1013,
            //[Remark("香水")]
            //GoodsForCarForPerfume = 1014,
            //[Remark("空气净化")]
            //GoodsForCarForAirPurge = 1015,
            //[Remark("方向盘套")]
            //GoodsForCarForSteeringWheelCover = 1016,
            //[Remark("座套")]
            //GoodsForCarForSeatCover = 1017,
            //[Remark("头枕")]
            //GoodsForCarForHeadPillow = 1018,
            [Remark("保险")]
            Insure = 2,
            [Remark("车险")]
            InsureForCar = 201,
            [Remark("车险投保")]
            InsureForCarForInsure = 2011,
            //[Remark("车险续保")]
            //InsureForCarForRenewal = 2012,
            [Remark("车险理赔")]
            InsureForCarForClaim = 2013,
            //[Remark("保险通")]
            //InsureForPopular = 202,
            [Remark("POS机服务费")]
            PosMachineServiceFee = 301,
            [Remark("人才输送")]
            TalentDemand = 401,
            [Remark("申请定损点")]
            ApplyLossAssess = 501,
            [Remark("违章查询积分充值")]
            LllegalQueryRecharge = 601,
            [Remark("违章处理")]
            LllegalDealt = 602

        }

        public enum ProductStatus
        {
            Unknow = 0,
            [Remark("上架")]
            OnLine = 1,
            [Remark("下架")]
            OffLine = 2
        }

        public enum DealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待处理")]
            Wait = 1,
            [Remark("处理中")]
            Handle = 2,
            [Remark("已处理")]
            Complete = 3
        }


        public enum OrderToCarClaimDealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("定损中")]
            Wait = 1,
            [Remark("复核中")]
            Handle = 2,
            [Remark("待支付")]
            Complete = 3
        }

        public enum CarInsuranceClaimResult
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("派单")]
            Dispatch = 1,
            [Remark("撤销")]
            Cancel = 2,
        }

        public enum OrderStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("已提交")]
            Submitted = 1,
            [Remark("跟进中")]
            Follow = 2,
            [Remark("待支付")]
            WaitPay = 3,
            [Remark("已完成")]
            Completed = 4,
            [Remark("已取消")]
            Cancled = 5
        }

        public enum OrderToCarInsureFollowStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待提交")]
            WaitSubmit = 1,
            [Remark("已提交")]
            Submitted = 2
        }

        public enum OrderToCarClaimFollowStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("定损中")]
            WaitEstimate = 1,
            [Remark("待上传定损单")]
            WaitUploadEstimateList = 2,
            [Remark("等待核实定损金额")]
            VerifyEstimateAmount = 3,
            [Remark("等待支付佣金")]
            WaitPayCommission = 6,
            [Remark("支付完成")]
            PayedCommission = 7
        }


        public enum OrderPayWay
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("POS")]
            POS = 1,
            [Remark("微信")]
            Wechat = 2,
            [Remark("支付宝")]
            Alipay = 3,
            [Remark("现金")]
            Cash = 4,
        }

        public enum BizProcessesAuditType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("扩展应用上架申请")]
            ExtendedAppOn = 1,
            [Remark("扩展应用下架申请")]
            ExtendedAppOff = 2,
            [Remark("扩展应用恢复申请")]
            ExtendedAppRecovery = 3,
            [Remark("车险订单")]
            CarInsure = 4,
            [Remark("车险理赔")]
            CarClaim = 5,
            [Remark("商户资料审核")]
            MerchantAudit = 6,
            [Remark("佣金审核")]
            CommissionRateAudit = 7,
            [Remark("人才输送")]
            TalentDemand = 8,
            [Remark("定损点申请")]
            ApplyLossAssess = 9,
            [Remark("违章处理")]
            LllegalDealt = 9
        }

        public enum MerchantStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待完善资料")]
            WaitFill = 1,
            [Remark("完善资料中")]
            InFill = 2,
            [Remark("已完善")]
            Filled = 3
        }

        public enum MerchantType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("车行")]
            CarSales = 1,
            [Remark("维修店")]
            CarRepair = 2,
            [Remark("美容店")]
            CarBeauty = 3
        }

        public enum HandMerchantType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("需求方")]
            Demand = 1,
            [Remark("维修方")]
            Supply = 2,
        }


        public enum RepairCapacity
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("无维修能力")]
            NoRepair = 1,
            [Remark("无定损权但有维修能力")]
            NoEstimateButRepair = 2,
            [Remark("有定损权也有维修能力")]
            EstimateAndRepair = 3,
        }

        public enum RepairsType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("维修和定损")]
            EstimateRepair = 1,
            [Remark("只定损")]
            Estimate = 2
        }

        public enum WithdrawStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("发起请求")]
            SendRequest = 1,
            [Remark("处理中")]
            Handing = 2,
            [Remark("成功")]
            Success = 3,
            [Remark("失败")]
            Failure = 4
        }

        public enum TransactionsType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("服务费")]
            ServiceFee = 1,
            [Remark("违章积分充值")]
            LllegalQueryRecharg = 2,
            [Remark("违章处理")]
            LllegalDealt = 3
        }

        public enum LllegalQueryScoreTransType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("初始增加积分")]
            IncreaseByInit = 1,
            [Remark("充值增加积分")]
            IncreaseByRecharge = 2,
            [Remark("查询扣除积分")]
            DecreaseByQuery = 3
        }


        public enum CommissionRateAuditStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待初审")]
            WaitPrimaryAudit = 1,
            [Remark("初审中")]
            InPrimaryAudit = 2,
            [Remark("等待复审")]
            WaitSeniorAudit = 3,
            [Remark("复审中")]
            InSeniorAudit = 4,
            [Remark("复审通过")]
            SeniorAuditPass = 5,
            [Remark("复审驳回")]
            SeniorAuditReject = 6,
            [Remark("复审拒绝")]
            SeniorAuditRefuse = 7
        }

        public enum CommissionRateAuditStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("申请")]
            Apply = 1,
            [Remark("初审")]
            PrimaryAudit = 2,
            [Remark("复审")]
            SeniorAudit = 3
        }


        public enum CarUserCharacter
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("家庭自用汽车")]
            JTZYQC = 1,
            [Remark("非营业企业客车")]
            FYYQYKC = 2,
            [Remark("非营业机关事业团体客车")]
            FYYJGSYTTKC = 3,
            [Remark("非营业货车")]
            FYYHC = 5,
            [Remark("非营业挂车")]
            FYYGC = 6,
            [Remark("兼用型拖拉机")]
            JYXTTLJ = 7,
            [Remark("其他非营业车辆")]
            QTFYYCL = 8
        }

        public enum CarVechicheType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("客车")]
            KC = 1,
            [Remark("货车")]
            HC = 2,
            [Remark("特种车")]
            TZC = 3,
            [Remark("拖拉机")]
            TLJ = 4
        }

        public enum CarSeat
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("六座以下客车")]
            S6 = 1,
            [Remark("六座至十座以下客车")]
            S10 = 2,
            [Remark("十座至十二座以下客车")]
            S12 = 3,
            [Remark("二十座至三十座以下客车")]
            S30 = 4,
            [Remark("三十座及三十六座以上客车")]
            S36 = 5
        }

        public enum CarInsuranceCompanyStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("佣金比例审核中")]
            Audit = 1,
            [Remark("正常")]
            Normal = 2,
            [Remark("停用")]
            Disable = 3
        }

        public enum OpenIdType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("车管家")]
            Cgj = 1
        }

        public enum WorkJob
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("储备店长")]
            储备店长 = 1,
            [Remark("机修")]
            机修 = 2,
            [Remark("钣喷")]
            钣喷 = 3,
            [Remark("美容")]
            美容 = 4,
            [Remark("销售顾问")]
            销售顾问 = 5,
            [Remark("电销专员")]
            电销专员 = 6,
            [Remark("前台接待")]
            前台接待 = 7,
            [Remark("理赔专员")]
            理赔专员 = 8,
            [Remark("配件销售")]
            配件销售 = 9,
            [Remark("仓管员")]
            仓管员 = 10,
            [Remark("财务出纳")]
            财务出纳 = 11,
            [Remark("行政人事专员")]
            行政人事专员 = 12
        }

        public enum PayResultNotifyParty
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("民顺主动通知")]
            MinShunNotifyUrl = 1,
            [Remark("定时任务查询民顺接口")]
            MinShunOrderQueryApi = 2,
            [Remark("后台确认")]
            Staff = 3,
            [Remark("App主动通知")]
            AppNotify = 4
        }

        public enum SysItemCacheType
        {
            Unknow = 0,
            User = 1,
            Banner = 2,
            CarInsCompanys = 3,
            CarKinds = 4,
            TalentDemandWorkJob = 5,
            ExtendedApp = 6,
        }


        public enum OrderToLllegalDealtDetailsStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待支付")]
            WaitPay = 1,
            [Remark("处理中")]
            Dealt = 2,
            [Remark("已完成")]
            Completed = 3
        }

    }
}
