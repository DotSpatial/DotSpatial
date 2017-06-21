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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Writes the GML representation of the features of Topology model.
    /// Uses GML 2.1.1 <c>Geometry.xsd</c> schema for base for features.
    /// </summary>
    public class GmlWriter
    {
        private const int INIT_VALUE = 100;
        private const int COORD_SIZE = 100;

        /// <summary>
        /// Formatter for double values of coordinates
        /// </summary>
        protected virtual NumberFormatInfo NumberFormatter
        {
            get
            {
                return Global.GetNfi();
            }
        }

        /// <summary>
        /// Returns an <c>XmlReader</c> with feature informations.
        /// Use <c>XmlDocument.Load(XmlReader)</c> for obtain a <c>XmlDocument</c> to work.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public virtual XmlReader Write(IGeometry geometry)
        {
            byte[] data = GetBytes(geometry);
            using (Stream stream = new MemoryStream(data))
                Write(geometry, stream);
            Stream outStream = new MemoryStream(data);
            return new XmlTextReader(outStream);
        }

        /// <summary>
        /// Writes a GML feature into a generic <c>Stream</c>, such a <c>FileStream</c> or other streams.
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="stream"></param>
        public virtual void Write(IGeometry geometry, Stream stream)
        {
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            Write(geometry, writer);
            writer.Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="writer"></param>
        protected void Write(Coordinate coordinate, XmlTextWriter writer)
        {
            writer.WriteStartElement("coord");
            writer.WriteElementString("X", coordinate.X.ToString("g", NumberFormatter));
            writer.WriteElementString("Y", coordinate.Y.ToString("g", NumberFormatter));
            writer.WriteEndElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="writer"></param>
        protected void Write(IList<Coordinate> coordinates, XmlTextWriter writer)
        {
            writer.WriteRaw("<coordinates>");
            foreach (Coordinate coordinate in coordinates)
            {
                writer.WriteRaw(coordinate.X.ToString("g", NumberFormatter) + " ");
                writer.WriteRaw(coordinate.Y.ToString("g", NumberFormatter) + " ");
            }
            writer.WriteRaw("</coordinates>");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IGeometry geometry, XmlTextWriter writer)
        {
            if (geometry is Point)
                Write(geometry as Point, writer);
            else if (geometry is LineString)
                Write(geometry as LineString, writer);
            else if (geometry is Polygon)
                Write(geometry as Polygon, writer);
            else if (geometry is MultiPoint)
                Write(geometry as MultiPoint, writer);
            else if (geometry is MultiLineString)
                Write(geometry as MultiLineString, writer);
            else if (geometry is MultiPolygon)
                Write(geometry as MultiPolygon, writer);
            else if (geometry is GeometryCollection)
                Write(geometry as GeometryCollection, writer);
            else throw new ArgumentException("Geometry not recognized: " + geometry.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IPoint point, XmlTextWriter writer)
        {
            writer.WriteStartElement("Point");
            Write(point.Coordinate, writer);
            writer.WriteEndElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lineString"></param>
        /// <param name="writer"></param>
        protected virtual void Write(LineString lineString, XmlTextWriter writer)
        {
            writer.WriteStartElement("LineString");
            Write(lineString.Coordinates, writer);
            writer.WriteEndElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="linearRing"></param>
        /// <param name="writer"></param>
        protected virtual void Write(LinearRing linearRing, XmlTextWriter writer)
        {
            writer.WriteStartElement("LinearRing");
            Write(linearRing.Coordinates, writer);
            writer.WriteEndElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="writer"></param>
        protected virtual void Write(Polygon polygon, XmlTextWriter writer)
        {
            writer.WriteStartElement("Polygon");
            writer.WriteStartElement("outerBoundaryIs");
            Write(polygon.ExteriorRing as LinearRing, writer);
            writer.WriteEndElement();
            for (int i = 0; i < polygon.NumHoles; i++)
            {
                writer.WriteStartElement("innerBoundaryIs");
                Write(polygon.Holes[i] as LinearRing, writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiPoint"></param>
        /// <param name="writer"></param>
        protected virtual void Write(MultiPoint multiPoint, XmlTextWriter writer)
        {
            writer.WriteStartElement("MultiPoint");
            for (int i = 0; i < multiPoint.NumGeometries; i++)
            {
                writer.WriteStartElement("pointMember");
                Write(multiPoint.Geometries[i] as Point, writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiLineString"></param>
        /// <param name="writer"></param>
        protected virtual void Write(MultiLineString multiLineString, XmlTextWriter writer)
        {
            writer.WriteStartElement("MultiLineString");
            for (int i = 0; i < multiLineString.NumGeometries; i++)
            {
                writer.WriteStartElement("lineStringMember");
                Write(multiLineString.Geometries[i] as LineString, writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiPolygon"></param>
        /// <param name="writer"></param>
        protected virtual void Write(MultiPolygon multiPolygon, XmlTextWriter writer)
        {
            writer.WriteStartElement("MultiPolygon");
            for (int i = 0; i < multiPolygon.NumGeometries; i++)
            {
                writer.WriteStartElement("polygonMember");
                Write(multiPolygon.Geometries[i] as Polygon, writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometryCollection"></param>
        /// <param name="writer"></param>
        protected virtual void Write(GeometryCollection geometryCollection, XmlTextWriter writer)
        {
            writer.WriteStartElement("MultiGeometry");
            for (int i = 0; i < geometryCollection.NumGeometries; i++)
            {
                writer.WriteStartElement("geometryMember");
                Write(geometryCollection.Geometries[i] as Geometry, writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// Sets corrent length for Byte Stream.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual byte[] GetBytes(IGeometry geometry)
        {
            if (geometry is IPoint)
                return new byte[SetByteStreamLength(geometry as Point)];
            if (geometry is ILineString)
                return new byte[SetByteStreamLength(geometry as LineString)];
            if (geometry is IPolygon)
                return new byte[SetByteStreamLength(geometry as Polygon)];
            if (geometry is IMultiPoint)
                return new byte[SetByteStreamLength(geometry as MultiPoint)];
            if (geometry is IMultiLineString)
                return new byte[SetByteStreamLength(geometry as MultiLineString)];
            if (geometry is IMultiPolygon)
                return new byte[SetByteStreamLength(geometry as MultiPolygon)];
            if (geometry is IGeometryCollection)
                return new byte[SetByteStreamLength(geometry as GeometryCollection)];
            throw new ArgumentException("ShouldNeverReachHere");
        }

        /// <summary>
        /// Sets corrent length for Byte Stream.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStreamLength(Geometry geometry)
        {
            if (geometry is Point)
                return SetByteStreamLength(geometry as Point);
            if (geometry is LineString)
                return SetByteStreamLength(geometry as LineString);
            if (geometry is Polygon)
                return SetByteStreamLength(geometry as Polygon);
            if (geometry is MultiPoint)
                return SetByteStreamLength(geometry as MultiPoint);
            if (geometry is MultiLineString)
                return SetByteStreamLength(geometry as MultiLineString);
            if (geometry is MultiPolygon)
                return SetByteStreamLength(geometry as MultiPolygon);
            if (geometry is GeometryCollection)
                return SetByteStreamLength(geometry as GeometryCollection);
            throw new ArgumentException("ShouldNeverReachHere");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometryCollection"></param>
        /// <returns></returns>
        protected virtual int SetByteStreamLength(GeometryCollection geometryCollection)
        {
            int count = INIT_VALUE;
            foreach (Geometry g in geometryCollection.Geometries)
                count += SetByteStreamLength(g);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiPolygon"></param>
        /// <returns></returns>
        protected virtual int SetByteStreamLength(MultiPolygon multiPolygon)
        {
            int count = INIT_VALUE;
            foreach (Polygon p in multiPolygon.Geometries)
                count += SetByteStreamLength(p);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiLineString"></param>
        /// <returns></returns>
        protected virtual int SetByteStreamLength(MultiLineString multiLineString)
        {
            int count = INIT_VALUE;
            foreach (LineString ls in multiLineString.Geometries)
                count += SetByteStreamLength(ls);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiPoint"></param>
        /// <returns></returns>
        protected virtual int SetByteStreamLength(MultiPoint multiPoint)
        {
            int count = INIT_VALUE;
            foreach (Point p in multiPoint.Geometries)
                count += SetByteStreamLength(p);
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        protected virtual int SetByteStreamLength(Polygon polygon)
        {
            int count = INIT_VALUE;
            count += polygon.NumPoints * COORD_SIZE;
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lineString"></param>
        /// <returns></returns>
        protected virtual int SetByteStreamLength(LineString lineString)
        {
            int count = INIT_VALUE;
            count += lineString.NumPoints * COORD_SIZE;
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected virtual int SetByteStreamLength(Point point)
        {
            return INIT_VALUE + COORD_SIZE;
        }
    }
}