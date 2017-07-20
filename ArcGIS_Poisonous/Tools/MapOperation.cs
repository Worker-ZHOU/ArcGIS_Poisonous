using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;


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
    }
}
