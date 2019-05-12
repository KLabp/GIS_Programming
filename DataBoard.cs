using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;

namespace GIS_Programming
{
    public partial class DataBoard : Form
    {
        public IMap m_map;
        IFeatureSelection featureSelect;
        public DataBoard(IMap map, DataTable dataTable)
        {
            InitializeComponent();
            m_map = map;
            for (int i = 0; i < m_map.LayerCount; i++)
            {
                cbLayerList.Items.Add(m_map.get_Layer(i).Name);
            }
            cbLayerList.SelectedIndex = 0;
            //tbDataName.Text = sDataName;
            dataGridView1.DataSource = dataTable;
        }

        public void FeatureSelection_Method(ISpatialFilter filter)
        {
            featureSelect.SelectFeatures(filter, esriSelectionResultEnum.esriSelectionResultNew, false);
        }

        private DataTable Get_Table()
        {
            DataOperator dataOperator = new DataOperator(m_map);
            DataTable table = dataOperator.GetLayerDataTable(cbLayerList.SelectedItem.ToString(), null);//不使用queryFilter进行查询
            return table;
        }
        public void TableRefresh_Method(DataTable freshDataTable)
        {
            dataGridView1.DataSource = freshDataTable;
            lbResult.Text = "结果：" + freshDataTable.Rows.Count.ToString() + "（行）";
        }

        //when the selected layer was changed, dispaly the datatable of the newly selected layer
        private void cbLayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            TableRefresh_Method(Get_Table());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            TableRefresh_Method(Get_Table());
            featureSelect.Clear();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataQuery frm_DQ = new DataQuery(m_map, cbLayerList.SelectedItem.ToString());
            frm_DQ.Owner = this;//将查询窗口的所有者设置为本父窗口
            frm_DQ.OkEvent += new angency_Select(frm_DQ_OkEvent);
            frm_DQ.CbLayerEvent += new angency_Layer(frm_DQ_CbLayerEvent);
            frm_DQ.Show(this);
        }
        void frm_DQ_OkEvent(IFeatureSelection f_selection)
        {
            featureSelect = f_selection;
        }
        void frm_DQ_CbLayerEvent(int index)
        {
            cbLayerList.SelectedIndex = index;
        }
 
    }
}
