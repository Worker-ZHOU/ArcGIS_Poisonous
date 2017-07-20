using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArcGIS_Poisonous.FormChapter05
{
    public partial class Form1 : FormCurrency
    {

        public delegate void EventHandle(string featureClassName, string fieldName);

        public event EventHandle Rander = null;

        public Form1()
        {
            InitializeComponent();
        }

        public override void InitUI(){
            base.InitUI();
            cmbSelectedField.Items.Clear();
            cmbSelectedField.Items.Add("Black");
            cmbSelectedField.Items.Add("Red");
            cmbSelectedField.Items.Add("Green");
            cmbSelectedField.Items.Add("IndianRed");
            cmbSelectedField.Items.Add("LightBlue");
        }

        protected override void  cmbSelectedLayer_SelectedIndexChanged(object sender, EventArgs e)
        {

            // FixMe

        }

    }
}
