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

        //when the selected layer was changed, dispaly the datatable of the newly selected layer
        private void cbLayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataOperator dataOperator = new DataOperator(m_map);
            dataGridView1.DataSource = dataOperator.GetLayerDataTable(cbLayerList.SelectedItem.ToString());
        }
    }
}
