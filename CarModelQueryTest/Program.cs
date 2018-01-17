using AnXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarModelQueryTest
{
    class Program
    {
        delegate TimeSpan BoilingDelegate();

        static TimeSpan Boil()
        {
            Console.WriteLine("水壶：开始烧水...");
            Thread.Sleep(6000);
            Console.WriteLine("水壶：水已经烧开了！");
            return TimeSpan.MinValue;
        }

        static void BoilingFinishedCallback(IAsyncResult result)
        {
            AsyncResult asyncResult = (AsyncResult)result;
            BoilingDelegate del = (BoilingDelegate)asyncResult.AsyncDelegate;
            del.EndInvoke(result);
            Console.WriteLine("小文：将热水灌到热水瓶");
            Console.WriteLine("小文：继续整理家务");

        }

        static void Main(string[] args)
        {

            CarModelQueryRequest CarModelQueryModel = new CarModelQueryRequest();

            CarModelQueryModel.CarModelQueryRequestMain.requestId = "VEH_02";//保请求标识
            CarModelQueryModel.CarModelQueryRequestMain.productRequestType = "F";//请求类型:D-直接查找  F-模糊查找
            CarModelQueryModel.CarModelQueryRequestMain.serviceType = "C";//业务类型： C-乘用车  A-传统车型
            CarModelQueryModel.CarModelQueryRequestMain.pagingFlag = "F";//分页类型 ：T-分页   F-不分页
            //CarModelQueryModel.CarModelQueryRequestMain.pageNo = "1";//页码
            //CarModelQueryModel.CarModelQueryRequestMain.pageSize = "500";//每页显示数量
            CarModelQueryModel.CarModelQueryRequestMain.vehicleName = "";//车型名别克牌SGM7243ATA*******************
            //CarModelQueryModel.CarModelQueryRequestMain.brandId = "";//品牌ID
            //CarModelQueryModel.CarModelQueryRequestMain.familyId = "";//车系ID
            //CarModelQueryModel.CarModelQueryRequestMain.gearboxType = "";//驱动型式
            //CarModelQueryModel.CarModelQueryRequestMain.engineDesc = "";//发动机描述
            //CarModelQueryResponse rtnModel = AnXin.CarModelQuery(CarModelQueryModel);
            ///rtn.Data = rtnModel;

            BoilingDelegate d = new BoilingDelegate(Boil);
            IAsyncResult result = d.BeginInvoke(BoilingFinishedCallback, null);

            Console.WriteLine("小文：开始整理家务...");
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("小文：整理第{0}项家务...", i + 1);
                Thread.Sleep(1000);
            }


        }
    }
}
