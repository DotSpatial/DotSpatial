// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://dotspatial.codeplex.com/license and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.IO;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Converts a Well-Known Binary byte data to a <c>Geometry</c>.
    /// </summary>
    public class WkbReader
    {
        private IGeometryFactory factory;

        /// <summary>
        /// Initialize reader with a standard <c>GeometryFactory</c>.
        /// </summary>
        public WkbReader() : this(new GeometryFactory()) { }

        /// <summary>
        /// Initialize reader with the given <c>GeometryFactory</c>.
        /// </summary>
        /// <param name="factory"></param>
        public WkbReader(IGeometryFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// <c>Geometry</c> builder.
        /// </summary>
        protected virtual IGeometryFactory Factory
        {
            get { return factory; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual IGeometry Read(byte[] data)
        {
            using (Stream stream = new MemoryStream(data))
                return Read(stream);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual IGeometry Read(Stream stream)
        {
            BinaryReader reader = null;
            ByteOrder byteOrder = (ByteOrder)stream.ReadByte();
            try
            {
                reader = byteOrder == ByteOrder.BigEndian ? new BeBinaryReader(stream) : new BinaryReader(stream);
                return Read(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry Read(BinaryReader reader)
        {
            WkbGeometryType geometryType = (WkbGeometryType)reader.ReadInt32();
            switch (geometryType)
            {
                case WkbGeometryType.Point:
                    return ReadPoint(reader);
                case WkbGeometryType.LineString:
                    return ReadLineString(reader);
                case WkbGeometryType.Polygon:
                    return ReadPolygon(reader);
                case WkbGeometryType.MultiPoint:
                    return ReadMultiPoint(reader);
                case WkbGeometryType.MultiLineString:
                    return ReadMultiLineString(reader);
                case WkbGeometryType.MultiPolygon:
                    return ReadMultiPolygon(reader);
                case WkbGeometryType.GeometryCollection:
                    return ReadGeometryCollection(reader);
                default:
                    throw new ArgumentException("Geometry type not recognized. GeometryCode: " + geometryType);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual ByteOrder ReadByteOrder(BinaryReader reader)
        {
            byte byteOrder = reader.ReadByte();
            return (ByteOrder)byteOrder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual Coordinate ReadCoordinate(BinaryReader reader)
        {
            return new Coordinate(reader.ReadDouble(), reader.ReadDouble());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual ILinearRing ReadRing(BinaryReader reader)
        {
            int numPoints = reader.ReadInt32();
            Coordinate[] coordinates = new Coordinate[numPoints];
            for (int i = 0; i < numPoints; i++)
                coordinates[i] = ReadCoordinate(reader);
            return Factory.CreateLinearRing(coordinates);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadPoint(BinaryReader reader)
        {
            return Factory.CreatePoint(ReadCoordinate(reader));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadLineString(BinaryReader reader)
        {
            int numPoints = reader.ReadInt32();
            Coordinate[] coordinates = new Coordinate[numPoints];
            for (int i = 0; i < numPoints; i++)
                coordinates[i] = ReadCoordinate(reader);
            return Factory.CreateLineString(coordinates);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadPolygon(BinaryReader reader)
        {
            int numRings = reader.ReadInt32();
            ILinearRing exteriorRing = ReadRing(reader);
            ILinearRing[] interiorRings = new LinearRing[numRings - 1];
            for (int i = 0; i < numRings - 1; i++)
                interiorRings[i] = ReadRing(reader);
            return Factory.CreatePolygon(exteriorRing, interiorRings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadMultiPoint(BinaryReader reader)
        {
            int numGeometries = reader.ReadInt32();
            Point[] points = new Point[numGeometries];
            for (int i = 0; i < numGeometries; i++)
            {
                ReadByteOrder(reader);
                WkbGeometryType geometryType = (WkbGeometryType)reader.ReadInt32();
                if (geometryType != WkbGeometryType.Point)
                    throw new ArgumentException("Point feature expected");
                points[i] = ReadPoint(reader) as Point;
            }
            return Factory.CreateMultiPoint(points);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadMultiLineString(BinaryReader reader)
        {
            int numGeometries = reader.ReadInt32();
            LineString[] strings = new LineString[numGeometries];
            for (int i = 0; i < numGeometries; i++)
            {
                ReadByteOrder(reader);
                WkbGeometryType geometryType = (WkbGeometryType)reader.ReadInt32();
                if (geometryType != WkbGeometryType.LineString)
                    throw new ArgumentException("LineString feature expected");
                strings[i] = ReadLineString(reader) as LineString;
            }
            return Factory.CreateMultiLineString(strings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadMultiPolygon(BinaryReader reader)
        {
            int numGeometries = reader.ReadInt32();
            Polygon[] polygons = new Polygon[numGeometries];
            for (int i = 0; i < numGeometries; i++)
            {
                ReadByteOrder(reader);
                WkbGeometryType geometryType = (WkbGeometryType)reader.ReadInt32();
                if (geometryType != WkbGeometryType.Polygon)
                    throw new ArgumentException("Polygon feature expected");
                polygons[i] = ReadPolygon(reader) as Polygon;
            }
            return Factory.CreateMultiPolygon(polygons);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadGeometryCollection(BinaryReader reader)
        {
            int numGeometries = reader.ReadInt32();
            IGeometry[] geometries = new Geometry[numGeometries];

            for (int i = 0; i < numGeometries; i++)
            {
                ReadByteOrder(reader);
                WkbGeometryType geometryType = (WkbGeometryType)reader.ReadInt32();
                switch (geometryType)
                {
                    case WkbGeometryType.Point:
                        geometries[i] = ReadPoint(reader);
                        break;
                    case WkbGeometryType.LineString:
                        geometries[i] = ReadLineString(reader);
                        break;
                    case WkbGeometryType.Polygon:
                        geometries[i] = ReadPolygon(reader);
                        break;
                    case WkbGeometryType.MultiPoint:
                        geometries[i] = ReadMultiPoint(reader);
                        break;
                    case WkbGeometryType.MultiLineString:
                        geometries[i] = ReadMultiLineString(reader);
                        break;
                    case WkbGeometryType.MultiPolygon:
                        geometries[i] = ReadMultiPolygon(reader);
                        break;
                    case WkbGeometryType.GeometryCollection:
                        geometries[i] = ReadGeometryCollection(reader);
                        break;
                    default:
                        throw new ArgumentException("Should never reach here!");
                }
            }
            return Factory.CreateGeometryCollection(geometries);
        }
    }
}