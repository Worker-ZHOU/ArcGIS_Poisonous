using ArcGIS_Poisonous.Tools;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
///  唯一值多字段符号化
/// </summary>
namespace ArcGIS_Poisonous.FormChapter05
{
    public partial class FormUniqueManyValueRenderer : Form
    {
        private IMap mMap;

        private List<IFeatureClass> mFeatureClassList = null;

        //定义一个数组，用来存储lstboxField选中的值
        private ArrayList mFieldName = new ArrayList();

        public delegate void EventHandler(string featureClassName, string[] fieldName);
        public event EventHandler Render = null;

        public IMap Map 
        {
            set {
                mMap = value;
            }
        }

        private string mItem = string.Empty;

        public FormUniqueManyValueRenderer()
        {
            InitializeComponent();
        }

        public void InitUI() {
            dataGridView.Rows.Clear();
            string className = string.Empty;
            IFeatureClass featureClass = null;
            cmbSelLyr.Items.Clear();
            mFeatureClassList = MapOperation.GetFeatureClassListByMap(mMap);
            for (int i = 0; i < mFeatureClassList.Count; i++)
            {
                featureClass = mFeatureClassList[i];

                className = featureClass.AliasName;

                if (!cmbSelLyr.Items.Contains(className))
                {
                    cmbSelLyr.Items.Add(className);
                    // 以下代码为判断该图层所包含数字值字段的个数，如果小于2，则将该字段从cmbSelLyr移除
                    int m = 0;
                    for (int j = 0; j < featureClass.Fields.FieldCount; j++)
                    {
                        IField pField = featureClass.Fields.get_Field(j);
                        //判断字段的数据类型是否为数字类型
                        if (pField.Type == esriFieldType.esriFieldTypeDouble ||
                            pField.Type == esriFieldType.esriFieldTypeInteger ||
                            pField.Type == esriFieldType.esriFieldTypeSingle ||
                            pField.Type == esriFieldType.esriFieldTypeSmallInteger)
                        {
                            m++;
                        }
                    }
                    if (m < 3)
                    {
                        cmbSelLyr.Items.Remove(className);
                    }
                }
            }
        }

        /// <summary>
        /// 由图层名称，获取该图层
        /// </summary>
        /// <param name="sFeatClsName">图层名称</param>
        /// <returns></returns>
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

        /// <summary>
        /// 选择选项发生改变时触发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSelLyr_SelectedIndexChanged(object sender, EventArgs e)
        {
            IFeatureClass featureClass;
            IField field = null;
            lstboxField.Items.Clear();
            featureClass = GetFeatClsByName(cmbSelLyr.SelectedItem.ToString());
            for (int i = 0; i < featureClass.Fields.FieldCount; i++)
            {
                field = featureClass.Fields.get_Field(i);
                // 判断字段的数据类型是否为数字类型
                if (field.Type == esriFieldType.esriFieldTypeDouble ||
                        field.Type == esriFieldType.esriFieldTypeInteger ||
                        field.Type == esriFieldType.esriFieldTypeSingle ||
                        field.Type == esriFieldType.esriFieldTypeSmallInteger)
                {
                    if (!lstboxField.Items.Contains(field.Name))
                    {
                        lstboxField.Items.Add(field.Name);
                    }
                }
            }
        }

        private void btnAddOne_Click(object sender, EventArgs e)
        {
            if (lstboxField.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要添加字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            mItem = this.lstboxField.SelectedItem.ToString();
            lstboxField.Items.RemoveAt(lstboxField.SelectedIndex);
            string fieldName = mItem;
            mFieldName.Add(fieldName);
            dataGridView.Rows.Clear();
            for (int i = 0; i < mFieldName.Count; i++)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = mFieldName[i];
            }
        }

        private void btnDelOne_Click(object sender, EventArgs e)
        {
            try
            {
                int index = dataGridView.CurrentRow.Index;
                string fieldName = dataGridView.Rows[index].Cells[0].Value.ToString();
                if (mFieldName.Contains(dataGridView.Rows[index].Cells[0].Value.ToString()))
                {
                    this.mFieldName.Remove(dataGridView.Rows[index].Cells[0].Value.ToString());
                }
                lstboxField.Items.Add(fieldName);
                dataGridView.Rows.RemoveAt(index);
                dataGridView.Refresh();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string[] add;
            if (dataGridView.Rows.Count <= 2)//因为dataGridView1包含一个空行
            {
                MessageBox.Show("所选的字段数不能少于2个");
                dataGridView.Rows.Clear();
                mFieldName.Clear();
                lstboxField.Items.Clear();
                cmbSelLyr.Items.Clear();
            }
            else if (dataGridView.Rows.Count >= 5)
            {
                MessageBox.Show("所选的字段数不能超过3个");
                dataGridView.Rows.Clear();
                mFieldName.Clear();
                lstboxField.Items.Clear();
                cmbSelLyr.Items.Clear();
            }
            if (dataGridView.Rows.Count == 3)
            {
                add = new string[2];
                for (int i = 0; i < 2; i++)
                {
                    add[i] = dataGridView.Rows[i].Cells[0].Value.ToString();
                }
                Render(cmbSelLyr.SelectedItem.ToString(), add);
                mFieldName.Clear();
            }
            else if (dataGridView.Rows.Count == 4)
            {
                add = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    add[i] = dataGridView.Rows[i].Cells[0].Value.ToString();
                }
                Render(cmbSelLyr.SelectedItem.ToString(), add);
                mFieldName.Clear();
            }
            cmbSelLyr.Text = "";
            lstboxField.Items.Clear();
            this.Close();
        }

        /// <summary>
        /// 取消按钮触发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            mFieldName.Clear();
            this.Close();
        }
    }
}
