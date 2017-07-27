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
            // �ж�MapControl�Ƿ���ͼ��
            if (m_HookHelper.FocusMap.LayerCount <= 0)
            {
                MessageBox.Show("���ȼ���ͼ������");
                return;
            }
            // �޸������ʽ
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
            // ��ȡ���λ�ò�ץ��Ϊ��ͼ��Ҫ��
            IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            // ��ȡ��ͼ�е�ͼ��
            IFeatureLayer featureLayer = activeView.FocusMap.get_Layer(0) as IFeatureLayer;
            if (featureLayer == null)
            {
                return;
            }
            IFeatureClass featureCLass = featureLayer.FeatureClass;
            // ���е����ѡ��Ҫ��
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = point;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor featureCursor = featureCLass.Search(spatialFilter, false);
            // ��õ����ѯ��Ҫ�� 
            IFeature feature = featureCursor.NextFeature();
            if (feature != null && feature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                map.ClearSelection();
                IRelationalOperator relationalOperator = feature.Shape as IRelationalOperator;
                // ���ñ�ѡ��Ҫ�ص���ɫ
                IRgbColor rgbColor = ColorTool.GetRgbColor(255, 0, 0);
                IFeatureSelection featureSelection = featureLayer as IFeatureSelection;
                featureSelection.SelectionColor = rgbColor;
                // �����ڽ�Ҫ�أ���֮���ͼ��ѡ����
                IFeatureCursor nearFeatureCursor = featureLayer.Search(null, false);
                IFeature nearFeature = nearFeatureCursor.NextFeature();

                // ����ͼ��������Ҫ�ؽ����ڽ��ж�
                while (nearFeature != null)
                {
                    if (relationalOperator.Touches(nearFeature.Shape))
                    {
                        // ����Ա�Ҫ���뵱ǰѡ��Ҫ���ڽӣ�����뵽��ͼѡ����
                        map.SelectFeature(featureLayer, nearFeature);
                    }
                    nearFeature = nearFeatureCursor.NextFeature();
                } 
            }

            // ˢ�µ�ͼ
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
