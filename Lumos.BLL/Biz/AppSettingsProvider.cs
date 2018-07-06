using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class AppSettingsProvider
    {

        public string ServiceHotline
        {
            get
            {
                return "020-36824118";
            }
        }

        public string MerchantName
        {
            get
            {
                return "广州市好易联科技有限公司";
            }
        }

        public bool IsTest
        {
            get
            {
                string istest = ConfigurationManager.AppSettings["custom:IsTest"];
                if (istest == null)
                {
                    return false;
                }

                if (istest.ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int CutOffIntervalByMinutes
        {
            get
            {
                string blacklistPath = ConfigurationManager.AppSettings["custom:CutOffIntervalByMinutes"];

                if (blacklistPath == null)
                    return 60;

                return int.Parse(blacklistPath);
            }
        }

        public int RenewalPriorNoticeDays
        {
            get
            {
                return 30;
            }
        }

        public string WebAppServerUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["custom:WebAppServerUrl"];

                if (url == null)
                    return "";

                return url;
            }
        }

        public string WebApiServerUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["custom:WebApiServerUrl"];

                if (url == null)
                    return "";

                return url;
            }
        }


        public Dictionary<string, string> LogPath
        {
            get
            {
                Dictionary<string, string> d = new Dictionary<string, string>();

                string logPath = ConfigurationManager.AppSettings["custom:LogPath"];

                if (logPath != null)
                {
                    string[] arr_logPath = logPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var m in arr_logPath)
                    {
                        string[] data = m.Split('=');
                        d.Add(data[0], data[1]);
                    }
                }

                return d;
            }
        }

    }
}
