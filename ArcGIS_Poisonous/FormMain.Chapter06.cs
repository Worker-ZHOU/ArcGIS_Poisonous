using ArcGIS_Poisonous.Tools;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArcGIS_Poisonous
{

    /// <summary>
    /// Chapter06
    /// </summary>
    public partial class ArcGisPoisonous : Form
    {

        private IMap mMap = null;
        private IActiveView mActiveView = null;
        private List<ILayer> mlistLayers = null;
        private IFeatureLayer mCurrentLyr = null;
        private IEngineEditor mEngineEditor = null;
        private IEngineEditTask mEngineEditTask = null;
        private IEngineEditLayers mEngineEditLayers = null;

        #region 开始编辑

        /// <summary>
        /// 开始编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartEdit_Click(object sender, EventArgs e)
        {
            try
            {
                mEngineEditor = new EngineEditorClass();
                mEngineEditTask = mEngineEditor as IEngineEditTask;
                mEngineEditLayers = mEngineEditor as IEngineEditLayers;
                mMap = MainMapControl.Map;
                mActiveView = mMap as IActiveView;
                mlistLayers = MapOperation.GetLayers(mMap);

                if (mlistLayers == null || mlistLayers.Count == 0)
                {
                    MessageBox.Show("请加载编辑图层！", "提示",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                mMap.ClearSelection();
                mActiveView.Refresh();
                InitComboBox(mlistLayers);
                ChangeButtonState(true);

                // 如果编辑已经开始，则直接退出
                if (mEngineEditor.EditState != esriEngineEditState.esriEngineStateNotEditing)
                    return;
                if (mCurrentLyr == null) return;
                // 获取当前编辑图层工作空间
                IDataset dataset = mCurrentLyr.FeatureClass as IDataset;
                IWorkspace workspace = dataset.Workspace;
                // 设置编辑模式，如果是ArcSDE采用版本模式
                if (workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    mEngineEditor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeVersioned;
                }
                else
                {
                    mEngineEditor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeNonVersioned;
                }
                // 设置编辑任务
                mEngineEditTask = mEngineEditor.GetTaskByUniqueName("ControlToolsEditing_CreateNewFeatureTask");
                mEngineEditor.CurrentTask = mEngineEditTask;
                //是否可以进行撤销、恢复操作
                mEngineEditor.EnableUndoRedo(true);
                //开始编辑操作
                mEngineEditor.StartEditing(workspace, mMap);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        /// <summary>
        /// 保存编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 退出编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndEdit_Click(object sender, EventArgs e)
        {

        }

        #region 设置编辑图层

        /// <summary>
        /// 设置编辑图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSelLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sLyrName = cmbSelLayer.SelectedItem.ToString();
                mCurrentLyr = MapOperation.GetLayerByName(mMap, sLyrName) as IFeatureLayer;
                mEngineEditLayers.SetTargetLayer(mCurrentLyr, 0);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region 选择要素

        private void btnSelFeat_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ICommand m_SelTool = new SelectFeatureToolClass();
            //    m_SelTool.OnCreate(MainMapControl.Object);
            //    m_SelTool.OnClick();
            //    MainMapControl.CurrentTool = m_SelTool as ITool;
            //    MainMapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
            //}
            //catch (Exception ex)
            //{
            //}
        }

        #endregion



        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveVertex_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///  添加节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddVertex_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelVertex_Click(object sender, EventArgs e)
        {

        }

        #region 设置按钮可点击状态

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="enable"></param>
        partial void ChangeButtonState(bool enable)
        {
            btnStartEdit.Enabled = !enable;
            btnSaveEdit.Enabled = enable;
            btnEndEdit.Enabled = enable;

            cmbSelLayer.Enabled = enable;

            btnSelFeat.Enabled = enable;
            btnSelMove.Enabled = enable;
            btnAddFeature.Enabled = enable;
            btnDelFeature.Enabled = enable;
            btnAttributeEdit.Enabled = enable;
            btnUndo.Enabled = enable;
            btnRedo.Enabled = enable;
            btnMoveVertex.Enabled = enable;
            btnAddVertex.Enabled = enable;
            btnDelVertex.Enabled = enable;
        }

        #endregion

        #region 设置可选图层信息

        /// <summary>
        /// 设置图层选项信息
        /// </summary>
        /// <param name="plstLyr"></param>
        private void InitComboBox(List<ILayer> plstLyr)
        {
            cmbSelLayer.Items.Clear();
            for (int i = 0; i < plstLyr.Count; i++)
            {
                if (!cmbSelLayer.Items.Contains(plstLyr[i].Name))
                {
                    cmbSelLayer.Items.Add(plstLyr[i].Name);
                }
            }
            if (cmbSelLayer.Items.Count != 0) cmbSelLayer.SelectedIndex = 0;
        }

        #endregion
    }
}
