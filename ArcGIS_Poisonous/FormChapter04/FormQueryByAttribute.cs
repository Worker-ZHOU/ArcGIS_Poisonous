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

namespace ArcGIS_Poisonous
{
    public partial class FormQueryByAttribute : Form
    {

        #region 成员变量

        // 当前MapControl控件中的Map对象
        private IMap currentMap;

        public IMap CurrentMap {
            set 
            {
                currentMap = value;
            }
        }

        // 设置临时类变量来使用IFeatureLayer接口的当前图层对象
        private IFeatureLayer currentFeatureLayer;

        // 设置临时类变量来存储字段名称
        private string currentFileName;

        #endregion

        #region 初始化窗体

        public FormQueryByAttribute()
        {
            InitializeComponent();

            // 获取唯一值按钮设置为不可用
            buttonGetUniqeValue.Enabled = false;
        }

        #endregion

        #region FormQueryByAttribute加载

        /// <summary>
        /// 窗体加载时触发事件执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormQueryByAttribute_Load(object sender, EventArgs e)
        {
            // 将当前图层列表清空
            comboBoxLayerName.Items.Clear();
            // 设置临时变量存储图层名称
            string layerName = string.Empty;
            // 对Map中的每个图层进行判断并加载名称
            for (int i = 0; i < currentMap.LayerCount; i++)
            {
                // 如果该图层为图层组类型，则分别对所包含的每个图层进行操作
                if (currentMap.get_Layer(i) is GroupLayer)
                {
                    // 使用ICompositeLayer接口进行遍历操作
                    ICompositeLayer compositeLayer = currentMap.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < compositeLayer.Count; j++)
                    {
                        // 将图层的名称添加到comboBoxLayerName控件中
                        layerName = compositeLayer.get_Layer(j).Name;
                        comboBoxLayerName.Items.Add(layerName);
                    }
                }
                // 如果图层不是图层组类型，则直接添加名字
                else 
                {
                    layerName = currentMap.get_Layer(i).Name;
                    comboBoxLayerName.Items.Add(layerName);
                }
            }
            // 将comboBoxLayerName控件的默认选项设置为第一个图层的名称
            comboBoxLayerName.SelectedIndex = 0;
            // 将comboBoxLayerMethod控件的默认选项设置为第一种选择方式
            comboBoxSelectMethod.SelectedIndex = 0;
        }

        #endregion

        #region comboBoxLayerName下拉框改变

        /// <summary>
        /// 在图层名称下拉框控件中所选择图层发生改变时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxLayerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 首先将字段列表和字段值列表清空
            listBoxFields.Items.Clear();
            listBoxValues.Items.Clear();
            
