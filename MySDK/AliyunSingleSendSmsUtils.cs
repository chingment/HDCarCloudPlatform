using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Sms.Model.V20160927;
using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySDK
{
    public class AliyunSingleSendSmsUtils
    {
        public static CustomJsonResult Send(string template, string smsparam, string mobile, out string token, string validCode = null, int? expireSecond = null)
        {
            CustomJsonResult result = new CustomJsonResult();
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "LTAInW2wvf70MRTU", "wCceTF0BOmMPgctSPPLfmMNMfyFRXS");
            IAcsClient client = new DefaultAcsClient(profile);

            SingleSendSmsRequest request = new SingleSendSmsRequest();

            LumosDbContext currentDb = new LumosDbContext();

            SysSmsSendHistory sendHistory = new SysSmsSendHistory();
            token = Guid.NewGuid().ToString();
            sendHistory.Token = token;
            sendHistory.ApiName = "AliyunSingleSendSmsUtils";
            sendHistory.TemplateParams = smsparam;
            sendHistory.TemplateCode = template;
            sendHistory.Phone = mobile;
            sendHistory.CreateTime = DateTime.Now;
            sendHistory.Creator = 0;
            sendHistory.ValidCode = validCode;
            if (expireSecond != null)
            {
                sendHistory.ExpireTime = sendHistory.CreateTime.AddSeconds(expireSecond.Value);
            }

            try
            {

                request.SignName = "";//"管理控制台中配置的短信签名（状态必须是验证通过）"
                request.TemplateCode = template;//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
                request.RecNum = mobile;//"接收号码，多个号码可以逗号分隔"
                request.ParamString = smsparam;//短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
                //SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);


                sendHistory.Result = Enumeration.SysSmsSendResult.Success;

                result = new CustomJsonResult(ResultType.Success, "发送成功");

            }
            catch (ServerException ex)
            {
                sendHistory.Result = Enumeration.SysSmsSendResult.Exception;

                sendHistory.FailureReason = ex.ErrorCode;

                result = new CustomJsonResult(ResultType.Exception, "发送失败");

            }
            catch (ClientException ex)
            {
                sendHistory.Result = Enumeration.SysSmsSendResult.Exception;

                sendHistory.FailureReason = ex.ErrorCode;

                result = new CustomJsonResult(ResultType.Exception, "发送失败");
            }
            catch (Exception ex)
            {
                sendHistory.Result = Enumeration.SysSmsSendResult.Exception;

                sendHistory.FailureReason = ex.Message;

                result = new CustomJsonResult(ResultType.Exception, "发送失败");
            }

            currentDb.SysSmsSendHistory.Add(sendHistory);
            currentDb.SaveChanges();

            return result;
        }
    }
}
