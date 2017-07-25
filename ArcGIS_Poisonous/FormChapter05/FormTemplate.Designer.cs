namespace ArcGIS_Poisonous.FormChapter05
{
    partial class FormTemplate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTemplate));
            this.tlstTemplate = new System.Windows.Forms.TreeView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pageLayoutCtrlMxt = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axLicenseControl1 = new AxESRI.ArcGIS.Controls.AxLicenseControl();
            ((System.ComponentModel.ISupportInitialize)(this.pageLayoutCtrlMxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // tlstTemplate
            // 
            this.tlstTemplate.Dock = System.Windows.Forms.DockStyle.Left;
            this.tlstTemplate.Location = new System.Drawing.Point(0, 0);
            this.tlstTemplate.Name = "tlstTemplate";
            this.tlstTemplate.Size = new System.Drawing.Size(133, 388);
            this.tlstTemplate.TabIndex = 1;
            this.tlstTemplate.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tlstTemplate_NodeMouseClick);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(190, 353);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "应用";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(316, 353);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pageLayoutCtrlMxt
            // 
            this.pageLayoutCtrlMxt.Location = new System.Drawing.Point(139, 0);
            this.pageLayoutCtrlMxt.Name = "pageLayoutCtrlMxt";
            this.pageLayoutCtrlMxt.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("pageLayoutCtrlMxt.OcxState")));
            this.pageLayoutCtrlMxt.Size = new System.Drawing.Size(303, 347);
            this.pageLayoutCtrlMxt.TabIndex = 0;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(410, 356);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 4;
            // 
            // FormTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 388);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tlstTemplate);
            this.Controls.Add(this.pageLayoutCtrlMxt);
            this.Name = "FormTemplate";
            this.Text = "模板预览";
            ((System.ComponentModel.ISupportInitialize)(this.pageLayoutCtrlMxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxPageLayoutControl pageLayoutCtrlMxt;
        private System.Windows.Forms.TreeView tlstTemplate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private AxESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
    }
}