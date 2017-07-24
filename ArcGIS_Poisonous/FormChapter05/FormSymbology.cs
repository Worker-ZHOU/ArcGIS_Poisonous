using System;
using System.Windows.Forms;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace ArcGIS_Poisonous.FormChapter05
{
    public partial class FormSymbology : Form
    {
        public ISymbol mSymbol;

        private IStyleGalleryItem mStyleGalleryItem;

        private ISymbologyStyleClass mSymbologyStyleClass;

        public delegate void EventHandler(ref IStyleGalleryItem styleGalleryItem);
        public EventHandler Render = null;

        private string mFilepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        private ArcGisPoisonous.EnumMapSurroundType mEnumMapSurroundType = ArcGisPoisonous.EnumMapSurroundType.None;

        private string mProjectName = "ArcGIS_Poisonous";

        public ArcGisPoisonous.EnumMapSurroundType EnumMapSurType
        {
            get {
                return mEnumMapSurroundType;
            }
            set
            {
                mEnumMapSurroundType = value;
            }
        }

        public FormSymbology()
        {
            InitializeComponent();
        }

        private  string getPath(string path)
        {
            int t;
            for (t = 0; t < path.Length - mProjectName.Length; t++)
            {

                if (path.Substring(t, mProjectName.Length) == mProjectName)
                {
                    break;
                }
            }
            string name = path.Substring(0, t + mProjectName.Length);
            return name;
        }

        public void InitUI()
        {
            SymbologyCtrl.Clear();

            // 载入系统符号库
            string styleFilePath = getPath(mFilepath) + "\\data\\Symbol\\ESRI.ServerStyle";

            Console.WriteLine("filepath:" + mFilepath);
            Console.WriteLine("StyleFilePath:" + styleFilePath);

            SymbologyCtrl.LoadStyleFile(styleFilePath);
            switch (mEnumMapSurroundType)
            {
                case ArcGisPoisonous.EnumMapSurroundType.NorthArrow:
                    SymbologyCtrl.StyleClass = esriSymbologyStyleClass.esriStyleClassNorthArrows;
                    mSymbologyStyleClass = SymbologyCtrl.GetStyleClass(esriSymbologyStyleClass.esriStyleClassNorthArrows);
                    break;
                case ArcGisPoisonous.EnumMapSurroundType.ScaleBar:
                    SymbologyCtrl.StyleClass = esriSymbologyStyleClass.esriStyleClassScaleBars;
                    mSymbologyStyleClass = SymbologyCtrl.GetStyleClass(esriSymbologyStyleClass.esriStyleClassScaleBars);
                    break;      
            }
            mSymbologyStyleClass.UnselectItem();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            Render(ref mStyleGalleryItem);//传递用户选择的值
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Render(ref mStyleGalleryItem);//传递用户选择的值
            Close();
        }

        private void SymbologyCtrl_OnMouseDown(object sender, ISymbologyControlEvents_OnMouseDownEvent e)
        {       
            mStyleGalleryItem = SymbologyCtrl.HitTest(e.x, e.y);
        }
    }
}
