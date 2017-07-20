using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace ArcGIS_Poisonous
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Debug.WriteLine("ArcGIS_Poisonous Program Start. ");

            //ArcGIS开始绑定
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);

            DateTime currentTime = DateTime.Now;
            string str = currentTime.Year + "-" + currentTime.Month + "-" + currentTime.Day + " " +
                currentTime.Hour + ":" + currentTime.Minute + ":" + currentTime.Second ;
            Debug.WriteLine("Start Time: "+str);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ArcGisPoisonous());
        }
    }
}
