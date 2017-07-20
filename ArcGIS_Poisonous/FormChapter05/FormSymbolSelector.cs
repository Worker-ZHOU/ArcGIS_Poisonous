using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace ArcGIS_Poisonous
{
    public partial class FormSymbolSelector : Form
    {
        private IStyleGalleryItem mStyleGalleryItem;
        private ILegendClass mLegendClass;
        private ILayer mLayer;
        private ISymbol mSymbol;
        public Image mSymbolImage;
        string mFilepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        // OperateMap mOperateMap = new OperateMap();
        bool mContextMenuMoreSymbolInitiated = false;

        public ISymbol Symbol {
            get {
                return mSymbol;
            }
            set {
                mSymbol = value;
            }
        }

        public FormSymbolSelector(ILegendClass legendClass, ILayer layer)
        {
            InitializeComponent();

            this.mLegendClass = legendClass;
            this.mLayer = layer;
        }

        private void FormSymbolSelector_Load(object sender, EventArgs e)
        {

        }

        #region 取消

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
