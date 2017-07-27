using ArcGIS_Poisonous.Tools;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ArcGIS_Poisonous.Chapter07
{
    /// <summary>
    /// 要素缓冲区分析
    /// </summary>
    [Guid("6a5015c2-5956-480f-a2dd-ac59e6decbfb")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcGIS_Poisonous.ToolBufferAnalysis")]
    public sealed class ToolBufferAnalysis : BaseTool
    {

        private IHookHelper m_hookHelper = null;

        public ToolBufferAnalysis()
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
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
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
            if (m_hookHelper.FocusMap.LayerCount <= 0)
            {
                MessageBox.Show("请先加载图层数据");
                return;
            }
            // 修改鼠标样式
            (this.m_hookHelper.Hook as MapControl).MousePointer = esriControlsMousePointer.esriPointerHand;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1 || m_hookHelper.FocusMap.LayerCount <= 0)
            {
                return;
            }

            IActiveView activeView = m_hookHelper.ActiveView;
            IGraphicsContainer graphicsContainer = activeView as IGraphicsContainer;
            // 删除地图上添加的所有Element
            graphicsContainer.DeleteAllElements();
            // 获得点击位置并转化为点图形要素
            IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            // 获取地图中的图层
            IFeatureLayer featureLayer = m_hookHelper.FocusMap.get_Layer(0) as IFeatureLayer;
            if (featureLayer == null)
            {
                return;
            }
            IFeatureClass featureClass = featureLayer.FeatureClass;
            // 进行点击，查询图层要素
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = point;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            // 获得点击查询的要素
            IFeature feature = featureCursor.NextFeature();
            if (feature != null && feature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                IGeometry geometry = feature.Shape as IGeometry;
                // 通过ITopologicalOperator接口进行多边行的简单化处理
                ITopologicalOperator topologicalOperator = geometry as ITopologicalOperator;
                topologicalOperator.Simplify();
                // 创建缓冲区
                IGeometry bufferGeometry = topologicalOperator.Buffer(5000);
                // 创建多边形符号样式并添加到地图上
                IScreenDisplay screenDisplay = activeView.ScreenDisplay;
                ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
                simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSCross;
                IRgbColor rgbColor = ColorTool.GetRgbColor(211,100,200);
                simpleFillSymbol.Color = rgbColor;
                // 创建多边行渲染效果的Element
                IFillShapeElement fillShapElement = new PolygonElementClass();
                IElement element = fillShapElement as IElement;
                element.Geometry = bufferGeometry;
                fillShapElement.Symbol = simpleFillSymbol;
                // 将渲染之后的多边形element添加到地图IGraphicsContainer中
                graphicsContainer.AddElement(fillShapElement as IElement, 0);
            }
            // 刷新地图
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolBufferAnalysis.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolBufferAnalysis.OnMouseUp implementation
        }
        #endregion
    }
}
