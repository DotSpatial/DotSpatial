// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DotSpatial.Data;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Noding.Snapround;
using NetTopologySuite.Operation.Linemerge;
using NetTopologySuite.Operation.Polygonize;

namespace DotSpatial.Plugins.Contourer
{
    /// <summary>
    /// An extension for create contour from raster.
    /// </summary>
    public class Contour
    {
        #region Fields

        private static double noData;
        private static ContourType type;

        #endregion

        /// <summary>
        /// The contour type.
        /// </summary>
        public enum ContourType
        {
            /// <summary>
            /// Indicate that the countour is of the line type.
            /// </summary>
            Line,

            /// <summary>
            /// Indicates that the contour is a polygon.
            /// </summary>
            Polygon
        }

        #region Methods

        /// <summary>
        /// Creates levels between minimum and maximum.
        /// </summary>
        /// <param name="minContour">The minimum.</param>
        /// <param name="maxContour">The maximum.</param>
        /// <param name="every">The step width.</param>
        /// <returns>An array with the created levels.</returns>
        public static double[] CreateLevels(double minContour, double maxContour, double every)
        {
            int c = (int)((maxContour - minContour) / every) + 1;

            double[] levels = new double[c];

            for (int i = 0; i < c; i++)
            {
                levels[i] = minContour + (every * i);
            }

            return levels;
        }

        /// <summary>
        /// Creates minimum, maximum and step width from the given raster.
        /// </summary>
        /// <param name="r">The raster used for getting the values.</param>
        /// <param name="contourType">The contour type used for creation.</param>
        /// <param name="minContour">The output parameter that takes the minimum.</param>
        /// <param name="maxContour">The output parameter that takes the maximum.</param>
        /// <param name="every">The output parameter that takes the step width.</param>
        public static void CreateMinMaxEvery(Raster r, ContourType contourType, out double minContour, out double maxContour, out double every)
        {
            double min = r.Minimum;
            double max = r.Maximum;

            if (min == max)
            {
                min = Math.Floor(min);
                max = Math.Ceiling(max);
                if (min == max)
                {
                    max += 1;
                }
            }

            double dz = max - min;
            double order = Math.Pow(10, Math.Floor(Math.Log10(dz)));

            if (order == dz) order /= 10;
            if (dz / order < 2) order /= 10;

            minContour = Math.Floor(min / order) * order;
            maxContour = Math.Ceiling(max / order) * order;

            if (maxContour < max) maxContour += order;

            every = order;

            if (contourType == ContourType.Line)
            {
                minContour += every;
                maxContour -= every;

                if (maxContour <= minContour)
                {
                    maxContour = minContour + every;
                }
            }
        }

