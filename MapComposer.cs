using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;


/* Class: MapComposer
 * Usage: Functions used for map rendering
 * Author: Ding
 * Time: 2019/03/22
 */
namespace GIS_Programming
{
    class MapComposer
    {
        /* Function: GetRendererTypeByLayer
         * Usage: choose renderer by the renderer contained in layer
         * Return: String of Renderer's type
         * Author: Ding
         * Time: 2019/03/22
         */
        public static String GetRendererTypeByLayer(ILayer layer)
        {
            if(layer==null)
                return "图层获取失败";
            
            IFeatureLayer featurelayer = layer as IFeatureLayer;
            IGeoFeatureLayer geoFeaturelayer = featurelayer as IGeoFeatureLayer;
            IFeatureRenderer featureRenderer = geoFeaturelayer.Renderer;

            if (featureRenderer is ISimpleRenderer)
            {
                return "SimpleRenderer";
            }
            else if (featureRenderer is IUniqueValueRenderer)
            {
                return "UniqueValueRenderer";
            }
            else if (featureRenderer is IDotDensityRenderer)
            {
                return "DotDensityRenderer";
            }
            else if (featureRenderer is IChartRenderer)
            {
                return "ChartRenderer";
            }
            else if (featureRenderer is IProportionalSymbolRenderer)
            {
                return "ProportionalSymbolRenderer";
            }
            else if (featureRenderer is IRepresentationRenderer)
            {
                return "RepresentationRenderer";
            }
            else if (featureRenderer is IClassBreaksRenderer)
            {
                return "ClassBreaksRenderer";
            }
            else if (featureRenderer is IBivariateRenderer)
            {
                return "BivariateRenderer";
            }
            else
                return "未知或渲染器获取失败";
        }


        /* Function: GetSymbolFromLayer
         * Usage: get the symbol type of layer
         * Return: ISymbol: Point/Lines...
         * Author: Ding
         * Time: 2019/03/22
         */
        public static ISymbol GetSymbolFromLayer(ILayer layer)
        {
            if (layer == null)
                return null;

            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureCursor featureCursor = featureLayer.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            if (feature == null)
                return null;

            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            IFeatureRenderer featureRenderer = geoFeatureLayer.Renderer;
            if (featureRenderer == null)
                return null;

            ISymbol symbol = featureRenderer.get_SymbolByFeature(feature);  //What's this "get_SymbolByFeature()?"
            return symbol;
        }

        /* Function: RenderSimply
         * Usage: render simlpy, using different renderer with different geotype
         * Return: bool value representing whether rendering is done propoly
         * Author: Ding
         * Time: 2019/03/22
         */
        public static bool RenderSimply(ILayer layer, IColor color)
        {
            if (layer == null || color == null)
                return false;

            ISymbol symbol = GetSymbolFromLayer(layer);
            if (symbol == null)
                return false;

            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            if (featureClass == null)
                return false;
            esriGeometryType geoType = featureClass.ShapeType;

            //choosing different renderer with different geometry type
            switch (geoType)
            {
                case esriGeometryType.esriGeometryPoint:
                    {
                        IMarkerSymbol markerSymbol = symbol as IMarkerSymbol;
                        markerSymbol.Color = color;
                        break;
                    }
                case esriGeometryType.esriGeometryMultipoint:
                    {
                        IMarkerSymbol markerSymbol = symbol as IMarkerSymbol;
                        markerSymbol.Color = color;
                        break;
                    }
                case esriGeometryType.esriGeometryPolyline:
                    {
                        ISimpleLineSymbol simpleLineSymbol = symbol as ISimpleLineSymbol;
                        simpleLineSymbol.Color = color;
                        break;
                    }
                case esriGeometryType.esriGeometryPolygon:
                    {
                        IFillSymbol fillSymbol = symbol as IFillSymbol;
                        fillSymbol.Color = color;
                        break;
                    }
                default:
                    return false;
            }

            //set the renderer
            ISimpleRenderer simpleRenderer = new SimpleRendererClass();
            simpleRenderer.Symbol = symbol;
            IFeatureRenderer featureRenderer = simpleRenderer as IFeatureRenderer;
            if (featureRenderer == null)
                return false;

            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            geoFeatureLayer.Renderer = featureRenderer;
            return true;
        }


    }
}
