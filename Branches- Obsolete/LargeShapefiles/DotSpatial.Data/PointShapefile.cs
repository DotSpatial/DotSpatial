// *******************************************************************************************************
// Product: DotSpatial.Data.PointShapefile.cs
// Description: A shapefile class that handles the special case where the data type is point.
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
    /// A shapefile class that handles the special case where the data type is point
    /// </summary>
    public class PointShapefile : Shapefile
    {
        #region Ctor

        /// <summary>
        /// Creates a new instance of a PointShapefile for in-ram handling only.
        /// </summary>
        public PointShapefile()
            : base(FeatureType.Point)
        {
        }

        /// <summary>
        /// Creates a new instance of a PointShapefile that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public PointShapefile(string fileName)
            : base(fileName, FeatureType.Point)
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
            var x = reader.ReadDouble();
            var y = reader.ReadDouble();
            return new Extent(x, y, x, y);
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

            // Initialize points arrays
            pointsNum = 1;
            vert = new double[2 * pointsNum];
            if (CoordinateType == CoordinateType.M || CoordinateType == CoordinateType.Z)
            {
                m = Enumerable.Repeat(double.MinValue, pointsNum).ToArray();
            }
            if (CoordinateType == CoordinateType.Z)
            {
                z = new double[pointsNum];
            }

            // Read X, Y
            vert[0] = reader.ReadDouble(); byteLen += 8;
            vert[1] = reader.ReadDouble(); byteLen += 8;
            xmin = xmax = vert[0];
            ymin = ymax = vert[1];

            // Read Z
            if (z != null)
            {
                z[0] = reader.ReadDouble(); byteLen += 8;
                zmin = zmax = z[0];
            }

            // Read M
            if (m != null)
            {
                if (byteLen + 8 == shapeHeaders[shp].ByteLength) // +8 means read one double
                {
                    m[0] = reader.ReadDouble(); byteLen += 8;
                    mmin = mmax = m[0];
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
                shr.Parts.Add(new PartRange(vert, 0, 0, FeatureType) { NumVertices = 1 });
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
            // wordSize is the length of the byte representation in 16 bit words of a single shape, including header.
            int wordSize;
            switch (CoordinateType)
            {
                case CoordinateType.Regular:
                    Header.ShapeType = ShapeType.Point;
                    wordSize = 14; // 3 int(2) and 2 double(4) = X,Y
                    break;
                case CoordinateType.M:
                    Header.ShapeType = ShapeType.PointM;
                    wordSize = 18; // 3 int(2), 3 double(4) = X,Y,M
                    break;
                case CoordinateType.Z:
                    Header.ShapeType = ShapeType.PointZ;
                    wordSize = 22; // 3 int(2), 4 double (4) = X,Y,Z,M
                    break;
                default:
                    throw new Exception("Unsupported CoordinateType");
            }

            // Calculate total .shp file length
            var totalOffset = 50;
            for (var shp = 0; shp < Count; shp++)
            {
                var f = GetFeature(shp);
                var shpt = f.ShapeType.GetValueOrDefault(Header.ShapeType);
                totalOffset += shpt == ShapeType.NullShape ? 6 : wordSize;
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
                    var contentLen = shpt == ShapeType.NullShape ? 2 : wordSize - 4;

                    shpStream.WriteBe(contentLen);
                    shpStream.WriteLe((int)shpt);
                    if (shpt != ShapeType.NullShape)
                    {
                        var coordinate = feature.Coordinates[0];
                        shpStream.WriteLe(coordinate.X);
                        shpStream.WriteLe(coordinate.Y);
                        if (shpt == ShapeType.PointZ)
                            shpStream.WriteLe(coordinate.Z);
                        if (shpt == ShapeType.PointZ || shpt == ShapeType.PointM)
                            shpStream.WriteLe(coordinate.M);
                    }

                    shxStream.WriteBe(offset);
                    shxStream.WriteBe(contentLen);
                    offset += shpt == ShapeType.NullShape ? 6 : wordSize;
                }
            }
        }
    }
}