        /// <summary>
        /// Creates a featureset from the given raster.
        /// </summary>
        /// <param name="rst">Raster used for creation.</param>
        /// <param name="contourType">The contour type used for creation.</param>
        /// <param name="fieldName">Name of the field that gets added to the featureset to put the level values into.</param>
        /// <param name="levels">The levels to sort the features into.</param>
        /// <returns>The featureset that was created from the raster.</returns>
        public static FeatureSet Execute(Raster rst, ContourType contourType, string fieldName = "Value", double[] levels = null)
        {
            double[] lev = levels;
            noData = rst.NoDataValue;
            type = contourType;
            Raster iRst = RasterCheck(rst, lev);

            string field = fieldName ?? "Value";

            double[] x = new double[rst.NumColumns];
            double[] y = new double[rst.NumRows];

            for (int i = 0; i < rst.NumColumns; i++)
            {
                x[i] = rst.Extent.MinX + (rst.CellWidth * i) + (rst.CellWidth / 2);
            }

            for (int i = 0; i < rst.NumRows; i++)
            {
                y[i] = rst.Extent.MaxY - (rst.CellHeight * i) - (rst.CellHeight / 2);
            }

            FeatureSet fs = null;

            switch (type)
            {
                case ContourType.Line:
                    fs = new FeatureSet(FeatureType.Line);
                    fs.DataTable.Columns.Add(field, typeof(double));

                    if (levels != null)
                    {
                        for (int z = 0; z < levels.Length; z++)
                        {
                            IList<IGeometry> cont = GetContours(ref iRst, x, y, lev[z]);

                            foreach (var g in cont)
                            {
                                var f = (Feature)fs.AddFeature((ILineString)g);
                                f.DataRow[field] = lev[z];
                            }
                        }
                    }

                    break;

                case ContourType.Polygon:

                    fs = new FeatureSet(FeatureType.Polygon);
                    fs.DataTable.Columns.Add("Lev", typeof(int));
                    fs.DataTable.Columns.Add("Label", typeof(string));

                    Collection<IGeometry> contours = new Collection<IGeometry>();
                    if (levels != null)
                    {
                        for (int z = 0; z < levels.Length; z++)
                        {
                            IList<IGeometry> cont = GetContours(ref iRst, x, y, lev[z]);

                            foreach (var g in cont)
                            {
                                contours.Add(new LineString(g.Coordinates));
                            }
                        }

                        Coordinate[] boundary = new Coordinate[5];

                        boundary[0] = new Coordinate(x[0], y[0]);
                        boundary[1] = new Coordinate(x[0], y[rst.NumRows - 1]);
                        boundary[2] = new Coordinate(x[rst.NumColumns - 1], y[rst.NumRows - 1]);
                        boundary[3] = new Coordinate(x[rst.NumColumns - 1], y[0]);
                        boundary[4] = new Coordinate(x[0], y[0]);

                        contours.Add(new LineString(boundary));

                        Collection<IGeometry> nodedContours = new Collection<IGeometry>();
                        IPrecisionModel pm = new PrecisionModel(1000d);
                        GeometryNoder geomNoder = new GeometryNoder(pm);

                        foreach (var c in geomNoder.Node(contours))
                        {
                            nodedContours.Add(c);
                        }

                        Polygonizer polygonizer = new Polygonizer();
                        polygonizer.Add(nodedContours);

                        foreach (IPolygon p in polygonizer.GetPolygons().OfType<IPolygon>())
                        {
                            IPoint pnt = p.InteriorPoint;

                            int c = (int)((pnt.X - iRst.Extent.MinX) / iRst.CellWidth);
                            int r = (int)((iRst.Extent.MaxY - pnt.Y) / iRst.CellHeight);

                            double z = iRst.Value[r, c];

                            int cls = GetLevel(z, lev);
                            string label = "Undefined";

                            if (cls == -1) label = "< " + lev[0];
                            else if (cls == lev.Length) label = "> " + lev[lev.Length - 1];
                            else if (cls >= 0 & cls < lev.Length) label = lev[cls] + " - " + lev[cls + 1];

                            IFeature f = fs.AddFeature(p);
                            f.DataRow["Lev"] = cls;
                            f.DataRow["Label"] = label;
                        }
                    }

                    break;
            }

            return fs;
        }

        /// <summary>
        /// Gets the contours from the raster.
        /// </summary>
        /// <param name="rst">Raster to get the contours from.</param>
        /// <param name="x">The x values.</param>
        /// <param name="y">The y values.</param>
        /// <param name="zlev">Level to get the contours for.</param>
        /// <returns>The contours that were found.</returns>
        public static IList<IGeometry> GetContours(ref Raster rst, double[] x, double[] y, double zlev)
        {
            List<LineString> lsList = new List<LineString>();

            double[] xx = new double[3];
            double[] yy = new double[3];
            double[] zz = new double[3];

            for (int j = 0; j < rst.NumColumns - 1; j++)
            {
                bool jpari = (int)(j / 2.0) * 2 == j;

                for (int i = 0; i < rst.NumRows - 1; i++)
                {
                    bool ipari = (int)(i / 2.0) * 2 == i;

                    if (jpari == ipari)
                    {
                        xx[0] = x[j];
                        yy[0] = y[i];
                        zz[0] = rst.Value[i, j];

                        xx[1] = x[j];
                        yy[1] = y[i + 1];
                        zz[1] = rst.Value[i + 1, j];

                        xx[2] = x[j + 1];
                        yy[2] = y[i];
                        zz[2] = rst.Value[i, j + 1];

                        Coordinate[] c = Intersect(xx, yy, zz, zlev);
                        if (c != null)
                        {
                            lsList.Add(new LineString(c));
                        }

                        xx[0] = x[j + 1];
                        yy[0] = y[i];
                        zz[0] = rst.Value[i, j + 1];

                        xx[1] = x[j];
                        yy[1] = y[i + 1];
                        zz[1] = rst.Value[i + 1, j];

                        xx[2] = x[j + 1];
                        yy[2] = y[i + 1];
                        zz[2] = rst.Value[i + 1, j + 1];

                        Coordinate[] c1 = Intersect(xx, yy, zz, zlev);
                        if (c1 != null)
                        {
                            lsList.Add(new LineString(c1));
                        }
                    }
                    else
                    {
                        xx[0] = x[j];
                        yy[0] = y[i];
                        zz[0] = rst.Value[i, j];

                        xx[1] = x[j];
                        yy[1] = y[i + 1];
                        zz[1] = rst.Value[i + 1, j];

                        xx[2] = x[j + 1];
                        yy[2] = y[i + 1];
                        zz[2] = rst.Value[i + 1, j + 1];

                        Coordinate[] c = Intersect(xx, yy, zz, zlev);
                        if (c != null)
                        {
                            lsList.Add(new LineString(c));
                        }

                        xx[0] = x[j];
                        yy[0] = y[i];
                        zz[0] = rst.Value[i, j];

                        xx[1] = x[j + 1];
                        yy[1] = y[i + 1];
                        zz[1] = rst.Value[i + 1, j + 1];

                        xx[2] = x[j + 1];
                        yy[2] = y[i];
                        zz[2] = rst.Value[i, j + 1];

                        Coordinate[] c1 = Intersect(xx, yy, zz, zlev);
                        if (c1 != null)
                        {
                            lsList.Add(new LineString(c1));
                        }
                    }
                }
            }

            LineMerger lm = new LineMerger();

            lm.Add(lsList);

            IList<IGeometry> merged = lm.GetMergedLineStrings();

            return merged;
        }

