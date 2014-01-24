using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using DotSpatial.Controls;
using DotSpatial.Symbology;
using Microsoft.CSharp.RuntimeBinder;
using System.Diagnostics;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// Used to support lbl files.
    /// </summary>
    public class LegacyLayerDeserializer
    {
        /// <summary>
        /// Opens the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="map">The map.</param>
        public static void TryDeserialization(ILayer layer, IMap map)
        {
            // Is there a way to simplify this method, which requires use of different interfaces for different types?
            FeatureLayer featureLayer = layer as FeatureLayer;
            if (featureLayer != null)
            {
                if (featureLayer.DataSet != null && !String.IsNullOrEmpty(featureLayer.DataSet.Filename))
                {
                    Open(featureLayer.DataSet.Filename, map, featureLayer);
                    return;
                }
            }

            MapImageLayer imageLayer = layer as MapImageLayer;
            if (imageLayer != null)
            {
                if (imageLayer.DataSet != null && !String.IsNullOrEmpty(imageLayer.DataSet.Filename))
                {
                    Open(imageLayer.DataSet.Filename, map, imageLayer);
                    return;
                }
            }
        }

        /// <summary>
        /// Opens the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="map">The map.</param>
        /// <param name="layer">The layer.</param>
        public static void Open(string fileName, IMap map, Layer layer)
        {
            Contract.Requires(!String.IsNullOrEmpty(fileName), "fileName is null or empty.");
            Contract.Requires(map != null, "map is null.");
            Contract.Requires(layer != null, "featureLayer is null.");

            string lblFile = Path.ChangeExtension(fileName, "lbl");
            if (File.Exists(lblFile) && layer is IFeatureLayer)
            {
                try
                {
                    dynamic parser = DynamicXMLNode.Load(lblFile);
                    DeserializeLabels(parser.Labels, map, layer as IFeatureLayer);
                }
                catch (RuntimeBinderException ex)
                {
                    Trace.WriteLine(ex.Message);
                }

            }
            string mwsrFile = Path.ChangeExtension(fileName, "mwsr");
            if (File.Exists(mwsrFile))
            {
                try
                {
                    dynamic parser = DynamicXMLNode.Load(mwsrFile);
                    DeserializeLayer(parser.Layer, map, layer);
                }
                catch (RuntimeBinderException ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
            string mwleg = Path.ChangeExtension(fileName, "mwleg");
            if (File.Exists(mwleg) && layer is MapImageLayer)
            {
                try
                {
                    dynamic parser = DynamicXMLNode.Load(mwleg);
                    DeserializeLegend(parser.GridColoringScheme, map, layer);
                }
                catch (RuntimeBinderException ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
        }

        private static void DeserializeLabels(dynamic labels, IMap map, IFeatureLayer featureLayer)
        {
            int fieldIndex = Convert.ToInt32(labels["Field"]) - 1;
            var fieldName = featureLayer.DataSet.DataTable.Columns[fieldIndex].ColumnName;

            var symbolizer = new LabelSymbolizer();
            symbolizer.FontFamily = labels["Font"];

            try
            {
                if (Convert.ToBoolean(labels["Bold"]))
                    symbolizer.FontStyle = FontStyle.Bold;
                else if (Convert.ToBoolean(labels["Italic"]))
                    symbolizer.FontStyle = FontStyle.Italic;
                else if (Convert.ToBoolean(labels["Underline"]))
                    symbolizer.FontStyle = FontStyle.Underline;
            }
            catch (RuntimeBinderException)
            {
                // ignore and continue.
                // some versions of the files don't have these properties.
            }

            symbolizer.FontColor = LegacyDeserializer.GetColor(labels["Color"]);

            LegacyHJustification typeOfJustification = (LegacyHJustification)Enum.ToObject(typeof(LegacyHJustification), Convert.ToInt32(labels["Justification"]));

            switch (typeOfJustification)
            {
                case LegacyHJustification.Center:
                    symbolizer.Orientation = ContentAlignment.MiddleCenter;
                    break;
                case LegacyHJustification.Left:
                    symbolizer.Orientation = ContentAlignment.MiddleLeft;
                    break;
                case LegacyHJustification.Right:
                    symbolizer.Orientation = ContentAlignment.MiddleRight;
                    break;
                case LegacyHJustification.None:
                case LegacyHJustification.Raw:
                default:
                    break;
            }

            try
            {
                symbolizer.DropShadowEnabled = Convert.ToBoolean(labels["UseShadows"]);
                symbolizer.DropShadowColor = LegacyDeserializer.GetColor(labels["Color"]);
            }
            catch (RuntimeBinderException) { }

            // not entirely sure if Offset from MW4 translates to OffsetX.
            try { symbolizer.OffsetX = Convert.ToInt32(labels["Offset"]); }
            catch (RuntimeBinderException) { }

            string expression = String.Format("[{0}]", fieldName);

            map.AddLabels(featureLayer, expression, null, symbolizer, expression);

            featureLayer.LabelLayer.UseDynamicVisibility = Convert.ToBoolean(labels["UseMinZoomLevel"]);

            try { featureLayer.LabelLayer.DynamicVisibilityWidth = Convert.ToDouble(labels["Scale"]); }
            catch (RuntimeBinderException) { }
        }

        private static void DeserializeLegend(dynamic layer, IMap map, Layer imageLayer)
        {
            //var polyLayer = imageLayer as MapImageLayer;

            ////var imageSymbolizer = new ImageSymbolizer();
            ////var outlineColor = LegacyDeserializer.GetColor(layer.ShapeFileProperties["OutLineColor"]);
            ////var outlineWidth = Convert.ToDouble(layer.ShapeFileProperties["LineOrPointSize"]);
            ////polySymbolizer.SetOutline(outlineColor, outlineWidth);
            ////if (Convert.ToBoolean(layer.ShapeFileProperties["DrawFill"]))
            ////{
            ////    System.Drawing.Color color = LegacyDeserializer.GetColor(layer.ShapeFileProperties["Color"]);
            ////    float transparency = Convert.ToSingle(layer.ShapeFileProperties["TransparencyPercent"]);
            ////    color = color.ToTransparent(transparency);
            ////    polySymbolizer.SetFillColor(color);
            ////}
            ////else
            ////{
            ////    polySymbolizer.SetFillColor(Color.Transparent);
            ////}

            ////layer.Symbolizer = imageSymbolizer;

            //var j = layer.Break;
            //try
            //{
            //    int fieldIndex = Convert.ToInt32(layer.ShapeFileProperties.Legend["FieldIndex"]);

            //    // we have to clear the categories or the collection ends up with a default item
            //    polyLayer.Symbology.Categories.Clear();

            //    //foreach (var colorBreak in layer.ShapeFileProperties.Legend.ColorBreaks.Elements())
            //    //{
            //    //    PolygonCategory category;

            //    //    string startValue = colorBreak["StartValue"];
            //    //    string endValue = colorBreak["EndValue"];

            //    //    if (startValue == endValue)
            //    //    {
            //    //        category = new PolygonCategory(LegacyDeserializer.GetColor(colorBreak["StartColor"]), LegacyDeserializer.GetColor(colorBreak["StartColor"]), 0);
            //    //        category.FilterExpression = String.Format("[{0}] = '{1}'", polyLayer.DataSet.DataTable.Columns[fieldIndex].ColumnName, startValue);
            //    //        category.LegendText = startValue;
            //    //    }
            //    //    else
            //    //    {
            //    //        category = new PolygonCategory(LegacyDeserializer.GetColor(colorBreak["StartColor"]), LegacyDeserializer.GetColor(colorBreak["EndColor"]), 0, GradientType.Linear, outlineColor, outlineWidth);
            //    //        category.FilterExpression = String.Format("'{2}' >= [{0}] >= '{1}'", polyLayer.DataSet.DataTable.Columns[fieldIndex].ColumnName, startValue, endValue);
            //    //        category.LegendText = String.Format("{0} - {1}", startValue, endValue); ;
            //    //    }
            //    //    category.LegendText = startValue;
            //    //    category.LegendItemVisible = Convert.ToBoolean(colorBreak["Visible"]);
            //    //    polyLayer.Symbology.AddCategory(category);
            //    //}

            //    // it took too a lot of work to figure out that we would need to do this...
            //    polyLayer.ApplyScheme(polyLayer.Symbology);
            //}
            //catch (RuntimeBinderException)
            //{
            //    // ignore and continue.
            //    // this means the legend is not available.
            //}
        }

        private static void DeserializeLayer(dynamic layer, IMap map, Layer featureLayer)
        {
            LegacyLayerType typeOfLayer = (LegacyLayerType)Enum.ToObject(typeof(LegacyLayerType), Convert.ToInt32(layer["Type"]));

            switch (typeOfLayer)
            {
                case LegacyLayerType.Grid:
                    break;
                case LegacyLayerType.Image:
                    break;
                case LegacyLayerType.Invalid:
                    throw new ArgumentException("The LayerType is an invalid layer type and cannot be loaded.");

                case LegacyLayerType.LineShapefile:
                    LegacyDeserializer.DeserializeLayer(layer, featureLayer as MapLineLayer);
                    break;
                case LegacyLayerType.PointShapefile:
                    LegacyDeserializer.DeserializeLayer(layer, featureLayer as MapPointLayer);
                    break;
                case LegacyLayerType.PolygonShapefile:
                    LegacyDeserializer.DeserializeLayer(layer, featureLayer as MapPolygonLayer);

                    break;
                default:
                    throw new NotImplementedException("That LayerType is not supported.");
            }

            LegacyDeserializer.DeserializeLayerProperties(layer, featureLayer);
        }
    }
}