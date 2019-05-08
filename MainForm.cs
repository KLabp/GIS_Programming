using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

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

namespace GIS_Programming
{
    public sealed partial class MainForm : Form
    {
        DrawAndAddLine drawAndAddLine = null;
        DrawAndAddPolygon drawAndAddPolygon = null;
        #region class private members
        private IMapControl3 m_mapControl = null;
        private string m_mapDocumentName = string.Empty;
        #endregion

        #region class constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            //get the MapControl
            m_mapControl = (IMapControl3)axMapControl1.Object;

            //disable the Save menu (since there is no document yet)
            menuSaveDoc.Enabled = false;
        }

        #region Main Menu event handlers
        private void menuNewDoc_Click(object sender, EventArgs e)
        {
            //execute New Document command
            ICommand command = new CreateNewDocument();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuOpenDoc_Click(object sender, EventArgs e)
        {
            //execute Open Document command
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuSaveDoc_Click(object sender, EventArgs e)
        {
            //execute Save Document command
            if (m_mapControl.CheckMxFile(m_mapDocumentName))
            {
                //create a new instance of a MapDocument
                IMapDocument mapDoc = new MapDocumentClass();
                mapDoc.Open(m_mapDocumentName, string.Empty);

                //Make sure that the MapDocument is not readonly
                if (mapDoc.get_IsReadOnly(m_mapDocumentName))
                {
                    MessageBox.Show("Map document is read only!");
                    mapDoc.Close();
                    return;
                }

                //Replace its contents with the current map
                mapDoc.ReplaceContents((IMxdContents)m_mapControl.Map);

                //save the MapDocument in order to persist it
                mapDoc.Save(mapDoc.UsesRelativePaths, false);

                //close the MapDocument
                mapDoc.Close();
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            //execute SaveAs Document command
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuExitApp_Click(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }
        #endregion

        /* Function: Load_Bookmark
         * Usage: load the opening map's bookmark into select-bookmark combobox
         * Input: sBookmarkName = the name of bookmark introducted
         * Output: a spatial reference
         * Author: JL Ding
         * Time: 2019/03/08
         */
        private void Load_Bookmark()
        {
            IMapBookmarks bookmarks = axMapControl1.Map as IMapBookmarks;  //get the map's bookmark through axMapControl1.Map
            IEnumSpatialBookmark enumSpatialBookmark = bookmarks.Bookmarks;

            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark = enumSpatialBookmark.Next();
            while (spatialBookmark != null)
            {
                cbBookmarkList.Items.Add(spatialBookmark.Name);
                spatialBookmark = enumSpatialBookmark.Next();
            }
        }

        //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //get the current document name from the MapControl
            m_mapDocumentName = m_mapControl.DocumentFilename;

            //transform  the document from axMapControl to PageLayoutControl
            axPageLayoutControl1.LoadMxFile(axMapControl1.DocumentFilename);
            
            //if there is no MapDocument, diable the Save menu and clear the statusbar
            if (m_mapDocumentName == string.Empty)
            {
                menuSaveDoc.Enabled = false;
                statusBarXY.Text = string.Empty;
            }
            else
            {
                //enable the Save manu and write the doc name to the statusbar
                menuSaveDoc.Enabled = true;
                miOutput.Enabled = true;
                statusBarXY.Text = System.IO.Path.GetFileName(m_mapDocumentName);
            }
            Load_Bookmark();//load bookmarks
        }


        /* ��꽻��: 
         * 1. OnMouseDown
         * 2. OnDoubleClick
         * 3. OnMouseMove
         */
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //IMapControlEvents2_OnMouseDownEvent e ��e.x��e.MapX��ʲô����
            if (miAddFeature.Checked == true)
            {
                //AddPointģʽ
                if (miAddPoint.Checked == true)
                {
                    //if (!AddPoint(e.mapX, e.mapY))
                        //MessageBox.Show("��ӵ�ʧ�ܣ�"); 
                    AddPoint(e.mapX, e.mapY);
                }
                //AddLineģʽ
                else if (miAddLine.Checked == true)
                {
                    //�������ڣ���Ҫ�и��ط������INewLineFeedback������������ʹ��ȫ�ֱ���
                    //������ȫ�ֱ���Ϊ�գ�Ϊ��ȫ�ֱ����½�ʵ��
                    if (drawAndAddLine == null)
                    {
                        drawAndAddLine = new DrawAndAddLine(axMapControl1.Map);
                        drawAndAddLine.OnMouseDown(e.button, e.shift, e.x, e.y);
                    }
                    //��Ϊ�գ�����������ӵ�
                    else
                        drawAndAddLine.OnMouseDown(e.button, e.shift, e.x, e.y);
                }
                else if (miAddPolygon.Checked == true)
                {
                    if (drawAndAddPolygon == null)
                    {
                        drawAndAddPolygon = new DrawAndAddPolygon(axMapControl1.Map);
                        drawAndAddPolygon.OnMouseDown(e.button, e.shift, e.x, e.y);
                    }
                    //��Ϊ�գ�����������ӵ�
                    else
                        drawAndAddPolygon.OnMouseDown(e.button, e.shift, e.x, e.y);
                }
            }
        }

        private void axMapControl1_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            if (miAddFeature.Checked == true)
            {
                if (miAddLine.Checked == true)
                {
                    //���������ʱ��˫������ʱ���Ȼ����OnMouseDown��������һ����
                    //if (!AddLine(drawAndAddLine.OnDoubleClick(e.button, e.shift, e.x, e.y)))
                    AddLine(drawAndAddLine.OnDoubleClick(e.button, e.shift, e.x, e.y));
                    //MessageBox.Show("�����ʧ�ܣ�");
                    //�ÿջ�ԭ
                    drawAndAddLine = null;
                }
                else if (miAddPolygon.Checked == true)
                {
                    //���������ʱ��˫������ʱ���Ȼ����OnMouseDown��������һ����
                    //if (!AddPolygon(drawAndAddLine.OnDoubleClick(e.button, e.shift, e.x, e.y)))
                    AddPolygon(drawAndAddPolygon.OnDoubleClick(e.button, e.shift, e.x, e.y));
                    //MessageBox.Show("�����ʧ�ܣ�");
                    //�ÿջ�ԭ
                    drawAndAddPolygon = null;
                }
            }
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
            if (miAddFeature.Checked == true)
            {
                if (miAddLine.Checked == true)
                {

                    if (drawAndAddLine != null)
                        drawAndAddLine.OnMouseMove(e.button, e.shift, e.x, e.y);
                }
                else if (miAddPolygon.Checked == true)
                {

                    if (drawAndAddPolygon != null)
                        drawAndAddPolygon.OnMouseMove(e.button, e.shift, e.x, e.y);
                }
            }
            
        }


        /* Usage: Create a bookmark
         * Input: sBookmarkName = the name of bookmark introducted
         * Output: a spatial reference
         * Author: Ding
         * Time: 2019/03/08
         */
        public void CreateBookmark(string sBookmarkName) //sBookmarkName is the name of bookmark created by user
        {

            //through intersect IAOIBookmark to create a variable, whose type is AOIBookmark, using to save the present scale of map
            IAOIBookmark aoiBookmark = new AOIBookmarkClass();
            if (aoiBookmark != null)
            {
                aoiBookmark.Location = axMapControl1.ActiveView.Extent;
                aoiBookmark.Name = sBookmarkName;
            }
            //through intersect IAOIBookmark to access present map and add new bookmark to map
            IMapBookmarks bookmarks = axMapControl1.Map as IMapBookmarks;
            if (bookmarks != null)
            {
                bookmarks.AddBookmark(aoiBookmark);
            }
            //add the name of new bookmark into ComboBox, for using the bookmarks later
            cbBookmarkList.Items.Add(aoiBookmark.Name);
        }

        private void miCreateBookmarkList_Click(object sender, EventArgs e)
        {
            AdmitBookmarkName frmABN = new AdmitBookmarkName(this);  //frmABN is the object short name of AdmitBookmarkName
            frmABN.Show();
        }

        private void cbBookmarkList_SelectedIndexChanged(object sender, EventArgs e)
        {
            IMapBookmarks bookmarks = axMapControl1.Map as IMapBookmarks;

            IEnumSpatialBookmark enumSpatialBookmark = bookmarks.Bookmarks;

            enumSpatialBookmark.Reset();  //what's the function?
            ISpatialBookmark spatialBookmark = enumSpatialBookmark.Next();
            while (spatialBookmark != null)
            {
                if (spatialBookmark.Name == cbBookmarkList.SelectedItem.ToString())
                {
                    spatialBookmark.ZoomTo(axMapControl1.Map);
                    axMapControl1.ActiveView.Refresh();
                    break;
                }
                spatialBookmark = enumSpatialBookmark.Next();

            }
        }
        /* Usage: View the data of selected layer
         * Author: Ding
         * Time: 2019/03/22
         */
        private void miAccessData_Click(object sender, EventArgs e)
        {
            IMap m_map = axMapControl1.Map;
            if (m_mapDocumentName == string.Empty)  //If no map was loaded, the function won't run, preventing stack overfolw.
            {
                MessageBox.Show("ERROR: û�п���ʾ��ͼ��!");
                return;
            }
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            DataBoard dataBoard = new DataBoard(axMapControl1.Map, dataOperator.GetLayerDataTable(m_map.get_Layer(0).Name));
            //default show the datatable of teh frist layer
            dataBoard.Show();
        }

        /* Usage: Simply render the layer - main function
          * Author: Ding
          * Time: 2019/03/22
          */
        private void mainRenderSimply(String LayerName)
        {
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            if (LayerName == "") 
            {
                MessageBox.Show("ERROR: û�п���ʾ��ͼ��!");
                return;
            }
            ILayer layer = dataOperator.GetLayerByName(LayerName);

            //Set the color of symbol from ColorDialog
            // -- I don't want to rewrite the colordialog class, so let it show in the middle of window
            ColorDialog colorDialog1 = new ColorDialog();
            Color selectedColor = Color.FromArgb(0, 255, 0, 0);  //selectedColor is the color selected for the symbol, default color is red
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog1.Color;

            }
            //set color
            if (selectedColor.A == 0)
                return;
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = selectedColor.R; rgbColor.Green = selectedColor.G; rgbColor.Blue = selectedColor.B;



            ISymbol symbol = MapComposer.GetSymbolFromLayer(layer);
            IColor color = rgbColor as IColor;

            bool bRes = MapComposer.RenderSimply(layer, color);
            if (bRes)
            {
                axTOCControl1.ActiveView.ContentsChanged();
                axMapControl1.ActiveView.Refresh();
                //miRenderSimply.Enabled = false;
            }
            else
                MessageBox.Show("����Ⱦͼ��ʧ�ܣ�");
        }

