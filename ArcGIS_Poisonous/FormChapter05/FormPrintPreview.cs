using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using System;
using System.Collections;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ArcGIS_Poisonous.FormChapter05
{
    public partial class FormPrintPreview : Form
    {
        private AxPageLayoutControl mMainPageLayoutControl;

        //定义页面设置、打印预览和打印对话框
        private PrintPreviewDialog printPreviewDialog1;
        private PrintDialog printDialog1;
        private PageSetupDialog pageSetupDialog1;

        // 定义页数变量
        private short m_CurrentPrintPage;

        public FormPrintPreview(AxPageLayoutControl pageLayoutControl)
        {
            InitializeComponent();

            mMainPageLayoutControl = pageLayoutControl;

            syn(pageLayoutControl);
        }


        [STAThread]
        private void Form1_Load(object sender, System.EventArgs e)
        {
            InitializePrintPreviewDialog(); //初始化打印预览对话框
            printDialog1 = new PrintDialog(); //实例化打印对话框
            InitializePageSetupDialog(); //初始化打印设置对话                    
        }

        private void FormPrintPreview_Load(object sender, EventArgs e)
        {
            InitializePrintPreviewDialog(); //初始化打印预览对话框
            printDialog1 = new PrintDialog(); //实例化打印对话框
            InitializePageSetupDialog(); //初始化打印设置对话       
        }

        #region  PageLayoutconrol同步 
        private void syn(AxPageLayoutControl mainlayoutControl)
        {
            IObjectCopy objectcopy = new ObjectCopyClass();
            object tocopymap = mainlayoutControl.ActiveView.GraphicsContainer;   //获取mapcontrol中的map   这个是原始的
            object copiedmap = objectcopy.Copy(tocopymap);       //复制一份map，是一个副本
            object tooverwritemap = PrintPagelayoutControl.ActiveView.GraphicsContainer;    //IActiveView.FocusMap : The map that tools and controls act on. 控件和工具作用的地图，大概是当前地图吧！！！
            objectcopy.Overwrite(copiedmap, ref tooverwritemap);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objectcopy);
            IGraphicsContainer mainGraphCon = tooverwritemap as IGraphicsContainer;
            mainGraphCon.Reset();
            IElement pElement = mainGraphCon.Next();
            IArray pArray = new ArrayClass();
            while (pElement != null)
            {
                pArray.Add(pElement);
                pElement = mainGraphCon.Next();
            }
            int pElementCount = pArray.Count;
            IPageLayout PrintPageLayout = PrintPagelayoutControl.PageLayout;
            IGraphicsContainer PrintGraphCon = PrintPageLayout as IGraphicsContainer;
            PrintGraphCon.Reset();
            IElement pPrintElement = PrintGraphCon.Next();
            while (pPrintElement != null)
            {
                PrintGraphCon.DeleteElement(pPrintElement);
                pPrintElement = PrintGraphCon.Next();
            }
            for (int i = 0; i < pArray.Count; i++)
            {
                PrintGraphCon.AddElement(pArray.get_Element(pElementCount - 1 - i) as IElement, 0);
            }
            PrintPagelayoutControl.Refresh();

        }
        #endregion

        #region 5.8.1打印设置

        private void InitializePageSetupDialog()
        {
            pageSetupDialog1 = new PageSetupDialog();
            //初始化页面设置对话框的页面设置属性为缺省设置
            pageSetupDialog1.PageSettings = new System.Drawing.Printing.PageSettings();
            //初始化页面设置对话框的打印机属性为缺省设置
            pageSetupDialog1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
        }

        private void btnPageSize_Click(object sender, EventArgs e)
        {
            // 实例化打印窗口设置
            DialogResult result = pageSetupDialog1.ShowDialog();
            // 设置打印文档对象的打印机
            document.PrinterSettings = pageSetupDialog1.PrinterSettings;
            // 设置打印文档对象的页面为用户在打印设置对话框中的设置
            document.DefaultPageSettings = pageSetupDialog1.PageSettings;

            // 页面设置
            int i;
            IEnumerator paperSizes = pageSetupDialog1.PrinterSettings.PaperSizes.GetEnumerator();
            paperSizes.Reset();

            for (i = 0; i < pageSetupDialog1.PrinterSettings.PaperSizes.Count; ++i)
            {
                paperSizes.MoveNext();
                if ((paperSizes.Current as PaperSize).Kind == document.DefaultPageSettings.PaperSize.Kind)
                {
                    document.DefaultPageSettings.PaperSize = paperSizes.Current as PaperSize;
                }
            }

            // 初始化纸张和打印机
            IPaper paper = new PaperClass();
            IPrinter printer = new EmfPrinterClass();

            // 关联打印机对象和纸张对象
            paper.Attach(pageSetupDialog1.PrinterSettings.GetHdevmode(pageSetupDialog1.PageSettings).ToInt32(), pageSetupDialog1.PrinterSettings.GetHdevnames().ToInt32());
            printer.Paper = paper;
            PrintPagelayoutControl.Printer = printer;
        }

        #endregion

        #region 5.8.2 打印预览

        private void InitializePrintPreviewDialog()
        {
            printPreviewDialog1 = new PrintPreviewDialog();
            //设置打印预览的尺寸，位置，名称，以及最小尺寸
            printPreviewDialog1.ClientSize = new System.Drawing.Size(800, 600);
            printPreviewDialog1.Location = new System.Drawing.Point(29, 29);
            printPreviewDialog1.Name = "打印预览对话框";
            printPreviewDialog1.MinimumSize = new System.Drawing.Size(375, 250);
            printPreviewDialog1.UseAntiAlias = true;
            this.document.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(document_PrintPage);
        }

        private void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            // 当printPreviewDialog的show方法触发时，引用这段代码
            PrintPagelayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingTile;
            // 获取打印分辨率
            short dpi = (short)e.Graphics.DpiX;
            IEnvelope devBounds = new EnvelopeClass();
            // 获取打印页面
            IPage page = PrintPagelayoutControl.Page;
            // 获取打印页数
            short printPageCount;
            printPageCount = PrintPagelayoutControl.get_PrinterPageCount(0);
            m_CurrentPrintPage++;
            // 选择打印机
            IPrinter printer = PrintPagelayoutControl.Printer;
            // 获取打印机页大小
            page.GetDeviceBounds(printer, m_CurrentPrintPage, 0, dpi, devBounds);
            // 获取页面大小的坐标范围，即四个角的坐标
            tagRECT deviceRect;
            double minX, minY, maxX, maxY;
            devBounds.QueryCoords(out minX, out minY, out maxX, out maxY);
            deviceRect.bottom = (int)maxY;
            deviceRect.left = (int)minX;
            deviceRect.top = (int)minY;
            deviceRect.right = (int)maxX;

            // 确定当前打印页面的大小
            IEnvelope visBounds = new EnvelopeClass();
            page.GetPageBounds(printer, m_CurrentPrintPage, 0, visBounds);
            IntPtr hdc = e.Graphics.GetHdc();
            PrintPagelayoutControl.ActiveView.Output(hdc.ToInt32(), dpi, ref deviceRect, visBounds, m_TrackCancel);
            e.Graphics.ReleaseHdc(hdc);
            if (m_CurrentPrintPage < printPageCount)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }

        }

        private void btnPrintpreview_Click(object sender, EventArgs e)
        {
            // 初始化当前打印页码
            m_CurrentPrintPage = 0;
            document.DocumentName = PrintPagelayoutControl.DocumentFilename;
            printPreviewDialog1.Document = document;
            printPreviewDialog1.Show();
        }

        #endregion

        #region 打印

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // 显示帮助按钮
            printDialog1.ShowHelp = true;
            printDialog1.Document = document;
            // 显示打印窗口
            DialogResult result = printDialog1.ShowDialog();
            // 如果显示成功，则打印
            if (result == DialogResult.OK)
            {
                document.Print();
            }
            Close();
        }


        #endregion

   
    }
}
