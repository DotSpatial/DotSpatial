using System;
using System.Diagnostics;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Geometries.Implementation;
using DotSpatial.Topology.IO;
using DotSpatial.Topology.Mathematics;

namespace DotSpatial.Topology.Triangulate.QuadEdge
{

    /// <summary>
    /// Algorithms for computing values and predicates
    /// associated with triangles.
    /// </summary>
    /// <remarks>
    /// For some algorithms extended-precision 
    /// implementations are provided, which are more robust 
    /// (i.e. they produce correct answers in more cases).
    /// Also, some more robust formulations of
    /// some algorithms are provided, which utilize 
    /// normalization to the origin.
    /// </remarks>
    /// <author>Martin Davis</author>
    public static class TrianglePredicate
    {
        #region Methods

        /// <summary>
        /// Checks if the computed value for isInCircle is correct, using
        /// double-double precision arithmetic.
        /// </summary>
        /// <param name="a">A vertex of the triangle</param>
        /// <param name="b">A vertex of the triangle</param>
        /// <param name="c">A vertex of the triangle</param>
        /// <param name="p">The point to test</param>
        private static void CheckRobustInCircle(Coordinate a, Coordinate b, Coordinate c, Coordinate p)
        {
            bool nonRobustInCircle = IsInCircleNonRobust(a, b, c, p);
            bool isInCircleDD = IsInCircleDDSlow(a, b, c, p);
            bool isInCircleCC = IsInCircleCC(a, b, c, p);

            Coordinate circumCentre = Triangle.Circumcentre(a, b, c);

            // ReSharper disable RedundantStringFormatCall
            // String.Format needed to build 2.0 release!
            Debug.WriteLine(String.Format("p radius diff a = {0}", Math.Abs(p.Distance(circumCentre) - a.Distance(circumCentre)) / a.Distance(circumCentre)));
            if (nonRobustInCircle != isInCircleDD || nonRobustInCircle != isInCircleCC)
            {
                Debug.WriteLine(String.Format("inCircle robustness failure (double result = {0}, DD result = {1}, CC result = {2})", nonRobustInCircle, isInCircleDD, isInCircleCC));
                Debug.WriteLine(WKTWriter.ToLineString(new CoordinateArraySequence(new[] { a, b, c, p })));
                Debug.WriteLine(String.Format("Circumcentre = {0} radius = {1}", WKTWriter.ToPoint(circumCentre), a.Distance(circumCentre)));
                Debug.WriteLine(String.Format("p radius diff a = {0}", Math.Abs(p.Distance(circumCentre) / a.Distance(circumCentre) - 1)));
                Debug.WriteLine(String.Format("p radius diff b = {0}", Math.Abs(p.Distance(circumCentre) / b.Distance(circumCentre) - 1)));
                Debug.WriteLine(String.Format("p radius diff c = {0}", Math.Abs(p.Distance(circumCentre) / c.Distance(circumCentre) - 1)));
                Debug.WriteLine("");
            }
            // ReSharper restore RedundantStringFormatCall
        }

        /// <summary>
        /// Computes the inCircle test using distance from the circumcentre. 
        /// Uses standard double-precision arithmetic.
        /// </summary>
        /// <remarks>
        /// In general this doesn't
        /// appear to be any more robust than the standard calculation. However, there
        /// is at least one case where the test point is far enough from the
        /// circumcircle that this test gives the correct answer. 
        /// <pre>
        /// LINESTRING (1507029.9878 518325.7547, 1507022.1120341457 518332.8225183258,
        /// 1507029.9833 518325.7458, 1507029.9896965567 518325.744909031)
        /// </pre>
        /// </remarks>
        /// <param name="a">A vertex of the triangle</param>
        /// <param name="b">A vertex of the triangle</param>
        /// <param name="c">A vertex of the triangle</param>
        /// <param name="p">The point to test</param>
        /// <returns>The area of a triangle defined by the points a, b and c</returns>
        public static bool IsInCircleCC(Coordinate a, Coordinate b, Coordinate c, Coordinate p)
        {
            Coordinate cc = Triangle.Circumcentre(a, b, c);
            double ccRadius = a.Distance(cc);
            double pRadiusDiff = p.Distance(cc) - ccRadius;
            return pRadiusDiff <= 0;
        }

