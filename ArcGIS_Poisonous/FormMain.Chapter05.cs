using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using stdole;
using ESRI.ArcGIS.Geodatabase;
using ArcGIS_Poisonous.Tools;

using ArcGIS_Poisonous.FormChapter05;

namespace ArcGIS_Poisonous
{
    /// <summary>
    /// Chapter05 地图制作
    /// </summary>
    public partial class ArcGisPoisonous : Form
    {
        /// <summary>
        /// 通用窗口1：选择图层与选择图层字段
        /// </summary>
        private FormCurrency mFormCurrency;

        #region 5.2 地图符号化

        #region 5.2.1 点状要素符号化

        /// <summary>
        /// 简单类型符号化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SimpleMaker_Click(object sender, EventArgs e)
        {
            // 获取目标图层
            ILayer layer = new FeatureLayerClass();
            layer = MainMapControl.get_Layer(0);
            IGeoFeatureLayer geoFeatrureLayer = layer as IGeoFeatureLayer;
            // 设置点符号
            ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbol();
            // 设置点符号样式为方形
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;
            // 设置点符号颜色
            simpleMarkerSymbol.Color = ColorTool.GetRgbColor(225, 100, 100);
            ISymbol symbol = simpleMarkerSymbol as ISymbol;

            // 更变符号样式
            ISimpleRenderer simpleRenderer = new SimpleRendererClass();
            simpleRenderer.Symbol = symbol;
            geoFeatrureLayer.Renderer = simpleRenderer as IFeatureRenderer;
            MainMapControl.Refresh();
            TOCControl.Update();
        }

        #endregion

        #region  5.2.2 线元素符号化

        /// <summary>
        /// 离散型符号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HashLine_Click(object sender, EventArgs e)
        {
            // 获取目标图层
            ILayer layer = new FeatureLayerClass();
            layer = MainMapControl.get_Layer(1);
            IGeoFeatureLayer geoFeatureLayer = layer as IGeoFeatureLayer;
            // 设置线符号
            IHashLineSymbol hashLineSymbol = new HashLineSymbolClass();
            ILineProperties lineProperties = hashLineSymbol as ILineProperties;
            lineProperties.Offset = 0;
            double[] dob = new double[6];
            for (int i = 0; i < dob.Length; i++)
            {
                dob[i] = i;
            }
            ITemplate template = new TemplateClass();
            template.Interval = 1;
            for (int i = 0; i < dob.Length; i += 2)
            {
                template.AddPatternElement(dob[i], dob[i + 1]);
            }
            lineProperties.Template = template;
            hashLineSymbol.Width = 2;
            // 设置单一线段的倾斜角度
            hashLineSymbol.Angle = 45;
            hashLineSymbol.Color = ColorTool.GetRgbColor(0, 0, 255);
            // 更改符号样式
            ISimpleRenderer simpleRenderer = new SimpleRendererClass();
            simpleRenderer.Symbol = hashLineSymbol as ISymbol;
            geoFeatureLayer.Renderer = simpleRenderer as IFeatureRenderer;
            MainMapControl.Refresh();
            MainMapControl.Update();
        }

        #endregion

        #region 5.2.3 面状要素符号化

        /// <summary>
        /// 多层填充符号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiLayerFill_Click(object sender, EventArgs e)
        {
            // 获取图层目标
            ILayer layer = new FeatureLayerClass();
            layer = MainMapControl.get_Layer(2);
            IGeoFeatureLayer geoFeatureLayer = layer as IGeoFeatureLayer;
            // 设置渐变色填充面符号
            IMultiLayerFillSymbol multiLayerFillSymbol = new MultiLayerFillSymbolClass();
            IGradientFillSymbol gradientFillSymbol = new GradientFillSymbolClass();
            IAlgorithmicColorRamp algorithmicColorRamp = new AlgorithmicColorRampClass();
            algorithmicColorRamp.FromColor = ColorTool.GetRgbColor(255, 0, 0);
            algorithmicColorRamp.ToColor = ColorTool.GetRgbColor(0, 255, 0);
            algorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            gradientFillSymbol.ColorRamp = algorithmicColorRamp;
            gradientFillSymbol.GradientAngle = 45;
            gradientFillSymbol.GradientPercentage = 0.9;
            gradientFillSymbol.Style = esriGradientFillStyle.esriGFSLinear;

            // 设置线填充面符号
            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDashDotDot;
            simpleLineSymbol.Width = 2;
            simpleLineSymbol.Color = ColorTool.GetRgbColor(255, 0, 0);

            ILineFillSymbol lineFillSymbol = new LineFillSymbol();
            lineFillSymbol.Angle = 45;
            lineFillSymbol.Separation = 10;
            lineFillSymbol.Offset = 5;
            lineFillSymbol.LineSymbol = simpleLineSymbol;

            // 组合填充符号
            multiLayerFillSymbol.AddLayer(gradientFillSymbol);
            multiLayerFillSymbol.AddLayer(lineFillSymbol);

            // 更改符号样式
            ISimpleRenderer simpleRenderer = new SimpleRendererClass();
            simpleRenderer.Symbol = multiLayerFillSymbol as ISymbol;
            geoFeatureLayer.Renderer = simpleRenderer as IFeatureRenderer;
            MainMapControl.Refresh();
            MainMapControl.Update();
        }

