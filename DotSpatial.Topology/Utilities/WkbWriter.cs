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

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Writes a Well-Known Binary byte data representation of a <c>Geometry</c>.
    /// </summary>
    public class WkbWriter
    {
        /// <summary>
        /// Standard byte size for each complex point.
        /// Each complex point (LineString, Polygon, ...) contains:
        ///     1 byte for ByteOrder and
        ///     4 bytes for WKBType.
        /// </summary>
        protected const int INIT_COUNT = 5;

        private readonly ByteOrder _encodingType;

        /// <summary>
        /// Initializes writer with LittleIndian byte order.
        /// </summary>
        public WkbWriter() : this(ByteOrder.LittleEndian) { }

        /// <summary>
        /// Initializes writer with the specified byte order.
        /// </summary>
        /// <param name="encodingType">Encoding type</param>
        public WkbWriter(ByteOrder encodingType)
        {
            _encodingType = encodingType;
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
        public virtual void Write(IGeometry geometry, Stream stream)
        {
            BinaryWriter writer = null;
            try
            {
                if (_encodingType == ByteOrder.LittleEndian)
                    writer = new BinaryWriter(stream);
                else writer = new BeBinaryWriter(stream);
                Write(geometry, writer);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IGeometry geometry, BinaryWriter writer)
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
            else throw new ArgumentException("Geometry not recognized: " + geometry.ToString());
        }

        /// <summary>
        /// Writes LittleIndian ByteOrder.
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void WriteByteOrder(BinaryWriter writer)
        {
            writer.Write((byte)_encodingType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IPoint point, BinaryWriter writer)
        {
            WriteByteOrder(writer);
            writer.Write((int)WkbGeometryType.Point);
            Write(point.Coordinate, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lineString"></param>
        /// <param name="writer"></param>
        protected virtual void Write(ILineString lineString, BinaryWriter writer)
        {
            WriteByteOrder(writer);
            writer.Write((int)WkbGeometryType.LineString);
            writer.Write(lineString.NumPoints);
            for (int i = 0; i < lineString.Coordinates.Count; i++)
                Write(lineString.Coordinates[i], writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IPolygon polygon, BinaryWriter writer)
        {
            WriteByteOrder(writer);
            writer.Write((int)WkbGeometryType.Polygon);
            writer.Write(polygon.NumHoles + 1);
            Write(polygon.Shell, writer);
            for (int i = 0; i < polygon.NumHoles; i++)
                Write(polygon.Holes[i], writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiPoint"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IMultiPoint multiPoint, BinaryWriter writer)
        {
            WriteByteOrder(writer);
            writer.Write((int)WkbGeometryType.MultiPoint);
            writer.Write(multiPoint.NumGeometries);
            for (int i = 0; i < multiPoint.NumGeometries; i++)
                Write(multiPoint.Geometries[i] as Point, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiLineString"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IMultiLineString multiLineString, BinaryWriter writer)
        {
            WriteByteOrder(writer);
            writer.Write((int)WkbGeometryType.MultiLineString);
            writer.Write(multiLineString.NumGeometries);
            for (int i = 0; i < multiLineString.NumGeometries; i++)
                Write(multiLineString.Geometries[i] as LineString, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiPolygon"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IMultiPolygon multiPolygon, BinaryWriter writer)
        {
            WriteByteOrder(writer);
            writer.Write((int)WkbGeometryType.MultiPolygon);
            writer.Write(multiPolygon.NumGeometries);
            for (int i = 0; i < multiPolygon.NumGeometries; i++)
                Write(multiPolygon.Geometries[i] as Polygon, writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomCollection"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IGeometryCollection geomCollection, BinaryWriter writer)
        {
            WriteByteOrder(writer);
            writer.Write((int)WkbGeometryType.GeometryCollection);
            writer.Write(geomCollection.NumGeometries);
            for (int i = 0; i < geomCollection.NumGeometries; i++)
                Write(geomCollection.Geometries[i], writer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="writer"></param>
        protected virtual void Write(Coordinate coordinate, BinaryWriter writer)
        {
            writer.Write(coordinate.X);
            writer.Write(coordinate.Y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="writer"></param>
        protected virtual void Write(ILinearRing ring, BinaryWriter writer)
        {
            writer.Write(ring.NumPoints);
            for (int i = 0; i < ring.Coordinates.Count; i++)
                Write(ring.Coordinates[i], writer);
        }

        /// <summary>
        /// Sets corrent length for Byte Stream.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual byte[] GetBytes(IGeometry geometry)
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
            if (geometry is Point)
                return SetByteStream(geometry as Point);
            if (geometry is LineString)
                return SetByteStream(geometry as LineString);
            if (geometry is Polygon)
                return SetByteStream(geometry as Polygon);
            if (geometry is MultiPoint)
                return SetByteStream(geometry as MultiPoint);
            if (geometry is MultiLineString)
                return SetByteStream(geometry as MultiLineString);
            if (geometry is MultiPolygon)
                return SetByteStream(geometry as MultiPolygon);
            if (geometry is GeometryCollection)
                return SetByteStream(geometry as GeometryCollection);
            throw new ArgumentException("ShouldNeverReachHere");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IGeometryCollection geometry)
        {
            int count = INIT_COUNT;
            count += 4;
            foreach (Geometry geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IMultiPolygon geometry)
        {
            int count = INIT_COUNT;
            count += 4;
            foreach (Polygon geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IMultiLineString geometry)
        {
            int count = INIT_COUNT;
            count += 4;
            foreach (LineString geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IMultiPoint geometry)
        {
            int count = INIT_COUNT;
            count += 4;     // NumPoints
            foreach (Point geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IPolygon geometry)
        {
            int count = INIT_COUNT;
            count += 4 + 4;                                 // NumRings + NumPoints
            count += 4 * (geometry.NumHoles + 1);   // Index parts
            count += geometry.NumPoints * 16;               // Points in exterior and interior rings
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(ILineString geometry)
        {
            int numPoints = geometry.NumPoints;
            int count = INIT_COUNT;
            count += 4;                             // NumPoints
            count += 16 * numPoints;
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IPoint geometry)
        {
            return 21;
        }
    }
}