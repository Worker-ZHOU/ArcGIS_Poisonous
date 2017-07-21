namespace ArcGIS_Poisonous.FormChapter05
{
    partial class FormCurrency
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
            this.labelControl1 = new System.Windows.Forms.Label();
            this.labelControl2 = new System.Windows.Forms.Label();
            this.cmbSelectedLayer = new System.Windows.Forms.ComboBox();
            this.cmbSelectedField = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSize = true;
            this.labelControl1.Location = new System.Drawing.Point(29, 27);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(65, 12);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "图层选择：";
            // 
            // labelControl2
            // 
            this.labelControl2.AutoSize = true;
            this.labelControl2.Location = new System.Drawing.Point(29, 81);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(65, 12);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "字段选择：";
            // 
            // cmbSelectedLayer
            // 
            this.cmbSelectedLayer.FormattingEnabled = true;
            this.cmbSelectedLayer.Location = new System.Drawing.Point(100, 24);
            this.cmbSelectedLayer.Name = "cmbSelectedLayer";
            this.cmbSelectedLayer.Size = new System.Drawing.Size(213, 20);
            this.cmbSelectedLayer.TabIndex = 2;
            this.cmbSelectedLayer.SelectedIndexChanged += new System.EventHandler(this.cmbSelectedLayer_SelectedIndexChanged);
            // 
            // cmbSelectedField
            // 
            this.cmbSelectedField.FormattingEnabled = true;
            this.cmbSelectedField.Location = new System.Drawing.Point(100, 78);
            this.cmbSelectedField.Name = "cmbSelectedField";
            this.cmbSelectedField.Size = new System.Drawing.Size(213, 20);
            this.cmbSelectedField.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(31, 132);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancle.Location = new System.Drawing.Point(238, 132);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 5;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // FormUniqueValueRender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancle;
            this.ClientSize = new System.Drawing.Size(344, 185);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbSelectedField);
            this.Controls.Add(this.cmbSelectedLayer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUniqueValueRender";
            this.ShowIcon = false;
            this.Text = "图层字段选择通用窗口";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelControl1;
        private System.Windows.Forms.Label labelControl2;
        protected System.Windows.Forms.ComboBox cmbSelectedLayer;
        protected System.Windows.Forms.ComboBox cmbSelectedField;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancle;
    }
}