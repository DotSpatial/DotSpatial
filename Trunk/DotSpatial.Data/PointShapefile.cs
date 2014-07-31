// *******************************************************************************************************
// Product: DotSpatial.Data.PointShapefile.cs
// Description: A shapefile class that handles the special case where the data type is point.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Max Miroshnikov    |  07/2014           | Created.
// *******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                case ShapeType.Point:
                    CoordinateType = CoordinateType.Regular;
                    break;
                case ShapeType.PointM:
                    CoordinateType = CoordinateType.M;
                    break;
                case ShapeType.PointZ:
                    CoordinateType = CoordinateType.Z;
                    break;
                default:
                    throw new Exception("Unsupported ShapeType");
            }

            Extent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(fileName);

            FillPoints(fileName, progressHandler);
            ReadProjection();
        }

        private List<ShapeHeader> _shapeHeaders;
        private List<ShapeHeader> ShapeHeaders
        {
            get
            {
                if (_shapeHeaders != null) return _shapeHeaders;
                _shapeHeaders = ReadIndexFile(Filename);
                return _shapeHeaders;
            }
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

            // Get the basic header information.
            var header = Header;
            Extent = Header.ToExtent();
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
            var progressMeter = new ProgressMeter(progressHandler, "Reading from " + Path.GetFileName(fileName))
            {
                StepPercent = 5
            };
            var numShapes = ShapeHeaders.Count;
            using (var reader = new FileStream(fileName, FileMode.Open))
            {
                for (var shp = 0; shp < numShapes; shp++)
                {
                    progressMeter.CurrentPercent = (int)(shp * 100.0 / numShapes);
                    var shape = ReadShape(shp, reader);
                    ShapeIndices.Add(shape.Range);
                }
            }
            progressMeter.Reset();
        }

        public override Shape GetShape(int index, bool getAttributes)
        {
            var sh = base.GetShape(index, getAttributes);
            if (sh != null) return sh;
            
            using (var reader = new FileStream(Filename, FileMode.Open))
                sh = ReadShape(index, reader);
            if (getAttributes)
            {
                sh.Attributes = AttributesPopulated
                    ? DataTable.Rows[index].ItemArray
                    : Attributes.SupplyPageOfData(index, 1).Rows[0].ItemArray;
            }
            return sh;
        }

        /// <inheritdoc />
        public override IFeature GetFeature(int index)
        {
            var f = base.GetFeature(index);
            if (f != null) return f;

            var shape = GetShape(index, false);
            var coordinate = new Coordinate(shape.Vertices);
            if (shape.M != null)
                coordinate.M = shape.M[0];
            if (shape.Z != null)
                coordinate.Z = shape.Z[0];

            var p = GetFeatureGeometryFactory().CreatePoint(coordinate);
            f = new Feature(p)
            {
                ParentFeatureSet = this,
                ShapeIndex = ShapeIndices[index],
                DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0],
            };
            return f;
        }

        private Shape ReadShape(int shp, Stream reader)
        {
            var pointsNum = 0;
            double xmin = 0, ymin = 0, xmax = 0, ymax = 0, zmin = 0, zmax = 0, mmin = 0, mmax = 0;
            double[] vert = null, m = null, z = null;

            var shapeHeaders = ShapeHeaders;
            reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);

            var recordNumber = reader.ReadInt32(Endian.BigEndian);
            Debug.Assert(recordNumber == shp + 1);
            var contentLen = reader.ReadInt32(Endian.BigEndian);
            Debug.Assert(contentLen == shapeHeaders[shp].ContentLength);

            var shapeType = (ShapeType)reader.ReadInt32();
            if (shapeType == ShapeType.NullShape) goto fin;

            var byteLen = 4; // from shapeType read

            // Initialize points arrays
            pointsNum = 1;
            vert = new double[2 * pointsNum];
            if (Header.ShapeType == ShapeType.PointM || Header.ShapeType == ShapeType.PointZ)
            {
                m = Enumerable.Repeat(double.MinValue, pointsNum).ToArray();
            }
            if (Header.ShapeType == ShapeType.PointZ)
            {
                z = new double[1];
            }

            // Read X, Y
            vert[0] = reader.ReadDouble(); byteLen += 8;
            vert[1] = reader.ReadDouble(); byteLen += 8;
            xmin = xmax = vert[0];
            ymin = ymax = vert[1];

            // Read Z
            if (z != null)
            {
                z[0] = reader.ReadDouble(); byteLen += 8;
                zmin = zmax = z[0];
            }

            // Read M
            if (m != null)
            {
                if (byteLen + 8 == shapeHeaders[shp].ByteLength) // +8 means read one double
                {
                    m[0] = reader.ReadDouble(); byteLen += 8;
                    mmin = mmax = m[0];
                }
            }

        fin:
            var shr = new ShapeRange(FeatureType.Point)
            {
                RecordNumber = recordNumber,
                StartIndex = 0,
                ContentLength = shapeHeaders[shp].ContentLength,
                NumPoints = pointsNum,
                Extent = new Extent(xmin, ymin, xmax, ymax),
                ShapeType = shapeType,
            };
            if (vert != null)
            {
                shr.Parts.Add(new PartRange(vert, 0, 0, FeatureType.Point) {NumVertices = 1});
            }
            return new Shape(FeatureType)
            {
                Range = shr,
                M = m,
                Z = z,
                Vertices = vert,
                MaxM = mmax,
                MinM = mmin,
                MaxZ = zmax,
                MinZ = zmin
            };
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

            if (IndexMode)
            {
                if (Filename == fileName)
                {
                    // files already there. Nothing to do.
                }
                else
                {
                    // In Index mode shapes are not loaded into memory, so we can just copy source .shp and .shx files to the new location
                    File.Copy(Filename, fileName);
                    var shx = Path.ChangeExtension(Filename, ".shx");
                    if (File.Exists(shx)) File.Copy(shx, Path.ChangeExtension(fileName, ".shx"));
                }
            }
            else
            {
                // Set Header.ShapeType before setting extent.
                // wordSize is the length of the byte representation in 16 bit words of a single shape, including header.
                int wordSize;
                switch (CoordinateType)
                {
                    case CoordinateType.Regular:
                        Header.ShapeType = ShapeType.Point;
                        wordSize = 14; // 3 int(2) and 2 double(4) = X,Y
                        break;
                    case CoordinateType.M:
                        Header.ShapeType = ShapeType.PointM;
                        wordSize = 18; // 3 int(2), 3 double(4) = X,Y,M
                        break;
                    case CoordinateType.Z:
                        Header.ShapeType = ShapeType.PointZ;
                        wordSize = 22; // 3 int(2), 4 double (4) = X,Y,Z,M
                        break;
                    default:
                        throw new Exception("Unsupported CoordinateType");
                }

                // Calculate total .shp file length
                var totalOffset = 50;
                for (var shp = 0; shp < Count; shp++)
                {
                    var f = GetFeature(shp);
                    var shpt = f.ShapeType.GetValueOrDefault(Header.ShapeType);
                    totalOffset += shpt == ShapeType.NullShape ? 6 : wordSize;
                }

                // Save headers for .shp and .shx files
                InvalidateEnvelope();
                Header.SetExtent(Extent);
                Header.ShxLength = 50 + Count * 4;
                Header.FileLength = totalOffset;
                Header.SaveAs(fileName);

                // Reset shapeheaders
                _shapeHeaders = null;

                // Append data into .shp and .shx
                int offset = 50;
                using (var shpStream = new FileStream(Header.Filename, FileMode.Append))
                using (var shxStream = new FileStream(Header.ShxFilename, FileMode.Append))
                {
                    for (var shp = 0; shp < Count; shp++)
                    {
                        shpStream.WriteBe(shp + 1);
                        var feature = GetFeature(shp);
                        var shpt = feature.ShapeType.GetValueOrDefault(Header.ShapeType);
                        var contentLen = shpt == ShapeType.NullShape ? 2 : wordSize - 4;

                        shpStream.WriteBe(contentLen);
                        shpStream.WriteLe((int)shpt);
                        if (shpt != ShapeType.NullShape)
                        {
                            var coordinate = feature.Coordinates[0];
                            shpStream.WriteLe(coordinate.X);
                            shpStream.WriteLe(coordinate.Y);
                            if (shpt == ShapeType.PointZ)
                                shpStream.WriteLe(coordinate.Z);
                            if (shpt == ShapeType.PointZ || shpt == ShapeType.PointM)
                                shpStream.WriteLe(coordinate.M);
                        }

                        shxStream.WriteBe(offset);
                        shxStream.WriteBe(contentLen);
                        offset += shpt == ShapeType.NullShape ? 6 : wordSize;
                    }
                }
            }

            // Update filename
            Filename = fileName;

            // Save .dbf and .prj files
            UpdateAttributes();
            SaveProjection();
        }
    }
}