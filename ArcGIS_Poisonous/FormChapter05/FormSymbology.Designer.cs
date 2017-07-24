namespace ArcGIS_Poisonous.FormChapter05
{
    partial class FormSymbology
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSymbology));
            this.SymbologyCtrl = new ESRI.ArcGIS.Controls.AxSymbologyControl();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.axLicenseControl1 = new AxESRI.ArcGIS.Controls.AxLicenseControl();
            ((System.ComponentModel.ISupportInitialize)(this.SymbologyCtrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // SymbologyCtrl
            // 
            this.SymbologyCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SymbologyCtrl.Location = new System.Drawing.Point(0, 0);
            this.SymbologyCtrl.Name = "SymbologyCtrl";
            this.SymbologyCtrl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SymbologyCtrl.OcxState")));
            this.SymbologyCtrl.Size = new System.Drawing.Size(390, 399);
            this.SymbologyCtrl.TabIndex = 0;
            this.SymbologyCtrl.OnMouseDown += new ESRI.ArcGIS.Controls.ISymbologyControlEvents_Ax_OnMouseDownEventHandler(this.SymbologyCtrl_OnMouseDown);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(104, 373);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(212, 373);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(358, 0);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 3;
            // 
            // FormSymbology
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 399);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.SymbologyCtrl);
            this.Name = "FormSymbology";
            this.Text = "符号选择器";
            ((System.ComponentModel.ISupportInitialize)(this.SymbologyCtrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxSymbologyControl SymbologyCtrl;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private AxESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
    }
}