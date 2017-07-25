using System;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

namespace ArcGIS_Poisonous
{
    public partial class ArcGisPoisonous : Form
    {
        #region 变量定义

        //地图导出窗体
        private FormExportMap FrmExpMap = null;

        // 地图鼠标操作标志
        string pMouseOperate = null;

        // 存储历史视图
        IExtentStack pExtentStack;

        // 测量变量
        // 追踪线对象
        private INewLineFeedback pNewLineFeedback;
        // 追踪面对象
        private INewPolygonFeedback pNewPolygonFeedback;
        //面积量算时画的点进行存储
        private IPointCollection pAreaPointCol = new MultipointClass();
        // 结果显示窗体
        private FormMeasureResult frmMeasureResult = null;
        // 鼠标点击点
        private IPoint pPointPt = null;
        // 鼠标移动是的当前点
        private IPoint pMovePt = null;
        // 量测总长度
        private double dToltalLength = 0;
        // 片段距离
        private double dSegmentLength = 0;
        // 地图单位变量
        private string sMapUnits = "未知单位";
        private object missing = Type.Missing;

        // 鹰眼同步
        // 鹰眼地图上的矩形框移动标志
        private bool CanDrag;
        // 记录在移动鹰眼地图上的矩形鼠标位置
        private IPoint MoveRectPoint;
        // 记录数据视图的Extent
        private IEnvelope pEnv;

        // TOCControl变量
        // 点击要素图层
        private IFeatureLayer pTocFeatureLayer = null;
        // 图层属性窗体
        private FormAtrribute frmAttribute = null;
        // 需要调整显示顺序的图层
        private ILayer pMoveLayer;
        // 存放拖动图层移动到索引号
        private int toIndex;

        //定义ISelectionEnvironment接口的对象来设置选择环境
        private ISelectionEnvironment selectionEnvironment;

        #endregion

        #region 声明分部方法

        partial void TxtSymbol(IMapControlEvents2_OnMouseDownEvent e);

        #endregion

        public ArcGisPoisonous()
        {
            InitializeComponent();
            TOCControl.SetBuddyControl(MainMapControl);

            EagleEyeMapControl.Extent = MainMapControl.FullExtent;
            pEnv = EagleEyeMapControl.Extent;
            DrawRectangle(pEnv);

            //窗体初始化时新建ISelectionEnvironment接口的对象，对象具有默认的选项设置值
            selectionEnvironment = new SelectionEnvironmentClass();
        }

        #region Method From Book

        #region 其他方法

        /// <summary>
        /// 将MainMapControl数据复制到PageLayout
        /// </summary>
        private void CopyToPageLayout()
        {
            IObjectCopy pObjectCopy = new ObjectCopyClass();
            object copyFromMap = MainMapControl.Map;
            object copiedMap = pObjectCopy.Copy(copyFromMap);//复制地图到copiedMap中
            object copyToMap = MainPageLayoutControl.ActiveView.FocusMap;
            pObjectCopy.Overwrite(copiedMap, ref copyToMap); //复制地图
            MainPageLayoutControl.ActiveView.Refresh();
        }


        /// <summary>
        /// 加载工作空间的方法进行封装，以便在空间数据库中调用（Personal Geodatabase、文件地理数据库、ArcSDE空间数据库）
        /// </summary>
        /// <param name="pWorkspace"></param>
        /// <param name="pMapControl"></param>
        private void AddAllDataset(IWorkspace pWorkspace, AxMapControl pMapControl)
        {
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
            pEnumDataset.Reset();

            // 将Enum数据集中的数据一个一个地读到DataSet中
            IDataset pDataset = pEnumDataset.Next();
            // 判断数据集是否有数据
            while (pDataset != null)
            {
                // 要素数据集
                if (pDataset is IFeatureDataset)
                {
                    IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                    IFeatureDataset pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(pDataset.Name);
                    IEnumDataset pEnumDataset1 = pFeatureDataset.Subsets;
                    pEnumDataset1.Reset();
                    IGroupLayer pGroupLayer = new GroupLayerClass();
                    pGroupLayer.Name = pFeatureDataset.Name;
                    IDataset pDataset1 = pEnumDataset1.Next();
                    while (pDataset1 != null)
                    {
                        // 要素类
                        if (pDataset1 is IFeatureClass)
                        {
                            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                            pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset1.Name);
                            if (pFeatureLayer.FeatureClass != null)
                            {
                                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                                pGroupLayer.Add(pFeatureLayer);
                                pMapControl.Map.AddLayer(pFeatureLayer);
                            }
                        }
                        pDataset1 = pEnumDataset1.Next();
                    }
                }
                else if (pDataset is IFeatureClass) //要素类
                {
                    IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset.Name);

                    pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                    pMapControl.Map.AddLayer(pFeatureLayer);
                }
                else if (pDataset is IRasterDataset) //栅格数据集
                {
                    IRasterWorkspaceEx pRasterWorkspace = (IRasterWorkspaceEx)pWorkspace;
                    IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(pDataset.Name);
                    //影像金字塔判断与创建
                    IRasterPyramid3 pRasPyrmid;
                    pRasPyrmid = pRasterDataset as IRasterPyramid3;
                    if (pRasPyrmid != null)
                    {
                        if (!(pRasPyrmid.Present))
                        {
                            pRasPyrmid.Create(); //创建金字塔
                        }
                    }
                    IRasterLayer pRasterLayer = new RasterLayerClass();
                    pRasterLayer.CreateFromDataset(pRasterDataset);
                    ILayer pLayer = pRasterLayer as ILayer;
                    pMapControl.AddLayer(pLayer, 0);
                }
                pDataset = pEnumDataset.Next();

            }

