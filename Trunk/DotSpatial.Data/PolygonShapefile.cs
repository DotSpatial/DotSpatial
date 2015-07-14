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
using DotSpatial.Topology.Algorithm;

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
            Header = new ShapefileHeader(fileName);

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
            Attributes.Open(fileName);

            LineShapefile.FillLines(fileName, progressHandler, this, FeatureType.Polygon);
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
                    IBasicPolygon pg = f.GetBasicGeometryN(iPart) as IBasicPolygon;
                    if (pg == null) continue;
                    IBasicLineString bl = pg.Shell;
                    IEnumerable<Coordinate> coords = bl.Coordinates;

                    if (CgAlgorithms.IsCounterClockwise(bl.Coordinates))
                    {
                        // Exterior rings need to be clockwise
                        coords = coords.Reverse();
                    }

                    foreach (Coordinate coord in coords)
                    {
                        points.Add(coord);
                    }
                    foreach (IBasicLineString hole in pg.Holes)
                    {
                        parts.Add(points.Count);
                        IEnumerable<Coordinate> holeCoords = hole.Coordinates;
                        if (!CgAlgorithms.IsCounterClockwise(hole.Coordinates))
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
                indexWriter.Write(offset, false);               // Byte 0     Offset             Integer     1           Big
                indexWriter.Write(contentLength, false);        // Byte 4    Content Length      Integer     1           Big

                //                                              X Y Poly Lines
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                bbWriter.Write(fid + 1, false);                  // Byte 0       Record Number       Integer     1           Big
                bbWriter.Write(contentLength, false);        // Byte 4       Content Length      Integer     1           Big
                bbWriter.Write((int)Header.ShapeType);       // Byte 8       Shape Type 3        Integer     1           Little
                if (Header.ShapeType == ShapeType.NullShape)
                {
                    continue;
                }

                bbWriter.Write(f.Envelope.Minimum.X);             // Byte 12      Xmin                Double      1           Little
                bbWriter.Write(f.Envelope.Minimum.Y);             // Byte 20      Ymin                Double      1           Little
                bbWriter.Write(f.Envelope.Maximum.X);             // Byte 28      Xmax                Double      1           Little
                bbWriter.Write(f.Envelope.Maximum.Y);             // Byte 36      Ymax                Double      1           Little
                bbWriter.Write(parts.Count);                 // Byte 44      NumParts            Integer     1           Little
                bbWriter.Write(points.Count);                // Byte 48      NumPoints           Integer     1           Little
                // Byte 52      Parts               Integer     NumParts    Little
                foreach (int iPart in parts)
                {
                    bbWriter.Write(iPart);
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
                bbWriter.Write(xyVals);

                if (Header.ShapeType == ShapeType.PolygonZ)
                {
                    bbWriter.Write(f.Envelope.Minimum.Z);
                    bbWriter.Write(f.Envelope.Maximum.Z);
                    double[] zVals = new double[points.Count];
                    for (int ipoint = 0; ipoint < points.Count; i++)
                    {
                        zVals[ipoint] = points[ipoint].Z;
                        ipoint++;
                    }
                    bbWriter.Write(zVals);
                }

                if (Header.ShapeType == ShapeType.PolygonM || Header.ShapeType == ShapeType.PolygonZ)
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
                    for (int ipoint = 0; ipoint < points.Count; i++)
                    {
                        mVals[ipoint] = points[ipoint].M;
                        ipoint++;
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
            WriteFileLength(fileName, offset);
            WriteFileLength(Header.ShxFilename, 50 + fid * 4);
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

            shpStream.Close();
            shxStream.Close();
            offset += contentLength;
            WriteFileLength(fileName, offset);
            WriteFileLength(Header.ShxFilename, 50 + fid * 4);
            UpdateAttributes();
            SaveProjection();
        }
    }
}