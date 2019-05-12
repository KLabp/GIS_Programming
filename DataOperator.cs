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
using ESRI.ArcGIS.DataSourcesFile;

//It's a class
namespace GIS_Programming
{
    
    class DataOperator
    {
        public IMap m_map;
        /* function DataOperator
         * Usage: structure function, when the class DataOperator was instancing, 
         *        the function will transit map into m_map to save current map data.
         *        Thus, in all the code below, m_map contains all the data of current map
         * Author: JL Ding
         * Time: 2019/03/20
         */
        public DataOperator(IMap map)
        {
            m_map = map;
        }

        /* function GetLayerByName
         * Usage: return a layer whose name is equal to the input name string
         * Author: JL Ding
         * Time: 2019/03/20
         */
        public ILayer GetLayerByName(String sLayerName)
        {
            if (sLayerName == "" || m_map == null)
            { 
                return null;
            }
            for (int i = 0; i < m_map.LayerCount; i++)
            {
                if (m_map.get_Layer(i).Name == sLayerName)
                {
                    return m_map.get_Layer(i);   //find the i-th layer
                }
            }
            MessageBox.Show("未找到相应图层！");
            return null;
        }

        /* function GetLayerDataTable
         * Usage: using ISpatialFilter to get the datatable of the layer
         * Error Return value: null
         * Author: JL Ding
         * Time: 2019/05/12
         */
        public DataTable GetLayerDataTable(String sLayerName, ISpatialFilter queryFilter)
        {

            ILayer layer = GetLayerByName(sLayerName);
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            
            if (featureLayer == null)
            {
                return null;
            }
            IFeature feature = null;
            IFeatureCursor featureCursor = null;
            try
            {
                featureCursor = featureLayer.Search(queryFilter, false);
                //IFeatureCursor: Provides access to members that hand out enumerated features, field collections and allows for the updating, deleting and inserting of features.
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("ERROR：非法的SQL语句！");
                return null;
            }//如果SQL语句非法，返回null作为错误值
            finally
            {
                feature = featureCursor.NextFeature();
            }
            if (feature == null)
            {
                return null;
            }
            
            DataTable dataTable = new DataTable();

            if (queryFilter != null)
            {
                featureLayer = (IFeatureLayer)layer;
                //featureClass = featureLayer.FeatureClass;
            }
            IFeatureClass featureClass = featureLayer.FeatureClass;
            
            //Add columns
            int[] columnIndex = new int[featureClass.Fields.FieldCount]; //用于存储某列对应的feature字段
            int columnCount = 0;
            for (int i = 0; i < featureClass.Fields.FieldCount; i++)
            {
                DataColumn datacolumn = new DataColumn();
                String columnName = featureClass.Fields.get_Field(i).Name;
                //MessageBox.Show(queryFilter.SubFields);
                if (queryFilter == null || queryFilter.SubFields.ToString().Contains(columnName))
                {
                    datacolumn.ColumnName = featureClass.Fields.get_Field(i).AliasName;  //copy field name to column name
                    if (datacolumn.ColumnName == "Shape")
                    {
                        datacolumn.DataType = System.Type.GetType("System.Object");
                    }
                    else
                    {
                        datacolumn.DataType = System.Type.GetType("System.Object");
                    }
                    dataTable.Columns.Add(datacolumn);
                    columnIndex[columnCount++] = i; //记录该列对应的feature字段
                }
            }

            //Add rows
            DataRow dataRow;
            while (feature != null)
            {
                dataRow = dataTable.NewRow();
                for (int i = 0; i < columnCount; i++)
                {
                    if (feature.Fields.get_Field(columnIndex[i]).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        switch (feature.Shape.GeometryType)   //for the type of geometry using special switch-case to output its value
                        {
                            case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                                dataRow[i] = "Point";
                                break;
                            case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine:
                                dataRow[i] = "Line";
                                break;
                            case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                                dataRow[i] = "Polygon";
                                break;
                            default:
                                dataRow[i] = "Others";
                                break;
                        }
                    }
                    else
                    {
                        dataRow[i] = feature.get_Value(columnIndex[i]);
                    }
                }
                dataTable.Rows.Add(dataRow);
                feature = featureCursor.NextFeature();
            }

            return dataTable;
        }



        public bool AddFeatureClassToMap(IFeatureClass featureClass, String sLayerName)
        {
            if(featureClass==null || sLayerName=="" ||m_map==null)
                return false;
            IFeatureLayer featureLayer=new FeatureLayerClass();
            featureLayer.FeatureClass=featureClass;
            featureLayer.Name=sLayerName;
            ILayer layer = featureLayer as ILayer;
            if(layer==null)
                return false;
            m_map.AddLayer(layer);
            IActiveView activeView= m_map as IActiveView;
            if (activeView==null)
                return false;
            activeView.Refresh();
            return true;
        }

