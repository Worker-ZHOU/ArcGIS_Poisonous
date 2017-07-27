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
    /// ��ȡ����α߽�
    /// </summary>
    [Guid("2929e0f1-e4cb-4d7e-8b80-4ad8f5cd98c9")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcGIS_Poisonous.Chapter07.ToolGetBoundary")]
    public sealed class ToolGetBoundary : BaseTool
    {
        private IHookHelper m_hookHelper = null;

        public ToolGetBoundary()
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
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // �ж�MapControl�Ƿ���ͼ��
            if (m_hookHelper.FocusMap.LayerCount <= 0)
            {
                MessageBox.Show("���ȼ���ͼ������");
                return;
            }
            // �޸������ʽ
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
            // ɾ����ͼ����ӵ�����element
            graphicsContainer.DeleteAllElements();
            // ��õ��λ�ò�ת��Ϊ��ͼ��Ҫ��
            IPoint ponit = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            // ��ȡ��ͼ�е�ͼ��
            IFeatureLayer featureLayer = m_hookHelper.FocusMap.get_Layer(0) as IFeatureLayer;
            if (featureLayer == null)
            {
                return;
            }
            IFeatureClass featureClass = featureLayer.FeatureClass;
            // ���е������ѯͼ��Ҫ��
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = ponit;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIndexIntersects;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            // ��õ����ѯ��Ҫ��
            IFeature feature = featureCursor.NextFeature();
            if (feature != null && feature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                IGeometry geometry = feature.Shape as IGeometry;
                ITopologicalOperator topologicalOperator = geometry as ITopologicalOperator;
                // ��ȡ�߽�
                IGeometry bounddary = topologicalOperator.Boundary;
                ILineElement lineElement = new LineElementClass();
                ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbol();
                IRgbColor rgbcolor = ColorTool.GetRgbColor(0,255,0);
                simpleLineSymbol.Color = rgbcolor;
                simpleLineSymbol.Width = 5;
                lineElement.Symbol = simpleLineSymbol;
                IElement element = lineElement as IElement;
                element.Geometry = bounddary;
                graphicsContainer.AddElement(element, 0);
            }
            // ˢ�µ�ͼ
            activeView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolGetBoundary.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolGetBoundary.OnMouseUp implementation
        }
        #endregion
    }
}