        #endregion

        #region 5.2.4 文本符号化

        /// <summary>
        /// 文本符号化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSymbol_Click(object sender, EventArgs e)
        {
            pMouseOperate = "TxtSymbol";
        }

        /// <summary>
        /// 添加文本操作
        /// </summary>
        partial void TxtSymbol(IMapControlEvents2_OnMouseDownEvent e)
        {
            IPoint point = new PointClass();
            point = MainMapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            // 设置文本格式
            ITextSymbol textSymbol = new TextSymbolClass();
            StdFont font = new stdole.StdFontClass();
            font.Name = "宋体";
            font.Size = 24;
            textSymbol.Font = font as IFontDisp;
            textSymbol.Angle = 0;
            // 文本由左向右对齐
            textSymbol.RightToLeft = false;
            // 垂直方向基线对齐
            textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
            // 文本两端对齐
            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull;

            textSymbol.Text = TextBox.Text;
            ITextElement textElement = new TextElementClass();
            textElement.Symbol = textSymbol;
            textElement.Text = textSymbol.Text;

            //获得一个图上坐标，且将文本添加到此位置
            IElement element = textElement as IElement;
            element.Geometry = point;
            MainMapControl.ActiveView.GraphicsContainer.AddElement(element, 0);
            MainMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, element, null);
        }

        #endregion

        #region 5.2.5 符号选择器

