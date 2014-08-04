// *******************************************************************************************************
// Product: DotSpatial.Data.MultiPointShapefile.cs
// Description:  A shapefile class that handles the special case where each features has multiple points.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Max Miroshnikov    |  07/2014           | Created.
// *******************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where each features has multiple points
    /// </summary>
    public class MultiPointShapefile : Shapefile
    {
        #region Ctor

        /// <summary>
        /// Creates a new instance of a MultiPointShapefile for in-ram handling only.
        /// </summary>
        public MultiPointShapefile()
            : base(FeatureType.MultiPoint)
        {
        }

        /// <summary>
        /// Creates a new instance of a MultiPointShapefile that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public MultiPointShapefile(string fileName)
            : base(fileName, FeatureType.MultiPoint)
        {
        }

        #endregion

        protected override Extent ReadExtent(int shp, Stream reader)
        {
            var shapeHeaders = ShapeHeaders;
            reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);
            reader.ReadInt32(Endian.BigEndian); // Record Number
            reader.ReadInt32(Endian.BigEndian); // Content Len
            var shapeType = (ShapeType)reader.ReadInt32();
            if (shapeType == ShapeType.NullShape) return new Extent();

            var xmin = reader.ReadDouble();
            var ymin = reader.ReadDouble();
            var xmax = reader.ReadDouble();
            var ymax = reader.ReadDouble();

            return new Extent(xmin, ymin, xmax, ymax);
        }

        protected override Shape ReadShape(int shp, Stream reader)
        {
            var pointsNum = 0;
            double xmin = 0, ymin = 0, xmax = 0, ymax = 0, zmin = 0, zmax = 0, mmin = 0, mmax = 0;
            double[] vert = null, m = null, z = null;

            var shapeHeaders = ShapeHeaders;
            reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);

            var recordNumber = reader.ReadInt32(Endian.BigEndian);
            Debug.Assert(recordNumber == shp + 1);
            var contentLen = reader.ReadInt32(Endian.BigEndian);
            Debug.Assert(contentLen == shapeHeaders[shp].ContentLength);

            var shapeType = (ShapeType)reader.ReadInt32();
            if (shapeType == ShapeType.NullShape) goto fin;

            var byteLen = 4; // from shapeType read

            // Read bounding box
            xmin = reader.ReadDouble(); byteLen += 8;
            ymin = reader.ReadDouble(); byteLen += 8;
            xmax = reader.ReadDouble(); byteLen += 8;
            ymax = reader.ReadDouble(); byteLen += 8;

            // Read num points
            pointsNum = reader.ReadInt32(); byteLen += 4;

            // Initialize points arrays
            vert = new double[2 * pointsNum]; // X, Y
            if (CoordinateType == CoordinateType.M || CoordinateType == CoordinateType.Z)
            {
                m = Enumerable.Repeat(double.MinValue, pointsNum).ToArray();
            }
            if (CoordinateType == CoordinateType.Z)
            {
                z = new double[pointsNum];
            }

            // Read X and Y coordinates
            for (var i = 0; i < pointsNum; i += 2)
            {
                vert[i] = reader.ReadDouble(); byteLen += 8;    // X
                vert[i+1] = reader.ReadDouble(); byteLen += 8;  // Y
            }

            // Read Z
            if (z != null)
            {
                zmin = reader.ReadDouble(); byteLen += 8;
                zmax = reader.ReadDouble(); byteLen += 8;

                for (var i = 0; i < pointsNum; i++)
                {
                    z[i] = reader.ReadDouble(); byteLen += 8;
                }
            }

            // Read M
            if (m != null)
            {
                if (byteLen + 2 * 8 + pointsNum * 8 == shapeHeaders[shp].ByteLength) // mmin, mmax and pointsNum doubles
                {
                    mmin = reader.ReadDouble(); byteLen += 8;
                    mmax = reader.ReadDouble(); byteLen += 8;

                    for (var i = 0; i < pointsNum; i++)
                    {
                        m[i] = reader.ReadDouble(); byteLen += 8;
                    }
                }
            }

       fin:
            var shr = new ShapeRange(FeatureType)
            {
                RecordNumber = recordNumber,
                StartIndex = 0,
                ContentLength = shapeHeaders[shp].ContentLength,
                NumPoints = pointsNum,
                Extent = new Extent(xmin, ymin, xmax, ymax),
                ShapeType = shapeType,
            };
            if (vert != null)
            {
                shr.Parts.Add(new PartRange(vert, 0, 0, FeatureType) { NumVertices = pointsNum });
            }
            return new Shape(FeatureType)
            {
                Range = shr,
                M = m,
                Z = z,
                Vertices = vert,
                MaxM = mmax,
                MinM = mmin,
                MaxZ = zmax,
                MinZ = zmin
            };
        }

        protected override void WriteFeatures(string fileName)
        {
            // Set Header.ShapeType before setting extent.
            switch (CoordinateType)
            {
                case CoordinateType.Regular:
                    Header.ShapeType = ShapeType.MultiPoint;
                    break;
                case CoordinateType.M:
                    Header.ShapeType = ShapeType.MultiPointM;
                    break;
                case CoordinateType.Z:
                    Header.ShapeType = ShapeType.MultiPointZ;
                    break;
                default:
                    throw new Exception("Unsupported CoordinateType");
            }

            // Calculate total .shp file length
            var totalOffset = 50;
            for (var shp = 0; shp < Count; shp++)
            {
                var f = GetFeature(shp);
                totalOffset += GetContentLength(f);
            }

            // Save headers for .shp and .shx files
            InvalidateEnvelope();
            Header.SetExtent(Extent);
            Header.ShxLength = 50 + Count * 4;
            Header.FileLength = totalOffset;
            Header.SaveAs(fileName);

            // Reset shapeheaders
            ResetShapeHeaders();

            // Append data into .shp and .shx
            int offset = 50;
            using (var shpStream = new FileStream(Header.Filename, FileMode.Append))
            using (var shxStream = new FileStream(Header.ShxFilename, FileMode.Append))
            {
                for (var shp = 0; shp < Count; shp++)
                {
                    shpStream.WriteBe(shp + 1);
                    var feature = GetFeature(shp);
                    var shpt = feature.ShapeType.GetValueOrDefault(Header.ShapeType);
                    var contentLen = GetContentLength(feature);

                    shpStream.WriteBe(contentLen - 4);
                    shpStream.WriteLe((int)shpt);
                    if (shpt != ShapeType.NullShape)
                    {
                        // Bounding Box
                        var extent = feature.Envelope.ToExtent();
                        shpStream.WriteLe(extent.MinX);
                        shpStream.WriteLe(extent.MinY);
                        shpStream.WriteLe(extent.MaxX);
                        shpStream.WriteLe(extent.MaxY);

                        // NumPoints
                        shpStream.WriteLe(feature.NumPoints);

                        // XY coordinates
                        for (int i = 0; i < feature.NumPoints; i++)
                        {
                            shpStream.WriteLe(feature.Coordinates[i].X);
                            shpStream.WriteLe(feature.Coordinates[i].Y);
                        }

                        // Z coordinates
                        if (shpt == ShapeType.MultiPointZ)
                        {
                            // Z-box
                            var minZ = feature.Coordinates.Min(_ => _.Z);
                            var maxZ = feature.Coordinates.Max(_ => _.Z);
                            shpStream.WriteLe(minZ);
                            shpStream.WriteLe(maxZ);

                            // Z coordinates
                            for (var i = 0; i < feature.NumPoints; i++)
                            {
                                shpStream.WriteLe(feature.Coordinates[i].Z);
                            }
                        }

                        // M coordinates
                        if (shpt == ShapeType.MultiPointZ || shpt == ShapeType.MultiPointM)
                        {
                            // M-box
                            var minm = feature.Coordinates.Min(_ => _.M);
                            var maxm = feature.Coordinates.Max(_ => _.M);
                            shpStream.WriteLe(minm);
                            shpStream.WriteLe(maxm);

                            // M coordinates
                            for (var i = 0; i < feature.NumPoints; i++)
                            {
                                shpStream.WriteLe(feature.Coordinates[i].M);
                            }
                        }
                    }

                    shxStream.WriteBe(offset);
                    shxStream.WriteBe(contentLen - 4);
                    offset += contentLen;
                }
            }
        }

        private int GetContentLength(IFeature f)
        {
            var shpt = f.ShapeType.GetValueOrDefault(Header.ShapeType);

            var baseLen = 3 * 2; // 3 ints
            switch (shpt)
            {
                case ShapeType.MultiPoint:
                    // Bounding Box, NumPoints, XY-coords
                    baseLen += 4 * 4 + 2 + f.NumPoints * 8;
                    break;
                case ShapeType.MultiPointM:
                    // Bounding Box, NumPoints, XY-points, M-box, M-coords
                    baseLen += 4 * 4 + 2 + f.NumPoints * 8 + 4 * 2 + 4 * f.NumPoints;
                    break;
                case ShapeType.MultiPointZ:
                    // Bounding Box, NumPoints, XY-points, M-box, M-coords, Z-box, Z-coords
                    baseLen += 4 * 4 + 2 + f.NumPoints * 8 + 4 * 2 + 4 * f.NumPoints + 4 * 2 + 4 * f.NumPoints;
                    break;
            }

            return baseLen;
        }
    }
}
