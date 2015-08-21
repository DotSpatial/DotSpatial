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
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.IO
{
    /// <summary> 
    /// Outputs the textual representation of a <see cref="Geometry" />.
    /// The <see cref="WKTWriter" /> outputs coordinates rounded to the precision
    /// model. No more than the maximum number of necessary decimal places will be
    /// output.
    /// The Well-known Text format is defined in the <A
    /// HREF="http://www.opengis.org/techno/specs.htm">OpenGIS Simple Features
    /// Specification for SQL</A>.
    /// A non-standard "LINEARRING" tag is used for LinearRings. The WKT spec does
    /// not define a special tag for LinearRings. The standard tag to use is
    /// "LINESTRING".
    /// </summary>
    public class WKTWriter : ITextGeometryWriter
    {
        #region Constant Fields

        private const string MaxPrecisionFormat = "{0:R}";

        #endregion

        #region Fields

        private readonly int _outputDimension = 2;
        private string _format;
        private NumberFormatInfo _formatter;
        private string _indentTabStr = "  ";
        private bool _useFormating;
        private bool _useMaxPrecision;

        #endregion

        #region Constructors

        public WKTWriter()
        {
            MaxCoordinatesPerLine = -1;
        }

        public WKTWriter(int outputDimension)
        {
            MaxCoordinatesPerLine = -1;
            if (outputDimension < 2 || outputDimension > 3)
                throw new ArgumentException(TopologyText.WktWriter_WrongOutputDimension, "outputDimension");
            _outputDimension = outputDimension;
        }

        #endregion

        #region Properties

        public Ordinates AllowedOrdinates
        {
            get { return Ordinates.XYZM; }
        }

        public bool EmitM { get; set; }
        public bool EmitSrid { get; set; }
        public bool EmitZ { get; set; }

        ///<summary>
        /// Gets/sets whther the output will be formatted.
        ///</summary>
        public bool Formatted { get; set; }

        public Ordinates HandleOrdinates
        {
            get
            {
                Ordinates ret = Ordinates.XY;
                if (EmitZ) ret |= Ordinates.Z;
                if (EmitM) ret |= Ordinates.M;
                return ret;
            }
            set
            {
                value &= AllowedOrdinates;
                if ((value & Ordinates.Z) != 0) EmitZ = true;
                if ((value & Ordinates.M) != 0) EmitM = true;
            }
        }

        public bool HandleSrid
        {
            get { return EmitSrid; }
            set { EmitSrid = value; }
        }

        ///<summary>
        /// Gets/sets the maximum number of coordinates per line written in formatted output.
        ///</summary>
        /// <remarks>If the provided coordinate number is &lt; 0, coordinates will be written all on one line.</remarks>
        public int MaxCoordinatesPerLine { get; set; }

        ///<summary>Gets/sets the tab size to use for indenting.</summary>
        /// <exception cref="ArgumentException">If the size is non-positive</exception>
        public int Tab
        {
            get { return _indentTabStr.Length; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(TopologyText.WktWriter_NegativeTabCount, "value");
                _indentTabStr = StringOfChar(' ', value);
            }
        }

        #endregion

        #region Methods

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
        /// Generates the WKT for a N-point <c>LineString</c> specified by a <see cref="ICoordinateSequence"/>.
        /// </summary>
        /// <param name="seq">The sequence to write.</param>
        /// <returns>The WKT</returns>
        public static string ToLineString(ICoordinateSequence seq)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("LINESTRING");
            if (seq.Count == 0)
                buf.Append(" EMPTY");
            else
            {
                buf.Append("(");
                for (int i = 0; i < seq.Count; i++)
                {
                    if (i > 0) buf.Append(", ");
                    buf.Append(string.Format(CultureInfo.InvariantCulture, "{0} {1}", seq.GetX(i), seq.GetY(i)));
                }
                buf.Append(")");
            }
            return buf.ToString();
        }

        /// <summary>
        /// Generates the WKT for a <c>LineString</c> specified by two <see cref="Coordinate"/>s.
        /// </summary>
        /// <param name="p0">The first coordinate.</param>
        /// <param name="p1">The second coordinate.</param>
        /// <returns>The WKT</returns>
        public static string ToLineString(Coordinate p0, Coordinate p1)
        {
#if LikeJTS
            return String.Format(CultureInfo.InvariantCulture, "LINESTRING({0:R} {1:R}, {2:R} {3:R})", p0.X, p0.Y, p1.X, p1.Y);
#else
            if (double.IsNaN(p0.Z))
                return string.Format(CultureInfo.InvariantCulture, "LINESTRING({0} {1}, {2} {3})", p0.X, p0.Y, p1.X, p1.Y);
            return string.Format(CultureInfo.InvariantCulture, "LINESTRING({0} {1} {2}, {3} {4} {5})", p0.X, p0.Y, p0.Z, p1.X, p1.Y, p1.Z);
#endif
        }

        /// <summary>
        /// Generates the WKT for a <c>Point</c> specified by a <see cref="Coordinate"/>.
        /// </summary>
        /// <param name="p0">The point coordinate.</param>
        /// <returns>The WKT</returns>
        public static string ToPoint(Coordinate p0)
        {
#if LikeJTS
            return String.Format(CultureInfo.InvariantCulture, "POINT({0} {1})", p0.X, p0.Y);
#else
            if (double.IsNaN(p0.Z))
                return string.Format(CultureInfo.InvariantCulture, "POINT({0} {1})", p0.X, p0.Y);
            return string.Format(CultureInfo.InvariantCulture, "POINT({0} {1} {2})", p0.X, p0.Y, p0.Z);
#endif
        }

        /// <summary>
        /// Converts a <c>Geometry</c> to its Well-known Text representation.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process.</param>
        /// <returns>A Geometry Tagged Text string (see the OpenGIS Simple Features Specification).</returns>
        public virtual string Write(IGeometry geometry)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter sw = new StringWriter(sb);
            TryWrite(geometry, sw);
            return sb.ToString();
        }

        /// <summary>
        /// Converts a <c>Geometry</c> to its Well-known Text representation.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process.</param>
        /// <param name="stream">A <c>Stream</c> to write into</param>
        public void Write(IGeometry geometry, Stream stream)
        {
            TextWriter sw = new StreamWriter(stream);
            TryWrite(geometry, sw);
        }

        /// <summary>
        /// Converts a <c>Geometry</c> to its Well-known Text representation.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process.</param>
        /// <param name="writer"></param>
        /// <returns>A "Geometry Tagged Text" string (see the OpenGIS Simple Features Specification)</returns>
        public virtual void Write(IGeometry geometry, TextWriter writer)
        {
            WriteFormatted(geometry, Formatted, writer);
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
        public virtual string WriteFormatted(IGeometry geometry)
        {
            TextWriter sw = new StringWriter();
            try
            {
                WriteFormatted(geometry, true, sw);
            }
            catch (IOException)
            {
                Assert.ShouldNeverReachHere();
            }
            return sw.ToString();
        }

        /// <summary>
        /// Same as <c>write</c>, but with newlines and spaces to make the
        /// well-known text more readable.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process</param>
        /// <param name="writer"></param>
        /// <returns>
        /// A Geometry Tagged Text string (see the OpenGIS Simple
        /// Features Specification), with newlines and spaces.
        /// </returns>
        public virtual void WriteFormatted(IGeometry geometry, TextWriter writer)
        {
            WriteFormatted(geometry, true, writer);
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
        /// A <c>NumberFormatInfo</c> that write <c>double</c>s 
        /// without scientific notation.
        /// </returns>        
        internal static NumberFormatInfo CreateFormatter(IPrecisionModel precisionModel)
        {
            // the default number of decimal places is 16, which is sufficient
            // to accomodate the maximum precision of a double.
            int digits = precisionModel.MaximumSignificantDigits;
            int decimalPlaces = Math.Max(0, digits); // negative values not allowed

            // specify decimal separator explicitly to avoid problems in other locales
            NumberFormatInfo nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = decimalPlaces,
                NumberGroupSeparator = string.Empty,
                NumberGroupSizes = new int[] { }
            };
            return nfi;
        }

        ///<summary>Appends the i'th coordinate from the sequence to the writer</summary>
        /// <param name="seq">the <see cref="ICoordinateSequence"/> to process</param>
        /// <param name="i">the index of the coordinate to write</param>
        /// <param name="writer">writer the output writer to append to</param>
        ///<exception cref="IOException"></exception>
        private void AppendCoordinate(ICoordinateSequence seq, int i, TextWriter writer)
        {
            writer.Write(WriteNumber(seq.GetX(i)) + " " + WriteNumber(seq.GetY(i)));
            if (_outputDimension >= 3 && seq.Dimension >= 3)
            {
                double z = seq.GetOrdinate(i, Ordinate.Z);
                if (!double.IsNaN(z))
                {
                    writer.Write(" ");
                    writer.Write(WriteNumber(z));
                }
            }
        }

        /// <summary>
        /// Converts a <c>Coordinate</c> to Point format, then appends
        /// it to the writer.
        /// </summary>
        /// <param name="coordinate">The <c>Coordinate</c> to process.</param>
        /// <param name="writer">The output writer to append to.</param>
        /// <param name="precisionModel">
        /// The <c>PrecisionModel</c> to use to convert
        /// from a precise coordinate to an external coordinate.
        /// </param>
        private void AppendCoordinate(Coordinate coordinate, TextWriter writer, IPrecisionModel precisionModel)
        {
            writer.Write(WriteNumber(coordinate.X) + " " + WriteNumber(coordinate.Y));
            if (_outputDimension >= 3 && !double.IsNaN(coordinate.Z))
            {
                writer.Write(" " + WriteNumber(coordinate.Z));
            }
        }

        /// <summary>
        /// Converts a <c>GeometryCollection</c> to GeometryCollection
        /// Tagged Text format, then appends it to the writer.
        /// </summary>
        /// <param name="geometryCollection">The <c>GeometryCollection</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendGeometryCollectionTaggedText(IGeometryCollection geometryCollection, int level, TextWriter writer)
        {
            writer.Write("GEOMETRYCOLLECTION ");
            AppendGeometryCollectionText(geometryCollection, level, writer);
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
        /// Converts a <c>Geometry</c> to &lt;Geometry Tagged Text format,
        /// then appends it to the writer.
        /// </summary>
        /// <param name="geometry">/he <c>Geometry</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">/he output writer to append to.</param>
        private void AppendGeometryTaggedText(IGeometry geometry, int level, TextWriter writer)
        {
            Indent(level, writer);

            if (geometry is IPoint)
            {
                IPoint point = (IPoint)geometry;
                AppendPointTaggedText(point.Coordinate, level, writer, point.PrecisionModel);
            }
            else if (geometry is ILinearRing)
                AppendLinearRingTaggedText((ILinearRing)geometry, level, writer);
            else if (geometry is ILineString)
                AppendLineStringTaggedText((ILineString)geometry, level, writer);
            else if (geometry is IPolygon)
                AppendPolygonTaggedText((IPolygon)geometry, level, writer);
            else if (geometry is IMultiPoint)
                AppendMultiPointTaggedText((IMultiPoint)geometry, level, writer);
            else if (geometry is IMultiLineString)
                AppendMultiLineStringTaggedText((IMultiLineString)geometry, level, writer);
            else if (geometry is IMultiPolygon)
                AppendMultiPolygonTaggedText((IMultiPolygon)geometry, level, writer);
            else if (geometry is IGeometryCollection)
                AppendGeometryCollectionTaggedText((IGeometryCollection)geometry, level, writer);
            else throw new UnsupportedGeometryException();
        }

        /// <summary>
        /// Converts a <c>LinearRing</c> to &lt;LinearRing Tagged Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="linearRing">The <c>LinearRing</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendLinearRingTaggedText(ILineString linearRing, int level, TextWriter writer)
        {
            writer.Write("LINEARRING ");
            AppendLineStringText(linearRing, level, false, writer);
        }

        /// <summary>
        /// Converts a <c>LineString</c> to &lt;LineString Tagged Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="lineString">The <c>LineString</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendLineStringTaggedText(ILineString lineString, int level, TextWriter writer)
        {
            writer.Write("LINESTRING ");
            AppendLineStringText(lineString, level, false, writer);
        }

        /// <summary>
        /// Converts a <c>LineString</c> to &lt;LineString Text format, then
        /// appends it to the writer.
        /// </summary>
        /// <param name="lineString">The <c>LineString</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="doIndent"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendLineStringText(ILineString lineString, int level, bool doIndent, TextWriter writer)
        {
            if (lineString.IsEmpty)
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
                        if (MaxCoordinatesPerLine > 0
                            && i % MaxCoordinatesPerLine == 0)
                        {
                            Indent(level + 1, writer);
                        }
                    }
                    AppendCoordinate(lineString.GetCoordinateN(i), writer, lineString.PrecisionModel);
                }
                writer.Write(")");
            }
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
        /// Converts a <c>MultiLineString</c> to &lt;MultiLineString Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="multiLineString">The <c>MultiLineString</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="indentFirst"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiLineStringText(IMultiLineString multiLineString, int level, bool indentFirst, TextWriter writer)
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
                    AppendLineStringText((ILineString)multiLineString.GetGeometryN(i), level2, doIndent, writer);
                }
                writer.Write(")");
            }
        }

        /// <summary>
        /// Converts a <c>MultiPoint</c> to &lt;MultiPoint Tagged Text
        /// format, then appends it to the writer.
        /// </summary>
        /// <param name="multipoint">The <c>MultiPoint</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiPointTaggedText(IMultiPoint multipoint, int level, TextWriter writer)
        {
            writer.Write("MULTIPOINT ");
            AppendMultiPointText(multipoint, level, writer);
        }

        /// <summary>
        /// Converts a <c>MultiPoint</c> to &lt;MultiPoint Text format, then
        /// appends it to the writer.
        /// </summary>
        /// <param name="multiPoint">The <c>MultiPoint</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        private void AppendMultiPointText(IMultiPoint multiPoint, int level, TextWriter writer)
        {
            if (multiPoint.IsEmpty)
                writer.Write("EMPTY");
            else
            {
                writer.Write("(");
                for (int i = 0; i < multiPoint.NumGeometries; i++)
                {
                    if (i > 0)
                    {
                        writer.Write(", ");
                        IndentCoords(i, level + 1, writer);
                    }
                    writer.Write("(");
                    AppendCoordinate((multiPoint.GetGeometryN(i)).Coordinate, writer, multiPoint.PrecisionModel);
                    writer.Write(")");
                }
                writer.Write(")");
            }
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
                    AppendPolygonText((IPolygon)multiPolygon.GetGeometryN(i), level2, doIndent, writer);
                }
                writer.Write(")");
            }
        }

        /// <summary>
        /// Converts a <c>Coordinate</c> to Point Tagged Text format,
        /// then appends it to the writer.
        /// </summary>
        /// <param name="coordinate">The <c>Coordinate</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        /// <param name="precisionModel"> 
        /// The <c>PrecisionModel</c> to use to convert
        /// from a precise coordinate to an external coordinate.
        /// </param>
        private void AppendPointTaggedText(Coordinate coordinate, int level, TextWriter writer, IPrecisionModel precisionModel)
        {
            writer.Write("POINT ");
            AppendPointText(coordinate, level, writer, precisionModel);
        }

        /// <summary>
        /// Converts a <c>Coordinate</c> to Point Text format, then
        /// appends it to the writer.
        /// </summary>
        /// <param name="coordinate">The <c>Coordinate</c> to process.</param>
        /// <param name="level"></param>
        /// <param name="writer">The output writer to append to.</param>
        /// <param name="precisionModel">
        /// The <c>PrecisionModel</c> to use to convert
        /// from a precise coordinate to an external coordinate.
        /// </param>
        private void AppendPointText(Coordinate coordinate, int level, TextWriter writer, IPrecisionModel precisionModel)
        {
            if (coordinate == null)
                writer.Write("EMPTY");
            else
            {
                writer.Write("(");
                AppendCoordinate(coordinate, writer, precisionModel);
                writer.Write(")");
            }
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

        ///<summary>Converts a <see cref="ICoordinateSequence"/> to &lt;LineString Text&gt; format, then appends it to the writer</summary>
        ///<exception cref="IOException"></exception>
        private void AppendSequenceText(ICoordinateSequence seq, int level, bool doIndent, TextWriter writer)
        {
            if (seq.Count == 0)
            {
                writer.Write("EMPTY");
            }
            else
            {
                if (doIndent) Indent(level, writer);
                writer.Write("(");
                for (int i = 0; i < seq.Count; i++)
                {
                    if (i > 0)
                    {
                        writer.Write(", ");
                        if (MaxCoordinatesPerLine > 0
                            && i % MaxCoordinatesPerLine == 0)
                        {
                            Indent(level + 1, writer);
                        }
                    }
                    AppendCoordinate(seq, i, writer);
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
            if (!_useFormating || level <= 0) return;
            writer.Write("\n");
            for (int i = 0; i < level; i++)
                writer.Write(_indentTabStr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordIndex"></param>
        /// <param name="level"></param>
        /// <param name="writer"></param>
        /// <exception cref="IOException"></exception>
        private void IndentCoords(int coordIndex, int level, TextWriter writer)
        {
            if (MaxCoordinatesPerLine <= 0 || coordIndex % MaxCoordinatesPerLine != 0)
                return;
            Indent(level, writer);
        }

        private void TryWrite(IGeometry geometry, TextWriter sw)
        {
            try
            {
                WriteFormatted(geometry, Formatted, sw);
            }
            catch (IOException)
            {
                Assert.ShouldNeverReachHere();
            }
        }

        /// <summary>
        /// Converts a <c>Geometry</c> to its Well-known Text representation.
        /// </summary>
        /// <param name="geometry">A <c>Geometry</c> to process</param>
        /// <param name="useFormatting"></param>
        /// <param name="writer"></param>
        /// <returns>
        /// A "Geometry Tagged Text" string (see the OpenGIS Simple
        /// Features Specification).
        /// </returns>
        private void WriteFormatted(IGeometry geometry, bool useFormatting, TextWriter writer)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry");

            _useFormating = useFormatting;
            // Enable maxPrecision (via {0:R} formatter) in WriteNumber method
            IPrecisionModel precisionModel = geometry.Factory.PrecisionModel;
            _useMaxPrecision = precisionModel.PrecisionModelType == PrecisionModelType.Floating;

            _formatter = CreateFormatter(geometry.PrecisionModel);
            _format = "0." + StringOfChar('#', _formatter.NumberDecimalDigits);
            AppendGeometryTaggedText(geometry, 0, writer);

            // Disable maxPrecision as default setting
            _useMaxPrecision = false;
        }

        /// <summary>
        /// Converts a <see cref="double" /> to a <see cref="string" />.
        /// </summary>
        /// <param name="d">The <see cref="double" /> to convert.</param>
        /// <returns>
        /// The <see cref="double" /> as a <see cref="string" />.
        /// </returns>        
        private string WriteNumber(double d)
        {
            string standard = d.ToString(_format, _formatter);
            if (!_useMaxPrecision)  return standard;
            try
            {
                double converted = Convert.ToDouble(standard, _formatter);
                // check if some precision is lost during text conversion: if so, use {0:R} formatter
                return converted == d ? standard : string.Format(_formatter, MaxPrecisionFormat, d);
            }
            catch (OverflowException)
            {
                // Use MaxPrecisionFormat anyway
                return string.Format(_formatter, MaxPrecisionFormat, d);
            }
        }

        #endregion
    }
}