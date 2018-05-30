
using log4net;
using Lumos.BLL;
using Lumos.Common;
using Lumos.DAL;
using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAppApi.Models;
using WebAppApi.Models.Account;
using WebAppApi.Models.CarService;
using WebAppApi.Models.Order;

namespace WebAppApi.Controllers
{
    public class HomeController : Controller
    {
        private string key = "test";
        private string secret = "6ZB97cdVz211O08EKZ6yriAYrHXFBowC";
        private long timespan = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;
        private string host = "http://localhost:16665";
        //private string host = "https://demo.gzhaoyilian.com";
        // private string host = "http://api.gzhaoyilian.com";
        // private string host = "https://www.ins-uplink.cn";
        //private string host = "http://120.79.233.231";
        private string YBS_key = "ybs_test";
        private string YBS_secret = "6ZB87cdVz222O08EKZ6yri8YrHXFBowA";


        public static string GetQueryString(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return "";

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }

            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);

            return s;
        }


        public static string GetQueryString2(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return "";

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }



            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);

            return s;
        }

        public string BulidOpendId(string merchantCode, string key, string secret)
        {

            byte[] result = Encoding.Default.GetBytes(merchantCode + key + secret);    //tbPass为输入密码的文本框  
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string s_output = BitConverter.ToString(output).Replace("-", "");

            return s_output;
        }

        private decimal GetDecimal(decimal d)
        {
            return Math.Round(d, 2);
        }

        Dictionary<string, string> model = new Dictionary<string, string>();

        public ActionResult Index()
        {
            decimal s2 = 4.5m;
            int s = (int)s2;

            LllegalQueryParams pms = new LllegalQueryParams();
            pms.MerchantId = 1;
            pms.UserId = 1001;
            pms.CarNo = "粤YGY662";
            pms.CarType = "02";
            pms.EnginNo = "713477";
            pms.RackNo = "004711";
            pms.IsCompany = "false";

            // var res = SdkFactory.HeLian.Query(0, pms);



            //res.


            //string aa = "{\"sign\":\"7baca0f788404905f55e28bf28f54df51fb4181a\",\"result_code\":\"00\",\"mercid\":\"894440155416002\",\"result_msg\":\"成功\",\"termid\":\"90117998\",\"txnamt\":\"1\",\"orderId\":\"D180202165700000007W\"}";

            //ReceiveNotifyModel model1 = JsonConvert.DeserializeObject<ReceiveNotifyModel>(aa);

            //OrderPayResultNotifyByMinShunLog receiveNotifyLog = new OrderPayResultNotifyByMinShunLog();

            //receiveNotifyLog.OrderId = model1.orderId;
            //receiveNotifyLog.Mercid = model1.mercid;
            //receiveNotifyLog.Termid = model1.termid;
            //receiveNotifyLog.Txnamt = model1.txnamt;
            //receiveNotifyLog.ResultCode = model1.result_code;
            //receiveNotifyLog.ResultCodeName = SdkFactory.MinShunPay.GetResultCodeName(model1.result_code);
            //receiveNotifyLog.ResultMsg = model1.result_msg;
            //receiveNotifyLog.Sign = model1.sign;
            //receiveNotifyLog.MwebUrl = null;
            //receiveNotifyLog.NotifyParty = Enumeration.PayResultNotifyParty.MinShunNotifyUrl;
            //receiveNotifyLog.NotifyPartyName = Enumeration.PayResultNotifyParty.MinShunNotifyUrl.GetCnName();
            //receiveNotifyLog.Creator = 0;
            //receiveNotifyLog.CreateTime = DateTime.Now;

            //BizFactory.Pay.ResultNotify(0, Enumeration.PayResultNotifyParty.MinShunNotifyUrl, receiveNotifyLog);

            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);



            string userName = "15989287031";
            string passWord = "123456";
            string newPassWord = "888888";
            string deviceId = "861097039013879";

            //int userId = 1216;
            //int merchantId = 242;
            //int posMachineId = 149;
            //http://localhost:16665/ExtendedApp/poscredit?userId=1215&merchantId=241&posMachineId=148
            //int userId = 1215;
            //int merchantId = 241;
            //int posMachineId = 148;

            int userId = 1234;
            int merchantId = 258;
            int posMachineId = 153;

            //CarIns(userId, merchantId, posMachineId);

            // model.Add("提交保险产品", SubmitInsurance(userId, merchantId, posMachineId));

            // model.Add("获取产品", GetProductList(userId, merchantId, posMachineId, 0));

            // model.Add("获取支付流水号", GetGetPayTranSn(1216, 1, 2, 1428, "18032722300000001428"));


            //model.Add("违章查询", SubmittLllegalQuery(1215, 241, 148));
            // model.Add("违章查询记录", GetLllegalQueryLog(1001, 1, 2));
            //model.Add("提交充值单", SubmitLllegalQueryScoreRecharge(userId, merchantId, posMachineId));
            //model.Add("提交核实支付违章处理", SubmitLllegalDealt(userId, merchantId, posMachineId, false));
            // model.Add("提交待支付违章处理", SubmitLllegalDealt(userId, merchantId, posMachineId, true));
            // model.Add("提交定损点申请", SubmittApplyLossAssess(userId, merchantId, posMachineId));
            //  model.Add("提交人才输送订单", SubmitTalentDemand(userId, merchantId, posMachineId));
            //  model.Add("提交POS机流水贷款", SubmitCredit(userId, merchantId, posMachineId));

            //model.Add("获取主页数据", GetAccoutHome(userId, merchantId, posMachineId, DateTime.Parse("2018-04-09 15:14:28")));
            //model.Add("获取全局数据", GlobalDataSet(1215, merchantId, posMachineId, DateTime.Parse("2018-04-09 15:14:28")));

            //model.Add("获取地址", GetShippingAddress(1215));

            //model.Add("添加账户", AddAccount(userName, passWord, "bf1b3357-1276-44b5-8b19-0ceba67e23e3", "959790", deviceId));
            // model.Add("登录接口", Login(userName, passWord, deviceId));

            // model.Add("获取支付二维码", QrCodeDownload(userId, merchantId, posMachineId, "18052209560000002606", Enumeration.OrderPayWay.Wechat));
            // model.Add("获取支付二维码2", QrCodeDownload(userId, merchantId, posMachineId, "D180205111300000007", Enumeration.OrderPayWay.Alipay));
            // model.Add("获取支付结果查询", PayResultQuery(userId, merchantId, posMachineId, "D180225100100001255"));

            ///model.Add("获取支付结果通知", PayResultNotify(userId, merchantId, posMachineId, "18040514310000001462", "118040514310000001462"));

            model.Add("提交投保单", SubmitInsure(userId, merchantId, posMachineId));
            // model.Add("提交跟进的投保单", SubmitFollowInsure(userId, 1600));
            //model.Add("提交理赔定损单1", SubmitEstimateList(userId, 24));
            //model.Add("提交理赔定损单2", SubmitEstimateList(userId, 25));
            //model.Add("获取订单", GetOrder(15, 1, 0));
            //model.Add("提交理赔需求1", SubmitClaim(userId, "邱大文", Enumeration.RepairsType.EstimateRepair));
            //model.Add("提交理赔需求2", SubmitClaim(userId, "邱庆文", Enumeration.RepairsType.EstimateRepair));
            //model.Add("获取订单详情1", GetOrderDetails(userId, merchantId, posMachineId, 1255, Enumeration.ProductType.PosMachineServiceFee));
            //model.Add("获取订单详情2", GetOrderDetails(userId, merchantId, 121, Enumeration.ProductType.InsureForCarForClaim));

            //model.Add("登录接口", Login(userName, passWord, "869612023700703"));

            //model.Add("获取注册账户短信", GetCreateAccountCode("181489224118"));
            //model.Add("获取忘记密码短信", GetForgetPwdCode("15989287032"));
            //model.Add("修改密码", ChangePassword(15, "123456", "123456"));
            // model.Add("重置密码接口", ResetPassword(userName, newPassWord, "382001", "5e04ef95-ac41-43a9-942d-a2b41758aef2"));
            // model.Add("注册账号", GetCreateAccountCode("15989287032"));
            //model.Add("支付结果确认", PayConfirm());
            //model.Add("支付结果", PayResultNotify());
            //model.Add("易办事销账", ReceiveNotifyByStar());




            //model.Add("提交续保单", SubmitRenewal(userId));
            //model.Add("获取订单列表1", GetOrderList(userId, merchantId, 0, 0));
            //model.Add("获取订单列表维修和定损", GetOrderList(1004, 23, 0, 0));
            //model.Add("获取订单列表只定损", GetOrderList(1006, 25, 0, 0));
            //model.Add("获取应付订单", GetPayableList(userId, merchantId, 0));
            //model.Add("检查用户是否存在", CheckUserName("uplink"));
            //model.Add("获取添加子账户短信", GetAddChildAccountCode(userId, "15989287032"));
            //model.Add("添加子账户", AddChildAccount(userId, merchantId, "邱庆文", "15989287032", "bf1b3357-1276-44b5-8b19-0ceba67e23e3", "959790"));
            //model.Add("获取子账户列表", GetChildAccountList(userId, merchantId,0));
            //model.Add("获取续保列表", GetCarServiceRenewalList(userId, merchantId, 0));
            //model.Add("获取更多推荐产品", GetMoreRecommend(userId, merchantId, 1));
            //model.Add("获取保险方案", GetInsurePlan(userId));
            //model.Add("获取保险方案详情1", GetInsurePlanKind(1,1));
            //model.Add("获取保险方案详情2", GetInsurePlanKind(1, 2));
            //model.Add("获取保险方案详情3", GetInsurePlanKind(1, 3));
            //model.Add("获取保险方案详情4", GetInsurePlanKind(1, 4));
            //model.Add("获取保险方案详情5", GetInsurePlanKind(1, 5));
            //model.Add("获取车险公司", GetInsuranceCompany(1));
            //model.Add("获取我的信息", GetPersonal(1001, 20, "00000000000000"));
            //model.Add("提现申请", WithdrawApply(userId, 1));
            //model.Add("资金明细", GetTransactions(userId, merchantId, 0));
            //model.Add("获取余额", GetBalance(userId, merchantId));
            //model.Add("提现申请", WithdrawApply(userId, 1));
            //model.Add("提现申请确认", WithdrawApplyConfirm(userId, 1));
            //model.Add("获取Banner", GetBannerList(1,Enumeration.BannerType.MainHomeTop));
            //model.Add("获取Banner详细", GetBannerDetails("4"));
            //model.Add("获取全部服务", GetExtendedAppList(ExtendedAppType.All));
            //model.Add("获取全部服务", GetExtendedAppList(ExtendedAppType.All));
            //model.Add("获取车务服务", GetExtendedAppList(ExtendedAppType.MainHomeCarService));
            //model.Add("获取推荐服务", GetExtendedAppList(ExtendedAppType.MainHomeRecommend));
            //model.Add("获取推荐商品", ProductGetHomeRecommend(1,20,1));
            //model.Add("获取商品", ProductGetList(1, 20, 101, 0,1));
            //model.Add("获取Banner", GetBannerList(Enumeration.BannerType.MainHomeTop));
            //model.Add("获取Banner详细", GetBannerDetails("4"));
            //model.Add("获取银行列表", GetBankList());
            //model.Add("获取银行卡列表", GetBankCardList(userId, merchantId));
            //model.Add("绑定银行卡", BindBankCard(userId, merchantId));
            //model.Add("解绑银行卡", RemoveBankCard(1,userId));



            return View(model);
        }

        public void TestSubmitInsure()
        {

            SubmitInsure(1107, 122, 1);

        }

        public static string stringSort(string str)
        {
            char[] chars = str.ToCharArray();
            List<string> lists = new List<string>();
            foreach (char s in chars)
            {
                lists.Add(s.ToString());
            }
            lists.Sort();//sort默认是从小到大的。显示123456789      

            str = "";
            foreach (string item in lists)
            {
                str += item;
            }
            return str;
        }

        public string ReceiveNotifyByStar()
        {
            //CrossoffAccountRequest model = new CrossoffAccountRequest();


            // string a1 = JsonConvert.SerializeObject(model);
            string a1 = "{\"request\":{\"transactioncode\":\"02\",\"bankcardnumber\":\"  \",\"cardtype\":\"DE\",\"merchantcode\":\"000567\",\"channel\":\"02\",\"serialnumber\":\"2017022210577794\",\"terminalnumber\":\"9999999B\",\"money\":\"3\",\"paytype\":\"01\",\"datetime\":\"20170222170331\"}}";



            string signStr = Signature.Compute(YBS_key, YBS_secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", YBS_key);
            headers.Add("timestamp", timespan.ToString());
            // headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/Pay/ReceiveNotifyByStar", a1, headers);

            return result;



        }

        public string PayResultNotify(int userId, int merchantId, int posMachineId, string orderSn, string paysn)
        {
            OrderPayResultNotifyLog model = new OrderPayResultNotifyLog();


            //model.MerchantId = merchantId;
            //model.PosMachineId = posMachineId;
            //model.UserId = userId;
            //model.TransResult = "1";
            //model.Amount = "1";
            //model.CardNo = "1";
            //model.BatchNo = "1";
            //model.TraceNo = "1";
            //model.RefNo = "1";
            //model.AuthCode = "1";
            //model.TransDate = "1";
            //model.TransTime = "1";
            //model.Order = paysn;
            //model.OrderSn = orderSn;
            model.CreateTime = DateTime.Now;
            model.Creator = 0;
            string a1 = JsonConvert.SerializeObject(model);



            //string a1 = "{\"productType\":\"2013\",\"merchantId\":\"" + merchantId + "\",\"userId\":\"" + userId + "\",\"orderSn\":\"" + orderSn + "\",\"orderId\":\"" + orderId + "\",\"params\":{\"merchantId\":\"861440360120020\",\"amount\":\"000000414000\",\"terminalId\":\"9999999B\",\"batchNo\":\"000001\",\"merchantName\":\"银联测试商户\",\"issue\":\"null\",\"merchantNo\":\"null\",\"traceNo\":\"000017\",\"failureReason\":\"\",\"referenceNo\":\"022310580194\",\"type\":\"\",\"result\":\"1\",\"cardNo\":\"6212263602044931384\",\"merchantInfo\":{\"order_no\":\"" + orderSn + "\",\"insurance_company\":\"平安保险\",\"insurance_type\":\"\",\"customer_id_type\":\"01\",\"customer_id\":\"a\",\"customer_sex\":\"\",\"customer_name\":\"a\",\"customer_mobile_no\":\"\",\"customer_birthdate\":\"\",\"insurance_order_no\":\"aaa\",\"car_type\":\"a\",\"car_license\":\"a\",\"car_frame_no\":\"\",\"payer_id_type\":\"\",\"payer_id\":\"\",\"payer_name\":\"\",\"payer_mobile_no\":\"\",\"payer_address\":\"\",\"ybs_mer_code\":\"000567\",\"merchant_id\":\"861440360120020\",\"merchant_name\":\"\",\"phone_no\":\"\",\"cashier_id\":\"\",\"teller_id\":\"45567\"}}";

            string signStr = Signature.Compute(key, secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/Order/PayResultNotify", a1, headers);

            return result;

        }


        public string PayConfirm()
        {
            PayConfirmModel model = new PayConfirmModel();
            model.UserId = 1001;
            model.MerchantId = 20;
            model.OrderId = 217;
            model.OrderSn = "CI1702072325220217";
            model.OrderType = Enumeration.OrderType.InsureForCarForInsure;
            model.Params = new { offerId = 232, shippingAddress = "jhjh" };


            //PayConfirmModel model = new PayConfirmModel();
            //model.UserId = 1096;
            //model.MerchantId = 111;
            //model.OrderId = 150;
            //model.OrderSn = "CI1702162039180266";
            //model.ProductType = Enumeration.ProductType.PosMachineDepositRent;
            //model.Params = new { rentMonths = 3 };


            string a1 = "{\"productType\":\"301\",\"merchantId\":\"46\",\"userId\":\"1048\",\"orderSn\":\"DR1702241513240331\",\"orderId\":\"331\",\"params\":{\"rentMonths\":\"12\",\"merchantInfo\":{}}}";

            // string a1 = JsonConvert.SerializeObject(model);

            string signStr = Signature.Compute(key, secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/Order/PayConfirm", a1, headers);

            return result;

        }


        public string GetAccoutHome(int userId, int merchantId, int posMachineId, DateTime datetime)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("posMachineId", posMachineId.ToString());
            parames.Add("datetime", datetime.ToUnifiedFormatDateTime());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Account/Home?userId=" + userId + "&merchantId=" + merchantId + "&posMachineId=" + posMachineId + "&datetime=" + HttpUtility.UrlEncode(datetime.ToUnifiedFormatDateTime(), UTF8Encoding.UTF8).ToUpper(), headers);

            return result;

        }

        public string GlobalDataSet(int userId, int merchantId, int posMachineId, DateTime datetime)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("posMachineId", posMachineId.ToString());
            parames.Add("datetime", datetime.ToUnifiedFormatDateTime());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Global/DataSet?userId=" + userId + "&merchantId=" + merchantId + "&posMachineId=" + posMachineId + "&datetime=" + HttpUtility.UrlEncode(datetime.ToUnifiedFormatDateTime(), UTF8Encoding.UTF8).ToUpper(), headers);

            return result;

        }

        public string GetMoreRecommend(int userId, int merchantId, int pageIndex)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());

            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Product/GetMoreRecommend?userId=" + userId + "&merchantId=" + merchantId + "&pageIndex=" + pageIndex, headers);

            return result;

        }


        public string Login(string username, string password, string deviceId)
        {
            LoginModel model1 = new LoginModel();
            model1.UserName = username;
            model1.Password = password;
            model1.DeviceId = deviceId;
            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Account/Login", a1, headers1);

            return respon_data4;
        }



        public string ResetPassword(string uesrName, string password, string validcode, string token)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.UserName = uesrName;
            model.Password = password;
            model.Token = token;
            model.ValidCode = validcode;
            string a1 = JsonConvert.SerializeObject(model);

            string signStr = Signature.Compute(key, secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/Account/ResetPassword", a1, headers);

            return result;

        }

        public string GetCreateAccountCode(string userName)
        {
            WebAppApi.Models.Sms.GetCreateAccountCodeModel model = new WebAppApi.Models.Sms.GetCreateAccountCodeModel();
            model.Phone = userName;

            string a1 = JsonConvert.SerializeObject(model);

            string signStr = Signature.Compute(key, secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/Sms/GetCreateAccountCode", a1, headers);

            return result;

        }


        public string GetForgetPwdCode(string phone)
        {
            WebAppApi.Models.Sms.GetForgetPwdCodeModel model = new WebAppApi.Models.Sms.GetForgetPwdCodeModel();
            model.Phone = phone;
            string a1 = JsonConvert.SerializeObject(model);

            string signStr = Signature.Compute(key, secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/Sms/GetForgetPwdCode", a1, headers);

            return result;

        }


        //public string GetCreateAccountCode(string phone)
        //{
        //    WebAppApi.Models.Sms.GetCreateAccountCodeModel model = new WebAppApi.Models.Sms.GetCreateAccountCodeModel();
        //    model.Phone = phone;
        //    string a1 = JsonConvert.SerializeObject(model);

        //    string signStr = Signature.Compute(key, secret, timespan, a1);


        //    Dictionary<string, string> headers = new Dictionary<string, string>();
        //    headers.Add("key", key);
        //    headers.Add("timestamp", timespan.ToString());
        //    headers.Add("sign", signStr);
        //    HttpUtil http = new HttpUtil();
        //    string result = http.HttpPostJson("" + host + "/api/Sms/GetCreateAccountCode", a1, headers);

        //    return result;

        //}


        public string AddAccount(string userName, string password, string token, string validCode, string deviceId)
        {
            CreateModel model = new CreateModel();
            model.UserName = userName;
            model.Password = password;
            model.Token = token;
            model.ValidCode = validCode;
            model.DeviceId = deviceId;

            string a1 = JsonConvert.SerializeObject(model);

            string signStr = Signature.Compute(key, secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/Account/Create", a1, headers);

            return result;


        }

        public string GetBannerList(int userId, Enumeration.BannerType type)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("type", ((int)type).ToString());

            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Banner/GetList?userId=" + userId.ToString() + "&type=" + ((int)type).ToString(), headers);

            return result;

        }

        public string GetBannerDetails(string id)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("id", id);

            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Banner/GetDetails?id=" + id, headers);

            return result;

        }

        //public string GetExtendedAppList(ExtendedAppType type)
        //{
        //    Dictionary<string, string> parames = new Dictionary<string, string>();
        //    parames.Add("type", ((int)type).ToString());
        //    parames.Add("userId", ((int)type).ToString());
        //    string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

        //    Dictionary<string, string> headers = new Dictionary<string, string>();
        //    headers.Add("key", key);
        //    headers.Add("timestamp", timespan.ToString());
        //    headers.Add("sign", signStr);
        //    HttpUtil http = new HttpUtil();
        //    string result = http.HttpGet("" + host + "/api/ExtendedApp/GetList?type=" + ((int)type).ToString() + "&userId=" + ((int)type).ToString(), headers);

        //    return result;

        //}

        public string GetBankList()
        {

            string signStr = Signature.Compute(key, secret, timespan, null);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Bank/GetList", headers);

            return result;

        }

        public string GetBankCardList(int userId, int merchantId)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/BankCard/GetList?userId=" + userId.ToString() + "&merchantId=" + merchantId, headers);

            return result;

        }

        public string BindBankCard(int userId, int merchantId)
        {

            //BindBankCardModel model = new BindBankCardModel();
            //model.UserId = userId;
            //model.MerchantId = merchantId;
            //model.BankId = 1;
            //model.BankAccountPhone = "15989287032";
            //model.BankAccountName = "邱庆文";
            //model.BankAccountNo = "545553232321";
            //string a1 = JsonConvert.SerializeObject(model);

            //string signStr = Signature.Compute(key, secret, timespan, a1);


            //Dictionary<string, string> headers = new Dictionary<string, string>();
            //headers.Add("key", key);
            //headers.Add("timestamp", timespan.ToString());
            //headers.Add("sign", signStr);
            //HttpUtil http = new HttpUtil();
            //string result = http.HttpPostJson("" + host + "/api/BankCard/Bind", a1, headers);

            //return result;

            return null;

        }

        public string RemoveBankCard(int id, int userId)
        {

            //RemoveBankCardModel model = new RemoveBankCardModel();
            //model.UserId = userId;
            //model.Id = 1;
            //string a1 = JsonConvert.SerializeObject(model);

            //string signStr = Signature.Compute(key, secret, timespan, a1);


            //Dictionary<string, string> headers = new Dictionary<string, string>();
            //headers.Add("key", key);
            //headers.Add("timestamp", timespan.ToString());
            //headers.Add("sign", signStr);
            //HttpUtil http = new HttpUtil();
            //string result = http.HttpPostJson("" + host + "/api/BankCard/Remove", a1, headers);

            //return result;

            return null;

        }

        public string ChangePassword(int userId, string oldPassword, string newPassword)
        {

            ChangePasswordModel model = new ChangePasswordModel();
            model.UserId = userId;
            model.OldPassword = oldPassword;
            model.NewPassword = newPassword;
            string a1 = JsonConvert.SerializeObject(model);

            string signStr = Signature.Compute(key, secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/Account/ChangePassword", a1, headers);

            return result;

        }

        public string GetInsurePlan(int userId)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/CarService/GetInsurePlan?userId=" + userId.ToString(), headers);

            return result;

        }

        public string GetInsurePlanKind(int userId, int insurePlanId)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("insurePlanId", insurePlanId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/CarService/GetInsurePlanKind?userId=" + userId.ToString() + "&insurePlanId=" + insurePlanId.ToString(), headers);

            return result;

        }


        public string GetInsuranceCompany(int userId)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/CarService/GetInsuranceCompany?userId=" + userId.ToString(), headers);

            return result;

        }


        public string GetImagesBase64String(string url)
        {
            System.IO.MemoryStream m1 = new System.IO.MemoryStream();
            System.Drawing.Bitmap bp1 = new System.Drawing.Bitmap(url);
            bp1.Save(m1, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] b1 = m1.GetBuffer();
            string base64string1 = Convert.ToBase64String(b1);


            return base64string1;
        }

        public string SubmitInsure(int userId, int merchantId, int posMachineId)
        {

            string base64string1 = GetImagesBase64String(@"d:\a.png");
            string base64string2 = GetImagesBase64String(@"d:\a.png");
            string base64string3 = GetImagesBase64String(@"d:\a.png");
            string base64string4 = GetImagesBase64String(@"d:\a.png");

            SubmitInsureModel model = new SubmitInsureModel();
            model.UserId = userId;
            model.MerchantId = merchantId;
            model.PosMachineId = posMachineId;
            model.InsurePlanId = 1;
            model.InsuranceCompanyId = new int[2] { 1, 2 };


            List<CarInsInsureKindModel> insureKindModel = new List<CarInsInsureKindModel>();
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 1, Value = "", Details = "", IsWaiverDeductible = false });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 2, Value = "", Details = "", IsWaiverDeductible = false });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 3, Value = "2000", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 4, Value = "20w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 5, Value = "30w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 6, Value = "40w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 7, Value = "3000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 8, Value = "国产", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 9, Value = "1000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 10, Value = "2000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 11, Value = "3000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 12, Value = "4000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 13, Value = "5000", Details = "购买机动" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 14, Value = "100/天", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 15, Value = "6000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 16, Value = "7000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 17, Value = "8000", Details = "" });


            model.InsureKind = insureKindModel;


            string a1 = JsonConvert.SerializeObject(model);

            if (a1.IndexOf("ImgData") > -1)
            {
                int x = a1.IndexOf("ImgData");
                a1 = a1.Substring(0, x - 2);
                a1 += "}";
            }


            ImageModel CZ_CL_XSZ_Img = new ImageModel() { Type = ".jpg", Data = base64string1 };
            ImageModel CZ_SFZ_Img = new ImageModel() { Type = ".jpg", Data = base64string2 };
            ImageModel CCSJM_WSZM_Img = new ImageModel() { Type = ".jpg", Data = base64string3 };
            ImageModel YCZ_CLDJZ_Img = new ImageModel() { Type = ".jpg", Data = base64string4 };

            model.ImgData = new Dictionary<string, ImageModel>();

            model.ImgData.Add("CZ_CL_XSZ_Img", CZ_CL_XSZ_Img);
            model.ImgData.Add("CZ_SFZ_Img", CZ_SFZ_Img);
            //model.ImgData.Add("CCSJM_WSZM_Img", CCSJM_WSZM_Img);
            //model.ImgData.Add("YCZ_CLDJZ_Img", YCZ_CLDJZ_Img);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            a1 = JsonConvert.SerializeObject(model);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            headers.Add("version", "1.0.0.5");

            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/CarService/SubmitInsure", a1, headers);

            return result;

        }

        public string SubmitRenewal(int userId)
        {

            string base64string1 = GetImagesBase64String(@"d:\a.jpg");
            string base64string2 = GetImagesBase64String(@"d:\b.jpg");
            string base64string3 = GetImagesBase64String(@"d:\c.jpg");
            string base64string4 = GetImagesBase64String(@"d:\d.jpg");

            SubmitInsureModel model = new SubmitInsureModel();
            model.UserId = userId;
            //model.Type = Enumeration.ProductType.InsureForCarForInsure;
            model.InsurePlanId = 1;
            model.InsuranceCompanyId = new int[1] { 1 };

            //list.Add(new YdtInsCoverageModel { UpLinkCode = 3, YdtCode = "001", Name = "车损险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 4, YdtCode = "002", Name = "三者险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 5, YdtCode = "003", Name = "司机险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 6, YdtCode = "004", Name = "乘客险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 7, YdtCode = "005", Name = "盗抢险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 8, YdtCode = "006", Name = "玻璃险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 9, YdtCode = "007", Name = "划痕险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 12, YdtCode = "008", Name = "自燃险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 11, YdtCode = "009", Name = "涉水险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 17, YdtCode = "010", Name = "指定维修厂" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 10, YdtCode = "011", Name = "无法找到第三方险" });


            List<CarInsInsureKindModel> insureKindModel = new List<CarInsInsureKindModel>();
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 1, Value = "", Details = "", IsWaiverDeductible = false });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 2, Value = "", Details = "", IsWaiverDeductible = false });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 3, Value = "2000", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 4, Value = "100w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 5, Value = "20w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 6, Value = "2w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 7, Value = "", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 8, Value = "国产", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 9, Value = "2000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 10, Value = "", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 11, Value = "", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 12, Value = "", Details = "" });
            //insureKindModel.Add(new InsureKindModel() { Id = 13, Value = "5000", Details = "购买机动" });
            //insureKindModel.Add(new InsureKindModel() { Id = 14, Value = "100/天", Details = "" });
            //insureKindModel.Add(new InsureKindModel() { Id = 15, Value = "6000", Details = "" });
            //insureKindModel.Add(new InsureKindModel() { Id = 16, Value = "7000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 17, Value = "", Details = "" });


            model.InsureKind = insureKindModel;


            string a1 = JsonConvert.SerializeObject(model);

            if (a1.IndexOf("ImgData") > -1)
            {
                int x = a1.IndexOf("ImgData");
                a1 = a1.Substring(0, x - 2);
                a1 += "}";
            }


            ImageModel CZ_CL_XSZ_Img = new ImageModel() { Type = ".jpg", Data = base64string1 };
            ImageModel CZ_SFZ_Img = new ImageModel() { Type = ".jpg", Data = base64string2 };
            ImageModel CCSJM_WSZM_Img = new ImageModel() { Type = ".jpg", Data = base64string3 };
            ImageModel YCZ_CLDJZ_Img = new ImageModel() { Type = ".jpg", Data = base64string4 };

            model.ImgData = new Dictionary<string, ImageModel>();

            model.ImgData.Add("CZ_CL_XSZ_Img", CZ_CL_XSZ_Img);
            // model.ImgData.Add("CZ_SFZ_Img", CZ_SFZ_Img);
            //   model.ImgData.Add("CCSJM_WSZM_Img", CCSJM_WSZM_Img);
            //  model.ImgData.Add("YCZ_CLDJZ_Img", YCZ_CLDJZ_Img);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            a1 = JsonConvert.SerializeObject(model);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/CarService/SubmitInsure", a1, headers);

            return result;

        }


        public string SubmitFollowInsure(int userId, int orderId)
        {


            string base64string1 = GetImagesBase64String(@"d:\a.png");
            string base64string2 = GetImagesBase64String(@"d:\a.png");
            string base64string3 = GetImagesBase64String(@"d:\a.png");
            string base64string4 = GetImagesBase64String(@"d:\a.png");

            SubmitFollowInsureModel model = new SubmitFollowInsureModel();
            model.UserId = userId;
            model.OrderId = orderId;


            string a1 = JsonConvert.SerializeObject(model);


            if (a1.IndexOf("ImgData") > -1)
            {
                int x = a1.IndexOf("ImgData");
                a1 = a1.Substring(0, x - 2);
                a1 += "}";
            }

            string signStr = Signature.Compute(key, secret, timespan, a1);


            ImageModel ZJ1_Img = new ImageModel() { Type = ".jpg", Data = base64string1 };
            ImageModel ZJ2_Img = new ImageModel() { Type = ".jpg", Data = base64string2 };
            ImageModel ZJ3_Img = new ImageModel() { Type = ".jpg", Data = base64string3 };
            ImageModel ZJ4_Img = new ImageModel() { Type = ".jpg", Data = base64string4 };

            model.ImgData = new Dictionary<string, ImageModel>();

            model.ImgData.Add("ZJ1_Img", ZJ1_Img);
            model.ImgData.Add("ZJ2_Img", ZJ2_Img);
            model.ImgData.Add("ZJ3_Img", ZJ3_Img);
            model.ImgData.Add("ZJ4_Img", ZJ4_Img);


            a1 = JsonConvert.SerializeObject(model);



            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/CarService/SubmitFollowInsure", a1, headers);

            return result;

        }

        public string GetOrder(int userId, int merchantId, int status)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("status", status.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Order/GetList?userId=" + userId.ToString() + "&merchantId=" + merchantId.ToString() + "&status= " + status.ToString(), headers);

            return result;

        }

        public string WithdrawApply(int userId, int bankCardId)
        {

            //WithdrawApplyModel model1 = new WithdrawApplyModel();
            //model1.UserId = userId;
            //model1.BankCardId = bankCardId;
            //model1.Confirm = true;
            //Random r = new Random();
            //int num = r.Next(1, 100);
            //model1.Amount = num;
            //string a1 = JsonConvert.SerializeObject(model1);

            //string signStr = Signature.Compute(key, secret, timespan, a1);

            //Dictionary<string, string> headers1 = new Dictionary<string, string>();
            //headers1.Add("key", key);
            //headers1.Add("timestamp", (timespan.ToString()).ToString());
            //headers1.Add("sign", signStr);

            //// string a1 = "a1=das&a2=323";
            //HttpUtil http = new HttpUtil();
            //string respon_data4 = http.HttpPostJson("" + host + "/api/Withdraw/Apply", a1, headers1);

            //return respon_data4;

            return null;

        }


        public string WithdrawApplyConfirm(int userId, int bankCardId)
        {

            //WithdrawApplyModel model1 = new WithdrawApplyModel();
            //model1.UserId = userId;
            //model1.BankCardId = bankCardId;
            //model1.Confirm = true;
            //Random r = new Random();
            //int num = r.Next(1, 100);
            //model1.Amount = num;
            //string a1 = JsonConvert.SerializeObject(model1);

            //string signStr = Signature.Compute(key, secret, timespan, a1);

            //Dictionary<string, string> headers1 = new Dictionary<string, string>();
            //headers1.Add("key", key);
            //headers1.Add("timestamp", (timespan.ToString()).ToString());
            //headers1.Add("sign", signStr);

            //// string a1 = "a1=das&a2=323";
            //HttpUtil http = new HttpUtil();
            //string respon_data4 = http.HttpPostJson("" + host + "/api/Withdraw/Apply", a1, headers1);

            //return respon_data4;

            return null;

        }



        public string GetTransactions(int userId, int merchantId, int pageIndex)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Fund/GetTransactions?userId=" + userId.ToString() + "&merchantId=" + merchantId.ToString() + "&pageIndex=" + pageIndex.ToString(), headers);

            return result;

        }

        public string GetBalance(int userId, int merchantId)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Fund/GetBalance?userId=" + userId.ToString() + "&merchantId=" + merchantId.ToString(), headers);

            return result;

        }


        public string CheckUserName(string userName)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userName", userName.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Account/CheckUserName?userName=" + userName.ToString(), headers);

            return result;
        }

        public string ProductGetHomeRecommend(int userId, int merchantId, int pageIndex)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());

            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Product/GetMoreRecommend?userId=" + userId.ToString() + "&merchantId=" + merchantId.ToString() + "&pageIndex=" + pageIndex, headers);

            return result;
        }


        public string ProductGetList(int userId, int merchantId, int type, int pageIndex, int pricearea)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());
            parames.Add("type", type.ToString());
            parames.Add("priceArea", pricearea.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Product/GetList?userId=" + userId.ToString() + "&merchantId=" + merchantId.ToString() + "&pageIndex=" + pageIndex + "&type=" + type + "&priceArea=" + pricearea, headers);

            return result;
        }



        public string SubmitClaim(int userId, string handPerson, Enumeration.RepairsType estimateRepair)
        {

            SubmitClaimModel model1 = new SubmitClaimModel();
            model1.UserId = userId;
            model1.RepairsType = estimateRepair;
            model1.InsuranceCompanyId = 1;
            model1.HandPerson = handPerson;
            model1.HandPersonPhone = "15989287032";
            model1.CarLicenseNumber = "粤AT88899";
            model1.ClientRequire = "我在附近修理";

            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/CarService/SubmitClaim", a1, headers1);

            return respon_data4;

        }

        public string SubmitEstimateList(int userId, int orderId)
        {
            string base64string1 = GetImagesBase64String(@"d:\a.jpg");


            SubmitEstimateListModel model = new SubmitEstimateListModel();
            model.UserId = userId;
            model.OrderId = orderId;

            string a1 = JsonConvert.SerializeObject(model);
            if (a1.IndexOf("ImgData") > -1)
            {
                int x = a1.IndexOf("ImgData");
                a1 = a1.Substring(0, x - 2);
                a1 += "}";
            }

            string signStr = Signature.Compute(key, secret, timespan, a1);

            ImageModel ZJ1_Img = new ImageModel() { Type = ".jpg", Data = base64string1 };

            model.ImgData = new Dictionary<string, ImageModel>();

            model.ImgData.Add("EstimateListImg", ZJ1_Img);

            a1 = JsonConvert.SerializeObject(model);



            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/CarService/SubmitEstimateList", a1, headers1);

            return respon_data4;

        }

        public string GetPersonal(int userId, int merchantId, string deviceId)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("deviceId", deviceId.ToString());

            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Account/Personal?userId=" + userId.ToString() + "&merchantId=" + merchantId + "&deviceId=" + deviceId, headers);

            return result;
        }

        public string GetChildAccountList(int userId, int merchantId, int pageIndex)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());

            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Account/GetChildAccountList?userId=" + userId.ToString() + "&merchantId=" + merchantId + "&pageIndex=" + pageIndex, headers);

            return result;
        }

        public string GetCarServiceRenewalList(int userId, int merchantId, int pageIndex)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());

            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/CarService/GetRenewal?userId=" + userId.ToString() + "&merchantId=" + merchantId + "&pageIndex=" + pageIndex, headers);

            return result;
        }

        public string GetOrderList(int userId, int merchantId, int pageIndex, int status)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());
            parames.Add("status", status.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Order/GetList?userId=" + userId.ToString() + "&merchantId=" + merchantId + "&pageIndex=" + pageIndex + "&status=" + status, headers);

            return result;
        }

        public string GetPayableList(int userId, int merchantId, int pageIndex)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Order/GetPayableList?userId=" + userId.ToString() + "&merchantId=" + merchantId + "&pageIndex=" + pageIndex, headers);

            return result;
        }



        public string GetOrderDetails(int userId, int merchantId, int posMachineId, int orderId, Enumeration.ProductType productType)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("posMachineId", posMachineId.ToString());
            parames.Add("orderId", orderId.ToString());
            parames.Add("productType", ((int)productType).ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Order/GetDetails?userId=" + userId.ToString() + "&merchantId=" + merchantId + "&posMachineId=" + posMachineId + "&orderId=" + orderId + "&productType=" + ((int)productType).ToString(), headers);

            return result;
        }


        public string SubmitTalentDemand(int userId, int merchantId, int posMachineId)
        {

            SubmitTalentDemandModel model1 = new SubmitTalentDemandModel();
            model1.UserId = userId;
            model1.Quantity = 2;
            model1.WorkJob = Enumeration.WorkJob.周末兼职;
            model1.MerchantId = merchantId;
            model1.PosMachineId = posMachineId;
            model1.UseStartTime = DateTime.Now;
            model1.UseEndTime = DateTime.Now;
            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/SubmitTalentDemand", a1, headers1);

            return respon_data4;

        }

        public string QrCodeDownload(int userId, int merchantId, int posMachineId, string ordersn, Enumeration.OrderPayWay payway)
        {

            PayUnifiedOrderParams model1 = new PayUnifiedOrderParams();
            model1.UserId = userId;
            model1.MerchantId = merchantId;
            model1.PosMachineId = posMachineId;
            model1.OrderSn = ordersn;
            model1.PayWay = payway;

            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/PayUnifiedOrder", a1, headers1);

            return respon_data4;

        }

        public string PayResultQuery(int userId, int merchantId, int posMachineId, string orderSn)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("posMachineId", posMachineId.ToString());
            parames.Add("orderSn", orderSn);

            //PayQueryParams model1 = new PayQueryParams();
            //model1.UserId = userId;
            ////model1.MerchantId = merchantId;
            ////model1.PosMachineId = posMachineId;
            //model1.OrderSn = ordersn;

            //string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpGet("" + host + "/api/Order/PayResultQuery?userId=" + userId.ToString() + "&merchantId=" + merchantId.ToString() + "&posMachineId=" + posMachineId.ToString() + "&orderSn=" + orderSn, headers1);

            return respon_data4;

        }


        public string SubmittApplyLossAssess(int userId, int merchantId, int posMachineId)
        {

            SubmitApplyLossAssessModel model1 = new SubmitApplyLossAssessModel();
            model1.UserId = userId;
            model1.InsuranceCompanyId = 2;
            model1.MerchantId = merchantId;
            model1.PosMachineId = posMachineId;

            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/SubmitApplyLossAssess", a1, headers1);

            return respon_data4;

        }


        public string SubmittLllegalQuery(int userId, int merchantId, int posMachineId)
        {
            //pms.CarNo = "粤YGY662";
            //pms.CarType = "02";
            //pms.EnginNo = "713477";
            //pms.RackNo = "004711";
            LllegalQueryParams pms = new LllegalQueryParams();
            pms.MerchantId = merchantId;
            pms.PosMachineId = posMachineId;
            pms.UserId = userId;
            pms.CarNo = "粤E58H56";
            pms.CarType = "02";
            pms.EnginNo = "713477";
            pms.RackNo = "004711";
            pms.IsCompany = "false";
            pms.IsOfferPrice = "false";
            //SubmitApplyLossAssessModel model1 = new SubmitApplyLossAssessModel();
            //model1.UserId = userId;
            //model1.InsuranceCompanyId = 2;
            //model1.MerchantId = merchantId;
            //model1.PosMachineId = posMachineId;

            string a1 = JsonConvert.SerializeObject(pms);

            a1 = "{\"merchantId\":258,\"carType\":\"02\",\"userId\":1234,\"carNo\":\"粤A7CR57\",\"isCompany\":\"false\",\"isOfferPrice\":\"true\",\"enginNo\":\"104595\",\"rackNo\":\"034618\",\"posMachineId\":153}";
            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Lllegal/Query", a1, headers1);

            return respon_data4;

        }

        public string GetLllegalQueryLog(int userId, int merchantId, int posMachineId)
        {

            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("posMachineId", posMachineId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Lllegal/QueryLog?userId=" + userId.ToString() + "&merchantId=" + merchantId.ToString() + "&posMachineId=" + posMachineId.ToString(), headers);

            return result;

        }


        public string SubmitLllegalQueryScoreRecharge(int userId, int merchantId, int posMachineId)
        {

            SubmitLllegalQueryScoreRechargeModel model1 = new SubmitLllegalQueryScoreRechargeModel();
            model1.UserId = userId;
            model1.MerchantId = merchantId;
            model1.PosMachineId = posMachineId;
            model1.Score = 50;

            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/SubmitLllegalQueryScoreRecharge", a1, headers1);

            return respon_data4;

        }


        public string SubmitLllegalDealt(int userId, int merchantId, int posMachineId, bool isget)
        {

            SubmitLllegalDealtModel model1 = new SubmitLllegalDealtModel();
            model1.UserId = userId;
            model1.MerchantId = merchantId;
            model1.PosMachineId = posMachineId;
            model1.CarNo = "粤AT88888";
            model1.IsOfferPrice = "true";
            model1.IsGetConfirmInfo = isget;
            List<LllegalRecord> record = new List<LllegalRecord>();


            record.Add(new LllegalRecord { bookNo = "1", address = "2", point = 1, serviceFee = 50, late_fees = 2, fine = 300, cityCode = "4400", content = "sdasdadd", lllegalTime = "2012-12-12" });
            record.Add(new LllegalRecord { bookNo = "2", address = "2", point = 1, serviceFee = 50, late_fees = 2, fine = 300, cityCode = "4400", content = "sdasdadd", lllegalTime = "2012-12-12" });
            record.Add(new LllegalRecord { bookNo = "3", address = "2", point = 1, serviceFee = 50, late_fees = 2, fine = 300, cityCode = "4400", content = "sdasdadd", lllegalTime = "2012-12-12" });
            record.Add(new LllegalRecord { bookNo = "4", address = "2", point = 1, serviceFee = 50, late_fees = 2, fine = 300, cityCode = "4400", content = "sdasdadd", lllegalTime = "2012-12-12" });


            model1.LllegalRecord = record;

            string a1 = JsonConvert.SerializeObject(model1);

            // string a1 = "{\"lllegalRecord\":[{\"bookNo\":\"4401107901494580\",\"bookType\":\"6001A\",\"bookTypeName\":\"非现场未处理违章数据\",\"lllegalCode\":\"1344\",\"cityCode\":\"440110\",\"lllegalTime\":\"2017-07-11 13:33:00\",\"point\":\"3.0\",\"offerType\":\"3\",\"ofserTypeName\":\"扣分单\",\"fine\":\"200.0\",\"serviceFee\":\"435.0\",\"late_fees\":\"200.0\",\"content\":\"免资料；2-3个工作日.超证另通知\",\"lllegalDesc\":\"机动车违反禁令标志指示的\",\"lllegalCity\":\"广东省广州市\",\"address\":\"元岗横路_元岗横路路段\"}],\"merchantId\":241,\"carNo\":\"粤YGY662\",\"userId\":1215,\"posMachineId\":149}";
            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Lllegal/Dealt", a1, headers1);

            return respon_data4;

        }


        public string GetGetPayTranSn(int userId, int merchantId, int posMachineId, int orderId, string orderSn)
        {


            GetPayTranSnParams pms = new GetPayTranSnParams();
            pms.MerchantId = merchantId;
            pms.PosMachineId = posMachineId;
            pms.UserId = userId;
            pms.OrderId = orderId;
            pms.OrderSn = orderSn;

            string a1 = JsonConvert.SerializeObject(pms);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/GetPayTranSn", a1, headers1);

            return respon_data4;


        }

        public string SubmitCredit(int userId, int merchantId, int posMachineId)
        {

            SubmitCreditModel model1 = new SubmitCreditModel();
            model1.UserId = userId;
            model1.Creditline = 20000;
            model1.CreditClass = "POS机流水款";
            model1.MerchantId = merchantId;
            model1.PosMachineId = posMachineId;

            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/SubmitCredit", a1, headers1);

            return respon_data4;

        }


        public string GetProductList(int userId, int merchantId, int posMachineId, int pageIndex)
        {

            int type = 0;
            int categoryId = 0;
            int kindId = 0;
            string name = "";
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("posMachineId", posMachineId.ToString());
            parames.Add("pageIndex", pageIndex.ToString());
            parames.Add("type", type.ToString());
            parames.Add("categoryId", categoryId.ToString());
            parames.Add("kindId", kindId.ToString());
            parames.Add("name", name.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Product/GetList?userId=" + userId.ToString() + "&merchantId=" + merchantId + "&posMachineId=" + posMachineId + "&pageIndex=" + pageIndex + "&type=" + type + "&categoryId=" + categoryId + "&kindId=" + kindId + "&name=" + name, headers);

            return result;
        }

        public string SubmitInsurance(int userId, int merchantId, int posMachineId)
        {

            SubmitInsuranceModel model1 = new SubmitInsuranceModel();
            model1.UserId = userId;
            model1.MerchantId = merchantId;
            model1.PosMachineId = posMachineId;
            model1.ProductSkuId = 1;

            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/SubmitInsurance", a1, headers1);

            return respon_data4;

        }

        public string GetShippingAddress(int userId)
        {

            int type = 0;
            int categoryId = 0;
            int kindId = 0;
            string name = "";
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/ShippingAddress/GetList?userId=" + userId.ToString(), headers);

            return result;
        }


        public void CarIns(int userId, int merchantId, int posMachineId)
        {


            //model.Add("获取车辆信息", CarIns_GetCarInfo(userId, merchantId, posMachineId));
            //model.Add("车辆查询接口", CarIns_GetCarModelInfo(userId, merchantId, posMachineId));

            //model.Add("添加基础信息", CarIns_EditBaseInfo(userId, merchantId, posMachineId));
            // model.Add("询价信息", CarIns_InsComanyInfo(userId, merchantId, posMachineId));
            // model.Add("报价信息", CarIns_InsInquiry(userId, merchantId, posMachineId));


            //string s_CarIns_GetCarInfo = CarIns_GetCarInfo(userId, merchantId, posMachineId);

            //CustomJsonResult<CarInfoResult> s1 = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomJsonResult<CarInfoResult>>(s_CarIns_GetCarInfo);


            //string s_CarIns_GetCarModelInfo = CarIns_GetCarModelInfo(userId, merchantId, posMachineId);

            //CustomJsonResult<CarModelInfoResult> s2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomJsonResult<CarModelInfoResult>>(s_CarIns_GetCarModelInfo);
            //CarIn_Notify(1, 2, 3, "5", "4");
            string s_CarIns_EditBaseInfo = CarIns_EditBaseInfo(userId, merchantId, posMachineId);

            CustomJsonResult<CarInsEditBaseInfoResult> s3 = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomJsonResult<CarInsEditBaseInfoResult>>(s_CarIns_EditBaseInfo);

            if (s3.Result == ResultType.Success)
            {
                //"ac9d0a28-2c55-49a3-bb0e-81029eddb23a"
                model.Add("报价信息", CarIns_InsInquiry(userId, merchantId, posMachineId, s3.Data.CarInfoOrderId, s3.Data.OrderSeq));

            }
        }


        public string CarIns_GetCarInfo(int userId, int merchantId, int posMachineId)
        {

            //string base64string1 = GetImagesBase64String(@"d:\demo_jsz.jpg");

            CarInfoPms model1 = new CarInfoPms();
            model1.KeywordType = KeyWordType.LicensePlateNo;
            model1.Keyword = "粤A9RS97";
            // model1.Keyword = "粤AT810P";
            //model1.KeywordType = KeyWordType.LicenseImg;
            // model1.Keyword = base64string1 + "@.jpg";


            //SubmitEstimateListModel model = new SubmitEstimateListModel();
            //model.UserId = userId;
            //model.OrderId = orderId;

            //string a1 = JsonConvert.SerializeObject(model);
            //if (a1.IndexOf("ImgData") > -1)
            //{
            //    int x = a1.IndexOf("ImgData");
            //    a1 = a1.Substring(0, x - 2);
            //    a1 += "}";
            //}


            string a1 = JsonConvert.SerializeObject(model1);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/CarIns/GetCarInfo", a1, headers1);

            return respon_data4;
        }

        public string CarIns_GetCarModelInfo(int userId, int merchantId, int posMachineId)
        {

            string keyword = "LFV3A23C6E3095934";
            string vin = "LFV3A23C6E3095934";
            string firstRegisterDate = "2016-06-12";
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("keyword", keyword);
            parames.Add("vin", vin);
            parames.Add("firstRegisterDate", firstRegisterDate);
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);

            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/CarIns/GetCarModelInfo?keyword=" + keyword.ToString() + "&vin=" + vin + "&firstRegisterDate=" + firstRegisterDate, headers);

            return result;
        }


        public string CarIns_EditBaseInfo(int userId, int merchantId, int posMachineId)
        {

            CarInsEditBaseInfoPms model = new CarInsEditBaseInfoPms();
            // model.OrderSeq = "46379";
            model.UserId = userId;
            model.MerchantId = merchantId;
            model.PosMachineId = posMachineId;
            model.Auto = "1";
            model.Car.Belong = "1";
            model.Car.LicensePlateNo = "粤A9RS97";
            model.Car.Vin = "LFV3A23C6E3095934";
            model.Car.EngineNo = "242956";
            model.Car.FirstRegisterDate = "2015-08-11";
            model.Car.ModelCode = "FV7207FCDWG";
            model.Car.ModelName = "FV7207FCDWG";
            model.Car.Displacement = "2000";
            model.Car.MarketYear = "2012";
            model.Car.RatedPassengerCapacity = 5;
            model.Car.ReplacementValue = 266300;
            model.Car.ChgownerType = "0";
            model.Car.ChgownerDate = "";
            model.Car.Tonnage = "";
            model.Car.WholeWeight = "";

            model.Customers.Add(new CarInsCustomerModel { InsuredFlag = "3", Name = "朱元刚", CertNo = "440182198804141553", Mobile = "15989287032", Address = "广东省花都区" });

            string a1 = JsonConvert.SerializeObject(model);
            // a1= "{\"auto\":\"1\",\"car\":{\"belong\":\"1\",\"carType\":\"1\",\"licensePlateNo\":\"粤AT810P\",\"vin\":\"LFMAPE2C8B0906269\",\"engineNo\":\"E764059\",\"firstRegisterDate\":\"2011-03-15\",\"displacement\":\"1600\",\"ratedPassengerCapacity\":\"108800\",\"replacementValue\":\"108800\",\"chgownerType\":\"1\"},\"orderSeq\":\"\",\"customers\":[{\"name\":\"黄大爷\"},{\"name\":\"黄大爷\"},{\"name\":\"黄大爷\",\"certNo\":\"\"}]}";
            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/CarIns/EditBaseInfo", a1, headers1);

            return respon_data4;
        }

        public string CarIns_InsComanyInfo(int userId, int merchantId, int posMachineId)
        {

            CarInsComanyInfoPms model = new CarInsComanyInfoPms();
            model.OrderSeq = "5b2e835c-15cd-4a53-b8c6-fedd9a26693d";
            model.AreaId = 440100;

            string a1 = JsonConvert.SerializeObject(model);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/CarIns/InsComanyInfo", a1, headers1);

            return respon_data4;
        }


        public string CarIns_InsInquiry(int userId, int merchantId, int posMachineId, int carInfoOrderId, string orderSeq)
        {
            CarInsInquiryPms model = new CarInsInquiryPms();
            model.Auto = 1;
            model.UserId = userId;
            model.MerchantId = merchantId;
            model.PosMachineId = posMachineId;
            model.CarInfoOrderId = carInfoOrderId;
            model.OrderSeq = orderSeq;
            model.BiStartDate = "2018-05-20";
            model.CiStartDate = "2018-05-20";
            model.ChannelId = 1;
            model.CompanyCode = "006000";

            model.Car.Belong = "1";
            model.Car.LicensePlateNo = "粤L29K83";
            model.Car.Vin = "LS4ASM2E2GF021746";
            model.Car.EngineNo = "GA8U020728";
            model.Car.FirstRegisterDate = "2011-03-15";
            model.Car.ModelCode = "CAD2586CQC";
            model.Car.ModelName = "长安SC6458A5多用途乘用车";
            model.Car.Displacement = "1500";
            model.Car.MarketYear = "2015";
            model.Car.RatedPassengerCapacity = 5;
            model.Car.ReplacementValue = 52900;
            model.Car.ChgownerType = "0";
            model.Car.ChgownerDate = "";
            model.Car.Tonnage = "";
            model.Car.WholeWeight = "";


            //list.Add(new YdtInsCoverageModel { UpLinkCode = 3, Code = "001", Name = "车损险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 4, Code = "002", Name = "三者险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 5, Code = "003", Name = "司机险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 6, Code = "004", Name = "乘客险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 7, Code = "005", Name = "盗抢险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 8, Code = "006", Name = "玻璃险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 9, Code = "007", Name = "划痕险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 12, Code = "008", Name = "自燃险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 11, Code = "009", Name = "涉水险" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 17, Code = "010", Name = "指定维修厂" });
            //list.Add(new YdtInsCoverageModel { UpLinkCode = 10, Code = "011", Name = "无法找到第三方险" });

            List<CarInsInsureKindModel> insureKindModel = new List<CarInsInsureKindModel>();
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 1, Value = "", Details = "", IsWaiverDeductible = false });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 2, Value = "", Details = "", IsWaiverDeductible = false });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 3, Value = "2000", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 4, Value = "100w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 5, Value = "20w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 6, Value = "2w", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 7, Value = "", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 8, Value = "国产", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 9, Value = "2000", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 10, Value = "", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 11, Value = "", Details = "", IsWaiverDeductible = true });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 12, Value = "", Details = "", IsWaiverDeductible = true });
            //insureKindModel.Add(new InsureKindModel() { Id = 13, Value = "5000", Details = "购买机动" });
            //insureKindModel.Add(new InsureKindModel() { Id = 14, Value = "100/天", Details = "" });
            //insureKindModel.Add(new InsureKindModel() { Id = 15, Value = "6000", Details = "" });
            //insureKindModel.Add(new InsureKindModel() { Id = 16, Value = "7000", Details = "" });
            insureKindModel.Add(new CarInsInsureKindModel() { Id = 17, Value = "", Details = "" });


            model.InsureKind = insureKindModel;


            string a1 = JsonConvert.SerializeObject(model);



            //a1 = "{\"car\":{\"replacementValue\":\"52900\",\"firstRegisterDate\":\"2011-03-15\"},\"auto\":1,\"channelId\":19,\"orderSeq\":\"5b2e835c-15cd-4a53-b8c6-fedd9a26693d\",\"companyCode\":\"002000\",\"risk\":3,\"biStartDate\":\"2018-05-05\",\"ciStartDate\":\"2018-05-05\",\"coverages\":[{\"code\":\"001\",\"compensation\":1,\"amount\":43900.0,\"unitAmount\":0.0,\"quantity\":0,\"glassType\":0},{\"code\":\"002\",\"compensation\":1,\"amount\":200000.0,\"unitAmount\":0.0,\"quantity\":0,\"glassType\":0},{\"code\":\"003\",\"compensation\":1,\"amount\":300000.0,\"unitAmount\":300000.0,\"quantity\":1,\"glassType\":0},{\"code\":\"004\",\"compensation\":1,\"amount\":-400000.0,\"unitAmount\":400000.0,\"quantity\":-1,\"glassType\":0},{\"code\":\"005\",\"compensation\":0,\"amount\":43900.0,\"unitAmount\":0.0,\"quantity\":0,\"glassType\":0},{\"code\":\"006\",\"compensation\":0,\"amount\":0.0,\"unitAmount\":0.0,\"quantity\":0,\"glassType\":1},{\"code\":\"007\",\"compensation\":0,\"amount\":1000.0,\"unitAmount\":1000.0,\"quantity\":1,\"glassType\":0},{\"code\":\"011\",\"compensation\":0,\"amount\":43900.0,\"unitAmount\":0.0,\"quantity\":0,\"glassType\":0},{\"code\":\"009\",\"compensation\":0,\"amount\":43900.0,\"unitAmount\":0.0,\"quantity\":0,\"glassType\":0},{\"code\":\"008\",\"compensation\":0,\"amount\":43900.0,\"unitAmount\":0.0,\"quantity\":0,\"glassType\":0},{\"code\":\"010\",\"compensation\":0,\"amount\":43900.0,\"unitAmount\":0.0,\"quantity\":0,\"glassType\":0}]}";


            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            // string a1 = "a1=das&a2=323";
            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/CarIns/InsInquiry", a1, headers1);

            return respon_data4;
        }

        public string CarIn_Notify(int userId, int merchantId, int posMachineId, string orderSn, string paysn)
        {
            OrderPayResultNotifyLog model = new OrderPayResultNotifyLog();


            //model.MerchantId = merchantId;
            //model.PosMachineId = posMachineId;
            //model.UserId = userId;
            //model.TransResult = "1";
            //model.Amount = "1";
            //model.CardNo = "1";
            //model.BatchNo = "1";
            //model.TraceNo = "1";
            //model.RefNo = "1";
            //model.AuthCode = "1";
            //model.TransDate = "1";
            //model.TransTime = "1";
            //model.Order = paysn;
            //model.OrderSn = orderSn;
            model.CreateTime = DateTime.Now;
            model.Creator = 0;
            string a1 = JsonConvert.SerializeObject(model);



            //string a1 = "{\"productType\":\"2013\",\"merchantId\":\"" + merchantId + "\",\"userId\":\"" + userId + "\",\"orderSn\":\"" + orderSn + "\",\"orderId\":\"" + orderId + "\",\"params\":{\"merchantId\":\"861440360120020\",\"amount\":\"000000414000\",\"terminalId\":\"9999999B\",\"batchNo\":\"000001\",\"merchantName\":\"银联测试商户\",\"issue\":\"null\",\"merchantNo\":\"null\",\"traceNo\":\"000017\",\"failureReason\":\"\",\"referenceNo\":\"022310580194\",\"type\":\"\",\"result\":\"1\",\"cardNo\":\"6212263602044931384\",\"merchantInfo\":{\"order_no\":\"" + orderSn + "\",\"insurance_company\":\"平安保险\",\"insurance_type\":\"\",\"customer_id_type\":\"01\",\"customer_id\":\"a\",\"customer_sex\":\"\",\"customer_name\":\"a\",\"customer_mobile_no\":\"\",\"customer_birthdate\":\"\",\"insurance_order_no\":\"aaa\",\"car_type\":\"a\",\"car_license\":\"a\",\"car_frame_no\":\"\",\"payer_id_type\":\"\",\"payer_id\":\"\",\"payer_name\":\"\",\"payer_mobile_no\":\"\",\"payer_address\":\"\",\"ybs_mer_code\":\"000567\",\"merchant_id\":\"861440360120020\",\"merchant_name\":\"\",\"phone_no\":\"\",\"cashier_id\":\"\",\"teller_id\":\"45567\"}}";

            string signStr = Signature.Compute(key, secret, timespan, a1);


            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpPostJson("" + host + "/api/CarIns/OfferNotify", a1, headers);

            return result;

        }
    }
}