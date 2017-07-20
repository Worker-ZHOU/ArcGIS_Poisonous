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
    public partial class FormBookmark : Form
    {

        // 书签名称
        private string m_bookmark; 
        // 是否创建书签
        private int m_check;

        // 只读的书签名称
        public string BookMark 
        {
            get 
            {
                return m_bookmark;
            }
        }

        // 是否创建书签变量
        public bool Check {
            get {
                return m_check == 1;
            }        
        }

        public FormBookmark()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            m_bookmark = txtBookmark.Text;
            txtBookmark.Text = "";
            m_check = 1;
            this.Close();
        }

        /// <summary>
        ///  取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtBookmark.Text = "";
            m_check = 0;
            this.Close();
        }

        /// <summary>
        /// 输入框变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBookmark_TextChanged(object sender, EventArgs e)
        {
            if (txtBookmark.Text == "")
            {
                btnOk.Enabled = false;
            }
            else
            {
                btnOk.Enabled = true;
            }
        }
    }
}
