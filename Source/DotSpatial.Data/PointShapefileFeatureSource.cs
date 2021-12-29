// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// This class is strictly the vector access code. This does not handle
    /// the attributes, which must be handled independently.
    /// </summary>
    public class PointShapefileFeatureSource : ShapefileFeatureSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointShapefileFeatureSource"/> class from the specified file.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        public PointShapefileFeatureSource(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointShapefileFeatureSource"/> class from the specified file and builds spatial index if requested.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        /// <param name="useSpatialIndexing">Indicates whether the spatial index should be build.</param>
        /// <param name="trackDeletedRows">Indicates whether deleted records should be tracked.</param>
        public PointShapefileFeatureSource(string fileName, bool useSpatialIndexing, bool trackDeletedRows)
            : base(fileName, useSpatialIndexing, trackDeletedRows)
        {
        }

        /// <inheritdoc />
        public override FeatureType FeatureType => FeatureType.Point;

        /// <inheritdoc />
        public override ShapeType ShapeType => ShapeType.Point;

        /// <inheritdoc />
        public override ShapeType ShapeTypeM => ShapeType.PointM;

        /// <inheritdoc />
        public override ShapeType ShapeTypeZ => ShapeType.PointZ;

        /// <inheritdoc />
        public override IShapeSource CreateShapeSource()
        {
            return new PointShapefileShapeSource(Filename, Quadtree, null);
        }

        /// <inheritdoc />
        public override void UpdateExtents()
        {
            UpdateExtents(new PointShapefileShapeSource(Filename));
        }

        /// <inheritdoc/>
        public override IFeatureSet Select(string filterExpression, Envelope envelope, ref int startIndex, int maxCount)
        {
            return Select(new PointShapefileShapeSource(Filename, Quadtree, null), filterExpression, envelope, ref startIndex, maxCount);
        }

        /// <inheritdoc/>
        public override void SearchAndModifyAttributes(Envelope envelope, int chunkSize, FeatureSourceRowEditEvent rowCallback)
        {
            SearchAndModifyAttributes(new PointShapefileShapeSource(Filename, Quadtree, null), envelope, chunkSize, rowCallback);
        }

        /// <inheritdoc />
        protected override void AppendGeometry(ShapefileHeader header, Geometry feature, int numFeatures)
        {
            var fi = new FileInfo(Filename);
            int offset = Convert.ToInt32(fi.Length / 2);

            var shpStream = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.None, 10000);
            var shxStream = new FileStream(header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 100);

            Coordinate point = feature.Coordinates[0];
            int contentLength = 10;
            if (header.ShapeType == ShapeType.PointM)
            {
                contentLength += 4; // one additional value (m)
            }

            if (header.ShapeType == ShapeType.PointZ)
            {
                contentLength += 8; // 2 additional values (m, z)
            }

            ////                                            Index File
            //                                              ---------------------------------------------------------
            //                                              Position     Value               Type        Number      Byte Order
            //                                              ---------------------------------------------------------
            shxStream.WriteBe(offset);                      // Byte 0     Offset             Integer     1           Big
            shxStream.WriteBe(contentLength);               // Byte 4    Content Length      Integer     1           Big
            shxStream.Flush();
            shxStream.Close();
            ////                                            X Y Points
            //                                              ---------------------------------------------------------
            //                                              Position     Value               Type        Number      Byte Order
            //                                              ---------------------------------------------------------
            shpStream.WriteBe(numFeatures);                 // Byte 0       Record Number       Integer     1           Big
            shpStream.WriteBe(contentLength);               // Byte 4       Content Length      Integer     1           Big
            shpStream.WriteLe((int)header.ShapeType);       // Byte 8       Shape Type 3        Integer     1           Little
            if (header.ShapeType == ShapeType.NullShape)
            {
                return;
            }

            shpStream.WriteLe(point.X);                     // Byte 12      X                   Double      1           Little
            shpStream.WriteLe(point.Y);                     // Byte 20      Y                   Double      1           Little

            if (header.ShapeType == ShapeType.PointM)
            {
                shpStream.WriteLe(point.M);                 // Byte 28      M                   Double      1           Little
            }
            else if (header.ShapeType == ShapeType.PointZ)
            {
                shpStream.WriteLe(point.Z);                 // Byte 28      Z                   Double      1           Little
                shpStream.WriteLe(point.M);                 // Byte 36      M                   Double      1           Little
            }

            shpStream.Flush();
            shpStream.Close();
            offset += contentLength;
            Shapefile.WriteFileLength(Filename, offset + 4); // Add 4 for the record header
            Shapefile.WriteFileLength(header.ShxFilename, 50 + numFeatures * 4);
        }
    }
}