        /* Usage: Simply render the layer - operation function
          * Author: Ding
          * Time: 2019/03/22
          */
        private void miRenderSimply_MouseEnter(object sender, EventArgs e)
        {
            cbLayerList.Text = "";
            cbLayerList.Text = "ѡȡ����ͼ��"; //bug here
        }
        private void cbLayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string LayerName;
            LayerName = cbLayerList.SelectedItem.ToString();

            mainRenderSimply(LayerName);
        }
        private void miCarto_Click(object sender, EventArgs e)
        {
            IMap m_map = axMapControl1.Map;
            if (cbLayerList.Items.Count == 0)
            {
                for (int i = 0; i < m_map.LayerCount; i++)
                {
                    cbLayerList.Items.Add(m_map.get_Layer(i).Name);
                }
            }
        }
        //end of operation function

        private void miGetRenderInfo_Click(object sender, EventArgs e)
        {
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            ILayer layer = axMapControl1.Map.get_Layer(0);
            MessageBox.Show(MapComposer.GetRendererTypeByLayer(layer));
        }


        // Function CopyToPageLayout: using to copy the map from MapControl to PageLayoutContol
        private void CopyToPageLayout()
        {
            IObjectCopy objectCopy = new ObjectCopyClass();
            object copyFromMap = axMapControl1.Map;
            object copyMap = objectCopy.Copy(copyFromMap);
            object copyToMap = axPageLayoutControl1.ActiveView.FocusMap;
            objectCopy.Overwrite(copyMap, ref copyToMap);

        }

        private void miPageLayout_Click(object sender, EventArgs e)
        {
            //using IObjectCopy to Connect MapControl and PageLayoutContol
            IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            IDisplayTransformation dispalyTransformation = activeView.ScreenDisplay.DisplayTransformation;
            dispalyTransformation.VisibleBounds = axMapControl1.Extent;
                //axPageLayoutControl1.ActiveView.Refresh();
            CopyToPageLayout();
            
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1.Object);
            axTOCControl1.SetBuddyControl(axPageLayoutControl1.Object);

            //Switch the window
            axPageLayoutControl1.Visible = true;
            axMapControl1.Visible = false;

            //switch the mode of buttons
            miPageLayout.Checked = true;
            miMap.Checked = false;
            miPrint.Enabled = true;
        }
        /*
        private void CopyToMap()
        {
            IObjectCopy objectCopy = new ObjectCopyClass();
            object copyFromMap = axPageLayoutControl1.ActiveView.FocusMap;
            object copyMap = objectCopy.Copy(copyFromMap);
            object copyToMap = axMapControl1.Map;
            objectCopy.Overwrite(copyMap, ref copyToMap);

        }
        */

        private void miMap_Click(object sender, EventArgs e)
        {
            //using IObjectCopy to Connect MapControl and PageLayoutContol
            IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            axPageLayoutControl1.ActiveView.Refresh();

            axToolbarControl1.SetBuddyControl(axMapControl1.Object);
            axTOCControl1.SetBuddyControl(axMapControl1.Object);

            //axMapControl1.LoadMxFile(axPageLayoutControl1.Name);
            axMapControl1.Map.MapScale = axPageLayoutControl1.ActiveView.FocusMap.MapScale;

            //Switch the window
            axMapControl1.Visible = true;
            axPageLayoutControl1.Visible = false;

            //switch the mode of buttons
            miPageLayout.Checked = false;
            miMap.Checked = true;
            miPrint.Enabled = false;
        }

        private void miPrint_Click(object sender, EventArgs e)
        {
            IPrinter printer = axPageLayoutControl1.Printer;
            if (printer == null)
                MessageBox.Show("��ȡĬ�ϴ�ӡ��ʧ��!");
            IPaper paper = printer.Paper;
            paper.Orientation = 2;

        }

        private void ShowOutputFileDialog(IActiveView docActiveView, int iOutputResolution = 320)
        {
            IExport docExport;
            IPrintAndExport docPrintExport;
            IWorldFileSettings worldFileSetting;
            //string localFilePath, fileNameExt, newFileName, FilePath; 
            SaveFileDialog sfd = new SaveFileDialog();
            
            //���öԻ������
            sfd.Title = "��ͼ���";
            //�����ļ����� 
            sfd.Filter = "EMF��*.emf��|*.emf|AI (*.ai) |*.ai|PDF (*.pdf)|*.pdf |SVG (*.svg)| *.svg|TIFF��*.tif��|*.tif|JPEG (*.jpg)| *.jpg|PNG (*.png)| *.png";
            //����Ĭ���ļ�������ʾ˳�� 
            sfd.FilterIndex = 5;
            //����Ի����Ƿ�����ϴδ򿪵�Ŀ¼ 
            sfd.RestoreDirectory = true;
            //����Ĭ�ϵ��ļ���
            int start = m_mapDocumentName.LastIndexOf("\\");
            int end = m_mapDocumentName.LastIndexOf(".");
            sfd.FileName = m_mapDocumentName.Substring(start + 1, end - start -1);
            //���˱��水ť���� 
            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName!="")
            {
                switch(sfd.FilterIndex)
                {       
                    case 1: { docExport = new ExportEMFClass(); break; }
                    case 2: { docExport = new ExportAIClass(); break; }
                    case 3: { docExport = new ExportPDFClass(); break; }
                    case 4: { docExport = new ExportSVGClass(); break; }
                    case 5:
                    {
                        docExport = new ExportTIFFClass();
                        IExportTIFF exportTiff = docExport as IExportTIFF;
                        exportTiff.GeoTiff = true;
                        exportTiff.CompressionType = esriTIFFCompression.esriTIFFCompressionLZW;
                        break;
                    } //����TIFF��ʽ�������е�����׼���ļ�
                    case 6: { docExport = new ExportJPEGClass(); break; }
                    case 7: { docExport = new ExportPNGClass(); break; }
                    default: { docExport = new ExportTIFFClass(); break; }
                }
                docPrintExport = new PrintAndExportClass();
                worldFileSetting = docExport as IWorldFileSettings;
                string localFilePath = sfd.FileName.ToString(); //����ļ�·�� 
                //string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1); //��ȡ�ļ���������·��
                docExport.ExportFileName = localFilePath;
                docPrintExport.Export(docActiveView, docExport, iOutputResolution, true, null);

            }
        }

        private void miOutput_Click(object sender, EventArgs e)
        {
            //miOutput.Enable�ĸı��ں���void axMapControl1_OnMapReplaced�����
            IActiveView docActiveView;
            //IExport docExport;
            //IPrintAndExport docPrintExport;
            int iOutputResolution = 320;
            if (miPageLayout.Checked)
                docActiveView = axPageLayoutControl1.ActiveView;
            else
                docActiveView = axMapControl1.ActiveView;
            ShowOutputFileDialog(docActiveView, iOutputResolution);

        }

        

        private void miCreateShapefile_Click(object sender, EventArgs e)
        {
            IMap m_map = axMapControl1.Map;
            NewFeatureClass frmNFC = new NewFeatureClass(this, m_map);
            frmNFC.Show();
            if (frmNFC.DialogResult == DialogResult.OK)
            {
                //�ڴ��������ȫ����ѡ��·��������shp�����뵽��ͼ�Ĳ���
                miCreateShapefile.Enabled = false;
                return;
            }
        }

        //�������������ǵ�����Ҫ�ص�point,line,polygon����Ӧ����
        private void miAddPoint_Click(object sender, EventArgs e)
        {
            //ȷ�������ǡ�AddPointģʽ����������ѡ������
            if (miAddPoint.Checked == false)
            {
                miAddFeature.Checked = true;   //ȷ���Ƿ��ڻ���ģʽ
                miAddPoint.Checked = true;
                miAddLine.Checked = false;
                miAddPolygon.Checked = false;
            }
            else
            {
                miAddFeature.Checked = false;
                miAddPoint.Checked = false;
                miAddLine.Checked = false;
                miAddPolygon.Checked = false;
            }
        }
        private void miAddLine_Click(object sender, EventArgs e)
        {
            //ȷ�������ǡ�AddLineģʽ����������ѡ������
            if (miAddLine.Checked == false)
            {
                miAddLine.Checked = true;
                miAddPoint.Checked = false;
                miAddPolygon.Checked = false;
                miAddFeature.Checked = true;
            }
            else
            {
                miAddLine.Checked = false;
                miAddPoint.Checked = false;
                miAddPolygon.Checked = false;
                miAddFeature.Checked = false;
            }
        }
        private void miAddPolygon_Click(object sender, EventArgs e)
        {
            //ȷ�������ǡ�AddPolygonģʽ����������ѡ������
            if (miAddLine.Checked == false)
            {
                miAddPolygon.Checked = true;
                miAddPoint.Checked = false;
                miAddLine.Checked = false;
                miAddFeature.Checked = true;
            }
            else
            {
                miAddPolygon.Checked = false;
                miAddPoint.Checked = false;
                miAddLine.Checked = false;
                miAddFeature.Checked = false;
            }
        }

        //�����������������Ҫ�ص�������
        private bool AddPoint(double x, double y)
        {
            IPoint point = new PointClass();
            point.PutCoords(x, y);
            AddFeatureToLayer frmAFTL = new AddFeatureToLayer(axMapControl1.Map, point);
            frmAFTL.Show();
            if (frmAFTL.DialogResult == DialogResult.OK)
                return true;
            else
                return false;
        }
        private bool AddLine(IPolyline polyline)
        {
            AddFeatureToLayer frmAFTL = new AddFeatureToLayer(axMapControl1.Map, polyline);
            frmAFTL.Show();
            if (frmAFTL.DialogResult == System.Windows.Forms.DialogResult.OK)
                return true;
            else
                return false;
        }
        private bool AddPolygon(IPolygon polygon)
        {
            AddFeatureToLayer frmAFTL = new AddFeatureToLayer(axMapControl1.Map, polygon);
            frmAFTL.Show();
            if (frmAFTL.DialogResult == System.Windows.Forms.DialogResult.OK)
                return true;
            else
                return false;
        }

        

        

        
        
    }
}