// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
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
using GeoAPI.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where each features has multiple points
    /// </summary>
    public class MultiPointShapefile : Shapefile
    {
        #region Constant Fields

        private const int BLOCKSIZE = 16000000;

        #endregion

        #region Constructors

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

        #endregion

        #region Methods

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
        // Byte 44      NumPoints           Integer     1           Little
        // Byte 48      Points              Point       NumPoints   Little

        // X Y M MultiPoints: Total Length = 34 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 28       Integer     1           Little
        // Byte 12      Box (Xmin - Ymax)   Double      4           Little
        // Byte 44      NumPoints           Integer     1           Little
        // Byte 48      Points              Point       NumPoints   Little
        // Byte X*      Mmin                Double      1           Little
        // Byte X+8*    Mmax                Double      1           Little
        // Byte X+16*   Marray              Double      NumPoints   Little
        // X = 48 + (16 * NumPoints)
        // * = optional

        // X Y Z M MultiPoints: Total Length = 44 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number  Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 18       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumPoints           Integer     1           Little
        // Byte 48      Points              Point       NumPoints   Little
        // Byte X       Zmin                Double      1           Little
        // Byte X+8     Zmax                Double      1           Little
        // Byte X+16    Zarray              Double      NumPoints   Little
        // Byte Y*      Mmin                Double      1           Little
        // Byte Y+8*    Mmax                Double      1           Little
        // Byte Y+16*   Marray              Double      NumPoints   Little
        // X = 48 + (16 * NumPoints)
        // Y = X + 16 + (8 * NumPoints)
        // * = optional
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
            var header = this.Header;
            // Check to ensure that the fileName is the correct shape type
            if (header.ShapeType != ShapeType.MultiPoint &&
                 header.ShapeType != ShapeType.MultiPointM &&
                 header.ShapeType != ShapeType.MultiPointZ)
            {
                throw new ArgumentException(DataStrings.FileNotLines_S.Replace("%S", fileName));
            }

            if (new FileInfo(fileName).Length == 100)
            {
                // the file is empty so we are done reading
                return;
            }


            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(fileName);
            int numShapes = shapeHeaders.Count;

            bool isM = (header.ShapeType == ShapeType.MultiPointZ || header.ShapeType == ShapeType.MultiPointM);
            bool isZ = (header.ShapeType == ShapeType.MultiPointZ);

            int totalPointsCount = 0;
            int totalPartsCount = 0;
            var shapeIndices = new List<ShapeRange>(numShapes);


            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 65536))
            {
                var boundsBytes = new byte[4 * 8];
                var bounds = new double[4];
                for (int shp = 0; shp < numShapes; shp++)
                {
                    // Read from the index file because some deleted records might still exist in the .shp file.
                    long offset = (shapeHeaders[shp].ByteOffset);
                    reader.Seek(offset, SeekOrigin.Begin);

                    var shape = new ShapeRange(FeatureType.MultiPoint, CoordinateType)
                    {
                        RecordNumber = reader.ReadInt32(Endian.BigEndian),
                        ContentLength = reader.ReadInt32(Endian.BigEndian),
                        ShapeType = (ShapeType)reader.ReadInt32(),
                        StartIndex = totalPointsCount,
                        NumParts = 1
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

                        //// Num Parts
                        //shape.NumParts = reader.ReadInt32();
                        totalPartsCount += 1;

                        // Num Points
                        shape.NumPoints = reader.ReadInt32();
                        totalPointsCount += shape.NumPoints;
                    }

                    shapeIndices.Add(shape);
                }

                var vert = new double[totalPointsCount * 2];
                var vertInd = 0;

                var parts = new int[totalPartsCount];
                //var partsInd = 0;

                int mArrayInd = 0, zArrayInd = 0;
                double[] mArray = null, zArray = null;
                if (isM)
                    mArray = new double[totalPointsCount];
                if (isZ)
                    zArray = new double[totalPointsCount];

                int partsOffset = 0;
                for (int shp = 0; shp < numShapes; shp++)
                {

                    var shape = shapeIndices[shp];
                    if (shape.ShapeType == ShapeType.NullShape) continue;
                    reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);
                    reader.Seek(3 * 4 + 32 + 4, SeekOrigin.Current); // Skip first bytes (Record Number, Content Length, Shapetype + BoundingBox + NumPoints)

                    //// Read parts
                    //var partsBytes = reader.ReadBytes(4 * shape.NumParts); //Numparts * Integer(4) = existing Parts
                    //Buffer.BlockCopy(partsBytes, 0, parts, partsInd, partsBytes.Length);
                    //partsInd += 4 * shape.NumParts;

                    // Read points
                    var pointsBytes = reader.ReadBytes(8 * 2 * shape.NumPoints); //Numpoints * Point (X(8) + Y(8))
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
                        var partR = new PartRange(vert, shape.StartIndex, parts[partsOffset + part], FeatureType.MultiPoint)
                        {
                            NumVertices = count
                        };
                        shape.Parts.Add(partR);
                    }
                    partsOffset += shape.NumParts;

                    // Fill M and Z arrays
                    switch (header.ShapeType)
                    {
                        case ShapeType.MultiPointM:
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
                        case ShapeType.MultiPointZ:
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
                                goto case ShapeType.MultiPointM;
                            }

                            break;
                    }
                }

                if (isM) M = mArray;
                if (isZ) Z = zArray;
                ShapeIndices = shapeIndices;
                Vertex = vert;
            }
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
                if (f != null)
                {
                    f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
                }
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
            Header = new ShapefileHeader(Filename);

            switch (Header.ShapeType)
            {
                case ShapeType.MultiPointM:
                    CoordinateType = CoordinateType.M;
                    break;
                case ShapeType.MultiPointZ:
                    CoordinateType = CoordinateType.Z;
                    break;
                default:
                    CoordinateType = CoordinateType.Regular;
                    break;
            }

            Extent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(Filename);
            FillPoints(Filename, progressHandler);
            ReadProjection();
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
            HeaderSaveAs(Filename);

            if (IndexMode)
            {
                SaveAsIndexed(Filename);
                return;
            }

            var bbWriter = new BufferedBinaryWriter(Filename);
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
                for (int iPart = 0; iPart < f.Geometry.NumGeometries; iPart++)
                {
                    IList<Coordinate> coords = f.Geometry.GetGeometryN(iPart).Coordinates;
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
                bbWriter.Write(f.Geometry.EnvelopeInternal.MinX);      // Byte 12   Xmin    Double      1           Little
                bbWriter.Write(f.Geometry.EnvelopeInternal.MinY);      // Byte 20   Ymin    Double      1           Little
                bbWriter.Write(f.Geometry.EnvelopeInternal.MaxX);      // Byte 28   Xmax    Double      1           Little
                bbWriter.Write(f.Geometry.EnvelopeInternal.MaxY);      // Byte 36   Ymax    Double      1           Little

                bbWriter.Write(points.Count);              // Byte 44   #Points Integer     1           Little
                // Byte X    Points   Point    #Points       Little

                foreach (Coordinate coord in points)
                {
                    bbWriter.Write(coord.X);
                    bbWriter.Write(coord.Y);
                }

                if (Header.ShapeType == ShapeType.MultiPointZ)
                {
                    bbWriter.Write(f.Geometry.EnvelopeInternal.Minimum.Z);
                    bbWriter.Write(f.Geometry.EnvelopeInternal.Maximum.Z);
                    foreach (Coordinate coord in points)
                    {
                        bbWriter.Write(coord.Z);
                    }
                }
                if (Header.ShapeType == ShapeType.MultiPointM || Header.ShapeType == ShapeType.MultiPointZ)
                {
                    if (f.Geometry.EnvelopeInternal == null)
                    {
                        bbWriter.Write(0.0);
                        bbWriter.Write(0.0);
                    }
                    else
                    {
                        bbWriter.Write(f.Geometry.EnvelopeInternal.Minimum.M);
                        bbWriter.Write(f.Geometry.EnvelopeInternal.Maximum.M);
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
            WriteFileLength(fileName, offset);
            UpdateAttributes();
            SaveProjection();
        }

        #endregion
    }
}