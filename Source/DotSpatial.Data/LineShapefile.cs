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
    /// A shapefile class that handles the special case where the vectors are lines
    /// </summary>
    public class LineShapefile : Shapefile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineShapefile"/> class for in-ram handling only.
        /// </summary>
        public LineShapefile()
            : base(FeatureType.Line, ShapeType.PolyLine)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineShapefile"/> class that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public LineShapefile(string fileName)
            : this()
        {
            Open(fileName, null);
        }

        /// <summary>
        /// Opens a shapefile.
        /// </summary>
        /// <param name="fileName">The string fileName of the line shapefile to load.</param>
        /// <param name="progressHandler">Any valid implementation of the DotSpatial.Data.IProgressHandler</param>
        public void Open(string fileName, IProgressHandler progressHandler)
        {
            if (!File.Exists(fileName)) return;

            Filename = fileName;
            IndexMode = true;
            Header = new ShapefileHeader(Filename);

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
            Attributes.Open(Filename);

            FillLines(Filename, progressHandler, this, FeatureType.Line);
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
        // X = 52 + 4 * NumParts

        // X Y M Poly Lines: Total Length = 34 Bytes
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
        // X = 52 + (4 * NumParts)
        // Y = X + (16 * NumPoints)
        // * = optional

        // X Y Z M Poly Lines: Total Length = 44 Bytes
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
        // X = 52 + (4 * NumParts)
        // Y = X + (16 * NumPoints)
        // Z = Y + 16 + (8 * NumPoints)
        // * = optional

        /// <summary>
        /// Gets the specified feature by constructing it from the vertices, rather
        /// than requiring that all the features be created. (which takes up a lot of memory).
        /// </summary>
        /// <param name="index">The integer index</param>
        /// <returns>The feature belonging to the specified index.</returns>
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

        /// <summary>
        /// Loads the shapes from the given file into the given shapefile.
        /// </summary>
        /// <param name="fileName">Name of the file whose shapes should get loaded.</param>
        /// <param name="progressHandler">ProgressHandler that shows the progress.</param>
        /// <param name="shapefile">Shapefile the shapes are loaded into.</param>
        /// <param name="featureType">FeatureType that should be inside the file.</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException, if the shapefile is null.</exception>
        /// <exception cref="ArgumentException">Throws an ArgumentException, if the FeatureType is Line but the files doesn't contain lines or the FeatureType is Polygon and the file doesn't contain polygons.</exception>
        /// <exception cref="NotSupportedException">Throws a NotSupportedException, if a FeatureType other than Line or Polygon is passed.</exception>
        /// <exception cref="FileNotFoundException">Throws a FileNotFoundException, if the file whith the path from fileName doesn't exist.</exception>
        /// <exception cref="NullReferenceException">Throws a NullReferenceException, if the fileName is null.</exception>
        internal static void FillLines(string fileName, IProgressHandler progressHandler, Shapefile shapefile, FeatureType featureType)
        {
            // Check to ensure that the fileName is the correct shape type
            switch (featureType)
            {
                case FeatureType.Line:
                    if (!CanBeRead(fileName, shapefile, ShapeType.PolyLine, ShapeType.PolyLineM, ShapeType.PolyLineZ)) return;

                    break;
                case FeatureType.Polygon:
                    if (!CanBeRead(fileName, shapefile, ShapeType.Polygon, ShapeType.PolygonM, ShapeType.PolygonZ)) return;
                    break;
                default:
                    throw new NotSupportedException(DataStrings.ShapeType0NotSupported);
            }

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            var header = shapefile.Header;
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
                    long offset = shapeHeaders[shp].ByteOffset;
                    reader.Seek(offset, SeekOrigin.Begin);

                    var shape = new ShapeRange(featureType, shapefile.CoordinateType)
                    {
                        RecordNumber = reader.ReadInt32(Endian.BigEndian),
                        ContentLength = reader.ReadInt32(Endian.BigEndian),
                        ShapeType = (ShapeType)reader.ReadInt32(),
                        StartIndex = totalPointsCount
                    };
                    Debug.Assert(shape.RecordNumber == shp + 1, "The shapes record number should equal" + shp + 1);

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
                    progressMeter.CurrentPercent = (int)(50 + (shp * 50.0 / numShapes));

                    var shape = shapeIndices[shp];
                    if (shape.ShapeType == ShapeType.NullShape) continue;
                    reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);
                    reader.Seek((3 * 4) + 32 + (2 * 4), SeekOrigin.Current); // Skip first bytes (Record Number, Content Length, Shapetype + BoundingBox + NumParts, NumPoints)

                    // Read parts
                    var partsBytes = reader.ReadBytes(4 * shape.NumParts); // Numparts * Integer(4) = existing Parts
                    Buffer.BlockCopy(partsBytes, 0, parts, partsInd, partsBytes.Length);
                    partsInd += 4 * shape.NumParts;

                    // Read points
                    var pointsBytes = reader.ReadBytes(8 * 2 * shape.NumPoints); // Numpoints * Point (X(8) + Y(8))
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
                            if (shape.ContentLength * 2 > 44 + (4 * shape.NumParts) + (16 * shape.NumPoints))
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

                            // These are listed as "optional" but there isn't a good indicator of how to determine if they were added.
                            // To handle the "optional" M values, check the contentLength for the feature.
                            // The content length does not include the 8-byte record header and is listed in 16-bit words.
                            if (shape.ContentLength * 2 > 60 + (4 * shape.NumParts) + (24 * shape.NumPoints))
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
        /// Populates the given streams for the shp and shx file when in IndexMode.
        /// </summary>
        /// <param name="shpStream">Stream that is used to write the shp file.</param>
        /// <param name="shxStream">Stream that is used to write the shx file.</param>
        /// <param name="shapefile">The shapefile that contains the features that are written.</param>
        /// <param name="expectedZType">Indicates which Z-ShapeType the header must have for the z values to be written.</param>
        /// <param name="expectedMType">Indicates which M-ShapeType the header must have for the m values to be written.</param>
        /// <param name="withParts">Indicates whether the parts should be written.</param>
        /// <returns>The lengths of the streams in bytes.</returns>
        internal static StreamLengthPair PopulateStreamsIndexed(Stream shpStream, Stream shxStream, Shapefile shapefile, ShapeType expectedZType, ShapeType expectedMType, bool withParts)
        {
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words

            foreach (ShapeRange shape in shapefile.ShapeIndices)
            {
                // null shapes have a contentLength of 2, all other shapes must have the same shape type
                int contentLength = shape.ShapeType == ShapeType.NullShape ? 2 : GetContentLength(shape.NumParts, shape.NumPoints, shapefile.Header.ShapeType);

                ////                                                           Index File
                //                                                             ---------------------------------------------------------
                //                                                             Position      Value              Type        Number      Byte Order
                //                                                             ---------------------------------------------------------
                shxStream.WriteBe(offset);                                     // Byte 0     Offset             Integer     1           Big
                shxStream.WriteBe(contentLength);                              // Byte 4     Content Length     Integer     1           Big

                ////                                                           X Y Poly Lines
                //                                                             ---------------------------------------------------------
                //                                                             Position        Value               Type        Number      Byte Order
                //                                                             ---------------------------------------------------------
                shpStream.WriteBe(fid + 1);                                    // Byte 0       Record Number       Integer     1           Big
                shpStream.WriteBe(contentLength);                              // Byte 4       Content Length      Integer     1           Big

                if (shape.ShapeType == ShapeType.NullShape)
                {
                    shpStream.WriteLe((int)ShapeType.NullShape);               // Byte 8       Shape Type 3        Integer     1           Little
                }
                else
                {
                    shpStream.WriteLe((int)shapefile.Header.ShapeType);        // Byte 8       Shape Type 3        Integer     1           Little
                    shpStream.WriteLe(shape.Extent.MinX);                      // Byte 12      Xmin                Double      1           Little
                    shpStream.WriteLe(shape.Extent.MinY);                      // Byte 20      Ymin                Double      1           Little
                    shpStream.WriteLe(shape.Extent.MaxX);                      // Byte 28      Xmax                Double      1           Little
                    shpStream.WriteLe(shape.Extent.MaxY);                      // Byte 36      Ymax                Double      1           Little
                    if (withParts) shpStream.WriteLe(shape.NumParts);          // Byte 44      NumParts            Integer     1           Little
                    shpStream.WriteLe(shape.NumPoints);                        // Byte 48      NumPoints           Integer     1           Little

                    if (withParts) ////                                           Byte 52      Parts               Integer     NumParts    Little
                    {
                        foreach (PartRange part in shape.Parts)
                        {
                            shpStream.WriteLe(part.PartOffset);
                        }
                    }

                    int start = shape.StartIndex;
                    int count = shape.NumPoints;
                    shpStream.WriteLe(shapefile.Vertex, start * 2, count * 2); // Byte X       Points              Point       NumPoints   Little

                    if (shapefile.Header.ShapeType == expectedZType)
                    {
                        WriteValues(shapefile.Z, start, count, shpStream);
                    }

                    if (shapefile.Header.ShapeType == expectedMType || shapefile.Header.ShapeType == expectedZType)
                    {
                        WriteValues(shapefile.M, start, count, shpStream);
                    }
                }

                fid++;
                offset += 4; // header bytes
                offset += contentLength; // adding the content length from each loop calculates the word offset
            }

            return new StreamLengthPair { ShpLength = offset, ShxLength = 50 + (fid * 4) };
        }

        /// <summary>
        /// Populates the given streams for the shp and shx file when not in IndexMode.
        /// </summary>
        /// <param name="shpStream">Stream that is used to write the shp file.</param>
        /// <param name="shxStream">Stream that is used to write the shx file.</param>
        /// <param name="shapefile">The shapefile that contains the features that are written.</param>
        /// <param name="addPoints">Function that is used to add the points from the features to the parts and points lists.</param>
        /// <param name="expectedZType">Indicates which Z-ShapeType the header must have for the z values to be written.</param>
        /// <param name="expectedMType">Indicates which M-ShapeType the header must have for the m values to be written.</param>
        /// <param name="withParts">Indicates whether the parts should be written.</param>
        /// <returns>The lengths of the streams in bytes.</returns>
        internal static StreamLengthPair PopulateStreamsNotIndexed(Stream shpStream, Stream shxStream, Shapefile shapefile, Action<List<int>, List<Coordinate>, IFeature> addPoints, ShapeType expectedZType, ShapeType expectedMType, bool withParts)
        {
            var progressMeter = new ProgressMeter(shapefile.ProgressHandler, "Saving (Not Indexed)...", shapefile.Features.Count);

            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words

            foreach (IFeature f in shapefile.Features)
            {
                List<int> parts = new List<int>();
                List<Coordinate> points = new List<Coordinate>();

                addPoints(parts, points, f);

                bool isNullShape = false;
                int contentLength;

                // null shapes have a contentLength of 2, all other shapes must have the same shape type
                if (f.Geometry.IsEmpty)
                {
                    contentLength = 2;
                    isNullShape = true;
                }
                else
                {
                    contentLength = GetContentLength(parts.Count, points.Count, shapefile.Header.ShapeType);
                }

                ////                                                      Index File
                //                                                       ---------------------------------------------------------
                //                                                       Position     Value          Type       Number  Byte Order
                //                                                       ---------------------------------------------------------
                shxStream.WriteBe(offset);                               // Byte 0     Offset         Integer     1      Big
                shxStream.WriteBe(contentLength);                        // Byte 4     Content Length Integer     1      Big

                ////                                                     X Y Poly Lines
                //                                                       ---------------------------------------------------------
                //                                                       Position     Value         Type        Number      Byte Order
                //                                                       -------------------------------------------------------
                shpStream.WriteBe(fid + 1);                              // Byte 0   Record Number  Integer     1           Big
                shpStream.WriteBe(contentLength);                        // Byte 4   Content Length Integer     1           Big

                if (isNullShape)
                {
                    shpStream.WriteLe((int)ShapeType.NullShape);         // Byte 8   Shape Type 0   Integer     1           Little
                }
                else
                {
                    shpStream.WriteLe((int)shapefile.Header.ShapeType);  // Byte 8   Shape Type     Integer     1           Little

                    shpStream.WriteLe(f.Geometry.EnvelopeInternal.MinX); // Byte 12   Xmin          Double      1           Little
                    shpStream.WriteLe(f.Geometry.EnvelopeInternal.MinY); // Byte 20   Ymin          Double      1           Little
                    shpStream.WriteLe(f.Geometry.EnvelopeInternal.MaxX); // Byte 28   Xmax          Double      1           Little
                    shpStream.WriteLe(f.Geometry.EnvelopeInternal.MaxY); // Byte 36   Ymax          Double      1           Little
                    if (withParts) shpStream.WriteLe(parts.Count);       // Byte 44   NumParts      Integer     1           Little
                    shpStream.WriteLe(points.Count);                     // Byte 48   NumPoints     Integer     1           Little

                    if (withParts) ////                                     Byte 52   Parts         Integer     NumParts    Little
                    {
                        foreach (int part in parts)
                        {
                            shpStream.WriteLe(part);
                        }
                    }

                    double[] xyVals = new double[points.Count * 2];      // Byte X     Points              Point       NumPoints   Little
                    for (int i = 0; i < points.Count; i++)
                    {
                        xyVals[i * 2] = points[i].X;
                        xyVals[(i * 2) + 1] = points[i].Y;
                    }

                    shpStream.WriteLe(xyVals);

                    if (shapefile.Header.ShapeType == expectedZType)
                    {
                        shpStream.WriteLe(f.Geometry.EnvelopeInternal.Minimum.Z);
                        shpStream.WriteLe(f.Geometry.EnvelopeInternal.Maximum.Z);
                        double[] zVals = new double[points.Count];
                        for (int i = 0; i < points.Count; i++)
                        {
                            zVals[i] = points[i].Z;
                        }

                        shpStream.WriteLe(zVals);
                    }

                    if (shapefile.Header.ShapeType == expectedMType || shapefile.Header.ShapeType == expectedZType)
                    {
                        shpStream.WriteLe(f.Geometry.EnvelopeInternal.Minimum.M);
                        shpStream.WriteLe(f.Geometry.EnvelopeInternal.Maximum.M);

                        double[] mVals = new double[points.Count];
                        for (int i = 0; i < points.Count; i++)
                        {
                            mVals[i] = points[i].M;
                        }

                        shpStream.WriteLe(mVals);
                    }
                }

                progressMeter.CurrentValue = fid;
                fid++;
                offset += 4; // header bytes
                offset += contentLength; // adding the content length from each loop calculates the word offset
            }

            progressMeter.Reset();

            return new StreamLengthPair { ShpLength = offset, ShxLength = 50 + (fid * 4) };
        }

        /// <summary>
        /// Populates the given streams for the shp and shx file.
        /// </summary>
        /// <param name="shpStream">Stream that is used to write the shp file.</param>
        /// <param name="shxStream">Stream that is used to write the shx file.</param>
        /// <param name="indexed">Indicates whether the streams are populated in IndexMode.</param>
        /// <returns>The lengths of the streams in bytes.</returns>
        protected override StreamLengthPair PopulateShpAndShxStreams(Stream shpStream, Stream shxStream, bool indexed)
        {
            if (indexed)
                return PopulateStreamsIndexed(shpStream, shxStream, this, ShapeType.PolyLineZ, ShapeType.PolyLineM, true);

            return PopulateStreamsNotIndexed(shpStream, shxStream, this, AddPoints, ShapeType.PolyLineZ, ShapeType.PolyLineM, true);
        }

        /// <inheritdoc />
        protected override void SetHeaderShapeType()
        {
            if (CoordinateType == CoordinateType.Regular)
                Header.ShapeType = ShapeType.PolyLine;
            else if (CoordinateType == CoordinateType.M)
                Header.ShapeType = ShapeType.PolyLineM;
            else if (CoordinateType == CoordinateType.Z)
                Header.ShapeType = ShapeType.PolyLineZ;
        }

        /// <summary>
        /// Adds the parts and points of the given feature to the given parts and points lists.
        /// </summary>
        /// <param name="parts">List of parts, where the features parts get added.</param>
        /// <param name="points">List of points, where the features points get added.</param>
        /// <param name="f">Feature, whose parts and points get added to the lists.</param>
        private static void AddPoints(List<int> parts, List<Coordinate> points, IFeature f)
        {
            for (int iPart = 0; iPart < f.Geometry.NumGeometries; iPart++)
            {
                parts.Add(points.Count);
                ILineString bl = f.Geometry.GetGeometryN(iPart) as ILineString;
                if (bl == null) continue;
                points.AddRange(bl.Coordinates);
            }
        }

        /// <summary>
        /// Calculates the ContentLength that is needed to save a shape with the given number of parts and points.
        /// </summary>
        /// <param name="numParts">Number of parts, that belong to the shape.</param>
        /// <param name="numPoints">Number of points, that belong to the shape.</param>
        /// <param name="header">The shapefile header that contains the shapetype needed for calculation.</param>
        /// <returns>ContentLength that is needed to save a shape with the given number of parts and points.</returns>
        private static int GetContentLength(int numParts, int numPoints, ShapeType header)
        {
            int contentLength = 20;

            // add NumParts for Polygon and Polyline Shapefiles
            if (header != ShapeType.MultiPoint && header != ShapeType.MultiPointM && header != ShapeType.MultiPointZ)
                contentLength += 2;

            contentLength += 2 * numParts;
            switch (header)
            {
                case ShapeType.MultiPoint:
                case ShapeType.Polygon:
                case ShapeType.PolyLine:
                    contentLength += numPoints * 8;
                    break;
                case ShapeType.MultiPointM:
                case ShapeType.PolygonM:
                case ShapeType.PolyLineM:
                    contentLength += 8; // mmin mmax
                    contentLength += numPoints * 12; // x, y, m
                    break;
                case ShapeType.MultiPointZ:
                case ShapeType.PolygonZ:
                case ShapeType.PolyLineZ:
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += numPoints * 16; // x, y, m, z
                    break;
            }

            return contentLength;
        }

        /// <summary>
        /// Takes count values from start and writes them to the stream.
        /// </summary>
        /// <param name="values">Values, whose subset gets written to stream.</param>
        /// <param name="start">Position of the first value, that gets written.</param>
        /// <param name="count">Number of values, that get written.</param>
        /// <param name="shpStream">Stream, the values get written to.</param>
        private static void WriteValues(double[] values, int start, int count, Stream shpStream)
        {
            if (values != null && values.Length >= start + count)
            {
                double[] vals = new double[count];
                Array.Copy(values, start, vals, 0, count);
                shpStream.WriteLe(vals.Min());
                shpStream.WriteLe(vals.Max());
                shpStream.WriteLe(vals);
            }
        }
    }
}