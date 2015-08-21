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
using System.IO;
using System.Text;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.IO
{
    /// <summary>
    /// Writes a Well-Known Binary byte data representation of a <c>Geometry</c>.
    /// </summary>
    /// <remarks>
    /// WKBWriter stores <see cref="Coordinate" /> X,Y,Z values if <see cref="Coordinate.Z" /> is not <see cref="double.NaN"/>,
    /// otherwise <see cref="Coordinate.Z" /> value is discarded and only X,Y are stored.
    /// </remarks>
    // Thanks to Roberto Acioli for Coordinate.Z patch
    public class WKBWriter : IBinaryGeometryWriter
    {
        #region Fields

        protected ByteOrder EncodingType;
        private int _coordinateSize = 16;
        private bool _emitM;
        private bool _emitZ;
        private Ordinates _handleOrdinates;
        private bool _handleSrid;
        private bool _strict = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes writer with LittleIndian byte order.
        /// </summary>
        public WKBWriter() :
            this(ByteOrder.LittleEndian, false) { }

        /// <summary>
        /// Initializes writer with the specified byte order.
        /// </summary>
        /// <param name="encodingType">Encoding type</param>
        public WKBWriter(ByteOrder encodingType) :
            this(encodingType, false)
        {
        }

        /// <summary>
        /// Initializes writer with the specified byte order.
        /// </summary>
        /// <param name="encodingType">Encoding type</param>
        /// <param name="handleSrid">SRID values, present or not, should be emitted.</param>
        public WKBWriter(ByteOrder encodingType, bool handleSrid) :
            this(encodingType, handleSrid, false)
        {
        }

        /// <summary>
        /// Initializes writer with the specified byte order.
        /// </summary>
        /// <param name="encodingType">Encoding type</param>
        /// <param name="handleSrid">SRID values, present or not, should be emitted.</param>
        /// <param name="emitZ">Z values, present or not, should be emitted</param>
        public WKBWriter(ByteOrder encodingType, bool handleSrid, bool emitZ) :
            this(encodingType, handleSrid, emitZ, false)
        {
        }

        /// <summary>
        /// Initializes writer with the specified byte order.
        /// </summary>
        /// <param name="encodingType">Encoding type</param>
        /// <param name="handleSrid">SRID values, present or not, should be emitted.</param>
        /// <param name="emitZ">Z values, present or not, should be emitted</param>
        /// <param name="emitM">M values, present or not, should be emitted</param>
        public WKBWriter(ByteOrder encodingType, bool handleSrid, bool emitZ, bool emitM)
        {
            EncodingType = encodingType;
            
            //Allow setting of HandleSRID
            if (handleSrid)_strict = false;
            HandleSrid = handleSrid;

            var handleOrdinates = Ordinates.XY;
            if (emitZ)
                handleOrdinates |= Ordinates.Z;
            if (emitM)
                handleOrdinates |= Ordinates.M;

            _handleOrdinates = handleOrdinates;
            CalcCoordinateSize();
        }

        #endregion

        #region Properties

        public Ordinates AllowedOrdinates
        {
            get { return Ordinates.XYZM; }
        }

        public ByteOrder ByteOrder
        {
            get { return EncodingType; }
            set { }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="ICoordinate.M"/> values should be emitted
        /// </summary>
        [Obsolete("Use HandleOrdintes instead.")]
        public bool EmitM
        {
            get { return _emitM; }
            set
            {
                if (value == _emitM)
                    return;

                _emitM = value;

                if (value)
                    HandleOrdinates |= Ordinates.M;
                else
                    HandleOrdinates &= ~Ordinates.M;

                CalcCoordinateSize();
            }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="IGeometry.Srid"/> value should be emitted
        /// </summary>
        [Obsolete("Use HandleSRID instead")]
        public bool EmitSrid
        {
            get { return HandleSrid; }
            set { HandleSrid = value; }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Coordinate.Z"/> values should be emitted
        /// </summary>
        [Obsolete("Use HandleOrdinates instead")]
        public bool EmitZ
        {
            get { return _emitZ; }
            set
            {
                if (value == _emitZ)
                    return;
                _emitZ = value;

                if (value)
                    HandleOrdinates |= Ordinates.Z;
                else
                    HandleOrdinates &= ~Ordinates.Z;

                CalcCoordinateSize();
            }
        }

        public Ordinates HandleOrdinates
        {
            get { return _handleOrdinates; }
            set
            {
                value = Ordinates.XY | AllowedOrdinates & value;
                if (value == _handleOrdinates)
                    return;

                _handleOrdinates = value;
                _emitZ = (value & Ordinates.Z) != 0;
                _emitM = (value & Ordinates.M) != 0;
                CalcCoordinateSize();
            }
        }

        public bool HandleSrid
        {
            get { return _handleSrid; }
            set
            {
                if (_strict && value)
                    throw new ArgumentException("Cannot set HandleSRID to true if Strict is set", "value");
                _handleSrid = value;
            }
        }

        /// <summary>
        /// Gets a value whether or not EWKB featues may be used.
        /// <para/>EWKB features are
        /// <list type="Bullet"><item>0x80000000 flag if geometry's z-ordinate values are written</item>
        /// <item>0x40000000 flag if geometry's m-ordinate values are written</item>
        /// <item>0x20000000 flag if geometry's SRID value is written</item></list>
        /// </summary>
        public bool Strict
        {
            get { return _strict; }
            set
            {
                _strict = value;
                if (_strict)
                    HandleSrid = false;
            }
        }

        /// <summary>
        /// Standard byte size for each complex point.
        /// Each complex point (LineString, Polygon, ...) contains:
        ///     1 byte for ByteOrder and
        ///     4 bytes for WKBType.
        ///     4 bytes for SRID value
        /// </summary>
        protected int InitCount { get { return 5 + (HandleSrid ? 4 : 0); } }

        #endregion

        #region Methods

        ///<summary>Converts a byte array to a hexadecimal string.</summary>
        /// <param name="bytes">A byte array</param>
        [Obsolete("Use ToHex(byte[])")]
        public static String BytesToHex(byte[] bytes)
        {
            return ToHex(bytes);
        }

        ///<summary>Converts a byte array to a hexadecimal string.</summary>
        /// <param name="bytes">A byte array</param>
        public static String ToHex(byte[] bytes)
        {
            var buf = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                buf.Append(ToHexDigit((b >> 4) & 0x0F));
                buf.Append(ToHexDigit(b & 0x0F));
            }
            return buf.ToString();
        }

        /// <summary>
        /// Writes a WKB representation of a given point.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public virtual byte[] Write(IGeometry geometry)
        {
            byte[] bytes = GetBytes(geometry);
            Write(geometry, new MemoryStream(bytes));
            return bytes;
        }

        /// <summary>
        /// Writes a WKB representation of a given point.
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual void Write(IGeometry geometry, Stream stream)
        {
            BinaryWriter writer = null;
            try
            {
                writer = EncodingType == ByteOrder.LittleEndian
                    ? new BinaryWriter(stream)
                    : new BEBinaryWriter(stream);
                Write(geometry, writer);
            }
            finally
            {
                if (writer != null)
                    ((IDisposable)writer).Dispose();
            }
        }

        /// <summary>
        /// Sets corrent length for Byte Stream.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected byte[] GetBytes(IGeometry geometry)
        {
            if (geometry is IPoint)
                return new byte[SetByteStream(geometry as IPoint)];
            if (geometry is ILineString)
                return new byte[SetByteStream(geometry as ILineString)];
            if (geometry is IPolygon)
                return new byte[SetByteStream(geometry as IPolygon)];
            if (geometry is IMultiPoint)
                return new byte[SetByteStream(geometry as IMultiPoint)];
            if (geometry is IMultiLineString)
                return new byte[SetByteStream(geometry as IMultiLineString)];
            if (geometry is IMultiPolygon)
                return new byte[SetByteStream(geometry as IMultiPolygon)];
            if (geometry is IGeometryCollection)
                return new byte[SetByteStream(geometry as IGeometryCollection)];
            throw new ArgumentException("ShouldNeverReachHere");
        }

        /// <summary>
        /// Sets corrent length for Byte Stream.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IGeometry geometry)
        {
            if (geometry is IPoint)
                return SetByteStream(geometry as IPoint);
            if (geometry is ILineString)
                return SetByteStream(geometry as ILineString);
            if (geometry is IPolygon)
                return SetByteStream(geometry as IPolygon);
            if (geometry is IMultiPoint)
                return SetByteStream(geometry as IMultiPoint);
            if (geometry is IMultiLineString)
                return SetByteStream(geometry as IMultiLineString);
            if (geometry is IMultiPolygon)
                return SetByteStream(geometry as IMultiPolygon);
            if (geometry is IGeometryCollection)
                return SetByteStream(geometry as IGeometryCollection);
            throw new ArgumentException("ShouldNeverReachHere");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected int SetByteStream(IGeometryCollection geometry)
        {
            int count = InitCount;
            count += 4;
            foreach (var geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected int SetByteStream(IMultiPolygon geometry)
        {
            int count = InitCount;
            count += 4;
            foreach (IPolygon geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected int SetByteStream(IMultiLineString geometry)
        {
            int count = InitCount;
            count += 4;
            foreach (ILineString geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected int SetByteStream(IMultiPoint geometry)
        {
            int count = InitCount;
            count += 4;     // NumPoints
            foreach (IPoint geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected int SetByteStream(IPolygon geometry)
        {
            var pointSize = _coordinateSize; //Double.IsNaN(geometry.Coordinate.Z) ? 16 : 24;
            var count = InitCount;
            count += 4 /*+ 4*/;                                 // NumRings /*+ NumPoints */
            count += 4 * (geometry.NumHoles + 1);   // Index parts
            count += geometry.NumPoints * pointSize;        // Points in exterior and interior rings
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected int SetByteStream(ILineString geometry)
        {
            int pointSize = _coordinateSize; //Double.IsNaN(geometry.Coordinate.Z) ? 16 : 24;
            int numPoints = geometry.NumPoints;
            int count = InitCount;
            count += 4;                             // NumPoints
            count += pointSize * numPoints;
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected int SetByteStream(IPoint geometry)
        {
            return InitCount + _coordinateSize;
            //return Double.IsNaN(geometry.Coordinate.Z) ? 21 : 29;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="writer"></param>
        protected void Write(IGeometry geometry, BinaryWriter writer)
        {
            if (geometry is IPoint)
                Write(geometry as IPoint, writer);
            else if (geometry is ILineString)
                Write(geometry as ILineString, writer);
            else if (geometry is IPolygon)
                Write(geometry as IPolygon, writer);
            else if (geometry is IMultiPoint)
                Write(geometry as IMultiPoint, writer);
            else if (geometry is IMultiLineString)
                Write(geometry as IMultiLineString, writer);
            else if (geometry is IMultiPolygon)
                Write(geometry as IMultiPolygon, writer);
            else if (geometry is IGeometryCollection)
                Write(geometry as IGeometryCollection, writer);
            else throw new ArgumentException("Geometry not recognized: " + geometry);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="writer"></param>
        protected void Write(Coordinate coordinate, BinaryWriter writer)
        {
            writer.Write(coordinate.X);
            writer.Write(coordinate.Y);
            if ((HandleOrdinates & Ordinates.Z) == Ordinates.Z)
                writer.Write(coordinate.Z);
            if ((HandleOrdinates & Ordinates.M) == Ordinates.M)
                //NOTE: Implement
                writer.Write(Double.NaN);
        }

        protected void Write(ICoordinateSequence sequence, bool emitSize, BinaryWriter writer)
        {
            if (emitSize)
                writer.Write(sequence.Count);

            // zm-values if not provided by sequence
            var ordinateZ = Coordinate.NullOrdinate;
            var ordinateM = Coordinate.NullOrdinate;

            // test if zm-values are provided by sequence
            var getZ = (sequence.Ordinates & Ordinates.Z) == Ordinates.Z;
            var getM = (sequence.Ordinates & Ordinates.M) == Ordinates.M;

            // test if zm-values should be emitted
            var writeZ = (HandleOrdinates & Ordinates.Z) == Ordinates.Z;
            var writeM = (HandleOrdinates & Ordinates.M) == Ordinates.M;

            for (var index = 0; index < sequence.Count; index++)
            {
                writer.Write(sequence.GetOrdinate(index, Ordinate.X));
                writer.Write(sequence.GetOrdinate(index, Ordinate.Y));
                if (writeZ)
                {
                    if (getZ) ordinateZ = sequence.GetOrdinate(index, Ordinate.Z);
                    writer.Write(ordinateZ);
                }
                if (writeM)
                {
                    if (getM) ordinateM = sequence.GetOrdinate(index, Ordinate.M);
                    writer.Write(ordinateM);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        /// <param name="writer"></param>
        protected void Write(IPoint point, BinaryWriter writer)
        {
            ////WriteByteOrder(writer);     // LittleIndian
            WriteHeader(writer, point);
            ////if (Double.IsNaN(point.Coordinate.Z))
            ////     writer.Write((int)WKBGeometryTypes.WKBPoint);
            ////else writer.Write((int)WKBGeometryTypes.WKBPointZ);
            //Write(point.Coordinate, writer);
            Write(point.CoordinateSequence, false, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lineString"></param>
        /// <param name="writer"></param>
        protected void Write(ILineString lineString, BinaryWriter writer)
        {
            ////WriteByteOrder(writer);     // LittleIndian
            WriteHeader(writer, lineString);
            ////if (Double.IsNaN(lineString.Coordinate.Z))
            ////     writer.Write((int)WKBGeometryTypes.WKBLineString);
            ////else writer.Write((int)WKBGeometryTypes.WKBLineStringZ);
            //writer.Write(lineString.NumPoints);
            //for (int i = 0; i < lineString.Coordinates.Length; i++)
            //    Write(lineString.Coordinates[i], writer);
            Write(lineString.CoordinateSequence, true, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="writer"></param>
        protected void Write(ILinearRing ring, BinaryWriter writer)
        {
            Write(ring.CoordinateSequence, true, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="writer"></param>
        protected void Write(IPolygon polygon, BinaryWriter writer)
        {
            WriteHeader(writer, polygon);
            writer.Write(polygon.NumHoles + 1);
            Write(polygon.Shell as ILinearRing, writer);
            for (int i = 0; i < polygon.NumHoles; i++)
                Write(polygon.Holes[i] as ILinearRing, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiPoint"></param>
        /// <param name="writer"></param>
        protected void Write(IMultiPoint multiPoint, BinaryWriter writer)
        {
            WriteHeader(writer, multiPoint);
            writer.Write(multiPoint.NumGeometries);
            for (var i = 0; i < multiPoint.NumGeometries; i++)
                Write(multiPoint.Geometries[i] as IPoint, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiLineString"></param>
        /// <param name="writer"></param>
        protected void Write(IMultiLineString multiLineString, BinaryWriter writer)
        {
            //WriteByteOrder(writer);     // LittleIndian
            WriteHeader(writer, multiLineString);
            //if (Double.IsNaN(multiLineString.Coordinate.Z))
            //     writer.Write((int)WKBGeometryTypes.WKBMultiLineString);
            //else writer.Write((int)WKBGeometryTypes.WKBMultiLineStringZ);
            writer.Write(multiLineString.NumGeometries);
            for (var i = 0; i < multiLineString.NumGeometries; i++)
                Write(multiLineString.Geometries[i] as ILineString, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiPolygon"></param>
        /// <param name="writer"></param>
        protected void Write(IMultiPolygon multiPolygon, BinaryWriter writer)
        {
            //WriteByteOrder(writer);     // LittleIndian
            WriteHeader(writer, multiPolygon);
            //if (Double.IsNaN(multiPolygon.Coordinate.Z))
            //     writer.Write((int)WKBGeometryTypes.WKBMultiPolygon);
            //else writer.Write((int)WKBGeometryTypes.WKBMultiPolygonZ);
            writer.Write(multiPolygon.NumGeometries);
            for (var i = 0; i < multiPolygon.NumGeometries; i++)
                Write(multiPolygon.Geometries[i] as IPolygon, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomCollection"></param>
        /// <param name="writer"></param>
        protected void Write(IGeometryCollection geomCollection, BinaryWriter writer)
        {
            //WriteByteOrder(writer);     // LittleIndian
            WriteHeader(writer, geomCollection);
            //if (Double.IsNaN(geomCollection.Coordinate.Z))
            //     writer.Write((int)WKBGeometryTypes.WKBGeometryCollection);
            //else writer.Write((int)WKBGeometryTypes.WKBGeometryCollectionZ);
            writer.Write(geomCollection.NumGeometries);
            for (var i = 0; i < geomCollection.NumGeometries; i++)
                Write(geomCollection.Geometries[i], writer);
        }

        /// <summary>
        /// Writes LittleIndian ByteOrder.
        /// </summary>
        /// <param name="writer"></param>
        protected void WriteByteOrder(BinaryWriter writer)
        {
            writer.Write((byte)EncodingType);
        }

        private void CalcCoordinateSize()
        {
            _coordinateSize = 16;
            if ((HandleOrdinates & Ordinates.Z) == Ordinates.Z) _coordinateSize += 8;
            if ((HandleOrdinates & Ordinates.M) == Ordinates.M) _coordinateSize += 8;
        }

        private static char ToHexDigit(int n)
        {
            if (n < 0 || n > 15)
                throw new ArgumentException("Nibble value out of range: " + n);
            if (n <= 9)
                return (char)('0' + n);
            return (char)('A' + (n - 10));
        }

        private void WriteHeader(BinaryWriter writer, IGeometry geom)
        {
            //Byte Order
            WriteByteOrder(writer);

            WkbGeometryType geometryType;
            switch (geom.GeometryType)
            {
                case "Point":
                    geometryType = WkbGeometryType.WkbPoint;
                    break;
                case "LineString":
                    geometryType = WkbGeometryType.WkbLineString;
                    break;
                case "Polygon":
                    geometryType = WkbGeometryType.WkbPolygon;
                    break;
                case "MultiPoint":
                    geometryType = WkbGeometryType.WkbMultiPoint;
                    break;
                case "MultiPolygon":
                    geometryType = WkbGeometryType.WkbMultiPolygon;
                    break;
                case "MultiLineString":
                    geometryType = WkbGeometryType.WkbMultiLineString;
                    break;
                case "GeometryCollection":
                    geometryType = WkbGeometryType.WkbGeometryCollection;
                    break;
                default:
                    Assert.ShouldNeverReachHere("Unknown geometry type:" + geom.GeometryType);
                    throw new ArgumentException("geom");
            }

            //Modify WKB Geometry type
            var intGeometryType = (uint)geometryType & 0xff;
            if ((HandleOrdinates & Ordinates.Z) == Ordinates.Z)
            {
                intGeometryType += 1000;
                if (!Strict) intGeometryType |= 0x80000000;
            }

            if ((HandleOrdinates & Ordinates.M) == Ordinates.M)
            {
                intGeometryType += 2000;
                if (!Strict) intGeometryType |= 0x40000000;
            }

            //Flag for SRID if needed
            if (HandleSrid)
                intGeometryType |= 0x20000000;

            //
            writer.Write(intGeometryType);

            //Write SRID if needed
            if (HandleSrid)
                writer.Write(geom.Srid);
        }

        #endregion
    }
}