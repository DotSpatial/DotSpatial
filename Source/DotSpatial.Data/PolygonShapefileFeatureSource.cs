// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoAPI.Geometries;
using NetTopologySuite.Algorithm;

namespace DotSpatial.Data
{
    /// <summary>
    /// This class is strictly the vector access code. This does not handle
    /// the attributes, which must be handled independently.
    /// </summary>
    public class PolygonShapefileFeatureSource : ShapefileFeatureSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonShapefileFeatureSource"/> class from the specified file.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        public PolygonShapefileFeatureSource(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonShapefileFeatureSource"/> class from the specified file and builds spatial index if requested.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        /// <param name="useSpatialIndexing">Indicates whether the spatial index should be build.</param>
        /// <param name="trackDeletedRows">Indicates whether deleted records should be tracked.</param>
        public PolygonShapefileFeatureSource(string fileName, bool useSpatialIndexing, bool trackDeletedRows)
            : base(fileName, useSpatialIndexing, trackDeletedRows)
        {
        }

        /// <inheritdoc />
        public override FeatureType FeatureType => FeatureType.Polygon;

        /// <inheritdoc />
        public override ShapeType ShapeType => ShapeType.Polygon;

        /// <inheritdoc />
        public override ShapeType ShapeTypeM => ShapeType.PolygonM;

        /// <inheritdoc />
        public override ShapeType ShapeTypeZ => ShapeType.PolygonZ;

        /// <inheritdoc />
        public override IShapeSource CreateShapeSource()
        {
            return new PolygonShapefileShapeSource(Filename, Quadtree, null);
        }

        /// <inheritdoc />
        public override void UpdateExtents()
        {
            UpdateExtents(new PolygonShapefileShapeSource(Filename));
        }

        /// <inheritdoc/>
        public override IFeatureSet Select(string filterExpression, Envelope envelope, ref int startIndex, int maxCount)
        {
            return Select(new PolygonShapefileShapeSource(Filename, Quadtree, null), filterExpression, envelope, ref startIndex, maxCount);
        }

        /// <inheritdoc/>
        public override void SearchAndModifyAttributes(Envelope envelope, int chunkSize, FeatureSourceRowEditEvent rowCallback)
        {
            SearchAndModifyAttributes(new PolygonShapefileShapeSource(Filename, Quadtree, null), envelope, chunkSize, rowCallback);
        }

        /// <inheritdoc/>
        protected override void AppendGeometry(ShapefileHeader header, IGeometry feature, int numFeatures)
        {
            FileInfo fi = new FileInfo(Filename);
            int offset = Convert.ToInt32(fi.Length / 2);

            FileStream shpStream = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.None, 10000);
            FileStream shxStream = new FileStream(header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 100);

            List<int> parts = new List<int>();

            List<Coordinate> points = new List<Coordinate>();
            int contentLength = 22;
            for (int iPart = 0; iPart < feature.NumGeometries; iPart++)
            {
                parts.Add(points.Count);
                IPolygon pg = feature.GetGeometryN(iPart) as IPolygon;
                if (pg == null) continue;
                var bl = pg.Shell;
                IEnumerable<Coordinate> coords = bl.Coordinates;

                if (CGAlgorithms.IsCCW(bl.Coordinates))
                {
                    // Exterior rings need to be clockwise
                    coords = coords.Reverse();
                }

                points.AddRange(coords);
                foreach (ILineString hole in pg.Holes)
                {
                    parts.Add(points.Count);
                    IEnumerable<Coordinate> holeCoords = hole.Coordinates;
                    if (!CGAlgorithms.IsCCW(hole.Coordinates))
                    {
                        // Interior rings need to be counter-clockwise
                        holeCoords = holeCoords.Reverse();
                    }

                    points.AddRange(holeCoords);
                }
            }

