using ArcGIS_Poisonous.FormChapter05;
using ArcGIS_Poisonous.Tools;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using stdole;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArcGIS_Poisonous
{
    /// <summary>
    /// Chapter05 地图制作
    /// </summary>
    public partial class ArcGisPoisonous : Form
    {

        #region 定义变量

        /// <summary>
        /// 单一符号化窗口
        /// </summary>
        private FormSingleRender mFormSingleRender;

        /// <summary>
        /// 唯一值多段符号化窗口
        /// </summary>
        private FormUniqueManyValueRenderer mFormUniqueManyValueRenderer;

        /// <summary>
        /// 通用窗口1：选择图层与选择图层字段
        /// </summary>
        private FormCurrency mFormCurrency;

        /// <summary>
        /// 通用窗口2：
        /// </summary>
        private FormCurrency2 mFormCurrency2;

        /// <summary>
        /// 通用窗口3
        /// </summary>
        private FormCurrency3 mFormCurrency3;

        /// <summary>
        ///  地图整饰与输出窗口
        /// </summary>
        private FormSymbology mFormSymbology;

        /// <summary>
        /// 统计图表枚举类型
        /// </summary>
        private ChartEnumTpye mChartEnumTpye = ChartEnumTpye.UnKnow;

        /// <summary>
        /// 添加地图要素类型
        /// </summary>
        private EnumMapSurroundType mEnumMapSurroundType = EnumMapSurroundType.None;

        #endregion

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

        #region 5.4.1 单一符号化

        private void SingleSymbol_Click(object sender, EventArgs e)
        {
            if (mFormSingleRender == null || mFormSingleRender.IsDisposed)
            {
                mFormSingleRender = new FormSingleRender();
                mFormSingleRender.Rander += new FormSingleRender.EventHandle(FormCurrency_SingleSymbol);
            }
            mFormSingleRender.FormTitleText = "单一符号化";
            mFormSingleRender.Map = MainMapControl.Map;
            mFormSingleRender.InitUI();
            mFormSingleRender.Show();
        }

        private void FormCurrency_SingleSymbol(string featureClassName, IRgbColor fieldName) {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            SingleSymbolRender(featureLayer, fieldName);
        }

        private void SingleSymbolRender(IFeatureLayer featureLayer, IRgbColor rgbColor)
        {
            esriGeometryType types = featureLayer.FeatureClass.ShapeType;
            ISimpleRenderer pSimRender = new SimpleRendererClass();
            if (types == esriGeometryType.esriGeometryPolygon)
            {
                ISimpleFillSymbol pSimFillSym = new SimpleFillSymbolClass();
                pSimFillSym.Color = rgbColor;
                pSimRender.Symbol = pSimFillSym as ISymbol; // 设置渲染的样式 
            }
            else if (types == esriGeometryType.esriGeometryPoint)
            {
                ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                pSimpleMarkerSymbol.Color = rgbColor;
                pSimRender.Symbol = pSimpleMarkerSymbol as ISymbol;
            }
            else if (types == esriGeometryType.esriGeometryPolyline)
            {
                ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
                pSimpleLineSymbol.Color = rgbColor;
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
            if (mFormUniqueManyValueRenderer == null || mFormUniqueManyValueRenderer.IsDisposed)
            {
                mFormUniqueManyValueRenderer = new FormUniqueManyValueRenderer();
                mFormUniqueManyValueRenderer.Render += new FormUniqueManyValueRenderer.EventHandler(FormUniqueManyValueRenderer_UniqueValuesManyFileds);
            }
            mFormUniqueManyValueRenderer.Map = MainMapControl.Map;
            mFormUniqueManyValueRenderer.InitUI();
            mFormUniqueManyValueRenderer.Show();
        }

        private void FormUniqueManyValueRenderer_UniqueValuesManyFileds(string featureClassName, string[] fieldNames)
        {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            UniqueValueMany_fieldsRenderer(featureLayer, fieldNames);
        }

        private void UniqueValueMany_fieldsRenderer(IFeatureLayer featureLayer, string[] fieldNames)
        {
            IUniqueValueRenderer uniqueValueRenderer;
            IColor nextUniqueColor;
            IEnumColors enumRamp;
            ITable table;
            IRow nextRow;
            ICursor cursor;
            IQueryFilter queryFilter;
            IRandomColorRamp randomColorRamp = new RandomColorRampClass();
            randomColorRamp.StartHue = 0;
            randomColorRamp.MinValue = 0;
            randomColorRamp.MinSaturation = 15;
            randomColorRamp.EndHue = 360;
            randomColorRamp.MaxValue = 100;
            randomColorRamp.MinSaturation = 30;
            // 根据渲染字段值的个数，设置一组随机颜色，如某一字段有5个不同值，则创建5个随机颜色与之匹配
            IQueryFilter queryFilter2 = new QueryFilterClass();
            randomColorRamp.Size = featureLayer.FeatureClass.FeatureCount(queryFilter2);
            bool success = false;
            randomColorRamp.CreateRamp(out success);
            // 所选字段为两个时
            if (fieldNames.Length == 2)
            {
                string fieldName1 = fieldNames[0];
                string fieldName2 = fieldNames[1];
                IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
                uniqueValueRenderer = new UniqueValueRendererClass();
                table = geoFeatureLayer as ITable;
                int fieldNumber = table.FindField(fieldName1);
                int fieldNumber2 = table.FindField(fieldName2);

                // 设置渲染字段的个数
                uniqueValueRenderer.FieldCount = 2;
                // 设置渲染的第一个字段
                uniqueValueRenderer.set_Field(0, fieldName1);
                // 设置渲染的第二个字段
                uniqueValueRenderer.set_Field(1, fieldName2);

                enumRamp = randomColorRamp.Colors;
                nextUniqueColor = null;
                // 获取渲染字段的每个属性值
                queryFilter = new QueryFilterClass();
                queryFilter.AddField(fieldName1);
                queryFilter.AddField(fieldName2);
                cursor = table.Search(queryFilter, true);
                nextRow = cursor.NextRow();
                // 这里的codeValue可以定位成object类型
                string codeValue;
                while (nextRow != null)
                {
                    codeValue = nextRow.get_Value(fieldNumber).ToString() + uniqueValueRenderer.FieldDelimiter + nextRow.get_Value(fieldNumber2).ToString();
                    nextUniqueColor = enumRamp.Next();
                    if (nextUniqueColor == null)
                    {
                        enumRamp.Reset();
                        nextUniqueColor = enumRamp.Next();
                    }
                    IFillSymbol fillSymbol;
                    ILineSymbol lineSymbol;
                    IMarkerSymbol markerSymbol;
                    switch (geoFeatureLayer.FeatureClass.ShapeType)
                    {     
                        case esriGeometryType.esriGeometryPoint:
                            markerSymbol = new SimpleMarkerSymbolClass();
                            markerSymbol.Color = nextUniqueColor;
                            uniqueValueRenderer.AddValue(codeValue, fieldName1 + " " + fieldName2, markerSymbol as ISymbol);
                            break;
                       
                        case esriGeometryType.esriGeometryPolyline:
                            lineSymbol = new SimpleLineSymbolClass();
                            lineSymbol.Color = nextUniqueColor;
                            uniqueValueRenderer.AddValue(codeValue, fieldName1 + " " + fieldName2, lineSymbol as ISymbol);
                            break;
                 
                        case esriGeometryType.esriGeometryPolygon:
                            fillSymbol = new SimpleFillSymbolClass();
                            fillSymbol.Color = nextUniqueColor;
                            // 渲染字段组合值对应的符号
                            uniqueValueRenderer.AddValue(codeValue, fieldName1 + " " + fieldName2, fillSymbol as ISymbol);
                            break;

                        default:
                            break;
                    }
                    nextRow = cursor.NextRow();
                }
                geoFeatureLayer.Renderer = uniqueValueRenderer as IFeatureRenderer;
                MainMapControl.Refresh();
                MainMapControl.Update();
                TOCControl.Update();
            }

        }




        #endregion

        #region 5.4.4 分级色彩符号化

        private void GraduatedColor_Click(object sender, EventArgs e)
        {
            if (mFormCurrency2 == null || mFormCurrency2.IsDisposed)
            {
                mFormCurrency2 = new FormCurrency2();
                mFormCurrency2.Render += new FormCurrency2.EventHandler(FormCurrency2_GraduatedColor);
            }
            mFormCurrency2.Map = MainMapControl.Map;
            mFormCurrency2.FormTitleText = "分级颜色";
            mFormCurrency2.Show();
        }

        private void FormCurrency2_GraduatedColor(String featureClassName, String fieldName, int intNumClass) {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            GraduatedColors(featureLayer, fieldName, intNumClass);
        }

        private void GraduatedColors(IFeatureLayer featureLayer, string fieldName, int intNumClass)
        {
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            object dataFrequency;
            object dataValues;
            bool ok;
            int breakIndex;
            ITable table = geoFeatureLayer.FeatureClass as ITable;
            ITableHistogram tableHistogram = new BasicTableHistogramClass();
            IBasicHistogram basicHistogram = tableHistogram as IBasicHistogram;
            tableHistogram.Field = fieldName;
            tableHistogram.Table = table;
            // 获取渲染字段的值及其出现的频率
            basicHistogram.GetHistogram(out dataValues, out dataFrequency);
            IClassifyGEN classify = new EqualIntervalClass();
            try
            {
                // 根据获取字段的值和出现的频率对其进行等级划分
                classify.Classify(dataValues, dataFrequency, ref intNumClass);
            }
            catch (Exception ex)
            {

            }
            // 返回一个数组
            double[] classes = classify.ClassBreaks as double[];
            int classesCount = classes.GetUpperBound(0);
            IClassBreaksRenderer classBreaksRenderer = new ClassBreaksRendererClass();
            // 设置分级字段
            classBreaksRenderer.Field = fieldName;
            // 设置分级数目
            classBreaksRenderer.BreakCount = classesCount;
            // 分级后的图例是否按升级顺序排序
            classBreaksRenderer.SortClassesAscending = true;
            // 设置分级着色所需色带的起止颜色
            IHsvColor fromColor = new HsvColorClass();
            // 黄色
            fromColor.Hue = 0;
            fromColor.Saturation = 50;
            fromColor.Value = 96;
            IHsvColor toColor = new HsvColorClass();
            toColor.Hue = 80;
            toColor.Saturation = 100;
            toColor.Value = 96;
            // 产生颜色带对象
            IAlgorithmicColorRamp algorithmicColorRamp = new AlgorithmicColorRampClass();
            algorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            algorithmicColorRamp.FromColor = fromColor;
            algorithmicColorRamp.ToColor = toColor;
            algorithmicColorRamp.Size = classesCount;
            algorithmicColorRamp.CreateRamp(out ok);
            // 获得颜色
            IEnumColors enumColors = algorithmicColorRamp.Colors;
            // 需注意的是分级着色对象中Symbol和break的下标都是从0开始
            for (breakIndex = 0; breakIndex < classesCount ; breakIndex++)
            {
                IColor color = enumColors.Next();
                switch (geoFeatureLayer.FeatureClass.ShapeType)
                {
                   
                    case esriGeometryType.esriGeometryPoint:
                        ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Color = color;
                        // 设置填充符号
                        classBreaksRenderer.set_Symbol(breakIndex, simpleMarkerSymbol as ISymbol);
                        // 设定每一分级的分级断点
                        classBreaksRenderer.set_Break(breakIndex, classes[breakIndex + 1]);
                        break;

                    case esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                        simpleLineSymbol.Color = color;
                        // 设置填充符号
                        classBreaksRenderer.set_Symbol(breakIndex, simpleLineSymbol as ISymbol);
                        // 设定每一分级的分级断点
                        classBreaksRenderer.set_Break(breakIndex, classes[breakIndex + 1]);
                        break;
                    
                    case esriGeometryType.esriGeometryPolygon:
                        ISimpleFillSymbol simpleFileSymbol = new SimpleFillSymbolClass();
                        simpleFileSymbol.Color = color;
                        simpleFileSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        // 设置填充符号
                        classBreaksRenderer.set_Symbol(breakIndex, simpleFileSymbol as ISymbol);
                        // 设定每一分级的分级断点
                        classBreaksRenderer.set_Break(breakIndex, classes[breakIndex + 1]);
                        break;
                   
                    default:
                        break;
                }
            }
            geoFeatureLayer.Renderer = classBreaksRenderer as IFeatureRenderer;
            MainMapControl.Refresh();
            TOCControl.Update();
        }

        #endregion

        #region 5.4.5 分级符号化

        private void Graduatedsymbol_Click(object sender, EventArgs e)
        {
            if (mFormCurrency2 == null || mFormCurrency2.IsDisposed)
            {
                mFormCurrency2 = new FormCurrency2();
                mFormCurrency2.Render += new FormCurrency2.EventHandler(FormCurrency_Graduatedsymbol);
            }
            mFormCurrency2.Map = MainMapControl.Map;
            mFormCurrency2.FormTitleText = "分级符号化";
            mFormCurrency2.Show();

        }

        private void FormCurrency_Graduatedsymbol(string featureClassName, string fieldName, int numClasses)
        {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            GraduatedSymbols(featureLayer,fieldName,numClasses);
        }

        private void GraduatedSymbols(IFeatureLayer featureLayer, string fieldName, int numClasses)
        {
            ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
            simpleMarkerSymbol.Color = ColorTool.GetRgbColor(255, 100, 100);
            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Color = ColorTool.GetRgbColor(255, 100, 100);
            int breakIndex;
            object dataFrequency;
            object dataValues;
            // 获得要着色的图层
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            ITable table = geoFeatureLayer.FeatureClass as ITable;
            ITableHistogram tableHistogram = new BasicTableHistogramClass();
            IBasicHistogram basicHistogram = tableHistogram as IBasicHistogram;
            tableHistogram.Field = fieldName;
            tableHistogram.Table = table;
            // 获取渲染字段的值及出现的频率
            basicHistogram.GetHistogram(out dataValues, out dataFrequency);
            IClassifyGEN classify = new EqualIntervalClass();
            try
            {
                // 根据获取字段的值和出现的频率对其进行风机划分
                classify.Classify(dataValues, dataFrequency, ref numClasses);
            }
            catch (Exception ex)
            {

            }
            // 返回一个数组
            double[] classes = classify.ClassBreaks as double[];
            int classesCount = classes.GetUpperBound(0);
            IClassBreaksRenderer classBreakRenderer = new ClassBreaksRendererClass();
            // 设置分级字段
            classBreakRenderer.Field = fieldName;
            // 设置着色对象的分组数目
            classBreakRenderer.BreakCount = classesCount;
            // 升序排序
            classBreakRenderer.SortClassesAscending = true;
            // 需要注意的时分级着色对象中的symbol和break的下标都是从0开始
            double symbolSizeOrigin = 5.0;
            if (classesCount <= 5)
            {
                symbolSizeOrigin = 8;
            }
            if (classesCount < 10 && classesCount > 5)
            {
                symbolSizeOrigin = 7;
            }
            IFillSymbol backgroundSymbol = new SimpleFillSymbolClass();
            backgroundSymbol.Color = ColorTool.GetRgbColor(255, 255, 100);
            //不同的要素类型，生成不同的分级符号
            switch (geoFeatureLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPolygon:
                    {
                        for (breakIndex = 0; breakIndex <= classesCount - 1; breakIndex++)
                        {
                            classBreakRenderer.set_Break(breakIndex, classes[breakIndex + 1]);
                            classBreakRenderer.BackgroundSymbol = backgroundSymbol;
                            simpleMarkerSymbol.Size = symbolSizeOrigin + breakIndex * symbolSizeOrigin / 3.0d;
                            classBreakRenderer.set_Symbol(breakIndex, (ISymbol)simpleMarkerSymbol);
                        }
                        break;
                    }
                case esriGeometryType.esriGeometryPolyline:
                    {
                        for (breakIndex = 0; breakIndex <= classesCount - 1; breakIndex++)
                        {
                            classBreakRenderer.set_Break(breakIndex, classes[breakIndex + 1]);
                            simpleLineSymbol.Width = symbolSizeOrigin / 5 + breakIndex * (symbolSizeOrigin / 5) / 5.0d;
                            classBreakRenderer.set_Symbol(breakIndex, (ISymbol)simpleLineSymbol);
                        }
                        break;
                    }
                case esriGeometryType.esriGeometryPoint:
                    {
                        for (breakIndex = 0; breakIndex <= classesCount - 1; breakIndex++)
                        {
                            classBreakRenderer.set_Break(breakIndex, classes[breakIndex + 1]);
                            simpleMarkerSymbol.Size = symbolSizeOrigin + breakIndex * symbolSizeOrigin / 3.0d;
                            classBreakRenderer.set_Symbol(breakIndex, (ISymbol)simpleMarkerSymbol);
                        }
                        break;
                    }
            }
            geoFeatureLayer.Renderer = classBreakRenderer as IFeatureRenderer;
            MainMapControl.ActiveView.Refresh();
            TOCControl.Update();
        }


        #endregion

        #region 5.4.6 比例符号化

        private void Proportionalsymbol_Click(object sender, EventArgs e)
        {
            if (mFormCurrency == null || mFormCurrency.IsDisposed)
            {
                mFormCurrency = new FormCurrency();
                mFormCurrency.Rander += new FormCurrency.EventHandle(FormCurrency_Proportionalsymbol);
            }
            mFormCurrency.Map = MainMapControl.Map;
            mFormCurrency.FormTitleText = "比例符号化";
            mFormCurrency.InitUI();
            mFormCurrency.Show();
        }

        private void FormCurrency_Proportionalsymbol(string featureClassName, string fieldName) {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map, featureClassName);
            Proportional(featureLayer, fieldName);
        }

        private void Proportional(IFeatureLayer featureLayer, string fieldName)
        {
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            ITable table = featureLayer as ITable;
            ICursor cursor = table.Search(null, true);
            // 利用IDataStatistics和IStatisticsResults获取渲染字段的统计值，最主要时获取最大值和最小值
            IDataStatistics dataStatistics = new DataStatisticsClass();
            dataStatistics.Cursor = cursor;
            dataStatistics.Field = fieldName;
            IStatisticsResults statisticsResults = dataStatistics.Statistics;
            if (statisticsResults != null)
            {
                // 设置渲染背景色
                IFillSymbol fillSymbol = new SimpleFillSymbolClass();
                fillSymbol.Color = ColorTool.GetRgbColor(155, 255, 0);
                // 设置比例符号的样式
                ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
                simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSDiamond;
                simpleMarkerSymbol.Size = 3;
                simpleMarkerSymbol.Color = ColorTool.GetRgbColor(255, 90, 0);
                IProportionalSymbolRenderer proportionalSymbolRenderer = new ProportionalSymbolRendererClass();
                // 设置渲染单位
                proportionalSymbolRenderer.ValueUnit = esriUnits.esriUnknownUnits;
                // 设置渲染字段
                proportionalSymbolRenderer.Field = fieldName;
                // 是否使用Flannery补偿
                proportionalSymbolRenderer.FlanneryCompensation = false;
                // 获取渲染字段的最小值
                proportionalSymbolRenderer.MinDataValue = statisticsResults.Minimum;
                // 获取渲染字段的最大值
                proportionalSymbolRenderer.MaxDataValue = statisticsResults.Maximum;
                proportionalSymbolRenderer.BackgroundSymbol = fillSymbol;
                // 设置渲染字段最小值的渲染符号，其余值的符号根据此符号产生
                proportionalSymbolRenderer.MinSymbol = simpleMarkerSymbol as ISymbol;
                // 控制TOC控件中显示的数目
                proportionalSymbolRenderer.LegendSymbolCount = 5;
                // 生成例图
                proportionalSymbolRenderer.CreateLegendSymbols();
                geoFeatureLayer.Renderer = proportionalSymbolRenderer as IFeatureRenderer;
            }
            MainMapControl.Refresh();
            TOCControl.Update();
        }

        #endregion

        #region 5.4.7 点密度符号化

        private void Dotdensitys_Click(object sender, EventArgs e)
        {
            if (mFormCurrency2 == null || mFormCurrency2.IsDisposed)
            {
                mFormCurrency2 = new FormCurrency2();
                mFormCurrency2.Render += new FormCurrency2.EventHandler(FormCurrency2_Dotdensitys);
            }
            mFormCurrency2.Map = MainMapControl.Map;
            mFormCurrency2.FormTitleText = "点密度符号化";
            mFormCurrency2.Show();
        }

        private void FormCurrency2_Dotdensitys(String featureClassName, String fieldName, int intNumClass) {
            IFeatureLayer featureLayer = MapOperation.GetFeatureLayerByName(MainMapControl.Map,featureClassName);
            _Dotdensitys(featureLayer, fieldName, intNumClass);
        }

        private void _Dotdensitys(IFeatureLayer featureLayer, string fieldName, int intNumClass)
        {
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            IDotDensityRenderer dotDensityRenderer = new DotDensityRendererClass();
            IRendererFields rendererFields = dotDensityRenderer as IRendererFields;
            // 设置渲染字段
            rendererFields.AddField(fieldName);
            // 设置填充背景色
            IDotDensityFillSymbol dotDensityFillSymbol = new DotDensityFillSymbolClass();
            dotDensityFillSymbol.DotSize = 3;
            dotDensityFillSymbol.BackgroundColor = ColorTool.GetRgbColor(0, 255, 0);
            // 设置渲染符号
            ISymbolArray symbolArray = dotDensityFillSymbol as ISymbolArray;
            ISimpleMarkerSymbol simpleMarkSymbol = new SimpleMarkerSymbolClass();
            simpleMarkSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            simpleMarkSymbol.Color = ColorTool.GetRgbColor(0,0,255);
            symbolArray.AddSymbol(simpleMarkSymbol as ISymbol);
            dotDensityRenderer.DotDensitySymbol = dotDensityFillSymbol;
            // 设置渲染密度，即每个点符号所代表的数值大小
            dotDensityRenderer.DotValue = intNumClass*500;
            // 创建图例
            dotDensityRenderer.CreateLegend();
            geoFeatureLayer.Renderer = dotDensityRenderer as IFeatureRenderer;

            MainMapControl.Refresh();
            TOCControl.Update();
        }

        #endregion

        #region 5.4.8 统计图表符号化

        private enum ChartEnumTpye:int
        {
            UnKnow = 0,
            PieChart = 1,
            BarChart = 2,
            StackChart = 3
        }

        private void Pie_Click(object sender, EventArgs e)
        {
            mChartEnumTpye = ChartEnumTpye.PieChart;
            if (mFormCurrency3 == null || mFormCurrency3.IsDisposed)
            {
                mFormCurrency3 = new FormCurrency3();
                mFormCurrency3.Render += new FormCurrency3.EventHandler(FormCurrency3_ChartRender);
            }
            mFormCurrency3.Map = MainMapControl.Map;
            mFormCurrency3.FormTitleText = "统计图表-饼分图";
            mFormCurrency3.Show();
        }

        private void Bar_Click(object sender, EventArgs e)
        {
            mChartEnumTpye = ChartEnumTpye.BarChart;
            if (mFormCurrency3 == null || mFormCurrency3.IsDisposed)
            {
                mFormCurrency3 = new FormCurrency3();
                mFormCurrency3.Render += new FormCurrency3.EventHandler(FormCurrency3_ChartRender);
            }
            mFormCurrency3.Map = MainMapControl.Map;
            mFormCurrency3.FormTitleText = "统计图表-条行图";
            mFormCurrency3.Show();
        }

        private void Stacked_Click(object sender, EventArgs e)
        {
            
                mChartEnumTpye = ChartEnumTpye.StackChart;
            if (mFormCurrency3 == null || mFormCurrency3.IsDisposed)
            {
                mFormCurrency3 = new FormCurrency3();
                mFormCurrency3.Render += new FormCurrency3.EventHandler(FormCurrency3_ChartRender);
            }
            mFormCurrency3.Map = MainMapControl.Map;
            mFormCurrency3.FormTitleText = "统计图表-堆叠图";
            mFormCurrency3.Show();

        }
         
        private void FormCurrency3_ChartRender(string sFeatClsName, Dictionary<string, IRgbColor> _dicFieldAndColor)
        {
            IFeatureLayer pFeatLyr = MapOperation.GetFeatureLayerByName(MainMapControl.Map, sFeatClsName);
            _ChartRenderer(pFeatLyr, _dicFieldAndColor);
        }

        private void _ChartRenderer(IFeatureLayer pFeatLyr, Dictionary<string, IRgbColor> _dicFieldAndColor)
        {
            IGeoFeatureLayer geoFeatureLayer = pFeatLyr as IGeoFeatureLayer;
            IChartRenderer chartRenderer = new ChartRendererClass();
            IRendererFields rendererFields = chartRenderer as IRendererFields;
            IFeatureCursor featureCursor = null;
            IDataStatistics dataStatistics = null;
            double max = 0;
            double temp = 0;
            IQueryFilter queryFilter = new QueryFilterClass();
            featureCursor = geoFeatureLayer.Search(queryFilter, true);

            // 遍历出所选择的第一个字段的最大值，作为设置专题图比例大小的依据
            foreach (KeyValuePair<string, IRgbColor> _keyValue in _dicFieldAndColor)
            {
                rendererFields.AddField(_keyValue.Key, _keyValue.Key);
                dataStatistics = new DataStatisticsClass();
                dataStatistics.Cursor = featureCursor as ICursor;
                dataStatistics.Field = _keyValue.Key;
                temp = dataStatistics.Statistics.Maximum;
                if (temp >= max)
                {
                    max = temp;
                }
            }
            IRgbColor rgbColor = null;
            IChartSymbol chartSymbol = null;
            IFillSymbol fillSymbol = null;
            IMarkerSymbol markerSymbol = null;
            IBarChartSymbol barChartSymbol = null;
            IPieChartSymbol pieChartSymbol = null;
            IStackedChartSymbol stackedChartSymbol = null;

            // 定义并设置渲染样式
            switch (mChartEnumTpye)
            {
                case ChartEnumTpye.PieChart:
                    pieChartSymbol = new PieChartSymbolClass();
                    // 说明饼图是否为顺时针方向
                    pieChartSymbol.Clockwise = true;
                    // 说明是否使用轮廓线
                    pieChartSymbol.UseOutline = true;

                    ILineSymbol lineSymbol = new SimpleLineSymbolClass();
                    lineSymbol.Color = ColorTool.GetRgbColor(100, 255, 30) as IColor;
                    lineSymbol.Width = 1;
                    pieChartSymbol.Outline = lineSymbol;
                    break;

                case ChartEnumTpye.BarChart:
                    barChartSymbol = new BarChartSymbolClass();
                    // 设置每个条形的宽度
                    barChartSymbol.Width = 6;

                    break;

                case ChartEnumTpye.StackChart:
                    stackedChartSymbol = new StackedChartSymbolClass();
                    // 设置每个堆叠图的宽度
                    stackedChartSymbol.Width = 6;
                    break;

                default:
                    break;
            }

            if (pieChartSymbol != null)
            {
                chartSymbol = pieChartSymbol as IChartSymbol;
                markerSymbol = pieChartSymbol as IMarkerSymbol;
                // 设置饼图的大小
                markerSymbol.Size = 30;
            }
            else if (barChartSymbol != null)
            {
                chartSymbol = barChartSymbol as IChartSymbol;
                markerSymbol = barChartSymbol as IMarkerSymbol;
                // 设置条形图的高度
                markerSymbol.Size = 30;
            }
            else if (stackedChartSymbol != null)
            {
                chartSymbol = stackedChartSymbol as IChartSymbol;
                markerSymbol = stackedChartSymbol as IMarkerSymbol;
                // 设置条形图的高度
                markerSymbol.Size = 30;
            }

            chartSymbol.MaxValue = max;
            ISymbolArray symbolArray = null;
            if (barChartSymbol != null)
            {
                symbolArray = barChartSymbol as ISymbolArray;
            }
            else if (stackedChartSymbol != null)
            {
                symbolArray = stackedChartSymbol as ISymbolArray;
            }
            else if (pieChartSymbol != null)
            {
                symbolArray = pieChartSymbol as ISymbolArray;
            }

            foreach (KeyValuePair<string,IRgbColor> _keyValue in _dicFieldAndColor)
            {
                // 获取渲染字段的颜色值
                rgbColor = _keyValue.Value;
                fillSymbol = new SimpleFillSymbolClass();
                fillSymbol.Color = rgbColor as IColor;
                symbolArray.AddSymbol(fillSymbol as ISymbol);
                if (barChartSymbol != null)
                {
                    chartRenderer.ChartSymbol = barChartSymbol as IChartSymbol;
                }
                else if (stackedChartSymbol != null)
                {
                    chartRenderer.ChartSymbol = stackedChartSymbol as IChartSymbol;
                }
                else if (pieChartSymbol != null)
                {
                    chartRenderer.ChartSymbol = pieChartSymbol as IChartSymbol;
                }

                fillSymbol = new SimpleFillSymbolClass();
                fillSymbol.Color = ColorTool.GetRgbColor(239, 228, 190);
                // 设置背景符号
                chartRenderer.BaseSymbol = fillSymbol as ISymbol;

                // 让符号处于图形中央
                chartRenderer.UseOverposter = false;
                chartRenderer.CreateLegend();
                geoFeatureLayer.Renderer = chartRenderer as IFeatureRenderer;
                MainMapControl.Refresh();
                TOCControl.Update();
                mChartEnumTpye = ChartEnumTpye.UnKnow;
            }
        }

        #endregion

        #region 5.4.9 双值符号化

        private void Bivariate_Click(object sender, EventArgs e)
        {
            if (mFormCurrency2 == null || mFormCurrency2.IsDisposed)
            {
                mFormCurrency2 = new FormCurrency2();
                mFormCurrency2.Render += new FormCurrency2.EventHandler(frmBivariateRenderer_BivariateRenderer);
            }
            mFormCurrency2.Map = MainMapControl.Map;
            mFormCurrency2.FormTitleText = "双值符号化";
            mFormCurrency2.Show();
        }

        void frmBivariateRenderer_BivariateRenderer(string sFeatClsName, string sFieldName, int numclasses)
        {
            IFeatureLayer pFeatLyr = MapOperation.GetFeatureLayerByName(MainMapControl.Map, sFeatClsName);
            BivariateRenderer(pFeatLyr, sFieldName, numclasses);
        }
        private void BivariateRenderer(IFeatureLayer pFeatLyr, string sFieldName, int numclasses)
        {
            IGeoFeatureLayer pGeoFeatLyr = pFeatLyr as IGeoFeatureLayer;
            ITable pTable = pFeatLyr as ITable;
            IUniqueValueRenderer pUniqueValueRender = new UniqueValueRendererClass();
            int intFieldNumber = pTable.FindField(sFieldName);
            pUniqueValueRender.FieldCount = 1;          //设置唯一值符号化的关键字段为一个
            pUniqueValueRender.set_Field(0, sFieldName);//设置唯一值符号化的第一个关键字段

            IRandomColorRamp pRandColorRamp = new RandomColorRampClass();
            pRandColorRamp.StartHue = 0;
            pRandColorRamp.MinValue = 0;
            pRandColorRamp.MinSaturation = 15;
            pRandColorRamp.EndHue = 360;
            pRandColorRamp.MaxValue = 100;
            pRandColorRamp.MaxSaturation = 30;
            //根据渲染字段的值的个数，设置一组随机颜色，如某一字段有5个值，则创建5个随机颜色与之匹配
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pRandColorRamp.Size = pFeatLyr.FeatureClass.FeatureCount(pQueryFilter);
            bool bSuccess = false;
            pRandColorRamp.CreateRamp(out bSuccess);
            IEnumColors pEnumRamp = pRandColorRamp.Colors;
            IColor pNextUniqueColor = null;
            pQueryFilter = new QueryFilterClass();
            pQueryFilter.AddField(sFieldName);
            ICursor pCursor = pTable.Search(pQueryFilter, true);
            IRow pNextRow = pCursor.NextRow();
            object codeValue = null;
            IRowBuffer pNextRowBuffer = null;


            while (pNextRow != null)
            {
                pNextRowBuffer = pNextRow as IRowBuffer;
                codeValue = pNextRowBuffer.get_Value(intFieldNumber);//获取渲染字段的每一个值

                pNextUniqueColor = pEnumRamp.Next();
                if (pNextUniqueColor == null)
                {
                    pEnumRamp.Reset();
                    pNextUniqueColor = pEnumRamp.Next();
                }
                IFillSymbol pFillSymbol = null;
                ILineSymbol pLineSymbol;
                IMarkerSymbol pMarkerSymbol;
                switch (pGeoFeatLyr.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        {
                            pFillSymbol = new SimpleFillSymbolClass();
                            pFillSymbol.Color = pNextUniqueColor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", pFillSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                    case esriGeometryType.esriGeometryPolyline:
                        {
                            pLineSymbol = new SimpleLineSymbolClass();
                            pLineSymbol.Color = pNextUniqueColor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", pLineSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                    case esriGeometryType.esriGeometryPoint:
                        {
                            pMarkerSymbol = new SimpleMarkerSymbolClass();
                            pMarkerSymbol.Color = pNextUniqueColor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", pMarkerSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                }

                // 分级渲染  
                ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                int IbreakIndex;
                object dataFrequency;
                object dataValues;
                //获得要着色的图层             
                ITable pTable2 = pGeoFeatLyr.FeatureClass as ITable;
                ITableHistogram pTableHistogram = new BasicTableHistogramClass();
                IBasicHistogram pBasicHistogram = (IBasicHistogram)pTableHistogram;
                pTableHistogram.Field = sFieldName;
                pTableHistogram.Table = pTable2;
                pBasicHistogram.GetHistogram(out dataValues, out dataFrequency);//获取渲染字段的值及其出现的频率
                //分级方法，用于根据获得的值计算得出符合要求的数据           
                IClassifyGEN pClassify = new EqualIntervalClass();
                try
                {
                    pClassify.Classify(dataValues, dataFrequency, ref numclasses);//根据获取字段的值和出现的频率对其进行等级划分 
                }
                catch (Exception ex)
                {
                }
                //返回一个数组
                double[] Classes = (double[])pClassify.ClassBreaks;
                int ClassesCount = Classes.GetUpperBound(0);

                ILegendInfo pLegendInfo = new ClassBreaksRendererClass();
                pLegendInfo.SymbolsAreGraduated = true;

                IClassBreaksRenderer pClassBreakRenderer = pLegendInfo as IClassBreaksRenderer;
                pClassBreakRenderer.Field = sFieldName;// 设置分级字段

                //设置着色对象的分级数目
                pClassBreakRenderer.BreakCount = ClassesCount;//设置分级数目   自己改的
                pClassBreakRenderer.SortClassesAscending = true;//升序排列
                //需要注意的是分级着色对象中的symbol和break的下标都是从0开始
                double symbolSizeOrigin = 5.0;
                if (ClassesCount <= 5)
                {
                    symbolSizeOrigin = 12;
                }
                if (ClassesCount < 10 && ClassesCount > 5)
                {
                    symbolSizeOrigin = 7;
                }
                IFillSymbol pBackgroundSymbol = new SimpleFillSymbolClass();
                for (IbreakIndex = 0; IbreakIndex <= ClassesCount - 1; IbreakIndex++)
                {
                    pClassBreakRenderer.set_Break(IbreakIndex, Classes[IbreakIndex + 1]);
                    pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                    pSimpleMarkerSymbol.Color = ColorTool.GetRgbColor(255, 100, 100);
                    //当渲染面图层时，需要设置BackgroundSymbol
                    switch (pFeatLyr.FeatureClass.ShapeType)
                    {
                        case esriGeometryType.esriGeometryPolygon:
                            {
                                pClassBreakRenderer.BackgroundSymbol = pBackgroundSymbol;
                                break;
                            }
                    }

                    pSimpleMarkerSymbol.Size = symbolSizeOrigin + IbreakIndex * symbolSizeOrigin / 3.0d;
                    pClassBreakRenderer.set_Symbol(IbreakIndex, (ISymbol)pSimpleMarkerSymbol);
                }

                IBivariateRenderer bivariateRenderer = new BiUniqueValueRendererClass();
                bivariateRenderer.MainRenderer = (IFeatureRenderer)pUniqueValueRender;//设置主渲染
                bivariateRenderer.VariationRenderer = (IFeatureRenderer)pClassBreakRenderer;//设置二元渲染
                bivariateRenderer.CreateLegend();
                pGeoFeatLyr.Renderer = (IFeatureRenderer)bivariateRenderer;
                MainMapControl.ActiveView.Refresh();
                TOCControl.Update();
            }
        }

        #endregion

        #region 5.4.10 多比例尺符号化

        private void ScaleDependent_Click(object sender, EventArgs e)
        {
            if (mFormCurrency2 == null || mFormCurrency2.IsDisposed)
            {
                mFormCurrency2 = new FormCurrency2();
                mFormCurrency2.Render += new FormCurrency2.EventHandler(frmScaleDependentRenderer_ScaleDependentRenderer);
            }
            mFormCurrency2.Map = MainMapControl.Map;
            mFormCurrency2.FormTitleText = "多比例尺符号化";
            mFormCurrency2.Show();
        }

        void frmScaleDependentRenderer_ScaleDependentRenderer(string sFeatClsName, string sFieldName, int numclasses)
        {
            IFeatureLayer pFeatLyr = MapOperation.GetFeatureLayerByName(MainMapControl.Map, sFeatClsName);
            ScaleDependentRenderer(pFeatLyr, sFieldName, numclasses);
        }
        private void ScaleDependentRenderer(IFeatureLayer pFeatLyr, string sFieldName, int numclasses)
        {
            IGeoFeatureLayer pGeoFeatLyr = pFeatLyr as IGeoFeatureLayer;
            ITable pTable = pFeatLyr as ITable;
            //唯一值渲染
            IUniqueValueRenderer pUniqueValueRender = new UniqueValueRendererClass();
            int intFieldNumber = pTable.FindField(sFieldName);
            pUniqueValueRender.FieldCount = 1;          //设置唯一值符号化的关键字段为一个
            pUniqueValueRender.set_Field(0, sFieldName);//设置唯一值符号化的第一个关键字段

            IRandomColorRamp pRandColorRamp = new RandomColorRampClass();
            pRandColorRamp.StartHue = 0;
            pRandColorRamp.MinValue = 0;
            pRandColorRamp.MinSaturation = 15;
            pRandColorRamp.EndHue = 360;
            pRandColorRamp.MaxValue = 100;
            pRandColorRamp.MaxSaturation = 30;
            //根据渲染字段的值的个数，设置一组随机颜色，如某一字段有5个值，则创建5个随机颜色与之匹配
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pRandColorRamp.Size = pFeatLyr.FeatureClass.FeatureCount(pQueryFilter);
            bool bSuccess = false;
            pRandColorRamp.CreateRamp(out bSuccess);

            IEnumColors pEnumRamp = pRandColorRamp.Colors;
            IColor pNextUniqueColor = null;

            pQueryFilter = new QueryFilterClass();
            pQueryFilter.AddField(sFieldName);
            ICursor pCursor = pTable.Search(pQueryFilter, true);
            IRow pNextRow = pCursor.NextRow();
            object codeValue = null;
            IRowBuffer pNextRowBuffer = null;


            while (pNextRow != null)
            {
                pNextRowBuffer = pNextRow as IRowBuffer;
                codeValue = pNextRowBuffer.get_Value(intFieldNumber);//获取渲染字段的每一个值

                pNextUniqueColor = pEnumRamp.Next();
                if (pNextUniqueColor == null)
                {
                    pEnumRamp.Reset();
                    pNextUniqueColor = pEnumRamp.Next();
                }
                IFillSymbol pFillSymbol = null;
                ILineSymbol pLineSymbol;
                IMarkerSymbol pMarkerSymbol;
                switch (pGeoFeatLyr.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        {
                            pFillSymbol = new SimpleFillSymbolClass();
                            pFillSymbol.Color = pNextUniqueColor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", pFillSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                    case esriGeometryType.esriGeometryPolyline:
                        {
                            pLineSymbol = new SimpleLineSymbolClass();
                            pLineSymbol.Color = pNextUniqueColor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", pLineSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                    case esriGeometryType.esriGeometryPoint:
                        {
                            pMarkerSymbol = new SimpleMarkerSymbolClass();
                            pMarkerSymbol.Color = pNextUniqueColor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", pMarkerSymbol as ISymbol);//添加渲染字段的值和渲染样式
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                }

                // 分级渲染
                ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                int IbreakIndex;
                object dataFrequency;
                object dataValues;
                //获得要着色的图层             
                ITable pTable2 = pGeoFeatLyr.FeatureClass as ITable;
                ITableHistogram pTableHistogram = new BasicTableHistogramClass();
                IBasicHistogram pBasicHistogram = (IBasicHistogram)pTableHistogram;
                pTableHistogram.Field = sFieldName;
                pTableHistogram.Table = pTable2;
                pBasicHistogram.GetHistogram(out dataValues, out dataFrequency);//获取渲染字段的值及其出现的频率
                //分级方法，用于根据获得的值计算得出符合要求的数据           
                IClassifyGEN pClassify = new EqualIntervalClass();
                try
                {
                    pClassify.Classify(dataValues, dataFrequency, ref numclasses);//根据获取字段的值和出现的频率对其进行等级划分 
                }
                catch (Exception ex)
                {
                }
                //返回一个数组
                double[] Classes = (double[])pClassify.ClassBreaks;
                int ClassesCount = Classes.GetUpperBound(0);
                ILegendInfo pLegendInfo = new ClassBreaksRendererClass();
                pLegendInfo.SymbolsAreGraduated = true;
                IClassBreaksRenderer pClassBreakRenderer = pLegendInfo as IClassBreaksRenderer;
                pClassBreakRenderer.Field = sFieldName;// 设置分级字段
                //设置着色对象的分级数目
                pClassBreakRenderer.BreakCount = ClassesCount;//设置分级数目   自己改的
                pClassBreakRenderer.SortClassesAscending = true;//升序排列
                //需要注意的是分级着色对象中的symbol和break的下标都是从0开始
                double symbolSizeOrigin = 5.0;
                if (ClassesCount <= 5)
                {
                    symbolSizeOrigin = 12;
                }
                if (ClassesCount < 10 && ClassesCount > 5)
                {
                    symbolSizeOrigin = 7;
                }
                IFillSymbol pBackgroundSymbol = new SimpleFillSymbolClass(); //另外添加（这个在分级符号中也得添加）
                pBackgroundSymbol.Color = ColorTool.GetRgbColor(255, 255, 100);
                for (IbreakIndex = 0; IbreakIndex <= ClassesCount - 1; IbreakIndex++)
                {
                    pClassBreakRenderer.set_Break(IbreakIndex, Classes[IbreakIndex + 1]);
                    pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                    pSimpleMarkerSymbol.Color = ColorTool.GetRgbColor(255, 100, 100);
                    pClassBreakRenderer.BackgroundSymbol = pBackgroundSymbol;// 另外添加（这个在分级符号中也得添加）
                    pSimpleMarkerSymbol.Size = symbolSizeOrigin + IbreakIndex * symbolSizeOrigin / 3.0d;
                    pClassBreakRenderer.set_Symbol(IbreakIndex, (ISymbol)pSimpleMarkerSymbol);
                }
                IScaleDependentRenderer pScaleDependentRenderer = new ScaleDependentRendererClass();
                pScaleDependentRenderer.AddRenderer(pUniqueValueRender as IFeatureRenderer);
                pScaleDependentRenderer.AddRenderer(pClassBreakRenderer as IFeatureRenderer);
                pScaleDependentRenderer.set_Break(0, 20000000);
                pScaleDependentRenderer.set_Break(1, 40000000);
                pGeoFeatLyr.Renderer = (IFeatureRenderer)pScaleDependentRenderer;
                MainMapControl.ActiveView.Refresh();
                TOCControl.Update();
            }
        }

        #endregion

        #endregion

        #region 5.5 地图整饰

        #region 5.5.1 ~ 5.5.3 地图要素

        public enum EnumMapSurroundType
        {
            None = 0,
            NorthArrow = 1,
            ScaleBar = 2,
            Legend = 3,
        }

        #region 5.5.1 添加图例

        private void AddLegend_Click(object sender, EventArgs e)
        {
            mEnumMapSurroundType = EnumMapSurroundType.Legend;        
        }

        private void MakeLegend(IActiveView activeView, IPageLayout pageLayout, IEnvelope envelope)
        {
            UID id = new UID();
            id.Value = "esriCarto.Legend";
            IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer;
            IMapFrame mapFrame = graphicsContainer.FindFrame(activeView.FocusMap) as IMapFrame;
            // 根据唯一标识符，穿件与之对应的MapSurriundFrame
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(id, null);
            // 获取pageLayer中的图例元素
            IElement deletElement = MainPageLayoutControl.FindElementByName("Legend");

            if (deletElement != null)
            {
                // 如果已经存在图例，删除已经存在的图例
                graphicsContainer.DeleteElement(deletElement);
            }

            // 设置MapSurroundFrame背景
            ISymbolBackground symbolBackground = new SymbolBackgroundClass();
            IFillSymbol fillSymbol = new SimpleFillSymbolClass();
            ILineSymbol lineSymbol = new SimpleLineSymbolClass();
            fillSymbol.Color = ColorTool.GetRgbColor(240,240,240);
            lineSymbol.Color = ColorTool.GetRgbColor(0, 0, 0);
            fillSymbol.Outline = lineSymbol;
            symbolBackground.FillSymbol = fillSymbol;
            mapSurroundFrame.Background = symbolBackground;
            // 添加图例
            IElement element = mapSurroundFrame as IElement;
            element.Geometry = envelope as IGeometry;
            IMapSurround mapSurround = mapSurroundFrame.MapSurround;
            ILegend legend = mapSurround as ILegend;
            legend.ClearItems();
            legend.Title = "图例";
            for (int i = 0; i < activeView.FocusMap.LayerCount; i++)
            {
                ILegendItem legendItem = new HorizontalLegendItemClass();
                // 获取添加图例关联图层
                legendItem.Layer = activeView.FocusMap.get_Layer(i);
                legendItem.ShowDescriptions = false;
                legendItem.Columns = 1;
                legendItem.ShowHeading = true;
                legendItem.ShowLabels = true;
                // 添加图例内容
                legend.AddItem(legendItem);
            }
            graphicsContainer.AddElement(element, 0);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }


        #endregion

        #region 5.5.2 添加指北针

        private void AddNorthArrows_Click(object sender, EventArgs e)
        {
            mEnumMapSurroundType = EnumMapSurroundType.NorthArrow;
            if (mFormSymbology == null || mFormSymbology.IsDisposed)
            {
                mFormSymbology = new FormSymbology();
                mFormSymbology.Render += new FormSymbology.EventHandler(FormSymbology_GetSelSymbolItem);
            }
            mFormSymbology.EnumMapSurType = mEnumMapSurroundType;
            mFormSymbology.InitUI();
            mFormSymbology.ShowDialog();
        }

        /// <summary>
        /// 插入指北针
        /// </summary>
        /// <param name="pageLayout"></param>
        /// <param name="envelope"></param>
        /// <param name="activeView"></param>
        private void addNorthArrow(IPageLayout pageLayout, IEnvelope envelope, IActiveView activeView)
        {
            IMap map = activeView.FocusMap;
            IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer;
            IMapFrame mapFrame = graphicsContainer.FindFrame(map) as IMapFrame;
            if (mStyleGalleryItem == null)
            {
                return;
            }
            IMapSurroundFrame mapSurroundFrame = new MapSurroundFrameClass();
            mapSurroundFrame.MapFrame = mapFrame;
            INorthArrow northArrow = new MarkerNorthArrowClass();
            northArrow = mStyleGalleryItem.Item as INorthArrow;
            northArrow.Size = envelope.Width * 50;
            // 根据用户的选取，获取相应的MapSurround
            mapSurroundFrame.MapSurround = northArrow as IMapSurround;
            // 获取PageLayer中的指北针元素
            IElement element = MainPageLayoutControl.FindElementByName("NorthArrows");
            if (element != null)
            {
                // 如果存在值北针，删除已经存在的指北针
                graphicsContainer.DeleteElement(element);
            }
            IElementProperties elementProperties = null;
            element = mapSurroundFrame as IElement;
            element.Geometry = envelope as IGeometry;
            elementProperties = element as IElementProperties;
            elementProperties.Name = "NorthArrows";
            graphicsContainer.AddElement(element, 0);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics,null,null);
        }

        #endregion

        #region 5.5.3 添加比例尺

        private void AddScaleBar_Click(object sender, EventArgs e)
        {
            mEnumMapSurroundType = EnumMapSurroundType.ScaleBar;
            if (mFormSymbology == null || mFormSymbology.IsDisposed)
            {
                mFormSymbology = new FormSymbology();
                mFormSymbology.Render += new FormSymbology.EventHandler(FormSymbology_GetSelSymbolItem);
            }
            mFormSymbology.EnumMapSurType = mEnumMapSurroundType;
            mFormSymbology.InitUI();
            mFormSymbology.ShowDialog();
        }

        private void makeScaleBar(IActiveView activeView, IPageLayout pageLayout, IEnvelope envelope)
        {
            IMap map = activeView.FocusMap;
            IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer;
            IMapFrame mapFrame = graphicsContainer.FindFrame(map) as IMapFrame;
            if (mStyleGalleryItem == null)
            {
                return;
            }
            IMapSurroundFrame mapSurroundFrame = new MapSurroundFrameClass();
            mapSurroundFrame.MapFrame = mapFrame;
            mapSurroundFrame.MapSurround = mStyleGalleryItem.Item as IMapSurround;
            IElement element = MainPageLayoutControl.FindElementByName("ScaleBar");
            if (element != null)
            {
                // 删除已经存在的比例尺
                graphicsContainer.DeleteElement(element);
            }
            IElementProperties elementProperties = null;
            element = mapSurroundFrame as IElement;
            element.Geometry = envelope as IGeometry;
            elementProperties = element as IElementProperties;
            elementProperties.Name = "ScaleBar";
            graphicsContainer.AddElement(element, 0);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        #endregion

        private IPoint mPoint = null;

        private IPoint mMovePoint = null;

        private INewEnvelopeFeedback mNewEnvelopeFeedback = null;

        private void MainPageLayoutControl_OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {

            if (mEnumMapSurroundType != EnumMapSurroundType.None)
            {
                IActiveView activeView = null;
                activeView = MainPageLayoutControl.PageLayout as IActiveView;
                mPoint = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                if (mNewEnvelopeFeedback == null)
                {
                    mNewEnvelopeFeedback = new NewEnvelopeFeedbackClass();
                    mNewEnvelopeFeedback.Display = activeView.ScreenDisplay;
                    mNewEnvelopeFeedback.Start(mPoint);
                }
                else
                {
                    mNewEnvelopeFeedback.MoveTo(mPoint);
                }
            }

        }

        private void MainPageLayoutControl_OnMouseMove(object sender, IPageLayoutControlEvents_OnMouseMoveEvent e)
        {
            if (mEnumMapSurroundType != EnumMapSurroundType.None)
            {
                if (mNewEnvelopeFeedback != null)
                {
                    mMovePoint = (MainPageLayoutControl.PageLayout as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                    mNewEnvelopeFeedback.MoveTo(mMovePoint);
                }
            }
        }

        private void MainPageLayoutControl_OnMouseUp(object sender, IPageLayoutControlEvents_OnMouseUpEvent e)
        {

            if (mEnumMapSurroundType != EnumMapSurroundType.None)
            {
                IActiveView activeView = null;
                activeView = MainPageLayoutControl.PageLayout as IActiveView;
                IEnvelope envelope = mNewEnvelopeFeedback.Stop();
                AddMapSuround(activeView, mEnumMapSurroundType, envelope);
                mNewEnvelopeFeedback = null;
                mEnumMapSurroundType = EnumMapSurroundType.None;
            }
          
        }

        private void AddMapSuround(IActiveView activeView, EnumMapSurroundType enumType, IEnvelope envelope) {
            switch (enumType)
            {

                case EnumMapSurroundType.NorthArrow:
                    addNorthArrow(MainPageLayoutControl.PageLayout, envelope, activeView);
                    break;
                case EnumMapSurroundType.ScaleBar:
                    makeScaleBar(activeView, MainPageLayoutControl.PageLayout, envelope);
                    break;
                case EnumMapSurroundType.Legend:
                    MakeLegend(activeView, MainPageLayoutControl.PageLayout, envelope);
                    break;
            }
        }

        private IStyleGalleryItem mStyleGalleryItem;

        private void FormSymbology_GetSelSymbolItem(ref IStyleGalleryItem pStyleItem)
        {
            mStyleGalleryItem = pStyleItem;
        }

        #endregion

        #region 5.5.4 添加地图网格

        #region Graticule网格

        private void AddGraticule_Click(object sender, EventArgs e)
        {
            try
            {
                IActiveView activeView = MainPageLayoutControl.ActiveView;
                IPageLayout pageLayout = MainPageLayoutControl.PageLayout;
                DeleteMapGrid(activeView, pageLayout);
                CreateGraticuleMapGrid(activeView, pageLayout);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateGraticuleMapGrid(IActiveView activeView, IPageLayout pageLayout)
        {
            IMap map = activeView.FocusMap;
            // 看这个改动是否争取
            IGraticule graticule = new GraticuleClass();
            graticule.Name = "Map Grid";

            // 设置格网线的符号样式
            ICartographicLineSymbol cartographicLineSymbol = new CartographicLineSymbolClass();
            cartographicLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            cartographicLineSymbol.Width = 1;
            cartographicLineSymbol.Color = ColorTool.GetRgbColor(166,187,208);
            graticule.LineSymbol = cartographicLineSymbol;

            // 设置格网的边框样式
            ISimpleMapGridBorder simpleMapGridBorder = new SimpleMapGridBorderClass();
            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            simpleLineSymbol.Color = ColorTool.GetRgbColor(100,255,0);
            simpleLineSymbol.Width = 2;
            simpleMapGridBorder.LineSymbol = simpleLineSymbol as ILineSymbol;
            graticule.Border = simpleMapGridBorder as IMapGridBorder;
            graticule.SetTickVisibility(true,true,true,true);

            // 设置格网的主刻度的样式和可见性
            graticule.TickLength = 15;
            cartographicLineSymbol = new CartographicLineSymbolClass();
            cartographicLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            cartographicLineSymbol.Width = 1;
            cartographicLineSymbol.Color = ColorTool.GetRgbColor(166,187,208);
            graticule.TickMarkSymbol = null;
            graticule.TickLineSymbol = simpleLineSymbol;
            graticule.SetTickVisibility(true, true, true, true);

            // 设置格网的次级刻度的样式和可见性
            graticule.SubTickCount = 5;
            graticule.SubTickLength = 10;
            cartographicLineSymbol = new CartographicLineSymbolClass();
            cartographicLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            cartographicLineSymbol.Width = 0.1;
            cartographicLineSymbol.Color = ColorTool.GetRgbColor(166, 187, 208);
            graticule.SubTickLineSymbol = cartographicLineSymbol;
            graticule.SetSubTickVisibility(true, true, true, true);

            // 设置格网标签的样式和可见性
            IGridLabel gridlabel = graticule.LabelFormat;
            gridlabel.LabelOffset = 15;
            StdFont font = new StdFont();
            font.Name = "Arial";
            font.Size = 16;
            graticule.LabelFormat.Font = font as IFontDisp;
            graticule.Visible = true;

            // 创建IMeasuredGrid对象
            IMeasuredGrid measuredGrid = new MeasuredGridClass();
            IProjectedGrid projectedGrid = measuredGrid as IProjectedGrid;
            projectedGrid.SpatialReference = map.SpatialReference;
            measuredGrid = graticule as IMeasuredGrid;

            // 获取坐标范围，设置格网的起始点和间隔
            double maxX, maxY, minX, minY;
            projectedGrid.SpatialReference.GetDomain(out minX, out maxX, out minY, out maxY);
            measuredGrid.FixedOrigin = true;
            measuredGrid.Units = map.MapUnits;

            // 维度范围
            measuredGrid.XIntervalSize = (maxX - minX) / 200;
            measuredGrid.XOrigin = minX;
            // 经度范围
            measuredGrid.YIntervalSize = (maxY - minY) / 200;
            measuredGrid.YOrigin = minY;

            // 将格网对象添加到地图控件中
            IGraphicsContainer graphicsContainer = activeView as IGraphicsContainer;
            IMapFrame mapFrame = graphicsContainer.FindFrame(map) as IMapFrame;
            IMapGrids mapGrid = mapFrame as IMapGrids;
            mapGrid.AddMapGrid(graticule);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
        }


        #endregion

        #region MeasuredGrid格网

        private void AddMeasuredGrid_Click(object sender, EventArgs e)
        {
            try
            {
                IActiveView activeView = MainPageLayoutControl.ActiveView;
                IPageLayout pageLayout = MainPageLayoutControl.PageLayout;
                DeleteMapGrid(activeView, pageLayout);
                CreateMeasuredGrid(activeView, pageLayout);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateMeasuredGrid(IActiveView activeView, IPageLayout pageLayout)
        {
            IMap map = activeView.FocusMap;
            IMeasuredGrid measuredGrid = new MeasuredGridClass();
            // 设置格网的基本属性
            measuredGrid.FixedOrigin = false;
            measuredGrid.Units = map.MapUnits;
            // 设置维度的间隔
            measuredGrid.XIntervalSize = 5;
            // 设置经度的间隔
            measuredGrid.YIntervalSize = 5;

            // 设置GridLabel格式
            IGridLabel gridLabel = new FormattedGridLabelClass();
            IFormattedGridLabel formattedGridLabel = new FormattedGridLabelClass();
            INumericFormat numericFormat = new NumericFormatClass();
            numericFormat.AlignmentOption = esriNumericAlignmentEnum.esriAlignLeft;
            numericFormat.RoundingOption = esriRoundingOptionEnum.esriRoundNumberOfDecimals;
            numericFormat.RoundingValue = 0;
            numericFormat.ZeroPad = true;
            formattedGridLabel.Format = numericFormat as INumberFormat;
            gridLabel = formattedGridLabel as IGridLabel;
            StdFont font = new StdFontClass();
            font.Name = "宋体";
            font.Size = 25;
            gridLabel.Font = font as IFontDisp;
            IMapGrid mapGrid = new MeasuredGridClass();
            mapGrid = measuredGrid as IMapGrid;
            mapGrid.LabelFormat = gridLabel;

            // 将格网添加到地图上
            IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer;
            IFrameElement frameElement = graphicsContainer.FindFrame(map);
            IMapFrame mapFrame = frameElement as IMapFrame;
            IMapGrids mapGrids = null;
            mapGrids = mapFrame as IMapGrids;
            mapGrids.AddMapGrid(mapGrid);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null); 
        }

        #endregion

        #region IndexGrid格网

        private void AddIndexGrid_Click(object sender, EventArgs e)
        {
            try
            {
                IActiveView activeView = MainPageLayoutControl.ActiveView;
                IPageLayout pageLayout = MainPageLayoutControl.PageLayout;
                DeleteMapGrid(activeView, pageLayout);
                CreateIndexGrid(activeView, pageLayout);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateIndexGrid(IActiveView activeView, IPageLayout pageLayout)
        {
            IndexGrid indexGrid = new IndexGridClass();

            // 设置Index属性
            indexGrid.ColumnCount = 5;
            indexGrid.RowCount = 5;
            string[] indexnum = { "A", "B", "C", "D", "F" };
            // 设置IndexLabel
            int i = 0;
            for (i = 0; i < indexGrid.ColumnCount; i++)
            {
                indexGrid.set_XLabel(i, indexnum[i]);
                indexGrid.set_YLabel(i, i.ToString());
            }

            // 设置GridLabel格式
            IGridLabel gridLabel = new RoundedTabStyleClass();
            StdFont font = new StdFontClass();
            font.Name = "宋体";
            font.Size = 18;
            gridLabel.Font = font as IFontDisp;
            IMapGrid mapGrid = new IndexGridClass();
            mapGrid = indexGrid as IMapGrid;
            mapGrid.LabelFormat = gridLabel;

            // 添加IndexGrid
            IMapGrid mapGird = indexGrid;
            IMap map = activeView.FocusMap;
            IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer;
            IFrameElement frameElement = graphicsContainer.FindFrame(map);
            IMapFrame mapFrame = frameElement as IMapFrame;
            IMapGrids mapGrids = mapFrame as IMapGrids;
            mapGrids.AddMapGrid(mapGrid);
            MainPageLayoutControl.Refresh();
        }

        #endregion


        private void DeleteGird_Click(object sender, EventArgs e)
        {
            try
            {
                IActiveView activeView = MainPageLayoutControl.ActiveView;
                IPageLayout pageLayout = MainPageLayoutControl.PageLayout;
                DeleteMapGrid(activeView, pageLayout);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 删除已经存在的格网
        /// </summary>
        /// <param name="activeView"></param>
        /// <param name="pageLayout"></param>
        private void DeleteMapGrid(IActiveView activeView, IPageLayout pageLayout)
        {
            IMap map = activeView.FocusMap;
            IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer;
            IFrameElement frameElement = graphicsContainer.FindFrame(map);
            IMapFrame mapFrame = frameElement as IMapFrame;
            IMapGrids mapGrids = null;
            mapGrids = mapFrame as IMapGrids;
            if (mapGrids.MapGridCount > 0)
            {
                IMapGrid mapGrid = mapGrids.get_MapGrid(0);
                mapGrids.DeleteMapGrid(mapGrid);
            }
            activeView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
        }

        #endregion

        #endregion

        #region 5.6 制图模板

        private void SelectTemplate_Click(object sender, EventArgs e)
        {
            FormTemplate formTemplate = new FormTemplate(MainPageLayoutControl);
            formTemplate.Show();
        }

        #endregion

        #region 5.8 打印输出

        private void Print_Click(object sender, EventArgs e)
        {
            FormPrintPreview formPrintPreview = new FormPrintPreview(MainPageLayoutControl);
            formPrintPreview.Show();
        }

        #endregion

    }

}
