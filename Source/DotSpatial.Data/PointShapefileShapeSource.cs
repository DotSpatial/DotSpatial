// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using GeoAPI.Geometries;
using NetTopologySuite.Index;

namespace DotSpatial.Data
{
    /// <summary>
    /// Retrieves specific shapes.
    /// </summary>
    public class PointShapefileShapeSource : ShapefileShapeSource
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointShapefileShapeSource"/> class with the specified point
        /// shapefile as the source.
        /// </summary>
        /// <param name="fileName">The string fileName</param>
        public PointShapefileShapeSource(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointShapefileShapeSource"/> class with the specified polygon shapefile as the source and provided indices
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="spatialIndex">The spatial index.</param>
        /// <param name="shx">The shapefile index file.</param>
        public PointShapefileShapeSource(string fileName, ISpatialIndex<int> spatialIndex, ShapefileIndexFile shx)
            : base(fileName, spatialIndex, shx)
        {
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public override FeatureType FeatureType => FeatureType.Point;

        /// <inheritdoc />
        protected override ShapeType ShapeType => ShapeType.Point;

        /// <inheritdoc />
        protected override ShapeType ShapeTypeM => ShapeType.PointM;

        /// <inheritdoc />
        protected override ShapeType ShapeTypeZ => ShapeType.PointZ;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override Shape GetShapeAtIndex(FileStream fs, ShapefileIndexFile shx, ShapefileHeader header, int shp, Envelope envelope)
        {
            // Read from the index file because some deleted records
            // might still exist in the .shp file.
            long offset = shx.Shapes[shp].ByteOffset;
            fs.Seek(offset, SeekOrigin.Begin);
            Shape myShape = new Shape();

            ShapeRange shape = new ShapeRange(FeatureType.Point) // Position     Value               Type        Number      Byte Order
            {
                RecordNumber = fs.ReadInt32(Endian.BigEndian),  // Byte 0       Record Number       Integer     1           Big
                ContentLength = fs.ReadInt32(Endian.BigEndian), // Byte 4       Content Length      Integer     1           Big
            };

            ShapeType shapeType = (ShapeType)fs.ReadInt32();    // Byte 8       Shape Type          Integer     1           Little
            if (shapeType == ShapeType.NullShape)
            {
                return null;
            }

            double[] vertices = fs.ReadDouble(2);
            double x = vertices[0], y = vertices[1];

            // Don't add this shape to the result
            if (envelope != null && !envelope.Contains(new Coordinate(x, y)))
            {
                return null;
            }

            shape.StartIndex = 0;
            shape.NumParts = 1;
            shape.NumPoints = 1;
            shape.ShapeType = shapeType;
            shape.Extent = new Extent(x, y, x, y);
            myShape.Range = shape;
            myShape.Vertices = vertices;

            if (header.ShapeType == ShapeType.PointM)
            {
                myShape.M = fs.ReadDouble(1);
                myShape.MinM = myShape.MaxM = myShape.M[0];
                shape.Extent = new ExtentM(shape.Extent, myShape.MinM, myShape.MaxM);
            }
            else if (header.ShapeType == ShapeType.PointZ)
            {
                // For Z shapefiles, the Z part is not optional.
                myShape.Z = fs.ReadDouble(1);
                myShape.MinZ = myShape.MaxZ = myShape.Z[0];
                myShape.M = fs.ReadDouble(1);
                myShape.MinM = myShape.MaxM = myShape.M[0];
                shape.Extent = new ExtentMz(shape.Extent.MinX, shape.Extent.MinY, myShape.MinM, myShape.MinZ, shape.Extent.MaxX, shape.Extent.MaxY, myShape.MaxM, myShape.MaxZ);
            }

            PartRange partR = new PartRange(myShape.Vertices, 0, 0, FeatureType.Point)
            {
                NumVertices = 1
            };
            shape.Parts.Add(partR);
            myShape.Range = shape;

            return myShape;
        }

        #endregion
    }
}