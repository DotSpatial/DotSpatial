// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Planargraph;

namespace DotSpatial.Topology.Operation.Polygonize
{
    /// <summary>
    /// Represents a ring of <c>PolygonizeDirectedEdge</c>s which form
    /// a ring of a polygon.  The ring may be either an outer shell or a hole.
    /// </summary>
    public class EdgeRing
    {
        private readonly IList _deList = new ArrayList();
        private readonly IGeometryFactory _factory;
        private IList _holes;

        // cache the following data for efficiency
        private ILinearRing _ring;

        private IList<Coordinate> _ringPts;

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        public EdgeRing(IGeometryFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Tests whether this ring is a hole.
        /// Due to the way the edges in the polyongization graph are linked,
        /// a ring is a hole if it is oriented counter-clockwise.
        /// </summary>
        /// <returns><c>true</c> if this ring is a hole.</returns>
        public virtual bool IsHole
        {
            get
            {
                return CgAlgorithms.IsCounterClockwise(Ring.Coordinates);
            }
        }

        /// <summary>
        /// Computes and returns the Polygon formed by this ring and any contained holes.
        /// </summary>
        public virtual IPolygon Polygon
        {
            get
            {
                ILinearRing[] holeLr = null;
                if (_holes != null)
                {
                    holeLr = new ILinearRing[_holes.Count];
                    for (int i = 0; i < _holes.Count; i++)
                        holeLr[i] = (ILinearRing)_holes[i];
                }
                IPolygon poly = _factory.CreatePolygon(_ring, holeLr);
                return poly;
            }
        }

        /// <summary>
        /// Tests if the LinearRing ring formed by this edge ring is topologically valid.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                EnsureCoordinates();
                if (_ringPts.Count <= 3)
                    return false;
                EnsureRing();
                return _ring.IsValid;
            }
        }

        /// <summary>
        /// Computes and returns the list of coordinates which are contained in this ring.
        /// The coordinatea are computed once only and cached.
        /// </summary>
        private IList<Coordinate> Coordinates
        {
            get
            {
                EnsureCoordinates();
                return _ringPts;
            }
        }

        /// <summary>
        /// Gets the coordinates for this ring as a <c>LineString</c>.
        /// Used to return the coordinates in this ring
        /// as a valid point, when it has been detected that the ring is topologically
        /// invalid.
        /// </summary>
        public virtual ILineString LineString
        {
            get
            {
                return _factory.CreateLineString(Coordinates);
            }
        }

        /// <summary>
        /// Returns this ring as a LinearRing, or null if an Exception occurs while
        /// creating it (such as a topology problem). Details of problems are written to
        /// standard output.
        /// </summary>
        public virtual ILinearRing Ring
        {
            get
            {
                EnsureRing();
                return _ring;
            }
        }

        /// <summary>
        /// Find the innermost enclosing shell EdgeRing containing the argument EdgeRing, if any.
        /// The innermost enclosing ring is the <i>smallest</i> enclosing ring.
        /// The algorithm used depends on the fact that:
        /// ring A contains ring B iff envelope(ring A) contains envelope(ring B).
        /// This routine is only safe to use if the chosen point of the hole
        /// is known to be properly contained in a shell
        /// (which is guaranteed to be the case if the hole does not touch its shell).
        /// </summary>
        /// <param name="testEr">The EdgeRing to test.</param>
        /// <param name="shellList">The list of shells to test.</param>
        /// <returns>Containing EdgeRing, if there is one, OR
        /// null if no containing EdgeRing is found.</returns>
        public static EdgeRing FindEdgeRingContaining(EdgeRing testEr, IList shellList)
        {
            ILinearRing teString = testEr.Ring;
            IEnvelope testEnv = teString.EnvelopeInternal;

            EdgeRing minShell = null;
            IEnvelope minEnv = null;
            for (IEnumerator it = shellList.GetEnumerator(); it.MoveNext(); )
            {
                EdgeRing tryShell = (EdgeRing)it.Current;
                ILinearRing tryRing = tryShell.Ring;
                IEnvelope tryEnv = tryRing.EnvelopeInternal;
                if (minShell != null)
                    minEnv = minShell.Ring.EnvelopeInternal;
                bool isContained = false;
                // the hole envelope cannot equal the shell envelope
                if (tryEnv.Equals(testEnv)) continue;

                Coordinate testPt = PtNotInList(teString.Coordinates, tryRing.Coordinates);
                if (tryEnv.Contains(testEnv) && CgAlgorithms.IsPointInRing(testPt, tryRing.Coordinates))
                    isContained = true;
                // check if this new containing ring is smaller than the current minimum ring
                if (isContained)
                {
                    if (minShell == null || minEnv.Contains(tryEnv))
                        minShell = tryShell;
                }
            }
            return minShell;
        }

