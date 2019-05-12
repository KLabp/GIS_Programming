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
    public partial class AddFeatureToLayer : Form
    {
        public IMap m_map;
        public ILayer layer;
        public IGeometry m_geometry;
        public String name;
        public AddFeatureToLayer(IMap map, IGeometry geometry)
        {
            InitializeComponent();
            m_map = map;
            m_geometry = geometry;
            
            for (int i = 0; i < m_map.LayerCount; i++)
            {
                //这段存疑,空shapefile不知道应该怎么处理
                //使用IFeatureClass接口，所得到的FeatureType是esriFeatureType——esriFTSimple，而不是esriGeometryType
                //ILayer layer2 = m_map.get_Layer(i);
                //IFeatureLayer featureLayer = layer2 as IFeatureLayer;
                //IFeatureClass featureClass = featureLayer.FeatureClass;
                //if (featureClass.FeatureDataset == null) //如果是空图层，跳过(这就是空shapefile的问题，如果直接getFeature会越界，但是这个shapefile确实是需要考虑的)
                //    continue;
                //IFeature feature = featureClass.GetFeature(0);
   
                //if(geometry.GeometryType==feature.Shape.GeometryType)
                    cbLayerList.Items.Add(m_map.get_Layer(i).Name);  //只有类型相同的图层才会被加入combobox
            }
            if (cbLayerList.Items.Count == 0)
            {
                MessageBox.Show("没有与要素类型匹配的图层，请从新选择要素类型或者添加相应的图层");
                return;
            }
            else
                cbLayerList.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            DataOperator dataOperator = new DataOperator(m_map);
            //IPoint point = new PointClass();
            bool SuccessAdd = dataOperator.AddFeatureToLayer(cbLayerList.SelectedItem.ToString(), tbName.Text, m_geometry);
            if (!SuccessAdd)
            {
                MessageBox.Show("添加要素失败！");
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            //return;
        }
    }
}
