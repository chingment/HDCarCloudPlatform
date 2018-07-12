using log4net;
using Lumos;
using Lumos.BLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace TaskCarInsQueryInquiry
{
    class Program
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DllImport("User32.dll", EntryPoint = "ShowWindow")]
        private static extern bool ShowWindow(IntPtr hWnd, int type);

        static void Main(string[] args)
        {
            //log4net.Config.XmlConfigurator.Configure();
            LogUtil.SetTrackId();
            LogUtil.Info("程序开始运行");
            ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 2);//隐藏本dos窗体, 0: 后台执行；1:正常启动；2:最小化到任务栏；3:最大化

            try
            {
                string taskProvider = System.Configuration.ConfigurationManager.AppSettings["custom:TaskProvider"];
                BizFactory.Launcher.Launch(taskProvider);
            }
            catch (Exception ex)
            {
                LogUtil.Error("",ex);
            }

            LogUtil.Info("程序结束运行\r\n");
        }
    }
}
