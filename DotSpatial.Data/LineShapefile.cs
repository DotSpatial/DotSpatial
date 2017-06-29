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
        /// <summary>
        /// Creates a new instance of a LineShapefile for in-ram handling only.
        /// </summary>
        public LineShapefile()
            : base(FeatureType.Line)
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader { FileLength = 100, ShapeType = ShapeType.PolyLine };
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
            if (!File.Exists(fileName)) return;

            Filename = fileName;
            IndexMode = true;
            Header = new ShapefileHeader(fileName);

            switch (Header.ShapeType)
            {
                case ShapeType.PolyLineM:
                    CoordinateType = CoordinateType.M;
                    break;
                case ShapeType.PolyLineZ:
                    CoordinateType = CoordinateType.Z;
                    break;
                default:
                    CoordinateType = CoordinateType.Regular;
                    break;
            }

            Extent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(fileName);

            FillLines(fileName, progressHandler, this, FeatureType.Line);
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
            IFeature f;
            if (!IndexMode)
            {
                f = Features[index];
            }
            else
            {
                f = GetLine(index);
                if (f != null)
                {
                    f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
                }
            }
            return f;
        }


        internal static void FillLines(string fileName, IProgressHandler progressHandler, Shapefile shapefile,
            FeatureType featureType)
        {
            // Check to ensure the fileName is not null
            if (fileName == null)
            {
                throw new NullReferenceException(DataStrings.ArgumentNull_S.Replace("%S", "fileName"));
            }
            if (shapefile == null) throw new ArgumentNullException("shapefile");

            if (File.Exists(fileName) == false)
            {
                throw new FileNotFoundException(DataStrings.FileNotFound_S.Replace("%S", fileName));
            }

            if (featureType != FeatureType.Line && featureType != FeatureType.Polygon)
            {
                throw new NotSupportedException();
            }

            var header = shapefile.Header;
            // Check to ensure that the fileName is the correct shape type
            switch (featureType)
            {
                case FeatureType.Line:
                    if (header.ShapeType != ShapeType.PolyLine &&
                        header.ShapeType != ShapeType.PolyLineM &&
                        header.ShapeType != ShapeType.PolyLineZ)
                    {
                        throw new ArgumentException(DataStrings.FileNotLines_S.Replace("%S", fileName));
                    }
                    break;
                case FeatureType.Polygon:
                    if (header.ShapeType != ShapeType.Polygon &&
                        header.ShapeType != ShapeType.PolygonM &&
                        header.ShapeType != ShapeType.PolygonZ)
                    {
                        throw new ArgumentException(DataStrings.FileNotLines_S.Replace("%S", fileName));
                    }
                    break;
            }

            if (new FileInfo(fileName).Length == 100)
            {
                // the file is empty so we are done reading
                return;
            }

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            var shapeHeaders = shapefile.ReadIndexFile(fileName);
            int numShapes = shapeHeaders.Count;

            bool isM = false, isZ = false;
            switch (header.ShapeType)
            {
                case ShapeType.PolyLineM:
                case ShapeType.PolygonM:
                    isM = true;
                    break;
                case ShapeType.PolyLineZ:
                case ShapeType.PolygonZ:
                    isZ = true;
                    isM = true;
                    break;
            }

            int totalPointsCount = 0;
            int totalPartsCount = 0;
            var shapeIndices = new List<ShapeRange>(numShapes);

            var progressMeter = new ProgressMeter(progressHandler, "Reading from " + Path.GetFileName(fileName))
            {
                StepPercent = 5
            };
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 65536))
            {
                var boundsBytes = new byte[4 * 8];
                var bounds = new double[4];
                for (int shp = 0; shp < numShapes; shp++)
                {
                    progressMeter.CurrentPercent = (int)(shp * 50.0 / numShapes);

                    // Read from the index file because some deleted records
                    // might still exist in the .shp file.
                    long offset = (shapeHeaders[shp].ByteOffset);
                    reader.Seek(offset, SeekOrigin.Begin);

                    var shape = new ShapeRange(featureType, shapefile.CoordinateType)
                    {
                        RecordNumber = reader.ReadInt32(Endian.BigEndian),
                        ContentLength = reader.ReadInt32(Endian.BigEndian),
                        ShapeType = (ShapeType)reader.ReadInt32(),
                        StartIndex = totalPointsCount
                    };
                    Debug.Assert(shape.RecordNumber == shp + 1);

                    if (shape.ShapeType != ShapeType.NullShape)
                    {
                        // Bounds
                        reader.Read(boundsBytes, 0, boundsBytes.Length);
                        Buffer.BlockCopy(boundsBytes, 0, bounds, 0, boundsBytes.Length);
                        shape.Extent.MinX = bounds[0];
                        shape.Extent.MinY = bounds[1];
                        shape.Extent.MaxX = bounds[2];
                        shape.Extent.MaxY = bounds[3];

                        // Num Parts
                        shape.NumParts = reader.ReadInt32();
                        totalPartsCount += shape.NumParts;

                        // Num Points
                        shape.NumPoints = reader.ReadInt32();
                        totalPointsCount += shape.NumPoints;
                    }

                    shapeIndices.Add(shape);
                }

                var vert = new double[totalPointsCount * 2];
                var vertInd = 0;

                var parts = new int[totalPartsCount];
                var partsInd = 0;

                double[] mArray = null, zArray = null;
                if (isM)
                {
                    mArray = new double[totalPointsCount];
                }
                int mArrayInd = 0;
                if (isZ)
                {
                    zArray = new double[totalPointsCount];
                }
                int zArrayInd = 0;

                int partsOffset = 0;
                for (int shp = 0; shp < numShapes; shp++)
                {
                    progressMeter.CurrentPercent = (int)(50 + shp * 50.0 / numShapes);

                    var shape = shapeIndices[shp];
                    if (shape.ShapeType == ShapeType.NullShape) continue;
                    reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);
                    reader.Seek(3 * 4 + 32 + 2 * 4, SeekOrigin.Current); // Skip first bytes

                    // Read parts
                    var partsBytes = reader.ReadBytes(4 * shape.NumParts);
                    Buffer.BlockCopy(partsBytes, 0, parts, partsInd, partsBytes.Length);
                    partsInd += 4 * shape.NumParts;

                    // Read points
                    var pointsBytes = reader.ReadBytes(8 * 2 * shape.NumPoints);
                    Buffer.BlockCopy(pointsBytes, 0, vert, vertInd, pointsBytes.Length);
                    vertInd += 8 * 2 * shape.NumPoints;

                    // Fill parts
                    shape.Parts.Capacity = shape.NumParts;
                    for (int part = 0; part < shape.NumParts; part++)
                    {
                        int endIndex = shape.NumPoints + shape.StartIndex;
                        int startIndex = parts[partsOffset + part] + shape.StartIndex;
                        if (part < shape.NumParts - 1)
                        {
                            endIndex = parts[partsOffset + part + 1] + shape.StartIndex;
                        }
                        int count = endIndex - startIndex;
                        var partR = new PartRange(vert, shape.StartIndex, parts[partsOffset + part], featureType)
                        {
                            NumVertices = count
                        };
                        shape.Parts.Add(partR);
                    }
                    partsOffset += shape.NumParts;

                    // Fill M and Z arrays
                    switch (header.ShapeType)
                    {
                        case ShapeType.PolyLineM:
                        case ShapeType.PolygonM:
                            if (shape.ContentLength * 2 > 44 + 4 * shape.NumParts + 16 * shape.NumPoints)
                            {
                                var mExt = (IExtentM)shape.Extent;
                                mExt.MinM = reader.ReadDouble();
                                mExt.MaxM = reader.ReadDouble();

                                var mBytes = reader.ReadBytes(8 * shape.NumPoints);
                                Buffer.BlockCopy(mBytes, 0, mArray, mArrayInd, mBytes.Length);
                                mArrayInd += 8 * shape.NumPoints;
                            }
                            break;
                        case ShapeType.PolyLineZ:
                        case ShapeType.PolygonZ:
                            var zExt = (IExtentZ)shape.Extent;
                            zExt.MinZ = reader.ReadDouble();
                            zExt.MaxZ = reader.ReadDouble();

                            var zBytes = reader.ReadBytes(8 * shape.NumPoints);
                            Buffer.BlockCopy(zBytes, 0, zArray, zArrayInd, zBytes.Length);
                            zArrayInd += 8 * shape.NumPoints;


                            // These are listed as "optional" but there isn't a good indicator of how to
                            // determine if they were added.
                            // To handle the "optional" M values, check the contentLength for the feature.
                            // The content length does not include the 8-byte record header and is listed in 16-bit words.
                            if (shape.ContentLength * 2 > 60 + 4 * shape.NumParts + 24 * shape.NumPoints)
                            {
                                goto case ShapeType.PolyLineM;
                            }

                            break;
                    }
                }

                if (isM) shapefile.M = mArray;
                if (isZ) shapefile.Z = zArray;
                shapefile.ShapeIndices = shapeIndices;
                shapefile.Vertex = vert;
            }

            progressMeter.Reset();
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

            // Set ShapeType before setting header.
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
                    points.AddRange(bl.Coordinates);
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

        private void SaveAsIndexed(string fileName)
        {
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

                switch (shape.ShapeType)
                {
                    case ShapeType.PolyLine:
                        contentLength += shape.NumPoints * 8; // x, y
                        break;
                    case ShapeType.PolyLineM:
                        contentLength += 8; // mmin mmax
                        contentLength += shape.NumPoints * 12; // x, y, m
                        break;
                    case ShapeType.PolyLineZ:
                        contentLength += 16; // mmin, mmax, zmin, zmax
                        contentLength += shape.NumPoints * 16; // x, y, z, m
                        break;
                    case ShapeType.NullShape:
                        contentLength = 2;
                        break;
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
                shpStream.WriteLe((int)shape.ShapeType);        // Byte 8       Shape Type 3        Integer     1           Little
                if (shape.ShapeType == ShapeType.NullShape)
                {
                    goto fin;
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

            fin:

                fid++;
                offset += 4; // header bytes
            }

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