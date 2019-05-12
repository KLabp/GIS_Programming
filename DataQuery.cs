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
    public delegate void angency_Layer(int index);
    public delegate void angency_Select(IFeatureSelection f_selection);//委托

    public partial class DataQuery : Form
    {
        public event angency_Select OkEvent;
        public event angency_Layer CbLayerEvent;//事件
        public IMap m_map;
        public bool isShow = true;
        string layerName;
        string ownersLayerName;
        ILayer layer;
        IFeatureSelection featureSelect;
        DataOperator dataOperator;
        public DataQuery(IMap map, string name)
        {
            //变量初始化
            m_map = map;
            ownersLayerName = layerName = name;
            featureSelect = null;
            dataOperator = new DataOperator(m_map);
            
            InitializeComponent();
            
            //查询图层选择框
            for (int i = 0; i < m_map.LayerCount; i++)
            {
                string t_layerName = m_map.get_Layer(i).Name;
                cbLayer.Items.Add(t_layerName);
                if (t_layerName == layerName)
                    cbLayer.SelectedIndex = i;
            }//end for，将图层名加到选择列表

            m_Initialize();

            //空间过滤图层选择框
            for (int i = 0; i < m_map.LayerCount; i++)
            {
                cbSLayerList.Items.Add(m_map.get_Layer(i).Name);
            }//end for，将图层名加到选择列表
            cbSLayerList.SelectedIndex = 0;

            //空间关系选择框
            cbSRelationList.SelectedIndex = 0;

        }

        private void m_Initialize()
        {
            layer = dataOperator.GetLayerByName(layerName);
            //标题
            this.Text = "查询" + layerName;
            //空间关系图层复选框初始化
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            clbFields.Items.Clear();
            for (int i = 0; i < featureClass.Fields.FieldCount; i++)
            {
                clbFields.Items.Add(featureClass.Fields.get_Field(i).Name);
            }//end for，将字段名添加到复选框
        }

        private void cbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            layerName = cbLayer.SelectedItem.ToString();
            m_Initialize();
            if (CbLayerEvent != null)
            {
                CbLayerEvent(cbLayer.SelectedIndex);
            }
        }

        private void clbFields_MouseClick(object sender, MouseEventArgs e)
        {
            if (ckbAll.CheckState == CheckState.Checked)
            {
                ckbAll.CheckState = CheckState.Indeterminate;
            }//全选状态下，取消任意一个勾选，全选状态变为“Indeterminate”,并取消勾选
        }

        private void ckbAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (ckbAll.CheckState == CheckState.Checked)
            {
                for (int i = 0; i < clbFields.Items.Count; i++)
                {
                    clbFields.SetItemChecked(i, true);
                }
            }
            else if (ckbAll.CheckState == CheckState.Unchecked)
            {
                for (int i = 0; i < clbFields.Items.Count; i++)
                {
                    clbFields.SetItemChecked(i, false);
                }
            }
            else
            {
                //ckbAll.Checked = false;
            }
            
        }

        /* function GetSpatialFilter
         * Usage: gather up the input to create an query-filter
         * Error Return value: null
         * Author: JL Ding
         * Time: 2019/05/08
         */
        private ISpatialFilter GetSpatialFilter()
        {
            ISpatialFilter spatailFilter = new SpatialFilter();
            
            //获取几何
            IGeometry geom;
            IFeature feature;
            IFeatureCursor featureCursor = null;
            IQueryFilter queryFilter = new QueryFilter();
            if (tbSFRelation.Text != "(SQL)")
                queryFilter.WhereClause = tbSFRelation.Text;//获取空间查询属性关系
            else
            {
                queryFilter.WhereClause = null;
                return spatailFilter;
            }//不进行空间查询
            IFeatureLayer featureLayer = (IFeatureLayer) dataOperator.GetLayerByName(cbSLayerList.SelectedItem.ToString());//获取空间查询图层
            try
            {
                featureCursor = featureLayer.FeatureClass.Search(queryFilter, false);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("ERROR：空间查询中使用了非法的SQL语句！");
                return null;
            }//避免非法SQL引起的程序卡死
            
            feature = featureCursor.NextFeature();
            geom = feature.Shape;//得到最终的图形几何
            //空间过滤
            spatailFilter.Geometry = geom;
            spatailFilter.SpatialRel = (ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum)(cbSRelationList.SelectedIndex + 1); 
            
            return spatailFilter;
        }


        /* function GetQueryFilter
         * Usage: gather up the input to create an query-filter
         * Error Return value: null
         * Author: JL Ding
         * Time: 2019/05/08
         */
        private ISpatialFilter GetQueryFilter()
        {
            ISpatialFilter filter = GetSpatialFilter(); //先添加空间属性
            if (filter == null)
            {
                return null;
            }//如果filter==null，说明空间查询存在错误，

            //添加查询子字段
            string MySubfields = "";
            for (int i = 0; i < clbFields.Items.Count; i++)
            {
                if (clbFields.GetItemChecked(i))
                {
                    if (MySubfields != "")
                        MySubfields += ",";
                    MySubfields += clbFields.GetItemText(clbFields.Items[i]);
                    //filter.AddField(clbFields.GetItemText(clbFields.Items[i]));
                    //不知道为什么，AddField并没有用
                }
            }
            if (MySubfields != "")
                filter.SubFields = MySubfields;
            else
            {
                MessageBox.Show("请选择至少一个查询子字段");
                return null;
            }

            //添加属性关系
            if (tbFRelation.Text != "(SQL)")
                filter.WhereClause = tbFRelation.Text;
            else
                filter.WhereClause = null;
            try
            {
                IFeatureCursor featureCursor = null;
                IFeatureLayer featureLayer = (IFeatureLayer)layer;
                featureCursor = featureLayer.FeatureClass.Search(filter, false);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("ERROR：属性查询中使用了非法的SQL语句！");
                return null;
            }//避免非法SQL引起的程序卡死    

            return filter;
        }

        private void Refresh_Table(string m_layerName, ISpatialFilter filter)
        {
            //获取结果表
            DataTable queryTable = dataOperator.GetLayerDataTable(m_layerName, filter);

            //刷新父窗口: https://www.cnblogs.com/bluewhy/p/5276323.html
            DataBoard dataBoard = (DataBoard)this.Owner;
            dataBoard.TableRefresh_Method(queryTable);

            //更新父窗口的选择集
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            ISpatialFilter filter=this.GetQueryFilter();

            if (filter != null)
            {
                //实例化选中对象
                IFeatureLayer featureLayer = (IFeatureLayer)layer;
                
                featureSelect = (IFeatureSelection)featureLayer;
                featureSelect.SelectFeatures(filter, esriSelectionResultEnum.esriSelectionResultNew, false);
                Refresh_Table(layerName, filter);
                this.DialogResult = DialogResult.OK;
            } //空间查询和属性查询都正确，才进一步获取结果
            //this.Close();
            if (OkEvent != null)
            {
                OkEvent(featureSelect);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            //return DialogResult.Cancel;
        }

        private void DataQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Refresh_Table(ownersLayerName, null);
            featureSelect.Clear();
            isShow = false;
        }

        

        

        
    }
}
