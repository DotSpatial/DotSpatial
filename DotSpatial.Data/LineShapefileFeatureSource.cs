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
// The Initial Developer of this Original Code is Kyle Ellison. Created 12/01/2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// |-----------------|----------|---------------------------------------------------------------------
// |      Name       |  Date    |                        Comments
// |-----------------|----------|----------------------------------------------------------------------
// | Kyle Ellison    |12/22/2010| Made FeatureType and ShapeType properties public
// |-----------------|----------|----------------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// This class is strictly the vector access code.  This does not handle
    /// the attributes, which must be handled independantly.
    /// </summary>
    public class LineShapefileFeatureSource : ShapefileFeatureSource
    {
        /// <summary>
        /// Sets the fileName and creates a new LineShapefileFeatureSource for the specified file.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        public LineShapefileFeatureSource(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Sets the fileName and creates a new LineShapefileFeatureSource for the specified file (and builds spatial index if requested)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="useSpatialIndexing"></param>
        /// <param name="trackDeletedRows"></param>
        public LineShapefileFeatureSource(string fileName, bool useSpatialIndexing, bool trackDeletedRows)
            : base(fileName, useSpatialIndexing, trackDeletedRows)
        {
        }

        /// <inheritdocs/>
        public override FeatureType FeatureType
        {
            get { return FeatureType.Line; }
        }

        /// <inheritdocs/>
        public override ShapeType ShapeType
        {
            get { return ShapeType.PolyLine; }
        }

        /// <inheritdocs/>
        public override ShapeType ShapeTypeM
        {
            get { return ShapeType.PolyLineM; }
        }

        /// <inheritdocs/>
        public override ShapeType ShapeTypeZ
        {
            get { return ShapeType.PolyLineZ; }
        }

        /// <inheritdocs/>
        protected override void AppendBasicGeometry(ShapefileHeader header, IBasicGeometry feature, int numFeatures)
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
                IBasicLineString pg = feature.GetBasicGeometryN(iPart) as IBasicLineString;
                if (pg == null) continue;
                points.AddRange(pg.Coordinates);
            }
            contentLength += 2 * parts.Count;
            if (header.ShapeType == ShapeType.PolyLine)
            {
                contentLength += points.Count * 8;
            }
            if (header.ShapeType == ShapeType.PolyLineM)
            {
                contentLength += 8; // mmin mmax
                contentLength += points.Count * 12; // x, y, m
            }
            if (header.ShapeType == ShapeType.PolyLineZ)
            {
                contentLength += 16; // mmin, mmax, zmin, zmax
                contentLength += points.Count * 16; // x, y, m, z
            }

            //                                              Index File
            //                                              ---------------------------------------------------------
            //                                              Position     Value               Type        Number      Byte Order
            //                                              ---------------------------------------------------------
            shxStream.WriteBe(offset);                      // Byte 0     Offset             Integer     1           Big
            shxStream.WriteBe(contentLength);               // Byte 4    Content Length      Integer     1           Big
            shxStream.Flush();
            shxStream.Close();
            //                                              X Y Poly Lines
            //                                              ---------------------------------------------------------
            //                                              Position     Value               Type        Number      Byte Order
            //                                              ---------------------------------------------------------
            shpStream.WriteBe(numFeatures);                 // Byte 0       Record Number       Integer     1           Big
            shpStream.WriteBe(contentLength);               // Byte 4       Content Length      Integer     1           Big
            shpStream.WriteLe((int)header.ShapeType);       // Byte 8       Shape Type 3        Integer     1           Little
            if (header.ShapeType == ShapeType.NullShape)
            {
                return;
            }

            shpStream.WriteLe(feature.Envelope.Minimum.X);             // Byte 12      Xmin                Double      1           Little
            shpStream.WriteLe(feature.Envelope.Minimum.Y);             // Byte 20      Ymin                Double      1           Little
            shpStream.WriteLe(feature.Envelope.Maximum.X);             // Byte 28      Xmax                Double      1           Little
            shpStream.WriteLe(feature.Envelope.Maximum.Y);             // Byte 36      Ymax                Double      1           Little
            shpStream.WriteLe(parts.Count);                            // Byte 44      NumParts            Integer     1           Little
            shpStream.WriteLe(points.Count);                           // Byte 48      NumPoints           Integer     1           Little
            // Byte 52      Parts               Integer     NumParts    Little
            foreach (int iPart in parts)
            {
                shpStream.WriteLe(iPart);
            }
            double[] xyVals = new double[points.Count * 2];

            int i = 0;
            foreach (Coordinate coord in points)
            {
                xyVals[i * 2] = coord.X;
                xyVals[i * 2 + 1] = coord.Y;
                i++;
            }
            shpStream.WriteLe(xyVals, 0, 2 * points.Count);

            if (header.ShapeType == ShapeType.PolyLineZ)
            {
                shpStream.WriteLe(feature.Envelope.Minimum.Z);
                shpStream.WriteLe(feature.Envelope.Maximum.Z);
                double[] zVals = new double[points.Count];
                for (int ipoint = 0; ipoint < points.Count; ipoint++)
                {
                    zVals[ipoint] = points[ipoint].Z;
                }
                shpStream.WriteLe(zVals, 0, points.Count);
            }

            if (header.ShapeType == ShapeType.PolyLineM || header.ShapeType == ShapeType.PolyLineZ)
            {
                if (feature.Envelope == null)
                {
                    shpStream.WriteLe(0.0);
                    shpStream.WriteLe(0.0);
                }
                else
                {
                    shpStream.WriteLe(feature.Envelope.Minimum.M);
                    shpStream.WriteLe(feature.Envelope.Maximum.M);
                }

                double[] mVals = new double[points.Count];
                for (int ipoint = 0; ipoint < points.Count; i++)
                {
                    mVals[ipoint] = points[ipoint].M;
                    ipoint++;
                }
                shpStream.WriteLe(mVals, 0, points.Count);
            }
            shpStream.Flush();
            shpStream.Close();
            offset += contentLength;
            Shapefile.WriteFileLength(Filename, offset + 4); // Add 4 for the record header
            Shapefile.WriteFileLength(header.ShxFilename, 50 + numFeatures * 4);
        }

        /// <inheritdocs/>
        public override IShapeSource CreateShapeSource()
        {
            return new LineShapefileShapeSource(Filename, Quadtree, null);
        }

        /// <inheritdocs/>
        public override void UpdateExtents()
        {
            UpdateExtents(new LineShapefileShapeSource(Filename));
        }

        /// <inheritdoc/>
        public override IFeatureSet Select(string filterExpression, IEnvelope envelope, ref int startIndex, int maxCount)
        {
            return Select(new LineShapefileShapeSource(Filename, Quadtree, null), filterExpression, envelope, ref startIndex,
                          maxCount);
        }

        /// <inheritdoc/>
        public override void SearchAndModifyAttributes(IEnvelope envelope, int chunkSize, FeatureSourceRowEditEvent rowCallback)
        {
            SearchAndModifyAttributes(new LineShapefileShapeSource(Filename, Quadtree, null), envelope, chunkSize, rowCallback);
        }
    }
}