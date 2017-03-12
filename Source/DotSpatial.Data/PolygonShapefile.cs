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
using System.IO;
using System.Linq;
using GeoAPI.Geometries;
using NetTopologySuite.Algorithm;

namespace DotSpatial.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where the data is made up of polygons
    /// </summary>
    public class PolygonShapefile : Shapefile
    {
        /// <summary>
        /// Creates a new instance of a PolygonShapefile for in-ram handling only.
        /// </summary>
        public PolygonShapefile()
            : base(FeatureType.Polygon)
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader { FileLength = 100, ShapeType = ShapeType.Polygon };
        }

        /// <summary>
        /// Creates a new instance of a PolygonShapefile that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public PolygonShapefile(string fileName)
            : this()
        {
            Open(fileName, null);
        }

        /// <summary>
        /// Opens a shapefile
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        /// <param name="progressHandler">Any valid implementation of the DotSpatial.Data.IProgressHandler</param>
        public void Open(string fileName, IProgressHandler progressHandler)
        {
            if (!File.Exists(fileName)) return;

            Filename = fileName;
            IndexMode = true;
            Header = new ShapefileHeader(Filename);

            switch (Header.ShapeType)
            {
                case ShapeType.PolygonM:
                    CoordinateType = CoordinateType.M;
                    break;
                case ShapeType.PolygonZ:
                    CoordinateType = CoordinateType.Z;
                    break;
                default:
                    CoordinateType = CoordinateType.Regular;
                    break;
            }

            Extent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(Filename);

            LineShapefile.FillLines(Filename, progressHandler, this, FeatureType.Polygon);
            ReadProjection();
        }

        // X Y Poly Lines: Total Length = 28 Bytes
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
        public override IFeature GetFeature(int index)
        {
            IFeature f;
            if (!IndexMode)
            {
                f = Features[index];
            }
            else
            {
                f = GetPolygon(index);
                f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
            }

            return f;
        }
        
        /// <summary>
        /// update header with correct shape type
        /// </summary>
        internal void SetHeaderShapeType()
        {
            // Set ShapeType before setting extent.
            if (CoordinateType == CoordinateType.Regular)
            {
                Header.ShapeType = ShapeType.Polygon;
            }
            if (CoordinateType == CoordinateType.M)
            {
                Header.ShapeType = ShapeType.PolygonM;
            }
            if (CoordinateType == CoordinateType.Z)
            {
                Header.ShapeType = ShapeType.PolygonZ;
            }
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

            SetHeaderShapeType(); 

            HeaderSaveAs(fileName);

            if (IndexMode)
            {
                var shpStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None, 10000000);
                var shxStream = new FileStream(Header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 10000000);
                var streamlengths = PopulateShpAndShxStreamsIndexed(shpStream, shxStream);
                shpStream.Close();
                shxStream.Close();
                WriteFileLength(fileName, streamlengths.ShpLength);
                WriteFileLength(Header.ShxFilename, streamlengths.ShxLength);
            }
            else
            {
                var shpWriter = new BufferedBinaryWriter(fileName);
                var shxWriter = new BufferedBinaryWriter(Header.ShxFilename);
                var streamlengths = PopulateShpAndShxStreamsNotIndexed(shpWriter, shxWriter);
                shpWriter.Close();
                shxWriter.Close();
                WriteFileLength(fileName, streamlengths.ShpLength);
                WriteFileLength(Header.ShxFilename, streamlengths.ShxLength);
            }
            UpdateAttributes();
            SaveProjection();
        }

        /// <summary>
        /// populates shp and shx file via buffered binary writers for NONINDEXed mode
        /// </summary>
        /// <param name="shpWriter"></param>
        /// <param name="shxWriter"></param>
        /// <returns>lengths of streams in bytes</returns>
        private StreamLengthPair PopulateShpAndShxStreamsNotIndexed(BufferedBinaryWriter shpWriter, BufferedBinaryWriter shxWriter)
        {
            int fid = 0;
            int offset = 50;
            int contentLength = 0;
            foreach (IFeature f in Features)
            {
                List<int> parts = new List<int>();

                offset += contentLength; // adding the previous content length from each loop calculates the word offset
                List<Coordinate> points = new List<Coordinate>();
                contentLength = 22;
                for (int iPart = 0; iPart < f.Geometry.NumGeometries; iPart++)
                {
                    parts.Add(points.Count);
                    IPolygon pg = f.Geometry.GetGeometryN(iPart) as IPolygon;
                    if (pg == null) continue;
                    ILineString bl = pg.Shell;
                    IEnumerable<Coordinate> coords = bl.Coordinates;

                    if (CGAlgorithms.IsCCW(bl.Coordinates))
                    {
                        // Exterior rings need to be clockwise
                        coords = coords.Reverse();
                    }

                    foreach (Coordinate coord in coords)
                    {
                        points.Add(coord);
                    }
                    foreach (ILineString hole in pg.Holes)
                    {
                        parts.Add(points.Count);
                        IEnumerable<Coordinate> holeCoords = hole.Coordinates;
                        if (!CGAlgorithms.IsCCW(hole.Coordinates))
                        {
                            // Interior rings need to be counter-clockwise
                            holeCoords = holeCoords.Reverse();
                        }
                        foreach (Coordinate coord in holeCoords)
                        {
                            points.Add(coord);
                        }
                    }
                }
                contentLength += 2 * parts.Count;
                if (Header.ShapeType == ShapeType.Polygon)
                {
                    contentLength += points.Count * 8;
                }
                if (Header.ShapeType == ShapeType.PolygonM)
                {
                    contentLength += 8; // mmin mmax
                    contentLength += points.Count * 12; // x, y, m
                }
                if (Header.ShapeType == ShapeType.PolygonZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += points.Count * 16; // x, y, m, z
                }

                //                                              Index File
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                shxWriter.Write(offset, false);               // Byte 0     Offset             Integer     1           Big
                shxWriter.Write(contentLength, false);        // Byte 4    Content Length      Integer     1           Big

                //                                              X Y Poly Lines
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                shpWriter.Write(fid + 1, false);                  // Byte 0       Record Number       Integer     1           Big
                shpWriter.Write(contentLength, false);        // Byte 4       Content Length      Integer     1           Big
                shpWriter.Write((int)Header.ShapeType);       // Byte 8       Shape Type 3        Integer     1           Little
                if (Header.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }

                shpWriter.Write(f.Geometry.EnvelopeInternal.MinX);             // Byte 12      Xmin                Double      1           Little
                shpWriter.Write(f.Geometry.EnvelopeInternal.MinY);             // Byte 20      Ymin                Double      1           Little
                shpWriter.Write(f.Geometry.EnvelopeInternal.MaxX);             // Byte 28      Xmax                Double      1           Little
                shpWriter.Write(f.Geometry.EnvelopeInternal.MaxY);             // Byte 36      Ymax                Double      1           Little
                shpWriter.Write(parts.Count);                 // Byte 44      NumParts            Integer     1           Little
                shpWriter.Write(points.Count);                // Byte 48      NumPoints           Integer     1           Little
                                                             // Byte 52      Parts               Integer     NumParts    Little
                foreach (int iPart in parts)
                {
                    shpWriter.Write(iPart);
                }
                double[] xyVals = new double[points.Count * 2];

                int i = 0;

                // Byte X       Points              Point       NumPoints   Little
                foreach (Coordinate coord in points)
                {
                    xyVals[i * 2] = coord.X;
                    xyVals[i * 2 + 1] = coord.Y;
                    i++;
                }
                shpWriter.Write(xyVals);

                if (Header.ShapeType == ShapeType.PolygonZ)
                {
                    shpWriter.Write(f.Geometry.EnvelopeInternal.Minimum.Z);
                    shpWriter.Write(f.Geometry.EnvelopeInternal.Maximum.Z);
                    double[] zVals = new double[points.Count];
                    for (int ipoint = 0; ipoint < points.Count; i++)
                    {
                        zVals[ipoint] = points[ipoint].Z;
                        ipoint++;
                    }
                    shpWriter.Write(zVals);
                }

                if (Header.ShapeType == ShapeType.PolygonM || Header.ShapeType == ShapeType.PolygonZ)
                {
                    if (f.Geometry.EnvelopeInternal == null)
                    {
                        shpWriter.Write(0.0);
                        shpWriter.Write(0.0);
                    }
                    else
                    {
                        shpWriter.Write(f.Geometry.EnvelopeInternal.Minimum.M);
                        shpWriter.Write(f.Geometry.EnvelopeInternal.Maximum.M);
                    }

                    double[] mVals = new double[points.Count];
                    for (int ipoint = 0; ipoint < points.Count; i++)
                    {
                        mVals[ipoint] = points[ipoint].M;
                        ipoint++;
                    }
                    shpWriter.Write(mVals);
                }

                fid++;
                offset += 4; // header bytes
            }

            offset += contentLength;

            return new StreamLengthPair() { ShpLength = offset, ShxLength = 50 + fid * 4 };  
        }

        /// <summary>
        /// populate shp and shx streams for INDEXED mode
        /// </summary>
        /// <param name="shpStream">ShpStream</param>
        /// <param name="shxStream">ShxStream</param>
        /// <returns>lengths of streams in bytes</returns>
        private StreamLengthPair PopulateShpAndShxStreamsIndexed(Stream shpStream, Stream shxStream)
        {
            int fid = 0;
            int offset = 50;
            int contentLength = 0;
            foreach (ShapeRange shape in ShapeIndices)
            {
                offset += contentLength; // adding the previous content length from each loop calculates the word offset
                contentLength = 22;

                contentLength += 2 * shape.NumParts;
                if (Header.ShapeType == ShapeType.Polygon)
                {
                    contentLength += shape.NumPoints * 8;
                }
                if (Header.ShapeType == ShapeType.PolygonM)
                {
                    contentLength += 8; // mmin mmax
                    contentLength += shape.NumPoints * 12; // x, y, m
                }
                if (Header.ShapeType == ShapeType.PolygonZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += shape.NumPoints * 16; // x, y, m, z
                }

                //                                         Index File
                //                                        ---------------------------------------------------------
                //                                          Position   Value            Type    Number  Byte Order
                //                                        ---------------------------------------------------------
                shxStream.WriteBe(offset);                // Byte 0    Offset           Integer   1     Big
                shxStream.WriteBe(contentLength);         // Byte 4    Content Length   Integer   1     Big

                //                                         X Y Poly Lines
                //                                        ---------------------------------------------------------
                //                                           Position   Value           Type    Number Byte Order
                //                                        ---------------------------------------------------------
                shpStream.WriteBe(fid + 1);               // Byte 0     Record Number   Integer   1    Big
                shpStream.WriteBe(contentLength);         // Byte 4     Content Length  Integer   1    Big
                shpStream.WriteLe((int)Header.ShapeType); // Byte 8     Shape Type 3    Integer   1    Little
                if (Header.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }

                shpStream.WriteLe(shape.Extent.MinX);     // Byte 12    Xmin             Double   1    Little
                shpStream.WriteLe(shape.Extent.MinY);     // Byte 20    Ymin             Double   1    Little
                shpStream.WriteLe(shape.Extent.MaxX);     // Byte 28    Xmax             Double   1    Little
                shpStream.WriteLe(shape.Extent.MaxY);     // Byte 36    Ymax             Double   1    Little
                shpStream.WriteLe(shape.NumParts);        // Byte 44    NumParts         Integer  1    Little
                shpStream.WriteLe(shape.NumPoints);       // Byte 48    NumPoints        Integer  1    Little
                // Byte 52    Parts            Integer NumParts  Little
                foreach (PartRange part in shape.Parts)
                {
                    shpStream.WriteLe(part.PartOffset);
                }
                int start = shape.StartIndex;
                int count = shape.NumPoints;
                shpStream.WriteLe(Vertex, start * 2, count * 2);
                if (Header.ShapeType == ShapeType.PolygonZ)
                {
                    double[] shapeZ = new double[count];
                    Array.Copy(Z, start, shapeZ, 0, count);
                    shpStream.WriteLe(shapeZ.Min());
                    shpStream.WriteLe(shapeZ.Max());
                    shpStream.WriteLe(Z, start, count);
                }

                if (Header.ShapeType == ShapeType.PolygonM || Header.ShapeType == ShapeType.PolygonZ)
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
            
            offset += contentLength;

            return new StreamLengthPair() { ShpLength = offset, ShxLength = 50 + fid * 4 };
        }

        /// <inheritdoc />
        public override ShapefilePackage ExportShapefilePackage()
        {
            SetHeaderShapeType();

            InvalidateEnvelope();
            Header.SetExtent(Extent);
            Header.ShxLength = IndexMode ? ShapeIndices.Count * 4 + 50 : Features.Count * 4 + 50;

            // get the streams with headers 
            Stream shpStream = Header.ExportSHPToStream();
            Stream shxStream = Header.ExportSHXToStream();

            if (IndexMode)
            {
                var streamlengths = PopulateShpAndShxStreamsIndexed(shpStream, shxStream);
                // writefilelength
                WriteFileLength(shpStream, streamlengths.ShpLength);
                WriteFileLength(shxStream, streamlengths.ShxLength);
            }
            else
            {
                var shpWriter = new BufferedBinaryWriter(shpStream);
                var shxWriter = new BufferedBinaryWriter(shxStream);
                var streamlengths = PopulateShpAndShxStreamsNotIndexed(shpWriter, shxWriter);
                WriteFileLength(shpStream, streamlengths.ShpLength);
                WriteFileLength(shxStream, streamlengths.ShxLength);
            }
            shxStream.Seek(0, SeekOrigin.Begin);
            shpStream.Seek(0, SeekOrigin.Begin);
            return PackageStreams(shpStream, shxStream);
        }

    }
}