        /* function CreateShapefile
         * Usage: create a new shapefile and return its featureclass
         * Author: JL Ding
         * Time: 2019/04/10
         */
        public IFeatureClass CreateShapefile(String sParentDirectory, String sWorkspaceName, String sType)
        {
            
            //如果指定的路径下已有此文件夹，则删除
            if(System.IO.Directory.Exists(sParentDirectory+sWorkspaceName))
                System.IO.Directory.Delete(sParentDirectory+sWorkspaceName, true);
            
            //通过工作空间工厂创建新的工作空间文件夹
            IWorkspaceFactory workspaceFactory=new ShapefileWorkspaceFactory(); //【为什么是这个类用来new？
            IWorkspaceName workspaceName = workspaceFactory.Create(sParentDirectory, sWorkspaceName, null, 0);
            ESRI.ArcGIS.esriSystem.IName name = workspaceName as ESRI.ArcGIS.esriSystem.IName;

            IWorkspace workspace = (IWorkspace)name.Open();  
              //为什么IName还能转化成IWorkspace？虽然帮助文档说了Use IName to open a workspace object，但是还是不知道会是这么用啊？
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
              //IFeatureWorkspace和IWorkspace都属于workspace类，可以直接相互转换

            IFields fields = new Fields();
              //书上用的FieldsClass()，不知道两者有什么区别
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;


            
            IFieldEdit fieldEdit = new FieldClass();
              //这边就只能用FieldClass()而不能用Fields()
            fieldEdit.Name_2 = "OID";
            fieldEdit.AliasName_2 = "序号";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField((IField)fieldEdit);     //使用IFieldsEdit接口将新建的字段加入字段集

            fieldEdit = new FieldClass();
            fieldEdit.Name_2 = "Name";
            fieldEdit.AliasName_2 = "名称";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fieldsEdit.AddField((IField)fieldEdit);

            //IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = new GeometryDefClass();//geometryDef as IGeometryDefEdit;
            ////这边就只能用GeometryDefClass()而不能用GeometryDef()
            
            if (sType == "Point")
                geometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint;
            else if (sType == "Line")
                geometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline;
                //geometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine;
            else if (sType == "Polygon")
                geometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon;
            else
                geometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryAny;
            ISpatialReference spatialReference = m_map.SpatialReference;
            geometryDefEdit.SpatialReference_2 = spatialReference;

            fieldEdit = new FieldClass();
            fieldEdit.Name_2 = "Shape";
            fieldEdit.AliasName_2 = "形状";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            IGeometryDef geometryDef = geometryDefEdit;
            fieldEdit.GeometryDef_2 = geometryDef;
            fieldsEdit.AddField((IField)fieldEdit);

            //CreateFeatureClass创建要素
            IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(sWorkspaceName, fields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
            if (featureWorkspace == null)
                return null;
            else
                return featureClass;

        }

        /* function AddFeatureToLayer
         * Usage: add an IFeature to layer
         * Author: JL Ding
         * Time: 2019/04/17
         */
        public bool AddFeatureToLayer(String sLayerName, String sFeatureName, IGeometry geometry)
        {
            if(sLayerName=="" || sFeatureName=="" || geometry==null || m_map==null)
                return false;

            //对地图对象中的图层进行遍历，找到需要添加的图层
            ILayer layer=null;
            for(int i=0;i<m_map.LayerCount;i++)
            {
                layer=m_map.get_Layer(i);
                if(layer.Name==sLayerName)
                    break;
                layer=null;
            }
            if (layer == null)
                return false;    //fail to find the layer
            ////这是一个test用语句//MessageBox.Show(layer.Name);
            //通过IFeatureLayer接口访问获取到的图层，并进一步获取其要素类
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;

            //通过IFeature接口访问要素类新创建的要素，并判断是否成功，若失败，函数返回false
            //ERROR: 有时候会出现HRESULT:0x80004005错误，如果是新建shapefile并加入就不会有这个错误
            //NOTE: 此处只能对有访问权限的要素类进行操作（Esri的几份样例地图均不可以操作，比如在world.mxd中的任意图层中添加都会报错）
            //NOTE: 其实我也不是很清楚是因为没有访问权限报错，还是因为featureclass有多个字段，而本函数没有对这些字段赋值，导致新feature加不进去
            IFeature feature = featureClass.CreateFeature();
            if (feature == null)
                return false;

            //对新创建的要素进行编辑，将其坐标、属性值进行设置，最后保留要素
            feature.Shape = geometry;
            int index = feature.Fields.FindField("Name");
            feature.set_Value(index, sFeatureName);
            feature.Store();
            if (feature == null)
                return false;

            //将地图对象转换为活动视图并添加要素
            IActiveView activeView = m_map as IActiveView;
            if (activeView == null)
                return false;
            activeView.Refresh();
            return true;
        }
    }
}
