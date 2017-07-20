using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

namespace ArcGIS_Poisonous
{
    public partial class FormOptions : Form
    {
        private ISelectionEnvironment currentSelectionEnvironment;  //设置类变量来使用ISelectionEnvironment接口的选择环境对象
        private int tolerance;  //设置类变量来存储当前设置的距离容限值
        private ESRI.ArcGIS.Display.IRgbColor esriColor;    //设置类变量来存储当前设置的颜色值

        public FormOptions()
        {
            InitializeComponent();
            //初始化esriColor对象
            esriColor = new ESRI.ArcGIS.Display.RgbColorClass();
        }

        /// <summary>
        /// 设置ISelectionEnvironment接口的选择环境对象，用来获取当前地图的选择环境对象
        /// </summary>
        public ISelectionEnvironment CurrentSelectionEnvironment
        {
            set
            {
                currentSelectionEnvironment = value;
            }
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            //在窗体加载时获取当前选择环境设置的默认颜色和默认容限值
            //首先获取当前选择环境对象的默认颜色
            ESRI.ArcGIS.Display.IColor esriColor = currentSelectionEnvironment.DefaultColor;
            //对IColor接口的对象进行接口转换，以便使用RGB颜色值
            ESRI.ArcGIS.Display.IRgbColor rgbColor = esriColor as ESRI.ArcGIS.Display.IRgbColor;
            //根据获取的RGB值，创建Windows颜色对象
            System.Drawing.Color color = System.Drawing.Color.FromArgb(rgbColor.Red, rgbColor.Green, rgbColor.Green);
            //将创建的Windows颜色对象应用于pictureBoxColor控件的背景颜色中
            pictureBoxColor.BackColor = color;
            //获取并显示当前选择环境的默认容限值
            textBoxTolerance.Text = currentSelectionEnvironment.SearchTolerance.ToString();
        }

        //设置容限值文本框内的内容发生改变时触发事件，执行本函数
        private void textBoxTolerance_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //根据输入的容限值修改当前设定的容限值
                tolerance = Convert.ToInt32(textBoxTolerance.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //设置颜色对话框被点击时触发事件，执行本函数
        private void buttonSetColor_Click(object sender, EventArgs e)
        {
            //弹出颜色选择对话框，根据选择的颜色设置当前选择环境对象的默认颜色
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //先修改pictureBoxColor控件的背景颜色
                pictureBoxColor.BackColor = colorDialog.Color;
                //设置esriColor对象的R、G、B值                
                esriColor.Red = colorDialog.Color.R;
                esriColor.Green = colorDialog.Color.G;
                esriColor.Blue = colorDialog.Color.B;
            }
        }

        //将目前修改的参数值设置到当前地图选择环境对象的相应属性值中，今后查询选择时将使用新的参数值
        private void buttonOK_Click(object sender, EventArgs e)
        {
            //根据输入的容限值修改当前选择环境对象的容限值
            currentSelectionEnvironment.SearchTolerance = tolerance;
            //设置当前选择环境对象的默认颜色
            currentSelectionEnvironment.DefaultColor = esriColor as ESRI.ArcGIS.Display.IColor;
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