            pMapControl.ActiveView.Refresh();
            // 同步鹰眼
            SynchronizeEagleEye();
        }

        /// <summary>
        /// 鹰眼同步
        /// </summary>
        private void SynchronizeEagleEye()
        {
            if (EagleEyeMapControl.LayerCount > 0)
            {
                EagleEyeMapControl.ClearLayers();
            }
            // 设置鹰眼视图与主地图坐标系统一致
            EagleEyeMapControl.SpatialReference = MainMapControl.SpatialReference;
            for (int i = MainMapControl.LayerCount - 1; i >= 0; i--)
            {
                // 使鹰眼和主地图的坐标保持一致
                ILayer pLayer = MainMapControl.get_Layer(i);
                if (pLayer is IGroupLayer || pLayer is ICompositeLayer)
                {
                    ICompositeLayer comositeLayer = pLayer as ICompositeLayer;
                    for (int j = comositeLayer.Count - 1; j >= 0; j--)
                    {
                        ILayer pSubLayer = comositeLayer.get_Layer(j);
                        IFeatureLayer featureLayer = pSubLayer as IFeatureLayer;
                        if (featureLayer != null)
                        {
                            if (featureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint && featureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                            {
                                EagleEyeMapControl.AddLayer(pLayer);
                            }
                        }
                    }
                }
                else
                {
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    if (pFeatureLayer != null)
                    {
                        if (pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint && pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                        {
                            EagleEyeMapControl.AddLayer(pLayer);
                        }
                    }
                }
            }
            // 设置鹰眼地图全图显示
            EagleEyeMapControl.Extent = MainMapControl.FullExtent;
            pEnv = MainMapControl.Extent as IEnvelope;
            DrawRectangle(pEnv);
            EagleEyeMapControl.ActiveView.Refresh();
        }
       
        /// <summary>
        /// 在鹰眼地图上矩形框绘制
        /// </summary>
        /// <param name="pEnv"></param>
        private void DrawRectangle(IEnvelope pEnv)
        {
            // 在绘制前，清除鹰眼地图上之前绘制的矩形框
            IGraphicsContainer graphicsContainer = EagleEyeMapControl.Map as IGraphicsContainer;
            IActiveView activeView = graphicsContainer as IActiveView;
            graphicsContainer.DeleteAllElements();

            // 得到当前视图的范围
            IRectangleElement rectangleElement = new RectangleElementClass();
            IElement element = rectangleElement as IElement;
            element.Geometry = pEnv;

            // 设置矩形框，中间为透明面
            IRgbColor color = new RgbColorClass();
            color = GetRgbColor(255, 0, 0);
            color.Transparency = 255;
            ILineSymbol outLine = new SimpleLineSymbolClass();
            outLine.Width = 2;
            outLine.Color = color;
            IFillSymbol fillSymbol = new SimpleFillSymbolClass();
            //color = new RgbColorClass();
            color.Transparency = 0;
            fillSymbol.Color = color;
            fillSymbol.Outline = outLine;

            // 像鹰眼中添加矩形框
            IFillShapeElement fillShapElement = element as IFillShapeElement;
            fillShapElement.Symbol = fillSymbol;
            graphicsContainer.AddElement(fillShapElement as IElement, 0);

            // 刷新
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 清除地图上的所有数据
        /// </summary>
        private void ClearAllData()
        {

            if (MainMapControl.Map != null && MainMapControl.Map.LayerCount > 0)
            {
                //新建mainMapControl中Map
                IMap dataMap = new MapClass();
                dataMap.Name = "Map";
                MainMapControl.DocumentFilename = string.Empty;
                MainMapControl.Map = dataMap;

                //新建EagleEyeMapControl中Map
                IMap eagleEyeMap = new MapClass();
                eagleEyeMap.Name = "eagleEyeMap";
                //EagleEyeMapControl.DocumentFilename = string.Empty;
                //EagleEyeMapControl.Map = eagleEyeMap;
            }
        }

        /// <summary>
        /// 获取地图单位
        /// </summary>
        /// <param name="_esriMapUnit"></param>
        /// <returns></returns>
        private string GetMapUnit(esriUnits _esriMapUnit)
        {
            string sMapUnits = string.Empty;
            switch (_esriMapUnit)
            {
                case esriUnits.esriCentimeters:
                    sMapUnits = "厘米";
                    break;
                case esriUnits.esriDecimalDegrees:
                    sMapUnits = "十进制";
                    break;
                case esriUnits.esriDecimeters:
                    sMapUnits = "分米";
                    break;
                case esriUnits.esriFeet:
                    sMapUnits = "尺";
                    break;
                case esriUnits.esriInches:
                    sMapUnits = "英寸";
                    break;
                case esriUnits.esriKilometers:
                    sMapUnits = "千米";
                    break;
                case esriUnits.esriMeters:
                    sMapUnits = "米";
                    break;
                case esriUnits.esriMiles:
                    sMapUnits = "英里";
                    break;
                case esriUnits.esriMillimeters:
                    sMapUnits = "毫米";
                    break;
                case esriUnits.esriNauticalMiles:
                    sMapUnits = "海里";
                    break;
                case esriUnits.esriPoints:
                    sMapUnits = "点";
                    break;
                case esriUnits.esriUnitsLast:
                    sMapUnits = "UnitsLast";
                    break;
                case esriUnits.esriUnknownUnits:
                    sMapUnits = "未知单位";
                    break;
                case esriUnits.esriYards:
                    sMapUnits = "码";
                    break;
                default:
                    break;
            }
            return sMapUnits;
        }

        private IPolygon DrawPolygon(AxMapControl axMaxControl)
        {
            IPolygon Geometry = null;
            if (axMaxControl == null)
            {
                return null;
            }
            IRubberBand rb = new RubberPolygonClass();
            Geometry = rb.TrackNew(axMaxControl.ActiveView.ScreenDisplay, null) as IPolygon;
            return Geometry;
        }

        /// <summary>
        /// 获取RGB颜色
        /// </summary>
        /// <param name="intR">红</param>
        /// <param name="intG">绿</param>
        /// <param name="intB">蓝</param>
        /// <returns></returns>
        private IRgbColor GetRgbColor(int intR, int intG, int intB)
        {
            IRgbColor pRgbColor = null;
            if (intR < 0 || intR > 255 || intG < 0 || intG > 255 || intB < 0 || intB > 255)
            {
                return pRgbColor;
            }
            pRgbColor = new RgbColorClass();
            pRgbColor.Red = intR;
            pRgbColor.Green = intG;
            pRgbColor.Blue = intB;
            return pRgbColor;
        }

        #endregion

        #region MainMapControl事件

        /// <summary>
        /// 地图中鼠标按下事件监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //屏幕坐标点转化为地图坐标点
            pPointPt = (MainMapControl.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            if (e.button == 1)
            {
                IActiveView pActiveView = MainMapControl.ActiveView;
                IEnvelope pEnvelope = new EnvelopeClass();

                switch (pMouseOperate)
                {
                    case "ZoomIn":
                        pEnvelope = MainMapControl.TrackRectangle();
                        //  如果拉框范围为空则返回
                        if (pEnvelope == null || pEnvelope.IsEmpty || pEnvelope.Height == 0 || pEnvelope.Width == 0)
                        {
                            return;
                        }
                        // 如果拉框有范围，则拉大到拉框范围
                        MainMapControl.ActiveView.Extent = pEnvelope;
                        MainMapControl.ActiveView.Refresh();
                        break;

                    case "ZoomOut":
                        pEnvelope = MainMapControl.TrackRectangle();
                        //  如果拉框范围为空则返回
                        if (pEnvelope == null || pEnvelope.IsEmpty || pEnvelope.Height == 0 || pEnvelope.Width == 0)
                        {
                            return;
                        }
                        else
                        {
                            double dWidth = pActiveView.Extent.Width * pActiveView.Extent.Width / pEnvelope.Width;
                            double dHeight = pActiveView.Extent.Height * pActiveView.Extent.Height / pEnvelope.Height;
                            double dXmin = pActiveView.Extent.XMin -
                                       ((pEnvelope.XMin - pActiveView.Extent.XMin) * pActiveView.Extent.Width /
                                        pEnvelope.Width);
                            double dYmin = pActiveView.Extent.YMin -
                                           ((pEnvelope.YMin - pActiveView.Extent.YMin) * pActiveView.Extent.Height /
                                            pEnvelope.Height);
                            double dXmax = dXmin + dWidth;
                            double dYmax = dYmin + dHeight;
                            pEnvelope.PutCoords(dXmin, dYmin, dXmax, dYmax);
                        }
                        pActiveView.Extent = pEnvelope;
                        pActiveView.Refresh();
                        break;

                    case "Pan":
                        MainMapControl.Pan();
                        break;

                    case "MeasureLength":
                        //判断追踪线对象是否为空，若是则实例化并设置当前鼠标点为起始点
                        if (pNewLineFeedback == null)
                        {
                            //实例化追踪线对象
                            pNewLineFeedback = new NewLineFeedbackClass();
                            pNewLineFeedback.Display = (MainMapControl.Map as IActiveView).ScreenDisplay;
                            //设置起点，开始动态线绘制
                            pNewLineFeedback.Start(pPointPt);
                            dToltalLength = 0;
                        }
                        else //如果追踪线对象不为空，则添加当前鼠标点
                        {
                            pNewLineFeedback.AddPoint(pPointPt);
                        }
                        //pGeometry = m_PointPt;
                        if (dSegmentLength != 0)
                        {
                            dToltalLength = dToltalLength + dSegmentLength;
                        }
                        break;

                    case "MeasureArea":
                        if (pNewPolygonFeedback == null)
                        {
                            //实例化追踪面对象
                            pNewPolygonFeedback = new NewPolygonFeedback();
                            pNewPolygonFeedback.Display = (MainMapControl.Map as IActiveView).ScreenDisplay;
                            ;
                            pAreaPointCol.RemovePoints(0, pAreaPointCol.PointCount);
                            //开始绘制多边形
                            pNewPolygonFeedback.Start(pPointPt);
                            pAreaPointCol.AddPoint(pPointPt, ref missing, ref missing);
                        }
                        else
                        {
                            pNewPolygonFeedback.AddPoint(pPointPt);
                            pAreaPointCol.AddPoint(pPointPt, ref missing, ref missing);
                        }
                        break;

                    case "SelFeature":
                        IEnvelope pEnv = MainMapControl.TrackRectangle();
                        IGeometry pGeo = pEnv as IGeometry;
                        // 矩形框若为空，即为点选时，对点的范围进行扩展
                        if (pEnv.IsEmpty == true)
                        {
                            tagRECT r;
                            r.left = e.x - 5;
                            r.top = e.y - 5;
                            r.right = e.x + 5;
                            r.bottom = e.y + 5;
                            pActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnv, ref r, 4);
                            pEnv.SpatialReference = pActiveView.FocusMap.SpatialReference;
                        }
                        pGeo = pEnv as IGeometry;
                        MainMapControl.Map.SelectByShape(pGeo, null, false);
                        MainMapControl.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                        //IEnvelope pEnv = MainMapControl.TrackRectangle();
                        //IGeometry pGeo = pEnv as IGeometry;
                        ////矩形框若为空，即为点选时，对点范围进行扩展
                        //if (pEnv.IsEmpty == true)
                        //{
                        //    tagRECT r;
                        //    r.left = e.x - 5;
                        //    r.top = e.y - 5;
                        //    r.right = e.x + 5;
                        //    r.bottom = e.y + 5;
                        //    pActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnv, ref r, 4);
                        //    pEnv.SpatialReference = pActiveView.FocusMap.SpatialReference;
                        //}
                        //pGeo = pEnv as IGeometry;
                        //MainMapControl.Map.SelectByShape(pGeo, null, false);
                        //MainMapControl.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                        break;

                    case "ExportRegion":
                        // 删除视图中的数据
                        MainMapControl.ActiveView.GraphicsContainer.DeleteAllElements();
                        MainMapControl.ActiveView.Refresh();
                        IPolygon polygon = DrawPolygon(MainMapControl);
                        if (polygon == null)
                        {
                            return;
                        }
                        ExportMap.AddElement(polygon, MainMapControl.ActiveView);
                        if (FrmExpMap == null || FrmExpMap.IsDisposed)
                        {
                            FrmExpMap = new FormExportMap(MainMapControl);
                        }
                        FrmExpMap.IsRegion = true;
                        FrmExpMap.GetGometry = polygon as IGeometry;
                        FrmExpMap.Show();
                        FrmExpMap.Activate();
                        break;
                    case "TxtSymbol":
                        TxtSymbol(e);
                        break;

                    default:
                        break;

                }
            }
            else if (e.button == 2)
            {
                pMouseOperate = "";
                MainMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
        }

        /// <summary>
        /// MainMapControl双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMapControl_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            #region 长度量算

            switch (pMouseOperate)
            {
                case "MeasureLength":
                    if (frmMeasureResult != null)
                    {
                        frmMeasureResult.lblMeasureResult.Text = "线段总长度为：" + dToltalLength + sMapUnits;
                    }
                    if (pNewLineFeedback != null)
                    {
                        pNewLineFeedback.Stop();
                        pNewLineFeedback = null;
                        //清空所画的线对象
                        (MainMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                    }
                    dToltalLength = 0;
                    dSegmentLength = 0;
                    break;
                case "MeasureArea":
                    if (pNewPolygonFeedback != null)
                    {
                        pNewPolygonFeedback.Stop();
                        pNewPolygonFeedback = null;
                        //清空所画的线对象
                        (MainMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                    }
                    pAreaPointCol.RemovePoints(0, pAreaPointCol.PointCount); //清空点集中所有点
                    break;

                default:
                    break;
            }

            #endregion
        }


        /// <summary>
        /// MainMapControl鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            sMapUnits = GetMapUnit(MainMapControl.Map.MapUnits);
            toolStripStatusLabel1.Text = String.Format("当前坐标：X = {0:#.###} Y = {1:#.###} {2}", e.mapX, e.mapY, sMapUnits);
            pMovePt = (MainMapControl.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);

            switch (pMouseOperate)
            {
                case "MeasureLength":
                    if (pNewLineFeedback != null)
                    {
                        pNewLineFeedback.MoveTo(pMovePt);
                    }
                    double deltaX = 0; //两点之间X差值
                    double deltaY = 0; //两点之间Y差值

                    if ((pPointPt != null) && (pNewLineFeedback != null))
                    {
                        deltaX = pMovePt.X - pPointPt.X;
                        deltaY = pMovePt.Y - pPointPt.Y;
                        dSegmentLength = Math.Round(Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)), 3);
                        dToltalLength = dToltalLength + dSegmentLength;
                        if (frmMeasureResult != null)
                        {
                            frmMeasureResult.lblMeasureResult.Text = String.Format(
                                "当前线段长度：{0:.###}{1};\r\n总长度为: {2:.###}{1}",
                                dSegmentLength, sMapUnits, dToltalLength);
                            dToltalLength = dToltalLength - dSegmentLength; //鼠标移动到新点重新开始计算
                        }
                        frmMeasureResult.frmClosed += new FormMeasureResult.FormClosedEventHandler(frmMeasureResult_frmColsed);
                    }
                    break;
                case "MeasureArea":
                    if (pNewPolygonFeedback != null)
                    {
                        pNewPolygonFeedback.MoveTo(pMovePt);
                    }

                    IPointCollection pPointCol = new Polygon();
                    IPolygon pPolygon = new PolygonClass();
                    IGeometry pGeo = null;

                    ITopologicalOperator pTopo = null;
                    for (int i = 0; i <= pAreaPointCol.PointCount - 1; i++)
                    {
                        pPointCol.AddPoint(pAreaPointCol.get_Point(i), ref missing, ref missing);
                    }
                    pPointCol.AddPoint(pMovePt, ref missing, ref missing);

                    if (pPointCol.PointCount < 3) return;
                    pPolygon = pPointCol as IPolygon;

                    if ((pPolygon != null))
                    {
                        pPolygon.Close();
                        pGeo = pPolygon as IGeometry;
                        pTopo = pGeo as ITopologicalOperator;
                        //使几何图形的拓扑正确
                        pTopo.Simplify();
                        pGeo.Project(MainMapControl.Map.SpatialReference);
                        IArea pArea = pGeo as IArea;

                        frmMeasureResult.lblMeasureResult.Text = String.Format(
                            "总面积为：{0:.####}平方{1};\r\n总长度为：{2:.####}{1}",
                            pArea.Area, sMapUnits, pPolygon.Length);
                        pPolygon = null;
                    }

                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 地图重载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMapControl_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            SynchronizeEagleEye();
        }

        /// <summary>
        /// 地图更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            IEnvelope envelope = e.newEnvelope as IEnvelope;
            DrawRectangle(envelope);
            TOCControl.Update();
        }

        /// <summary>
        /// 地图绘制完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMapControl_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            IActiveView pActiveView = (IActiveView)MainPageLayoutControl.ActiveView.FocusMap;
            IDisplayTransformation displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
            displayTransformation.VisibleBounds = MainMapControl.Extent;
            MainPageLayoutControl.ActiveView.Refresh();
            CopyToPageLayout();
        }

        #endregion

        #region EagleEyeMapControl事件

        /// <summary>
        /// 鹰眼地图控制器按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EagleEyeMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //if (EagleEyeMapControl.Map.LayerCount > 0)
            //{
            //    // 按下鼠标左键，移动矩形框
            //    if (e.button == 1)
            //    {
            //        if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
            //        {
            //            CanDrag = true;
            //        }
            //        MoveRectPoint = new PointClass();
            //        // 记录点击的第一个点的坐标
            //        MoveRectPoint.PutCoords(e.mapX, e.mapY);
            //    }
            //    //按下鼠标右键绘制矩形框
            //    else if (e.button == 2)
            //    {
            //        IEnvelope pEnvelope = EagleEyeMapControl.TrackRectangle();

            //        IPoint pTempPoint = new PointClass();
            //        pTempPoint.PutCoords(pEnvelope.XMin + pEnvelope.Width / 2, pEnvelope.YMin + pEnvelope.Height / 2);
            //        MainMapControl.Extent = pEnvelope;
            //        //矩形框的高宽和数据试图的高宽不一定成正比，这里做一个中心调整
            //        MainMapControl.CenterAt(pTempPoint);
            //    }
            //}
            if (EagleEyeMapControl.Map.LayerCount > 0)
            {
                //按下鼠标左键移动矩形框
                if (e.button == 1)
                {
                    //如果指针落在鹰眼的矩形框中，标记可移动
                    if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
                    {
                        CanDrag = true;
                    }
                    MoveRectPoint = new PointClass();
                    MoveRectPoint.PutCoords(e.mapX, e.mapY);  //记录点击的第一个点的坐标
                }
                //按下鼠标右键绘制矩形框
                else if (e.button == 2)
                {
                    IEnvelope pEnvelope = EagleEyeMapControl.TrackRectangle();

                    IPoint pTempPoint = new PointClass();
                    pTempPoint.PutCoords(pEnvelope.XMin + pEnvelope.Width / 2, pEnvelope.YMin + pEnvelope.Height / 2);
                    MainMapControl.Extent = pEnvelope;
                    //矩形框的高宽和数据试图的高宽不一定成正比，这里做一个中心调整
                    MainMapControl.CenterAt(pTempPoint);
                }
            }
        }

        /// <summary>
        /// 鹰眼地图矩形框移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EagleEyeMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            //if (e.mapX > pEnv.XMin && e.mapX < pEnv.XMax && e.mapY > pEnv.YMin && e.mapY < pEnv.YMax)
            //{
            //    // 如果鼠标移动到矩形框中，鼠标换成小手，表示可以移动
            //    EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerHand;
            //    // 如果在内部点击鼠标右键，将鼠标颜色的样式设置为默认
            //    if (e.button == 2)
            //    {
            //        EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            //    }
            //}
            //else
            //{
            //    //在其他位置鼠标设置为默认样式
            //    EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            //}
            //if (CanDrag)
            //{
            //    double Dx, Dy; // 记录鼠标移动的距离
            //    Dx = e.mapX - MoveRectPoint.X;
            //    Dy = e.mapY - MoveRectPoint.Y;
            //    pEnv.Offset(Dx, Dy);
            //    MoveRectPoint.PutCoords(e.mapX, e.mapY);
            //    DrawRectangle(pEnv);
            //    MainMapControl.Extent = pEnv;
            //}

            if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
            {
                //如果鼠标移动到矩形框中，鼠标换成小手，表示可以拖动
                EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerHand;
                if (e.button == 2)  //如果在内部按下鼠标右键，将鼠标演示设置为默认样式
                {
                    EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
                }
            }
            else
            {
                //在其他位置将鼠标设为默认的样式
                EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }

            if (CanDrag)
            {
                double Dx, Dy;  //记录鼠标移动的距离
                Dx = e.mapX - MoveRectPoint.X;
                Dy = e.mapY - MoveRectPoint.Y;
                pEnv.Offset(Dx, Dy); //根据偏移量更改 pEnv 位置
                MoveRectPoint.PutCoords(e.mapX, e.mapY);
                DrawRectangle(pEnv);
                MainMapControl.Extent = pEnv;
            }
        }

        /// <summary>
        /// 鹰眼地图控制器弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EagleEyeMapControl_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            //if (e.button == 1 && MoveRectPoint != null)
            //{
            //    if (e.mapX == MoveRectPoint.X && e.mapY == MoveRectPoint.Y)
            //    {
            //        MainMapControl.CenterAt(MoveRectPoint);
            //        MainMapControl.Refresh();
            //    }
            //    CanDrag = false;
            //}

            if (e.button == 1 && MoveRectPoint != null)
            {
                if (e.mapX == MoveRectPoint.X && e.mapY == MoveRectPoint.Y)
                {
                    MainMapControl.CenterAt(MoveRectPoint);
                }
                CanDrag = false;
            }
        }
        
        #endregion

        #region TOCControl事件

        private void TOCControl_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                object unk = null;
                object data = null;
                ILayer layer = null;
                TOCControl.HitTest(e.x,e.y,ref item,ref map,ref layer,ref unk,ref data);
                pTocFeatureLayer = layer as IFeatureLayer;
                if (item == esriTOCControlItem.esriTOCControlItemLayer && pTocFeatureLayer != null)
                {
                    btnLayerSel.Enabled = !pTocFeatureLayer.Selectable;
                    btnLayerUnSel.Enabled = pTocFeatureLayer.Selectable;
                    contextMenuStrip1.Show(Control.MousePosition);
                }
            }
        }

        #endregion

        #endregion

        #region Personal Method

        /// <summary>
        /// 获取打开文件的Dialog
        /// </summary>
        /// <param name="title">打开窗口的标题名称</param>
        /// <param name="filter">打开窗口的过滤信息</param>
        /// <returns>返回一个OpenFileDialog实例</returns>
        private OpenFileDialog createOpenFileDialog(string title, string filter)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.CheckFileExists = true;
            pOpenFileDialog.Title = title;
            pOpenFileDialog.Filter = filter;
            pOpenFileDialog.ShowDialog();
            return pOpenFileDialog;
        }

        /// <summary>
        /// 获取文件的文件位置以及文件名
        /// </summary>
        /// <param name="title">打开窗口的标题名称</param>
        /// <param name="filter">打开窗口的过滤信息</param>
        /// <param name="filePath">文件位置</param>
        /// <param name="fileName">文件名称</param>
        private void outFilePathAndFileName(string title, string filter, out string filePath, out string fileName)
        {
            filePath = null;
            fileName = null;
            OpenFileDialog pOpenFileDialog = createOpenFileDialog(title, filter);
            String pFullpath = pOpenFileDialog.FileName;
            if (pFullpath == "")
            {
                return;
            }
            filePath = System.IO.Path.GetDirectoryName(pFullpath);
            fileName = System.IO.Path.GetFileName(pFullpath);
        }

        #endregion

        #region  4.5 查询选项设置

        /// <summary>
        /// 查询选项设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemOptions_Click(object sender, EventArgs e)
        {
            //新创建选择操作选项窗体
            FormOptions formOptions = new FormOptions();
            //将当前主窗体中MapControl控件中的Map对象赋值给FormOptions窗体的CurrentMap属性
            formOptions.CurrentSelectionEnvironment = selectionEnvironment;
            //显示选择操作选项窗体
            formOptions.Show();
        }









        #endregion

    }
}
