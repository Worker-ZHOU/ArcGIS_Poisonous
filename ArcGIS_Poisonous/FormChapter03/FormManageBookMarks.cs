using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;

namespace ArcGIS_Poisonous
{
    public partial class FormManageBookMarks : Form
    {
        private IMap _currentMap = null;

        private Dictionary<string, ISpatialBookmark> pDictionary = new Dictionary<string, ISpatialBookmark>();

        private IMapBookmarks pMapBookmarks = null;

        /// <summary>
        /// 构造初始化函数
        /// </summary>
        /// <param name="map">当前地图的IMap</param>
        public FormManageBookMarks(IMap map)
        {
            InitializeComponent();
            _currentMap = map;
            InitControl();
        }

        /// <summary>
        /// 获取空间书签，对tviewBookmark进行初始化
        /// </summary>
        private void InitControl()
        {
            pMapBookmarks = _currentMap as IMapBookmarks;
            IEnumSpatialBookmark enumSpatialBookmark = pMapBookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark = enumSpatialBookmark.Next();
            string bookmarkName = string.Empty;
            while (spatialBookmark != null) {
                bookmarkName = spatialBookmark.Name;
                tviewBookMark.Nodes.Add(bookmarkName);
                pDictionary.Add(bookmarkName, spatialBookmark);
                spatialBookmark = enumSpatialBookmark.Next();
            }
        }


        /// <summary>
        /// 书签视图定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLocate_Click(object sender, EventArgs e)
        {
            TreeNode selectNode = tviewBookMark.SelectedNode;
            // 获取选中的书签对象
            ISpatialBookmark spatialBookmark = pDictionary[selectNode.Text];
            // 缩放在书签所选中的范围
            spatialBookmark.ZoomTo(_currentMap);
            IActiveView activeView = _currentMap as IActiveView;
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        /// <summary>
        /// 删除书签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            TreeNode selectNode = tviewBookMark.SelectedNode;
            ISpatialBookmark spatialBookmark = pDictionary[selectNode.Text];
            // 删除选中的书签对象
            pMapBookmarks.RemoveBookmark(spatialBookmark);
            // 删除字典中的数据
            pDictionary.Remove(selectNode.Text);
            tviewBookMark.Refresh();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tviewBookMark_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
