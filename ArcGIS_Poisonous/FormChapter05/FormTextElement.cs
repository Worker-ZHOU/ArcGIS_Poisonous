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

/// <summary>
///  这个类暂时时没什么用的，废弃
/// </summary>
namespace ArcGIS_Poisonous
{
    public partial class FormTextElement : Form
    {
        private IMap mCurrentMap;

        public IMap CurrentMap 
        {
            set {
                mCurrentMap = value;
            }
        }

        private List<IFeatureClass> mListFeatureClass = null;

        public delegate void TextElementLabelEventHandler(string sFeatClsName, string sFieldName);
        public event TextElementLabelEventHandler TextElement;
        //public ITextElement TextElement
        //{
        //    set{
        //        mTextElement = value;
        //    }
        //}

        public FormTextElement()
        {
            InitializeComponent();
        }

        public void InitUI() 
        {
            string className = string.Empty;
            IFeatureClass featureClass = null;

            ILayer pLayer = null;
            IFeatureLayer pFeatLyr = null;
            mListFeatureClass = new List<IFeatureClass>();
            for (int i = 0; i < mCurrentMap.LayerCount; i++)
            {
                pLayer = mCurrentMap.get_Layer(i);
                pFeatLyr = pLayer as IFeatureLayer;
                GetLstFeatCls(pLayer, ref mListFeatureClass);
            }
            for (int i = 0; i < mListFeatureClass.Count; i++)
            {
                featureClass = mListFeatureClass[i];
                className = featureClass.AliasName;
                if (!cmbSelLyr.Items.Contains(className))
                {
                    cmbSelLyr.Items.Add(className);
                }
            }
        }

        public void GetLstFeatCls(ILayer pLayer, ref List<IFeatureClass> _lstFeatCls)
        {
            try
            {
                ILayer pLyr = null;
                ICompositeLayer pComLyr = pLayer as ICompositeLayer;
                if (pComLyr == null)
                {
                    IFeatureLayer pFeatLyr = pLayer as IFeatureLayer;
                    if (!_lstFeatCls.Contains(pFeatLyr.FeatureClass))
                    {
                        _lstFeatCls.Add(pFeatLyr.FeatureClass);
                    }
                }
                else
                {
                    for (int i = 0; i < pComLyr.Count; i++)
                    {
                        pLyr = pComLyr.get_Layer(i);
                        GetLstFeatCls(pLyr, ref _lstFeatCls);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool check() 
        {
            if (cmbSelLyr.SelectedIndex == -1)
            {
                MessageBox.Show("请选择注记图层！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return false;
            }
            if (cmbSelField.SelectedIndex == -1)
            {
                MessageBox.Show("请选择注记字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }


        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!check()) return;
            TextElement(cmbSelLyr.SelectedItem.ToString(), cmbSelField.SelectedItem.ToString());
            cmbSelField.Items.Clear();
            cmbSelField.Text = "";
            cmbSelLyr.Items.Clear();
            cmbSelLyr.Text = "";
            Close();
        }

        private void cmbSelLyr_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelField.Items.Clear();
            cmbSelField.Text = "";
            IField pField = null;
            IFeatureClass pFeatCls = GetFeatClsByName(cmbSelLyr.SelectedItem.ToString());
            for (int i = 0; i < pFeatCls.Fields.FieldCount; i++)
            {
                pField = pFeatCls.Fields.get_Field(i);
                if (pField.Type == esriFieldType.esriFieldTypeDouble ||
                    pField.Type == esriFieldType.esriFieldTypeInteger ||
                    pField.Type == esriFieldType.esriFieldTypeSingle ||
                    pField.Type == esriFieldType.esriFieldTypeSmallInteger ||
                    pField.Type == esriFieldType.esriFieldTypeString ||
                     pField.Type == esriFieldType.esriFieldTypeOID)
                {
                    if (!cmbSelField.Items.Contains(pField.Name))
                    {
                        cmbSelField.Items.Add(pField.Name);
                    }
                }
            }
        }
        private IFeatureClass GetFeatClsByName(string sFeatClsName)
        {
            IFeatureClass pFeatCls = null;
            for (int i = 0; i < mListFeatureClass.Count; i++)
            {
                pFeatCls = mListFeatureClass[i];
                if (pFeatCls.AliasName == sFeatClsName)
                {
                    break;
                }
            }
            return pFeatCls;
        }
    }
}
