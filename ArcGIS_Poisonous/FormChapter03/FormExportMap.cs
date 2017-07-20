using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;


namespace ArcGIS_Poisonous
{
    public partial class FormExportMap : Form
    {
        private string SavePath = "";

        private IActiveView ActiveView;

        private IGeometry Geometry;

        /// <summary>
        /// 只读属性，地图导出空间图形   
        /// </summary>
        public IGeometry GetGometry {
            set 
            {
                Geometry = value;
            }
        }

        private bool Region = true;

        public bool IsRegion 
        {
            set
            {
                Region = value;
            }
        }


        public FormExportMap(AxMapControl axMapControl)
        {
            InitializeComponent();
            ActiveView = axMapControl.ActiveView;
        }

        private void FormExportMap_Load(object sender, EventArgs e)
        {
            InitFormSize();
        }

        private void InitFormSize()
        {
            // 读取当前窗口分辨率并将其放入cboResolution中
            cboResolution.Text = ActiveView.ScreenDisplay.DisplayTransformation.Resolution.ToString();
            cboResolution.Items.Add(cboResolution.Text);
            if (Region)
            {
                IEnvelope pEnvelope = Geometry.Envelope;
                tagRECT pRECT = new tagRECT();
                ActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvelope, ref pRECT, 9);
                if (cboResolution.Text != "")
                {
                    txtWidth.Text = pRECT.right.ToString();
                    txtHeight.Text = pRECT.bottom.ToString();
                }
            }
            else
            {
                if (cboResolution.Text != "")
                {
                    txtWidth.Text = ActiveView.ExportFrame.right.ToString();
                    txtHeight.Text = ActiveView.ExportFrame.bottom.ToString();
                }
            }

        }

        private void cboResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            double num = (int)Math.Round(ActiveView.ScreenDisplay.DisplayTransformation.Resolution);
            if (cboResolution.Text == "")
            {
                txtWidth.Text = "";
                txtHeight.Text = "";
                return;
            }
            if (Region)
            {
                IEnvelope pEnvelope = Geometry.Envelope;
                tagRECT pRECT = new tagRECT();
                ActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvelope, ref pRECT, 9);
                if (cboResolution.Text != "")
                {
                    txtWidth.Text = Math.Round((double)(pRECT.right * (double.Parse(cboResolution.Text) / (double)num))).ToString();
                    txtHeight.Text = Math.Round((double)(pRECT.bottom * (double.Parse(cboResolution.Text) / (double)num))).ToString();
                }
            }
            else
            {
                txtWidth.Text = Math.Round((double)(ActiveView.ExportFrame.right * (double.Parse(cboResolution.Text) / (double)num))).ToString();
                txtHeight.Text = Math.Round((double)(ActiveView.ExportFrame.bottom * (double.Parse(cboResolution.Text) / (double)num))).ToString();
            }
        }

        private void btnExPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfdExportMap = new SaveFileDialog();
            sfdExportMap.DefaultExt = "jpg|bmp|gig|tif|png|pdf";
            sfdExportMap.Filter = "JPGE 文件(*.jpg)|*.jpg|BMP 文件(*.bmp)|*.bmp|GIF 文件(*.gif)|*.gif|TIF 文件(*.tif)|*.tif|PNG 文件(*.png)|*.png|PDF 文件(*.pdf)|*.pdf";
            sfdExportMap.OverwritePrompt = true;
            sfdExportMap.Title = "保存为";
            txtExPath.Text = "";
            if (sfdExportMap.ShowDialog() != DialogResult.Cancel)
            {
                SavePath = sfdExportMap.FileName;
                txtExPath.Text = sfdExportMap.FileName;
            }  
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (txtExPath.Text == "")
            {
                MessageBox.Show("请先确定导出路径!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (cboResolution.Text == "")
            {
                if (txtExPath.Text == "")
                {
                    MessageBox.Show("请输入分辨率！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (Convert.ToInt16(cboResolution.Text) == 0)
            {
                MessageBox.Show("请正确输入分辨率！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else 
            {
                try
                {
                    int resolution = int.Parse(cboResolution.Text);
                    int width = int.Parse(txtWidth.Text);
                    int height = int.Parse(txtHeight.Text);
                    ExportMap.ExportView(ActiveView, Geometry,resolution,width,height,SavePath,Region);
                    ActiveView.GraphicsContainer.DeleteAllElements();
                    ActiveView.Refresh();

                    MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("导出失败","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }

        private void FormExportMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            //局部导出时没有导出图像就关闭
            ActiveView.GraphicsContainer.DeleteAllElements();
            ActiveView.Refresh();
            Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //局部导出时没有导出图像就关闭
            ActiveView.GraphicsContainer.DeleteAllElements();
            ActiveView.Refresh();
            Dispose();
        }
    }
}
