using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PayParameter
    {
        public string transNo { set; get; }//商户订单号:渠道编码+yyyyMMrrHHmmss+随机数（最多32个字符，可包含字母、数字）
        public string requestCode { set; get; }//请求方代码:安心提供
        public int transAmt { set; get; }//交易金额:以分为单位(int类型）
        public string payName { set; get; }//支付名称:支付商品的描述，例如：保险产品名称“机动车保险”
        public string bgRetUrl { set; get; }//后台回调地址
        public string remark { set; get; }//描述
        public string checkValue { set; get; }//加密串:通过md5对(合作验证码 + 原始数据) 进行签名,测试环境：合作验证码：xxx
        public string payCancelUR { set; get; }//支付取消页面地址
        public string payFinishURL { set; get; }//支付完成页面地址
        public string payType { set; get; }//支付方式
        public string attach { set; get; }//附加数据:通知原样返回，携带自定义参数
        public PayParameter()
        {
            transAmt = 0;
            bgRetUrl = "";
        }

    }
}
