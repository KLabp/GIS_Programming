using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIS_Programming
{
    public partial class AdmitBookmarkName : Form
    {
        public MainForm m_frmMain;

        public AdmitBookmarkName(MainForm frm)
        {
            InitializeComponent();
            m_frmMain = frm;
        }

        private bool Check_name_unique(String newname)
        {
            int i = 0;
            for (i = 0; i < m_frmMain.cbBookmarkList.Items.Count; i++)
            {
                if (newname == m_frmMain.cbBookmarkList.Items[i].ToString())
                    break;
            }
            if (i == m_frmMain.cbBookmarkList.Items.Count)
                return true;
            else
                return false;
        }

        private void btnName_Click(object sender, EventArgs e)
        {


            if (m_frmMain != null && tbBookmarkName.Text != "")
            {
                if (!Check_name_unique(tbBookmarkName.Text))
                {
                    MessageBox.Show("书签名称不能重复", "重复的书签", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    tbBookmarkName.Text = "";
                }
                else
                {
                    m_frmMain.CreateBookmark(tbBookmarkName.Text);
                    this.Close();
                }
            }

        }
    }
}
