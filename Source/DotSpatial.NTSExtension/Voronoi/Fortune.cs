// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// The Original Code is from a code project example:
// http://www.codeproject.com/KB/recipes/fortunevoronoi.aspx
// which is protected under the Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx
//
namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// Contains static methods and parameters that organize the major elements of applying the Fortune linesweep methods.
    /// </summary>
    public abstract class Fortune
    {
        #region Fields

        /// <summary>
        /// Represents an infinite vector location.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly Vector2 VVInfinite = new Vector2(double.PositiveInfinity, double.PositiveInfinity);

        /// <summary>
        /// The default definition of a coordinate that uses double.NaN to clarify that no value has yet been assigned to this vector.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly Vector2 VVUnkown = new Vector2(double.NaN, double.NaN);

        /// <summary>
        /// Gets or sets a value indicating whether the cleanup method should be called. This is unnecessary, for
        /// the mapwindow implementation and will in fact cause the implementation to break
        /// because infinities and other bad values start showing up.
        /// </summary>
        public static bool DoCleanup { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the voronoi graph, but specifies a tolerance below which values should be considered equal.
        /// </summary>
        /// <param name="vertices">The original points to use during the calculation.</param>
        /// <param name="tolerance">A double value that controls the test for equality.</param>
        /// <param name="cleanup">This for Ben's code and should be passed as true if cleanup should be done.</param>
        /// <returns>A VoronoiGraph structure containing the output geometries.</returns>
        public static VoronoiGraph ComputeVoronoiGraph(double[] vertices, double tolerance, bool cleanup)
        {
            Vector2.Tolerance = tolerance;
            DoCleanup = cleanup;
            return ComputeVoronoiGraph(vertices);
        }

        /// <summary>
        /// Calculates a list of edges and junction vertices by using the specified points.
        /// This defaults to not using any tolerance for determining if points are equal,
        /// and will not use the cleanup algorithm, which breaks the HandleBoundaries
        /// method in the Voronoi class.
        /// </summary>
        /// <param name="vertices">The original points to use during the calculation.</param>
        /// <returns>A VoronoiGraph structure containing the output geometries.</returns>
        public static VoronoiGraph ComputeVoronoiGraph(double[] vertices)
        {
            SortedDictionary<VEvent, VEvent> pq = new SortedDictionary<VEvent, VEvent>();

            Dictionary<VDataNode, VCircleEvent> currentCircles = new Dictionary<VDataNode, VCircleEvent>();
            VoronoiGraph vg = new VoronoiGraph();
            VNode rootNode = null;
            for (int i = 0; i < vertices.Length / 2; i++)
            {
                VDataEvent e = new VDataEvent(new Vector2(vertices, i * 2));
                if (pq.ContainsKey(e)) continue;
                pq.Add(e, e);
            }

            while (pq.Count > 0)
            {
                VEvent ve = pq.First().Key;
                pq.Remove(ve);

                VDataNode[] circleCheckList = new VDataNode[] { };
                if (ve is VDataEvent)
                {
                    rootNode = VNode.ProcessDataEvent(ve as VDataEvent, rootNode, vg, ve.Y, out circleCheckList);
                }
                else if (ve is VCircleEvent)
                {
                    currentCircles.Remove(((VCircleEvent)ve).NodeN);
                    if (!((VCircleEvent)ve).Valid) continue;
                    rootNode = VNode.ProcessCircleEvent(ve as VCircleEvent, rootNode, vg, out circleCheckList);
                }
                else if (ve != null)
                {
                    throw new Exception("Got event of type " + ve.GetType() + "!");
                }

                foreach (VDataNode vd in circleCheckList)
                {
                    if (currentCircles.ContainsKey(vd))
                    {
                        currentCircles[vd].Valid = false;
                        currentCircles.Remove(vd);
                    }

                    if (ve == null) continue;
                    VCircleEvent vce = VNode.CircleCheckDataNode(vd, ve.Y);
                    if (vce == null) continue;

                    pq.Add(vce, vce);

                    currentCircles[vd] = vce;
                }

                if (!(ve is VDataEvent)) continue;
                Vector2 dp = ((VDataEvent)ve).DataPoint;
                foreach (VCircleEvent vce in currentCircles.Values)
                {
                    if (MathTools.Dist(dp.X, dp.Y, vce.Center.X, vce.Center.Y) < vce.Y - vce.Center.Y && Math.Abs(MathTools.Dist(dp.X, dp.Y, vce.Center.X, vce.Center.Y) - (vce.Y - vce.Center.Y)) > 1e-10) vce.Valid = false;
                }
            }

            // This is where the MapWindow version should exit since it uses the HandleBoundaries
            // function instead. The following code is needed for Benjamin Ditter's original process to work.
            if (!DoCleanup) return vg;

            VNode.CleanUpTree(rootNode);
            foreach (VoronoiEdge ve in vg.Edges)
            {
                if (ve.Done) continue;
                if (ve.VVertexB != VVUnkown) continue;
                ve.AddVertex(VVInfinite);
                if (Math.Abs(ve.LeftData.Y - ve.RightData.Y) < 1e-10 && ve.LeftData.X < ve.RightData.X)
                {
                    Vector2 t = ve.LeftData;
                    ve.LeftData = ve.RightData;
                    ve.RightData = t;
                }
            }

            ArrayList minuteEdges = new ArrayList();
            foreach (VoronoiEdge ve in vg.Edges)
            {
                if (ve.IsPartlyInfinite || !ve.VVertexA.Equals(ve.VVertexB)) continue;
                minuteEdges.Add(ve);

                // prevent rounding errors from expanding to holes
                foreach (VoronoiEdge ve2 in vg.Edges)
                {
                    if (ve2.VVertexA.Equals(ve.VVertexA)) ve2.VVertexA = ve.VVertexA;
                    if (ve2.VVertexB.Equals(ve.VVertexA)) ve2.VVertexB = ve.VVertexA;
                }
            }

            foreach (VoronoiEdge ve in minuteEdges)
            {
                vg.Edges.Remove(ve);
            }

            return vg;
        }

        /// <summary>
        /// Applies an optional cleanup method needed by Benjamine Ditter for laser data calculations.
        /// This is not used by the MapWindow calculations.
        /// </summary>
        /// <param name="vg">The output voronoi graph created in the Compute Voronoi Graph section.</param>
        /// <param name="minLeftRightDist">A minimum left to right distance.</param>
        /// <returns>The Voronoi Graph after it has been filtered.</returns>
        public static VoronoiGraph FilterVg(VoronoiGraph vg, double minLeftRightDist)
        {
            VoronoiGraph vgErg = new VoronoiGraph();
            foreach (VoronoiEdge ve in vg.Edges)
            {
                if (ve.LeftData.Distance(ve.RightData) >= minLeftRightDist) vgErg.Edges.Add(ve);
            }

            foreach (VoronoiEdge ve in vgErg.Edges)
            {
                vgErg.Vertices.Add(ve.VVertexA);
                vgErg.Vertices.Add(ve.VVertexB);
            }

            return vgErg;
        }

        /// <summary>
        /// Gets the center of the circle.
        /// </summary>
        /// <param name="a">First circle point.</param>
        /// <param name="b">Second circle point.</param>
        /// <param name="c">Third circle point.</param>
        /// <returns>The center vector.</returns>
        /// <exception cref="ArgumentException">Thrown if at least 2 of the points are the same.</exception>
        internal static Vector2 CircumCircleCenter(Vector2 a, Vector2 b, Vector2 c)
        {
            if (a == b || b == c || a == c) throw new ArgumentException("Need three different points!");

            double tx = (a.X + c.X) / 2;
            double ty = (a.Y + c.Y) / 2;

            double vx = (b.X + c.X) / 2;
            double vy = (b.Y + c.Y) / 2;

            double ux, uy, wx, wy;

            if (a.X == c.X)
            {
                ux = 1;
                uy = 0;
            }
            else
            {
                ux = (c.Y - a.Y) / (a.X - c.X);
                uy = 1;
            }

            if (b.X == c.X)
            {
                wx = -1;
                wy = 0;
            }
            else
            {
                wx = (b.Y - c.Y) / (b.X - c.X);
                wy = -1;
            }

            double alpha = ((wy * (vx - tx)) - (wx * (vy - ty))) / ((ux * wy) - (wx * uy));

            return new Vector2(tx + (alpha * ux), ty + (alpha * uy));
        }

        /// <summary>
        /// Performs a parabolic cut with the given values.
        /// </summary>
        /// <param name="x1">First x value.</param>
        /// <param name="y1">First y value.</param>
        /// <param name="x2">Second x value.</param>
        /// <param name="y2">Second y value.</param>
        /// <param name="ys">The ys.</param>
        /// <returns>The resulting x value.</returns>
        /// <exception cref="ArgumentException">Thrown if the two points are the same.</exception>
        internal static double ParabolicCut(double x1, double y1, double x2, double y2, double ys)
        {
            if (x1 == x2 && y1 == y2)
            {
                throw new ArgumentException("Identical datapoints are not allowed!");
            }

            if (y1 == ys && y2 == ys) return (x1 + x2) / 2;
            if (y1 == ys) return x1;
            if (y2 == ys) return x2;
            double a1 = 1 / (2 * (y1 - ys));
            double a2 = 1 / (2 * (y2 - ys));
            if (a1 == a2) return (x1 + x2) / 2;
            double root = Math.Sqrt((-8 * a1 * x1 * a2 * x2) - (2 * a1 * y1) + (2 * a1 * y2) + (4 * a1 * a2 * x2 * x2) + (2 * a2 * y1) + (4 * a2 * a1 * x1 * x1) - (2 * a2 * y2));
            double xs1 = 0.5 / ((2 * a1) - (2 * a2)) * ((4 * a1 * x1) - (4 * a2 * x2) + (2 * root));
            double xs2 = 0.5 / ((2 * a1) - (2 * a2)) * ((4 * a1 * x1) - (4 * a2 * x2) - (2 * root));
            if (xs1 > xs2)
            {
                double h = xs1;
                xs1 = xs2;
                xs2 = h;
            }

            return y1 >= y2 ? xs2 : xs1;
        }

        #endregion
    }
}