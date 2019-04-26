using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Geodatabase;

namespace GIS_Programming
{
    public partial class NewFeatureClass : Form
    {
        public MainForm m_frmMain;
        public IMap m_map;
        public IFeatureClass featureClass;
        public String m_Name;
       
        public NewFeatureClass(MainForm frm, IMap map)
        {
            InitializeComponent();
            m_frmMain = frm;
            m_map = map;

            cbType.Items.Add("Point");
            cbType.Items.Add("Line");
            cbType.Items.Add("Polygon");
            cbType.SelectedIndex = 0;
            tbName.Text = "输入要素集名称";
        }

        
        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)

                tbPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
            DataOperator dataoperator = new DataOperator(m_map);
            m_Name = tbName.Text;
            featureClass = dataoperator.CreateShapefile(tbPath.Text, m_Name, cbType.SelectedItem.ToString());
            if (featureClass == null)
            {
                MessageBox.Show("创建Shapefile失败！");
                return;
            }
            else
            {
                bool bRes = dataoperator.AddFeatureClassToMap(featureClass, m_Name);
                if (bRes)
                    this.DialogResult = DialogResult.OK;
                else
                    MessageBox.Show("将新建的Shapefile加入地图失败！");
                this.Close();
            }
            //return;
        }

        

        
    }
}
