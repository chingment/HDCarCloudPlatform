using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MinShunPaySdk
{
    public static class TdsPayUtil
    {
        public static string UrlEncode(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str)
            {
                if (HttpUtility.UrlEncode(c.ToString()).Length > 1)
                {
                    builder.Append(HttpUtility.UrlEncode(c.ToString()).ToUpper());
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        public static string GetSignData(Dictionary<string, string> dic)
        {
            string resStr = "";

            List<KeyValuePair<string, string>> kvs = dic.ToList();



            kvs.Sort(delegate (KeyValuePair<string, string> pair1, KeyValuePair<string, string> pair2)
            {
                return pair1.Key.CompareTo(pair2.Key);
            });


            foreach (var kv in kvs)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    resStr = resStr + TdsPayUtil.UrlEncode(kv.Value);
                }
            }



            //// 第二步：把所有参数名和参数值串在一起
            //StringBuilder query = new StringBuilder("");  //签名字符串
            //StringBuilder queryStr = new StringBuilder(""); //url参数
            //if (parames == null || parames.Count == 0)
            //    return "";

            //while (kvs.MoveNext())
            //{
            //    string key = dem.Current.Key;
            //    string value = dem.Current.Value;
            //    if (!string.IsNullOrEmpty(key))
            //    {
            //        queryStr.Append("&").Append(key).Append("=").Append(value);
            //    }
            //}

            return resStr;
        }

        public static string GetShaSign(string material)
        {
            string strResult = "";

            //Create
            System.Security.Cryptography.SHA1 sha = System.Security.Cryptography.SHA1.Create();
            byte[] bytResult = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(material));
            for (int i = 0; i < bytResult.Length; i++)
            {
                strResult = strResult + bytResult[i].ToString("x2");
            }
            return strResult;
        }
    }
}
