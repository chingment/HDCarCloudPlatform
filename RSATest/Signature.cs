using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSATest
{
    public class Signature
    {
        ///// <summary>
        ///// 服务器可以接受的调用方时间戳与服务器时间的时差，单位分
        ///// </summary>
        public const short MaxTimeDiff = 5;

        /// <summary>
        /// 时间戳计算的基准
        /// </summary>
        private readonly DateTime TimestampBase = new DateTime(1970, 1, 1);

        /// <summary>
        /// 用于构建签名的有序参数列表
        /// </summary>
        public List<KeyValuePair<string, string>> Parameters;

        /// <summary>
        /// 时间戳，本地时间
        /// </summary>
        public int Timestamp;

        /// <summary>
        /// 用于混淆签名内容的，调用方唯一的128位字符串
        /// </summary>
        public string Salt;


        private int GetTimestamplerance(int timestamp)
        {
            DateTime dt = TimestampBase.AddSeconds(timestamp);
            DateTime now = DateTime.Now;
            Console.WriteLine(now);
            var ts = now - dt;
            if (System.Math.Abs(ts.TotalMinutes) > MaxTimeDiff)
            {
                timestamp = (int)(now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
                this.IsTimeOut = true;
            }
            return timestamp;
        }


        public Signature(List<KeyValuePair<string, string>> parameters, string salt)
        {
            if (parameters == null || parameters.Count == 0)
                throw new ArgumentOutOfRangeException("parameters");

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentOutOfRangeException("salt");

            this.Parameters = parameters;

            this.Salt = salt;
        }

        public Signature(List<KeyValuePair<string, string>> parameters, string salt, int timestamp)
        {
            if (parameters == null || parameters.Count == 0)
                throw new ArgumentOutOfRangeException("parameters");

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentOutOfRangeException("salt");

            this.Parameters = parameters;

            this.Timestamp = timestamp;
 
            this.Salt = salt;
        }


        private string BuildMaterial()
        {
            var sb = new StringBuilder();
            var diff = TimestampBase.AddSeconds(Timestamp) - TimestampBase;
            var seconds = diff.Days * 24 * 60 * 60 + diff.Hours * 60 * 60 + diff.Minutes * 60 + diff.Seconds;

            foreach (KeyValuePair<string, string> kvp in this.Parameters)
            {
                sb.AppendFormat("{0}={1}&", kvp.Key, kvp.Value);
            }

            sb.AppendFormat("salt={0}&", Salt);
            sb.AppendFormat("timestamp={0}", seconds.ToString());
            return sb.ToString();
        }

        private string ComputeHash(string material)
        {
            if (string.IsNullOrEmpty(material))
                throw new ArgumentOutOfRangeException();

            var input = Encoding.UTF8.GetBytes(material);
            var hash = SHA256Managed.Create().ComputeHash(input);
            var output = Convert.ToBase64String(hash);

            return output;
        }

        public string Compute()
        {
            var material = this.BuildMaterial();
            var hash = this.ComputeHash(material);
            return hash;
        }




        public string ComputeByTimestamp()
        {
            this.Timestamp = GetTimestamplerance(this.Timestamp);
            var material = this.BuildMaterial();
            var hash = this.ComputeHash(material);
            return hash;
        }


        public bool IsTimeOut { get; set; }

    }
}