        /// <summary>
        /// Gets the level that contains the given z value.
        /// </summary>
        /// <param name="z">Z value to look for.</param>
        /// <param name="lev">Levels that get searched.</param>
        /// <returns>-9999 if no fitting level was found, otherwise the level that was found.</returns>
        public static int GetLevel(double z, double[] lev)
        {
            if (z < lev[0]) return -1;
            if (z > lev[lev.Length - 1]) return lev.Length;

            for (int i = 0; i < lev.Length - 1; i++)
            {
                if (z >= lev[i] & z <= lev[i + 1])
                {
                    return i;
                }
            }

            return -9999;
        }

        /// <summary>
        /// Checks the raster.
        /// </summary>
        /// <param name="raster">Raster to check.</param>
        /// <param name="levels">Levels needed for checking.</param>
        /// <returns>The checked raster.</returns>
        public static Raster RasterCheck(Raster raster, double[] levels)
        {
            double eps = (raster.Maximum - raster.Minimum) / 1000;
            int n = levels.Length;
            Raster rst = raster;

            for (int i = 0; i <= rst.NumRows - 1; i++)
            {
                for (int j = 0; j <= rst.NumColumns - 1; j++)
                {
                    if (rst.Value[i, j] != rst.NoDataValue)
                    {
                        for (int l = 0; l < n; l++)
                        {
                            if (rst.Value[i, j] == levels[l])
                            {
                                rst.Value[i, j] += eps;
                            }
                        }
                    }
                }
            }

            return rst;
        }

        private static Coordinate[] Intersect(double[] xx, double[] yy, double[] zz, double zlevel)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            double zmin = Math.Min(zz[0], Math.Min(zz[1], zz[2]));
            double zmax = Math.Max(zz[0], Math.Max(zz[1], zz[2]));

            if (zlevel < zmin || zlevel > zmax) return null;
            if (zz.Count(x => x == noData) >= 1 && type == ContourType.Line)
            {
                return null;
            }

            for (int i = 0; i < 3; i++)
            {
                var i1 = i == 2 ? 0 : i + 1;

                zmin = Math.Min(zz[i], zz[i1]);
                zmax = Math.Max(zz[i], zz[i1]);

                if (zlevel > zmin && zlevel <= zmax)
                {
                    double dz = zz[i1] - zz[i];
                    {
                        double frac = (zlevel - zz[i]) / dz;

                        if (frac > 0)
                        {
                            double dx = xx[i1] - xx[i];
                            double dy = yy[i1] - yy[i];

                            Coordinate c = new Coordinate
                            {
                                X = xx[i] + (frac * dx),
                                Y = yy[i] + (frac * dy),
                                Z = zlevel
                            };

                            coordinates.Add(c);
                        }
                    }
                }
            }

            if (coordinates.Count >= 3)
            {
                throw new Exception("three intersection!");
            }

            if (coordinates.Count == 1)
            {
                throw new Exception("one intersection!");
            }

            return coordinates.ToArray();
        }

        #endregion
    }
}