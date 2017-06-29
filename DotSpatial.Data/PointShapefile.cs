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
            //JK - handle case when filename doesn't exist
            if (!File.Exists(fileName))
            {
                Attributes = new AttributeTable();
                Header = new ShapefileHeader { FileLength = 100, ShapeType = ShapeType.Point };
                FeatureType = FeatureType.Point;
                return;
            }

            IndexMode = true;
            Filename = fileName;

            Header = new ShapefileHeader(fileName);
            CoordinateType = CoordinateType.Regular;
            if (Header.ShapeType == ShapeType.PointM)
            {
                CoordinateType = CoordinateType.M;
            }
            if (Header.ShapeType == ShapeType.PointZ)
            {
                CoordinateType = CoordinateType.Z;
            }
            MyExtent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(fileName);
            FillPoints(fileName, progressHandler);
            ReadProjection();
        }

        /// <summary>
        /// Obtains a typed list of ShapefilePoint structures with double values associated with the various coordinates.
        /// </summary>
        /// <param name="fileName">A string fileName</param>
        /// <param name="progressHandler">A progress indicator</param>
        private void FillPoints(string fileName, IProgressHandler progressHandler)
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

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(fileName);

            // Get the basic header information.
            ShapefileHeader header = new ShapefileHeader(fileName);
            MyExtent = header.ToExtent();
            // Check to ensure that the fileName is the correct shape type
            if (header.ShapeType != ShapeType.Point && header.ShapeType != ShapeType.PointM
                && header.ShapeType != ShapeType.PointZ)
            {
                throw new ApplicationException(DataStrings.FileNotPoints_S.Replace("%S", fileName));
            }

            // This will set up a reader so that we can read values in huge chunks, which is much
            // faster than one value at a time.
            BufferedBinaryReader bbReader = new BufferedBinaryReader(fileName, progressHandler);

            if (bbReader.FileLength == 100)
            {
                bbReader.Close();
                // the file is empty so we are done reading
                return;
            }

            // Skip the shapefile header by skipping the first 100 bytes in the shapefile
            bbReader.Seek(100, SeekOrigin.Begin);
            int numShapes = shapeHeaders.Count;
            byte[] bigEndian = new byte[numShapes * 8];
            byte[] allCoords = new byte[numShapes * 16];
            bool isM = false;
            bool isZ = false;
            if (header.ShapeType == ShapeType.PointM || header.ShapeType == ShapeType.PointZ)
            {
                isM = true;
            }
            if (header.ShapeType == ShapeType.PointZ)
            {
                isZ = true;
            }
            byte[] allM = new byte[8];
            if (isM) allM = new byte[numShapes * 8];
            byte[] allZ = new byte[8];
            if (isZ) allZ = new byte[numShapes * 8];
            for (int shp = 0; shp < numShapes; shp++)
            {
                // Read from the index file because some deleted records
                // might still exist in the .shp file.
                long offset = (shapeHeaders[shp].ByteOffset);
                bbReader.Seek(offset, SeekOrigin.Begin);
                bbReader.Read(bigEndian, shp * 8, 8);
                bbReader.ReadInt32(); // Skip ShapeType.  Null shapes may break this.
                //bbReader.Seek(4, SeekOrigin.Current);
                bbReader.Read(allCoords, shp * 16, 16);
                if (isZ)
                {
                    bbReader.Read(allZ, shp * 8, 8);
                }
                if (isM)
                {
                    bbReader.Read(allM, shp * 8, 8);
                }
                ShapeRange shape = new ShapeRange(FeatureType.Point)
                                   {
                                       StartIndex = shp,
                                       ContentLength = 8,
                                       NumPoints = 1,
                                       NumParts = 1
                                   };
                ShapeIndices.Add(shape);
            }
            double[] vert = new double[2 * numShapes];
            Buffer.BlockCopy(allCoords, 0, vert, 0, numShapes * 16);
            Vertex = vert;
            if (isM)
            {
                double[] m = new double[numShapes];
                Buffer.BlockCopy(allM, 0, m, 0, numShapes * 8);
                M = m;
            }
            if (isZ)
            {
                double[] z = new double[numShapes];
                Buffer.BlockCopy(allZ, 0, z, 0, numShapes * 8);
                Z = z;
            }
            for (int shp = 0; shp < numShapes; shp++)
            {
                PartRange part = new PartRange(vert, shp, 0, FeatureType.Point);
                part.NumVertices = 1;
                ShapeRange shape = ShapeIndices[shp];
                shape.Parts.Add(part);
                shape.Extent = new Extent(new[] { vert[shp * 2], vert[shp * 2 + 1], vert[shp * 2], vert[shp * 2 + 1] });
            }

            bbReader.Dispose();
        }

        /// <inheritdoc />
        public override IFeature GetFeature(int index)
        {
            IFeature f = GetPoint(index);
            f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
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
            Filename = fileName;
            string dir = Path.GetDirectoryName(Path.GetFullPath(fileName));
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
            // Set Header.ShapeType before setting extent.
            Header.SetExtent(MyExtent);

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
            Stream shpStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None, 1000000);
            Stream shxStream =
                new FileStream(Header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 1000000);

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
                    Coordinate c = f.BasicGeometry.Coordinates[0];
                    shpStream.WriteBe(fid + 1);
                    shpStream.WriteBe(wordSize - 4);
                    shxStream.WriteBe(50 + fid * wordSize);
                    shxStream.WriteBe(wordSize - 4);
                    shpStream.WriteLe((int)Header.ShapeType);
                    if (Header.ShapeType == ShapeType.NullShape)
                    {
                        continue;
                    }
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
                    fid++;
                }
            }
            shpStream.Flush();
            shpStream.Close();
            shxStream.Flush();
            shxStream.Close();
            UpdateAttributes();
            SaveProjection();
        }
    }
}