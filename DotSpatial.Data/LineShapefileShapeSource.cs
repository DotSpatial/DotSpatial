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
// The Initial Developer of this Original Code is Kyle Ellison. Created 11/29/2010.
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
    public class LineShapefileShapeSource : ShapefileShapeSource
    {
        /// <summary>
        /// Creates a new instance of the LineShapefileShapeSource with the specified line
        /// shapefile as the source.
        /// </summary>
        /// <param name="fileName">The string fileName</param>
        public LineShapefileShapeSource(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Creates a new instance of the LineShapefileShapeSource with the specified polygon shapefile as the source and provided indices
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="spatialIndex"></param>
        /// <param name="shx"></param>
        public LineShapefileShapeSource(string fileName, ISpatialIndex spatialIndex, ShapefileIndexFile shx)
            : base(fileName, spatialIndex, shx)
        {
        }

        /// <inheritdocs/>
        protected override ShapeType ShapeType
        {
            get { return ShapeType.PolyLine; }
        }

        /// <inheritdocs/>
        protected override ShapeType ShapeTypeM
        {
            get { return ShapeType.PolyLineM; }
        }

        /// <inheritdocs/>
        protected override ShapeType ShapeTypeZ
        {
            get { return ShapeType.PolyLineZ; }
        }

        /// <inheritdocs/>
        public override FeatureType FeatureType
        {
            get { return FeatureType.Line; }
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
            ShapeRange shape = new ShapeRange(FeatureType.Line);
            //--------------------------------------------------------------------
            shape.RecordNumber = fs.ReadInt32(Endian.BigEndian);
            // Byte 0       Record Number       Integer     1           Big
            shape.ContentLength = fs.ReadInt32(Endian.BigEndian);
            // Byte 4       Content Length      Integer     1           Big
            shape.ShapeType = (ShapeType)fs.ReadInt32();
            // Byte 8       Shape Type          Integer     1           Little
            shape.StartIndex = 0;
            if (shape.ShapeType == ShapeType.NullShape)
            {
                return null;
            }

            myShape.Range = shape;

            //bbReader.Read(allBounds, shp*32, 32);
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
            //feature.NumPoints = bbReader.ReadInt32();             // Byte 48      NumPoints           Integer     1           Little
            shape.NumPoints = fs.ReadInt32();

            // Create an envelope from the extents box in the file.
            //feature.Envelope = new Envelope(xMin, xMax, yMin, yMax);
            int[] partIndices = fs.ReadInt32(shape.NumParts);
            myShape.Vertices = fs.ReadDouble(shape.NumPoints * 2);

            if (header.ShapeType == ShapeType.PolyLineM)
            {
                // These are listed as "optional" but there isn't a good indicator of how to determine if they were added.
                // To handle the "optional" M values, check the contentLength for the feature.
                // The content length does not include the 8-byte record header and is listed in 16-bit words.
                if (shape.ContentLength * 2 > 44 + 4 * shape.NumParts + 16 * shape.NumPoints)
                {
                    myShape.MinM = fs.ReadDouble();
                    myShape.MaxM = fs.ReadDouble();

                    myShape.M = fs.ReadDouble(shape.NumPoints);
                    shape.Extent = new ExtentM(shape.Extent, myShape.MinM, myShape.MaxM);
                }
            }
            else if (header.ShapeType == ShapeType.PolyLineZ)
            {
                bool hasM = shape.ContentLength * 2 > 60 + 4 * shape.NumParts + 24 * shape.NumPoints;
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
                    shape.Extent = new ExtentMZ(shape.Extent.MinX, shape.Extent.MinY, myShape.MinM, myShape.MinZ, shape.Extent.MaxX, shape.Extent.MaxY, myShape.MaxM, myShape.MaxZ);
                }
                else
                    shape.Extent = new ExtentMZ(shape.Extent.MinX, shape.Extent.MinY, double.MaxValue, myShape.MinZ, shape.Extent.MaxX, shape.Extent.MaxY, double.MinValue, myShape.MaxZ);
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
                PartRange partR = new PartRange(myShape.Vertices, 0, partOff, FeatureType.Line) { NumVertices = pointCount };
                shape.Parts.Add(partR);
            }
            return myShape;
        }
    }
}