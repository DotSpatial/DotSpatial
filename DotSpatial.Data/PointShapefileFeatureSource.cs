// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Kyle Ellison. Created 12/01/2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// |-----------------|----------|---------------------------------------------------------------------
// |      Name       |  Date    |                        Comments
// |-----------------|----------|----------------------------------------------------------------------
// | Kyle Ellison    |12/22/2010| Made FeatureType and ShapeType properties public
// |-----------------|----------|----------------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.IO;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// This class is strictly the vector access code.  This does not handle
    /// the attributes, which must be handled independantly.
    /// </summary>
    public class PointShapefileFeatureSource : ShapefileFeatureSource
    {
        /// <summary>
        /// Sets the fileName and creates a new PointShapefileFeatureSource for the specified file.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        public PointShapefileFeatureSource(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Sets the fileName and creates a new PointshapefileFeatureSource for the specified file (and builds spatial index if requested)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="useSpatialIndexing"></param>
        /// <param name="trackDeletedRows"></param>
        public PointShapefileFeatureSource(string fileName, bool useSpatialIndexing, bool trackDeletedRows)
            : base(fileName, useSpatialIndexing, trackDeletedRows)
        {
        }

        /// <inheritdocs/>
        public override FeatureType FeatureType
        {
            get { return FeatureType.Point; }
        }

        /// <inheritdocs/>
        public override ShapeType ShapeType
        {
            get { return ShapeType.Point; }
        }

        /// <inheritdocs/>
        public override ShapeType ShapeTypeM
        {
            get { return ShapeType.PointM; }
        }

        /// <inheritdocs/>
        public override ShapeType ShapeTypeZ
        {
            get { return ShapeType.PointZ; }
        }

        /// <inheritdocs/>
        protected override void AppendBasicGeometry(ShapefileHeader header, IBasicGeometry feature, int numFeatures)
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

            //                                              Index File
            //                                              ---------------------------------------------------------
            //                                              Position     Value               Type        Number      Byte Order
            //                                              ---------------------------------------------------------
            shxStream.WriteBe(offset);                      // Byte 0     Offset             Integer     1           Big
            shxStream.WriteBe(contentLength);               // Byte 4    Content Length      Integer     1           Big
            shxStream.Flush();
            shxStream.Close();
            //                                              X Y Points
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

            shpStream.WriteLe(point.X);             // Byte 12      X                   Double      1           Little
            shpStream.WriteLe(point.Y);             // Byte 20      Y                   Double      1           Little

            if (header.ShapeType == ShapeType.PointM)
            {
                shpStream.WriteLe(point.M);                            // Byte 28      M                   Double      1           Little
            }
            else if (header.ShapeType == ShapeType.PointZ)
            {
                shpStream.WriteLe(point.Z);                            // Byte 28      Z                   Double      1           Little
                shpStream.WriteLe(point.M);                            // Byte 36      M                   Double      1           Little
            }
            shpStream.Flush();
            shpStream.Close();
            offset += contentLength;
            Shapefile.WriteFileLength(Filename, offset + 4); // Add 4 for the record header
            Shapefile.WriteFileLength(header.ShxFilename, 50 + numFeatures * 4);
        }

        /// <inheritdocs/>
        public override IShapeSource CreateShapeSource()
        {
            return new PointShapefileShapeSource(Filename, Quadtree, null);
        }

        /// <inheritdocs/>
        public override void UpdateExtents()
        {
            UpdateExtents(new PointShapefileShapeSource(Filename));
        }

        /// <inheritdoc/>
        public override IFeatureSet Select(string filterExpression, IEnvelope envelope, ref int startIndex, int maxCount)
        {
            return Select(new PointShapefileShapeSource(Filename, Quadtree, null), filterExpression, envelope, ref startIndex,
                          maxCount);
        }

        /// <inheritdoc/>
        public override void SearchAndModifyAttributes(IEnvelope envelope, int chunkSize, FeatureSourceRowEditEvent rowCallback)
        {
            SearchAndModifyAttributes(new PointShapefileShapeSource(Filename, Quadtree, null), envelope, chunkSize, rowCallback);
        }
    }
}