        /// <summary>
        /// Tests if a point is inside the circle defined by 
        /// the triangle with vertices a, b, c (oriented counter-clockwise). 
        /// </summary>
        /// <remarks>
        /// The computation uses <see cref="DD"/> arithmetic for robustness, but a faster approach.
        /// </remarks>
        /// <param name="a">A vertex of the triangle</param>
        /// <param name="b">A vertex of the triangle</param>
        /// <param name="c">A vertex of the triangle</param>
        /// <param name="p">The point to test</param>
        /// <returns>true if this point is inside the circle defined by the points a, b, c</returns>
        public static bool IsInCircleDDFast(Coordinate a, Coordinate b, Coordinate c, Coordinate p)
        {
            DoubleDouble aTerm = (DoubleDouble.Sqr(a.X) + DoubleDouble.Sqr(a.Y)) * TriAreaDDFast(b, c, p);
            DoubleDouble bTerm = (DoubleDouble.Sqr(b.X) + DoubleDouble.Sqr(b.Y)) * TriAreaDDFast(a, c, p);
            DoubleDouble cTerm = (DoubleDouble.Sqr(c.X) + DoubleDouble.Sqr(c.Y)) * TriAreaDDFast(a, b, p);
            DoubleDouble pTerm = (DoubleDouble.Sqr(p.X) + DoubleDouble.Sqr(p.Y)) * TriAreaDDFast(a, b, c);

            DoubleDouble sum = aTerm - bTerm + cTerm - pTerm;
            bool isInCircle = sum.ToDoubleValue() > 0;

            return isInCircle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsInCircleDDNormalized(Coordinate a, Coordinate b, Coordinate c, Coordinate p)
        {
            DoubleDouble adx = DoubleDouble.ValueOf(a.X) - p.X;
            DoubleDouble ady = DoubleDouble.ValueOf(a.Y) - p.Y;
            DoubleDouble bdx = DoubleDouble.ValueOf(b.X) - p.X;
            DoubleDouble bdy = DoubleDouble.ValueOf(b.Y) - p.Y;
            DoubleDouble cdx = DoubleDouble.ValueOf(c.X) - p.X;
            DoubleDouble cdy = DoubleDouble.ValueOf(c.Y) - p.Y;

            DoubleDouble abdet = adx * bdy - bdx * ady;
            DoubleDouble bcdet = bdx * cdy - cdx * bdy;
            DoubleDouble cadet = cdx * ady - adx * cdy;
            DoubleDouble alift = adx * adx + ady * ady;
            DoubleDouble blift = bdx * bdx + bdy * bdy;
            DoubleDouble clift = cdx * cdx + cdy * cdy;

            DoubleDouble sum = alift * bcdet + blift * cadet + clift * abdet;

            bool isInCircle = sum.ToDoubleValue() > 0;

            return isInCircle;
        }

        /// <summary>
        /// Tests if a point is inside the circle defined by 
        /// the triangle with vertices a, b, c (oriented counter-clockwise). 
        /// </summary>
        /// <remarks>
        /// The computation uses <see cref="DD"/> arithmetic for robustness.
        /// </remarks>
        /// <param name="a">A vertex of the triangle</param>
        /// <param name="b">A vertex of the triangle</param>
        /// <param name="c">A vertex of the triangle</param>
        /// <param name="p">The point to test</param>
        /// <returns>true if this point is inside the circle defined by the points a, b, c</returns>
        public static bool IsInCircleDDSlow(Coordinate a, Coordinate b, Coordinate c, Coordinate p)
        {
            DoubleDouble px = DoubleDouble.ValueOf(p.X);
            DoubleDouble py = DoubleDouble.ValueOf(p.Y);
            DoubleDouble ax = DoubleDouble.ValueOf(a.X);
            DoubleDouble ay = DoubleDouble.ValueOf(a.Y);
            DoubleDouble bx = DoubleDouble.ValueOf(b.X);
            DoubleDouble by = DoubleDouble.ValueOf(b.Y);
            DoubleDouble cx = DoubleDouble.ValueOf(c.X);
            DoubleDouble cy = DoubleDouble.ValueOf(c.Y);

            DoubleDouble aTerm = (ax * ax + ay * ay) * TriAreaDDSlow(bx, by, cx, cy, px, py);
            DoubleDouble bTerm = (bx * bx + by * by) * TriAreaDDSlow(ax, ay, cx, cy, px, py);
            DoubleDouble cTerm = (cx * cx + cy * cy) * TriAreaDDSlow(ax, ay, bx, by, px, py);
            DoubleDouble pTerm = (px * px + py * py) * TriAreaDDSlow(ax, ay, bx, by, cx, cy);

            DoubleDouble sum = aTerm - bTerm + cTerm - pTerm;
            bool isInCircle = sum.ToDoubleValue() > 0;

            return isInCircle;
        }

        /// <summary>
        /// Tests if a point is inside the circle defined by 
        /// the triangle with vertices a, b, c (oriented counter-clockwise). 
        /// This test uses simple
        /// double-precision arithmetic, and thus is not 100% robust.
        /// </summary>
        /// <param name="a">A vertex of the triangle</param>
        /// <param name="b">A vertex of the triangle</param>
        /// <param name="c">A vertex of the triangle</param>
        /// <param name="p">The point to test</param>
        /// <returns>true if this point is inside the circle defined by the points a, b, c</returns>
        public static bool IsInCircleNonRobust(Coordinate a, Coordinate b, Coordinate c, Coordinate p)
        {
            bool isInCircle =
                  (a.X * a.X + a.Y * a.Y) * TriArea(b, c, p)
                - (b.X * b.X + b.Y * b.Y) * TriArea(a, c, p)
                + (c.X * c.X + c.Y * c.Y) * TriArea(a, b, p)
                - (p.X * p.X + p.Y * p.Y) * TriArea(a, b, c)
                > 0;
            return isInCircle;
        }

        /// <summary>
        /// Tests if a point is inside the circle defined by 
        /// the triangle with vertices a, b, c (oriented counter-clockwise).
        /// </summary>
        /// <remarks>
        /// <para> This test uses simple
        /// double-precision arithmetic, and thus is not 100% robust.
        /// However, by using normalization to the origin
        /// it provides improved robustness and increased performance.</para>
        /// <para>Based on code by J.R.Shewchuk.</para>
        /// </remarks>
        /// <param name="a">A vertex of the triangle</param>
        /// <param name="b">A vertex of the triangle</param>
        /// <param name="c">A vertex of the triangle</param>
        /// <param name="p">The point to test</param>
        /// <returns>true if this point is inside the circle defined by the points a, b, c</returns>
        public static bool IsInCircleNormalized(Coordinate a, Coordinate b, Coordinate c, Coordinate p)
        {
            double adx = a.X - p.X;
            double ady = a.Y - p.Y;
            double bdx = b.X - p.X;
            double bdy = b.Y - p.Y;
            double cdx = c.X - p.X;
            double cdy = c.Y - p.Y;

            double abdet = adx * bdy - bdx * ady;
            double bcdet = bdx * cdy - cdx * bdy;
            double cadet = cdx * ady - adx * cdy;
            double alift = adx * adx + ady * ady;
            double blift = bdx * bdx + bdy * bdy;
            double clift = cdx * cdx + cdy * cdy;

            double disc = alift * bcdet + blift * cadet + clift * abdet;
            return disc > 0;
        }

        /// <summary>
        /// Tests if a point is inside the circle defined by 
        /// the triangle with vertices a, b, c (oriented counter-clockwise). 
        /// </summary>
        /// <remarks>
        /// This method uses more robust computation.
        /// </remarks>
        /// <param name="a">A vertex of the triangle</param>
        /// <param name="b">A vertex of the triangle</param>
        /// <param name="c">A vertex of the triangle</param>
        /// <param name="p">The point to test</param>
        /// <returns>true if this point is inside the circle defined by the points a, b, c</returns>
        public static bool IsInCircleRobust(Coordinate a, Coordinate b, Coordinate c, Coordinate p)
        {
            return IsInCircleNormalized(a, b, c, p);
        }

        /// <summary>
        /// Computes twice the area of the oriented triangle (a, b, c), i.e., the area is positive if the
        /// triangle is oriented counterclockwise.
        /// </summary>
        /// <param name="a">A vertex of the triangle</param>
        /// <param name="b">A vertex of the triangle</param>
        /// <param name="c">A vertex of the triangle</param>
        /// <returns>The area of the triangle defined by the points a, b, c</returns>
        private static double TriArea(Coordinate a, Coordinate b, Coordinate c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        /// <summary>
        /// Computes twice the area of the oriented triangle (a, b, c), i.e., the area
        /// is positive if the triangle is oriented counterclockwise.
        /// </summary>
        /// <remarks>
        /// The computation uses {@link DD} arithmetic for robustness.
        /// </remarks>
        /// <param name="a">a vertex of the triangle</param>
        /// <param name="b">a vertex of the triangle</param>
        /// <param name="c">a vertex of the triangle</param>
        /// <returns>The area of a triangle defined by the points a, b and c</returns>
        private static DoubleDouble TriAreaDDFast(Coordinate a, Coordinate b, Coordinate c)
        {

            DoubleDouble t1 = (DoubleDouble.ValueOf(b.X) - a.X) * (DoubleDouble.ValueOf(c.Y) - a.Y);
            DoubleDouble t2 = (DoubleDouble.ValueOf(b.Y) - a.Y) * (DoubleDouble.ValueOf(c.X) - a.X);

            return t1 - t2;
        }

        /// <summary>
        /// Computes twice the area of the oriented triangle (a, b, c), i.e., the area
        /// is positive if the triangle is oriented counterclockwise.
        /// </summary>
        /// <remarks>
        /// The computation uses {@link DD} arithmetic for robustness.
        /// </remarks>
        /// <param name="ax">x ordinate of a vertex of the triangle</param>
        /// <param name="ay">y ordinate of a vertex of the triangle</param>
        /// <param name="bx">x ordinate of a vertex of the triangle</param>
        /// <param name="by">y ordinate of a vertex of the triangle</param>
        /// <param name="cx">x ordinate of a vertex of the triangle</param>
        /// <param name="cy">y ordinate of a vertex of the triangle</param>
        /// <returns>The area of a triangle defined by the points a, b and c</returns>
        private static DoubleDouble TriAreaDDSlow(DoubleDouble ax, DoubleDouble ay, DoubleDouble bx, DoubleDouble by, DoubleDouble cx, DoubleDouble cy)
        {
            return (bx - ax) * (cy - ay) - (by - ay) * (cx - ax);
        }

        #endregion
    }
}