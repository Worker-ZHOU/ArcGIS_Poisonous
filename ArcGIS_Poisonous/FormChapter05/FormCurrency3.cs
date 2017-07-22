using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using ArcGIS_Poisonous.Tools;
using System.Drawing;

namespace ArcGIS_Poisonous.FormChapter05
{
    public partial class FormCurrency3 : Form
    {
        private List<IFeatureClass> mFeatureClassList;

        private IMap mMap;
        public IMap Map
        {
            set
            {
                mMap = value;
            }
        }

        public delegate void EventHandler(string featureClassName, Dictionary<string, IRgbColor> dictonaryFieldAndColor);
        public event EventHandler Render = null;

        private ArrayList mFieldArrayName = new ArrayList();

        private string item = "";

        public string FormTitleText
        {
            set {
                this.Text = value;

                string className = string.Empty;
                IFeatureClass featureClass = null;
                cmbSelLayer.Items.Clear();
                mFeatureClassList = MapOperation.GetFeatureClassListByMap(mMap);
                for (int i = 0; i < mFeatureClassList.Count; i++)
                {
                    featureClass = mFeatureClassList[i];
                    className = featureClass.AliasName;
                    if (!cmbSelLayer.Items.Contains(className))
                    {
                        cmbSelLayer.Items.Add(className);
                    }
                }
            }
        }

        public FormCurrency3()
        {
            InitializeComponent();
        }

        private void cmbSelLayer_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {

           
            IField field = null;
            lstboxField.Items.Clear();
            IFeatureClass featureClass = GetFeatClsByName(cmbSelLayer.SelectedItem.ToString());
            for (int i = 0; i < featureClass.Fields.FieldCount; i++)
            {
                field = featureClass.Fields.get_Field(i);
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
            catch (Exception)
            {

            }
        }

        private IFeatureClass GetFeatClsByName(string sFeatClsName)
        {
            IFeatureClass featureClass = null;
            for (int i = 0; i < mFeatureClassList.Count; i++)
            {
                featureClass = mFeatureClassList[i];
                if (featureClass.AliasName == sFeatClsName)
                {
                    break;
                }
            }
            return featureClass;
        }

        /// <summary>
        /// 添加字段，并设置默认颜色为蓝色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddOne_Click(object sender, EventArgs e)
        {
            if (lstboxField.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要添加字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            item = this.lstboxField.SelectedItem.ToString();
            lstboxField.Items.RemoveAt(lstboxField.SelectedIndex);
            string name = item.ToString();
            mFieldArrayName.Add(name);
            dataGridView.Rows.Clear();
            for (int i = 0; i < mFieldArrayName.Count; i++)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = mFieldArrayName[i];
                dataGridView.Rows[i].Cells[1].Value = mFieldArrayName[i];
                dataGridView.Rows[i].Cells[2].Value = "Blue";
            }
        }

        /// <summary>
        /// 删除单个字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelOne_Click(object sender, EventArgs e)
        {
            int index = dataGridView.CurrentRow.Index;
            string name = dataGridView.Rows[index].Cells[0].Value.ToString();
            if (mFieldArrayName.Contains(name))
            {
                mFieldArrayName.Remove(name);
            }
            lstboxField.Items.Add(name);
            dataGridView.Rows.RemoveAt(index);
            dataGridView.Refresh();
        }

        /// <summary>
        /// 添加所有字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstboxField.Items.Count; i++)
            {
                mFieldArrayName.Add(lstboxField.Items[i]);
                
            }
            lstboxField.Items.Clear();
            dataGridView.Rows.Clear();
            for (int i = 0; i < mFieldArrayName.Count; i++)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = mFieldArrayName[i];
                dataGridView.Rows[i].Cells[1].Value = mFieldArrayName[i];
                dataGridView.Rows[i].Cells[2].Value = "Blue";
            }

        }

        /// <summary>
        /// 删除所有字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count -1; i++)
            {
                if (!lstboxField.Items.Contains(dataGridView.Rows[i].Cells[0].Value.ToString()))
                {
                    lstboxField.Items.Add(dataGridView.Rows[i].Cells[0].Value.ToString());
                    if (mFieldArrayName.Contains(dataGridView.Rows[i].Cells[0].Value.ToString()))
                    {
                        mFieldArrayName.Remove(dataGridView.Rows[i].Cells[0].Value.ToString());
                    }
                }
            }
            dataGridView.Rows.Clear();
        }

        private bool SelectedCheck()
        {
            if (cmbSelLayer.SelectedIndex == -1)
            {
                MessageBox.Show("请选择专题图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("请选择图层字段及所对应的颜色值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 添加字段，并设置字段对应的颜色，存储在_dicFieldAndColor中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!SelectedCheck()) return;
            Color color;
            IRgbColor rgbColor = null;
            string fieldName = string.Empty;
            Dictionary<string, IRgbColor> _dicFieldAndColor = null;
            _dicFieldAndColor = new Dictionary<string, IRgbColor>();
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                fieldName = dataGridView.Rows[i].Cells[0].Value.ToString();
                switch (dataGridView.Rows[i].Cells[2].Value.ToString())
                {
                    case "Red":
                        color =Color.Red;
                        break;
                    case "Blue":
                        color = Color.Blue;
                        break;
                    case "Green":
                        color = Color.Green;
                        break;
                    case "Brown":
                        color = Color.Brown;
                        break;
                    default:
                        color = Color.Yellow;
                        break;
                }
                rgbColor = ColorTool.GetRgbColor((int)color.R, (int)color.G, (int)color.B);
                if (!_dicFieldAndColor.ContainsKey(fieldName))
                {
                    _dicFieldAndColor.Add(fieldName, rgbColor);
                }
            }
            Render(cmbSelLayer.SelectedItem.ToString(), _dicFieldAndColor);
            mFieldArrayName.Clear();
            cmbSelLayer.SelectedIndex = -1;
            cmbSelLayer.Text = "";
            dataGridView.Rows.Clear();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
