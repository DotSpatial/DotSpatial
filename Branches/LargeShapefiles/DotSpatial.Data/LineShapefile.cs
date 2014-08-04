// *******************************************************************************************************
// Product: DotSpatial.Data.LineShapefile.cs
// Description:  A shapefile class that handles the special case where the vectors are lines.
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
    /// A shapefile class that handles the special case where the vectors are lines
    /// </summary>
    public class LineShapefile : Shapefile
    {
        #region Ctor

        /// <summary>
        /// Creates a new instance of a LineShapefile for in-ram handling only.
        /// </summary>
        public LineShapefile()
            : base(FeatureType.Line)
        {
        }

        /// <summary>
        /// Creates a new instance of a LineShapefile that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public LineShapefile(string fileName)
            : this(fileName, false)
        {
        }
        

        protected LineShapefile(string fileName, bool isPolygon)
            : base(fileName, isPolygon? FeatureType.Polygon: FeatureType.Line)
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
            int pointsNum = 0, numParts = 0;
            double xmin = 0, ymin = 0, xmax = 0, ymax = 0, zmin = 0, zmax = 0, mmin = 0, mmax = 0;
            double[] vert = null, m = null, z = null;
            int[] parts = null;

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

            // The number of parts in the PolyLine.
            numParts = reader.ReadInt32(); byteLen += 4;

            // The total number of points for all parts.
            pointsNum = reader.ReadInt32(); byteLen += 4;

            // Initialize arrays
            vert = new double[2 * pointsNum]; // X, Y
            parts = new int[numParts];
            if (CoordinateType == CoordinateType.M || CoordinateType == CoordinateType.Z)
            {
                m = Enumerable.Repeat(double.MinValue, pointsNum).ToArray();
            }
            if (CoordinateType == CoordinateType.Z)
            {
                z = new double[pointsNum];
            }

            var partsBytes = reader.ReadBytes(4 * numParts); byteLen += 4*numParts;
            Buffer.BlockCopy(partsBytes, 0, parts, 0, partsBytes.Length);

            var pointsBytes = reader.ReadBytes(8 * 2 * pointsNum); byteLen += 8*2*pointsNum;
            Buffer.BlockCopy(pointsBytes, 0, vert, 0, pointsBytes.Length);

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
                var totalVertices = 0;
                for (var i = 0; i < numParts; i++)
                {
                    int numVertices, partOffset;
                    if (numParts == 1)
                    {
                        numVertices = pointsNum;
                        partOffset = 0;
                    }
                    else
                    {
                        numVertices = i == numParts - 1
                            ? (vert.Length + 1 - parts[i])/2
                            : (parts[i + 1] - 1 - parts[i])/2;

                        partOffset = totalVertices;
                        totalVertices += numVertices;
                    }


                    var partRange = new PartRange(vert, 0, partOffset, FeatureType) { NumVertices = numVertices };
                    shr.Parts.Add(partRange);    
                }
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
            throw new NotImplementedException();
        }
    }
}