            // 设置临时变量存储使用IField接口的对象
            IField field;
            for (int i = 0; i < currentMap.LayerCount; i++)
            {
                if (currentMap.get_Layer(i) is GroupLayer)
                {
                    ICompositeLayer compositeLayer = currentMap.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < compositeLayer.Count; j++)
                    {
                        // 判断图层的名称是否与comboBoxLayerName控件中选择的图层名称相同
                        if (compositeLayer.get_Layer(j).Name == comboBoxLayerName.SelectedIndex.ToString())
                        {
                            // 如果相同则设置为整个窗体所使用的IFeatureLayer接口对象
                            currentFeatureLayer = compositeLayer.get_Layer(j) as IFeatureLayer;
                            break;
                        }
                    }
                }
                else
                {
                    // 判断图层名称是否与comboBoxLayerName中选择的图层名称相同
                    if (currentMap.get_Layer(i).Name == comboBoxLayerName.SelectedItem.ToString())
                    {
                        // 如果相同则设置为整个窗体所使用的IFeature接口对象
                        currentFeatureLayer = currentMap.get_Layer(i) as IFeatureLayer;
                        break;
                    }
                }
            }

            // 使用IFeatureClass接口对该图层的所有属性字段进行遍历，并填充listBoxFields控件
            for (int i = 0; i < currentFeatureLayer.FeatureClass.Fields.FieldCount; i++)
            {
                // 根据索引值获取图层的字段
                field = currentFeatureLayer.FeatureClass.Fields.get_Field(i);
                // 排除SHAPE字段，并在其他字段名称前后添加字符\和自己"
                if (field.Name.ToUpper() != "SHAPE")
                {
                    listBoxFields.Items.Add("\"" + field.Name + "\"");
                }
            }
            // 更新labelwhere控件中的图层名称信息
            labelwhere.Text = currentFeatureLayer.Name + "WHERE:";
            // 将显示where语句的文本框内容清空
            textBoxWhere.Clear();

        }

        #endregion

        #region listBoxFields控件

        /// <summary>
        /// 在字段名称列表控件中所选择的字段发生改变时触发事件执行本函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 首先将listBoxValue控件中的字段属性值清空
            listBoxValues.Items.Clear();
            // 将buttonGetUniqeValue按钮控件置为可用状态(获取唯一属性值按钮)
            if (buttonGetUniqeValue.Enabled == false)
            {
                buttonGetUniqeValue.Enabled = true;
            }
            // 设置整个窗体可用的字段名称
            string str = listBoxFields.SelectedItem.ToString();
            // 使用string类中的方法将字段名称中的两个"字符去掉
            str = str.Substring(1);
            str = str.Substring(0, str.Length - 1);
            currentFileName = str;
        }

        /// <summary>
        /// 在字段列表中双击属性字段名称时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxFields_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //在where语句中加入字段名称信息
            textBoxWhere.Text += listBoxFields.SelectedItem.ToString();
        }

        #endregion

        #region 获取唯一属性值

        /// <summary>
        /// 在点击“获取唯一属性值”按钮时触发事件执行本函数
        /// 对图层的某个字段进行唯一值获取操作，并将所有的唯一值显示在listBoxValue控件中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetUniqeValue_Click(object sender, EventArgs e)
        {
            // 先清除listBoxFields上面的信息，避免重复
            listBoxValues.Items.Clear();
            // 使用Feature对象的IDataset接口来获取dataset和workspace的信息
            IDataset dataset = currentFeatureLayer.FeatureClass as IDataset;
            // 使用IQueryDef接口的对象来定义和查询属性信息。通过IWorkspace接口的CreateQuertDef()方法创建该对象
            IQueryDef queryDef = (dataset.Workspace as IFeatureWorkspace).CreateQueryDef();
            // 设置所需查询的表格名称为dataset的名称
            queryDef.Tables = dataset.Name;
            // 设置查询的字段名称。可以联合使用SQL语言的关键字，如查询唯一值可以使用DISTINCT关键字
            queryDef.SubFields = "DISTINCT(" + currentFileName +")";
            // 执行查询并返回ICursor接口的对象来访问整个结果的集合
            ICursor cursor = queryDef.Evaluate();
            // 使用IField接口获取当前所需要使用的字段的信息
            IFields fields = currentFeatureLayer.FeatureClass.Fields;
            IField field = fields.get_Field(fields.FindField(currentFileName));
            // 对整个结果集合进行遍历，从而添加所有的唯一值
            // 使用IRow接口来操作结果集合。首先定位到第一个查询结果
            IRow row = cursor.NextRow();
            // 如果查询结果非空，则一直进行添加操作
            while (row != null)
            {
                // 对String类型的字段，唯一值的前后添加\和'，以符合SQL语句的要求
                if (field.Type == esriFieldType.esriFieldTypeString)
                {
                    listBoxValues.Items.Add("\'"+ row.get_Value(0).ToString() + "\'");
                }
                else
                {
                    listBoxValues.Items.Add(row.get_Value(0).ToString());
                }
                // 继续执行下一个结果的添加
                row = cursor.NextRow();
            }
        }

        #endregion

        #region listBoxValues控件

        private void listBoxValues_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //在where语句中加入字段名称信息
            textBoxWhere.Text += listBoxValues.SelectedItem.ToString();
        }

        #endregion

        #region 通过属性选择要素

        /// <summary>
        /// 通过属性选择要素
        /// </summary>
        private void SelectFeatureByAttribute()
        {
            // 使用FeatureLayer对象的IFeatureSelection接口来执行查询操作。这里有一个接口转换操作
            IFeatureSelection featureSelection = currentFeatureLayer as IFeatureSelection;
            // 新建IQueryFilter接口的对象来进行where语句的定义
            IQueryFilter queryFilter = new QueryFilterClass();
            // 设置where语句内容
            queryFilter.WhereClause = textBoxWhere.Text;
            // 通过接口转换使用Map对象的IActiveView接口来部分刷新地图窗口，从而高亮显示查询结果
            IActiveView activeView = currentMap as IActiveView;
            // 根据查询选择方式的不同得到不同的选择集
            switch (comboBoxSelectMethod.SelectedIndex)
            {
                case 0:
                    // 在新建选择集的情况下
                    currentMap.ClearSelection();
                    // 根据定义的where语句使用IFeatureSelection接口的SelectFeatures方法选择要素并将其添加到选择集中
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    break;
                case 1:
                    // 添加到当前选择集的情况
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultXOR, false);
                    break;
                case 2:
                    // 从当前选择集中删除的情况
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAnd, false);
                    break;
                case 3:
                    // 从当前选择集中选择的情况
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAnd, false);
                    break;
                default:
                    // 默认为新建选择集情况
                    currentMap.ClearSelection();
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    break;
            }
            // 进行不菲刷新操作，只刷新地理选择集的内容
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);
        }

        #endregion

        #region 其他按钮

        /// <summary>
        /// 在点击“=”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void buttonEqual_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonEqual.Text + " ";
        }

        /// <summary>
        /// 在点击“<>”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNotEqual_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonNotEqual.Text + " ";
        }

        /// <summary>
        /// 在点击“Like”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLike_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonLike.Text + " ";
        }
        /// <summary>
        /// 在点击“>”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGreater_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonGreater.Text + " ";
        }

        /// <summary>
        /// 在点击“>=”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGeaterEqual_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonGeaterEqual.Text + " ";
           
        }

        /// <summary>
        /// 在点击“And”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void buttonAnd_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonAnd.Text + " ";
        }

        /// <summary>
        /// 在点击“<”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLess_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonLess.Text + " ";
        }

        /// <summary>
        /// 在点击“<=”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLessEqual_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonLessEqual.Text + " ";
        }

        /// <summary>
        /// 在点击“Or”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOr_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonOr.Text + " ";
        }

        /// <summary>
        /// 在点击“_”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUnderLine_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonUnderLine.Text + " ";
        }

        /// <summary>
        /// 在点击“%”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPercent_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonPercent.Text + " ";
        }

        /// <summary>
        /// 在点击“()”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrackets_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonBrackets.Text + " ";
        }

        /// <summary>
        /// 在点击“Not”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNot_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonNot.Text + " ";
        }

        /// <summary>
        /// 在点击“Is”运算符时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonIs_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonIs.Text + " ";
        }

        /// <summary>
        /// 清空代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text = string.Empty;
        }

        /// <summary>
        ///  应用并关闭（确认按钮）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            SelectFeatureByAttribute();
            Close();
        }

        /// <summary>
        /// 应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonApply_Click(object sender, EventArgs e)
        {
            SelectFeatureByAttribute();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion 
    }
}
