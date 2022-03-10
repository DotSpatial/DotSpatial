// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using NetTopologySuite.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// A reader that reads features from WKB text.
    /// </summary>
    public static class WkbFeatureReader
    {
        #region Fields

        private static ByteOrder endian;

        #endregion

        #region Methods

        /// <summary>
        /// Given the array of bytes, this reverses the bytes based on size. So if size if 4, the
        /// reversal will flip uints of 4 bytes at a time.
        /// </summary>
        /// <param name="data">Data that gets reversed.</param>
        /// <param name="size">Number of bytes that will be flipped together.</param>
        public static void CheckEndian(byte[] data, int size)
        {
            if ((endian == ByteOrder.LittleEndian) == BitConverter.IsLittleEndian) return;

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
        /// Since WKB can in fact store different kinds of shapes, this will split out
        /// each type of shape into a different featureset. If all the shapes are
        /// the same kind of feature, thre will only be one list of feature types.
        /// </summary>
        /// <param name="data">Data to get the feature sets from.</param>
        /// <returns>The feature sets gotten from the data.</returns>
        public static FeatureSetPack GetFeatureSets(byte[] data)
        {
            MemoryStream ms = new(data);
            return GetFeatureSets(ms);
        }

        /// <summary>
        /// Gets a FeatureSetPack from the WKB.
        /// </summary>
        /// <param name="data">Data to get the feature sets from.</param>
        /// <returns>The feature sets gotten from the data.</returns>
        public static FeatureSetPack GetFeatureSets(Stream data)
        {
            FeatureSetPack result = new();

            while (data.Position < data.Length)
            {
                ReadFeature(data, result);
            }

            result.StopEditing();
            return result;
        }

        /// <summary>
        /// Reads the specified number of doubles.
        /// </summary>
        /// <param name="data">Data to read the double from.</param>
        /// <param name="count">Number of doubles that should be read.</param>
        /// <returns>The doubles that were read.</returns>
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
        /// Reads only a single geometry into a feature. This may be a multi-part geometry.
        /// </summary>
        /// <param name="data">The data to read the features from.</param>
        /// <param name="results">FeatureSetPack the read features get added to.</param>
        public static void ReadFeature(Stream data, FeatureSetPack results)
        {
            endian = (ByteOrder)data.ReadByte();
            WKBGeometryTypes type = (WKBGeometryTypes)ReadInt32(data);

            switch (type)
            {
                case WKBGeometryTypes.WKBPoint:
                    ReadPoint(data, results);
                    return;
                case WKBGeometryTypes.WKBLineString:
                    ReadLineString(data, results);
                    return;
                case WKBGeometryTypes.WKBPolygon:
                    ReadPolygon(data, results);
                    break;
                case WKBGeometryTypes.WKBMultiPoint:
                    ReadMultiPoint(data, results);
                    break;
                case WKBGeometryTypes.WKBMultiLineString:
                    ReadMultiLineString(data, results);
                    break;
                case WKBGeometryTypes.WKBMultiPolygon:
                    ReadMultiPolygon(data, results);
                    break;
                case WKBGeometryTypes.WKBGeometryCollection:
                    ReadGeometryCollection(data, results);
                    break;
            }
        }

        /// <summary>
        /// Reads an int from the stream.
        /// </summary>
        /// <param name="data">The raw byte data.</param>
        /// <returns>The int that was read.</returns>
        public static int ReadInt32(Stream data)
        {
            byte[] vals = new byte[4];
            data.Read(vals, 0, 4);
            CheckEndian(vals, 4);
            return BitConverter.ToInt32(vals, 0);
        }

        /// <summary>
        /// Reads ints from the stream.
        /// </summary>
        /// <param name="data">The raw byte data.</param>
        /// <param name="count">The count of integers, not bytes.</param>
        /// <returns>The ints that were read.</returns>
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
        /// Reads a point from the data. This assumes that the byte order and shapetype have already been read.
        /// </summary>
        /// <param name="data">Data to read the point from.</param>
        /// <returns>The read point as shape.</returns>
        public static Shape ReadPoint(Stream data)
        {
            Shape result = new()
            {
                Range = new ShapeRange(FeatureType.Point)
            };
            PartRange prt = new(FeatureType.Point)
            {
                NumVertices = 1
            };
            result.Range.Parts.Add(prt);
            result.Vertices = ReadDouble(data, 2);
            return result;
        }

        /// <summary>
        /// Reads only a single geometry into a feature. This may be a multi-part geometry,
        /// but cannot be a mixed part geometry. Anything that registers as "geometryCollection"
        /// will trigger an exception.
        /// </summary>
        /// <param name="data">The data to read from.</param>
        /// <returns>The shape that was read.</returns>
        public static Shape ReadShape(Stream data)
        {
            return ReadShape(data, FeatureType.Unspecified);
        }

        /// <summary>
        /// Attempts to read in an entry to the specified feature type. If the feature type does not match
        /// the geometry type, this will return null. (A Point geometry will be accepted by MultiPoint
        /// feature type, but not the other way arround. Either way, this will advance the reader
        /// through the shape feature. Using the Unspecified will always return the shape it reads,
        /// or null in the case of mixed feature collections which are not supported.
        /// </summary>
        /// <param name="data">Data that contains the WKB feature.</param>
        /// <param name="featureType">The feature type.</param>
        /// <returns>The resulting shape.</returns>
        public static Shape ReadShape(Stream data, FeatureType featureType)
        {
            endian = (ByteOrder)data.ReadByte();
            WKBGeometryTypes type = (WKBGeometryTypes)ReadInt32(data);
            Shape result;
            switch (type)
            {
                case WKBGeometryTypes.WKBPoint:
                    result = ReadPoint(data);
                    if (featureType == FeatureType.Point || featureType == FeatureType.MultiPoint || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }

                    return null;
                case WKBGeometryTypes.WKBLineString:
                    result = ReadLineString(data);
                    if (featureType == FeatureType.Line || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }

                    return null;
                case WKBGeometryTypes.WKBPolygon:
                    result = ReadPolygon(data);
                    if (featureType == FeatureType.Polygon || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }

                    return null;
                case WKBGeometryTypes.WKBMultiPoint:
                    result = ReadMultiPoint(data);
                    if (featureType == FeatureType.MultiPoint || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }

                    return null;
                case WKBGeometryTypes.WKBMultiLineString:
                    result = ReadMultiLineString(data);
                    if (featureType == FeatureType.Line || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }

                    return null;
                case WKBGeometryTypes.WKBMultiPolygon:
                    result = ReadMultiPolygon(data);
                    if (featureType == FeatureType.Polygon || featureType == FeatureType.Unspecified)
                    {
                        return result;
                    }

                    return null;
                case WKBGeometryTypes.WKBGeometryCollection: throw new ArgumentException("Mixed shape type collections are not supported by this method.");
            }

            return null;
        }

        /// <summary>
        /// Calculates the area and if the area is negative, this is considered a hole.
        /// </summary>
        /// <param name="coords">Coordinates whose direction gets checked.</param>
        /// <returns>Boolean, true if this has a negative area and should be thought of as a hole.</returns>
        private static bool IsCounterClockwise(double[] coords)
        {
            double area = 0;
            for (int i = 0; i < coords.Length / 2; i++)
            {
                double x1 = coords[i * 2];
                double y1 = coords[(i * 2) + 1];
                double x2, y2;
                if (i == (coords.Length / 2) - 1)
                {
                    x2 = coords[0];
                    y2 = coords[1];
                }
                else
                {
                    x2 = coords[(i + 1) * 2];
                    y2 = coords[((i + 1) * 2) + 1];
                }

                double trapArea = (x1 * y2) - (x2 * y1);
                area += trapArea;
            }

            return area >= 0;
        }

        private static void ReadGeometryCollection(Stream data, FeatureSetPack results)
        {
            int numGeometries = ReadInt32(data);

            // Don't worry about "multi-parting" these. Simply create a separate shape
            // entry for every single geometry here since we have to split out the features
            // based on feature type. (currently we don't have a mixed feature type for drawing.)
            for (int i = 0; i < numGeometries; i++)
            {
                endian = (ByteOrder)data.ReadByte();
                WKBGeometryTypes type = (WKBGeometryTypes)ReadInt32(data);
                switch (type)
                {
                    case WKBGeometryTypes.WKBPoint:
                        ReadPoint(data, results);
                        return;
                    case WKBGeometryTypes.WKBLineString:
                        ReadLineString(data, results);
                        return;
                    case WKBGeometryTypes.WKBPolygon:
                        ReadPolygon(data, results);
                        break;
                    case WKBGeometryTypes.WKBMultiPoint:
                        ReadMultiPoint(data, results);
                        break;
                    case WKBGeometryTypes.WKBMultiLineString:
                        ReadMultiLineString(data, results);
                        break;
                    case WKBGeometryTypes.WKBMultiPolygon:
                        ReadMultiPolygon(data, results);
                        break;
                    case WKBGeometryTypes.WKBGeometryCollection:
                        ReadGeometryCollection(data, results);
                        break;
                }
            }
        }

        private static Shape ReadLineString(Stream data)
        {
            Shape result = new(FeatureType.Line);
            int count = ReadInt32(data);
            double[] coords = ReadDouble(data, 2 * count);
            PartRange lPrt = new(FeatureType.Line)
            {
                NumVertices = count
            };
            result.Range.Parts.Add(lPrt);
            result.Vertices = coords;
            return result;
        }

        private static void ReadLineString(Stream data, FeatureSetPack results)
        {
            int count = ReadInt32(data);
            double[] coords = ReadDouble(data, 2 * count);
            ShapeRange lShp = new(FeatureType.Line);
            PartRange lPrt = new(FeatureType.Line)
            {
                NumVertices = count
            };
            lShp.Parts.Add(lPrt);
            results.Add(coords, lShp);
        }

        private static Shape ReadMultiLineString(Stream data)
        {
            Shape result = new(FeatureType.Line);
            int numLineStrings = ReadInt32(data);
            List<double[]> strings = new();
            int partOffset = 0;
            for (int iString = 0; iString < numLineStrings; iString++)
            {
                // Each of these needs to read a full WKBLineString
                data.Seek(5, SeekOrigin.Current); // ignore header
                int numPoints = ReadInt32(data);
                double[] coords = ReadDouble(data, 2 * numPoints);
                PartRange lPrt = new(FeatureType.Line)
                {
                    PartOffset = partOffset,
                    NumVertices = numPoints
                };
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
            ShapeRange shp = new(FeatureType.Line);
            List<double[]> strings = new();
            int partOffset = 0;
            for (int iString = 0; iString < numLineStrings; iString++)
            {
                // Each of these needs to read a full WKBLineString
                data.Seek(5, SeekOrigin.Current); // ignore header
                int numPoints = ReadInt32(data);
                double[] coords = ReadDouble(data, 2 * numPoints);
                PartRange lPrt = new(FeatureType.Line)
                {
                    PartOffset = partOffset,
                    NumVertices = numPoints
                };
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

        /// <summary>
        /// Reads one multipoint shape from a data stream.
        /// (this assumes that the two bytes (endian and type) have already been read.
        /// </summary>
        /// <param name="data">The data to read from.</param>
        /// <returns>The multipoint that was read as shape.</returns>
        private static Shape ReadMultiPoint(Stream data)
        {
            Shape result = new(FeatureType.MultiPoint);
            int count = ReadInt32(data);
            PartRange prt = new(FeatureType.MultiPoint)
            {
                NumVertices = count
            };
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
            ShapeRange sr = new(FeatureType.MultiPoint);
            PartRange prt = new(FeatureType.MultiPoint)
            {
                NumVertices = count
            };
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

        private static Shape ReadMultiPolygon(Stream data)
        {
            Shape result = new(FeatureType.Polygon);
            int numPolygons = ReadInt32(data);
            List<double[]> rings = new();
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
                        if (IsCounterClockwise(coords)) coords = ReverseCoords(coords);
                    }
                    else
                    {
                        // By shapefile standard, the holes should be counter clockwise.
                        if (!IsCounterClockwise(coords)) coords = ReverseCoords(coords);
                    }

                    PartRange lPrt = new(FeatureType.Polygon)
                    {
                        PartOffset = partOffset,
                        NumVertices = numPoints
                    };
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
            ShapeRange lShp = new(FeatureType.Polygon);
            List<double[]> rings = new();
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
                        if (IsCounterClockwise(coords)) coords = ReverseCoords(coords);
                    }
                    else
                    {
                        // By shapefile standard, the holes should be counter clockwise.
                        if (!IsCounterClockwise(coords)) coords = ReverseCoords(coords);
                    }

                    PartRange lPrt = new(FeatureType.Polygon)
                    {
                        PartOffset = partOffset,
                        NumVertices = numPoints
                    };
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
        /// This assumes that the byte order and shapetype have already been read.
        /// </summary>
        /// <param name="data">The data to read from.</param>
        /// <param name="results">The featureSetPack the read point gets added to.</param>
        private static void ReadPoint(Stream data, FeatureSetPack results)
        {
            ShapeRange sr = new(FeatureType.MultiPoint);
            PartRange prt = new(FeatureType.MultiPoint)
            {
                NumVertices = 1
            };
            sr.Parts.Add(prt);
            double[] coord = ReadDouble(data, 2);
            results.Add(coord, sr);
        }

        private static Shape ReadPolygon(Stream data)
        {
            Shape result = new(FeatureType.Polygon);
            int numRings = ReadInt32(data);
            List<double[]> rings = new();
            int partOffset = 0;
            for (int iRing = 0; iRing < numRings; iRing++)
            {
                int numPoints = ReadInt32(data); // ring structures are like a linestring without the final point.
                double[] coords = ReadDouble(data, 2 * numPoints);
                if (iRing == 0)
                {
                    // By shapefile standard, the shell should be clockwise
                    if (IsCounterClockwise(coords)) coords = ReverseCoords(coords);
                }
                else
                {
                    // By shapefile standard, the holes should be counter clockwise.
                    if (!IsCounterClockwise(coords)) coords = ReverseCoords(coords);
                }

                PartRange lPrt = new(FeatureType.Polygon)
                {
                    PartOffset = partOffset,
                    NumVertices = numPoints
                };
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
            ShapeRange lShp = new(FeatureType.Polygon);
            List<double[]> rings = new();
            int partOffset = 0;
            for (int iRing = 0; iRing < numRings; iRing++)
            {
                int numPoints = ReadInt32(data); // ring structures are like a linestring without the final point.
                double[] coords = ReadDouble(data, 2 * numPoints);
                if (iRing == 0)
                {
                    // By shapefile standard, the shell should be clockwise
                    if (IsCounterClockwise(coords)) coords = ReverseCoords(coords);
                }
                else
                {
                    // By shapefile standard, the holes should be counter clockwise.
                    if (!IsCounterClockwise(coords)) coords = ReverseCoords(coords);
                }

                PartRange lPrt = new(FeatureType.Polygon)
                {
                    PartOffset = partOffset,
                    NumVertices = numPoints
                };
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

        /// <summary>
        /// Fix introduced by JamesP@esdm.co.uk. 3/11/2010
        /// Using Array.Reverse does not work because it has the unwanted effect of flipping
        /// the X and Y values.
        /// </summary>
        /// <param name="coords">The double precision XY coordinate array of vertices.</param>
        /// <returns>The double array in reverse order.</returns>
        private static double[] ReverseCoords(double[] coords)
        {
            int numCoords = coords.Length;
            double[] newCoords = new double[numCoords];
            for (int i = numCoords - 1; i >= 0; i -= 2)
            {
                newCoords[i - 1] = coords[numCoords - i - 1]; // X
                newCoords[i] = coords[numCoords - i]; // Y
            }

            return newCoords;
        }

        #endregion
    }
}