// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where the data type is point.
    /// </summary>
    public class PointShapefile : Shapefile
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointShapefile"/> class for in-ram handling only.
        /// </summary>
        public PointShapefile()
            : base(FeatureType.Point, ShapeType.Point)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointShapefile"/> class that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load.</param>
        public PointShapefile(string fileName)
            : this()
        {
            Open(fileName, null);
        }

        #endregion

        #region Methods

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
        /// Opens a shapefile.
        /// </summary>
        /// <param name="fileName">The string fileName of the point shapefile to load.</param>
        /// <param name="progressHandler">Any valid implementation of the DotSpatial.Data.IProgressHandler.</param>
        public void Open(string fileName, IProgressHandler progressHandler)
        {
            if (!File.Exists(fileName)) return;

            Filename = fileName;
            IndexMode = true;
            Header = new ShapefileHeader(Filename);

            CoordinateType = Header.ShapeType switch
            {
                ShapeType.PointM => CoordinateType.M,
                ShapeType.PointZ => CoordinateType.Z,
                _ => CoordinateType.Regular,
            };
            Extent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(Filename);

            FillPoints(Filename, progressHandler);
            ReadProjection();
        }

        /// <inheritdoc />
        protected override StreamLengthPair PopulateShpAndShxStreams(Stream shpStream, Stream shxStream, bool indexed)
        {
            if (indexed) return PopulateShpAndShxStreamsIndexed(shpStream, shxStream);

            return PopulateShpAndShxStreamsNotIndexed(shpStream, shxStream);
        }

        /// <inheritdoc />
        protected override void SetHeaderShapeType()
        {
            if (CoordinateType == CoordinateType.Regular) Header.ShapeType = ShapeType.Point;
            else if (CoordinateType == CoordinateType.M) Header.ShapeType = ShapeType.PointM;
            else if (CoordinateType == CoordinateType.Z) Header.ShapeType = ShapeType.PointZ;
        }

        /// <summary>
        /// Calculates the ContentLength that is needed to save a shape with the given number of parts and points.
        /// </summary>
        /// <param name="shapeType">The shape type.</param>
        /// <returns>ContentLength that is needed to save a shape with the given number of parts and points.</returns>
        private static int GetContentLength(ShapeType shapeType)
        {
            int contentLength = 2; // Shape Type

            switch (shapeType)
            {
                case ShapeType.Point:
                    contentLength += 8; // x, y
                    break;
                case ShapeType.PointM:
                    contentLength += 12; // x, y, m
                    break;
                case ShapeType.PointZ:
                    contentLength += 16; // x, y, m, z
                    break;
            }

            return contentLength;
        }

        // X Y Points: Total Length = 28 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 1        Integer     1           Little
        // Byte 12      X                   Double      1           Little
        // Byte 20      Y                   Double      1           Little

        // X Y M Points: Total Length = 36 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 21       Integer     1           Little
        // Byte 12      X                   Double      1           Little
        // Byte 20      Y                   Double      1           Little
        // Byte 28      M                   Double      1           Little

        // X Y Z M Points: Total Length = 44 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 11       Integer     1           Little
        // Byte 12      X                   Double      1           Little
        // Byte 20      Y                   Double      1           Little
        // Byte 28      Z                   Double      1           Little
        // Byte 36      M                   Double      1           Little

        /// <summary>
        /// Obtains a typed list of ShapefilePoint structures with double values associated with the various coordinates.
        /// </summary>
        /// <param name="fileName">A string fileName.</param>
        /// <param name="progressHandler">Progress handler.</param>
        private void FillPoints(string fileName, IProgressHandler progressHandler)
        {
            if (!CanBeRead(fileName, this, ShapeType.Point, ShapeType.PointM, ShapeType.PointZ)) return;

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(fileName);
            var numShapes = shapeHeaders.Count;
            var shapeIndices = new List<ShapeRange>(numShapes);
            int totalPointsCount = 0;

            var progressMeter = new ProgressMeter(progressHandler, "Reading from " + Path.GetFileName(fileName))
            {
                StepPercent = 5
            };
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                for (var shp = 0; shp < numShapes; shp++)
                {
                    progressMeter.CurrentPercent = (int)(shp * 50.0 / numShapes);

                    reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);

                    var shape = new ShapeRange(FeatureType.Point, CoordinateType)
                    {
                        RecordNumber = reader.ReadInt32(Endian.BigEndian),
                        ContentLength = reader.ReadInt32(Endian.BigEndian),
                        StartIndex = totalPointsCount,
                        ShapeType = (ShapeType)reader.ReadInt32()
                    };

                    Debug.Assert(shape.RecordNumber == shp + 1, "The record number should equal " + shp + 1);
                    Debug.Assert(shape.ContentLength == shapeHeaders[shp].ContentLength, "The shapes content length should equals the shapeHeaders content length.");

                    if (shape.ShapeType == ShapeType.NullShape)
                    {
                        shape.NumPoints = 0;
                        shape.NumParts = 0;
                    }
                    else
                    {
                        totalPointsCount += 1;
                        shape.NumPoints = 1;
                        shape.NumParts = 1;
                    }

                    shapeIndices.Add(shape);
                }

                double[] m = null;
                double[] z = null;
                var vert = new double[2 * totalPointsCount]; // X,Y

                if (Header.ShapeType == ShapeType.PointM || Header.ShapeType == ShapeType.PointZ)
                {
                    m = new double[totalPointsCount];
                }

                if (Header.ShapeType == ShapeType.PointZ)
                {
                    z = new double[totalPointsCount];
                }

                int i = 0;
                for (var shp = 0; shp < numShapes; shp++)
                {
                    progressMeter.CurrentPercent = (int)(50 + (shp * 50.0 / numShapes));

                    var shape = shapeIndices[shp];
                    if (shape.ShapeType == ShapeType.NullShape) continue;

                    reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);
                    reader.Seek(3 * 4, SeekOrigin.Current); // Skip first bytes (Record Number, Content Length, Shapetype)

                    // Read X
                    var ind = 4;
                    vert[i * 2] = reader.ReadDouble();
                    ind += 8;

                    // Read Y
                    vert[(i * 2) + 1] = reader.ReadDouble();
                    ind += 8;

                    // Read Z
                    if (z != null)
                    {
                        z[i] = reader.ReadDouble();
                        ind += 8;
                    }

                    // Read M
                    if (m != null)
                    {
                        if (shapeHeaders[shp].ByteLength <= ind)
                        {
                            m[i] = double.MinValue;
                        }
                        else
                        {
                            m[i] = reader.ReadDouble();
                        }
                    }

                    var part = new PartRange(vert, shape.StartIndex, 0, FeatureType.Point)
                    {
                        NumVertices = 1
                    };
                    shape.Parts.Add(part);
                    shape.Extent = new Extent(new[] { vert[i * 2], vert[(i * 2) + 1], vert[i * 2], vert[(i * 2) + 1] });
                    i++;
                }

                Vertex = vert;
                M = m;
                Z = z;
                ShapeIndices = shapeIndices;
            }

            progressMeter.Reset();
        }

        /// <summary>
        /// Populates the given streams for the shp and shx file when in IndexMode.
        /// </summary>
        /// <param name="shpStream">Stream that is used to write the shp file.</param>
        /// <param name="shxStream">Stream that is used to write the shx file.</param>
        /// <returns>The lengths of the streams in bytes.</returns>
        private StreamLengthPair PopulateShpAndShxStreamsIndexed(Stream shpStream, Stream shxStream)
        {
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words

            foreach (ShapeRange shape in ShapeIndices)
            {
                int contentLength = shape.ShapeType == ShapeType.NullShape ? 2 : GetContentLength(Header.ShapeType);

                shxStream.WriteBe(offset);
                shxStream.WriteBe(contentLength);

                shpStream.WriteBe(fid + 1);
                shpStream.WriteBe(contentLength);
                if (shape.ShapeType == ShapeType.NullShape)
                {
                    shpStream.WriteLe((int)ShapeType.NullShape); // Byte 8       Shape Type 3        Integer     1           Little
                }
                else
                {
                    shpStream.WriteLe((int)Header.ShapeType);
                    shpStream.WriteLe(Vertex[shape.StartIndex * 2]);
                    shpStream.WriteLe(Vertex[(shape.StartIndex * 2) + 1]);
                    if (Z != null) shpStream.WriteLe(Z[shape.StartIndex]);
                    if (M != null) shpStream.WriteLe(M[shape.StartIndex]);
                }

                fid++;
                offset += 4; // header bytes
                offset += contentLength; // adding the content length from each loop calculates the word offset
            }

            return new StreamLengthPair
            {
                ShpLength = offset,
                ShxLength = 50 + (fid * 4)
            };
        }

        /// <summary>
        /// Populates the given streams for the shp and shx file when not in IndexMode.
        /// </summary>
        /// <param name="shpStream">Stream that is used to write the shp file.</param>
        /// <param name="shxStream">Stream that is used to write the shx file.</param>
        /// <returns>The lengths of the streams in bytes.</returns>
        private StreamLengthPair PopulateShpAndShxStreamsNotIndexed(Stream shpStream, Stream shxStream)
        {
            var progressMeter = new ProgressMeter(ProgressHandler, "Saving (Not Indexed)...", Features.Count);

            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words

            foreach (IFeature f in Features)
            {
                bool isNullShape = false;
                int contentLength;

                if (f.Geometry.IsEmpty)
                {
                    contentLength = 2;
                    isNullShape = true;
                }
                else
                {
                    contentLength = GetContentLength(Header.ShapeType);
                }

                shxStream.WriteBe(offset);
                shxStream.WriteBe(contentLength);

                shpStream.WriteBe(fid + 1);
                shpStream.WriteBe(contentLength);

                if (isNullShape)
                {
                    shpStream.WriteLe((int)ShapeType.NullShape); // Byte 8   Shape Type 0   Integer     1           Little
                }
                else
                {
                    shpStream.WriteLe((int)Header.ShapeType); // Byte 8   Shape Type    Integer     1           Little

                    Coordinate c = f.Geometry.Coordinates[0];
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

                progressMeter.CurrentValue = fid;
                fid++;
                offset += 4; // header bytes
                offset += contentLength; // adding the content length from each loop calculates the word offset
            }

            progressMeter.Reset();

            return new StreamLengthPair
            {
                ShpLength = offset,
                ShxLength = 50 + (fid * 4)
            };
        }

        #endregion
    }
}