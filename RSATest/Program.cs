using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSATest
{
    class Program
    {
        static void Main(string[] args)
        {

            //RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            //string a = provider.ToXmlString(false);
            //string b = provider.ToXmlString(true); 
            //RSALibrary r = new RSALibrary();
            //r.RSAKey(@"D:\",@"D:\");

            //string s =EncryptUtils.MD5Encrypt("沁asddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
            //Console.WriteLine(s);

            StringBuilder postData = new StringBuilder();
            postData.Append("username=chingment&");
            postData.Append("password=123456&");

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            string strPostData = postData.ToString();
            foreach (string ss in strPostData.Split('&'))
            {
                if (!string.IsNullOrEmpty(ss))
                {
                    string key = ss.Split('=')[0];
                    string value = ss.Split('=')[1];
                    parameters.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            DateTime now = DateTime.Now.AddMinutes(-9);
         
            int timesSpan =(int)(now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

            Signature s = new Signature(parameters, "6ZB97cdVz211O08EKZ6yriAYrHXFBowC", timesSpan);

            string sign = s.Compute();

            postData.Append("sign=" + sign + "&");
            postData.Append("timestamp=" + timesSpan);
   
            Console.WriteLine(postData);

            string sign1 = Valid(postData.ToString());
            Console.WriteLine(sign1);
            Console.ReadLine();


        }



        public static string Valid(string data)
        {

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            int timestamp = 0;
            string sign = "";
            foreach (string ss in data.Split('&'))
            {
                if (!string.IsNullOrEmpty(ss))
                {
                    string key = ss.Split('=')[0];
                    string value = ss.Split('=')[1];

                    if (key == "username" || key == "password")
                    {
                        parameters.Add(new KeyValuePair<string, string>(key, value));
                    }
                    else if (key == "timestamp")
                    {
                        timestamp = int.Parse(value);
                    }
                    else if (key == "sign")
                    {
                        sign = value;
                    }
                }
            }


            Signature a = new Signature(parameters, "6ZB97cdVz211O08EKZ6yriAYrHXFBowC", timestamp);
            string sign1 = a.ComputeByTimestamp();

            return sign1;

        }

    }
}
