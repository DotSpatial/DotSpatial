using System;
using System.Drawing;
using DotSpatial.Controls;
using DotSpatial.Symbology;
using Microsoft.CSharp.RuntimeBinder;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    internal static class LegacyDeserializer
    {
        /// <summary>
        /// Gets the color from a MW settings string.
        /// </summary>
        /// <param name="colorFromMW">The color from MW.</param>
        /// <returns></returns>
        public static Color GetColor(object colorFromMW)
        {
            return ColorTranslator.FromOle(Convert.ToInt32(colorFromMW));
        }

        private static bool UseAlternateParser(dynamic layer)
        {
            // use a try catch to determine what xml version we are using.
            try
            {
                if (layer.ShapeFileProperties == null)
                {
                    // The element is empty but exists.
                    return false;
                }
                else
                {
                    // The element is present.
                    return false;
                }
            }
            catch (RuntimeBinderException)
            {
                // this means the are using ShapefileProperties and a different attribute set.
                // probably version="4.8.2" +
                return true;
            }
        }

        /// <summary>
        /// Deserializes the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="polyLayer">The poly layer.</param>
        internal static void DeserializeLayer(dynamic layer, MapPolygonLayer polyLayer)
        {
            if (UseAlternateParser(layer))
            {
                DeserializeLayerAlternateVersion(layer.ShapefileProperties, polyLayer);
                return;
            }
            var polySymbolizer = new PolygonSymbolizer();
            var outlineColor = LegacyDeserializer.GetColor(layer.ShapeFileProperties["OutLineColor"]);
            var outlineWidth = Convert.ToDouble(layer.ShapeFileProperties["LineOrPointSize"]);
            polySymbolizer.SetOutline(outlineColor, outlineWidth);
            if (Convert.ToBoolean(layer.ShapeFileProperties["DrawFill"]))
            {
                Color color = LegacyDeserializer.GetColor(layer.ShapeFileProperties["Color"]);
                float transparency = Convert.ToSingle(layer.ShapeFileProperties["TransparencyPercent"]);
                color = color.ToTransparent(transparency);
                polySymbolizer.SetFillColor(color);
            }
            else
            {
                polySymbolizer.SetFillColor(Color.Transparent);
            }

            polyLayer.Symbolizer = polySymbolizer;
            try
            {
                int fieldIndex = Convert.ToInt32(layer.ShapeFileProperties.Legend["FieldIndex"]);

                // we have to clear the categories or the collection ends up with a default item
                polyLayer.Symbology.Categories.Clear();

                foreach (var colorBreak in layer.ShapeFileProperties.Legend.ColorBreaks.Elements())
                {
                    PolygonCategory category;

                    string startValue = colorBreak["StartValue"];
                    string endValue = colorBreak["EndValue"];

                    if (startValue == endValue)
                    {
                        category = new PolygonCategory(LegacyDeserializer.GetColor(colorBreak["StartColor"]), LegacyDeserializer.GetColor(colorBreak["StartColor"]), 0);
                        category.FilterExpression = String.Format("[{0}] = '{1}'", polyLayer.DataSet.DataTable.Columns[fieldIndex].ColumnName, startValue);
                        category.LegendText = startValue;
                    }
                    else
                    {
                        category = new PolygonCategory(LegacyDeserializer.GetColor(colorBreak["StartColor"]), LegacyDeserializer.GetColor(colorBreak["EndColor"]), 0, GradientType.Linear, outlineColor, outlineWidth);
                        category.FilterExpression = String.Format("'{2}' >= [{0}] >= '{1}'", polyLayer.DataSet.DataTable.Columns[fieldIndex].ColumnName, startValue, endValue);
                        category.LegendText = String.Format("{0} - {1}", startValue, endValue);
                    }
                    category.LegendText = startValue;
                    category.LegendItemVisible = Convert.ToBoolean(colorBreak["Visible"]);
                    polyLayer.Symbology.AddCategory(category);
                }

                // it took too a lot of work to figure out that we would need to do this...
                polyLayer.ApplyScheme(polyLayer.Symbology);
            }
            catch (RuntimeBinderException)
            {
                // ignore and continue.
                // this means the legend is not available.
            }
        }

        private static void DeserializeLayerAlternateVersion(dynamic shapefileProperties, MapPolygonLayer polyLayer)
        {
            var polySymbolizer = new PolygonSymbolizer();
            var outlineColor = LegacyDeserializer.GetColor(shapefileProperties.DefaultDrawingOptions["LineColor"]);
            var outlineWidth = Convert.ToDouble(shapefileProperties.DefaultDrawingOptions["LineWidth"]);
            polySymbolizer.SetOutline(outlineColor, outlineWidth);
            if (Convert.ToBoolean(shapefileProperties.DefaultDrawingOptions["FillVisible"]))
            {
                Color color = LegacyDeserializer.GetColor(shapefileProperties.DefaultDrawingOptions["FillBgColor"]);
                float transparency = Convert.ToInt32(shapefileProperties.DefaultDrawingOptions["FillTransparency"]) / 255f;
                color = color.ToTransparent(transparency);
                polySymbolizer.SetFillColor(color);
            }
            else
            {
                polySymbolizer.SetFillColor(Color.Transparent);
            }

            polyLayer.Symbolizer = polySymbolizer;
        }

        /// <summary>
        /// Deserializes the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="lineLayer">The line layer.</param>
        internal static void DeserializeLayer(dynamic layer, MapLineLayer lineLayer)
        {
            if (UseAlternateParser(layer))
            {
                // TODO: write alternate parser for this layer information.
                return;
            }
            var lineSymbolizer = new LineSymbolizer();
            lineSymbolizer.SetWidth(Convert.ToDouble(layer.ShapeFileProperties["LineOrPointSize"]));

            lineLayer.Symbolizer = lineSymbolizer;
        }

        /// <summary>
        /// Deserializes the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="pointLayer">The point layer.</param>
        internal static void DeserializeLayer(dynamic layer, MapPointLayer pointLayer)
        {
            if (UseAlternateParser(layer))
            {
                // TODO: write alternate parser for this layer information.
                return;
            }
            LegacyPointType typeOfPoint = (LegacyPointType)Enum.ToObject(typeof(LegacyPointType), Convert.ToInt32(layer.ShapeFileProperties["PointType"]));

            PointSymbolizer pointSymbolizer;

            if (ConvertLegacyPointTypeToPointShape(typeOfPoint) == PointShape.Undefined)
            {
                pointSymbolizer = new PointSymbolizer();
            }
            else
            {
                var color = LegacyDeserializer.GetColor(layer.ShapeFileProperties["Color"]);
                var width = Convert.ToDouble(layer.ShapeFileProperties["LineOrPointSize"]);

                pointSymbolizer = new PointSymbolizer(color, ConvertLegacyPointTypeToPointShape(typeOfPoint), width);
            }
            pointLayer.Symbolizer = pointSymbolizer;
        }

        private static PointShape ConvertLegacyPointTypeToPointShape(LegacyPointType typeOfPoint)
        {
            PointShape pointShape = PointShape.Undefined;
            switch (typeOfPoint)
            {
                case LegacyPointType.Circle:
                    pointShape = PointShape.Ellipse;
                    break;
                case LegacyPointType.Diamond:
                    pointShape = PointShape.Diamond;
                    break;
                case LegacyPointType.FontChar:
                    break;
                case LegacyPointType.ImageList:
                    break;
                case LegacyPointType.Square:
                    pointShape = PointShape.Rectangle;
                    break;
                case LegacyPointType.TriangleDown:
                case LegacyPointType.TriangleLeft:
                case LegacyPointType.TriangleRight:
                case LegacyPointType.TriangleUp:
                    pointShape = PointShape.Triangle;
                    break;
                case LegacyPointType.UserDefined:
                    break;
                default:
                    break;
            }
            return pointShape;
        }

        /// <summary>
        /// Deserializes the layer properties.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="mapLayer">The map layer.</param>
        internal static void DeserializeLayerProperties(dynamic layer, Layer mapLayer)
        {
            mapLayer.LegendText = layer["Name"];
            mapLayer.IsVisible = Convert.ToBoolean(layer["Visible"]);
            mapLayer.IsExpanded = Convert.ToBoolean(layer["Expanded"]);
            try
            {
                mapLayer.UseDynamicVisibility = Convert.ToBoolean(layer.DynamicVisibility["UseDynamicVisibility"]);
                mapLayer.DynamicVisibilityWidth = Convert.ToDouble(layer.DynamicVisibility["Scale"]);
            }
            catch (RuntimeBinderException)
            {
                // No DynamicVisibility node, ignore that.
            }
        }
    }
}