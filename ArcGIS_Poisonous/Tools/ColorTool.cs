using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Display;

namespace ArcGIS_Poisonous.Tools
{
    /// <summary>
    /// 颜色处理工具类
    /// </summary>
    public  class ColorTool
    {

        /// <summary>
        /// 输入RGB值，获得IRgbColor的值
        /// </summary>
        /// <param name="intR">Red Value</param>
        /// <param name="intG">Green Value</param>
        /// <param name="intB">Green Value</param>
        /// <returns>IRgbColor Value</returns>
        public static IRgbColor GetRgbColor(int intR, int intG, int intB)
        {
            IRgbColor rgbColor = null;
            if (intR < 0 || intR > 255||intG < 0 || intG > 255||intB < 0 || intB > 255)
            {
                return rgbColor;
            }
            rgbColor = new RgbColorClass();
            rgbColor.Red = intR;
            rgbColor.Green = intG;
            rgbColor.Blue = intB;
            return rgbColor;
        }

        /// <summary>
        /// 输入HSV值，获取IHsvColor型值
        /// </summary>
        /// <param name="intH">Hue</param>
        /// <param name="intS">Saturation</param>
        /// <param name="intV">Value</param>
        /// <returns>IHsvColor</returns>
        public static IHsvColor GetHsvColor(int intH, int intS, int intV) 
        {
            IHsvColor hsvColor = null;
            if (intH < 0 || intH > 360 || intS < 0 || intS > 100 || intV < 0 || intV > 100)
            {
                return hsvColor;
            }
            hsvColor = new HsvColorClass();
            hsvColor.Hue = intH;
            hsvColor.Saturation = intS;
            hsvColor.Value = intV;
            return hsvColor;
        }

        /// <summary>
        ///  创建色带
        /// </summary>
        /// <returns>IColorRamp 色带</returns>
        public static IColorRamp CreateAlgorithmicColorRamp() 
        {
            // 创建一个新的AlgorithmicColorRampClass对象
            IAlgorithmicColorRamp algorithmicColorRamp = new AlgorithmicColorRampClass();
            // 创建起始终止颜色
            IRgbColor fromColor = GetRgbColor(255, 0, 0);
            IRgbColor toColor = GetRgbColor(0, 255, 0);
            algorithmicColorRamp.FromColor = fromColor;
            algorithmicColorRamp.ToColor = toColor;
            
            // 设置梯度类型
            algorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
            // 设置颜色色带数量
            algorithmicColorRamp.Size = 10;

            // 创建颜色带
            bool bTrue = true;
            algorithmicColorRamp.CreateRamp(out bTrue);

            return algorithmicColorRamp;
        }

    }
}
