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
// The Initial Developer of this Original Code is Kyle Ellison. Created 11/23/2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|----------|---------------------------------------------------------------------
// |      Name       |  Date    |                        Comments
// |-----------------|----------|----------------------------------------------------------------------
// | Kyle Ellison    |12/02/2010| Pushed common code into base class and derived from it.
// | Kyle Ellison    |12/15/2010| Added method to get multiple shapes by index values, and consolidated code.
// |-----------------|----------|----------------------------------------------------------------------
// ********************************************************************************************************

using System.IO;
using DotSpatial.Topology;
using DotSpatial.Topology.Index;

namespace DotSpatial.Data
{
    /// <summary>
    /// Retrieves specific shapes.
    /// </summary>
    public class PointShapefileShapeSource : ShapefileShapeSource
    {
        /// <summary>
        /// Creates a new instance of the PointShapefileShapeSource with the specified point
        /// shapefile as the source.
        /// </summary>
        /// <param name="fileName">The string fileName</param>
        public PointShapefileShapeSource(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Creates a new instance of the PointShapefileShapeSource with the specified polygon shapefile as the source and provided indices
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="spatialIndex"></param>
        /// <param name="shx"></param>
        public PointShapefileShapeSource(string fileName, ISpatialIndex spatialIndex, ShapefileIndexFile shx)
            : base(fileName, spatialIndex, shx)
        {
        }

        /// <inheritdocs/>
        protected override ShapeType ShapeType
        {
            get { return ShapeType.Point; }
        }

        /// <inheritdocs/>
        protected override ShapeType ShapeTypeM
        {
            get { return ShapeType.PointM; }
        }

        /// <inheritdocs/>
        protected override ShapeType ShapeTypeZ
        {
            get { return ShapeType.PointZ; }
        }

        /// <inheritdocs/>
        public override FeatureType FeatureType
        {
            get { return FeatureType.Point; }
        }

        /// <inheritdocs/>
        protected override Shape GetShapeAtIndex(FileStream fs, ShapefileIndexFile shx, ShapefileHeader header, int shp, IEnvelope envelope)
        {
            // Read from the index file because some deleted records
            // might still exist in the .shp file.
            long offset = (shx.Shapes[shp].ByteOffset);
            fs.Seek(offset, SeekOrigin.Begin);
            Shape myShape = new Shape();
            // Position     Value               Type        Number      Byte Order
            ShapeRange shape = new ShapeRange(FeatureType.Point); //--------------------------------------------------------------------
            shape.RecordNumber = fs.ReadInt32(Endian.BigEndian);     // Byte 0       Record Number       Integer     1           Big
            shape.ContentLength = fs.ReadInt32(Endian.BigEndian);    // Byte 4       Content Length      Integer     1           Big
            ShapeType shapeType = (ShapeType)fs.ReadInt32(); // Byte 8       Shape Type          Integer     1           Little
            if (shapeType == ShapeType.NullShape)
            {
                return null;
            }
            double[] vertices = fs.ReadDouble(2);
            double x = vertices[0], y = vertices[1];
            // Don't add this shape to the result
            if (envelope != null)
            {
                if (!envelope.Contains(new Coordinate(x, y)))
                {
                    return null;
                }
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
                shape.Extent = new ExtentMZ(shape.Extent.MinX, shape.Extent.MinY, myShape.MinM, myShape.MinZ, shape.Extent.MaxX, shape.Extent.MaxY, myShape.MaxM, myShape.MaxZ);
            }

            PartRange partR = new PartRange(myShape.Vertices, 0, 0, FeatureType.Point) { NumVertices = 1 };
            shape.Parts.Add(partR);
            myShape.Range = shape;

            return myShape;
        }
    }
}