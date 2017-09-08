// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotSpatial.Controls;
using DotSpatial.Projections;
using DotSpatial.Symbology;
using NetTopologySuite.IO;
using OSGeo.OGR;
using OSGeo.OSR;
using Layer = OSGeo.OGR.Layer;
using OgrFeature = OSGeo.OGR.Feature;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    ///  OgrDatareader provide readonly forward only access to OsGeo data sources accessible though the OGR project.
    /// </summary>
    public class OgrDataReader : IDisposable
    {
        #region Fields

        private static bool hasTextField;

        private readonly string _fileName;

        private readonly DataSource _ogrDataSource;

        #endregion

        #region Constructors

        static OgrDataReader()
        {
            GdalConfiguration.ConfigureOgr();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OgrDataReader"/> class.
        /// </summary>
        /// <param name="sDataSource">The path of the data source file.</param>
        public OgrDataReader(string sDataSource)
        {
            _ogrDataSource = Ogr.Open(sDataSource, 0);
            _fileName = sDataSource;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Translates the style strings from the list to DotSpatial style categories and adds them to the given layer.
        /// </summary>
        /// <param name="layer">The layer the styles get added to.</param>
        /// <param name="styles">The style strings that should get translated.</param>
        public static void TranslateStyles(IMapFeatureLayer layer, IList<string> styles)
        {
            var featureType = layer.DataSet.FeatureType;

            switch (featureType)
            {
                case FeatureType.MultiPoint:
                case FeatureType.Point:
                    {
                        // create the scheme
                        var scheme = new PointScheme();
                        scheme.Categories.Clear();
                        scheme.EditorSettings.ClassificationType = ClassificationType.UniqueValues;
                        scheme.EditorSettings.FieldName = "style";
                        scheme.LegendText = "Point";

                        // add the default category
                        var cat = new PointCategory(Color.Black, Symbology.PointShape.Rectangle, 5)
                        {
                            LegendText = "default",
                            Symbolizer =
                                  {
                                      ScaleMode = ScaleMode.Simple,
                                      Smoothing = false
                                  }
                        };
                        scheme.AddCategory(cat);

                        var labelLayer = new MapLabelLayer();
                        labelLayer.Symbology.Categories.Clear();

                        bool needsLabels = styles.Any(_ => !string.IsNullOrWhiteSpace(_) && _.StartsWith("LABEL"));

                        foreach (var style in styles)
                        {
                            TranslatePointStyle(scheme, labelLayer.Symbology, style);
                        }

                        // assign the scheme
                        layer.Symbology = scheme;

                        // assign the label layer if needed
                        if (needsLabels)
                        {
                            layer.LabelLayer = labelLayer;
                            layer.ShowLabels = true;
                            layer.LabelLayer.CreateLabels();
                        }

                        layer.DataSet.UpdateExtent();
                        layer.DataSet.InitializeVertices();
                        layer.AssignFastDrawnStates();

                        break;
                    }

                case FeatureType.Line:
                    {
                        // create the scheme
                        var scheme = new LineScheme();
                        scheme.Categories.Clear();
                        scheme.EditorSettings.ClassificationType = ClassificationType.UniqueValues;
                        scheme.EditorSettings.FieldName = "style";
                        scheme.LegendText = "Line";

                        // add the default category
                        var cat = new LineCategory(Color.Black, 1)
                        {
                            LegendText = "default",
                            Symbolizer =
                                  {
                                      ScaleMode = ScaleMode.Simple,
                                      Smoothing = false
                                  }
                        };
                        scheme.AddCategory(cat);

                        // add the categories from the file
                        foreach (var style in styles)
                        {
                            TranslateLineStyle(scheme, style);
                        }

                        // assign the scheme
                        layer.Symbology = scheme;

                        layer.DataSet.UpdateExtent();
                        layer.DataSet.InitializeVertices();
                        layer.AssignFastDrawnStates();

                        break;
                    }

                case FeatureType.Polygon:
                    {
                        // create the scheme
                        var scheme = new PolygonScheme();
                        scheme.Categories.Clear();
                        scheme.EditorSettings.ClassificationType = ClassificationType.UniqueValues;
                        scheme.EditorSettings.FieldName = "style";
                        scheme.LegendText = "Polygon";

                        // add the default category
                        var cat = new PolygonCategory(Color.GhostWhite, Color.Black, 1)
                        {
                            LegendText = "default",
                            Symbolizer =
                                  {
                                      ScaleMode = ScaleMode.Simple,
                                      Smoothing = false
                                  }
                        };
                        scheme.AddCategory(cat);

                        // add the categories from the file
                        foreach (var style in styles)
                        {
                            TranslatePolygonStyle(scheme, style);
                        }

                        // assign the scheme
                        layer.Symbology = scheme;

                        layer.DataSet.UpdateExtent();
                        layer.DataSet.InitializeVertices();
                        layer.AssignFastDrawnStates();

                        break;
                    }

                default: throw new ArgumentOutOfRangeException(nameof(featureType), featureType, null);
            }
        }

        /// <summary>
        /// This disposes the underlying data source.
        /// </summary>
        public void Dispose()
        {
            _ogrDataSource.Dispose();
        }

        /// <summary>
        /// Gets the layers of the given file.
        /// </summary>
        /// <returns>List with the layers of the file.</returns>
        public IDictionary<IFeatureSet, IList<string>> GetLayers()
        {
            var drv = _ogrDataSource.GetDriver().GetName().ToLower();

            if (drv == "dxf") return GetDxfLayers();

            return GetOtherLayers();
        }

        private static void AddColumns(IFeatureSet fs, IList<Column> schemaTable)
        {
            foreach (Column column in schemaTable)
            {
                string sFieldName = column.Name;

                int uniqueNumber = 1;
                string uniqueName = sFieldName;
                while (fs.DataTable.Columns.Contains(uniqueName))
                {
                    uniqueName = sFieldName + uniqueNumber;
                    uniqueNumber++;
                }

                if (uniqueName.ToLower() == "text") hasTextField = true;

                fs.DataTable.Columns.Add(new DataColumn(uniqueName, column.Type));
            }

            // add style and label column
            if (!fs.DataTable.Columns.Contains("style"))
            {
                fs.DataTable.Columns.Add(new DataColumn("style", typeof(string)));
            }
        }

        /// <summary>
        /// Adds the features for the other file types.
        /// </summary>
        /// <param name="fs">The featureset the features get added to.</param>
        /// <param name="styles">The list the styles get added to.</param>
        /// <param name="layer">The layer to get the features from.</param>
        /// <param name="ogrFeatureDefinition">The feature definition that contains the field count.</param>
        /// <param name="schema">The list of columns used to fill the datarow.</param>
        private static void AddFeatures(IFeatureSet fs, IList<string> styles, Layer layer, FeatureDefn ogrFeatureDefinition, IList<Column> schema)
        {
            var wkbReader = new WKBReader();
            OgrFeature ogrFeature = layer.GetNextFeature();

            var fieldCount = ogrFeatureDefinition.GetFieldCount();

            while (ogrFeature != null)
            {
                var wkbGeometry = GetGeometry(ogrFeature);
                var geometry = wkbReader.Read(wkbGeometry);

                if (geometry != null && geometry.IsValid && !geometry.IsEmpty)
                {
                    IFeature feature = new Feature(geometry);
                    if (fs.Features.Count == 0 || feature.FeatureType == fs.FeatureType)
                    {
                        fs.Features.Add(feature);
                        for (int i = 0; i < fieldCount; i++)
                        {
                            feature.DataRow[i] = GetValue(i, schema, ogrFeature) ?? DBNull.Value;
                        }

                        var str = ogrFeature.GetStyleString();

                        feature.DataRow["style"] = str;
                        if (!styles.Contains(str))
                        {
                            // add the style to the layer
                            styles.Add(str);
                        }
                    }
                }

                ogrFeature = layer.GetNextFeature();
            }
        }

        /// <summary>
        /// Adds the features for the dxf files.
        /// </summary>
        /// <param name="fs">The list of featuresets the features get added to.</param>
        /// <param name="styles">The list of lists the styles get added to.</param>
        /// <param name="layer">The layer to get the features from.</param>
        /// <param name="fieldCount">The number of field contained in the features datarow.</param>
        /// <param name="schema">The list of columns used to fill the datarow.</param>
        private static void AddFeatures(Dictionary<FeatureType, IFeatureSet> fs, Dictionary<FeatureType, List<string>> styles, Layer layer, int fieldCount, IList<Column> schema)
        {
            var wkbReader = new WKBReader();
            OgrFeature ogrFeature = layer.GetNextFeature();

            while (ogrFeature != null)
            {
                var wkbGeometry = GetGeometry(ogrFeature);
                var geometry = wkbReader.Read(wkbGeometry);

                if (geometry != null && geometry.IsValid && !geometry.IsEmpty)
                {
                    IFeature feature = new Feature(geometry);
                    if (feature.FeatureType != FeatureType.Unspecified)
                    {
                        fs[feature.FeatureType].Features.Add(feature);
                        for (int i = 0; i < fieldCount; i++)
                        {
                            feature.DataRow[i] = GetValue(i, schema, ogrFeature) ?? DBNull.Value;
                        }

                        var str = ogrFeature.GetStyleString();
                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            str = RemoveText(str);
                            feature.DataRow["style"] = str;
                            if (!styles[feature.FeatureType].Contains(str))
                            {
                                // add the style to the layer
                                styles[feature.FeatureType].Add(str);
                            }
                        }
                    }
                }

                ogrFeature = layer.GetNextFeature();
            }
        }

        private static IList<Column> BuildSchemaTable(FeatureDefn ogrFeatureDefinition)
        {
            IList<Column> schema = new List<Column>();

            for (int i = 0; i < ogrFeatureDefinition.GetFieldCount(); i++)
            {
                FieldDefn f = ogrFeatureDefinition.GetFieldDefn(i);
                schema.Add(new Column(f.GetName(), f.GetFieldType()));
            }

            return schema;
        }

        /// <summary>
        /// Gets the angle.
        /// </summary>
        /// <param name="style">The style to get the angle from. The parameter part gets removed from it.</param>
        /// <returns>0 if no angle was found otherwise the angle that was found.</returns>
        private static double GetAngle(ref string style)
        {
            var angle = GetValue(ref style, "a:");
            var num = GetNumber(angle, 0);

            while (num > 360) num -= 360;
            while (num < 0) num += 360;

            num = 360 - num; // turn around because the angle is counter clockwise

            return num;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="style">The style to get the color from. The parameter part gets removed from it.</param>
        /// <param name="parameter">The parameter whose color should be returned.</param>
        /// <returns>Black color / white fill color if nothing was found otherwise the color that was found.</returns>
        private static Color GetColor(ref string style, Parameter parameter)
        {
            var color = GetValue(ref style, parameter.Name);
            return string.IsNullOrWhiteSpace(color) ? parameter.DefaultColor : ColorTranslator.FromHtml(color);
        }

        /// <summary>
        /// Gets the DashStyle.
        /// </summary>
        /// <param name="style">The style to get the DashStyle from. The parameter part gets removed from it.</param>
        /// <param name="transparent">This out value indicates whether the color should be turned to transparent. This is needed for ogr-pen-1 which is an invisible pen.</param>
        /// <returns>Solid if nothing was found otherwise the DashStyle best fitting to the ogr-pen that was found.</returns>
        private static DashStyle GetDashStyle(ref string style, out bool transparent)
        {
            transparent = false;
            if (string.IsNullOrWhiteSpace(style)) return DashStyle.Solid;

            var id = GetValue(ref style, "id:").Trim('\"');
            if (string.IsNullOrWhiteSpace(id)) return DashStyle.Solid;

            // ogr-pen-0: solid(the default when no id is provided)
            // ogr-pen-1: null pen(invisible)
            // ogr-pen-2: dash
            // ogr-pen-3: short-dash
            // ogr-pen-4: long-dash
            // ogr-pen-5: dot line
            // ogr-pen-6: dash - dot line
            // ogr-pen-7: dash - dot - dot line
            // ogr-pen-8: alternate - line(sets every other pixel)
            string[] keys = { "ogr-pen-0", "ogr-pen-1", "ogr-pen-2", "ogr-pen-3", "ogr-pen-4", "ogr-pen-5", "ogr-pen-6", "ogr-pen-7", "ogr-pen-8" };
            string sKeyResult = keys.FirstOrDefault(s => id.Contains(s));

            if (string.IsNullOrWhiteSpace(sKeyResult)) return DashStyle.Solid;

            switch (sKeyResult[sKeyResult.Length - 1])
            {
                case '1':
                    transparent = true;
                    return DashStyle.Solid;
                case '2':
                case '3':
                case '4': return DashStyle.Dash;
                case '5':
                case '8': return DashStyle.Dot;
                case '6': return DashStyle.DashDot;
                case '7': return DashStyle.DashDotDot;
                default: return DashStyle.Solid;
            }
        }

        /// <summary>
        /// Gets the font name.
        /// </summary>
        /// <param name="style">The style to get the color from. The parameter part gets removed from it.</param>
        /// <returns>Null, of no font was found otherwise the name of the font that was found.</returns>
        private static string GetFontName(ref string style)
        {
            if (string.IsNullOrWhiteSpace(style)) return null;

            var fontName = GetValue(ref style, "f:").Trim('\"');
            if (string.IsNullOrWhiteSpace(fontName)) return null;

            return fontName;
        }

        /// <summary>
        /// Gets the font size. This gets only the numeric value without the units.
        /// </summary>
        /// <param name="style">The style to get the font size from. The parameter part gets removed from it.</param>
        /// <returns>1 if no font size was found otherwise the font size that was found.</returns>
        private static double GetFontSize(ref string style)
        {
            var size = GetValue(ref style, "s:");
            var num = GetNumber(size);
            if (num < 5) num += 5;
            return num;
        }

        /// <summary>
        /// Gets the features geometry as Wkb.
        /// </summary>
        /// <param name="feature">Feature whose geometry is returned.</param>
        /// <returns>The features geometry.</returns>
        private static byte[] GetGeometry(OgrFeature feature)
        {
            Geometry ogrGeometry = feature.GetGeometryRef();
            ogrGeometry.FlattenTo2D();
            byte[] wkbGeometry = new byte[ogrGeometry.WkbSize()];
            ogrGeometry.ExportToWkb(wkbGeometry, wkbByteOrder.wkbXDR);

            return wkbGeometry;
        }

        /// <summary>
        /// Gets the numeric value of the given string. This can only be used for strings that contain a single number.
        /// </summary>
        /// <param name="numberString">The string whose number should be returned.</param>
        /// <param name="defaultValue">The default value that should be returned if nothing was found or number parsing went wrong.</param>
        /// <returns>1 if no number was found otherwise the number that was found.</returns>
        private static double GetNumber(string numberString, double defaultValue = 1)
        {
            double number = defaultValue;

            if (!string.IsNullOrWhiteSpace(numberString))
            {
                var regex = Regex.Match(numberString, "[0-9]+([.,][0-9]+)?").Value;
                double.TryParse(regex, out number);
            }

            return number;
        }

        /// <summary>
        /// Gets the orientation of the label.
        /// </summary>
        /// <param name="style">The style to get the orientation from. The parameter part gets removed from it.</param>
        /// <returns>MiddleCenter if nothing was found otherwise the orientation that was found.</returns>
        private static ContentAlignment GetOrientation(ref string style)
        {
            var position = GetValue(ref style, "p:");
            int num = (int)GetNumber(position, 5);

            switch (num)
            {
                case 1:
                case 10:
                    return ContentAlignment.BottomLeft;
                case 2:
                case 11:
                    return ContentAlignment.BottomCenter;
                case 3:
                case 12:
                    return ContentAlignment.BottomRight;
                case 4:
                    return ContentAlignment.MiddleLeft;
                case 6:
                    return ContentAlignment.MiddleRight;
                case 7:
                    return ContentAlignment.TopLeft;
                case 8:
                    return ContentAlignment.TopCenter;
                case 9:
                    return ContentAlignment.TopRight;
                default:
                    return ContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Gets the pattern. This gets only the numeric values without the units.
        /// </summary>
        /// <param name="style">The style to get the pattern from. The parameter part gets removed from it.</param>
        /// <returns>Null if nothing was found otherwise the pattern values.</returns>
        private static float[] GetPattern(ref string style)
        {
            if (string.IsNullOrWhiteSpace(style)) return null;

            var pattern = GetValue(ref style, "p:").Trim('\"');
            if (string.IsNullOrWhiteSpace(pattern)) return null;

            var regex = Regex.Matches(pattern, "[0-9]+([.,][0-9]+)?");
            var nums = new List<float>();

            for (int i = 0; i < regex.Count; i++)
            {
                float val;
                if (float.TryParse(regex[i].Value, out val) && val > 0)
                {
                    nums.Add(val);
                }
            }

            return nums.Count < 2 ? null : nums.ToArray();
        }

        /// <summary>
        /// Adds the projection info of the OGR layer to the FeatureSet.
        /// </summary>
        /// <param name="layer">The layer that is used to create the FeatureSet.</param>
        /// <returns>Null on error otherwise the ProjectionInfo object that contains the data of the layers proj4 string.</returns>
        private static ProjectionInfo GetProjectionInfo(Layer layer)
        {
            try
            {
                SpatialReference osrSpatialref = layer.GetSpatialRef();
                if (osrSpatialref != null)
                {
                    string sProj4String;
                    osrSpatialref.ExportToProj4(out sProj4String);
                    return ProjectionInfo.FromProj4String(sProj4String);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the substring of the given parameter followed by a comma.
        /// </summary>
        /// <param name="style">Style to get the substring from.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>An empty string if nothing was found otherwise the substring that was extracted.</returns>
        private static string GetSubString(ref string style, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(style)) return string.Empty;

            int paramLength = parameterName.Length;

            var i1 = style.IndexOf(parameterName, StringComparison.Ordinal);
            if (i1 > -1)
            {
                var i2 = style.IndexOf(",", i1, StringComparison.Ordinal);

                if (i2 == -1) i2 = style.IndexOf(")", i1 + paramLength, StringComparison.Ordinal); // this is the last parameter

                if (i2 > i1 + paramLength)
                {
                    var value = style.Substring(i1, i2 - i1) + ",";
                    style = style.Remove(i1, i2 - i1 + 1);
                    return value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the names of the tools contained in the given style together with the corresponding styles parts.
        /// </summary>
        /// <param name="style">The style that gets separated.</param>
        /// <returns>A dictionary that contains all the tool names found in the style together with the substring belonging to the tool.</returns>
        private static Dictionary<string, string> GetToolNames(string style)
        {
            // find the tools
            var list = new SortedDictionary<int, string>();
            int index = style.IndexOf("(", StringComparison.Ordinal);

            while (index > -1)
            {
                var i2 = style.LastIndexOf(";", index, StringComparison.Ordinal);

                if (i2 == -1)
                {
                    // this is the first tool
                    var toolname = style.Substring(0, index);
                    list.Add(0, toolname);
                }
                else
                {
                    var toolname = style.Substring(i2, index - i2);
                    list.Add(i2, toolname);
                }

                index += 1;
                if (index < style.Length)
                {
                    index = style.IndexOf("(", index, StringComparison.Ordinal);
                }
                else
                {
                    index = -1;
                }
            }

            // get the substrings belonging to the tools
            var returns = new Dictionary<string, string>();
            string lastName = string.Empty;
            int startIndex = 0;

            foreach (var l in list)
            {
                if (l.Key > 0)
                {
                    returns.Add(lastName, style.Substring(startIndex, l.Key - startIndex));
                }

                lastName = l.Value;
                startIndex = l.Key;
            }

            returns.Add(lastName, style.Substring(startIndex));

            return returns;
        }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <param name="schema">Schema that contains the field type.</param>
        /// <param name="feature">The feature whose value gets returned.</param>
        /// <returns>The field value as object.</returns>
        private static object GetValue(int i, IList<Column> schema, OgrFeature feature)
        {
            switch (schema[i].FieldType)
            {
                case FieldType.OFTString:
                    return feature.GetFieldAsString(i);

                case FieldType.OFTInteger:
                    return feature.GetFieldAsInteger(i);

                case FieldType.OFTDateTime:
                    {
                        int year;
                        int month;
                        int day;

                        int h;
                        int m;
                        int s;
                        int flag;

                        feature.GetFieldAsDateTime(i, out year, out month, out day, out h, out m, out s, out flag);
                        try
                        {
                            return new DateTime(year, month, day, h, m, s);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            return DateTime.MinValue;
                        }
                    }

                case FieldType.OFTReal:
                    return feature.GetFieldAsDouble(i);

                default: return null;
            }
        }

        /// <summary>
        /// Gets the value of the parameter with the given name. The name must contain the : at the end.
        /// </summary>
        /// <param name="style">The style that gets searched. The parameter part gets removed from it.</param>
        /// <param name="parameterName">The name of the paramater whose value should be returned.</param>
        /// <returns>Null if nothing could be found otherwise the value that was found.</returns>
        private static string GetValue(ref string style, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(style)) return null;

            string value = null;

            int paramLength = parameterName.Length;

            var i1 = style.IndexOf(parameterName, StringComparison.Ordinal);
            if (i1 > -1)
            {
                var i2 = style.IndexOf(",", i1, StringComparison.Ordinal);

                if (i2 == -1) i2 = style.IndexOf(")", i1 + paramLength, StringComparison.Ordinal); // this is the last parameter

                if (i2 > i1 + paramLength)
                {
                    value = style.Substring(i1 + paramLength, i2 - i1 - paramLength);
                    style = style.Remove(i1, i2 - i1 + 1);
                }
            }

            return value;
        }

        /// <summary>
        /// Gets the width. This gets only the numeric value without the units.
        /// </summary>
        /// <param name="style">The style to get the width from. The parameter part gets removed from it.</param>
        /// <returns>1 if no width was found otherwise the width that was found.</returns>
        private static double GetWidth(ref string style)
        {
            var width = GetValue(ref style, "w:");
            return GetNumber(width);
        }

        /// <summary>
        /// Removes all the unused parameters from Label styles.
        /// </summary>
        /// <param name="style">Style that should be cleaned.</param>
        /// <returns>The whole style for non label styles otherwise the cleaned style.</returns>
        private static string RemoveText(string style)
        {
            if (!style.StartsWith("LABEL")) return style;

            var oldStyle = style;
            var sb = new StringBuilder();

            sb.Append(GetSubString(ref oldStyle, "s:"));
            sb.Append(GetSubString(ref oldStyle, Parameters.Color.Name));
            sb.Append(GetSubString(ref oldStyle, Parameters.BoxColor.Name));
            sb.Append(GetSubString(ref oldStyle, Parameters.HaloColor.Name));
            sb.Append(GetSubString(ref oldStyle, Parameters.ShadowColor.Name));
            sb.Append(GetSubString(ref oldStyle, "f:"));
            sb.Append(GetSubString(ref oldStyle, "a:"));
            sb.Append(GetSubString(ref oldStyle, "p:"));

            var newStyle = sb.ToString();

            if (!string.IsNullOrWhiteSpace(newStyle))
            {
                newStyle = "LABEL(" + newStyle.Trim(',') + ")";
            }

            return newStyle;
        }

        /// <summary>
        /// Translates the given style as line style and adds it to the given line scheme.
        /// </summary>
        /// <param name="scheme">The scheme the style gets added to.</param>
        /// <param name="style">The style that gets translated.</param>
        private static void TranslateLineStyle(LineScheme scheme, string style)
        {
            if (string.IsNullOrWhiteSpace(style)) return;

            var mystyle = style;
            int index = mystyle.IndexOf("(", StringComparison.Ordinal);
            if (index < 1) return;

            var toolname = mystyle.Substring(0, index);
            mystyle = mystyle.Substring(index + 1);

            Color col = Color.Black;
            double width = 1;
            DashStyle dashStyle = DashStyle.Solid;
            float[] pattern = null;
            bool invisible = true;

            switch (toolname)
            {
                case "PEN":
                    col = GetColor(ref mystyle, Parameters.Color);
                    width = GetWidth(ref mystyle);
                    pattern = GetPattern(ref mystyle);
                    dashStyle = GetDashStyle(ref mystyle, out invisible);
                    break;
            }

            if (pattern != null) dashStyle = DashStyle.Custom;
            if (invisible) col = Color.Transparent;

            var stroke = new CartographicStroke(col)
            {
                Width = width,
                DashStyle = dashStyle
            };
            if (pattern != null) stroke.DashPattern = pattern;

            var symb = new LineSymbolizer
            {
                ScaleMode = ScaleMode.Simple,
                Smoothing = false,
                Strokes =
                {
                    [0] = stroke
                }
            };

            var cat = new LineCategory
            {
                FilterExpression = $"[style] = '{style}'",
                LegendText = style,
                Symbolizer = symb
            };

            scheme.AddCategory(cat);
        }

        /// <summary>
        /// Translates the given ogrType to the corresponding System.Type.
        /// </summary>
        /// <param name="ogrType">The ogrType to get the System.Type for.</param>
        /// <returns>The type that corresponds to the given ogrType.</returns>
        private static Type TranslateOgrType(FieldType ogrType)
        {
            switch (ogrType)
            {
                case FieldType.OFTInteger:
                    return typeof(int);

                case FieldType.OFTReal:
                    return typeof(double);

                case FieldType.OFTWideString:
                case FieldType.OFTString:
                    return typeof(string);

                case FieldType.OFTBinary:
                    return typeof(byte[]);

                case FieldType.OFTDate:
                case FieldType.OFTTime:
                case FieldType.OFTDateTime:
                    return typeof(DateTime);

                default:
                    throw new NotSupportedException("Type not supported: " + ogrType);
            }
        }

        /// <summary>
        /// Translates the given style as point style and adds it to the given point scheme.
        /// </summary>
        /// <param name="scheme">The scheme the style gets added to.</param>
        /// <param name="labelScheme">The scheme the label definitions gets added to.</param>
        /// <param name="style">The style that gets translated.</param>
        private static void TranslatePointStyle(PointScheme scheme, ILabelScheme labelScheme, string style)
        {
            if (string.IsNullOrWhiteSpace(style)) return;

            var myStyle = style;
            int index = myStyle.IndexOf("(", StringComparison.Ordinal);

            if (index < 1) return;

            var toolname = myStyle.Substring(0, index);
            myStyle = myStyle.Substring(index + 1);

            switch (toolname)
            {
                case "PEN":
                    {
                        var color = GetColor(ref myStyle, Parameters.Color);
                        var width = GetWidth(ref myStyle);

                        var cat = new PointCategory(color, Symbology.PointShape.Rectangle, width)
                        {
                            FilterExpression = $"[style] = '{style}'",
                            LegendText = style,
                            Symbolizer =
                            {
                                ScaleMode = ScaleMode.Simple,
                                Smoothing = false
                            }
                        };
                        scheme.AddCategory(cat);
                        break;
                    }

                case "LABEL":
                    {
                        var fontSize = GetFontSize(ref myStyle);
                        var fontColor = GetColor(ref myStyle, Parameters.Color);
                        var boxColor = GetColor(ref myStyle, Parameters.BoxColor);
                        var haloColor = GetColor(ref myStyle, Parameters.HaloColor);
                        var shadowColor = GetColor(ref myStyle, Parameters.ShadowColor);
                        var angle = GetAngle(ref myStyle);
                        var font = GetFontName(ref myStyle);
                        var orientation = GetOrientation(ref myStyle);

                        // create a transparent point category because the point is only used to position the label
                        var cat = new PointCategory(Color.Transparent, Symbology.PointShape.Rectangle, 1)
                        {
                            FilterExpression = $"[style] = '{style}'",
                            LegendText = style,
                            Symbolizer =
                            {
                                ScaleMode = ScaleMode.Simple,
                                Smoothing = false
                            }
                        };
                        scheme.AddCategory(cat);

                        // create the label category only if the text column exists, otherwise we don't know where to take the text from
                        if (hasTextField)
                        {
                            var lcat = new LabelCategory
                            {
                                Name = style,
                                Expression = "[Text]",
                                FilterExpression = $"[style] = '{style}'",
                                Symbolizer =
                                {
                                    ScaleMode = ScaleMode.Simple,
                                    Alignment = StringAlignment.Center,
                                    FontColor = fontColor,
                                    FontSize = (float)fontSize,
                                    Orientation = orientation
                                }
                            };

                            if (!string.IsNullOrWhiteSpace(font)) lcat.Symbolizer.FontFamily = font;

                            if (angle != 0)
                            {
                                lcat.Symbolizer.Angle = angle;
                                lcat.Symbolizer.UseAngle = true;
                            }

                            if (haloColor != Color.Empty)
                            {
                                lcat.Symbolizer.HaloColor = haloColor;
                                lcat.Symbolizer.HaloEnabled = true;
                            }

                            if (boxColor != Color.Empty)
                            {
                                lcat.Symbolizer.BackColor = boxColor;
                                lcat.Symbolizer.BackColorEnabled = true;
                            }

                            if (shadowColor != Color.Empty)
                            {
                                lcat.Symbolizer.DropShadowColor = shadowColor;
                                lcat.Symbolizer.DropShadowEnabled = true;
                            }

                            labelScheme.Categories.Add(lcat);
                        }

                        break;
                    }

                default: throw new NotImplementedException($"The translation of the point tool {toolname} is not yet implemented.");
            }
        }

        /// <summary>
        /// Translates the given style as polygon style and adds it to the given polygon scheme.
        /// </summary>
        /// <param name="scheme">The scheme the style gets added to.</param>
        /// <param name="style">The style that gets translated.</param>
        private static void TranslatePolygonStyle(PolygonScheme scheme, string style)
        {
            if (string.IsNullOrWhiteSpace(style)) return;

            var tools = GetToolNames(style);

            Color col = Color.Black;
            Color fillCol = Color.White;
            double numWidth = 1;

            foreach (var tool in tools)
            {
                switch (tool.Key)
                {
                    case "PEN":
                        {
                            var myStyle = tool.Value.Substring(4);
                            col = GetColor(ref myStyle, Parameters.Color);
                            numWidth = GetWidth(ref myStyle);
                            break;
                        }

                    case "BRUSH":
                        {
                            var myStyle = tool.Value.Substring(6);
                            fillCol = GetColor(ref myStyle, Parameters.BrushForeColor);
                            break;
                        }
                }
            }

            var cat = new PolygonCategory(fillCol, col, numWidth)
            {
                FilterExpression = $"[style] = '{style}'",
                LegendText = style,
                Symbolizer =
                {
                    ScaleMode = ScaleMode.Simple,
                    Smoothing = false
                }
            };
            scheme.AddCategory(cat);
        }

        /// <summary>
        /// Gets the layers of the given dxf file.
        /// </summary>
        /// <returns>The list of featuresets and styles contained in this file.</returns>
        private IDictionary<IFeatureSet, IList<string>> GetDxfLayers()
        {
            var liste = new Dictionary<FeatureType, IFeatureSet>
                        {
                            { FeatureType.Point, new FeatureSet(FeatureType.Point) },
                            { FeatureType.Polygon, new FeatureSet(FeatureType.Polygon) },
                            { FeatureType.MultiPoint, new FeatureSet(FeatureType.MultiPoint) },
                            { FeatureType.Line, new FeatureSet(FeatureType.Line) }
                        };

            var styles = new Dictionary<FeatureType, List<string>>
                         {
                             { FeatureType.Point, new List<string>() },
                             { FeatureType.Polygon, new List<string>() },
                             { FeatureType.MultiPoint, new List<string>() },
                             { FeatureType.Line, new List<string>() }
                         };

            using (var layer = _ogrDataSource.GetLayerByIndex(0)) // dxf files contain only one layer called entities, the layers seen in programs are defined by the Layer column
            using (var ogrFeatureDefinition = layer.GetLayerDefn())
            {
                var schema = BuildSchemaTable(ogrFeatureDefinition);
                var projInfo = GetProjectionInfo(layer);

                foreach (var entry in liste)
                {
                    AddColumns(entry.Value, schema);
                    if (projInfo != null) entry.Value.Projection = projInfo;
                    entry.Value.Name = entry.Value.FeatureType.ToString();
                }

                var fieldCount = ogrFeatureDefinition.GetFieldCount();

                AddFeatures(liste, styles, layer, fieldCount, schema);
            }

            var retVal = new Dictionary<IFeatureSet, IList<string>>();

            foreach (var l in liste)
            {
                if (l.Value.Features.Count > 0)
                {
                    retVal.Add(l.Value, styles[l.Key]);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Gets the layers of the given file.
        /// </summary>
        /// <returns>The list of featuresets contained in this file.</returns>
        private IDictionary<IFeatureSet, IList<string>> GetOtherLayers()
        {
            using (var layer = _ogrDataSource.GetLayerByIndex(0)) // dxf files contain only one layer called entities, the layers seen in programs are defined by the Layer column
            using (var ogrFeatureDefinition = layer.GetLayerDefn())
            {
                var schema = BuildSchemaTable(ogrFeatureDefinition);
                var projInfo = GetProjectionInfo(layer);

                var fs = new FeatureSet
                {
                    Name = Path.GetFileNameWithoutExtension(_fileName),
                    Filename = _fileName
                };
                AddColumns(fs, schema);
                if (projInfo != null) fs.Projection = projInfo;

                var styles = new List<string>();

                AddFeatures(fs, styles, layer, ogrFeatureDefinition, schema);
                return new Dictionary<IFeatureSet, IList<string>> { { fs, styles } };
            }
        }

        #endregion

        #region Classes

        /// <summary>
        /// This contains the color parameters that can be found in a style string.
        /// </summary>
        private static class Parameters
        {
            #region Fields

            public static readonly Parameter BoxColor = new Parameter("b:", System.Drawing.Color.Empty);
            public static readonly Parameter BrushBackColor = new Parameter("bc:", System.Drawing.Color.White);
            public static readonly Parameter BrushForeColor = new Parameter("fc:", System.Drawing.Color.Black);
            public static readonly Parameter Color = new Parameter("c:", System.Drawing.Color.Black);
            public static readonly Parameter HaloColor = new Parameter("o:", System.Drawing.Color.Empty);
            public static readonly Parameter ShadowColor = new Parameter("h:", System.Drawing.Color.Empty);

            #endregion
        }

        private class Column
        {
            #region Constructors

            public Column(string name, FieldType fieldType)
            {
                Name = name;
                FieldType = fieldType;
                Type = TranslateOgrType(fieldType);
            }

            #endregion

            #region Properties

            public FieldType FieldType { get; }

            public string Name { get; }

            public Type Type { get; }

            #endregion
        }

        private class Parameter
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Parameter"/> class.
            /// </summary>
            /// <param name="name">Name of the parameter.</param>
            /// <param name="defaultColor">The default color that should be assigned if the parameter can't be found.</param>
            public Parameter(string name, Color defaultColor)
            {
                Name = name;
                DefaultColor = defaultColor;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the default color that should be assigned if the parameter can't be found.
            /// </summary>
            public Color DefaultColor { get; }

            /// <summary>
            /// Gets the name of the parameter. This contains some letters and the : indicative for the parameter according to http://www.gdal.org/ogr_feature_style.html.
            /// </summary>
            public string Name { get; }

            #endregion
        }

        #endregion
    }
}