using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;

namespace ArcGIS_Poisonous
{
    // Chapter03
    public partial class ArcGisPoisonous : Form
    {
        #region 3 地图基本操作

        #region 3.3 数据加载

        #region 3.3.1 加载地图文档

        /// <summary>
        /// 使用IMapControl接口的LoadFxFile方法加载地图文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadMxFile_Click(object sender, EventArgs e)
        {
            //加载数据前如果有数据则清空
            try
            {
                OpenFileDialog pOpenFileDialog = new OpenFileDialog();
                pOpenFileDialog.CheckFileExists = true;
                pOpenFileDialog.Title = "打开地图文档";
                pOpenFileDialog.Filter = "ArcMap文档(*.mxd)|*.mxd;|ArcMap模板(*.mxt)|*.mxt|发布地图文件(*.pmf)|*.pmf|所有地图格式(*.mxd;*.mxt;*.pmf)|*.mxd;*.mxt;*.pmf";
                pOpenFileDialog.Multiselect = false;   //不允许多个文件同时选择
                pOpenFileDialog.RestoreDirectory = true;   //存储打开的文件路径
                if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string pFileName = pOpenFileDialog.FileName;
                    if (pFileName == "")
                    {
                        return;
                    }

                    if (MainMapControl.CheckMxFile(pFileName)) //检查地图文档有效性
                    {
                        ClearAllData();
                        MainMapControl.LoadMxFile(pFileName);
                    }
                    else
                    {
                        MessageBox.Show(pFileName + "是无效的地图文档!", "信息提示");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开地图文档失败" + ex.Message);
            }
        }

        /// <summary>
        /// 通过IMapDocument接口加载地图文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMapDocument_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog pOpenFileDialog = new OpenFileDialog();
                pOpenFileDialog.CheckFileExists = true;
                pOpenFileDialog.Title = "打开地图文档";
                pOpenFileDialog.Filter = "ArcMap文档(*.mxd)|*.mxd;|ArcMap模板(*.mxt)|*.mxt|发布地图文件(*.pmf)|*.pmf|所有地图格式(*.mxd;*.mxt;*.pmf)|*.mxd;*.mxt;*.pmf";
                pOpenFileDialog.Multiselect = false;
                pOpenFileDialog.RestoreDirectory = true;
                if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string pFileName = pOpenFileDialog.FileName;
                    if (pFileName == "")
                    {
                        return;
                    }

                    if (MainMapControl.CheckMxFile(pFileName)) //检查地图文档有效性
                    {
                        //将数据载入pMapDocument并与Map控件关联
                        IMapDocument pMapDocument = new MapDocument();//using ESRI.ArcGIS.Carto;
                        pMapDocument.Open(pFileName, "");
                        //获取Map中激活的地图文档
                        MainMapControl.Map = pMapDocument.ActiveView.FocusMap;
                        MainMapControl.ActiveView.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(pFileName + "是无效的地图文档!", "信息提示");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开地图文档失败" + ex.Message);
            }
        }

        /// <summary>
        /// 使用ControlsOpenDocCommandClass加载地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnControlOpenDocCommandClass_Click(object sender, EventArgs e)
        {
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(MainMapControl.Object);
            command.OnClick();
        }

        #endregion

        #region 3.3.2 加载Shapefile数据

        /// <summary>
        /// 加载Shapefile数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddShapefile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog pOpenFileDialog = new OpenFileDialog();
                pOpenFileDialog.CheckFileExists = true;
                pOpenFileDialog.Title = "打开Shape文件";
                pOpenFileDialog.Filter = "Shape文件（*.shp）|*.shp";
                pOpenFileDialog.ShowDialog();
                // 获取文件路径
                IWorkspaceFactory pWorkspaceFactory;
                IFeatureWorkspace pFeatureWorkspace;
                IFeatureLayer pFeatureLayer;
                string pFullPath = pOpenFileDialog.FileName;
                if (pFullPath == "")
                {
                    return;
                }
                int pIndex = pFullPath.LastIndexOf("\\");
                // 文件路径
                string pFilePath = pFullPath.Substring(0, pIndex);
                // 文件名称
                string pFileName = pFullPath.Substring(pIndex + 1);

                // 实例化ShapefileWorkspaceFactory工作空间，打开shp文件
                pWorkspaceFactory = new ShapefileWorkspaceFactory();
                pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(pFilePath, 0);
                // 创建并实例化要素集
                IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);
                pFeatureLayer = new FeatureLayer();
                pFeatureLayer.FeatureClass = pFeatureClass;
                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;