            contentLength += 2 * parts.Count;
            if (header.ShapeType == ShapeType.Polygon)
            {
                contentLength += points.Count * 8;
            }
            else if (header.ShapeType == ShapeType.PolygonM)
            {
                contentLength += 8; // mmin mmax
                contentLength += points.Count * 12; // x, y, m
            }
            else if (header.ShapeType == ShapeType.PolygonZ)
            {
                contentLength += 16; // mmin, mmax, zmin, zmax
                contentLength += points.Count * 16; // x, y, m, z
            }

            ////                                              Index File
            //                                                 ---------------------------------------------------------
            //                                                 Position     Value               Type        Number      Byte Order
            //                                                 ---------------------------------------------------------
            shxStream.WriteBe(offset);                         // Byte 0     Offset             Integer     1           Big
            shxStream.WriteBe(contentLength);                  // Byte 4    Content Length      Integer     1           Big
            shxStream.Flush();
            shxStream.Close();
            ////                                                 X Y Poly Lines
            //                                                 ---------------------------------------------------------
            //                                                 Position     Value               Type        Number      Byte Order
            //                                                 ---------------------------------------------------------
            shpStream.WriteBe(numFeatures);                    // Byte 0       Record Number       Integer     1           Big
            shpStream.WriteBe(contentLength);                  // Byte 4       Content Length      Integer     1           Big
            shpStream.WriteLe((int)header.ShapeType);          // Byte 8       Shape Type 3        Integer     1           Little
            if (header.ShapeType == ShapeType.NullShape)
            {
                return;
            }

            shpStream.WriteLe(feature.EnvelopeInternal.MinX);  // Byte 12      Xmin                Double      1           Little
            shpStream.WriteLe(feature.EnvelopeInternal.MinY);  // Byte 20      Ymin                Double      1           Little
            shpStream.WriteLe(feature.EnvelopeInternal.MaxX);  // Byte 28      Xmax                Double      1           Little
            shpStream.WriteLe(feature.EnvelopeInternal.MaxY);  // Byte 36      Ymax                Double      1           Little
            shpStream.WriteLe(parts.Count);                    // Byte 44      NumParts            Integer     1           Little
            shpStream.WriteLe(points.Count);                   // Byte 48      NumPoints           Integer     1           Little

            foreach (int iPart in parts)
            {
                shpStream.WriteLe(iPart);                      // Byte 52      Parts               Integer     NumParts    Little
            }

            double[] xyVals = new double[points.Count * 2];
            for (var i = 0; i < points.Count; i++)
            {
                xyVals[i * 2] = points[i].X;
                xyVals[i * 2 + 1] = points[i].Y;
            }

            shpStream.WriteLe(xyVals, 0, 2 * points.Count);

            if (header.ShapeType == ShapeType.PolygonZ)
            {
                shpStream.WriteLe(feature.EnvelopeInternal.Minimum.Z);
                shpStream.WriteLe(feature.EnvelopeInternal.Maximum.Z);
                double[] zVals = new double[points.Count];
                for (int ipoint = 0; ipoint < points.Count; ipoint++)
                {
                    zVals[ipoint] = points[ipoint].Z;
                }

                shpStream.WriteLe(zVals, 0, points.Count);
            }

            if (header.ShapeType == ShapeType.PolygonM || header.ShapeType == ShapeType.PolygonZ)
            {
                if (feature.EnvelopeInternal == null)
                {
                    shpStream.WriteLe(0.0);
                    shpStream.WriteLe(0.0);
                }
                else
                {
                    shpStream.WriteLe(feature.EnvelopeInternal.Minimum.M);
                    shpStream.WriteLe(feature.EnvelopeInternal.Maximum.M);
                }

                double[] mVals = new double[points.Count];
                for (int ipoint = 0; ipoint < points.Count; ipoint++)
                {
                    mVals[ipoint] = points[ipoint].M;
                }

                shpStream.WriteLe(mVals, 0, points.Count);
            }

            shpStream.Flush();
            shpStream.Close();
            offset += contentLength;
            Shapefile.WriteFileLength(Filename, offset + 4); // Add 4 for the record header
            Shapefile.WriteFileLength(header.ShxFilename, 50 + numFeatures * 4);
        }
    }
}