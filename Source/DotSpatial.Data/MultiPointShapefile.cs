// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where each features has multiple points.
    /// </summary>
    public class MultiPointShapefile : Shapefile
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPointShapefile"/> class for in-ram handling only.
        /// </summary>
        public MultiPointShapefile()
            : base(FeatureType.MultiPoint, ShapeType.MultiPoint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPointShapefile"/> class that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load.</param>
        public MultiPointShapefile(string fileName)
            : this()
        {
            Open(fileName, null);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the feature at the specified index offset.
        /// </summary>
        /// <param name="index">the integer index.</param>
        /// <returns>An IFeature created from the shape at the specified offset.</returns>
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
        /// Opens a shapefile.
        /// </summary>
        /// <param name="fileName">The string fileName of the point shapefile to load.</param>
        /// <param name="progressHandler">Any valid implementation of the DotSpatial.Data.IProgressHandler.</param>
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

        /// <inheritdoc />
        protected override StreamLengthPair PopulateShpAndShxStreams(Stream shpStream, Stream shxStream, bool indexed)
        {
            if (indexed) return LineShapefile.PopulateStreamsIndexed(shpStream, shxStream, this, ShapeType.MultiPointZ, ShapeType.MultiPointM, false);

            return LineShapefile.PopulateStreamsNotIndexed(shpStream, shxStream, this, AddPoints, ShapeType.MultiPointZ, ShapeType.MultiPointM, false);
        }

        /// <inheritdoc />
        protected override void SetHeaderShapeType()
        {
            if (CoordinateType == CoordinateType.Regular) Header.ShapeType = ShapeType.MultiPoint;
            else if (CoordinateType == CoordinateType.M) Header.ShapeType = ShapeType.MultiPointM;
            else if (CoordinateType == CoordinateType.Z) Header.ShapeType = ShapeType.MultiPointZ;
        }

        /// <summary>
        /// Adds the parts and points of the given feature to the given parts and points lists.
        /// </summary>
        /// <param name="parts">List of parts, where the features parts get added.</param>
        /// <param name="points">List of points, where the features points get added.</param>
        /// <param name="f">Feature, whose parts and points get added to the lists.</param>
        private static void AddPoints(List<int> parts, List<Coordinate> points, IFeature f)
        {
            for (int i = 0; i < f.Geometry.NumGeometries; i++)
            {
                points.AddRange(f.Geometry.GetGeometryN(i).Coordinates);
            }
        }

        // X Y MultiPoints
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

        // X Y M MultiPoints
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

        // X Y Z M MultiPoints
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

        /// <summary>
        /// Obtains a typed list of MultiPoint structures with double values associated with the various coordinates.
        /// </summary>
        /// <param name="fileName">Name of the file that gets loaded.</param>
        /// <param name="progressHandler">Progress handler.</param>
        private void FillPoints(string fileName, IProgressHandler progressHandler)
        {
            // Check whether file is empty or not all parameters are set correctly.
            if (!CanBeRead(fileName, this, ShapeType.MultiPoint, ShapeType.MultiPointM, ShapeType.MultiPointZ)) return;

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(fileName);
            int numShapes = shapeHeaders.Count;

            bool isM = Header.ShapeType == ShapeType.MultiPointZ || Header.ShapeType == ShapeType.MultiPointM;
            bool isZ = Header.ShapeType == ShapeType.MultiPointZ;

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

                    // Read from the index file because some deleted records might still exist in the .shp file.
                    long offset = shapeHeaders[shp].ByteOffset;
                    reader.Seek(offset, SeekOrigin.Begin);

                    var shape = new ShapeRange(FeatureType.MultiPoint, CoordinateType)
                    {
                        RecordNumber = reader.ReadInt32(Endian.BigEndian),
                        ContentLength = reader.ReadInt32(Endian.BigEndian),
                        ShapeType = (ShapeType)reader.ReadInt32(),
                        StartIndex = totalPointsCount,
                        NumParts = 1
                    };
                    Debug.Assert(shape.RecordNumber == shp + 1, "shape.RecordNumber == shp + 1");

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

                int mArrayInd = 0, zArrayInd = 0;
                double[] mArray = null, zArray = null;
                if (isM) mArray = new double[totalPointsCount];
                if (isZ) zArray = new double[totalPointsCount];

                int partsOffset = 0;
                for (int shp = 0; shp < numShapes; shp++)
                {
                    progressMeter.CurrentPercent = (int)(50 + shp * 50.0 / numShapes);

                    var shape = shapeIndices[shp];
                    if (shape.ShapeType == ShapeType.NullShape) continue;

                    reader.Seek(shapeHeaders[shp].ByteOffset, SeekOrigin.Begin);
                    reader.Seek(3 * 4 + 32 + 4, SeekOrigin.Current); // Skip first bytes (Record Number, Content Length, Shapetype + BoundingBox + NumPoints)

                    // Read points
                    var pointsBytes = reader.ReadBytes(8 * 2 * shape.NumPoints); // Numpoints * Point (X(8) + Y(8))
                    Buffer.BlockCopy(pointsBytes, 0, vert, vertInd, pointsBytes.Length);
                    vertInd += 8 * 2 * shape.NumPoints;

                    // Fill parts
                    shape.Parts.Capacity = shape.NumParts;
                    for (int p = 0; p < shape.NumParts; p++)
                    {
                        int endIndex = shape.NumPoints + shape.StartIndex;
                        int startIndex = parts[partsOffset + p] + shape.StartIndex;
                        if (p < shape.NumParts - 1)
                        {
                            endIndex = parts[partsOffset + p + 1] + shape.StartIndex;
                        }

                        int count = endIndex - startIndex;
                        var part = new PartRange(vert, shape.StartIndex, parts[partsOffset + p], FeatureType.MultiPoint)
                        {
                            NumVertices = count
                        };
                        shape.Parts.Add(part);
                    }

                    partsOffset += shape.NumParts;

                    // Fill M and Z arrays
                    switch (Header.ShapeType)
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

            progressMeter.Reset();
        }

        #endregion
    }
}