        /// <summary>
        /// 符号选择器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TOCControl_OnDoubleClick(object sender, ITOCControlEvents_OnDoubleClickEvent e)
        {
            esriTOCControlItem itemType = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap basicMap = null;
            ILayer layer = null;
            object unk = null;
            object data = null;
            TOCControl.HitTest(e.x, e.y, ref itemType, ref basicMap, ref layer, ref unk, ref data);
            if (e.button == 1)
            {
                if (itemType == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    // 获得图例
                    ILegendClass legendClass = (unk as ILegendGroup).get_Class((int)data);
                    // 创建符号选择器窗体
                    FormSymbolSelector formSymbolSelector = new FormSymbolSelector(legendClass, layer);
                    if (formSymbolSelector.ShowDialog() == DialogResult.OK)
                    {
                        // 局部更新主Map控件
                        MainMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        // 设置新的符号
                        legendClass.Symbol = formSymbolSelector.Symbol;
                        // 更新主Map控件和图层控件
                        this.MainMapControl.ActiveView.Refresh();
                        this.MainMapControl.Refresh();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region 5.3 地图标注

        #region 5.3.1 TextElement标注

        /// <summary>
        /// TextElement标注点击触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtElement_Click(object sender, EventArgs e)
        {
            if (mFormCurrency == null || mFormCurrency.IsDisposed)
            {
                mFormCurrency = new FormCurrency();
                mFormCurrency.Rander += new FormCurrency.EventHandle(FormCurrency_TextElement);

            }

            mFormCurrency.Map = MainMapControl.Map;
            mFormCurrency.FormTitleText = "TextElement标注";
            mFormCurrency.InitUI();
            mFormCurrency.Show();
        }

        public void FormCurrency_TextElement(string sFeatClsName, string sFieldName)
        {
            IFeatureLayer pFeatLyr = GetFeatLyrByName(MainMapControl.Map, sFeatClsName);
            TextElementLabel(pFeatLyr, sFieldName);
        }

        private void TextElementLabel(IFeatureLayer featureLayer, string fieldName)
        {
            IMap map = MainMapControl.Map;
            // 获得图层所有要素
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, true);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                IFields fields = feature.Fields;
                // 找出标注字段的索引号
                int index = fields.FindField(fieldName);
                // 得到要素的Envelope
                IEnvelope envelope = feature.Extent;
                IPoint point = new PointClass();
                point.PutCoords(envelope.XMin + envelope.Width / 2, envelope.YMin + envelope.Height / 2);
                // 新建字体对象
                stdole.IFontDisp font = new stdole.StdFontClass() as stdole.IFontDisp;
                font.Name = "arial";
                // 产生一个文本字符
                ITextSymbol textSymbol = new TextSymbolClass();
                // 设置文本符号大小
                textSymbol.Size = 20;
                textSymbol.Font = font;
                textSymbol.Color = ColorTool.GetRgbColor(255, 0, 0);
                // 产生一个文本对象
                ITextElement textElement = new TextElementClass();
                textElement.Text = feature.get_Value(index).ToString();
                textElement.ScaleText = true;
                textElement.Symbol = textSymbol;
                IElement element = textElement as IElement;
                element.Geometry = point;
                IActiveView activeView = map as IActiveView;
                IGraphicsContainer graphicsContainer = map as IGraphicsContainer;
                // 添加元素
                graphicsContainer.AddElement(element, 0);
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                point = null;
                element = null;
                feature = featureCursor.NextFeature();
            }
        }


        #region 待修改

        /// <summary>
        /// 根据图层名称获取图层  
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="sFeatLyrName"></param>
        /// <returns></returns>
        public IFeatureLayer GetFeatLyrByName(IMap pMap, string sFeatLyrName)
        {
            IFeatureLayer pFeatLyr = null;
            try
            {
                ILayer pLayer = null;
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    pLayer = pMap.get_Layer(i);
                    pFeatLyr = GetFeatLyrByName(pLayer, sFeatLyrName);
                    if (pFeatLyr != null) break;
                }
            }
            catch (Exception ex)
            {
            }
            return pFeatLyr;
        }
        /// <summary>
        /// 由名称获取图层
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="sFeatLyrName"></param>
        /// <returns></returns>
        public IFeatureLayer GetFeatLyrByName(ILayer pLayer, string sFeatLyrName)
        {
            ILayer pLyr = null;
            IFeatureLayer pFeatureLyr = null;
            IFeatureLayer pFeatLyr = null;
            ICompositeLayer pComLyr = pLayer as ICompositeLayer;
            if (pComLyr == null)
            {
                pFeatLyr = pLayer as IFeatureLayer;
                if (pFeatLyr.FeatureClass.AliasName == sFeatLyrName)
                {
                    pFeatureLyr = pFeatLyr;
                    return pFeatureLyr;
                }
            }
            else
            {
                for (int i = 0; i < pComLyr.Count; i++)
                {
                    pLyr = pComLyr.get_Layer(i);
                    GetFeatLyrByName(pLyr, sFeatLyrName);
                }
            }
            return pFeatureLyr;
        }


        #endregion

        #endregion

        #region 5.3.2 Annotation注记

        private void txtAnnotation_Click(object sender, EventArgs e)
        {
            if (mFormCurrency == null || mFormCurrency.IsDisposed)
            {
                mFormCurrency = new FormCurrency();
                mFormCurrency.Rander += new FormCurrency.EventHandle(FormCurrency_Annotation);
            }

            mFormCurrency.Map = MainMapControl.Map;
            mFormCurrency.FormTitleText = "Annotation注记";
            mFormCurrency.InitUI();
            mFormCurrency.Show();
        }

        private void FormCurrency_Annotation(string featureClassName, string fieldName)
        {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            Annotation(featureLayer, fieldName);
        }

        private void Annotation(IFeatureLayer featureLayer, string fieldName)
        {
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            IAnnotateLayerPropertiesCollection annotateLayerPropertiesCollection = geoFeatureLayer.AnnotationProperties;
            annotateLayerPropertiesCollection.Clear();
            // 设置注记体格式
            ITextSymbol textSymbol = new TextSymbolClass();
            stdole.StdFont font = new stdole.StdFontClass();
            font.Name = "verdana";
            font.Size = 10;
            textSymbol.Font = font as stdole.IFontDisp;
            // 设置注记放置格式
            ILineLabelPosition lineLabelposition = new LineLabelPositionClass();
            lineLabelposition.Parallel = false;
            lineLabelposition.Perpendicular = true;
            ILineLabelPlacementPriorities lineLabelPlacementPriorities = new LineLabelPlacementPrioritiesClass();
            IBasicOverposterLayerProperties basicOverposterLayerProperties = new BasicOverposterLayerProperties();
            basicOverposterLayerProperties.FeatureType = esriBasicOverposterFeatureType.esriOverposterPolyline;
            // 设置注记文本摆设路径权重
            basicOverposterLayerProperties.LineLabelPlacementPriorities = lineLabelPlacementPriorities;
            // 控制文本的排放位置
            basicOverposterLayerProperties.LineLabelPosition = lineLabelposition;
            ILabelEngineLayerProperties labelEngineLayerProperties = new LabelEngineLayerPropertiesClass();
            // 设置注记文本的放置方式和文字间冲突的处理方式等
            labelEngineLayerProperties.Symbol = textSymbol;
            labelEngineLayerProperties.BasicOverposterLayerProperties = basicOverposterLayerProperties;
            // 输入VBScript或者JavaScript语言，设置要标注的字段
            labelEngineLayerProperties.Expression = "[" + fieldName + "]";
            IAnnotateLayerProperties annotateLayerProperties = labelEngineLayerProperties as IAnnotateLayerProperties;
            annotateLayerPropertiesCollection.Add(annotateLayerProperties);
            geoFeatureLayer.DisplayAnnotation = true;
            MainMapControl.Refresh(esriViewDrawPhase.esriViewBackground, null, null);
            MainMapControl.Update();
        }

        #endregion

        #region 5.3.3 MapTip显示

        private void txtmapTips_Click(object sender, EventArgs e)
        {
            if (mFormCurrency == null || mFormCurrency.IsDisposed)
            {
                mFormCurrency = new FormCurrency();
                mFormCurrency.Rander += new FormCurrency.EventHandle(FormCurrency_MapTips);
            }
            mFormCurrency.FormTitleText = "MapTips显示";
            mFormCurrency.Map = MainMapControl.Map;
            mFormCurrency.InitUI();
            mFormCurrency.Show();
        }

        private void FormCurrency_MapTips(string featureClassName, string fieldName)
        {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            MapTips(featureLayer, fieldName);
        }

        private void MapTips(IFeatureLayer featureLayer, string fieldName)
        {
            ILayer layer = featureLayer;
            layer.ShowTips = true;
            ILayerFields layerFields = layer as ILayerFields;
            for (int i = 0; i < layerFields.FieldCount; i++)
            {
                IField field = layerFields.get_Field(i);
                if (field.Name == fieldName)
                {
                    featureLayer.DisplayField = fieldName;
                    break;
                }
            }
            MainMapControl.ShowMapTips = true;

        }

        #endregion

        #endregion

        #region 5.4 专题地图

        #region 单一符号化

        Form1 mFormCurrency2;

        private void SingleSymbol_Click(object sender, EventArgs e)
        {
            if (mFormCurrency2 == null || mFormCurrency2.IsDisposed)
            {
                mFormCurrency2 = new Form1();
                mFormCurrency2.Rander += new Form1.EventHandle(FormCurrency_SingleSymbol);
            }
            mFormCurrency2.FormTitleText = "单一符号化";
            mFormCurrency2.Map = MainMapControl.Map;
            mFormCurrency2.InitUI();
            mFormCurrency2.Show();
        }

        private void FormCurrency_SingleSymbol(string featureClassName, string fieldName) {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            SingleSymbolRender(featureLayer, fieldName);
        }

        private void SingleSymbolRender(IFeatureLayer featureLayer, string fieldName)
        {
            esriGeometryType types = featureLayer.FeatureClass.ShapeType;
            ISimpleRenderer pSimRender = new SimpleRendererClass();
            if (types == esriGeometryType.esriGeometryPolygon)
            {
                ISimpleFillSymbol pSimFillSym = new SimpleFillSymbolClass();
               // pSimFillSym.Color = pRgbColor;
                pSimRender.Symbol = pSimFillSym as ISymbol; // 设置渲染的样式 
            }
            else if (types == esriGeometryType.esriGeometryPoint)
            {
                ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                //pSimpleMarkerSymbol.Color = pRgbColor;
                pSimRender.Symbol = pSimpleMarkerSymbol as ISymbol;
            }
            else if (types == esriGeometryType.esriGeometryPolyline)
            {
                ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
                //pSimpleLineSymbol.Color = pRgbColor;
                pSimRender.Symbol = pSimpleLineSymbol as ISymbol;
            }
            IGeoFeatureLayer pGeoFeatLyr = featureLayer as IGeoFeatureLayer;
            pGeoFeatLyr.Renderer = pSimRender as IFeatureRenderer;
            (MainMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            TOCControl.Update();
        }

        #endregion


        #region 5.4.2 唯一值符号化

        /// <summary>
        /// 唯一值符号化点击触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UniqueValue_Click(object sender, EventArgs e)
        {
            if (mFormCurrency == null || mFormCurrency.IsDisposed)
            {
                mFormCurrency = new FormCurrency();
                mFormCurrency.Rander += new FormCurrency.EventHandle(formUniqueValueRen_UniqueValueRender);
            }

            mFormCurrency.FormTitleText = "唯一值符号化";
            mFormCurrency.Map = MainMapControl.Map;
            mFormCurrency.InitUI();

            mFormCurrency.Show();
        }

        private void formUniqueValueRen_UniqueValueRender(string featureClassName, string fieldName)
        {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            UniqueValueRenderers(featureLayer, fieldName);
        }

        private void UniqueValueRenderers(IFeatureLayer featureLayer, string fieldName)
        {
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            ITable table = featureLayer as ITable;
            IUniqueValueRenderer uniqueValueRenderer = new UniqueValueRendererClass();
            int intFieldNumber = table.FindField(fieldName);
            // 设置唯一值符号化的关键字段为一个
            uniqueValueRenderer.FieldCount = 1;
            // 设置唯一值符号化的第一个关键字段
            // uniqueValueRenderer.set_Field(0,fieldName);
            uniqueValueRenderer.Field[0] = fieldName;

            // 声明一条色带
            IRandomColorRamp randomColorRamp = new RandomColorRamp();
            randomColorRamp.StartHue = 0;
            randomColorRamp.MinValue = 0;
            randomColorRamp.MinSaturation = 15;
            randomColorRamp.EndHue = 360;
            randomColorRamp.MaxValue = 100;
            randomColorRamp.MaxSaturation = 30;
            // 根据渲染字段的值的个数，设置一组随机颜色，如某一字段有5个值，则创建5个随机颜色与之匹配
            IQueryFilter queryFilter = new QueryFilterClass();
            randomColorRamp.Size = featureLayer.FeatureClass.FeatureCount(queryFilter);
            bool success;
            randomColorRamp.CreateRamp(out success);
            IEnumColors enumColorsRamp = randomColorRamp.Colors;
            IColor nextUniqueColor = null;

            // 查询字段的值
            queryFilter = new QueryFilterClass();
            queryFilter.AddField(fieldName);
            ICursor cursor = table.Search(queryFilter, true);
            IRow nextRow = cursor.NextRow();
            object codeValue = null;
            IRowBuffer nextRowBuffer = null;
            while (nextRow != null)
            {
                nextRowBuffer = nextRow as IRowBuffer;
                // 获取渲染字段的每一个值
                codeValue = nextRowBuffer.get_Value(intFieldNumber);
                nextUniqueColor = enumColorsRamp.Next();
                if (nextUniqueColor == null)
                {
                    enumColorsRamp.Reset();
                    nextUniqueColor = enumColorsRamp.Next();
                }
                IFillSymbol pFillSymbol = null;
                ILineSymbol pLineSymbol;
                IMarkerSymbol pMarkerSymbol;
                switch (featureLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        {
                            pFillSymbol = new SimpleFillSymbolClass();
                            pFillSymbol.Color = nextUniqueColor;
                            uniqueValueRenderer.AddValue(codeValue.ToString(), "", pFillSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            nextRow = cursor.NextRow();
                            break;
                        }
                    case esriGeometryType.esriGeometryPolyline:
                        {
                            pLineSymbol = new SimpleLineSymbolClass();
                            pLineSymbol.Color = nextUniqueColor;
                            uniqueValueRenderer.AddValue(codeValue.ToString(), "", pLineSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            nextRow = cursor.NextRow();
                            break;
                        }
                    case esriGeometryType.esriGeometryPoint:
                        {
                            pMarkerSymbol = new SimpleMarkerSymbolClass();
                            pMarkerSymbol.Color = nextUniqueColor;
                            uniqueValueRenderer.AddValue(codeValue.ToString(), "", pMarkerSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            nextRow = cursor.NextRow();
                            break;
                        }
                }
            }
            geoFeatureLayer.Renderer = uniqueValueRenderer as IFeatureRenderer;
            MainMapControl.Refresh();
            MainMapControl.Update();
        }



        #endregion

        #region 5.4.3 唯一值多段符号化

        private void UniqueValuesManyFileds_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #endregion

    }

}
