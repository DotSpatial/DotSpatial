// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index;

namespace DotSpatial.Data
{
    /// <summary>
    /// Retrieves specific shapes.
    /// </summary>
    public class LineShapefileShapeSource : ShapefileShapeSource
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineShapefileShapeSource"/> class with the specified line shapefile as the source.
        /// </summary>
        /// <param name="fileName">The string fileName.</param>
        public LineShapefileShapeSource(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineShapefileShapeSource"/> class with the specified line shapefile as the source and provided indices.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="spatialIndex">The spatial index.</param>
        /// <param name="shx">The shapefile index file.</param>
        public LineShapefileShapeSource(string fileName, ISpatialIndex<int> spatialIndex, ShapefileIndexFile shx)
            : base(fileName, spatialIndex, shx)
        {
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public override FeatureType FeatureType => FeatureType.Line;

        /// <inheritdoc />
        protected override ShapeType ShapeType => ShapeType.PolyLine;

        /// <inheritdoc />
        protected override ShapeType ShapeTypeM => ShapeType.PolyLineM;

        /// <inheritdoc />
        protected override ShapeType ShapeTypeZ => ShapeType.PolyLineZ;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override Shape GetShapeAtIndex(FileStream fs, ShapefileIndexFile shx, ShapefileHeader header, int shp, Envelope envelope)
        {
            // Read from the index file because some deleted records
            // might still exist in the .shp file.
            long offset = shx.Shapes[shp].ByteOffset;
            fs.Seek(offset, SeekOrigin.Begin);
            Shape myShape = new();

            ShapeRange shape = new(FeatureType.Line) // Position     Value               Type        Number      Byte Order
            {
                RecordNumber = fs.ReadInt32(Endian.BigEndian),  // Byte 0       Record Number       Integer     1           Big
                ContentLength = fs.ReadInt32(Endian.BigEndian), // Byte 4       Content Length      Integer     1           Big
                ShapeType = (ShapeType)fs.ReadInt32(),          // Byte 8       Shape Type          Integer     1           Little
                StartIndex = 0
            };

            if (shape.ShapeType == ShapeType.NullShape)
            {
                return null;
            }

            myShape.Range = shape;

            double xMin = fs.ReadDouble(); // Byte 12      Xmin                Double      1           Little
            double yMin = fs.ReadDouble(); // Byte 20      Ymin                Double      1           Little
            double xMax = fs.ReadDouble(); // Byte 28      Xmax                Double      1           Little
            double yMax = fs.ReadDouble(); // Byte 36      Ymax                Double      1           Little
            shape.Extent = new Extent(xMin, yMin, xMax, yMax);

            // Don't add this shape to the result
            if (envelope != null)
            {
                if (!myShape.Range.Extent.Intersects(envelope))
                {
                    return null;
                }
            }

            shape.NumParts = fs.ReadInt32(); // Byte 44      NumParts            Integer     1           Little

            // feature.NumPoints = bbReader.ReadInt32();             // Byte 48      NumPoints           Integer     1           Little
            shape.NumPoints = fs.ReadInt32();

            // Create an envelope from the extents box in the file.
            // feature.Envelope = new Envelope(xMin, xMax, yMin, yMax);
            int[] partIndices = fs.ReadInt32(shape.NumParts);
            myShape.Vertices = fs.ReadDouble(shape.NumPoints * 2);

            if (header.ShapeType == ShapeType.PolyLineM)
            {
                // These are listed as "optional" but there isn't a good indicator of how to determine if they were added.
                // To handle the "optional" M values, check the contentLength for the feature.
                // The content length does not include the 8-byte record header and is listed in 16-bit words.
                if (shape.ContentLength * 2 > 44 + (4 * shape.NumParts) + (16 * shape.NumPoints))
                {
                    myShape.MinM = fs.ReadDouble();
                    myShape.MaxM = fs.ReadDouble();

                    myShape.M = fs.ReadDouble(shape.NumPoints);
                    shape.Extent = new ExtentM(shape.Extent, myShape.MinM, myShape.MaxM);
                }
            }
            else if (header.ShapeType == ShapeType.PolyLineZ)
            {
                bool hasM = shape.ContentLength * 2 > 60 + (4 * shape.NumParts) + (24 * shape.NumPoints);
                myShape.MinZ = fs.ReadDouble();
                myShape.MaxZ = fs.ReadDouble();

                // For Z shapefiles, the Z part is not optional.
                myShape.Z = fs.ReadDouble(shape.NumPoints);

                // These are listed as "optional" but there isn't a good indicator of how to determine if they were added.
                // To handle the "optional" M values, check the contentLength for the feature.
                // The content length does not include the 8-byte record header and is listed in 16-bit words.)
                if (hasM)
                {
                    myShape.MinM = fs.ReadDouble();
                    myShape.MaxM = fs.ReadDouble();
                    myShape.M = fs.ReadDouble(shape.NumPoints);
                    shape.Extent = new ExtentMz(shape.Extent.MinX, shape.Extent.MinY, myShape.MinM, myShape.MinZ, shape.Extent.MaxX, shape.Extent.MaxY, myShape.MaxM, myShape.MaxZ);
                }
                else
                {
                    shape.Extent = new ExtentMz(shape.Extent.MinX, shape.Extent.MinY, double.MaxValue, myShape.MinZ, shape.Extent.MaxX, shape.Extent.MaxY, double.MinValue, myShape.MaxZ);
                }
            }

            myShape.Range = shape;

            for (int part = 0; part < shape.NumParts; part++)
            {
                int partOff = partIndices[part];
                int pointCount = shape.NumPoints - partOff;
                if (part < shape.NumParts - 1)
                {
                    pointCount = partIndices[part + 1] - partOff;
                }

                PartRange partR = new(myShape.Vertices, 0, partOff, FeatureType.Line)
                {
                    NumVertices = pointCount
                };
                shape.Parts.Add(partR);
            }

            return myShape;
        }

        #endregion
    }
}