        /// <summary>
        /// Finds a point in a list of points which is not contained in another list of points.
        /// </summary>
        /// <param name="testPts">The <c>Coordinate</c>s to test.</param>
        /// <param name="pts">An array of <c>Coordinate</c>s to test the input points against.</param>
        /// <returns>A <c>Coordinate</c> from <c>testPts</c> which is not in <c>pts</c>,
        /// or <c>null</c>.</returns>
        public static Coordinate PtNotInList(IList<Coordinate> testPts, IList<Coordinate> pts)
        {
            for (int i = 0; i < testPts.Count; i++)
            {
                if (IsInList(testPts[i], pts) == false)
                    return new Coordinate(testPts[i]);
            }
            return null;
        }

        /// <summary>
        /// Tests whether a given point is in an array of points.
        /// Uses a value-based test.
        /// </summary>
        /// <param name="pt">A <c>Coordinate</c> for the test point.</param>
        /// <param name="pts">An array of <c>Coordinate</c>s to test,</param>
        /// <returns><c>true</c> if the point is in the array.</returns>
        public static bool IsInList(Coordinate pt, IList<Coordinate> pts)
        {
            for (int i = 0; i < pts.Count; i++)
                if (new Coordinate(pt).Equals(pts[i]))
                    return true;
            return false;
        }

        /// <summary>
        /// Adds a DirectedEdge which is known to form part of this ring.
        /// </summary>
        /// <param name="de">The DirectedEdge to add.</param>
        public virtual void Add(DirectedEdge de)
        {
            _deList.Add(de);
        }

        /// <summary>
        /// Adds a hole to the polygon formed by this ring.
        /// </summary>
        /// <param name="hole">The LinearRing forming the hole.</param>
        public virtual void AddHole(ILinearRing hole)
        {
            if (_holes == null)
                _holes = new ArrayList();
            _holes.Add(hole);
        }

        private void EnsureCoordinates()
        {
            if (_ringPts != null) return;
            CoordinateList coordList = new CoordinateList();
            for (IEnumerator i = _deList.GetEnumerator(); i.MoveNext(); )
            {
                DirectedEdge de = (DirectedEdge)i.Current;
                PolygonizeEdge edge = (PolygonizeEdge)de.Edge;
                AddEdge(edge.Line.Coordinates, de.EdgeDirection, coordList);
            }
            _ringPts = coordList;
        }

        private void EnsureRing()
        {
            if (_ring != null) return;
            _ring = _factory.CreateLinearRing(Coordinates);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="isForward"></param>
        /// <param name="coordList"></param>
        private static void AddEdge(IList<Coordinate> coords, bool isForward, CoordinateList coordList)
        {
            if (isForward)
            {
                for (int i = 0; i < coords.Count; i++)
                    coordList.Add(coords[i], false);
            }
            else
            {
                for (int i = coords.Count - 1; i >= 0; i--)
                    coordList.Add(coords[i], false);
            }
        }
    }
}