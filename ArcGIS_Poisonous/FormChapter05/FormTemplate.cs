using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace ArcGIS_Poisonous.FormChapter05
{
    public partial class FormTemplate : Form
    {
        private AxPageLayoutControl mMainPageLayoutControl;

        private static string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        string spath = getPath(filepath) + "\\data\\Symbol\\Templates";//载入系统模板       
        private string sTemplatePath = string.Empty;
        private string sExtention = ".mxt";

        public FormTemplate(AxPageLayoutControl pageLayoutControl)
        {
            InitializeComponent();

            mMainPageLayoutControl = pageLayoutControl;

            InitUI();
        }

        private void InitUI()
        {
            try
            {
                List<string> listDirName = null;
                List<string> listFileName = null;

                string fileName = string.Empty;
                string parentName = string.Empty;

                // 获取指定路径文件夹下文件夹名称
                listDirName = GetChildDirectoryName(spath);

                for (int i = 0; i < listDirName.Count; i++)
                {
                    parentName = listDirName[i];
                    TreeNode parentNode = new TreeNode();
                    parentNode.Text = parentName;
                    parentNode.ExpandAll();
                    listFileName = GetFiles(spath + "\\" + parentName);
                    for (int j = 0; j < listFileName.Count; j++)
                    {
                        fileName = listFileName[i];
                        // 获取除去后缀类型外的样式名
                        fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                        TreeNode ssan = new TreeNode();
                        ssan.Text = fileName;
                        parentNode.Nodes.Add(ssan);
                    }
                    tlstTemplate.Nodes.Add(parentNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sTemplatePath))
            {
                //类中的界面控件也可以控制主界面
                useTemplateMxtToPageLayout(mMainPageLayoutControl, sTemplatePath);
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择要应用模板！", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
        }
        #region 模板替换
        /// <summary>
        /// 模板替换
        /// </summary>
        /// <param name="_pageLayoutCtrl"></param>
        /// <param name="sTemplatePath"></param>
        /// <returns></returns>
        public static bool useTemplateMxtToPageLayout(AxPageLayoutControl _pageLayoutCtrl, string sTemplatePath)
        {
            bool bSuccess = false;
            try
            {
                IMap pMap = null;
                IActiveView pActiveView = null;
                IPageLayout pCurPageLayout = _pageLayoutCtrl.PageLayout;
                pActiveView = pCurPageLayout as IActiveView;
                pMap = pActiveView.FocusMap;

                //读取模板
                IMapDocument pTempMapDocument = new MapDocumentClass();
                pTempMapDocument.Open(sTemplatePath, "");

                IPageLayout pTempPageLayout = pTempMapDocument.PageLayout;
                IPage pTempPage = pTempPageLayout.Page;
                IPage pCurPage = pCurPageLayout.Page;

                //替换单位及地图方向
                pCurPage.Orientation = pTempPage.Orientation;
                pCurPage.Units = pTempPage.Units;

                //替换页面尺寸
                Double dWidth; Double dHeight;
                pTempPage.QuerySize(out dWidth, out dHeight);
                pCurPage.PutCustomSize(dWidth, dHeight);

                //删除当前layout中除了mapframe外的所有element
                IGraphicsContainer pGraph;
                pGraph = pCurPageLayout as IGraphicsContainer;
                pGraph.Reset();
                IElement pElement = pGraph.Next();
                IMapFrame pMapFrame = null;
                pMapFrame = pGraph.FindFrame(pMap) as IMapFrame;
                while (pElement != null)
                {
                    if (pElement is IMapFrame)
                    {
                        pMapFrame = pElement as IMapFrame;
                    }
                    else
                    {
                        pGraph.DeleteElement(pElement);
                        pGraph.Reset();
                    }
                    pElement = pGraph.Next();
                }

                //遍历模板中的PageLayout所有元素，替换当前PageLayout的所有元素
                IGraphicsContainer pTempGraph = pTempPageLayout as IGraphicsContainer;
                pTempGraph.Reset();
                pElement = pTempGraph.Next();
                IArray pArray = new ArrayClass();
                while (pElement != null)
                {
                    if (pElement is IMapFrame)
                    {
                        IElement pMapFrameElement = pMapFrame as IElement;
                        pMapFrameElement.Geometry = pElement.Geometry;
                    }
                    else
                    {
                        if (pElement is IMapSurroundFrame)
                        {
                            IMapSurroundFrame pTempMapSurroundFrame = pElement as IMapSurroundFrame;
                            pTempMapSurroundFrame.MapFrame = pMapFrame;
                            IMapSurround pTempMapSurround = pTempMapSurroundFrame.MapSurround;
                        }
                        pArray.Add(pElement);
                    }
                    pElement = pTempGraph.Next();
                }

                int pElementCount = pArray.Count;
                for (int i = 0; i < pArray.Count; i++)
                {
                    pGraph.AddElement(pArray.get_Element(pElementCount - 1 - i) as IElement, 0);
                }

                pActiveView.Refresh();
                bSuccess = true;
            }
            catch (Exception ex)
            {

            }
            return bSuccess;
        }
        #endregion     

        private bool UseTemplateMxtToPageLayout(AxPageLayoutControl mainPageLayoutControl, string sTemplatePath)
        {
            bool success = false;

            try
            {
                IMap map = null;
                IActiveView activeView = null;
                IPageLayout curPageLayout = mainPageLayoutControl.PageLayout;
                activeView = curPageLayout as IActiveView;
                map = activeView.FocusMap;

                // 读取模板
                IMapDocument tempMapDocument = new MapDocumentClass();
                tempMapDocument.Open(sTemplatePath, "");
                IPageLayout tempPageLayout = tempMapDocument.PageLayout;
                IPage tempPage = tempPageLayout.Page;
                IPage curPage = curPageLayout.Page;

                // 替换单位及地图方向
                curPage.Orientation = tempPage.Orientation;
                curPage.Units = tempPage.Units;

                // 替换页面尺寸
                Double width, height;
                tempPage.QuerySize(out width, out height);
                curPage.PutCustomSize(width, height);

                // 删除当前layout中除了mapframe外所有element
                IGraphicsContainer graphicsContainer = curPageLayout as IGraphicsContainer;
                graphicsContainer.Reset();
                IElement element = graphicsContainer.Next();
                IMapFrame mapFrame = null;
                mapFrame = graphicsContainer.FindFrame(map) as IMapFrame;
                while (element != null)
                {
                    if (element is IMapFrame)
                    {
                        mapFrame = element as IMapFrame;
                    }
                    else
                    {
                        graphicsContainer.DeleteElement(element);
                        graphicsContainer.Reset();
                    }
                    element = graphicsContainer.Next();
                }

                // 遍历模板中的PageLayout所有元素，替换当前PageLayout的所有元素
                IGraphicsContainer tempGraphicsContainer = tempPageLayout as IGraphicsContainer;
                tempGraphicsContainer.Reset();
                IArray array = new ArrayClass();
                while (element != null)
                {
                    if (element is IMapFrame)
                    {
                        IElement mapFrameElement = mapFrame as IElement;
                        mapFrameElement.Geometry = element.Geometry;
                    }
                    else
                    {
                        if (element is IMapSurroundFrame)
                        {
                            IMapSurroundFrame mapSurroundFrame = element as IMapSurroundFrame;
                            mapSurroundFrame.MapFrame = mapFrame;
                            IMapSurround tempMapSurround = mapSurroundFrame.MapSurround;
                        }
                        array.Add(element);
                    }
                    element = tempGraphicsContainer.Next();
                }

                int elementCount = array.Count;
                for (int i = 0; i < array.Count; i++)
                {
                    graphicsContainer.AddElement(array.get_Element(elementCount - 1 - i) as IElement, 0);
                }
                activeView.Refresh();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return success;
        }

        private void tlstTemplate_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                // 鼠标获取的节点
                tlstTemplate.SelectedNode = e.Node;
                // 当前节点
                TreeNode focuseNode = tlstTemplate.SelectedNode;
                TreeNode parentNode = new TreeNode();
                if (focuseNode != null)
                {
                    string dirName = string.Empty;
                    string fileName = string.Empty;
                    string filePath = string.Empty;
                    parentNode = focuseNode.Parent;
                    fileName = focuseNode.Text + sExtention;

                    // 有父节点
                    if (parentNode != null)
                    {
                        dirName = parentNode.Text;
                        filepath = spath + "\\" + dirName + "\\" + fileName;
                    }
                    // 没有父节点，即第一级目录的样式
                    else
                    {
                        filepath = spath + "\\" + fileName;
                    }

                    if (pageLayoutCtrlMxt.CheckMxFile(filepath))
                    {
                        pageLayoutCtrlMxt.LoadMxFile(filepath);
                        sTemplatePath = filepath;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); ;
            }
        }


        #region 获取项目文件夹地址

        /// <summary>
        /// 获取项目文件夹地址
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getPath(string path)
        {
            string project_name = "ArcGIS_Poisonous";

            int t;
            for (t = 0; t < path.Length; t++)
            {

                if (path.Substring(t, project_name.Length) == project_name)
                {
                    break;
                }
            }
            string name = path.Substring(0, t + project_name.Length);
            return name;
        }

        #endregion

        #region 获取指定路径文件夹下子文件夹名称
        /// <summary>
        /// 获取指定路径文件夹下子文件夹名称
        /// </summary>
        /// <param name="sDirPath"></param>
        /// <returns></returns>
        public static List<string> GetChildDirectoryName(string sDirPath)
        {
            List<string> plstDirName = null;
            try
            {
                string sDirName = string.Empty;
                plstDirName = new List<string>();
                DirectoryInfo _direcInfo = new DirectoryInfo(sDirPath);
                DirectoryInfo[] _dirInfo = _direcInfo.GetDirectories();
                foreach (DirectoryInfo _directoryInfo in _dirInfo)
                {
                    sDirName = _directoryInfo.Name;
                    if (!plstDirName.Contains(sDirName))
                    {
                        plstDirName.Add(sDirName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return plstDirName;
        }
        #endregion

        #region 获取指定文件夹下文件名称
        /// <summary>
        /// 获取指定文件夹下文件名称
        /// </summary>
        /// <param name="sDirPath"></param>
        /// <returns></returns>
        public static List<string> GetFiles(string sDirPath)
        {
            List<string> plstFileName = null;
            try
            {
                FileInfo[] _fileInfo = null;
                string sFileName = string.Empty;
                plstFileName = new List<string>();

                DirectoryInfo _direcInfo = new DirectoryInfo(sDirPath);
                _fileInfo = _direcInfo.GetFiles();
                foreach (FileInfo _fInfo in _fileInfo)
                {
                    sFileName = _fInfo.Name;
                    if (!plstFileName.Contains(sFileName))
                    {
                        plstFileName.Add(sFileName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return plstFileName;
        }
        #endregion
    }
}
