using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ArcGIS_Poisonous.Tools
{
    /// <summary>
    /// 地图操作类
    /// </summary>
    public class MapOperation
    {
        #region 获取图层要方法

        /// <summary>
        /// 获取图层列表方法
        /// </summary>
        /// <param name="layer">图层信息</param>
        /// <param name="featureClassList">需要返回的图层列表</param>
        public static void GetListFeature(ILayer layer,ref List<IFeatureClass> featureClassList)
        {
            try
            {
                // 使用ICompositeLayer获取图层信息，若不是图层组，得到的结果为空
                ICompositeLayer compositeLayer = layer as ICompositeLayer;
               
                if (compositeLayer == null)
                {
                    IFeatureLayer featureLayer = layer as IFeatureLayer;
                    if (!featureClassList.Contains(featureLayer.FeatureClass))
                    {
                        featureClassList.Add(featureLayer.FeatureClass);
                    }
                }
                else
                {
                    for (int i = 0; i < compositeLayer.Count; i++)
                    {
                        GetListFeature(compositeLayer.get_Layer(i), ref featureClassList);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("namespace:ArcGIS_Poisonous.Tools,class:MapOperation,method:GetListFeature.\n ErrorMessage:" + ex.Message);
            }
        }

        #endregion

        #region 获取当前地图中所有图层的要素方法

        /// <summary>
        /// 通过IMap获取图层要素信息
        /// </summary>
        /// <param name="map">当前地图的Map</param>
        /// <returns>返回图层要素列表</returns>
        public static List<IFeatureClass> GetFeatureClassListByMap(IMap map)
        {
            List<IFeatureClass> listFeatureClass = null;
            try
            {
                ILayer layer = null;
                IFeatureLayer featureLayer = null;
                listFeatureClass = new List<IFeatureClass>();
                for (int i = 0; i < map.LayerCount; i++)
                {
                    layer = map.get_Layer(i);
                    featureLayer = layer as IFeatureLayer;
                    GetListFeature(layer, ref listFeatureClass);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("namespace:ArcGIS_Poisonous.Tools,class:MapOperation,method:GetFeatureClassListByMap.\n ErrorMessage:" + ex.Message);
            }
            return listFeatureClass;
        }

        #endregion

        #region 根据图层名称获取图层

        public static IFeatureLayer GetFeatureLayerByName(IMap map, string featureLayerName)
        {
            IFeatureLayer featureLayer = null;
            try
            {
                ILayer layer = null;
                for (int i = 0; i < map.LayerCount; i++)
                {
                    layer = map.get_Layer(i);
                    featureLayer = GetFeatureLayerByName(layer, featureLayerName);
                    if (featureLayer != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("namespace:ArcGIS_Poisonous.Tools,class:MapOperation,method:GetFeatureLayerByName.\n ErrorMessage:" + ex.Message);
            }

            return featureLayer;
        }

        public static IFeatureLayer GetFeatureLayerByName(ILayer layer, string featureLayerName)
        {
            IFeatureLayer featureLayer = null;
            ICompositeLayer compositeLayer = layer as ICompositeLayer;
            if (compositeLayer == null)
            {
                IFeatureLayer currentFeatureLayer = layer as IFeatureLayer;
                if (currentFeatureLayer.FeatureClass.AliasName == featureLayerName)
                {
                    featureLayer = currentFeatureLayer;
                    return featureLayer;
                }
            }
            else
            {
                for (int i = 0; i < compositeLayer.Count; i++)
                {
                    GetFeatureLayerByName(compositeLayer.get_Layer(i), featureLayerName);
                }
            }
            return featureLayer;
        }

        #endregion

        #region 根据图层名获取图层

        /// <summary>
        /// 根据图层名获取图层
        /// </summary>
        /// <param name="pMap">地图文档</param>
        /// <param name="sLyrName">图层名</param>
        /// <returns></returns>
        public static ILayer GetLayerByName(IMap pMap, string sLyrName)
        {
            ILayer pLyr = null;
            ILayer pLayer = null;
            try
            {
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    pLyr = pMap.get_Layer(i);
                    if (pLyr.Name.ToUpper() == sLyrName.ToUpper())
                    {
                        pLayer = pLyr;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return pLayer;
        }

        #endregion

        #region 获取地图文档所有图层集合

        /// <summary>
        /// 获取当前地图文档所有图层集合
        /// </summary>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public static List<ILayer> GetLayers(IMap pMap)
        {
            ILayer plyr = null;
            List<ILayer> pLstLayers = null;
            try
            {
                pLstLayers = new List<ILayer>();
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    plyr = pMap.get_Layer(i);
                    if (!pLstLayers.Contains(plyr))
                    {
                        pLstLayers.Add(plyr);
                    }
                }
            }
            catch (Exception ex)
            { }
            return pLstLayers;
        }

        #endregion

        #region 计算两点之间X轴方向和Y轴方向上的距离

        /// <summary>
        /// 计算两点之间X轴方向和Y轴方向上的距离
        /// </summary>
        /// <param name="lastPoint"></param>
        /// <param name="firstPoint"></param>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <returns></returns>
        public static bool CalDistance(IPoint lastPoint, IPoint firstPoint, out double deltaX, out double deltaY) {
            deltaX = 0;
            deltaY = 0;
            if (lastPoint == null || firstPoint == null)
            {
                return false;
            }
            deltaX = lastPoint.X - firstPoint.X;
            deltaY = lastPoint.Y - firstPoint.Y;
            return true;
        }

        #endregion

        #region 单位转换

        /// <summary>
        /// 单位转换
        /// </summary>
        /// <param name="activeView"></param>
        /// <param name="pixelUnits"></param>
        /// <returns></returns>
        public static double ConvertPixelsToMapUnits(IActiveView activeView, double pixelUnits)
        {
            int pixelExtent = activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame().right -
                              activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame().left;
            double realWorldDisplayExtent = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return pixelUnits * sizeOfOnePixel;
        }

        #endregion

        #region 获取选择要素

        /// <summary>
        /// 获取选择要素
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        public static IFeatureCursor GetSelectedFeatures(IFeatureLayer featureLayer)
        {
            ICursor cursor = null;
            IFeatureCursor featureCursor = null;
            if (featureLayer == null)
            {
                return null;
            }
            IFeatureSelection featureSelection = featureLayer as IFeatureSelection;
            ISelectionSet selset = featureSelection.SelectionSet;
            if (selset.Count == 0)
            {
                return null;
            }
            selset.Search(null, false, out cursor);
            featureCursor = cursor as IFeatureCursor;
            return featureCursor;
        }

        #endregion

        #region 修改FeatureClass的Z、M值

        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="modifiedGeo"></param>
        /// <returns></returns>
        public static IGeometry ModifyGeomtryZMValue(IObjectClass featureClass, IGeometry modifiedGeo)
        {
            IFeatureClass trgFtCls = featureClass as IFeatureClass;
            if (trgFtCls == null) return null;
            string shapeFieldName = trgFtCls.ShapeFieldName;
            IFields fields = trgFtCls.Fields;
            int geometryIndex = fields.FindField(shapeFieldName);
            IField field = fields.get_Field(geometryIndex);
            IGeometryDef pGeometryDef = field.GeometryDef;
            IPointCollection pPointCollection = modifiedGeo as IPointCollection;
            if (pGeometryDef.HasZ)
            {
                IZAware pZAware = modifiedGeo as IZAware;
                pZAware.ZAware = true;
                IZ iz1 = modifiedGeo as IZ;
                //将Z值设置为0
                iz1.SetConstantZ(0);
            }
            else
            {
                IZAware pZAware = modifiedGeo as IZAware;
                pZAware.ZAware = false;
            }
            if (pGeometryDef.HasM)
            {
                IMAware pMAware = modifiedGeo as IMAware;
                pMAware.MAware = true;
            }
            else
            {
                IMAware pMAware = modifiedGeo as IMAware;
                pMAware.MAware = false;
            }
            return modifiedGeo;
        }

        #endregion
    }
}
