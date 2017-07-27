using ArcGIS_Poisonous.Chapter07;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArcGIS_Poisonous
{
    /// <summary>
    /// Chapter07 矢量数据空间分析
    /// </summary>
    public partial class ArcGisPoisonous : Form
    {

        #region 7.1.2 缓冲区分析

        private void btnBufferAnalysis_Click(object sender, EventArgs e)
        {
            ICommand command = new ToolBufferAnalysis();
            command.OnCreate(MainMapControl.Object);
            MainMapControl.CurrentTool = command as ITool;
            command = null;
        }

        #endregion

        #region 7.1.3 获取多边形要素边界

        private void btnGetFeatureBoundary_Click(object sender, EventArgs e)
        {
            ICommand command = new ToolGetBoundary();
            command.OnCreate(MainMapControl.Object);
            MainMapControl.CurrentTool = command as ITool;
            command = null;
        }

        #endregion

        #region 7.2.2 查找一多边形要素的所有邻接要素

        private void btnQueryNearFeature_Click(object sender, EventArgs e)
        {
            ToolGetNearFeature toolGetNearFeature = new ToolGetNearFeature();
            toolGetNearFeature.OnCreate(MainMapControl.Object);
            MainMapControl.CurrentTool = toolGetNearFeature as ITool;
            toolGetNearFeature = null;
        }

        #endregion

    }
}
