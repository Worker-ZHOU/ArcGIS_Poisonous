using ESRI.ArcGIS.Display;
using System;
using System.Collections.Generic;
using ArcGIS_Poisonous.Tools;


namespace ArcGIS_Poisonous.FormChapter05
{
    /// <summary>
    /// 继承了通用窗体1FormCurrency，以后尽量不要这样实现，纯属闹着玩随便写写
    /// </summary>
    public partial class FormSingleRender : FormCurrency
    {

        public new delegate void EventHandle(string featureClassName, IRgbColor rgbColor);

        public new event EventHandle Rander = null;

        private Dictionary<String, IRgbColor> ColorDictionary;

        public FormSingleRender()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重写重置UI事件
        /// </summary>
        public override void InitUI(){
            base.InitUI();
            cmbSelectedField.Items.Clear();
            cmbSelectedField.Items.Add("Black");
            cmbSelectedField.Items.Add("Whtile");
            cmbSelectedField.Items.Add("Red");
            cmbSelectedField.Items.Add("Green");
            cmbSelectedField.Items.Add("Blue");

            ColorDictionary = new Dictionary<string, IRgbColor>();

            ColorDictionary.Add("Black", ColorTool.GetRgbColor(0, 0, 0));
            ColorDictionary.Add("Whtile", ColorTool.GetRgbColor(255, 255, 255));
            ColorDictionary.Add("Red", ColorTool.GetRgbColor(255, 0, 0));
            ColorDictionary.Add("Green", ColorTool.GetRgbColor(0, 255, 0));
            ColorDictionary.Add("Blue", ColorTool.GetRgbColor(0, 0, 255));
        }

        protected override void  cmbSelectedLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 重写覆盖
        }

        /// <summary>
        /// 重写ok按钮点击处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void btnOK_Click(object sender, EventArgs e)
        {

            Rander(cmbSelectedLayer.SelectedItem.ToString(), ColorDictionary[cmbSelectedField.SelectedItem.ToString()]);
            this.Close();
        }

    }
}
