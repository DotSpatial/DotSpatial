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
    /// A shapefile class that handles the special case where the vectors are lines
    /// </summary>
    public class LineShapefile : Shapefile
    {
        private const int BLOCKSIZE = 16000000;

        /// <summary>
        /// Creates a new instance of a LineShapefile for in-ram handling only.
        /// </summary>
        public LineShapefile()
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader { FileLength = 100, ShapeType = ShapeType.PolyLine };
            FeatureType = FeatureType.Line;
        }

        /// <summary>
        /// Creates a new instance of a LineShapefile that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public LineShapefile(string fileName)
            : this()
        {
            Open(fileName, null);
        }

        /// <summary>
        /// Opens a shapefile
        /// </summary>
        /// <param name="fileName">The string fileName of the line shapefile to load</param>
        /// <param name="progressHandler">Any valid implementation of the DotSpatial.Data.IProgressHandler</param>
        public void Open(string fileName, IProgressHandler progressHandler)
        {
            if (!File.Exists(fileName))
            {
                Attributes = new AttributeTable();
                Header = new ShapefileHeader { FileLength = 100, ShapeType = ShapeType.PolyLine };
                FeatureType = FeatureType.Line;
                return;
            }

            Filename = fileName;
            FeatureType = FeatureType.Line;
            Header = new ShapefileHeader(fileName);
            CoordinateType = CoordinateType.Regular;
            IndexMode = true;
            if (Header.ShapeType == ShapeType.PolyLineM)
            {
                CoordinateType = CoordinateType.M;
            }
            if (Header.ShapeType == ShapeType.PolyLineZ)
            {
                CoordinateType = CoordinateType.Z;
            }
            MyExtent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(fileName);
            FillLines(fileName, progressHandler);
            ReadProjection();
        }

        // X Y Poly Lines
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 3        Integer     1           Little
        // Byte 12      Xmin                Double      1           Little
        // Byte 20      Ymin                Double      1           Little
        // Byte 28      Xmax                Double      1           Little
        // Byte 36      Ymax                Double      1           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little

        // X Y M Poly Lines
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 23       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y*      Mmin                Double      1           Little
        // Byte Y + 8*  Mmax                Double      1           Little
        // Byte Y + 16* Marray              Double      NumPoints   Little

        // X Y Z M Poly Lines
        // ---------------------------------------------------------
        // Position     Value               Type        Number  Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 13       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y       Zmin                Double      1           Little
        // Byte Y + 8   Zmax                Double      1           Little
        // Byte Y + 16  Zarray              Double      NumPoints   Little
        // Byte Z*      Mmin                Double      1           Little
        // Byte Z+8*    Mmax                Double      1           Little
        // Byte Z+16*   Marray              Double      NumPoints   Little

        /// <summary>
        /// Gets the specified feature by constructing it from the vertices, rather
        /// than requiring that all the features be created. (which takes up a lot of memory).
        /// </summary>
        /// <param name="index">The integer index</param>
        public override IFeature GetFeature(int index)
        {
            IFeature f = GetLine(index);
            if (f != null)
            {
                f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
            }
            return f;
        }

        private void FillLines(string fileName, IProgressHandler progressHandler)
        {
            // Check to ensure the fileName is not null
            if (fileName == null)
            {
                throw new NullReferenceException(DataStrings.ArgumentNull_S.Replace("%S", fileName));
            }

            if (File.Exists(fileName) == false)
            {
                throw new FileNotFoundException(DataStrings.FileNotFound_S.Replace("%S", fileName));
            }

            // Get the basic header information.
            ShapefileHeader header = new ShapefileHeader(fileName);
            Extent = new Extent(new[] { header.Xmin, header.Ymin, header.Xmax, header.Ymax });
            // Check to ensure that the fileName is the correct shape type
            if (header.ShapeType != ShapeType.PolyLine &&
                 header.ShapeType != ShapeType.PolyLineM &&
                 header.ShapeType != ShapeType.PolyLineZ)
            {
                throw new ArgumentException(DataStrings.FileNotLines_S.Replace("%S", fileName));
            }

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(fileName);

            // This will set up a reader so that we can read values in huge chunks, which is much faster
            // than one value at a time.
            BufferedBinaryReader bbReader = new BufferedBinaryReader(fileName, progressHandler);

            if (bbReader.FileLength == 100)
            {
                // We have reached the end of the file so we can close the file
                bbReader.Close();
                return;
            }

            // Skip the shapefile header by skipping the first 100 bytes in the shapefile
            bbReader.Seek(100, SeekOrigin.Begin);

            int numShapes = shapeHeaders.Count;

            int[] partOffsets = new int[numShapes];
            byte[] allBounds = new byte[numShapes * 32];
            // probably all will be in one block, but use a byteBlock just in case.
            ByteBlock allParts = new ByteBlock(BLOCKSIZE);
            ByteBlock allCoords = new ByteBlock(BLOCKSIZE);
            bool isM = (header.ShapeType == ShapeType.PolyLineM || header.ShapeType == ShapeType.PolyLineZ);
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
                ShapeRange shape = new ShapeRange(FeatureType.Line)
                                   {
                                       RecordNumber = bbReader.ReadInt32(false),
                                       ContentLength = bbReader.ReadInt32(false),
                                       ShapeType = (ShapeType)bbReader.ReadInt32(),
                                       StartIndex = pointOffset
                                   };

                if (shape.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }
                bbReader.Read(allBounds, shp * 32, 32);
                shape.NumParts = bbReader.ReadInt32();   // Byte 44      NumParts    Integer     1      Little
                shape.NumPoints = bbReader.ReadInt32();  // Byte 48      NumPoints   Integer     1      Little

                partOffsets[shp] = allParts.IntOffset();
                allParts.Read(shape.NumParts * 4, bbReader);

                allCoords.Read(shape.NumPoints * 16, bbReader);

                pointOffset += shape.NumPoints;

                if (header.ShapeType == ShapeType.PolyLineM)
                {
                    // These are listed as "optional" but there isn't a good indicator of how to
                    // determine if they were added.
                    // To handle the "optional" M values, check the contentLength for the feature.
                    // The content length does not include the 8-byte record header and is listed in 16-bit words.
                    if (shape.ContentLength * 2 > 44 + 4 * shape.NumParts + 16 * shape.NumPoints)
                    {
                        //mMin = bbReader.ReadDouble();
                        //mMax = bbReader.ReadDouble();
                        bbReader.Seek(16, SeekOrigin.Current);
                        if (allM != null) allM.Read(shape.NumPoints * 8, bbReader);
                    }
                }
                if (header.ShapeType == ShapeType.PolyLineZ)
                {
                    bool hasM = shape.ContentLength * 2 > 60 + 4 * shape.NumParts + 24 * shape.NumPoints;
                    IExtentZ zExt = (IExtentZ)shape.Extent;
                    zExt.MinZ = bbReader.ReadDouble();
                    zExt.MaxZ = bbReader.ReadDouble();
                    if (allZ != null) allZ.Read(shape.NumPoints * 8, bbReader);

                    // These are listed as "optional" but there isn't a good indicator of how to
                    // determine if they were added.
                    // To handle the "optional" M values, check the contentLength for the feature.
                    // The content length does not include the 8-byte record header and is listed in 16-bit words.
                    IExtentM mExt = (IExtentM)shape.Extent;
                    if (hasM)
                    {
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
            List<ShapeRange> shapes = ShapeIndices;
            double[] bounds = new double[numShapes * 4];
            Buffer.BlockCopy(allBounds, 0, bounds, 0, allBounds.Length);
            int[] parts = allParts.ToIntArray();
            for (int shp = 0; shp < numShapes; shp++)
            {
                ShapeRange shape = shapes[shp];
                shape.Extent = new Extent(bounds, shp * 4);
                for (int part = 0; part < shape.NumParts; part++)
                {
                    int offset = partOffsets[shp];
                    int endIndex = shape.NumPoints + shape.StartIndex;
                    int startIndex = parts[offset + part] + shape.StartIndex;
                    if (part < shape.NumParts - 1)
                    {
                        int prt = parts[offset + part + 1];
                        endIndex = prt + shape.StartIndex;
                    }
                    int count = endIndex - startIndex;
                    PartRange partR = new PartRange(vert, shape.StartIndex, parts[offset + part], FeatureType.Line) { NumVertices = count };
                    shape.Parts.Add(partR);
                }
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
            if (IndexMode)
            {
                SaveAsIndexed(fileName, overwrite);
                return;
            }
            Filename = fileName;
            string dir = Path.GetDirectoryName(fileName);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(fileName))
            {
                if (fileName != Filename && overwrite == false) throw new IOException("File exists.");
                File.Delete(fileName);
                string shx = Path.ChangeExtension(fileName, ".shx");
                if (File.Exists(shx)) File.Delete(shx);
            }
            InvalidateEnvelope();

            if (CoordinateType == CoordinateType.Regular)
            {
                Header.ShapeType = ShapeType.PolyLine;
            }
            if (CoordinateType == CoordinateType.M)
            {
                Header.ShapeType = ShapeType.PolyLineM;
            }
            if (CoordinateType == CoordinateType.Z)
            {
                Header.ShapeType = ShapeType.PolyLineZ;
            }

            // Set ShapeType before setting header.
            //Changed by Jiri Kadlec from SetExtent(MyExtent) to SetExtent(Extent)
            Header.SetExtent(Extent); //Header.SetExtent(MyExtent);
            Header.ShxLength = Features.Count * 4 + 50;
            Header.SaveAs(fileName);

            BufferedBinaryWriter bbWriter = new BufferedBinaryWriter(fileName);
            BufferedBinaryWriter indexWriter = new BufferedBinaryWriter(Header.ShxFilename);
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words
            int contentLength = 0;

            foreach (IFeature f in Features)
            {
                List<int> parts = new List<int>();
                offset += contentLength; // adding the previous content length from each loop calculates the word offset
                List<Coordinate> points = new List<Coordinate>();
                contentLength = 22;
                for (int iPart = 0; iPart < f.NumGeometries; iPart++)
                {
                    parts.Add(points.Count);
                    IBasicLineString bl = f.GetBasicGeometryN(iPart) as IBasicLineString;
                    if (bl == null) continue;
                    foreach (Coordinate coord in bl.Coordinates)
                    {
                        points.Add(coord);
                    }
                }
                contentLength += 2 * parts.Count;
                if (Header.ShapeType == ShapeType.PolyLine)
                {
                    contentLength += points.Count * 8; // x, y
                }
                if (Header.ShapeType == ShapeType.PolyLineM)
                {
                    contentLength += 8; // mmin mmax
                    contentLength += points.Count * 12; // x, y, m
                }
                if (Header.ShapeType == ShapeType.PolyLineZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += points.Count * 16; // x, y, z, m
                }

                //                                        Index File
                //                                        ---------------------------------------------------------
                //                                        Position     Value          Type       Number  Byte Order
                //                                        ---------------------------------------------------------
                indexWriter.Write(offset, false);        // Byte 0     Offset         Integer     1      Big
                indexWriter.Write(contentLength, false); // Byte 4     Content Length Integer     1      Big

                //                                         X Y Poly Lines
                //                                         ---------------------------------------------------------
                //                                        Position     Value         Type        Number      Byte Order
                //                                              -------------------------------------------------------
                bbWriter.Write(fid + 1, false);             // Byte 0   Record Number  Integer     1           Big
                bbWriter.Write(contentLength, false);     // Byte 4   Content Length Integer     1           Big
                bbWriter.Write((int)Header.ShapeType);    // Byte 8   Shape Type 3   Integer     1           Little
                if (Header.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }
                IEnvelope env = f.Envelope ?? new Envelope();
                bbWriter.Write(env.Minimum.X);            // Byte 12   Xmin          Double      1           Little
                bbWriter.Write(env.Minimum.Y);            // Byte 20   Ymin          Double      1           Little
                bbWriter.Write(env.Maximum.X);            // Byte 28   Xmax          Double      1           Little
                bbWriter.Write(env.Maximum.Y);            // Byte 36   Ymax          Double      1           Little
                bbWriter.Write(parts.Count);              // Byte 44   NumParts      Integer     1           Little
                bbWriter.Write(points.Count);             // Byte 48   NumPoints     Integer     1           Little
                // Byte 52   Parts         Integer     NumParts    Little
                foreach (int iPart in parts)
                {
                    bbWriter.Write(iPart);
                }

                double[] xyVals = new double[points.Count * 2];
                for (int ipoint = 0; ipoint < points.Count; ipoint++)
                {
                    double[] c = points[ipoint].ToArray();
                    xyVals[ipoint * 2] = c[0];
                    xyVals[ipoint * 2 + 1] = c[1];
                }
                bbWriter.Write(xyVals);
                if (Header.ShapeType == ShapeType.PolyLineZ)
                {
                    if (f.Envelope != null)
                    {
                        bbWriter.Write(f.Envelope.Minimum.Z);
                        bbWriter.Write(f.Envelope.Maximum.Z);
                    }
                    double[] zVals = new double[points.Count];
                    for (int ipoint = 0; ipoint < points.Count; ipoint++)
                    {
                        zVals[ipoint] = points[ipoint].Z;
                    }
                    bbWriter.Write(zVals);
                }

                if (Header.ShapeType == ShapeType.PolyLineM || Header.ShapeType == ShapeType.PolyLineZ)
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

                    double[] mVals = new double[points.Count];
                    for (int ipoint = 0; ipoint < points.Count; ipoint++)
                    {
                        mVals[ipoint] = points[ipoint].M;
                    }
                    bbWriter.Write(mVals);
                }

                fid++;
                offset += 4; // header bytes
            }

            bbWriter.Close();
            indexWriter.Close();

            offset += contentLength;
            //offset += 4;
            WriteFileLength(Filename, offset);
            WriteFileLength(Header.ShxFilename, 50 + fid * 4);
            UpdateAttributes();
            SaveProjection();
        }

        /// <summary>
        /// Saves the file to a new location
        /// </summary>
        /// <param name="fileName">The fileName to save</param>
        /// <param name="overwrite">Boolean that specifies whether or not to overwrite the existing file</param>
        private void SaveAsIndexed(string fileName, bool overwrite)
        {
            Filename = fileName;
            string dir = Path.GetDirectoryName(fileName);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(fileName))
            {
                if (fileName != Filename && overwrite == false) throw new IOException("File exists.");
                File.Delete(fileName);

                string shx = Path.ChangeExtension(fileName, ".shx");
                if (File.Exists(shx)) File.Delete(shx);
            }

            // comment out by keen edge as per this discussion
            // http://dotspatial.codeplex.com/Thread/View.aspx?ThreadId=234754
            // InvalidateEnvelope();

            if (CoordinateType == CoordinateType.Regular)
            {
                Header.ShapeType = ShapeType.PolyLine;
            }
            if (CoordinateType == CoordinateType.M)
            {
                Header.ShapeType = ShapeType.PolyLineM;
            }
            if (CoordinateType == CoordinateType.Z)
            {
                Header.ShapeType = ShapeType.PolyLineZ;
            }
            // Set Header.ShapeType before calling SetExtent.
            Header.SetExtent(MyExtent);

            Header.ShxLength = ShapeIndices.Count * 4 + 50;
            Header.SaveAs(fileName);

            FileStream shpStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None, 10000000);
            FileStream shxStream = new FileStream(Header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 10000000);
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words
            int contentLength = 0;

            foreach (ShapeRange shape in ShapeIndices)
            {
                offset += contentLength; // adding the previous content length from each loop calculates the word offset

                contentLength = 22;
                contentLength += 2 * shape.Parts.Count;
                if (Header.ShapeType == ShapeType.PolyLine)
                {
                    contentLength += shape.NumPoints * 8; // x, y
                }
                if (Header.ShapeType == ShapeType.PolyLineM)
                {
                    contentLength += 8; // mmin mmax
                    contentLength += shape.NumPoints * 12; // x, y, m
                }
                if (Header.ShapeType == ShapeType.PolyLineZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += shape.NumPoints * 16; // x, y, z, m
                }

                //                                              Index File
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                shxStream.WriteBe(offset);                     // Byte 0     Offset             Integer     1           Big
                shxStream.WriteBe(contentLength);              // Byte 4     Content Length     Integer     1           Big

                //                                              X Y Poly Lines
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                shpStream.WriteBe(fid + 1);                     // Byte 0       Record Number       Integer     1           Big
                shpStream.WriteBe(contentLength);               // Byte 4       Content Length      Integer     1           Big
                shpStream.WriteLe((int)Header.ShapeType);       // Byte 8       Shape Type 3        Integer     1           Little
                if (Header.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }
                shpStream.WriteLe(shape.Extent.MinX);             // Byte 12      Xmin                Double      1           Little
                shpStream.WriteLe(shape.Extent.MinY);             // Byte 20      Ymin                Double      1           Little
                shpStream.WriteLe(shape.Extent.MaxX);             // Byte 28      Xmax                Double      1           Little
                shpStream.WriteLe(shape.Extent.MaxY);             // Byte 36      Ymax                Double      1           Little
                shpStream.WriteLe(shape.NumParts);                 // Byte 44      NumParts            Integer     1           Little
                shpStream.WriteLe(shape.NumPoints);                // Byte 48      NumPoints           Integer     1           Little
                // Byte 52      Parts               Integer     NumParts    Little
                foreach (PartRange part in shape.Parts)
                {
                    shpStream.WriteLe(part.PartOffset);
                }
                int start = shape.StartIndex;
                int count = shape.NumPoints;
                shpStream.WriteLe(Vertex, start * 2, count * 2);            // Byte X       Points              Point       NumPoints   Little

                if (Header.ShapeType == ShapeType.PolyLineZ)
                {
                    double[] shapeZ = new double[count];
                    Array.Copy(Z, start, shapeZ, 0, count);
                    shpStream.WriteLe(shapeZ.Min());
                    shpStream.WriteLe(shapeZ.Max());
                    shpStream.WriteLe(Z, start, count);
                }

                if (Header.ShapeType == ShapeType.PolyLineM || Header.ShapeType == ShapeType.PolyLineZ)
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
                offset += 4; // header bytes
            }

            shpStream.Flush();
            shxStream.Flush();
            shpStream.Close();
            shxStream.Close();

            offset += contentLength;
            //offset += 4;
            WriteFileLength(Filename, offset);
            WriteFileLength(Header.ShxFilename, 50 + fid * 4);
            UpdateAttributes();
            SaveProjection();
        }
    }
}