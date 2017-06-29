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
// The Initial Developer of this Original Code is Ted Dunsford. Created in February, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
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
        private const int BLOCKSIZE = 16000000;

        /// <summary>
        /// Creates a new instance of a MultiPointShapefile for in-ram handling only.
        /// </summary>
        public MultiPointShapefile()
            : base(FeatureType.MultiPoint)
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader { FileLength = 100, ShapeType = ShapeType.MultiPoint };
        }

        /// <summary>
        /// Creates a new instance of a MultiPointShapefile that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public MultiPointShapefile(string fileName)
            : this()
        {
            Open(fileName, null);
        }

        /// <summary>
        /// Gets the feature at the specified index offset
        /// </summary>
        /// <param name="index">the integer index</param>
        /// <returns>An IFeature created from the shape at the specified offset</returns>
        public override IFeature GetFeature(int index)
        {
            IFeature f;
            if (!IndexMode)
            {
                f = Features[index];
            }
            else
            {
                f = GetMultiPoint(index);
                f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
            }
            return f;
        }

        /// <summary>
        /// Opens a shapefile
        /// </summary>
        /// <param name="fileName">The string fileName of the point shapefile to load</param>
        /// <param name="progressHandler">Any valid implementation of the DotSpatial.Data.IProgressHandler</param>
        public void Open(string fileName, IProgressHandler progressHandler)
        {
            IndexMode = true;
            Filename = fileName;
            FeatureType = FeatureType.MultiPoint;
            Header = new ShapefileHeader(fileName);
            CoordinateType = CoordinateType.Regular;
            if (Header.ShapeType == ShapeType.MultiPointM)
            {
                CoordinateType = CoordinateType.M;
            }
            if (Header.ShapeType == ShapeType.MultiPointZ)
            {
                CoordinateType = CoordinateType.Z;
            }
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(fileName);
            FillPoints(fileName, progressHandler);
            ReadProjection();
        }

        // X Y MultiPoints: Total Length = 28 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 8        Integer     1           Little
        // Byte 12      Xmin                Double      1           Little
        // Byte 20      Ymin                Double      1           Little
        // Byte 28      Xmax                Double      1           Little
        // Byte 36      Ymax                Double      1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte X       Points              Point       NumPoints   Little

        // X Y M MultiPoints: Total Length = 34 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 28       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumPoints           Integer     1           Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y*      Mmin                Double      1           Little
        // Byte Y + 8*  Mmax                Double      1           Little
        // Byte Y + 16* Marray              Double      NumPoints   Little

        // X Y Z M MultiPoints: Total Length = 44 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number  Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 18       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumPoints           Integer     1           Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y       Zmin                Double      1           Little
        // Byte Y + 8   Zmax                Double      1           Little
        // Byte Y + 16  Zarray              Double      NumPoints   Little
        // Byte Z*      Mmin                Double      1           Little
        // Byte Z+8*    Mmax                Double      1           Little
        // Byte Z+16*   Marray              Double      NumPoints   Little

        private void FillPoints(string fileName, IProgressHandler progressHandler)
        {
            // Check to ensure the fileName is not null
            if (fileName == null)
            {
                throw new NullReferenceException(DataStrings.ArgumentNull_S.Replace("%S", "fileName"));
            }

            if (File.Exists(fileName) == false)
            {
                throw new FileNotFoundException(DataStrings.FileNotFound_S.Replace("%S", fileName));
            }

            // Get the basic header information.
            ShapefileHeader header = new ShapefileHeader(fileName);
            Extent = header.ToExtent();
            // Check to ensure that the fileName is the correct shape type
            if (header.ShapeType != ShapeType.MultiPoint &&
                 header.ShapeType != ShapeType.MultiPointM &&
                 header.ShapeType != ShapeType.MultiPointZ)
            {
                throw new ArgumentException(DataStrings.FileNotLines_S.Replace("%S", fileName));
            }

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(fileName);

            // This will set up a reader so that we can read values in huge chunks, which is much faster than one value at a time.
            BufferedBinaryReader bbReader = new BufferedBinaryReader(fileName, progressHandler);

            if (bbReader.FileLength == 100)
            {
                // The shapefile is empty so we can simply return here
                bbReader.Close();
                return;
            }

            // Skip the shapefile header by skipping the first 100 bytes in the shapefile
            bbReader.Seek(100, SeekOrigin.Begin);

            int numShapes = shapeHeaders.Count;

            byte[] bigEndians = new byte[numShapes * 8];
            byte[] allBounds = new byte[numShapes * 32];

            ByteBlock allCoords = new ByteBlock(BLOCKSIZE);
            bool isM = (header.ShapeType == ShapeType.MultiPointZ || header.ShapeType == ShapeType.MultiPointM);
            bool isZ = (header.ShapeType == ShapeType.PolyLineZ);
            ByteBlock allZ = null;
            ByteBlock allM = null;
            if (isZ)
            {
                allZ = new ByteBlock(BLOCKSIZE);
            }
            if (isM)
            {
                allM = new ByteBlock(BLOCKSIZE);
            }
            int pointOffset = 0;
            for (int shp = 0; shp < numShapes; shp++)
            {
                // Read from the index file because some deleted records
                // might still exist in the .shp file.
                long offset = (shapeHeaders[shp].ByteOffset);
                bbReader.Seek(offset, SeekOrigin.Begin);

                // time: 200 ms
                ShapeRange shape = new ShapeRange(FeatureType.MultiPoint)
                                   {
                                       RecordNumber = bbReader.ReadInt32(false),
                                       ContentLength = bbReader.ReadInt32(false),
                                       ShapeType = (ShapeType)bbReader.ReadInt32(),
                                       StartIndex = pointOffset
                                   };

                //bbReader.Read(bigEndians, shp * 8, 8);
                if (shape.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }
                bbReader.Read(allBounds, shp * 32, 32);
                shape.NumParts = 1;
                shape.NumPoints = bbReader.ReadInt32();
                allCoords.Read(shape.NumPoints * 16, bbReader);
                pointOffset += shape.NumPoints;

                if (header.ShapeType == ShapeType.MultiPointM)
                {
                    // These are listed as "optional" but there isn't a good indicator of
                    // how to determine if they were added.
                    // To handle the "optional" M values, check the contentLength for the feature.
                    // The content length does not include the 8-byte record header and is listed in 16-bit words.
                    if (shape.ContentLength * 2 > 44 + 4 * shape.NumParts + 16 * shape.NumPoints)
                    {
                        IExtentM mExt = (IExtentM)MyExtent;
                        mExt.MinM = bbReader.ReadDouble();
                        mExt.MaxM = bbReader.ReadDouble();
                        if (allM != null) allM.Read(shape.NumPoints * 8, bbReader);
                    }
                }
                if (header.ShapeType == ShapeType.MultiPointZ)
                {
                    bool hasM = shape.ContentLength * 2 > 60 + 4 * shape.NumParts + 24 * shape.NumPoints;
                    IExtentZ zExt = (IExtentZ)MyExtent;
                    zExt.MinZ = bbReader.ReadDouble();
                    zExt.MaxZ = bbReader.ReadDouble();
                    // For Z shapefiles, the Z part is not optional.
                    if (allZ != null) allZ.Read(shape.NumPoints * 8, bbReader);

                    // These are listed as "optional" but there isn't a good indicator of
                    // how to determine if they were added.
                    // To handle the "optional" M values, check the contentLength for the feature.
                    // The content length does not include the 8-byte record header and is listed in 16-bit words.
                    if (hasM)
                    {
                        IExtentM mExt = (IExtentM)MyExtent;
                        mExt.MinM = bbReader.ReadDouble();
                        mExt.MaxM = bbReader.ReadDouble();
                        if (allM != null) allM.Read(shape.NumPoints * 8, bbReader);
                    }
                }
                // Now that we have read all the values, create the geometries from the points and parts arrays.
                ShapeIndices.Add(shape);
            }
            double[] vert = allCoords.ToDoubleArray();
            Vertex = vert;
            if (isM) M = allM.ToDoubleArray();
            if (isZ) Z = allZ.ToDoubleArray();
            Array.Reverse(bigEndians);
            List<ShapeRange> shapes = ShapeIndices;
            double[] bounds = new double[numShapes * 4];
            Buffer.BlockCopy(allBounds, 0, bounds, 0, allBounds.Length);
            for (int shp = 0; shp < numShapes; shp++)
            {
                ShapeRange shape = shapes[shp];
                shape.Extent = new Extent(bounds, shp * 4);
                int endIndex = shape.NumPoints + shape.StartIndex;
                int startIndex = shape.StartIndex;
                int count = endIndex - startIndex;
                PartRange partR = new PartRange(vert, shape.StartIndex, 0, FeatureType.MultiPoint) { NumVertices = count };
                shape.Parts.Add(partR);
            }

            bbReader.Dispose();
        }

        /// <summary>
        /// Saves the file to a new location
        /// </summary>
        /// <param name="fileName">The fileName to save</param>
        /// <param name="overwrite">Boolean that specifies whether or not to overwrite the existing file</param>
        public override void SaveAs(string fileName, bool overwrite)
        {
            EnsureValidFileToSave(fileName, overwrite);
            Filename = fileName;

            // Set ShapeType before setting extent.
            if (CoordinateType == CoordinateType.Regular)
            {
                Header.ShapeType = ShapeType.MultiPoint;
            }
            if (CoordinateType == CoordinateType.M)
            {
                Header.ShapeType = ShapeType.MultiPointM;
            }
            if (CoordinateType == CoordinateType.Z)
            {
                Header.ShapeType = ShapeType.MultiPointZ;
            }
            HeaderSaveAs(fileName);

            if (IndexMode)
            {
                SaveAsIndexed(fileName);
                return;
            }

            var bbWriter = new BufferedBinaryWriter(fileName);
            var indexWriter = new BufferedBinaryWriter(Header.ShxFilename);
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words
            int contentLength = 0;
            ProgressMeter = new ProgressMeter(ProgressHandler, "Saving (Not Indexed)...", Features.Count);
            foreach (IFeature f in Features)
            {
                offset += contentLength; // adding the previous content length from each loop calculates the word offset
                List<Coordinate> points = new List<Coordinate>();
                contentLength = 20;
                for (int iPart = 0; iPart < f.NumGeometries; iPart++)
                {
                    IList<Coordinate> coords = f.BasicGeometry.GetBasicGeometryN(iPart).Coordinates;
                    foreach (Coordinate coord in coords)
                    {
                        points.Add(coord);
                    }
                }

                if (Header.ShapeType == ShapeType.MultiPoint)
                {
                    contentLength += points.Count * 8;
                }
                if (Header.ShapeType == ShapeType.MultiPointM)
                {
                    contentLength += 8; // mmin, mmax
                    contentLength += points.Count * 12;
                }
                if (Header.ShapeType == ShapeType.MultiPointZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += points.Count * 16;
                }

                //                                           Index File
                //                                       ---------------------------------------------------------
                //                                         Position     Value   Type        Number      Byte Order
                //                                        ---------------------------------------------------------
                indexWriter.Write(offset, false);        // Byte 0     Offset   Integer     1           Big
                indexWriter.Write(contentLength, false); // Byte 4     Length   Integer     1           Big

                //                                           X Y Poly Lines
                //                                        ---------------------------------------------------------
                //                                         Position     Value   Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                bbWriter.Write(fid + 1, false);             // Byte 0     Record  Integer     1           Big
                bbWriter.Write(contentLength, false);     // Byte 4     Length  Integer     1           Big
                bbWriter.Write((int)Header.ShapeType);    // Byte 8     Shape   Integer     1           Little
                if (Header.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }
                bbWriter.Write(f.Envelope.Minimum.X);      // Byte 12   Xmin    Double      1           Little
                bbWriter.Write(f.Envelope.Minimum.Y);      // Byte 20   Ymin    Double      1           Little
                bbWriter.Write(f.Envelope.Maximum.X);      // Byte 28   Xmax    Double      1           Little
                bbWriter.Write(f.Envelope.Maximum.Y);      // Byte 36   Ymax    Double      1           Little

                bbWriter.Write(points.Count);              // Byte 44   #Points Integer     1           Little
                // Byte X    Points   Point    #Points       Little

                foreach (Coordinate coord in points)
                {
                    bbWriter.Write(coord.X);
                    bbWriter.Write(coord.Y);
                }

                if (Header.ShapeType == ShapeType.MultiPointZ)
                {
                    bbWriter.Write(f.Envelope.Minimum.Z);
                    bbWriter.Write(f.Envelope.Maximum.Z);
                    foreach (Coordinate coord in points)
                    {
                        bbWriter.Write(coord.Z);
                    }
                }
                if (Header.ShapeType == ShapeType.MultiPointM || Header.ShapeType == ShapeType.MultiPointZ)
                {
                    if (f.Envelope == null)
                    {
                        bbWriter.Write(0.0);
                        bbWriter.Write(0.0);
                    }
                    else
                    {
                        bbWriter.Write(f.Envelope.Minimum.M);
                        bbWriter.Write(f.Envelope.Maximum.M);
                    }
                    foreach (Coordinate coord in points)
                    {
                        bbWriter.Write(coord.M);
                    }
                }

                ProgressMeter.CurrentValue = fid;
                fid++;
                offset += 4;
            }
            ProgressMeter.Reset();
            bbWriter.Close();
            indexWriter.Close();

            offset += contentLength;
            WriteFileLength(Filename, offset);
            UpdateAttributes();
            SaveProjection();
        }
        
        private void SaveAsIndexed(string fileName)
        {
            var shpStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None, 10000000);
            var shxStream = new FileStream(Header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 10000000);
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words
            int contentLength = 0;

            foreach (ShapeRange shape in ShapeIndices)
            {
                offset += contentLength; // adding the previous content length from each loop calculates the word offset

                contentLength = 20;

                if (Header.ShapeType == ShapeType.MultiPoint)
                {
                    contentLength += shape.NumPoints * 8;
                }
                if (Header.ShapeType == ShapeType.MultiPointM)
                {
                    contentLength += 8; // mmin, mmax
                    contentLength += shape.NumPoints * 12;
                }
                if (Header.ShapeType == ShapeType.MultiPointZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += shape.NumPoints * 16;
                }

                //                                         Index File
                //                                         ---------------------------------------------------------
                //                                        Position     Value   Type     Number  Byte Order
                //                                        ---------------------------------------------------------
                shxStream.WriteBe(offset);                 // Byte 0  Offset   Integer    1     Big
                shxStream.WriteBe(contentLength);          // Byte 4  Length   Integer    1     Big

                //                                         X Y Poly Lines
                //                                         ---------------------------------------------------------
                //                                          Position  Value    Type      Number  Byte Order
                //                                        ---------------------------------------------------------
                shpStream.WriteBe(fid + 1);                // Byte 0  Record   Integer     1     Big
                shpStream.WriteBe(contentLength);          // Byte 4  Length   Integer     1     Big
                shpStream.WriteLe((int)Header.ShapeType);  // Byte 8  Type 3   Integer     1     Little
                if (Header.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }
                shpStream.WriteLe(shape.Extent.MinX);     // Byte 12  Xmin     Double      1     Little
                shpStream.WriteLe(shape.Extent.MinY);     // Byte 20  Ymin     Double      1     Little
                shpStream.WriteLe(shape.Extent.MaxX);     // Byte 28  Xmax     Double      1     Little
                shpStream.WriteLe(shape.Extent.MaxY);     // Byte 36  Ymax     Double      1     Little
                shpStream.WriteLe(shape.NumPoints);       // Byte 44  #Points  Integer     1     Little

                int start = shape.StartIndex;
                int count = shape.NumPoints;
                shpStream.WriteLe(Vertex, start * 2, count * 2);

                if (Header.ShapeType == ShapeType.MultiPointZ)
                {
                    double[] shapeZ = new double[count];
                    Array.Copy(Z, start, shapeZ, 0, count);
                    shpStream.WriteLe(shapeZ.Min());
                    shpStream.WriteLe(shapeZ.Max());
                    shpStream.WriteLe(Z, start, count);
                }
                if (Header.ShapeType == ShapeType.MultiPointM || Header.ShapeType == ShapeType.MultiPointZ)
                {
                    if (M != null && M.Length >= start + count)
                    {
                        double[] shapeM = new double[count];
                        Array.Copy(M, start, shapeM, 0, count);
                        shpStream.WriteLe(shapeM.Min());
                        shpStream.WriteLe(shapeM.Max());
                        shpStream.WriteLe(M, start, count);
                    }
                }

                fid++;
                offset += 4;
            }
            
            shpStream.Close();
            shxStream.Close();

            offset += contentLength;
            WriteFileLength(Filename, offset);
            UpdateAttributes();
            SaveProjection();
        }
    }
}