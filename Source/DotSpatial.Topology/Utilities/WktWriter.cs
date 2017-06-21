// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Outputs the textual representation of a Geometry.
    /// The WktWriter outputs coordinates rounded to the precision
    /// model. No more than the maximum number of necessary decimal places will be
    /// output.
    /// The Well-known Text format is defined in the OpenGIS Simple Features  Specification
    /// (http://www.opengis.org/techno/specs.htm) for SQL.
    /// A non-standard "LINEARRING" tag is used for LinearRings. The WKT spec does
    /// not define a special tag for LinearRings. The standard tag to use is
    /// "LINESTRING".
    /// </summary>
    public class WktWriter
    {
        private const int WKT_WRITER_INDENT = 2;
        private NumberFormatInfo _formatter;
        private bool _isFormatted;

        /// <summary>
        /// Generates the WKT for a <c>Point</c>.
        /// </summary>
        /// <param name="p0">The point coordinate.</param>
        /// <returns></returns>
        public static String ToPoint(Coordinate p0)
        {
            return "POINT ( " + p0.X + " " + p0.Y + " )";
        }

        /// <summary>
        /// Generates the WKT for a N-point <c>LineString</c>.
        /// </summary>
        /// <param name="seq">The sequence to output.</param>
        /// <returns></returns>
        public static String ToLineString(ICoordinateSequence seq)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("LINESTRING ");
            if (seq.Count == 0)
                buf.Append(" EMPTY");
            else
            {
                buf.Append("(");
                for (int i = 0; i < seq.Count; i++)
                {
                    if (i > 0)
                        buf.Append(", ");
                    buf.Append(seq[i].X + " " + seq[i].Y);
                }
                buf.Append(")");
            }
            return buf.ToString();
        }

        /// <summary>
        /// Generates the WKT for a 2-point <c>LineString</c>.
        /// </summary>
        /// <param name="p0">The first coordinate.</param>
        /// <param name="p1">The second coordinate.</param>
        /// <returns></returns>
        public static String ToLineString(Coordinate p0, Coordinate p1)
        {
            return "LINESTRING ( " + p0.X + " " + p0.Y + ", " + p1.X + " " + p1.Y + " )";
        }

        /// <summary>
        /// Creates the <c>NumberFormatInfo</c> used to write <c>double</c>s
        /// with a sufficient number of decimal places.
        /// </summary>
        /// <param name="precisionModel">
        /// The <c>PrecisionModel</c> used to determine
        /// the number of decimal places to write.
        /// </param>
        /// <returns>
        /// A <c>NumberFormatInfo</c> that write <c>double</c>
        /// s without scientific notation.
        /// </returns>
        private static NumberFormatInfo CreateFormatter(PrecisionModel precisionModel)
        {
            // the default number of decimal places is 16, which is sufficient
            // to accomodate the maximum precision of a double.
            int decimalPlaces = precisionModel.MaximumSignificantDigits;
            // specify decimal separator explicitly to avoid problems in other locales
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberDecimalDigits = decimalPlaces;
            nfi.NumberGroupSeparator = String.Empty;
            nfi.NumberGroupSizes = new int[] { };
            return nfi;
        }

        /// <summary>
        /// Returns a <c>String</c> of repeated characters.
        /// </summary>
        /// <param name="ch">The character to repeat.</param>
        /// <param name="count">The number of times to repeat the character.</param>
        /// <returns>A <c>string</c> of characters.</returns>
        public static string StringOfChar(char ch, int count)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < count; i++)
                buf.Append(ch);
            return buf.ToString();
        }

        /// <summary>
        /// Converts a <c>Geometry</c> to its Well-known Text representation.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process.</param>
        /// <returns>A Geometry Tagged Text string (see the OpenGIS Simple Features Specification).</returns>
        public virtual string Write(Geometry geometry)
        {
            TextWriter sw = new StringWriter();
            WriteFormatted(geometry, false, sw);
            return sw.ToString();
        }

        /// <summary>
        /// Converts a <c>Geometry</c> to its Well-known Text representation.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process.</param>
        /// <param name="writer"></param>
        public virtual void Write(Geometry geometry, TextWriter writer)
        {
            WriteFormatted(geometry, false, writer);
        }

        /// <summary>
        /// Same as <c>write</c>, but with newlines and spaces to make the
        /// well-known text more readable.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process</param>
        /// <returns>
        /// A "Geometry Tagged Text" string (see the OpenGIS Simple
        /// Features Specification), with newlines and spaces.
        /// </returns>
        public virtual string WriteFormatted(Geometry geometry)
        {
            TextWriter sw = new StringWriter();
            WriteFormatted(geometry, true, sw);
            return sw.ToString();
        }

        /// <summary>
        /// Same as <c>write</c>, but with newlines and spaces to make the
        /// well-known text more readable.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process</param>
        /// <param name="writer"></param>
        public virtual void WriteFormatted(Geometry geometry, TextWriter writer)
        {
            WriteFormatted(geometry, true, writer);
        }

        /// <summary>
        /// Converts a <c>Geometry</c> to its Well-known Text representation.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process</param>
        /// <param name="isFormatted"></param>
        /// <param name="writer"></param>
        private void WriteFormatted(IGeometry geometry, bool isFormatted, TextWriter writer)
        {
            _isFormatted = isFormatted;
            _formatter = CreateFormatter(new PrecisionModel(geometry.PrecisionModel));
            AppendGeometryTaggedText(geometry, 0, writer);
        }

        /// <summary>
        /// Converts a <c>Geometry</c> to &lt;Geometry Tagged Text format,
        /// then appends it to the writer.
        /// </summary>
        /// <param name="geometry">/he <c>Geometry</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">/he output writer to append to.</param>
        private void AppendGeometryTaggedText(IGeometry geometry, int level, TextWriter writer)
        {
            Indent(level, writer);

            if (geometry is Point)
            {
                Point point = (Point)geometry;
                AppendPointTaggedText(point.Coordinate, writer);
            }
            else if (geometry is ILinearRing)
                AppendLinearRingTaggedText((ILinearRing)geometry, level, writer);
            else if (geometry is ILineString)
                AppendLineStringTaggedText((ILineString)geometry, level, writer);
            else if (geometry is IPolygon)
                AppendPolygonTaggedText((IPolygon)geometry, level, writer);
            else if (geometry is IMultiPoint)
                AppendMultiPointTaggedText((IMultiPoint)geometry, writer);
            else if (geometry is IMultiLineString)
                AppendMultiLineStringTaggedText((IMultiLineString)geometry, level, writer);
            else if (geometry is IMultiPolygon)
                AppendMultiPolygonTaggedText((IMultiPolygon)geometry, level, writer);
            else if (geometry is IGeometryCollection)
                AppendGeometryCollectionTaggedText((IGeometryCollection)geometry, level, writer);
            else throw new UnsupportedGeometryException();
        }

        /// <summary>
        /// Converts a <c>Coordinate</c> to Point Tagged Text format,
        /// then appends it to the writer.
        /// </summary>
        /// <param name="coordinate">The <c>Coordinate</c> to process.</param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendPointTaggedText(Coordinate coordinate, TextWriter writer)
        {
            writer.Write("POINT ");
            AppendPointText(coordinate, writer);
        }

        /// <summary>
        /// Converts a <c>LineString</c> to &lt;LineString Tagged Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="lineString">The <c>LineString</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendLineStringTaggedText(IBasicLineString lineString, int level, TextWriter writer)
        {
            writer.Write("LINESTRING ");
            AppendLineStringText(lineString, level, false, writer);
        }

        /// <summary>
        /// Converts a <c>LinearRing</c> to &lt;LinearRing Tagged Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="linearRing">The <c>LinearRing</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendLinearRingTaggedText(ILinearRing linearRing, int level, TextWriter writer)
        {
            writer.Write("LINEARRING ");
            AppendLineStringText(linearRing, level, false, writer);
        }

        /// <summary>
        /// Converts a <c>Polygon</c> to Polygon Tagged Text format,
        /// then appends it to the writer.
        /// </summary>
        /// <param name="polygon">The <c>Polygon</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendPolygonTaggedText(IPolygon polygon, int level, TextWriter writer)
        {
            writer.Write("POLYGON ");
            AppendPolygonText(polygon, level, false, writer);
        }

        /// <summary>
        /// Converts a <c>MultiPoint</c> to &lt;MultiPoint Tagged Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="multipoint">The <c>MultiPoint</c> to process.</param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiPointTaggedText(IMultiPoint multipoint, TextWriter writer)
        {
            writer.Write("MULTIPOINT ");
            AppendMultiPointText(multipoint, writer);
        }

        /// <summary>
        /// Converts a <c>MultiLineString</c> to MultiLineString Tagged
        /// Text format, then appends it to the writer.
        /// </summary>
        /// <param name="multiLineString">The <c>MultiLineString</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiLineStringTaggedText(IMultiLineString multiLineString, int level, TextWriter writer)
        {
            writer.Write("MULTILINESTRING ");
            AppendMultiLineStringText(multiLineString, level, false, writer);
        }

        /// <summary>
        /// Converts a <c>MultiPolygon</c> to MultiPolygon Tagged Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="multiPolygon">The <c>MultiPolygon</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiPolygonTaggedText(IMultiPolygon multiPolygon, int level, TextWriter writer)
        {
            writer.Write("MULTIPOLYGON ");
            AppendMultiPolygonText(multiPolygon, level, writer);
        }

        /// <summary>
        /// Converts a <c>GeometryCollection</c> to GeometryCollection
        /// Tagged Text format, then appends it to the writer.
        /// </summary>
        /// <param name="geometryCollection">The <c>GeometryCollection</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendGeometryCollectionTaggedText(IGeometryCollection geometryCollection, int level,
                                                        TextWriter writer)
        {
            writer.Write("GEOMETRYCOLLECTION ");
            AppendGeometryCollectionText(geometryCollection, level, writer);
        }

        /// <summary>
        /// Converts a <c>Coordinate</c> to Point Text format, then
        /// appends it to the writer.
        /// </summary>
        /// <param name="coordinate">The <c>Coordinate</c> to process.</param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendPointText(Coordinate coordinate, TextWriter writer)
        {
            if (coordinate == null)
                writer.Write("EMPTY");
            else
            {
                writer.Write("(");
                AppendCoordinate(coordinate, writer);
                writer.Write(")");
            }
        }

        /// <summary>
        /// Converts a <c>Coordinate</c> to Point format, then appends
        /// it to the writer.
        /// </summary>
        /// <param name="coordinate">The <c>Coordinate</c> to process.</param>
        /// <param name="writer">The output writer to append to.</param>
        /// The <c>PrecisionModel</c> to use to convert
        /// from a precise coordinate to an external coordinate.
        private void AppendCoordinate(Coordinate coordinate, TextWriter writer)
        {
            writer.Write(WriteNumber(coordinate.X) + " " + WriteNumber(coordinate.Y));
        }

        /// <summary>
        /// Converts a <see cref="double" /> to a <see cref="string" />,
        /// not in scientific notation.
        /// </summary>
        /// <param name="d">The <see cref="double" /> to convert.</param>
        /// <returns>
        /// The <see cref="double" /> as a <see cref="string" />,
        /// not in scientific notation.
        /// </returns>
        private string WriteNumber(double d)
        {
            // return Convert.ToString(d, formatter) not generate decimals well formatted!
            return d.ToString("N", _formatter);
        }

        /// <summary>
        /// Converts a <c>LineString</c> to &lt;LineString Text format, then
        /// appends it to the writer.
        /// </summary>
        /// <param name="lineString">The <c>LineString</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="doIndent"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendLineStringText(IBasicLineString lineString, int level, bool doIndent, TextWriter writer)
        {
            if (lineString.Coordinates.Count == 0)
                writer.Write("EMPTY");
            else
            {
                if (doIndent) Indent(level, writer);
                writer.Write("(");
                for (int i = 0; i < lineString.NumPoints; i++)
                {
                    if (i > 0)
                    {
                        writer.Write(", ");
                        if (i % 10 == 0) Indent(level + 2, writer);
                    }
                    AppendCoordinate(lineString.Coordinates[i], writer);
                }
                writer.Write(")");
            }
        }

        /// <summary>
        /// Converts a <c>Polygon</c> to Polygon Text format, then
        /// appends it to the writer.
        /// </summary>
        /// <param name="polygon">The <c>Polygon</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="indentFirst"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendPolygonText(IPolygon polygon, int level, bool indentFirst, TextWriter writer)
        {
            if (polygon.IsEmpty)
                writer.Write("EMPTY");
            else
            {
                if (indentFirst) Indent(level, writer);
                writer.Write("(");
                AppendLineStringText(polygon.Shell, level, false, writer);
                for (int i = 0; i < polygon.NumHoles; i++)
                {
                    writer.Write(", ");
                    AppendLineStringText(polygon.GetInteriorRingN(i), level + 1, true, writer);
                }
                writer.Write(")");
            }
        }

        /// <summary>
        /// Converts a <c>MultiPoint</c> to &lt;MultiPoint Text format, then
        /// appends it to the writer.
        /// </summary>
        /// <param name="multiPoint">The <c>MultiPoint</c> to process.</param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiPointText(IGeometry multiPoint, TextWriter writer)
        {
            if (multiPoint.IsEmpty)
                writer.Write("EMPTY");
            else
            {
                writer.Write("(");
                for (int i = 0; i < multiPoint.NumGeometries; i++)
                {
                    if (i > 0) writer.Write(", ");
                    AppendCoordinate(((Point)multiPoint.GetGeometryN(i)).Coordinate, writer);
                }
                writer.Write(")");
            }
        }

        /// <summary>
        /// Converts a <c>MultiLineString</c> to &lt;MultiLineString Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="multiLineString">The <c>MultiLineString</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="indentFirst"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiLineStringText(IMultiLineString multiLineString, int level, bool indentFirst,
                                               TextWriter writer)
        {
            if (multiLineString.IsEmpty)
                writer.Write("EMPTY");
            else
            {
                int level2 = level;
                bool doIndent = indentFirst;
                writer.Write("(");
                for (int i = 0; i < multiLineString.NumGeometries; i++)
                {
                    if (i > 0)
                    {
                        writer.Write(", ");
                        level2 = level + 1;
                        doIndent = true;
                    }
                    AppendLineStringText((LineString)multiLineString.GetGeometryN(i), level2, doIndent, writer);
                }
                writer.Write(")");
            }
        }

        /// <summary>
        /// Converts a <c>MultiPolygon</c> to &lt;MultiPolygon Text format,
        /// then appends it to the writer.
        /// </summary>
        /// <param name="multiPolygon">The <c>MultiPolygon</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiPolygonText(IMultiPolygon multiPolygon, int level, TextWriter writer)
        {
            if (multiPolygon.IsEmpty)
                writer.Write("EMPTY");
            else
            {
                int level2 = level;
                bool doIndent = false;
                writer.Write("(");
                for (int i = 0; i < multiPolygon.NumGeometries; i++)
                {
                    if (i > 0)
                    {
                        writer.Write(", ");
                        level2 = level + 1;
                        doIndent = true;
                    }
                    AppendPolygonText((Polygon)multiPolygon.GetGeometryN(i), level2, doIndent, writer);
                }
                writer.Write(")");
            }
        }

        /// <summary>
        /// Converts a <c>GeometryCollection</c> to GeometryCollectionText
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="geometryCollection">The <c>GeometryCollection</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendGeometryCollectionText(IGeometryCollection geometryCollection, int level, TextWriter writer)
        {
            if (geometryCollection.IsEmpty)
                writer.Write("EMPTY");
            else
            {
                int level2 = level;
                writer.Write("(");
                for (int i = 0; i < geometryCollection.NumGeometries; i++)
                {
                    if (i > 0)
                    {
                        writer.Write(", ");
                        level2 = level + 1;
                    }
                    AppendGeometryTaggedText(geometryCollection.GetGeometryN(i), level2, writer);
                }
                writer.Write(")");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        /// <param name="writer"></param>
        private void Indent(int level, TextWriter writer)
        {
            if (!_isFormatted || level <= 0) return;
            writer.Write("\n");
            writer.Write(StringOfChar(' ', WKT_WRITER_INDENT * level));
        }
    }
}