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
// The Initial Developer of this Original Code is Ted Dunsford. Created in January, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where the data type is point
    /// </summary>
    public class PointShapefile : Shapefile
    {
        /// <summary>
        /// Creates a new instance of a PointShapefile for in-ram handling only.
        /// </summary>
        public PointShapefile()
            : base(FeatureType.Point)
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader { FileLength = 100, ShapeType = ShapeType.Point };
        }

        /// <summary>
        /// Creates a new instance of a PointShapefile that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public PointShapefile(string fileName)
            : this()
        {
            Open(fileName, null);
        }

        /// <summary>
        /// Opens a shapefile
        /// </summary>
        /// <param name="fileName">The string fileName of the point shapefile to load</param>
        /// <param name="progressHandler">Any valid implementation of the DotSpatial.Data.IProgressHandler</param>
        public void Open(string fileName, IProgressHandler progressHandler)
        {
            if (!File.Exists(fileName)) return;

            Filename = fileName;
            IndexMode = true;
            Header = new ShapefileHeader(fileName);

            switch (Header.ShapeType)
            {
                case ShapeType.PointM:
                    CoordinateType = CoordinateType.M;
                    break;
                case ShapeType.PointZ:
                    CoordinateType = CoordinateType.Z;
                    break;
                default:
                    CoordinateType = CoordinateType.Regular;
                    break;
            }

            Extent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(fileName);

            FillPoints(fileName, progressHandler);
            ReadProjection();
        }

        /// <summary>
        /// Obtains a typed list of ShapefilePoint structures with double values associated with the various coordinates.
        /// </summary>
        /// <param name="fileName">A string fileName</param>
        /// <param name="progressHandler">Progress handler</param>
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

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(fileName);

            // Get the basic header information.
            var header = new ShapefileHeader(fileName);
            Extent = header.ToExtent();
            // Check to ensure that the fileName is the correct shape type
            if (header.ShapeType != ShapeType.Point && header.ShapeType != ShapeType.PointM
                && header.ShapeType != ShapeType.PointZ)
            {
                throw new ApplicationException(DataStrings.FileNotPoints_S.Replace("%S", fileName));
            }

            if (new FileInfo(fileName).Length == 100)
            {
                // the file is empty so we are done reading
                return;
            }

            var numShapes = shapeHeaders.Count;
            double[] m = null;
            double[] z = null;
            var vert = new double[2 * numShapes]; // X,Y

            if (header.ShapeType == ShapeType.PointM || header.ShapeType == ShapeType.PointZ)
            {
                m = new double[numShapes];
            }
            if (header.ShapeType == ShapeType.PointZ)
            {
                z = new double[numShapes];
            }

            var progressMeter = new ProgressMeter(progressHandler, "Reading from " + Path.GetFileName(fileName))
            {
                StepPercent = 5
            };
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                for (var shp = 0; shp < numShapes; shp++)
                {
                    progressMeter.CurrentPercent = (int)(shp * 100.0 / numShapes);

                    reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);

                    var recordNumber = reader.ReadInt32(Endian.BigEndian);
                    Debug.Assert(recordNumber == shp + 1);
                    var contentLen = reader.ReadInt32(Endian.BigEndian);
                    Debug.Assert(contentLen == shapeHeaders[shp].ContentLength);

                    var shapeType = (ShapeType)reader.ReadInt32();
                    if (shapeType == ShapeType.NullShape)
                    {
                        if (m != null)
                        {
                            m[shp] = double.MinValue;
                        }
                        goto fin;
                    }

                    // Read X
                    var ind = 4;
                    vert[shp * 2] = reader.ReadDouble();
                    ind += 8;

                    // Read Y
                    vert[shp * 2 + 1] = reader.ReadDouble();
                    ind += 8;

                    // Read Z
                    if (z != null)
                    {
                        z[shp] = reader.ReadDouble();
                        ind += 8;
                    }

                    // Read M
                    if (m != null)
                    {
                        if (shapeHeaders[shp].ByteLength <= ind)
                        {
                            m[shp] = double.MinValue;
                        }
                        else
                        {
                            m[shp] = reader.ReadDouble();
                            ind += 8;
                        }
                    }

                fin:
                    var shape = new ShapeRange(FeatureType.Point)
                    {
                        RecordNumber = recordNumber,
                        StartIndex = shp,
                        ContentLength = shapeHeaders[shp].ContentLength,
                        NumPoints = 1,
                        NumParts = 1
                    };
                    ShapeIndices.Add(shape);
                    var part = new PartRange(vert, shp, 0, FeatureType.Point) { NumVertices = 1 };
                    shape.Parts.Add(part);
                    shape.Extent = new Extent(new[] { vert[shp * 2], vert[shp * 2 + 1], vert[shp * 2], vert[shp * 2 + 1] });

                }
            }

            Vertex = vert;
            M = m;
            Z = z;

            progressMeter.Reset();
        }

        /// <inheritdoc />
        public override IFeature GetFeature(int index)
        {
            IFeature f;
            if (!IndexMode)
            {
                f = Features[index];
            }
            else
            {
                f = GetPoint(index);
                f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
            }
            return f;
        }

        /// <summary>
        /// Saves the shapefile to a different fileName, but still as a shapefile.  This method does not support saving to
        /// any other file format.
        /// </summary>
        /// <param name="fileName">The string fileName to save as</param>
        /// <param name="overwrite">A boolean that is true if the file should be overwritten</param>
        public override void SaveAs(string fileName, bool overwrite)
        {
            EnsureValidFileToSave(fileName, overwrite);
            Filename = fileName;

            // Set Header.ShapeType before setting extent.
            // wordSize is the length of the byte representation in 16 bit words of a single shape, including header.
            int wordSize = 14; // 3 int(2) and 2 double(4)
            if (CoordinateType == CoordinateType.Regular)
            {
                Header.ShapeType = ShapeType.Point;
            }
            if (CoordinateType == CoordinateType.M)
            {
                Header.ShapeType = ShapeType.PointM;
                wordSize = 18; // 3 int(2), 3 double(4)
            }
            if (CoordinateType == CoordinateType.Z)
            {
                Header.ShapeType = ShapeType.PointZ;
                wordSize = 22; // 3 int(2), 4 double (4)
            }

            InvalidateEnvelope();
            Header.SetExtent(Extent);

            if (IndexMode)
            {
                Header.ShxLength = ShapeIndices.Count * 4 + 50;
                Header.FileLength = ShapeIndices.Count * wordSize + 50;
            }
            else
            {
                Header.ShxLength = Features.Count * 4 + 50;
                Header.FileLength = Features.Count * wordSize + 50;
            }

            Header.SaveAs(fileName);
            var shpStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None, 1000000);
            var shxStream = new FileStream(Header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 1000000);

            // Special slightly faster writing for index mode
            if (IndexMode)
            {
                for (int shp = 0; shp < ShapeIndices.Count; shp++)
                {
                    shpStream.WriteBe(shp + 1);
                    shpStream.WriteBe(wordSize - 4); // shape word size without 4 shapeHeader words.
                    shxStream.WriteBe(50 + shp * wordSize);
                    shxStream.WriteBe(wordSize - 4);
                    shpStream.WriteLe((int)Header.ShapeType);
                    shpStream.WriteLe(Vertex[shp * 2]);
                    shpStream.WriteLe(Vertex[shp * 2 + 1]);
                    if (Z != null) shpStream.WriteLe(Z[shp]);
                    if (M != null) shpStream.WriteLe(M[shp]);
                }
            }
            else
            {
                int fid = 0;
                foreach (IFeature f in Features)
                {
                    //CGX

                    //Coordinate c = f.BasicGeometry.Coordinates[0];
                    shpStream.WriteBe(fid + 1);
                    shpStream.WriteBe(wordSize - 4);
                    shxStream.WriteBe(50 + fid * wordSize);
                    shxStream.WriteBe(wordSize - 4);
                    shpStream.WriteLe((int)Header.ShapeType);
                    if (Header.ShapeType == ShapeType.NullShape)
                    {
                        continue;
                    }
                    //CGX
                    if (f.BasicGeometry != null)
                    {
                        Coordinate c = f.BasicGeometry.Coordinates[0];
                        shpStream.WriteLe(c.X);
                        shpStream.WriteLe(c.Y);
                        if (Header.ShapeType == ShapeType.PointZ)
                        {
                            shpStream.WriteLe(c.Z);
                        }
                        if (Header.ShapeType == ShapeType.PointM || Header.ShapeType == ShapeType.PointZ)
                        {
                            shpStream.WriteLe(c.M);
                        }
                    }
                    fid++;
                }
            }

            shpStream.Close();
            shxStream.Close();

            UpdateAttributes();
            SaveProjection();
        }
    }
}