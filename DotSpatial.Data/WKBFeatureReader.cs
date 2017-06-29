// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/2/2010 10:07:31 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using DotSpatial.Topology;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Data
{
    /// <summary>
    /// WKBPointReader
    /// </summary>
    public static class WkbFeatureReader
    {
        private static ByteOrder _endian;

        /// <summary>
        /// Given the array of bytes, this reverses the bytes based on size.  So if size if 4, the
        /// reversal will flip uints of 4 bytes at a time.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public static void CheckEndian(byte[] data, int size)
        {
            if ((_endian == ByteOrder.LittleEndian) == BitConverter.IsLittleEndian) return;
            int count = data.Length / size;
            for (int i = 0; i < count; i++)
            {
                byte[] temp = new byte[size];
                Array.Copy(data, i * size, temp, 0, size);
                Array.Reverse(temp);
                Array.Copy(temp, 0, data, i * size, size);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data">The raw byte data.</param>
        /// <returns></returns>
        public static int ReadInt32(Stream data)
        {
            byte[] vals = new byte[4];
            data.Read(vals, 0, 4);
            CheckEndian(vals, 4);
            return BitConverter.ToInt32(vals, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data">The raw byte data.</param>
        /// <param name="count">The count of integers, not bytes.</param>
        /// <returns></returns>
        public static int[] ReadInt32(Stream data, int count)
        {
            byte[] vals = new byte[4 * count];
            data.Read(vals, 0, 4 * count);
            CheckEndian(vals, 4);
            int[] result = new int[count];
            Buffer.BlockCopy(vals, 0, result, 0, count * 4);
            return result;
        }

        /// <summary>
        /// Reads the specified number of doubles
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static double[] ReadDouble(Stream data, int count)
        {
            byte[] vals = new byte[8 * count];
            data.Read(vals, 0, count * 8);
            CheckEndian(vals, 8);
            double[] result = new double[count];
            Buffer.BlockCopy(vals, 0, result, 0, count * 8);
            return result;
        }

        /// <summary>
        /// Since WKB can in fact store different kinds of shapes, this will split out
        /// each type of shape into a different featureset.  If all the shapes are
        /// the same kind of feature, thre will only be one list of feature types.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FeatureSetPack GetFeatureSets(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            return GetFeatureSets(ms);
        }

        /// <summary>
        /// Gets a FeatureSetPack from the wkb
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FeatureSetPack GetFeatureSets(Stream data)
        {
            FeatureSetPack result = new FeatureSetPack();

            while (data.Position < data.Length)
            {
                ReadFeature(data, result);
            }
            result.StopEditing();
            return result;
        }

        /// <summary>
        /// Reads only a single geometry into a feature.  This may be a multi-part geometry.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="results"></param>
        public static void ReadFeature(Stream data, FeatureSetPack results)
        {
            _endian = (ByteOrder)data.ReadByte();
            WkbGeometryType type = (WkbGeometryType)ReadInt32(data);

            switch (type)
            {
                case WkbGeometryType.Point:
                    ReadPoint(data, results);
                    return;
                case WkbGeometryType.LineString:
                    ReadLineString(data, results);
                    return;
                case WkbGeometryType.Polygon:
                    ReadPolygon(data, results);
                    break;
                case WkbGeometryType.MultiPoint:
                    ReadMultiPoint(data, results);
                    break;
                case WkbGeometryType.MultiLineString:
                    ReadMultiLineString(data, results);
                    break;
                case WkbGeometryType.MultiPolygon:
                    ReadMultiPolygon(data, results);
                    break;
                case WkbGeometryType.GeometryCollection:
                    ReadGeometryCollection(data, results);
                    break;
            }
        }

        /// <summary>
        /// Reads only a single geometry into a feature.  This may be a multi-part geometry,
        /// but cannot be a mixed part geometry.  Anything that registers as "geometryCollection"
        /// will trigger an exception.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Shape ReadShape(Stream data)
        {
            return ReadShape(data, FeatureType.Unspecified);
        }

        /// <summary>
        /// Attempts to read in an entry to the specified feature type.  If the feature type does not match
        /// the geometry type, this will return null.  (A Point geometry will be accepted by MultiPoint
        /// feature type, but not the other way arround.  Either way, this will advance the reader
        /// through the shape feature.  Using the Unspecified will always return the shape it reads,
        /// or null in the case of mixed feature collections which are not supported.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="featureType"></param>
        /// <returns></returns>
        public static Shape ReadShape(Stream data, FeatureType featureType)
        {
            _endian = (ByteOrder)data.ReadByte();
            WkbGeometryType type = (WkbGeometryType)ReadInt32(data);
            Shape result;
            switch (type)
            {
                case WkbGeometryType.Point:
                    result = ReadPoint(data);
                    if (featureType == FeatureType.Point || featureType == FeatureType.MultiPoint || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }
                    return null;
                case WkbGeometryType.LineString:
                    result = ReadLineString(data);
                    if (featureType == FeatureType.Line || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }
                    return null;
                case WkbGeometryType.Polygon:
                    result = ReadPolygon(data);
                    if (featureType == FeatureType.Polygon || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }
                    return null;
                case WkbGeometryType.MultiPoint:
                    result = ReadMultiPoint(data);
                    if (featureType == FeatureType.MultiPoint || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }
                    return null;
                case WkbGeometryType.MultiLineString:
                    result = ReadMultiLineString(data);
                    if (featureType == FeatureType.Line || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }
                    return null;
                case WkbGeometryType.MultiPolygon:
                    result = ReadMultiPolygon(data);
                    if (featureType == FeatureType.Polygon || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }
                    return null;
                case WkbGeometryType.GeometryCollection:
                    throw new ArgumentException("Mixed shape type collections are not supported by this method.");
            }
            return null;
        }

        /// <summary>
        /// This assumes that the byte order and shapetype have already been read.
        /// </summary>
        /// <param name="data"></param>
        public static Shape ReadPoint(Stream data)
        {
            Shape result = new Shape();
            result.Range = new ShapeRange(FeatureType.Point);
            PartRange prt = new PartRange(FeatureType.Point);
            prt.NumVertices = 1;
            result.Range.Parts.Add(prt);
            result.Vertices = ReadDouble(data, 2);
            return result;
        }

        private static void ReadGeometryCollection(Stream data, FeatureSetPack results)
        {
            int numGeometries = ReadInt32(data);
            // Don't worry about "multi-parting" these.  Simply create a separate shape
            // entry for every single geometry here since we have to split out the features
            // based on feature type.  (currently we don't have a mixed feature type for drawing.)
            for (int i = 0; i < numGeometries; i++)
            {
                _endian = (ByteOrder)data.ReadByte();
                WkbGeometryType type = (WkbGeometryType)ReadInt32(data);
                switch (type)
                {
                    case WkbGeometryType.Point:
                        ReadPoint(data, results);
                        return;
                    case WkbGeometryType.LineString:
                        ReadLineString(data, results);
                        return;
                    case WkbGeometryType.Polygon:
                        ReadPolygon(data, results);
                        break;
                    case WkbGeometryType.MultiPoint:
                        ReadMultiPoint(data, results);
                        break;
                    case WkbGeometryType.MultiLineString:
                        ReadMultiLineString(data, results);
                        break;
                    case WkbGeometryType.MultiPolygon:
                        ReadMultiPolygon(data, results);
                        break;
                    case WkbGeometryType.GeometryCollection:
                        ReadGeometryCollection(data, results);
                        break;
                }
            }
        }

        /// <summary>
        /// This assumes that the byte order and shapetype have already been read.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="results"></param>
        private static void ReadPoint(Stream data, FeatureSetPack results)
        {
            ShapeRange sr = new ShapeRange(FeatureType.MultiPoint);
            PartRange prt = new PartRange(FeatureType.MultiPoint);
            prt.NumVertices = 1;
            sr.Parts.Add(prt);
            double[] coord = ReadDouble(data, 2);
            results.Add(coord, sr);
        }

        /// <summary>
        /// Reads one multipoint shape from a data stream.
        /// (this assumes that the two bytes (endian and type) have already been read.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Shape ReadMultiPoint(Stream data)
        {
            Shape result = new Shape(FeatureType.MultiPoint);
            int count = ReadInt32(data);
            PartRange prt = new PartRange(FeatureType.MultiPoint);
            prt.NumVertices = count;
            result.Range.Parts.Add(prt);
            double[] vertices = new double[count * 2];
            for (int iPoint = 0; iPoint < count; iPoint++)
            {
                data.ReadByte(); // ignore endian
                ReadInt32(data); // ignore geometry type
                double[] coord = ReadDouble(data, 2);
                Array.Copy(coord, 0, vertices, iPoint * 2, 2);
            }
            result.Vertices = vertices;
            return result;
        }

        private static void ReadMultiPoint(Stream data, FeatureSetPack results)
        {
            int count = ReadInt32(data);
            ShapeRange sr = new ShapeRange(FeatureType.MultiPoint);
            PartRange prt = new PartRange(FeatureType.MultiPoint);
            prt.NumVertices = count;
            sr.Parts.Add(prt);
            double[] vertices = new double[count * 2];
            for (int iPoint = 0; iPoint < count; iPoint++)
            {
                data.ReadByte(); // ignore endian
                ReadInt32(data); // ignore geometry type
                double[] coord = ReadDouble(data, 2);
                Array.Copy(coord, 0, vertices, iPoint * 2, 2);
            }
            results.Add(vertices, sr);
        }

        private static Shape ReadLineString(Stream data)
        {
            Shape result = new Shape(FeatureType.Line);
            int count = ReadInt32(data);
            double[] coords = ReadDouble(data, 2 * count);
            PartRange lPrt = new PartRange(FeatureType.Line);
            lPrt.NumVertices = count;
            result.Range.Parts.Add(lPrt);
            result.Vertices = coords;
            return result;
        }

        private static void ReadLineString(Stream data, FeatureSetPack results)
        {
            int count = ReadInt32(data);
            double[] coords = ReadDouble(data, 2 * count);
            ShapeRange lShp = new ShapeRange(FeatureType.Line);
            PartRange lPrt = new PartRange(FeatureType.Line);
            lPrt.NumVertices = count;
            lShp.Parts.Add(lPrt);
            results.Add(coords, lShp);
        }

        private static Shape ReadMultiLineString(Stream data)
        {
            Shape result = new Shape(FeatureType.Line);
            int numLineStrings = ReadInt32(data);
            List<double[]> strings = new List<double[]>();
            int partOffset = 0;
            for (int iString = 0; iString < numLineStrings; iString++)
            {
                // Each of these needs to read a full WKBLineString
                data.Seek(5, SeekOrigin.Current); //ignore header
                int numPoints = ReadInt32(data);
                double[] coords = ReadDouble(data, 2 * numPoints);
                PartRange lPrt = new PartRange(FeatureType.Line);
                lPrt.PartOffset = partOffset;
                lPrt.NumVertices = numPoints;
                result.Range.Parts.Add(lPrt);
                partOffset += coords.Length / 2;
                strings.Add(coords);
            }
            double[] allVertices = new double[partOffset * 2];
            int offset = 0;
            foreach (double[] ring in strings)
            {
                Array.Copy(ring, 0, allVertices, offset, ring.Length);
                offset += ring.Length;
            }
            result.Vertices = allVertices;
            return result;
        }

        private static void ReadMultiLineString(Stream data, FeatureSetPack results)
        {
            int numLineStrings = ReadInt32(data);
            ShapeRange shp = new ShapeRange(FeatureType.Line);
            List<double[]> strings = new List<double[]>();
            int partOffset = 0;
            for (int iString = 0; iString < numLineStrings; iString++)
            {
                // Each of these needs to read a full WKBLineString
                data.Seek(5, SeekOrigin.Current); //ignore header
                int numPoints = ReadInt32(data);
                double[] coords = ReadDouble(data, 2 * numPoints);
                PartRange lPrt = new PartRange(FeatureType.Line);
                lPrt.PartOffset = partOffset;
                lPrt.NumVertices = numPoints;
                shp.Parts.Add(lPrt);
                partOffset += coords.Length / 2;
                strings.Add(coords);
            }
            double[] allVertices = new double[partOffset * 2];
            int offset = 0;
            foreach (double[] ring in strings)
            {
                Array.Copy(ring, 0, allVertices, offset, ring.Length);
                offset += ring.Length;
            }
            results.Add(allVertices, shp);
        }

        private static Shape ReadPolygon(Stream data)
        {
            Shape result = new Shape(FeatureType.Polygon);
            int numRings = ReadInt32(data);
            List<double[]> rings = new List<double[]>();
            int partOffset = 0;
            for (int iRing = 0; iRing < numRings; iRing++)
            {
                int numPoints = ReadInt32(data); // ring structures are like a linestring without the final point.
                double[] coords = ReadDouble(data, 2 * numPoints);
                if (iRing == 0)
                {
                    // By shapefile standard, the shell should be clockwise
                    if (IsCounterClockwise(coords))
                        coords = ReverseCoords(coords);
                }
                else
                {
                    // By shapefile standard, the holes should be counter clockwise.
                    if (!IsCounterClockwise(coords))
                        coords = ReverseCoords(coords);
                }
                PartRange lPrt = new PartRange(FeatureType.Polygon);
                lPrt.PartOffset = partOffset;
                lPrt.NumVertices = numPoints;
                result.Range.Parts.Add(lPrt);
                partOffset += coords.Length / 2;
                rings.Add(coords);
            }
            double[] allVertices = new double[partOffset * 2];
            int offset = 0;
            foreach (double[] ring in rings)
            {
                Array.Copy(ring, 0, allVertices, offset, ring.Length);
                offset += ring.Length;
            }
            result.Vertices = allVertices;
            return result;
        }

        private static void ReadPolygon(Stream data, FeatureSetPack results)
        {
            int numRings = ReadInt32(data);
            ShapeRange lShp = new ShapeRange(FeatureType.Polygon);
            List<double[]> rings = new List<double[]>();
            int partOffset = 0;
            for (int iRing = 0; iRing < numRings; iRing++)
            {
                int numPoints = ReadInt32(data); // ring structures are like a linestring without the final point.
                double[] coords = ReadDouble(data, 2 * numPoints);
                if (iRing == 0)
                {
                    // By shapefile standard, the shell should be clockwise
                    if (IsCounterClockwise(coords))
                        coords = ReverseCoords(coords);
                }
                else
                {
                    // By shapefile standard, the holes should be counter clockwise.
                    if (!IsCounterClockwise(coords))
                        coords = ReverseCoords(coords);
                }
                PartRange lPrt = new PartRange(FeatureType.Polygon);
                lPrt.PartOffset = partOffset;
                lPrt.NumVertices = numPoints;
                lShp.Parts.Add(lPrt);
                partOffset += coords.Length / 2;
                rings.Add(coords);
            }
            double[] allVertices = new double[partOffset * 2];
            int offset = 0;
            foreach (double[] ring in rings)
            {
                Array.Copy(ring, 0, allVertices, offset, ring.Length);
                offset += ring.Length;
            }
            results.Add(allVertices, lShp);
        }

        private static Shape ReadMultiPolygon(Stream data)
        {
            Shape result = new Shape(FeatureType.Polygon);
            int numPolygons = ReadInt32(data);
            List<double[]> rings = new List<double[]>();
            int partOffset = 0;
            for (int iPoly = 0; iPoly < numPolygons; iPoly++)
            {
                data.Seek(5, SeekOrigin.Current); // endian and geometry type
                int numRings = ReadInt32(data);
                for (int iRing = 0; iRing < numRings; iRing++)
                {
                    int numPoints = ReadInt32(data); // ring structures are like a linestring without the final point.
                    double[] coords = ReadDouble(data, 2 * numPoints);
                    if (iRing == 0)
                    {
                        // By shapefile standard, the shell should be clockwise
                        if (IsCounterClockwise(coords))
                            coords = ReverseCoords(coords);
                    }
                    else
                    {
                        // By shapefile standard, the holes should be counter clockwise.
                        if (!IsCounterClockwise(coords))
                            coords = ReverseCoords(coords);
                    }
                    PartRange lPrt = new PartRange(FeatureType.Polygon);
                    lPrt.PartOffset = partOffset;
                    lPrt.NumVertices = numPoints;
                    result.Range.Parts.Add(lPrt);
                    partOffset += coords.Length / 2;
                    rings.Add(coords);
                }
            }

            double[] allVertices = new double[partOffset * 2];
            int offset = 0;
            foreach (double[] ring in rings)
            {
                Array.Copy(ring, 0, allVertices, offset, ring.Length);
                offset += ring.Length;
            }
            result.Vertices = allVertices;
            return result;
        }

        private static void ReadMultiPolygon(Stream data, FeatureSetPack results)
        {
            int numPolygons = ReadInt32(data);
            ShapeRange lShp = new ShapeRange(FeatureType.Polygon);
            List<double[]> rings = new List<double[]>();
            int partOffset = 0;
            for (int iPoly = 0; iPoly < numPolygons; iPoly++)
            {
                data.Seek(5, SeekOrigin.Current); // endian and geometry type
                int numRings = ReadInt32(data);
                for (int iRing = 0; iRing < numRings; iRing++)
                {
                    int numPoints = ReadInt32(data); // ring structures are like a linestring without the final point.
                    double[] coords = ReadDouble(data, 2 * numPoints);
                    if (iRing == 0)
                    {
                        // By shapefile standard, the shell should be clockwise
                        if (IsCounterClockwise(coords))
                            coords = ReverseCoords(coords);
                    }
                    else
                    {
                        // By shapefile standard, the holes should be counter clockwise.
                        if (!IsCounterClockwise(coords))
                            coords = ReverseCoords(coords);
                    }
                    PartRange lPrt = new PartRange(FeatureType.Polygon);
                    lPrt.PartOffset = partOffset;
                    lPrt.NumVertices = numPoints;
                    lShp.Parts.Add(lPrt);
                    partOffset += coords.Length / 2;
                    rings.Add(coords);
                }
            }

            double[] allVertices = new double[partOffset * 2];
            int offset = 0;
            foreach (double[] ring in rings)
            {
                Array.Copy(ring, 0, allVertices, offset, ring.Length);
                offset += ring.Length;
            }
            results.Add(allVertices, lShp);
        }

        /// <summary>
        /// Fix introduced by JamesP@esdm.co.uk. 3/11/2010
        /// Using Array.Reverse does not work because it has the unwanted effect of flipping
        /// the X and Y values.
        /// </summary>
        /// <param name="coords">The double precision XY coordinate array of vertices</param>
        /// <returns>The double array in reverse order.</returns>
        private static double[] ReverseCoords(double[] coords)
        {
            int numCoords = coords.Length;
            double[] newCoords = new double[numCoords];
            for (int i = numCoords - 1; i >= 0; i -= 2)
            {
                newCoords[i - 1] = coords[numCoords - i - 1]; //X
                newCoords[i] = coords[numCoords - i]; //Y
            }
            return newCoords;
        }

        /// <summary>
        /// Calculates the area and if the area is negative, this is considered a hole.
        /// </summary>
        /// <returns>Boolean, true if this has a negative area and should be thought of as a hole</returns>
        private static bool IsCounterClockwise(double[] coords)
        {
            double area = 0;
            for (int i = 0; i < coords.Length / 2; i++)
            {
                double x1 = coords[i * 2];
                double y1 = coords[i * 2 + 1];
                double x2, y2;
                if (i == coords.Length / 2 - 1)
                {
                    x2 = coords[0];
                    y2 = coords[1];
                }
                else
                {
                    x2 = coords[(i + 1) * 2];
                    y2 = coords[(i + 1) * 2 + 1];
                }

                double trapArea = (x1 * y2) - (x2 * y1);
                area += trapArea;
            }
            return area >= 0;
        }
    }
}