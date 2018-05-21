using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StarPaySdk
{
    public static class CommonUtil
    {
        public static string MakeMd5Sign(SortedDictionary<string, string> m_values, string key)
        {
            string buff = "";
            foreach (KeyValuePair<string, string> pair in m_values)
            {
                buff += pair.Value;
            }
            buff += key;
            string str = string.Concat(buff.OrderBy(c => c));

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            System.Security.Cryptography.MD5 md5;
            byte[] bytesSrc;
            byte[] result;
            StringBuilder sb = new StringBuilder();
            bytesSrc = encoding.GetBytes(buff);
            md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            result = md5.ComputeHash(bytesSrc);
            for (int i = 0; i < result.Length; i++)
            {
                sb.AppendFormat("{0:x2}", result[i]);
            }

            string s = sb.ToString();
            return s;

        }
    }
}
