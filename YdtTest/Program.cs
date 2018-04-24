using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YdtSdk;

namespace YdtTest
{
    class Program
    {
        static void Main(string[] args)
        {
           // Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail("http://www.cnblogs.com/", 800, 1200, 800, 1200); //宽高根据要获取快照的网页决定
            //m_Bitmap.Save("render_img.bmp", System.Drawing.Imaging.ImageFormat.Bmp); //图片格式可以自由控制


            YdtApi c = new YdtApi();
            YdtToken ydtToken = new YdtToken("hylian", "hylian_2018");
            var ydtTokenResult = c.DoGet(ydtToken);

            // ydtTokenResult


            YdtEmLogin ydtEmLogin = new YdtEmLogin(ydtTokenResult.data.token, "15012405333", "7c4a8d09ca3762af61e59520943dc26494f8941b");
            var ydtEmLoginResult = c.DoGet(ydtEmLogin);


            YdtInscarCar ydtInscarCar = new YdtInscarCar(ydtTokenResult.data.token, ydtEmLoginResult.data.session, "SQR7080T", "LVVDB22A96D041568", "2006-01-18 00:00:00");
            var ydtInscarCarResult = c.DoGet(ydtInscarCar);


            string fileUrl = "https://file.ins-uplink.cn/upload/CarInsuranceCompany/gzrbbx.png";


            YdtUpload ydtUpdate = new YdtUpload(ydtTokenResult.data.token, ydtEmLoginResult.data.session, "1", fileUrl);
            var ydtUpdateResult = c.DoPostFile(ydtUpdate, Path.GetFileName(fileUrl));



     

        }
    }
}
