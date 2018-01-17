using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace WebAppApi.Controllers
{
    public class HttpRequestClient
    {
        public static void Send(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));//设定要响应的数据格式
                using (var content = new MultipartFormDataContent())//表明是通过multipart/form-data的方式上传数据
                {

                    NameValueCollection collection = new NameValueCollection();

                    collection.Add("myname", "sda");

                    HashSet<string> filess = new HashSet<string>();

                    filess.Add("D:\\a.png");
                    var formDatas = HttpRequestClient.GetFormDataByteArrayContent(collection);//获取键值集合对应的ByteArrayContent集合
                    var files = HttpRequestClient.GetFileByteArrayContent(filess);//获取文件集合对应的ByteArrayContent集合
                    Action<List<ByteArrayContent>> act = (dataContents) =>
                    {//声明一个委托，该委托的作用就是将ByteArrayContent集合加入到MultipartFormDataContent中
                        foreach (var byteArrayContent in dataContents)
                        {
                            content.Add(byteArrayContent);
                        }
                    };
                    act(formDatas);//执行act
                    act(files);//执行act
                    try
                    {
                        var result = client.PostAsync(url, content).Result;//post请求

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 获取文件集合对应的ByteArrayContent集合
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private static List<ByteArrayContent> GetFileByteArrayContent(HashSet<string> files)
        {
            List<ByteArrayContent> list = new List<ByteArrayContent>();
            foreach (var file in files)
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(file));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Path.GetFileName(file),
                    Name = "SS"
                };
                list.Add(fileContent);
            }
            return list;
        }
        /// <summary>
        /// 获取键值集合对应的ByteArrayContent集合
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private static List<ByteArrayContent> GetFormDataByteArrayContent(NameValueCollection collection)
        {
            List<ByteArrayContent> list = new List<ByteArrayContent>();
            foreach (var key in collection.AllKeys)
            {
                var dataContent = new ByteArrayContent(Encoding.UTF8.GetBytes(collection[key]));
                dataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = key
                };
                list.Add(dataContent);
            }
            return list;
        }
    }
}