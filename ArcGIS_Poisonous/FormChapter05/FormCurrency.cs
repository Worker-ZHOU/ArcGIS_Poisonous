using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ArcGIS_Poisonous.Tools;
using System.Diagnostics;


namespace ArcGIS_Poisonous.FormChapter05
{
    /// <summary>
    /// 符号化图层与字段选择通用窗口
    /// 可作用窗口:
    /// 1.唯一值符号化
    /// 2.TextElement标注
    /// 3.Annotation注记
    /// </summary>
    public partial class FormCurrency : Form
    {
        private List<IFeatureClass> mListFeatureClass;

        public delegate void EventHandle(string featureClassName, string fieldName);
        public event EventHandle Rander = null;
       

        /// <summary>
        /// 设置窗口标题名称
        /// </summary>
        public string FormTitleText
        {
            set {
                this.Text = value;
            }
        }

        private IMap mMap;
        
        /// <summary>
        /// 当前MapControl的Map对象
        /// </summary>
        public IMap Map
        {
            get
            {
                return mMap;
            }
            set
            {
                mMap = value;
            }
        }

        /// <summary>
        /// 初始化UI界面
        /// </summary>
        public virtual void InitUI() {
            string className = string.Empty;
            IFeatureClass featureClass = null;
            mListFeatureClass = MapOperation.GetFeatureClassListByMap(mMap);
            for (int i = 0; i < mListFeatureClass.Count; i++)
            {
                featureClass = mListFeatureClass[i];
                className = featureClass.AliasName;
                if (!cmbSelectedLayer.Items.Contains(className))
                {
                    cmbSelectedLayer.Items.Add(className);
                }
            }
        }


        public FormCurrency()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 图层选择窗口发生改变触发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void cmbSelectedLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 清空字段选择器信息
            cmbSelectedField.Items.Clear();
            cmbSelectedField.Text = "";
            IField field = null;
            IFeatureClass featureClass = GetFeatureClassByName(cmbSelectedLayer.SelectedItem.ToString());
            if (featureClass != null)
            {
                for (int i = 0; i < featureClass.Fields.FieldCount; i++)
                {
                    field = featureClass.Fields.get_Field(i);
                    if (field.Type == esriFieldType.esriFieldTypeDouble ||
                    field.Type == esriFieldType.esriFieldTypeInteger ||
                    field.Type == esriFieldType.esriFieldTypeSingle ||
                    field.Type == esriFieldType.esriFieldTypeSmallInteger)
                    {
                        if (!cmbSelectedField.Items.Contains(field.Name))
                        {
                            cmbSelectedField.Items.Add(field.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通过要素名称查找要素
        /// </summary>
        /// <param name="featureClassName">要素名称</param>
        /// <returns>要素名称对应的要素</returns>
        private IFeatureClass GetFeatureClassByName(string featureClassName)
        {
            IFeatureClass featureClass = null;

            for (int i = 0; i < mListFeatureClass.Count; i++)
            {
                featureClass = mListFeatureClass[i];
                if (featureClass.AliasName == featureClassName)
                {
                    break;
                }
            }
            return featureClass;
        }

        /// <summary>
        /// 点击确认窗口触发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!SelectCheck())
            {
                return;
            }

            Rander(cmbSelectedLayer.SelectedItem.ToString(), cmbSelectedField.SelectedItem.ToString());
            cmbSelectedLayer.Items.Clear();
            cmbSelectedField.Items.Clear();
            cmbSelectedLayer.Text = "";
            cmbSelectedField.Text = "";
            Close();

        }

        /// <summary>
        /// 确认是否都已经选择
        /// </summary>
        /// <returns>全部有选择返回True,存在未选择部分返回False</returns>
        private bool SelectCheck() {
            if (cmbSelectedLayer.SelectedIndex == -1)
            {
                MessageBox.Show("请选择符号化图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (cmbSelectedField.SelectedIndex == -1)
            {
                MessageBox.Show("请选择符号化字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }



        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
