using log4net;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace AnXinSdk
{

    public class AnXin
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public List<AnXinCarKinds> AnXinCarKindsMap()
        {

            List<AnXinCarKinds> list = new List<AnXinCarKinds>();

            list.Add(new AnXinCarKinds { CodeByAnXin = "030001", CodeByUplink = "1", Name = "交强险" });

            list.Add(new AnXinCarKinds { CodeByAnXin = "033001", CodeByUplink = "3", Name = "机动车损失保险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "030000", CodeByUplink = "4", Name = "机动车第三者责任保险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033003", CodeByUplink = "5", Name = "机动车车上人员责任保险（司机）" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033004", CodeByUplink = "6", Name = "机动车车上人员责任保险（乘客）" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033005", CodeByUplink = "7", Name = "机动车全车盗抢保险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033006", CodeByUplink = "8", Name = "玻璃单独破碎险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033014", CodeByUplink = "9", Name = "车身划痕损失险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033012", CodeByUplink = "10", Name = "机动车损失保险无法找到第三方特约险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033015", CodeByUplink = "11", Name = "发动机涉水损失险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033007", CodeByUplink = "12", Name = "自燃损失险" });
            //list.Add(new AnXinCarKinds { CodeByAnXin = "", CodeByUplink = "13", Name = "新增加设备损失险" });
            //list.Add(new AnXinCarKinds { CodeByAnXin = "", CodeByUplink = "14", Name = "修理期间费用补偿险" });
            //list.Add(new AnXinCarKinds { CodeByAnXin = "", CodeByUplink = "15", Name = "车上货物责任险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033011", CodeByUplink = "16", Name = "精神损害抚慰金责任险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033013", CodeByUplink = "17", Name = "指定修理厂险" });
            list.Add(new AnXinCarKinds { CodeByAnXin = "033018", CodeByUplink = "", Name = "不计免赔特约险" });
            return list;
        }



        public static string Pay(PayParameter parm, string cooperationCode)
        {

            Encoding code = Encoding.GetEncoding("utf-8");

            //待请求参数数组字符串
            parm.checkValue = Sign(parm, "xxx");
            string strRequestData = BuildRequest("", "utf-8", "POST", parm);//BuildRequestParaToString(sParaTemp, code);

            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = code.GetBytes(strRequestData);

            //构造请求地址
            string strUrl = "http://axwxtest.answern.com/axPay/product/authorization/";

            //请求远程HTTP
            string strResult = "";
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";

                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();

                //获取服务器返回信息
                StreamReader reader = new StreamReader(myStream, code);
                StringBuilder responseData = new StringBuilder();
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    responseData.Append(line);
                }

                //释放
                myStream.Close();

                strResult = responseData.ToString();
            }
            catch (Exception exp)
            {
                strResult = "报错：" + exp.Message;
            }

            return Sign(parm, cooperationCode);

        }

        public static string BuildRequest(string gateWay, string inputCharset, string strMethod, PayParameter parm)
        {

            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + gateWay + "_input_charset=" + inputCharset + "' method='" + strMethod.ToLower().Trim() + "'>");
            foreach (System.Reflection.PropertyInfo item in parm.GetType().GetProperties())
            {
                string name = item.Name;
                object value = item.GetValue(parm, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    sbHtml.Append("<input type='hidden' name='" + name + "' value='" + value + "'/>");
                }
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='确认' style='display:none;'></form>");
            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();

        }



        /// <summary>
        /// 对支付参数据进行MD5签名
        /// </summary>
        /// <param name="ParmList">支付参数键值列表</param>
        /// <param name="cooperationCode">合作码</param>
        /// <returns></returns>
        public static string Sign(PayParameter parm, string cooperationCode)
        {
            SortedDictionary<string, string> ParmList = new SortedDictionary<string, string>();

            foreach (System.Reflection.PropertyInfo item in parm.GetType().GetProperties())
            {
                string name = item.Name;
                object value = item.GetValue(parm, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    ParmList.Add(name, value.ToString());
                }
            }

            StringBuilder str = new StringBuilder();
            if (string.IsNullOrEmpty(cooperationCode) == false)
            {
                str.Append(cooperationCode);
            }
            int i = 0;
            foreach (KeyValuePair<string, string> kv in ParmList)
            {
                if ((kv.Key != "checkValue") && (string.IsNullOrEmpty(kv.Value) == false))
                {
                    if (i == 0)
                    {
                        str.Append(kv.Key + "=" + kv.Value);
                    }
                    else
                    {
                        str.Append("&" + kv.Key + "=" + kv.Value);
                    }
                    i++;
                }
            }

            string rtn = MD5HexDigit(str.ToString()).ToLower();

            return rtn;
        }

        public static string MD5HexDigit(string s)
        {
            byte[] md = MD5Encrypt(s);//MD5加密

            // 把密文转换成十六进制的字符串形式
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            int j = md.Length;
            char[] str = new char[j * 2];
            int k = 0;
            for (int i = 0; i < j; i++)
            {
                byte byte0 = md[i];
                str[k++] = hexDigits[byte0 >> 4 & 0xf];
                str[k++] = hexDigits[byte0 & 0xf];
            }
            string rtn = new string(str);
            return rtn;
        }



        /// <summary>
        /// 车辆信息查询（安心接口）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static VehicleInformationQueryResponse VehicleInformationQuery(VehicleInformationQueryRequest requestModel)
        {

            ServicePointManager.DefaultConnectionLimit = 120000;
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest request = null;
            HttpWebResponse SendSMSResponse = null;
            Stream dataStream = null;
            StreamReader SendSMSResponseStream = null;
            request = WebRequest.Create("https://antx11.answern.com/carchannel?comid=AEC16110001") as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 120000;
            request.AllowAutoRedirect = true;
            request.Timeout = 120000;
            request.ReadWriteTimeout = 120000;
            request.ContentType = "text/xml;charset=UTF-8";
            request.Accept = "application/xml";
            request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
            request.Proxy = null;
            request.CookieContainer = cookieContainer;


            //生成加密报文/////////////////////////////////////////////////////////////////////////////////////////////
            //byte[] bytes = CreateRequestBytes(cImgTyp, imagePath);//生成加密报文
            //加载xml模版文件
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/xmlModel/车辆信息查询请求报文.xml");//读模版
            xmlDoc.Load(xmlFilePath);
            //修改模版字段值
            XmlNode node;
            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/RequestHead/tradeTime");
            node.InnerText = requestModel.RequestHead.tradeTime;
            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/RequestHead/requestType");
            node.InnerText = requestModel.RequestHead.request_Type;
            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/RequestHead/responseType");
            node.InnerText = requestModel.RequestHead.response_Type;

            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/VehicleInformationQueryRequestMain/Channel/channelCode");
            node.InnerText = requestModel.VehicleInformationQueryRequestMain.Channel.channelCode;
            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/VehicleInformationQueryRequestMain/Channel/channelTradeCode");
            node.InnerText = requestModel.VehicleInformationQueryRequestMain.Channel.channelTradeCode;
            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/VehicleInformationQueryRequestMain/Channel/channelTradeSerialNo");
            node.InnerText = requestModel.VehicleInformationQueryRequestMain.Channel.channelTradeSerialNo;
            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/VehicleInformationQueryRequestMain/Channel/channelTradeDate");
            node.InnerText = requestModel.VehicleInformationQueryRequestMain.Channel.channelTradeDate;

            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/VehicleInformationQueryRequestMain/CarNo");
            node.InnerText = requestModel.VehicleInformationQueryRequestMain.CarNo;
            node = xmlDoc.SelectSingleNode("/VehicleInformationQueryRequest/VehicleInformationQueryRequestMain/OwnerName");

            //对报文加密
            string requestXmlStr = AESEncrypt(xmlDoc.InnerXml, "123456");
            byte[] bytes = Encoding.UTF8.GetBytes(requestXmlStr);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////


            request.ContentLength = bytes.Length;
            using (dataStream = request.GetRequestStream())
            {
                dataStream.Write(bytes, 0, bytes.Length);
            }
            SendSMSResponse = (HttpWebResponse)request.GetResponse();
            if (SendSMSResponse.StatusCode == HttpStatusCode.RequestTimeout)
            {
                if (SendSMSResponse != null)
                {
                    SendSMSResponse.Close();
                    SendSMSResponse = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
                return null;
            }
            SendSMSResponseStream = new StreamReader(SendSMSResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string strRespone = SendSMSResponseStream.ReadToEnd();
            string result = AESDecrypt(strRespone, "123456");//解密XML
            log.InfoFormat("安心-车辆信息查询接口:{0}", result);
            //把返回的MXL转换为CarModelQueryResponse实体
            VehicleInformationQueryResponse VehicleInformationQueryResponse = new VehicleInformationQueryResponse();
            using (StringReader sr = new StringReader(result))
            {
                XmlSerializer xmldes = new XmlSerializer(VehicleInformationQueryResponse.GetType());
                VehicleInformationQueryResponse = (VehicleInformationQueryResponse)xmldes.Deserialize(sr);
            }

            return VehicleInformationQueryResponse;
        }



        /// <summary>
        /// 车型查询（安心接口）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static CarModelQueryResponse CarModelQuery(CarModelQueryRequest requestModel)
        {

            ServicePointManager.DefaultConnectionLimit = 120000;
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest request = null;
            HttpWebResponse SendSMSResponse = null;
            Stream dataStream = null;
            StreamReader SendSMSResponseStream = null;
            request = WebRequest.Create("https://antx11.answern.com/carchannel?comid=AEC16110001") as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 120000;
            request.AllowAutoRedirect = true;
            request.Timeout = 120000;
            request.ReadWriteTimeout = 120000;
            request.ContentType = "text/xml;charset=UTF-8";
            request.Accept = "application/xml";
            request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
            request.Proxy = null;
            request.CookieContainer = cookieContainer;


            //生成加密报文/////////////////////////////////////////////////////////////////////////////////////////////
            //byte[] bytes = CreateRequestBytes(cImgTyp, imagePath);//生成加密报文
            //加载xml模版文件
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/xmlModel/车型查询请求报文.xml");//读模版
            xmlDoc.Load(xmlFilePath);
            //修改模版字段值
            XmlNode node;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/RequestHead/request_Type");
            node.InnerText = requestModel.RequestHead.request_Type;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/RequestHead/tradeTime");
            node.InnerText = requestModel.RequestHead.tradeTime;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/RequestHead/response_Type");
            node.InnerText = requestModel.RequestHead.response_Type;


            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/Channel/channelCode");
            node.InnerText = requestModel.CarModelQueryRequestMain.Channel.channelCode;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/Channel/channelTradeCode");
            node.InnerText = requestModel.CarModelQueryRequestMain.Channel.channelTradeCode;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/Channel/channelTradeSerialNo");
            node.InnerText = requestModel.CarModelQueryRequestMain.Channel.channelTradeSerialNo;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/Channel/channelTradeDate");
            node.InnerText = requestModel.CarModelQueryRequestMain.Channel.channelTradeDate;

            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/requestId");
            node.InnerText = requestModel.CarModelQueryRequestMain.requestId;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/productRequestType");
            node.InnerText = requestModel.CarModelQueryRequestMain.productRequestType;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/serviceType");
            node.InnerText = requestModel.CarModelQueryRequestMain.serviceType;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/pagingFlag");
            node.InnerText = requestModel.CarModelQueryRequestMain.pagingFlag;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/pageNo");
            node.InnerText = requestModel.CarModelQueryRequestMain.pageNo;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/pageSize");
            node.InnerText = requestModel.CarModelQueryRequestMain.pageSize;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/vehicleName");
            node.InnerText = requestModel.CarModelQueryRequestMain.vehicleName;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/brandId");
            node.InnerText = requestModel.CarModelQueryRequestMain.brandId;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/familyId");
            node.InnerText = requestModel.CarModelQueryRequestMain.familyId;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/gearboxType");
            node.InnerText = requestModel.CarModelQueryRequestMain.gearboxType;
            node = xmlDoc.SelectSingleNode("/CarModelQueryRequest/CarModelQueryRequestMain/engineDesc");
            node.InnerText = requestModel.CarModelQueryRequestMain.engineDesc;


            //CvrgVO CvrgVO = new CvrgVO();


            //对报文加密
            string requestXmlStr = AESEncrypt(xmlDoc.InnerXml, "123456");
            byte[] bytes = Encoding.UTF8.GetBytes(requestXmlStr);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////


            request.ContentLength = bytes.Length;
            using (dataStream = request.GetRequestStream())
            {
                dataStream.Write(bytes, 0, bytes.Length);
            }
            SendSMSResponse = (HttpWebResponse)request.GetResponse();
            if (SendSMSResponse.StatusCode == HttpStatusCode.RequestTimeout)
            {
                if (SendSMSResponse != null)
                {
                    SendSMSResponse.Close();
                    SendSMSResponse = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
                return null;
            }
            SendSMSResponseStream = new StreamReader(SendSMSResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string strRespone = SendSMSResponseStream.ReadToEnd();
            string result = AESDecrypt(strRespone, "123456");//解密XML

            //把返回的MXL转换为CarModelQueryResponse实体
            CarModelQueryResponse CarModelQueryResponse = new CarModelQueryResponse();
            using (StringReader sr = new StringReader(result.Replace("engineDesc>", "string>").Replace("gearboxTyp>", "string>")))
            {
                XmlSerializer xmldes = new XmlSerializer(CarModelQueryResponse.GetType());
                CarModelQueryResponse = (CarModelQueryResponse)xmldes.Deserialize(sr);
            }


            return CarModelQueryResponse;
        }

        /// <summary>
        /// 保费计算（安心接口）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static CarQuotePriceResponse GetPremium(CarQuotePriceRequest requestModel)
        {

            ServicePointManager.DefaultConnectionLimit = 120000;
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest request = null;
            HttpWebResponse SendSMSResponse = null;
            Stream dataStream = null;
            StreamReader SendSMSResponseStream = null;
            request = WebRequest.Create("https://antx11.answern.com/carchannel?comid=AEC16110001") as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 120000;
            request.AllowAutoRedirect = true;
            request.Timeout = 120000;
            request.ReadWriteTimeout = 120000;
            request.ContentType = "text/xml;charset=UTF-8";
            request.Accept = "application/xml";
            request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
            request.Proxy = null;
            request.CookieContainer = cookieContainer;


            //生成加密报文/////////////////////////////////////////////////////////////////////////////////////////////
            //byte[] bytes = CreateRequestBytes(cImgTyp, imagePath);//生成加密报文
            //加载xml模版文件
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/xmlModel/保费计算请求报文_混合.xml");//读模版
            xmlDoc.Load(xmlFilePath);
            //修改模版字段值
            XmlNode node;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/RequestHead/tradeTime");
            node.InnerText = requestModel.RequestHead.tradeTime;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/RequestHead/response_Type");
            node.InnerText = requestModel.RequestHead.response_Type;

            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/Channel/channelCode");
            node.InnerText = requestModel.CarQuotePriceRequestMain.Channel.channelCode;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/Channel/channelTradeCode");
            node.InnerText = requestModel.CarQuotePriceRequestMain.Channel.channelTradeCode;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/Channel/channelTradeSerialNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.Channel.channelTradeSerialNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/Channel/channelTradeDate");
            node.InnerText = requestModel.CarQuotePriceRequestMain.Channel.channelTradeDate;

            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/BaseVO/cprovince");
            node.InnerText = requestModel.CarQuotePriceRequestMain.BaseVO.cprovince;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/BaseVO/cAreaFlag");
            node.InnerText = requestModel.CarQuotePriceRequestMain.BaseVO.cAreaFlag;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/BaseVO/cCountryFlag");
            node.InnerText = requestModel.CarQuotePriceRequestMain.BaseVO.cCountryFlag;

            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/InsuredVO/cInsuredNme");
            node.InnerText = requestModel.CarQuotePriceRequestMain.InsuredVO.cInsuredNme;

            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/VhlownerVO/cOwnerNme");
            node.InnerText = requestModel.CarQuotePriceRequestMain.VhlownerVO.cOwnerNme;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/VhlownerVO/cCertfCls");
            node.InnerText = requestModel.CarQuotePriceRequestMain.VhlownerVO.cCertfCls;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/VhlownerVO/cCertfCde");
            node.InnerText = requestModel.CarQuotePriceRequestMain.VhlownerVO.cCertfCde;

            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/cProdNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.cProdNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/tInsrncBgnTm");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.tInsrncBgnTm;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/TInsrncEndTm");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.TInsrncEndTm;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cProdPlace");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cProdPlace;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cPlateNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cPlateNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cEngNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cEngNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cFrmNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cFrmNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cModelCde");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cModelCde;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cUsageCde");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cUsageCde;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cVhlTyp");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cVhlTyp;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cFstRegYm");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cFstRegYm;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cTravelAreaCde");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cTravelAreaCde;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cGlassTyp");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cGlassTyp;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cDevice1Mrk");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cDevice1Mrk;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cPlateTyp");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cPlateTyp;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cRegVhlTyp");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cRegVhlTyp;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cEcdemicMrk");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cEcdemicMrk;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cRiskDesc");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cRiskDesc;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageJQVO/VhlVO/cFleetMrk");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageJQVO.VhlVO.cFleetMrk;

            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/cProdNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.cProdNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/tInsrncBgnTm");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.tInsrncBgnTm;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/TInsrncEndTm");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.TInsrncEndTm;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/cProdNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.cProdNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/tInsrncBgnTm");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.tInsrncBgnTm;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/TInsrncEndTm");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.TInsrncEndTm;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cProdPlace");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cProdPlace;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cPlateNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cPlateNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cEngNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cEngNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cFrmNo");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cFrmNo;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cModelCde");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cModelCde;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cUsageCde");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cUsageCde;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cVhlTyp");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cVhlTyp;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cFstRegYm");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cFstRegYm;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cTravelAreaCde");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cTravelAreaCde;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cGlassTyp");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cGlassTyp;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cDevice1Mrk");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cDevice1Mrk;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cPlateTyp");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cPlateTyp;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cRegVhlTyp");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cRegVhlTyp;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cEcdemicMrk");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cEcdemicMrk;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cRiskDesc");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cRiskDesc;
            node = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/VhlVO/cFleetMrk");
            node.InnerText = requestModel.CarQuotePriceRequestMain.PackageSYVO.VhlVO.cFleetMrk;

            XmlNode cvrgList = xmlDoc.SelectSingleNode("/CarQuotePriceRequest/CarQuotePriceRequestMain/PackageSYVO/CvrgList");
            cvrgList.RemoveAll();


            foreach (CvrgVO cvrgVO in requestModel.CarQuotePriceRequestMain.PackageSYVO.CvrgList)
            {
                XmlElement CvrgVO = xmlDoc.CreateElement("CvrgVO");

                if (string.IsNullOrEmpty(cvrgVO.cCvrgNo) == false)
                {
                    XmlElement cCvrgNo = xmlDoc.CreateElement("cCvrgNo");
                    cCvrgNo.InnerText = cvrgVO.cCvrgNo;
                    CvrgVO.AppendChild(cCvrgNo);
                }
                if (string.IsNullOrEmpty(cvrgVO.nAmt) == false)
                {
                    XmlElement nAmt = xmlDoc.CreateElement("nAmt");
                    nAmt.InnerText = cvrgVO.nAmt;
                    CvrgVO.AppendChild(nAmt);
                }

                if (string.IsNullOrEmpty(cvrgVO.nLiabDaysLmt) == false)
                {
                    XmlElement nLiabDaysLmt = xmlDoc.CreateElement("nLiabDaysLmt");
                    nLiabDaysLmt.InnerText = cvrgVO.nLiabDaysLmt;
                    CvrgVO.AppendChild(nLiabDaysLmt);
                }

                if (string.IsNullOrEmpty(cvrgVO.nPerAmt) == false)
                {
                    XmlElement nPerAmt = xmlDoc.CreateElement("nPerAmt");
                    nPerAmt.InnerText = cvrgVO.nPerAmt;
                    CvrgVO.AppendChild(nPerAmt);
                }
                if (string.IsNullOrEmpty(cvrgVO.cDductMrk) == false)
                {
                    XmlElement cDductMrk = xmlDoc.CreateElement("cDductMrk");
                    cDductMrk.InnerText = cvrgVO.cDductMrk;
                    CvrgVO.AppendChild(cDductMrk);
                }
                cvrgList.AppendChild(CvrgVO);
            }

            //CvrgVO CvrgVO = new CvrgVO();


            //对报文加密
            string requestXmlStr = AESEncrypt(xmlDoc.InnerXml, "123456");
            byte[] bytes = Encoding.UTF8.GetBytes(requestXmlStr);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////


            request.ContentLength = bytes.Length;
            using (dataStream = request.GetRequestStream())
            {
                dataStream.Write(bytes, 0, bytes.Length);
            }
            SendSMSResponse = (HttpWebResponse)request.GetResponse();
            if (SendSMSResponse.StatusCode == HttpStatusCode.RequestTimeout)
            {
                if (SendSMSResponse != null)
                {
                    SendSMSResponse.Close();
                    SendSMSResponse = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
                return null;
            }
            SendSMSResponseStream = new StreamReader(SendSMSResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string strRespone = SendSMSResponseStream.ReadToEnd();
            string result = AESDecrypt(strRespone, "123456");//解密XML

            //把返回的MXL转换为CarModelQueryResponse实体
            CarQuotePriceResponse CarQuotePriceResponse = new CarQuotePriceResponse();
            using (StringReader sr = new StringReader(result))
            {
                XmlSerializer xmldes = new XmlSerializer(CarQuotePriceResponse.GetType());
                CarQuotePriceResponse = (CarQuotePriceResponse)xmldes.Deserialize(sr);
            }

            return CarQuotePriceResponse;
        }




        /// <summary>
        /// 投保（安心接口）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static ProposalGenerateResponse ProposalGenerate(ProposalGenerateRequest requestModel)
        {

            ServicePointManager.DefaultConnectionLimit = 120000;
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest request = null;
            HttpWebResponse SendSMSResponse = null;
            Stream dataStream = null;
            StreamReader SendSMSResponseStream = null;
            request = WebRequest.Create("https://antx11.answern.com/carchannel?comid=AEC16110001") as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 120000;
            request.AllowAutoRedirect = true;
            request.Timeout = 120000;
            request.ReadWriteTimeout = 120000;
            request.ContentType = "text/xml;charset=UTF-8";
            request.Accept = "application/xml";
            request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
            request.Proxy = null;
            request.CookieContainer = cookieContainer;


            //生成加密报文/////////////////////////////////////////////////////////////////////////////////////////////
            //byte[] bytes = CreateRequestBytes(cImgTyp, imagePath);//生成加密报文
            //加载xml模版文件
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/xmlModel/投保服务请求报文.xml");//读模版
            xmlDoc.Load(xmlFilePath);
            //修改模版字段值
            XmlNode node;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/RequestHead/tradeTime");
            node.InnerText = requestModel.RequestHead.tradeTime;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/RequestHead/response_Type");
            node.InnerText = requestModel.RequestHead.response_Type;

            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/Channel/channelCode");
            node.InnerText = requestModel.ProposalGenerateRequestMain.Channel.channelCode;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/Channel/channelTradeCode");
            node.InnerText = requestModel.ProposalGenerateRequestMain.Channel.channelTradeCode;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/Channel/channelTradeSerialNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.Channel.channelTradeSerialNo;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/Channel/channelTradeDate");
            node.InnerText = requestModel.ProposalGenerateRequestMain.Channel.channelTradeDate;
            //投保人信息
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cAppNme");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cAppNme;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cCertfCls");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cCertfCls;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cCertfCde");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cCertfCde;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cWorkArea");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cWorkArea;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cZipCde");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cZipCde;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cMobile");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cMobile;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cWorkDpt");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cWorkDpt;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cCustRiskRank");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cCustRiskRank;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cAppCate");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cAppCate;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/ApplicantVO/cEmail");
            node.InnerText = requestModel.ProposalGenerateRequestMain.ApplicantVO.cEmail;

            //被保险人信息
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cInsuredNme");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cInsuredNme;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cCertfCls");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cCertfCls;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cCertfCde");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cCertfCde;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cResidAddr");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cResidAddr;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cZipCde");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cZipCde;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cMobile");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cMobile;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cWorkDpt");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cWorkDpt;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cCustRiskRank");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cCustRiskRank;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cResvTxt20");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cResvTxt20;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/InsuredVO/cEmail");
            node.InnerText = requestModel.ProposalGenerateRequestMain.InsuredVO.cEmail;

            //车主信息
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/VhlownerVO/cOwnerNme");
            node.InnerText = requestModel.ProposalGenerateRequestMain.VhlownerVO.cOwnerNme;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/VhlownerVO/cCertfCls");
            node.InnerText = requestModel.ProposalGenerateRequestMain.VhlownerVO.cCertfCls;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/VhlownerVO/cCertfCde");
            node.InnerText = requestModel.ProposalGenerateRequestMain.VhlownerVO.cCertfCde;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/VhlownerVO/cResidAddr");
            node.InnerText = requestModel.ProposalGenerateRequestMain.VhlownerVO.cResidAddr;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/VhlownerVO/cZipCde");
            node.InnerText = requestModel.ProposalGenerateRequestMain.VhlownerVO.cZipCde;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/VhlownerVO/cOwnerCls");
            node.InnerText = requestModel.ProposalGenerateRequestMain.VhlownerVO.cOwnerCls;

            //交强险信息
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageJQVO/cAppNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageJQVO.cAppNo;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageJQVO/cProdNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageJQVO.cProdNo;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageJQVO/cQryCde");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageJQVO.cQryCde;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageJQVO/cPlyTyp");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageJQVO.cPlyTyp;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageJQVO/VhlVO/cAppNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageJQVO.cAppNo;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageJQVO/VhlVO/cAppValidateNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageJQVO.cAppValidateNo;


            //商业险信息
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageSYVO/cAppNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageSYVO.cAppNo;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageSYVO/cProdNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageSYVO.cProdNo;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageSYVO/cQryCde");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageSYVO.cQryCde;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageSYVO/cPlyTyp");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageSYVO.cPlyTyp;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageSYVO/VhlVO/cAppNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageSYVO.cAppNo;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/PackageSYVO/VhlVO/cAppValidateNo");
            node.InnerText = requestModel.ProposalGenerateRequestMain.PackageSYVO.cAppValidateNo;

            //配送信息
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/DeliveryVO/cSignInCnm");
            node.InnerText = requestModel.ProposalGenerateRequestMain.DeliveryVO.cSignInCnm;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/DeliveryVO/cSignInTel");
            node.InnerText = requestModel.ProposalGenerateRequestMain.DeliveryVO.cSignInTel;
            node = xmlDoc.SelectSingleNode("/ProposalGenerateRequest/ProposalGenerateRequestMain/DeliveryVO/cSendOrderAddr");
            node.InnerText = requestModel.ProposalGenerateRequestMain.DeliveryVO.cSendOrderAddr;

            //对报文加密
            string requestXmlStr = AESEncrypt(xmlDoc.InnerXml, "123456");
            byte[] bytes = Encoding.UTF8.GetBytes(requestXmlStr);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////


            request.ContentLength = bytes.Length;
            using (dataStream = request.GetRequestStream())
            {
                dataStream.Write(bytes, 0, bytes.Length);
            }
            SendSMSResponse = (HttpWebResponse)request.GetResponse();
            if (SendSMSResponse.StatusCode == HttpStatusCode.RequestTimeout)
            {
                if (SendSMSResponse != null)
                {
                    SendSMSResponse.Close();
                    SendSMSResponse = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
                return null;
            }
            SendSMSResponseStream = new StreamReader(SendSMSResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string strRespone = SendSMSResponseStream.ReadToEnd();
            string result = AESDecrypt(strRespone, "123456");//解密XML

            //把返回的MXL转换为CarModelQueryResponse实体
            ProposalGenerateResponse ProposalGenerateResponse = new ProposalGenerateResponse();
            using (StringReader sr = new StringReader(result))
            {
                XmlSerializer xmldes = new XmlSerializer(ProposalGenerateResponse.GetType());
                ProposalGenerateResponse = (ProposalGenerateResponse)xmldes.Deserialize(sr);
            }

            return ProposalGenerateResponse;
        }

        /// <summary>
        /// 承保（安心接口）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static PolicyGenerateResponse PolicyGenerate(PolicyGenerateRequest requestModel)
        {

            ServicePointManager.DefaultConnectionLimit = 120000;
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest request = null;
            HttpWebResponse SendSMSResponse = null;
            Stream dataStream = null;
            StreamReader SendSMSResponseStream = null;
            request = WebRequest.Create("https://antx11.answern.com/carchannel?comid=AEC16110001") as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 120000;
            request.AllowAutoRedirect = true;
            request.Timeout = 120000;
            request.ReadWriteTimeout = 120000;
            request.ContentType = "text/xml;charset=UTF-8";
            request.Accept = "application/xml";
            request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
            request.Proxy = null;
            request.CookieContainer = cookieContainer;


            //生成加密报文/////////////////////////////////////////////////////////////////////////////////////////////
            //byte[] bytes = CreateRequestBytes(cImgTyp, imagePath);//生成加密报文
            //加载xml模版文件
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/xmlModel/承保请求报文.xml");//读模版
            xmlDoc.Load(xmlFilePath);
            //修改模版字段值
            XmlNode node;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/RequestHead/tradeTime");
            node.InnerText = requestModel.RequestHead.tradeTime;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/RequestHead/request_Type");
            node.InnerText = requestModel.RequestHead.request_Type;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/RequestHead/response_Type");
            node.InnerText = requestModel.RequestHead.response_Type;

            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/Channel/channelCode");
            node.InnerText = requestModel.PolicyGenerateRequestMain.Channel.channelCode;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/Channel/channelTradeCode");
            node.InnerText = requestModel.PolicyGenerateRequestMain.Channel.channelTradeCode;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/Channel/channelTradeSerialNo");
            node.InnerText = requestModel.PolicyGenerateRequestMain.Channel.channelTradeSerialNo;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/Channel/channelTradeDate");
            node.InnerText = requestModel.PolicyGenerateRequestMain.Channel.channelTradeDate;

            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/cBizConsultNo");
            node.InnerText = requestModel.PolicyGenerateRequestMain.cBizConsultNo;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/cPaySequence");
            node.InnerText = requestModel.PolicyGenerateRequestMain.cPaySequence;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/cPayTyp");
            node.InnerText = requestModel.PolicyGenerateRequestMain.cPayTyp;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/cTerNo");
            node.InnerText = requestModel.PolicyGenerateRequestMain.cTerNo;
            node = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/tChargeTm");
            node.InnerText = requestModel.PolicyGenerateRequestMain.tChargeTm;

            XmlNode PayCfmVOList = xmlDoc.SelectSingleNode("/PolicyGenerateRequest/PolicyGenerateRequestMain/PayConfirmInfoList");
            PayCfmVOList.RemoveAll();
            foreach (PayConfirmInfoVO PayCfmVO in requestModel.PolicyGenerateRequestMain.PayConfirmInfoList)
            {
                XmlElement PayConfirmInfoVO = xmlDoc.CreateElement("PayConfirmInfoVO");

                if (string.IsNullOrEmpty(PayCfmVO.cAppNo) == false)
                {
                    XmlElement cAppNo = xmlDoc.CreateElement("cAppNo");
                    cAppNo.InnerText = PayCfmVO.cAppNo;
                    PayConfirmInfoVO.AppendChild(cAppNo);
                }
                if (string.IsNullOrEmpty(PayCfmVO.cAppNo) == false)
                {
                    XmlElement cAppValidateNo = xmlDoc.CreateElement("cAppValidateNo");
                    cAppValidateNo.InnerText = PayCfmVO.cAppValidateNo;
                    PayConfirmInfoVO.AppendChild(cAppValidateNo);
                }
                PayCfmVOList.AppendChild(PayConfirmInfoVO);
            }


            //对报文加密
            string requestXmlStr = AESEncrypt(xmlDoc.InnerXml, "123456");
            byte[] bytes = Encoding.UTF8.GetBytes(requestXmlStr);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////


            request.ContentLength = bytes.Length;
            using (dataStream = request.GetRequestStream())
            {
                dataStream.Write(bytes, 0, bytes.Length);
            }
            SendSMSResponse = (HttpWebResponse)request.GetResponse();
            if (SendSMSResponse.StatusCode == HttpStatusCode.RequestTimeout)
            {
                if (SendSMSResponse != null)
                {
                    SendSMSResponse.Close();
                    SendSMSResponse = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
                return null;
            }
            SendSMSResponseStream = new StreamReader(SendSMSResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string strRespone = SendSMSResponseStream.ReadToEnd();
            string result = AESDecrypt(strRespone, "123456");//解密XML

            //把返回的MXL转换为CarModelQueryResponse实体
            PolicyGenerateResponse PolicyGenerateResponse = new PolicyGenerateResponse();
            using (StringReader sr = new StringReader(result))
            {
                XmlSerializer xmldes = new XmlSerializer(PolicyGenerateResponse.GetType());
                PolicyGenerateResponse = (PolicyGenerateResponse)xmldes.Deserialize(sr);
            }

            return PolicyGenerateResponse;
        }




        /// <summary>
        /// 投保单、保单查询（安心接口）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static CarBusinessDetailInfoQueryResponse CarBusinessDetailInfoQuery(CarBusinessDetailInfoQueryRequest requestModel)
        {

            ServicePointManager.DefaultConnectionLimit = 120000;
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest request = null;
            HttpWebResponse SendSMSResponse = null;
            Stream dataStream = null;
            StreamReader SendSMSResponseStream = null;
            request = WebRequest.Create("https://antx11.answern.com/carchannel?comid=AEC16110001") as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 120000;
            request.AllowAutoRedirect = true;
            request.Timeout = 120000;
            request.ReadWriteTimeout = 120000;
            request.ContentType = "text/xml;charset=UTF-8";
            request.Accept = "application/xml";
            request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
            request.Proxy = null;
            request.CookieContainer = cookieContainer;


            //生成加密报文/////////////////////////////////////////////////////////////////////////////////////////////
            //byte[] bytes = CreateRequestBytes(cImgTyp, imagePath);//生成加密报文
            //加载xml模版文件
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/xmlModel/保单详情查询请求报文.xml");//读模版
            xmlDoc.Load(xmlFilePath);
            //修改模版字段值
            XmlNode node;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/RequestHead/tradeTime");
            node.InnerText = requestModel.RequestHead.tradeTime;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/RequestHead/request_Type");
            node.InnerText = requestModel.RequestHead.request_Type;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/RequestHead/response_Type");
            node.InnerText = requestModel.RequestHead.response_Type;

            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/Channel/channelCode");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.Channel.channelCode;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/Channel/channelTradeCode");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.Channel.channelTradeCode;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/Channel/channelTradeSerialNo");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.Channel.channelTradeSerialNo;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/Channel/channelTradeDate");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.Channel.channelTradeDate;

            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/cPlyNo");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.cPlyNo;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/cAppNo");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.cAppNo;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/cInsuredNme");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.cInsuredNme;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/cCertfCls");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.cCertfCls;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/cCertfCde");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.cCertfCde;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/cPlateNo");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.cPlateNo;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/cFrmNo");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.cFrmNo;
            node = xmlDoc.SelectSingleNode("/CarBusinessDetailInfoQueryRequest/CarBusinessDetailInfoQueryRequestMain/cAppTyp");
            node.InnerText = requestModel.CarBusinessDetailInfoQueryRequestMain.cAppTyp;



            //对报文加密
            string requestXmlStr = AESEncrypt(xmlDoc.InnerXml, "123456");
            byte[] bytes = Encoding.UTF8.GetBytes(requestXmlStr);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////


            request.ContentLength = bytes.Length;
            using (dataStream = request.GetRequestStream())
            {
                dataStream.Write(bytes, 0, bytes.Length);
            }
            SendSMSResponse = (HttpWebResponse)request.GetResponse();
            if (SendSMSResponse.StatusCode == HttpStatusCode.RequestTimeout)
            {
                if (SendSMSResponse != null)
                {
                    SendSMSResponse.Close();
                    SendSMSResponse = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
                return null;
            }
            SendSMSResponseStream = new StreamReader(SendSMSResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string strRespone = SendSMSResponseStream.ReadToEnd();
            string result = AESDecrypt(strRespone, "123456");//解密XML

            //把返回的MXL转换为CarBusinessDetailInfoQueryResponse实体
            CarBusinessDetailInfoQueryResponse CarBusinessDetailInfoQueryResponse = new CarBusinessDetailInfoQueryResponse();
            using (StringReader sr = new StringReader(result))
            {
                XmlSerializer xmldes = new XmlSerializer(CarBusinessDetailInfoQueryResponse.GetType());
                CarBusinessDetailInfoQueryResponse = (CarBusinessDetailInfoQueryResponse)xmldes.Deserialize(sr);
            }

            return CarBusinessDetailInfoQueryResponse;
        }



        public static IdentityCardVO GetIdentityCardByImageBase64(string imageBase64)
        {
            IdentityCardVO model = new IdentityCardVO();

            try
            {
                imageBase64 = HttpUtility.UrlEncode(imageBase64, Encoding.UTF8);

                //log.Info("安心接口imageBase64:" + imageBase64);
                string xml = GetCertInfoByImageBase64("identityCard", imageBase64);
                //log.Info("安心接口:" + xml);
                StringBuilder sb = new StringBuilder();

                //sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                //sb.Append("<CertificateIdentificationSystemResponse>");
                //sb.Append("<ResponseHead>");
                //sb.Append("<responseCode>0</responseCode>");
                //sb.Append("<requestType>01</requestType>");
                //sb.Append("<errorCode></errorCode>");
                //sb.Append("<errorMessage></errorMessage>");
                //sb.Append("<esbCode>00</esbCode>");
                //sb.Append("<esbMessage>成功</esbMessage>");
                //sb.Append("<signData></signData>");
                //sb.Append("</ResponseHead>");
                //sb.Append("<CertificateIdentificationSystemResponseMain>");
                //sb.Append("<!-- 证件信息  -->");
                //sb.Append("<LicenceInfoVO>");
                //sb.Append("<!-- 识别类型 -->");
                //sb.Append("<type>identityCard</type>");
                //sb.Append("<!-- 错误信息 -->");
                //sb.Append("<errorMessage></errorMessage>");
                //sb.Append("<IdentityCardVO>");
                //sb.Append("<!-- 住址 -->");
                //sb.Append("<address>湖北省洪湖市曹市镇天井村3-27</address>");
                //sb.Append("<!-- 出生日期-->");
                //sb.Append("<birthday>1985年9月8日</birthday>");
                //sb.Append("<!-- 身份证号 -->");
                //sb.Append("<idNumber>421083198509081218</idNumber>");
                //sb.Append("<!-- 姓名 -->");
                //sb.Append("<name>李佳</name>");
                //sb.Append("<!-- 民族 -->");
                //sb.Append("<people>汉</people>");
                //sb.Append("<!-- 性别 -->");
                //sb.Append("<sex>男</sex>");
                //sb.Append("<!-- 证件类型 -->");
                //sb.Append("<type>第二代身份证</type>");
                //sb.Append("<!-- 发证机关 -->");
                //sb.Append("<issueAuthority></issueAuthority>");
                //sb.Append("<!-- 有效期 -->");
                //sb.Append("<validity></validity>");
                //sb.Append("</IdentityCardVO>");
                //sb.Append("</LicenceInfoVO>");
                //sb.Append("</CertificateIdentificationSystemResponseMain>");
                //sb.Append("</CertificateIdentificationSystemResponse>");


                //string xml = sb.ToString();


                IdentityCardSystemResponse response = XmlUtils.Deserialize(typeof(IdentityCardSystemResponse), xml) as IdentityCardSystemResponse;

                log.Info(Newtonsoft.Json.JsonConvert.SerializeObject(response));

                if (response == null)
                    return model;
                if (response.CertificateIdentificationSystemResponseMain == null)
                    return model;
                if (response.CertificateIdentificationSystemResponseMain.LicenceInfoVO == null)
                    return model;

                //  Console.Write(string.Format("名字:{0},年龄:{1}", stu2.Name, stu2.Age));
                //log.Info("姓名" + model.name);

                if (response.CertificateIdentificationSystemResponseMain.LicenceInfoVO.IdentityCardVO == null)
                    return model;

                model = response.CertificateIdentificationSystemResponseMain.LicenceInfoVO.IdentityCardVO;

                return model;
            }
            catch (Exception ex)
            {
                log.Error("调用安心接口报错", ex);
                return model;
            }




        }


        public static DrivingLicenceVO GetDrivingLicenceByImageBase64(string imageBase64)
        {
            DrivingLicenceVO model = new DrivingLicenceVO();

            try
            {
                imageBase64 = HttpUtility.UrlEncode(imageBase64, Encoding.UTF8);

                string xml = GetCertInfoByImageBase64("drivingLicence", imageBase64);
                log.Info("安心接口:" + xml);
                StringBuilder sb = new StringBuilder();

                //sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                //sb.Append("<CertificateIdentificationSystemResponse>");
                //sb.Append("<ResponseHead>");
                //sb.Append("<responseCode>0</responseCode>");
                //sb.Append("<requestType>01</requestType>");
                //sb.Append("<errorCode></errorCode>");
                //sb.Append("<errorMessage></errorMessage>");
                //sb.Append("<esbCode>00</esbCode>");
                //sb.Append("<esbMessage>成功</esbMessage>");
                //sb.Append("<signData></signData>");
                //sb.Append("</ResponseHead>");
                //sb.Append("<CertificateIdentificationSystemResponseMain>");
                //sb.Append("<!-- 证件信息  -->");
                //sb.Append("<LicenceInfoVO>");
                //sb.Append("<!-- 识别类型 -->");
                //sb.Append("<type>identityCard</type>");
                //sb.Append("<!-- 错误信息 -->");
                //sb.Append("<errorMessage></errorMessage>");
                //sb.Append("<IdentityCardVO>");
                //sb.Append("<!-- 住址 -->");
                //sb.Append("<address>湖北省洪湖市曹市镇天井村3-27</address>");
                //sb.Append("<!-- 出生日期-->");
                //sb.Append("<birthday>1985年9月8日</birthday>");
                //sb.Append("<!-- 身份证号 -->");
                //sb.Append("<idNumber>421083198509081218</idNumber>");
                //sb.Append("<!-- 姓名 -->");
                //sb.Append("<name>李佳</name>");
                //sb.Append("<!-- 民族 -->");
                //sb.Append("<people>汉</people>");
                //sb.Append("<!-- 性别 -->");
                //sb.Append("<sex>男</sex>");
                //sb.Append("<!-- 证件类型 -->");
                //sb.Append("<type>第二代身份证</type>");
                //sb.Append("<!-- 发证机关 -->");
                //sb.Append("<issueAuthority></issueAuthority>");
                //sb.Append("<!-- 有效期 -->");
                //sb.Append("<validity></validity>");
                //sb.Append("</IdentityCardVO>");
                //sb.Append("</LicenceInfoVO>");
                //sb.Append("</CertificateIdentificationSystemResponseMain>");
                //sb.Append("</CertificateIdentificationSystemResponse>");


                //   string xml = sb.ToString();


                DrivingLicenceSystemResponse response = XmlUtils.Deserialize(typeof(DrivingLicenceSystemResponse), xml) as DrivingLicenceSystemResponse;

                log.Info(Newtonsoft.Json.JsonConvert.SerializeObject(response));

                if (response == null)
                    return model;
                if (response.CertificateIdentificationSystemResponseMain == null)
                    return model;
                if (response.CertificateIdentificationSystemResponseMain.LicenceInfoVO == null)
                    return model;


                if (response.CertificateIdentificationSystemResponseMain.LicenceInfoVO.DrivingLicenceVO == null)
                    return model;

                model = response.CertificateIdentificationSystemResponseMain.LicenceInfoVO.DrivingLicenceVO;

                return model;
            }
            catch (Exception ex)
            {
                log.Error("调用安心接口报错", ex);
                return model;
            }


        }

        //public static void GetDrivingLicenceByImageBase64(string imageBase64)
        //{
        //    GetCertInfoByImageBase64("drivingLicence", imageBase64);
        //}


        /// <summary>
        /// 证件识别（安心接口）
        /// </summary>
        /// <param name="cImgTyp">证件类型：identityCard身份证;drivingLicence驾驶证;bankcard银行卡</param>
        /// <param name="imagePath">图像路径</param>
        /// <returns></returns>
        public static string GetCertInfoByImageBase64(string cImgTyp, string imageBase64)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            string result = "";
            try
            {
                ServicePointManager.DefaultConnectionLimit = 120000;
                CookieContainer cookieContainer = new CookieContainer();
                HttpWebRequest request = null;
                HttpWebResponse SendSMSResponse = null;
                Stream dataStream = null;
                StreamReader SendSMSResponseStream = null;
                request = WebRequest.Create("https://antx11.answern.com/carchannel?comid=AEC16110001") as HttpWebRequest;
                request.Method = "POST";
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 120000;
                request.AllowAutoRedirect = true;
                request.Timeout = 120000;
                request.ReadWriteTimeout = 120000;
                request.ContentType = "text/xml;charset=UTF-8";
                request.Accept = "application/xml";
                request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
                request.Proxy = null;
                request.CookieContainer = cookieContainer;
                byte[] bytes = CreateRequestBytesByImageBase64(cImgTyp, imageBase64);//生成加密报文
                request.ContentLength = bytes.Length;
                using (dataStream = request.GetRequestStream())
                {
                    dataStream.Write(bytes, 0, bytes.Length);
                }
                SendSMSResponse = (HttpWebResponse)request.GetResponse();
                if (SendSMSResponse.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    if (SendSMSResponse != null)
                    {
                        SendSMSResponse.Close();
                        SendSMSResponse = null;
                    }
                    if (request != null)
                    {
                        request.Abort();
                    }
                    return null;
                }
                SendSMSResponseStream = new StreamReader(SendSMSResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                string strRespone = SendSMSResponseStream.ReadToEnd();
                //log.InfoFormat("图片识别返回结果:{0}", strRespone);
                result = AESDecrypt(strRespone, "123456");//解密XML
                log.InfoFormat("图片识别返回解密结果:{0}", result);
            }
            catch (Exception ex)
            {
                log.Error("调用安心接口报错", ex);
            }

            return result;

        }

        public static string GetCertInfoByImagePath(string cImgTyp, string imagePath)
        {

            string base64Image = ImgToBase64String(imagePath);

            return GetCertInfoByImageBase64(cImgTyp, base64Image);



        }


        /// <summary>
        /// 生成图文识别加密报文
        /// </summary>
        /// <param name="cImgTyp">证件类型：identityCard身份证;drivingLicence驾驶证;bankcard银行卡</param>
        /// <param name="imagePath">图像路径</param>
        /// <returns></returns>
        public static byte[] CreateRequestBytesByImageBase64(string cImgTyp, string imageBase64)
        {
            //加载xml模版文件
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/xmlModel/图文识别.xml");//读模版
            xmlDoc.Load(xmlFilePath);
            //读取图片


            string base64Image = imageBase64;

            //修改图片内容
            XmlNode imageContentNode = xmlDoc.SelectSingleNode("/CertificateIdentificationSystemRequest/CertificateIdentificationSystemRequestMain/ImageVO/imageContent");
            if (imageContentNode != null)
            {
                imageContentNode.InnerText = base64Image;
            }
            //修改改证件类型
            XmlNode cImgTypNode = xmlDoc.SelectSingleNode("/CertificateIdentificationSystemRequest/CertificateIdentificationSystemRequestMain/ImageVO/cImgTyp");
            if (cImgTypNode != null)
            {
                cImgTypNode.InnerText = cImgTyp;
            }
            //对报文加密
            string requestXmlStr = AESEncrypt(xmlDoc.InnerXml, "123456");
            return Encoding.UTF8.GetBytes(requestXmlStr);
        }


        public static byte[] CreateRequestBytesByImagePath(string cImgTyp, string imagePath)
        {

            string base64Image = ImgToBase64String(imagePath);

            return CreateRequestBytesByImageBase64(cImgTyp, base64Image);

        }

        /// <summary>
        /// 读取图片文件并转换为Base64
        /// </summary>
        /// <param name="Imagefilename">图片文件路径</param>
        /// <returns></returns>
        public static string ImgToBase64String(string Imagefilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefilename);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return HttpUtility.UrlEncode(Convert.ToBase64String(arr), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>  
        /// AES加密(无向量)  
        /// </summary>  
        /// <param name="plainBytes">被加密的明文</param>  
        /// <param name="key">密钥</param>  
        /// <returns>密文</returns>  
        public static string AESEncrypt(String Data, String Key)
        {
            MemoryStream mStream = new MemoryStream();
            RijndaelManaged aes = new RijndaelManaged();
            byte[] plainBytes = Encoding.UTF8.GetBytes(Data);
            Byte[] bKey = new Byte[16];//32
            Array.Copy(MD5Encrypt(Key), bKey, bKey.Length);//对密钥进行MD5加密（长度为16字节)
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>  
        /// AES解密(无向量)  
        /// </summary>  
        /// <param name="encryptedBytes">被加密的明文</param>  
        /// <param name="key">密钥</param>  
        /// <returns>明文</returns>  
        public static string AESDecrypt(String Data, String Key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(Data);
            Byte[] bKey = new Byte[16];
            Array.Copy(MD5Encrypt(Key), bKey, bKey.Length);//对密钥进行MD5加密（长度为16字节)
            MemoryStream mStream = new MemoryStream(encryptedBytes);
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + Key.Length];

                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + Key.Length);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns>16字节</returns>
        public static byte[] MD5Encrypt(string data)
        {
            byte[] temp = Encoding.UTF8.GetBytes(data);    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(temp);
            return result;
        }

    }
}
