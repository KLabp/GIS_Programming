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
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;

namespace GIS_Programming
{
    class DrawAndAddPolygon
    {
        protected ESRI.ArcGIS.Display.INewPolygonFeedback m_polygonFeedback;
        protected IMap m_map;
        protected IActiveView m_focusMap;

        public DrawAndAddPolygon(IMap m_map)
        {
            m_focusMap = m_map as IActiveView;
        }

        public void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            IPoint point = m_focusMap.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y) as IPoint;

            if (m_polygonFeedback == null)
            {
                m_polygonFeedback = new ESRI.ArcGIS.Display.NewPolygonFeedback();
                m_polygonFeedback.Display = m_focusMap.ScreenDisplay;
                m_polygonFeedback.Start(point);
            }

            m_polygonFeedback.AddPoint(point);
            m_polygonFeedback.MoveTo(point);

        }
        public void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO: Add WxCrossSectionTool.OnMouseMove implementation
            IPoint point = m_focusMap.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y) as IPoint;
            if (m_polygonFeedback != null)
                m_polygonFeedback.MoveTo(point);
        }
        public IPolygon OnDoubleClick(int Button, int Shift, int X, int Y)
        {
            //首先会先触发Down，然后执行下面的程序：
            m_focusMap.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            IPolygon polygon = null;
            if (m_polygonFeedback != null)
            {
                polygon = m_polygonFeedback.Stop();
            }

            m_focusMap.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            m_polygonFeedback = null;

            return polygon;
        }

    }
}
