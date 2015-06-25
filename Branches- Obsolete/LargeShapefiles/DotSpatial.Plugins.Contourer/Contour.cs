using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GeoAPI.Geometries;

using NetTopologySuite.Geometries;
using NetTopologySuite.Noding.Snapround;
using NetTopologySuite.Operation.Linemerge;
using NetTopologySuite.Operation.Polygonize;

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

namespace Contourer
{
    public class Contour
    {
        public enum ContourType
        {
            Line,
            Polygon
        }

        private static ContourType type;
        public static DotSpatial.Data.FeatureSet Execute(DotSpatial.Data.Raster rst, ContourType contourType, string FieldName = "Value", double[] levels = null)
        {
            double[] lev = levels;
            noData = rst.NoDataValue;
            type = contourType;
            DotSpatial.Data.Raster iRst = RasterCheck(rst, lev); ;

            string field;

            if (FieldName == null)
            {
                field = "Value";
            }
            else
            {
                field = FieldName;
            }

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

            DotSpatial.Data.FeatureSet fs = null;

            switch (type)
            {
                case ContourType.Line:
                    {
                        fs = new DotSpatial.Data.FeatureSet(DotSpatial.Topology.FeatureType.Line);
                        fs.DataTable.Columns.Add(field, typeof(double));

                        for (int z = 0; z < levels.Length; z++)
                        {
                            IList<IGeometry> cont = GetContours(ref iRst, x, y, lev[z]);

                            foreach (var g in cont)
                            {
                                var f = (DotSpatial.Data.Feature)fs.AddFeature(ToDotSpatialLineString((ILineString)g));
                                f.DataRow[field] = lev[z];
                            }
                        }
                    }
                    break;

                case ContourType.Polygon:
                    {
                        fs = new DotSpatial.Data.FeatureSet(DotSpatial.Topology.FeatureType.Polygon);

                        fs.DataTable.Columns.Add("Lev", typeof(int));
                        fs.DataTable.Columns.Add("Label", typeof(string));

                        Collection<IGeometry> Contours = new Collection<IGeometry>();

                        for (int z = 0; z < levels.Count(); z++)
                        {
                            IList<IGeometry> cont = GetContours(ref iRst, x, y, lev[z]);

                            foreach (var g in cont)
                            {
                                Contours.Add(new LineString(g.Coordinates));
                            }
                        }

                        Coordinate[] Boundary = new Coordinate[5];

                        Boundary[0] = new Coordinate(x[0], y[0]);
                        Boundary[1] = new Coordinate(x[0], y[rst.NumRows - 1]);
                        Boundary[2] = new Coordinate(x[rst.NumColumns - 1], y[rst.NumRows - 1]);
                        Boundary[3] = new Coordinate(x[rst.NumColumns - 1], y[0]);
                        Boundary[4] = new Coordinate(x[0], y[0]);
                        
                        Contours.Add(new LineString(Boundary));

                        Collection<IGeometry> NodedContours = new Collection<IGeometry>();
                        GeometryNoder geomNoder = new GeometryNoder(new PrecisionModel(1000d));

                        foreach (var c in geomNoder.Node(Contours))
                        {
                            NodedContours.Add(c);
                        }

                        Polygonizer polygonizer = new Polygonizer();
                        polygonizer.Add(NodedContours);

                        foreach (IPolygon p in polygonizer.GetPolygons())
                        {

                            Point pnt = (Point)p.InteriorPoint;

                            int c = (int)((pnt.X - iRst.Extent.MinX) / iRst.CellWidth);
                            int r = (int)((iRst.Extent.MaxY - pnt.Y) / iRst.CellHeight);

                            double z = ((DotSpatial.Data.Raster)iRst).Value[r, c];

                            int Cls = GetLevel(z, lev);
                            string label = "Undefined";

                            if (Cls == -1) label = "< " + lev[0].ToString();
                            if (Cls == lev.Count()) label = "> " + lev[lev.Count() - 1].ToString();
                            if (Cls >= 0 & Cls < lev.Count()) label = lev[Cls].ToString() + " - " + lev[Cls + 1].ToString();

                            DotSpatial.Topology.Polygon dsp = ToDotSpatialPolygon(p);

                            DotSpatial.Data.Feature f = (DotSpatial.Data.Feature)fs.AddFeature(dsp);
                            f.DataRow["Lev"] = Cls;
                            f.DataRow["Label"] = label;
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

        //public static int GetLevel(Polygon p, double[] lev)
        //{
        //    double zmin = double.MaxValue;
        //    double zmax = double.MinValue;

        //    foreach (Coordinate c in p.ExteriorRing.Coordinates)
        //    {
        //        if (c.Z < zmin) zmin = c.Z;
        //        if (c.Z > zmax) zmax = c.Z;
        //    }

        //    foreach (LineString ls in p.InteriorRings)
        //    {
        //        foreach (Coordinate c in ls.Coordinates)
        //        {
        //            if (c.Z < zmin) zmin = c.Z;
        //            if (c.Z > zmax) zmax = c.Z;
        //        }
        //    }

        //    if (zmin == zmax)
        //    {
        //        if (zmin == lev[0]) return -1;
        //        if (zmin == lev[lev.Count() - 1]) return lev.Count();
        //    }

        //    for (int i = 0; i < lev.Count(); i++)
        //    {
        //        if (lev[i] == zmin && lev[i + 1] == zmax)
        //        {
        //            return i;
        //        }
        //    }

        //     return -100;
        //}

        public static void CreateMinMaxEvery(DotSpatial.Data.Raster r, ContourType type, out double MinContour, out double MaxContour, out double every)
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

            double Order = Math.Pow(10, Math.Floor(Math.Log10(dz)));

            if (Order == dz) Order /= 10;

            if (dz / Order < 2) Order /= 10;

            MinContour = Math.Floor(min / Order) * Order;
            MaxContour = Math.Ceiling(max / Order) * Order;

            if (MaxContour < max) MaxContour += Order;

            every = Order;

            if (type == ContourType.Line)
            {
                MinContour += every;
                MaxContour -= every;

                if (MaxContour <= MinContour)
                {
                    MaxContour = MinContour + every;
                }
            }
        }

        public static double[] CreateLevels(double MinContour, double MaxContour, double every)
        {
            int c = (int)((MaxContour - MinContour) / every) + 1;

            double[] levels = new double[c];

            for (int i = 0; i < c; i++)
            {
                levels[i] = MinContour + every * i;
            }

            return levels;
        }

        public static DotSpatial.Topology.Geometry toDotSpatial(Geometry l)
        {
            if (l.GeometryType == "LineString")
            {
                return ToDotSpatialLineString((ILineString)l);
            }

            if (l.GeometryType == "Polygon")
            {
                Polygon p = l as Polygon;

                DotSpatial.Topology.Polygon dsp = ToDotSpatialPolygon((GeoAPI.Geometries.IPolygon)l);
                return (DotSpatial.Topology.Geometry)dsp;
            }

            return null;
        }

        internal static DotSpatial.Topology.Point ToDotSpatialPoint(GeoAPI.Geometries.IPoint point)
        {
            return new DotSpatial.Topology.Point(point.X, point.Y);
        }

        internal static DotSpatial.Topology.Point ToDotSpatialPoint(GeoAPI.Geometries.Coordinate point)
        {
            return new DotSpatial.Topology.Point(point.X, point.Y);
        }

        internal static DotSpatial.Topology.Coordinate ToDotSpatialCoordinate(GeoAPI.Geometries.Coordinate coordinate)
        {
            return new DotSpatial.Topology.Coordinate(coordinate.X, coordinate.Y);
        }

        internal static DotSpatial.Topology.LineString ToDotSpatialLineString(GeoAPI.Geometries.ILineString l)
        {
            DotSpatial.Topology.Coordinate[] c = new DotSpatial.Topology.Coordinate[l.Coordinates.Count()];

            for (int i = 0; i < l.Coordinates.Count(); i++)
            {
                c[i] = new DotSpatial.Topology.Coordinate(l.Coordinates[i].X, l.Coordinates[i].Y);
            }
            return new DotSpatial.Topology.LineString(c);
        }

        internal static DotSpatial.Topology.LinearRing ToDotSpatialLinearRing(GeoAPI.Geometries.ILinearRing geom)
        {
            Collection<DotSpatial.Topology.Point> vertices = new Collection<DotSpatial.Topology.Point>();

            foreach (Coordinate coordinate in geom.Coordinates)
            {
                DotSpatial.Topology.Point p = ToDotSpatialPoint(coordinate);

                vertices.Add(p);
            }
            return new DotSpatial.Topology.LinearRing(vertices);
        }

        internal static DotSpatial.Topology.Polygon ToDotSpatialPolygon(GeoAPI.Geometries.IPolygon geom)
        {
            DotSpatial.Topology.LinearRing exteriorRing = ToDotSpatialLinearRing((GeoAPI.Geometries.ILinearRing)geom.ExteriorRing);

            DotSpatial.Topology.LinearRing[] interiorRings = new DotSpatial.Topology.LinearRing[geom.InteriorRings.Count()];

            //foreach (GeoAPI.Geometries.ILineString interiorRing in geom.InteriorRings)
            for (int i = 0; i < geom.InteriorRings.Count(); i++)
            {
                DotSpatial.Topology.LinearRing hole = ToDotSpatialLinearRing((GeoAPI.Geometries.ILinearRing)geom.InteriorRings[i]);
                interiorRings[i] = hole;
            }

            return new DotSpatial.Topology.Polygon(exteriorRing, interiorRings);
        }

        public static DotSpatial.Data.Raster RasterCheck(DotSpatial.Data.Raster raster, double[] levels)
        {
            double eps = (raster.Maximum - raster.Minimum) / 1000;
            int n = levels.Count();

            DotSpatial.Data.Raster rst = raster;

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

        public static IList<IGeometry> GetContours(ref DotSpatial.Data.Raster rst, double[] x, double[] y, double zlev)
        {
            List<LineString> lsList = new List<LineString>();

            bool ipari, jpari;
            double[] xx = new double[3];
            double[] yy = new double[3];
            double[] zz = new double[3];

            for (int j = 0; j < rst.NumColumns - 1; j++)
            {
                if (((int)((double)j / 2.0)) * 2 == j)
                    jpari = true;
                else
                    jpari = false;

                for (int i = 0; i < rst.NumRows - 1; i++)
                {
                    if (((int)((double)i / 2.0)) * 2 == i)
                        ipari = true;
                    else
                        ipari = false;

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

            IList<IGeometry> merged = (IList<IGeometry>)lm.GetMergedLineStrings();

            return merged;
        }

        //static void Comapct(ref List<LineString> lsList)
        //{
        //    int n = lsList.Count();
        //    bool found = true;

        //    while (found)
        //    {
        //        found = false;

        //        for (int i = 0; i < n - 2; i++)
        //        {
        //            for (int j = i + 1; j < n - 1; j++)
        //            {
        //                LineString l = UnionLineStrings(lsList[i], lsList[j]);

        //                if (l != null)
        //                {
        //                    lsList[i] = l;
        //                    lsList.RemoveAt(j);
        //                    n--;
        //                    found = true;
        //                }
        //            }
        //        }

        //    }

        //}

        private static LineString UnionLineStrings(LineString ls1, LineString ls2)
        {
            LineString l = null;

            if (ls1.Coordinates.Count() < 0 | ls2.Coordinates.Count() < 0)
            {
                return null;
            }

            Coordinate[] c1 = null;
            Coordinate[] c2 = null;

            if (ls1.StartPoint.Distance(ls2.StartPoint) < 0.001)
            {
                c1 = Reverse(ls1.Coordinates);
                //c.Add(ls2.Coordinates, false);
                c2 = ls2.Coordinates;
            }

            if (ls1.EndPoint.Distance(ls2.EndPoint) < 0.001)
            {
                c1 = ls1.Coordinates;
                //c.Add((CoordinateList)ls2.Coordinates.Reverse(), false);
                c2 = Reverse(ls2.Coordinates);
            }

            if (ls1.StartPoint.Distance(ls2.EndPoint) < 0.001)
            {
                c1 = ls2.Coordinates;
                //c.Add((CoordinateList)ls1.Coordinates, false);
                c2 = ls1.Coordinates;
            }

            if (ls1.EndPoint.Distance(ls2.StartPoint) < 0.001)
            {
                c1 = ls1.Coordinates;
                //c.Add((CoordinateList)ls2.Coordinates, false);
                c2 = ls2.Coordinates;
            }

            if (c1 != null && c2 != null)
            {
                Coordinate[] xx = c1;
                xx.Concat(c2);
                l = new LineString(xx);
            }
            return l;
        }

        private static Coordinate[] Reverse(Coordinate[] l)
        {
            Coordinate[] x = l;

            Array.Reverse(x);

            return x;

            //List<Coordinate> l1 = new List<Coordinate>();

            //int n = l.Count();

            //for (int i = 0; i < n; i++)
            //{
            //    l1.Add(l[n - i - 1]);
            //}

            //return l1;
        }

        private static double noData;

        private static Coordinate[] Intersect(double[] xx, double[] yy, double[] zz, double zlevel)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            double zmin = Math.Min(zz[0], Math.Min(zz[1], zz[2]));
            double zmax = Math.Max(zz[0], Math.Max(zz[1], zz[2]));

            if (zlevel < zmin || zlevel > zmax) return null;
            if ((zz.Count(x => x == noData) >= 1) && type == ContourType.Line)
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

                            Coordinate c = new Coordinate();

                            c.X = xx[i] + frac * dx;
                            c.Y = yy[i] + frac * dy;
                            c.Z = zlevel;

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