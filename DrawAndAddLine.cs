using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;

namespace GIS_Programming
{
    class DrawAndAddLine
    {
        private ESRI.ArcGIS.Display.INewLineFeedback m_lineFeedback;
        private IMap m_map;
        private IActiveView m_focusMap;

        public DrawAndAddLine(int x, int y, IMap m_map)
        {
            m_focusMap = m_map as IActiveView;
            IPoint point = new PointClass();
            point.X = x; 
            point.Y = y;

            if (m_lineFeedback == null)
            {
                m_lineFeedback = new ESRI.ArcGIS.Display.NewLineFeedback();
                m_lineFeedback.Display = m_focusMap.ScreenDisplay;
                m_lineFeedback.Start(point);
            }
            else
            {
                m_lineFeedback.AddPoint(point);
            }
        }
        public void AddPointOfLine(int x, int y)
        {
            if (m_lineFeedback == null)
                return;

            IPoint point = m_focusMap.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y) as IPoint;
            m_lineFeedback.AddPoint(point);
            m_lineFeedback.MoveTo(point);
        }
        public void FinishLine(int x, int y)
        {
            m_focusMap.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            IPolyline polyline;

            if (m_lineFeedback != null)
            {
                polyline = m_lineFeedback.Stop();
                if (polyline != null)
                {
                    //HERE TO CREATE A NEW LINE
                }
            }
            m_focusMap.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            m_lineFeedback = null;
        }

        /*
        protected override void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_lineFeedback == null)
                return;

            IPoint point = m_focusMap.ScreenDisplay.DisplayTransformation.ToMapPoint(e.X, e.Y) as IPoint;
            m_lineFeedback.AddPoint(point);
            m_lineFeedback.MoveTo(point);
            
        }
        protected override void OnDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //首先会先触发Down，然后执行下面的程序：
            m_focusMap.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            IPolyline polyline;

            if (m_lineFeedback != null)
            {
                polyline = m_lineFeedback.Stop();
                if (polyline != null)
                {
                    //HERE TO CREATE A NEW LINE
                }
            }


            m_focusMap.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            m_lineFeedback = null;
        }
         */
    }
}
    