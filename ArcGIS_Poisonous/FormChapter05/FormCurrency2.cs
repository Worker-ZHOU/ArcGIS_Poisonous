using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArcGIS_Poisonous.Tools;

namespace ArcGIS_Poisonous.FormChapter05
{
    /// <summary>
    /// 通用选择窗口2，获取图层，字段以及数量
    /// 可作用窗口
    /// 1.分级色彩符号化
    /// 2.分级符号化
    /// 3.点密度符号化
    /// </summary>
    public partial class FormCurrency2 : Form
    {
        private List<IFeatureClass> mFeatureClassList = null;

        public delegate void EventHandler(String featureClassName, String fieldName, int intNumClass);
        public event EventHandler Render = null;

        private IMap mMap;
        public IMap Map {
            set
            {
                mMap = value;
            }
        }

        private string mFormTitleText;

        public string FormTitleText {
            set {
                mFormTitleText = value;

                string className = string.Empty;
                cmbSelLyr.Items.Clear();
                IFeatureClass featureClass = null;
                mFeatureClassList = MapOperation.GetFeatureClassListByMap(mMap);
                for (int i = 0; i < mFeatureClassList.Count; i++)
                {
                    featureClass = mFeatureClassList[i];
                    className = featureClass.AliasName;
                    if (!cmbSelLyr.Items.Contains(className))
                    {
                        cmbSelLyr.Items.Add(className);
                    }
                }
                Text = mFormTitleText;
            }
        }

        public FormCurrency2()
        {
            InitializeComponent();
        }

        private bool CheckSelect()
        {
            if (cmbSelLyr.SelectedIndex == -1)
            {
                MessageBox.Show("请选择符号化图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (cmbSelField.SelectedIndex == -1)
            {
                MessageBox.Show("请选择符号化字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (cmbnumclasses.SelectedIndex == -1)
            {
                MessageBox.Show("请选择分级数目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private IFeatureClass GetFeatClsByName(string sFeatClsName)
        {
            IFeatureClass pFeatCls = null;
            for (int i = 0; i < mFeatureClassList.Count; i++)
            {
                pFeatCls = mFeatureClassList[i];
                if (pFeatCls.AliasName == sFeatClsName)
                {
                    break;
                }
            }
            return pFeatCls;
        }

        private void cmbSelLyr_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelField.Items.Clear();
            cmbSelField.Text = "";
            IField field = null;
            IFeatureClass featureClass = GetFeatClsByName(cmbSelLyr.SelectedItem.ToString());
            for (int i = 0; i < featureClass.Fields.FieldCount; i++)
            {
                field = featureClass.Fields.get_Field(i);
                if (field.Type == esriFieldType.esriFieldTypeDouble ||
                    field.Type == esriFieldType.esriFieldTypeInteger ||
                    field.Type == esriFieldType.esriFieldTypeSingle ||
                    field.Type == esriFieldType.esriFieldTypeSmallInteger)
                {
                    if (!cmbSelField.Items.Contains(field.AliasName))
                    {
                        cmbSelField.Items.Add(field.AliasName);
                    }
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!CheckSelect())
            {
                return;
            }
            Render.Invoke(cmbSelLyr.SelectedItem.ToString(), cmbSelField.SelectedItem.ToString(), cmbnumclasses.SelectedIndex);
            cmbSelLyr.Items.Clear();
            cmbSelField.Items.Clear();
            cmbSelLyr.Text = "";
            cmbSelField.Text = "";
            cmbnumclasses.SelectedIndex = -1;
            Close();
        }
    }
}
