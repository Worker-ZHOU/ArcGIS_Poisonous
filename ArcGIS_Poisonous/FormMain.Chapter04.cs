using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ArcGIS_Poisonous
{   
    /// <summary>
    ///  Chapter04 属性查询
    /// </summary>
    public partial class ArcGisPoisonous : Form
    {

        //定义ISelectionEnvironment接口的对象来设置选择环境
        // partial ISelectionEnvironment selectionEnvironment;

        #region 4 查询统计

        #region 4.1 属性查询

        /// <summary>
        /// 打开属性查询窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemQueryByAttribute_Click(object sender, EventArgs e)
        {
            // 创建新的窗体
            FormQueryByAttribute formQueryByAttribute = new FormQueryByAttribute();
            // 将当前窗体的中的MapControl控件中的Map赋值到FormQueryByAttribute中的CurrentMap中
            formQueryByAttribute.CurrentMap = MainMapControl.Map;
            // 显示属性查询窗体
            formQueryByAttribute.Show();
        }

        #endregion

        #region 4.2 空间查询

        /// <summary>
        /// 打开空间查询窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemQueryBySpatial_Click(object sender, EventArgs e)
        {
            // 创建新的窗体
            FormQueryBySpatial formQueryBySpatial = new FormQueryBySpatial();
            // 将当前窗体的中的MapControl控件中的Map赋值到FormQueryByAttribute中的CurrentMap中
            formQueryBySpatial.CurrentMap = MainMapControl.Map;
            // 显示空间查询窗体
            formQueryBySpatial.Show();
        }

        #endregion

        #region 4.3 图形查询

        /// <summary>
        /// 图形查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemQueryByGraphics_Click(object sender, EventArgs e)
        {
            // 清空地图选择集以进行后续的选择操作
            MainMapControl.Map.FeatureSelection.Clear();
            // 使用IGraphicsContainer接口获取地图中的各个地图
            IGraphicsContainer graphicsContainer = MainMapControl.Map as IGraphicsContainer;
            // 重置访问图形的游标，使IGraphicsContainer接口的Next()方法定位于地图中的第一个图形
            graphicsContainer.Reset();
            // 使用IElement接口操作获取第一个图形
            IElement element = graphicsContainer.Next();
            // 获取图形的几何信息
            IGeometry geometry = element.Geometry;
            // 使用第一个图形的几何形状来选择地图中的要素
            MainMapControl.Map.SelectByShape(geometry, null, false);
            // 进行部分刷新以显示最新的选择集
            MainMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,   MainMapControl.ActiveView.Extent);
        }

        #endregion 

        #region 4.4 选择集

        /// <summary>
        /// 选择集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemMapSelection_Click(object sender, EventArgs e)
        {
            // 创建窗体
            FormSelection formSelection = new FormSelection();
            // 将当前窗体的中的MapControl控件中的Map赋值到FormSelection中的CurrentMap中
            formSelection.CurrentMap = MainMapControl.Map;
            // 显示选择集窗体
            formSelection.Show();
        }

        #endregion

        #region 4.6 统计分析

        /// <summary>
        /// 统计分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemStatistics_Click(object sender, EventArgs e)
        {
            FormStatistics formStatistics = new FormStatistics();
            formStatistics.CurrentMap = MainMapControl.Map;
            formStatistics.Show();
        }

        #endregion


        #endregion

    }
}
