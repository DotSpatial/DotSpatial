// ********************************************************************************************************
// Product Name: Conturer
// Description:  An extension for create contour from raste
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Tomaso Tonelli in memory of his mother in 2012.
// for questions/ info tomaso.tonelli@gmail.com
// ********************************************************************************************************

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
    public class Contour
    {
        public enum ContourType
        {
            Line,
            Polygon
        }

        private static ContourType _type;
        public static FeatureSet Execute(Raster rst, ContourType contourType, string fieldName = "Value", double[] levels = null)
        {
            double[] lev = levels;
            _noData = rst.NoDataValue;
            _type = contourType;
            Raster iRst = RasterCheck(rst, lev);

            string field = fieldName ?? "Value";

            double[] x = new double[rst.NumColumns];
            double[] y = new double[rst.NumRows];

            for (int i = 0; i < rst.NumColumns; i++)
            {
                x[i] = rst.Extent.MinX + rst.CellWidth * i + rst.CellWidth / 2;
            }

            for (int i = 0; i < rst.NumRows; i++)
            {
                y[i] = rst.Extent.MaxY - rst.CellHeight * i - rst.CellHeight / 2;
            }

            FeatureSet fs = null;

            switch (_type)
            {
                case ContourType.Line:
                    {
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

                    }
                    break;

                case ContourType.Polygon:
                    {
                        fs = new FeatureSet(FeatureType.Polygon);

                        fs.DataTable.Columns.Add("Lev", typeof(int));
                        fs.DataTable.Columns.Add("Label", typeof(string));

                        Collection<IGeometry> contours = new Collection<IGeometry>();
                        if (levels != null)
                        {
                            for (int z = 0; z < levels.Count(); z++)
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

                            foreach (IPolygon p in polygonizer.GetPolygons())
                            {
                                IPoint pnt = p.InteriorPoint;

                                int c = (int)((pnt.X - iRst.Extent.MinX) / iRst.CellWidth);
                                int r = (int)((iRst.Extent.MaxY - pnt.Y) / iRst.CellHeight);

                                double z = iRst.Value[r, c];

                                int cls = GetLevel(z, lev);
                                string label = "Undefined";

                                if (cls == -1) label = "< " + lev[0];
                                else if (cls == lev.Count()) label = "> " + lev[lev.Count() - 1];
                                else if (cls >= 0 & cls < lev.Count()) label = lev[cls] + " - " + lev[cls + 1];

                                IFeature f = fs.AddFeature(p);
                                f.DataRow["Lev"] = cls;
                                f.DataRow["Label"] = label;
                            }
                        }
                    }
                    break;
            }

            return fs;
        }

        public static int GetLevel(double z, double[] lev)
        {
            if (z < lev[0]) return -1;
            if (z > lev[lev.Count() - 1]) return lev.Count();

            for (int i = 0; i < lev.Count() - 1; i++)
            {
                if (z >= lev[i] & z <= lev[i + 1])
                {
                    return i;
                }
            }

            return -9999;
        }



        public static void CreateMinMaxEvery(Raster r, ContourType type, out double minContour, out double maxContour, out double every)
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

            if (type == ContourType.Line)
            {
                minContour += every;
                maxContour -= every;

                if (maxContour <= minContour)
                {
                    maxContour = minContour + every;
                }
            }
        }

        public static double[] CreateLevels(double minContour, double maxContour, double every)
        {
            int c = (int)((maxContour - minContour) / every) + 1;

            double[] levels = new double[c];

            for (int i = 0; i < c; i++)
            {
                levels[i] = minContour + every * i;
            }

            return levels;
        }

        public static Raster RasterCheck(Raster raster, double[] levels)
        {
            double eps = (raster.Maximum - raster.Minimum) / 1000;
            int n = levels.Count();

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

        public static IList<IGeometry> GetContours(ref Raster rst, double[] x, double[] y, double zlev)
        {
            List<LineString> lsList = new List<LineString>();

            bool ipari, jpari;
            double[] xx = new double[3];
            double[] yy = new double[3];
            double[] zz = new double[3];

            for (int j = 0; j < rst.NumColumns - 1; j++)
            {
                jpari = (((int)(j / 2.0)) * 2 == j);

                for (int i = 0; i < rst.NumRows - 1; i++)
                {
                    ipari = (((int)(i / 2.0)) * 2 == i);

                    if (!jpari && !ipari || jpari && ipari)
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

                        {
                            Coordinate[] c = Intersect(xx, yy, zz, zlev);
                            if (c != null)
                            {
                                lsList.Add(new LineString(c));
                            }
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

                        {
                            Coordinate[] c = Intersect(xx, yy, zz, zlev);
                            if (c != null)
                            {
                                lsList.Add(new LineString(c));
                            }
                        }
                    }

                    if (jpari && !ipari || !jpari && ipari)
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

                        {
                            Coordinate[] c = Intersect(xx, yy, zz, zlev);
                            if (c != null)
                            {
                                lsList.Add(new LineString(c));
                            }
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

                        {
                            Coordinate[] c = Intersect(xx, yy, zz, zlev);
                            if (c != null)
                            {
                                lsList.Add(new LineString(c));
                            }
                        }
                    }
                }
            }

            LineMerger lm = new LineMerger();

            lm.Add(lsList);

            IList<IGeometry> merged = lm.GetMergedLineStrings();

            return merged;
        }



        //private static LineString UnionLineStrings(LineString ls1, LineString ls2)
        //{
        //    LineString l = null;

        //    if (ls1.Coordinates.Count() < 0 | ls2.Coordinates.Count() < 0)
        //    {
        //        return null;
        //    }

        //    Coordinate[] c1 = null;
        //    Coordinate[] c2 = null;



        //    if (ls1.StartPoint.Distance(ls2.StartPoint) < 0.001)
        //    {
        //        c1 = Reverse(ls1.Coordinates);
        //        //c.Add(ls2.Coordinates, false);
        //        c2 = ls2.Coordinates;
        //    }

        //    if (ls1.EndPoint.Distance(ls2.EndPoint) < 0.001)
        //    {
        //        c1 = ls1.Coordinates;
        //        //c.Add((CoordinateList)ls2.Coordinates.Reverse(), false);
        //        c2 = Reverse(ls2.Coordinates);
        //    }

        //    if (ls1.StartPoint.Distance(ls2.EndPoint) < 0.001)
        //    {
        //        c1 = ls2.Coordinates;
        //        //c.Add((CoordinateList)ls1.Coordinates, false);
        //        c2 = ls1.Coordinates;
        //    }

        //    if (ls1.EndPoint.Distance(ls2.StartPoint) < 0.001)
        //    {
        //        c1 = ls1.Coordinates;
        //        //c.Add((CoordinateList)ls2.Coordinates, false);
        //        c2 = ls2.Coordinates;
        //    }

        //    if (c1 != null && c2 != null)
        //    {
        //        Coordinate[] xx = c1;
        //        xx.Concat(c2);
        //        l = new LineString(xx);
        //    }
        //    return l;
        //}

        //private static Coordinate[] Reverse(Coordinate[] l)
        //{
        //    Coordinate[] x = l;

        //    Array.Reverse(x);

        //    return x;
        //}

        private static double _noData;

        private static Coordinate[] Intersect(double[] xx, double[] yy, double[] zz, double zlevel)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            double zmin = Math.Min(zz[0], Math.Min(zz[1], zz[2]));
            double zmax = Math.Max(zz[0], Math.Max(zz[1], zz[2]));

            if (zlevel < zmin || zlevel > zmax) return null;
            if ((zz.Count(x => x == _noData) >= 1) && _type == ContourType.Line)
            {
                return null;
            }

            int i, i1;

            for (i = 0; i < 3; i++)
            {
                i1 = i + 1;
                if (i == 2) i1 = 0;

                zmin = Math.Min(zz[i], zz[i1]);
                zmax = Math.Max(zz[i], zz[i1]);

                if (zlevel > zmin && zlevel <= zmax)
                {
                    double dz = zz[i1] - zz[i];
                    {
                        double frac = (zlevel - zz[i]) / dz;

                        if (frac > 0)
                        {
                            double dx = (xx[i1] - xx[i]);
                            double dy = (yy[i1] - yy[i]);

                            Coordinate c = new Coordinate
                            {
                                X = xx[i] + frac * dx,
                                Y = yy[i] + frac * dy,
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

            //Debug.Print("(" + coordinates[0].X.ToString() + "," + coordinates[0].Y.ToString() + " " + coordinates[1].X.ToString() + "," + coordinates[1].Y.ToString() + ")");

            return coordinates.ToArray();
        }
    }
}