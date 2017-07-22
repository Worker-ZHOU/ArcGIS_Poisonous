namespace ArcGIS_Poisonous.FormChapter05
{
    partial class FormCurrency3
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSelLayer = new System.Windows.Forms.ComboBox();
            this.groupControl1 = new System.Windows.Forms.GroupBox();
            this.lstboxField = new System.Windows.Forms.ListBox();
            this.btnAddOne = new System.Windows.Forms.Button();
            this.btnDelOne = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnDelAll = new System.Windows.Forms.Button();
            this.groupControl2 = new System.Windows.Forms.GroupBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.fieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fieldAlaisName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupControl1.SuspendLayout();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图层选择：";
            // 
            // cmbSelLayer
            // 
            this.cmbSelLayer.FormattingEnabled = true;
            this.cmbSelLayer.Location = new System.Drawing.Point(100, 26);
            this.cmbSelLayer.Name = "cmbSelLayer";
            this.cmbSelLayer.Size = new System.Drawing.Size(350, 20);
            this.cmbSelLayer.TabIndex = 1;
            this.cmbSelLayer.SelectedIndexChanged += new System.EventHandler(this.cmbSelLayer_SelectedIndexChanged);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lstboxField);
            this.groupControl1.Location = new System.Drawing.Point(1, 66);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(108, 245);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.TabStop = false;
            this.groupControl1.Text = "字段选择";
            // 
            // lstboxField
            // 
            this.lstboxField.FormattingEnabled = true;
            this.lstboxField.ItemHeight = 12;
            this.lstboxField.Location = new System.Drawing.Point(0, 31);
            this.lstboxField.Name = "lstboxField";
            this.lstboxField.Size = new System.Drawing.Size(108, 172);
            this.lstboxField.TabIndex = 0;
            // 
            // btnAddOne
            // 
            this.btnAddOne.Location = new System.Drawing.Point(125, 80);
            this.btnAddOne.Name = "btnAddOne";
            this.btnAddOne.Size = new System.Drawing.Size(50, 23);
            this.btnAddOne.TabIndex = 3;
            this.btnAddOne.Text = ">";
            this.btnAddOne.UseVisualStyleBackColor = true;
            this.btnAddOne.Click += new System.EventHandler(this.btnAddOne_Click);
            // 
            // btnDelOne
            // 
            this.btnDelOne.Location = new System.Drawing.Point(125, 131);
            this.btnDelOne.Name = "btnDelOne";
            this.btnDelOne.Size = new System.Drawing.Size(50, 23);
            this.btnDelOne.TabIndex = 4;
            this.btnDelOne.Text = "<";
            this.btnDelOne.UseVisualStyleBackColor = true;
            this.btnDelOne.Click += new System.EventHandler(this.btnDelOne_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(125, 186);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(50, 23);
            this.btnAddAll.TabIndex = 5;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnDelAll
            // 
            this.btnDelAll.Location = new System.Drawing.Point(125, 234);
            this.btnDelAll.Name = "btnDelAll";
            this.btnDelAll.Size = new System.Drawing.Size(50, 23);
            this.btnDelAll.TabIndex = 6;
            this.btnDelAll.Text = "<<";
            this.btnDelAll.UseVisualStyleBackColor = true;
            this.btnDelAll.Click += new System.EventHandler(this.btnDelAll_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.dataGridView);
            this.groupControl2.Location = new System.Drawing.Point(192, 66);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(354, 199);
            this.groupControl2.TabIndex = 7;
            this.groupControl2.TabStop = false;
            this.groupControl2.Text = "符号设置";
            // 
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fieldName,
            this.fieldAlaisName,
            this.Column3});
            this.dataGridView.Location = new System.Drawing.Point(6, 14);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(345, 185);
            this.dataGridView.TabIndex = 0;
            // 
            // fieldName
            // 
            this.fieldName.HeaderText = "字段名称";
            this.fieldName.Name = "fieldName";
            // 
            // fieldAlaisName
            // 
            this.fieldAlaisName.HeaderText = "字段别名";
            this.fieldAlaisName.Name = "fieldAlaisName";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "颜色";
            this.Column3.Items.AddRange(new object[] {
            "Blue",
            "Red",
            "Green",
            "Yellow",
            "Brown"});
            this.Column3.Name = "Column3";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(238, 271);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(401, 271);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormCurrency3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 299);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.btnDelAll);
            this.Controls.Add(this.btnAddAll);
            this.Controls.Add(this.btnDelOne);
            this.Controls.Add(this.btnAddOne);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.cmbSelLayer);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCurrency3";
            this.ShowIcon = false;
            this.Text = "统计图表符号化";
            this.groupControl1.ResumeLayout(false);
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSelLayer;
        private System.Windows.Forms.GroupBox groupControl1;
        private System.Windows.Forms.ListBox lstboxField;
        private System.Windows.Forms.Button btnAddOne;
        private System.Windows.Forms.Button btnDelOne;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnDelAll;
        private System.Windows.Forms.GroupBox groupControl2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldAlaisName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column3;
    }
}