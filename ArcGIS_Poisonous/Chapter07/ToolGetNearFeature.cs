using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ArcGIS_Poisonous.Tools;

namespace ArcGIS_Poisonous.Chapter07
{
    /// <summary>
    /// Summary description for ToolGetNearFeature.
    /// </summary>
    [Guid("d5fd7295-5800-4c4b-9e64-b657551368f7")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcGIS_Poisonous.Chapter07.ToolGetNearFeature")]
    public sealed class ToolGetNearFeature : BaseTool
    {

        private IHookHelper m_HookHelper = null;

        public ToolGetNearFeature()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            m_HookHelper = new HookHelperClass();
            m_HookHelper.Hook = hook;
            if (m_HookHelper.ActiveView == null)
            {
                m_HookHelper = null;
            }

            if (m_HookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // 判断MapControl是否有图层
            if (m_HookHelper.FocusMap.LayerCount <= 0)
            {
                MessageBox.Show("请先加载图层数据");
                return;
            }
            // 修改鼠标样式
            (this.m_HookHelper.Hook as MapControl).MousePointer = esriControlsMousePointer.esriPointerHand;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1 || m_HookHelper.FocusMap.LayerCount <=0)
            {
                return;
            }
            IMap map = m_HookHelper.FocusMap;
            IActiveView activeView = m_HookHelper.ActiveView;
            // 获取点击位置并抓化为点图形要素
            IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            // 获取地图中的图层
            IFeatureLayer featureLayer = activeView.FocusMap.get_Layer(0) as IFeatureLayer;
            if (featureLayer == null)
            {
                return;
            }
            IFeatureClass featureCLass = featureLayer.FeatureClass;
            // 进行点击，选择要素
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = point;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor featureCursor = featureCLass.Search(spatialFilter, false);
            // 获得点击查询的要素 
            IFeature feature = featureCursor.NextFeature();
            if (feature != null && feature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                map.ClearSelection();
                IRelationalOperator relationalOperator = feature.Shape as IRelationalOperator;
                // 设置被选择要素的颜色
                IRgbColor rgbColor = ColorTool.GetRgbColor(255, 0, 0);
                IFeatureSelection featureSelection = featureLayer as IFeatureSelection;
                featureSelection.SelectionColor = rgbColor;
                // 查找邻接要素，并之余地图的选择集中
                IFeatureCursor nearFeatureCursor = featureLayer.Search(null, false);
                IFeature nearFeature = nearFeatureCursor.NextFeature();

                // 遍历图层内所有要素进行邻接判定
                while (nearFeature != null)
                {
                    if (relationalOperator.Touches(nearFeature.Shape))
                    {
                        // 如果对比要素与当前选择要素邻接，则加入到地图选择集中
                        map.SelectFeature(featureLayer, nearFeature);
                    }
                    nearFeature = nearFeatureCursor.NextFeature();
                } 
            }

            // 刷新地图
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolGetNearFeature.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolGetNearFeature.OnMouseUp implementation
        }
        #endregion
    }
}