                // 删除所有已经加载的数据
                ClearAllData();

                MainMapControl.Map.AddLayer(pFeatureLayer);
                MainMapControl.ActiveView.Refresh();

                // 同步鹰眼
                SynchronizeEagleEye();
            }
            catch (Exception ex)
            {
                MessageBox.Show(".shp文件加载失败！" + ex.Message, "报错");
            }
        }

        #endregion

        #region 3.3.3 加载栅格数据

        /// <summary>
        /// 加载栅格数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRaster_Click(object sender, EventArgs e)
        {
            string title = "打开Raster文件";
            string filter = "栅格文件 (*.*)|*.bmp;*.tif;*.jpg;*.img|(*.bmp)|*.bmp|(*.tif)|*.tif|(*.jpg)|*.jpg|(*.img)|*.img";
            OpenFileDialog pOpenFileDialog = createOpenFileDialog(title, filter);
            string pRasterFileName = pOpenFileDialog.FileName;
            if (pRasterFileName == "")
            {
                return;
            }
            string pPath = System.IO.Path.GetDirectoryName(pRasterFileName);
            string pFileName = System.IO.Path.GetFileName(pRasterFileName);

            IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactory();
            IRasterWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pPath, 0) as IRasterWorkspace;
            IRasterDataset pRasterDataSet = pWorkspace.OpenRasterDataset(pFileName);

            // 影像金字塔判断与创建
            IRasterPyramid3 pRasPyrmid;
            pRasPyrmid = pRasterDataSet as IRasterPyramid3;
            if (pRasPyrmid != null)
            {
                if (!(pRasPyrmid.Present))
                {
                    // 创建金字塔
                    pRasPyrmid.Create();
                }
            }

            IRaster pRaster = pRasterDataSet.CreateDefaultRaster();
            IRasterLayer pRasterLayer = new RasterLayerClass();
            pRasterLayer.CreateFromRaster(pRaster);
            ILayer pLayer = pRasterLayer as ILayer;
            MainMapControl.AddLayer(pLayer);
        }

        #endregion

        #region 3.3,4 加载CAD数据

        /// <summary>
        /// CAD文件作为矢量图层加载
        /// 分图层加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCADByLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath;
            string fileName;
            string filter = "CAD(*.dwg)|*.dwg";
            string title = "打开CAD数据文件";
            outFilePathAndFileName(title, filter, out filePath, out fileName);
            if (fileName == null || filePath == null)
            {
                return;
            }

            IWorkspaceFactory pWorkspaceFactory = new CadWorkspaceFactory();
            IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(filePath, 0) as IFeatureWorkspace;

            // 加载CAD文件中的线文件
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(fileName + ":polyline");
            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            pFeatureLayer.Name = fileName;
            pFeatureLayer.FeatureClass = pFeatureClass;

            // 新增删除数据
            ClearAllData();

            MainMapControl.AddLayer(pFeatureLayer);
            MainMapControl.Refresh();

            //同步鹰眼
            SynchronizeEagleEye();
        }

        /// <summary>
        /// CAD文件作为矢量图层加载
        /// 整幅图加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddWhileCADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath;
            string fileName;
            string filter = "CAD(*.dwg)|*.dwg";
            string title = "打开CAD数据文件";
            outFilePathAndFileName(title, filter, out filePath, out fileName);
            if (fileName == null || filePath == null)
            {
                return;
            }
            // 打开一个CAD数据集
            IWorkspaceFactory pWorkspaceFactory = new CadWorkspaceFactory();
            IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(filePath, 0) as IFeatureWorkspace;
            // 打开一个要素集
            IFeatureDataset pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(fileName);
            // IFeatureClassContainer 可以管理IFeatureDataset中的每个要素类
            IFeatureClassContainer pFeatureClassContainer = pFeatureDataset as IFeatureClassContainer;

            // 新增删除数据
            ClearAllData();

            // 对CAD文件中的要素进行遍历处理
            for (int i = 0; i < pFeatureClassContainer.ClassCount; i++)
            {
                IFeatureClass pFeatClass = pFeatureClassContainer.get_Class(i);

                // 如果是注记，则添加注记层
                if (pFeatClass.FeatureType == esriFeatureType.esriFTCoverageAnnotation)
                {
                    IFeatureLayer pFeatureLayer = new CadAnnotationLayerClass();
                    pFeatureLayer.Name = pFeatClass.AliasName;
                    pFeatureLayer.FeatureClass = pFeatClass;
                    MainMapControl.AddLayer(pFeatureLayer);
                }
                // 如果是点线面则添加要素层
                else
                {
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.Name = pFeatClass.AliasName;
                    pFeatureLayer.FeatureClass = pFeatClass;
                    MainMapControl.AddLayer(pFeatureLayer);
                }
                MainMapControl.Refresh();
            }

            // 同步鹰眼
            SynchronizeEagleEye();
        }

        /// <summary>
        /// CAD文件作为栅格图层加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRasterByCADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath;
            string fileName;
            string filter = "CAD(*.dwg)|*.dwg";
            string title = "打开CAD数据文件";
            outFilePathAndFileName(title, filter, out filePath, out fileName);
            if (fileName == null || filePath == null)
            {
                return;
            }
            IWorkspaceFactory pWorkspaceFactory = new CadWorkspaceFactory();
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(filePath, 0);
            ICadDrawingWorkspace pCadDrawingWorkspace = pWorkspace as ICadDrawingWorkspace;
            // 获取CAD数据集
            ICadDrawingDataset pCadDrawingDataset = pCadDrawingWorkspace.OpenCadDrawingDataset(fileName);
            ICadLayer pCadLayer = new CadLayerClass();
            pCadLayer.CadDrawingDataset = pCadDrawingDataset;
            MainMapControl.AddLayer(pCadLayer);
            MainMapControl.Refresh();
        }

        #endregion

        #region 3.3.5 加载个人地理数据库数据

        /// <summary>
        /// 加载个人地理数据库数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPersonalGeodatabase_Click(object sender, EventArgs e)
        {
            string title = "打开Personal Geodatabase文件";
            string filter = "Personal Geodatabase(*.mdb)|*.mdb";
            OpenFileDialog pOpenFileDialog = createOpenFileDialog(title, filter);
            string pFullPath = pOpenFileDialog.FileName;
            if (pFullPath == "")
            {
                return;
            }
            IWorkspaceFactory pAccessWorkspcaeFactory = new AccessWorkspaceFactory();
            // 获取工作空间
            IWorkspace pWorkspace = pAccessWorkspcaeFactory.OpenFromFile(pFullPath, 0);
            ClearAllData();
            // 加载工作空间的数据
            AddAllDataset(pWorkspace, MainMapControl);
        }

        #endregion

        #region 3.3.6 加载文件地理信息数据库

        /// <summary>
        /// 加载文件地理信息数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFileGeodatabase_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string pFullPaht = dlg.SelectedPath;

            IWorkspaceFactory pFileGDBWorkspaceFactory = new FileGDBWorkspaceFactoryClass();

            ClearAllData();
            // 获取工作空间
            IWorkspace pWorkspace = pFileGDBWorkspaceFactory.OpenFromFile(pFullPaht, 0);
            AddAllDataset(pWorkspace, MainMapControl);
        }

        #endregion

        #endregion

        #region 3.4 地图文档保存

        #region 3.4.1 地图文档保存

        /// <summary>
        /// 地图文档保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveMap_Click(object sender, EventArgs e)
        {
            try
            {
                string sMxdFileName = MainMapControl.DocumentFilename;
                IMapDocument pMapDocument = new MapDocumentClass();
                if (sMxdFileName != null && MainMapControl.CheckMxFile(sMxdFileName))
                {
                    if (pMapDocument.get_IsReadOnly(sMxdFileName))
                    {
                        MessageBox.Show("本地图文档是只读,不能保存!");
                        pMapDocument.Close();
                        return;
                    }
                    else
                    {
                        SaveFileDialog pSaveFileDialog = new SaveFileDialog();
                        pSaveFileDialog.Title = "请选择保存路径";
                        pSaveFileDialog.OverwritePrompt = true;
                        pSaveFileDialog.Filter = "ArcMap文档（*.mxd）|*.mxd|ArcMap模板（*.mxt）|*.mxt";
                        pSaveFileDialog.RestoreDirectory = true;
                        if (pSaveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            sMxdFileName = pSaveFileDialog.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }

                    pMapDocument.New(sMxdFileName);
                    pMapDocument.ReplaceContents(MainMapControl.Map as IMxdContents);
                    pMapDocument.Save(pMapDocument.UsesRelativePaths, true);
                    pMapDocument.Close();
                    MessageBox.Show("保存地图文档成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 3.4.2 地图文档另存为

        /// <summary>
        /// 地图文档另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAsMap_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog pSaveFileDialog = new SaveFileDialog();
                pSaveFileDialog.Title = "另存为";
                pSaveFileDialog.OverwritePrompt = true;
                pSaveFileDialog.Filter = "ArcMap文档（*.mxd）|*.mxd|ArcMap模板（*.mxt）|*.mxt";
                pSaveFileDialog.RestoreDirectory = true;
                if (pSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sFilePath = pSaveFileDialog.FileName;
                    IMapDocument pMapDocument = new MapDocumentClass();
                    pMapDocument.New(sFilePath);
                    pMapDocument.ReplaceContents(MainMapControl.Map as IMxdContents);
                    pMapDocument.Save(true, true);
                    pMapDocument.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #endregion

        #region 3.5 地图浏览

        #region 3.5.1 放大与缩小

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomOutStep_Click(object sender, EventArgs e)
        {
            IActiveView pActiveView = MainMapControl.ActiveView;
            IPoint centerPoint = new PointClass();
            centerPoint.PutCoords((pActiveView.Extent.XMax + pActiveView.Extent.XMin) / 2, (pActiveView.Extent.YMax + pActiveView.Extent.YMin) / 2);
            IEnvelope pEnvelope = pActiveView.Extent;
            pEnvelope.Expand(1.5, 1.5, true);
            pActiveView.Extent.CenterAt(centerPoint);
            pActiveView.Extent = pEnvelope;
            pActiveView.Refresh();
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomInStep_Click(object sender, EventArgs e)
        {
            IEnvelope pEnvenlop;
            pEnvenlop = MainMapControl.Extent;
            // 设置放大两倍，可以根据需求进行设置
            pEnvenlop.Expand(0.5, 0.5, true);
            MainMapControl.Extent = pEnvenlop;
            MainMapControl.ActiveView.Refresh();
        }

        #endregion

        #region 3.5.2 拉框放大与缩小



        /// <summary>
        /// 拉框放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            MainMapControl.CurrentTool = null;
            pMouseOperate = "ZoomIn";
            MainMapControl.MousePointer = esriControlsMousePointer.esriPointerZoomIn;
        }

        /// <summary>
        /// 拉框缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            MainMapControl.CurrentTool = null;
            pMouseOperate = "ZoomOut";
            MainMapControl.MousePointer = esriControlsMousePointer.esriPointerZoomOut;
        }

        #endregion

        #region 3.5.3 漫游

        /// <summary>
        /// 漫游拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPan_Click(object sender, EventArgs e)
        {
            MainMapControl.CurrentTool = null;
            pMouseOperate = "Pan";
            MainMapControl.MousePointer = esriControlsMousePointer.esriPointerZoomOut;
        }



        #endregion

        #region 3.5.4 全图显示

        /// <summary>
        /// 全图显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFullView_Click(object sender, EventArgs e)
        {
            MainMapControl.Extent = MainMapControl.FullExtent;
        }

        #endregion

        #region 3.5.5 历史视图切换

        /// <summary>
        /// 前一个视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrontView_Click(object sender, EventArgs e)
        {
            pExtentStack = MainMapControl.ActiveView.ExtentStack;
            if (pExtentStack.CanUndo())
            {
                pExtentStack.Undo();
                btnForWardView.Enabled = true;
                if (!pExtentStack.CanUndo())
                {
                    btnFrontView.Enabled = false;
                }
                MainMapControl.ActiveView.Refresh();
            }
        }

        /// <summary>
        /// 后一个视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForWardView_Click(object sender, EventArgs e)
        {
            pExtentStack = MainMapControl.ActiveView.ExtentStack;
            //判断是否可以回到后一视图，最后一个视图没有后一视图
            if (pExtentStack.CanRedo())
            {
                pExtentStack.Redo();
                btnFrontView.Enabled = true;
                if (!pExtentStack.CanRedo())
                {
                    btnForWardView.Enabled = false;
                }
            }
            MainMapControl.ActiveView.Refresh();
        }

        #endregion

        #endregion

        #region 3.6 书签

        #region 3.6.1 添加书签

        /// <summary>
        /// 添加书签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddBookMark_Click(object sender, EventArgs e)
        {
            FormBookmark formBookmark = new FormBookmark();
            formBookmark.ShowDialog();
            string pName = string.Empty;
            pName = formBookmark.BookMark;
            if (!formBookmark.Check || string.IsNullOrEmpty(pName))
            {
                return;
            }

            // 书签进行重命名判断
            IMapBookmarks mapBookmarks = MainMapControl.Map as IMapBookmarks;
            IEnumSpatialBookmark enumSpatialBookmark = mapBookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark;
            while ((spatialBookmark = enumSpatialBookmark.Next()) != null)
            {
                if (pName == spatialBookmark.Name)
                {
                    DialogResult dr = MessageBox.Show("书签重命名，时候替换？", "提示", MessageBoxButtons.YesNoCancel);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            mapBookmarks.RemoveBookmark(spatialBookmark);
                            break;
                        case DialogResult.No:
                            formBookmark.ShowDialog();
                            break;
                        default:
                            return;
                    }
                }
            }
            // 获取当前地图的对象
            IActiveView activeView = MainMapControl.Map as IActiveView;
            // 创建一个新的书签并设置其位置为当前视图范围
            IAOIBookmark pBookmark = new AOIBookmark();
            pBookmark.Location = activeView.Extent;
            // 获得书签名称
            pBookmark.Name = pName;
            // 通过IMapBookmarks访问当前地图书签集，添加书签到地图的书签集中
            IMapBookmarks pMapBookmarks = MainMapControl.Map as IMapBookmarks;
            pMapBookmarks.AddBookmark(pBookmark);

        }

        #endregion

        #region 3.6.2 管理书签

        /// <summary>
        ///   管理书签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMangeBookMark_Click(object sender, EventArgs e)
        {
            try
            {
                FormManageBookMarks frmManageBookMark = new FormManageBookMarks(MainMapControl.Map);
                frmManageBookMark.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #endregion

        #region 3.8 测量

        #region 3.8.2 距离测量

        /// <summary>
        /// 距离量测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMeasureLength_Click(object sender, EventArgs e)
        {
            MainMapControl.CurrentTool = null;
            pMouseOperate = "MeasureLength";
            MainMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            if (frmMeasureResult == null || frmMeasureResult.IsDisposed)
            {
                frmMeasureResult = new FormMeasureResult();
                frmMeasureResult.frmClosed += new FormMeasureResult.FormClosedEventHandler(frmMeasureResult_frmColsed);
                frmMeasureResult.lblMeasureResult.Text = "";
                frmMeasureResult.Text = "距离量测";
                frmMeasureResult.Show();
            }
            else
            {
                frmMeasureResult.Activate();
            }
        }

        #endregion

        #region 3.8.3 面积测量

        /// <summary>
        /// 面积测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMeasureArea_Click(object sender, EventArgs e)
        {
            MainMapControl.CurrentTool = null;
            pMouseOperate = "MeasureArea";
            MainMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            if (frmMeasureResult == null || frmMeasureResult.IsDisposed)
            {
                frmMeasureResult = new FormMeasureResult();
                frmMeasureResult.frmClosed += new FormMeasureResult.FormClosedEventHandler(frmMeasureResult_frmColsed);
                frmMeasureResult.lblMeasureResult.Text = "";
                frmMeasureResult.Text = "面积量测";
                frmMeasureResult.Show();
            }
            else
            {
                frmMeasureResult.Activate();
            }
        }

        #endregion

        /// <summary>
        /// 测量结果窗口关闭响应事件
        /// </summary>
        private void frmMeasureResult_frmColsed()
        {
            //清空线对象
            if (pNewLineFeedback != null)
            {
                pNewLineFeedback.Stop();
                pNewLineFeedback = null;
            }
            //清空面对象
            if (pNewPolygonFeedback != null)
            {
                pNewPolygonFeedback.Stop();
                pNewPolygonFeedback = null;
                pAreaPointCol.RemovePoints(0, pAreaPointCol.PointCount); //清空点集中所有点
            }
            //清空量算画的线、面对象
            MainMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            //结束量算功能
            pMouseOperate = string.Empty;
            MainMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
        }

        #endregion

        #region 3.9 要素选择操作

        #region 3.9.1 要素选择

        /// <summary>
        /// 要素选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelFeature_Click(object sender, EventArgs e)
        {
            // 调用资源库的方式实现
            //MainMapControl.CurrentTool = null;
            //ControlsSelectFeaturesTool pTool = new ControlsSelectFeaturesToolClass();
            //pTool.OnCreate(MainMapControl.Object);
            //MainMapControl.CurrentTool = pTool as ITool;

            // 调用类库资源实现
            pMouseOperate = "SelFeature";
        }

        #endregion

        #region 3.9.2缩放至选择

        /// <summary>
        /// 缩放至选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomToSel_Click(object sender, EventArgs e)
        {
            // 调用资源类库实现
            //ICommand command = new ESRI.ArcGIS.Controls.ControlsZoomToSelectedCommandClass();
            //command.OnCreate(MainMapControl.Object);
            //command.OnClick();

            // 调用类库资源实现
            int nSlection = MainMapControl.Map.SelectionCount;
            if (nSlection == 0)
            {
                MessageBox.Show("请选择要素！", "提示");
            }
            else
            {
                ISelection selection = MainMapControl.Map.FeatureSelection;
                IEnumFeature enumFeature = selection as IEnumFeature;
                enumFeature.Reset();
                IEnvelope envelope = new EnvelopeClass();
                IFeature feature = enumFeature.Next();
                while (feature != null)
                {
                    envelope.Union(feature.Extent);
                    feature = enumFeature.Next();
                }
                envelope.Expand(1.1, 1.1, true);
                MainMapControl.ActiveView.Extent = envelope;
                MainMapControl.ActiveView.Refresh();
            }
        }

        #endregion

        #region 3.9.3清除选择

        /// <summary>
        /// 清除选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSel_Click(object sender, EventArgs e)
        {
            // 调用类库资源实现
            //ICommand command = new ESRI.ArcGIS.Controls.ControlsClearSelectionCommandClass();
            //command.OnCreate(MainMapControl.Object);
            //command.OnClick();

            // 编写代码实现
            IActiveView activeView = MainMapControl.ActiveView;
            activeView.FocusMap.ClearSelection();
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);
        }

        #endregion

        #endregion

        #region 3.10 地图导出

        #region 3.10.1 全域导出

        /// <summary>
        /// 全域导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportMap_Click(object sender, EventArgs e)
        {
            if (FrmExpMap == null || FrmExpMap.IsDisposed)
            {
                FrmExpMap = new FormExportMap(MainMapControl);
            }
            FrmExpMap.IsRegion = false;
            FrmExpMap.GetGometry = MainMapControl.ActiveView.Extent;
            FrmExpMap.Show();
            FrmExpMap.Activate();
        }

        #endregion

        #region 3.10.2 区域导出

        /// <summary>
        /// 区域导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportRegion_Click(object sender, EventArgs e)
        {
            MainMapControl.CurrentTool = null;
            MainMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            pMouseOperate = "ExportRegion";
        }

        #endregion

        #endregion

        #region 3.12 TOCControl控件

        #region 3.12.3 TOCControl的右键菜单

        /// <summary>
        /// 属性菜单点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAttribute_Click(object sender, EventArgs e)
        {
            if (frmAttribute == null || frmAttribute.IsDisposed)
            {
                frmAttribute = new FormAtrribute();
            }
            frmAttribute.CurFeatureLayer = pTocFeatureLayer;
            frmAttribute.InitUI();
            frmAttribute.ShowDialog();
        }

        /// <summary>
        /// 缩放到图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomToLayer_Click(object sender, EventArgs e)
        {
            if (pTocFeatureLayer == null)
            {
                return;
            }
            (MainMapControl.Map as IActiveView).Extent = pTocFeatureLayer.AreaOfInterest;
            (MainMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        /// <summary>
        /// 移除图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveLayer_Click(object sender, EventArgs e)
        {
            if (pTocFeatureLayer == null)
            {
                return;
            }
            DialogResult result = MessageBox.Show("时候删除【" + pTocFeatureLayer.Name + "】图层", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                MainMapControl.Map.DeleteLayer(pTocFeatureLayer);
            }
            MainMapControl.ActiveView.Refresh();
        }

        /// <summary>
        /// 图层可选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLayerSel_Click(object sender, EventArgs e)
        {
            pTocFeatureLayer.Selectable = true;
            btnLayerSel.Enabled = !btnLayerSel.Enabled;
        }

        /// <summary>
        /// 图层不可选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLayerUnSel_Click(object sender, EventArgs e)
        {
            pTocFeatureLayer.Selectable = false;
            btnLayerSel.Enabled = !btnLayerSel.Enabled;
        }


        #endregion


        #endregion

        #endregion
    }
}
