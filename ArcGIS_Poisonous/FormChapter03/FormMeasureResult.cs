using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArcGIS_Poisonous
{
    public partial class FormMeasureResult : Form
    {
        // 声明运行结果时间关闭
        public delegate void FormClosedEventHandler();
        public event FormClosedEventHandler frmClosed = null;

        public FormMeasureResult()
        {
            InitializeComponent();
        }

        private void FormMeasureResult_FormClosed(object sender, EventArgs e)
        {
            if (frmClosed != null)
            {
                frmClosed();
            }
        }
    }
}
