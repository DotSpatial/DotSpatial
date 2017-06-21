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

namespace DotSpatial.Topology.Operation.Polygonize
{
    /// <summary>
    /// Polygonizes a set of Geometrys which contain linework that
    /// represents the edges of a planar graph.
    /// Any dimension of Geometry is handled - the constituent linework is extracted
    /// to form the edges.
    /// The edges must be correctly noded; that is, they must only meet
    /// at their endpoints.  The Polygonizer will still run on incorrectly noded input
    /// but will not form polygons from incorrected noded edges.
    /// The Polygonizer reports the follow kinds of errors:
    /// Dangles - edges which have one or both ends which are not incident on another edge endpoint
    /// Cut Edges - edges which are connected at both ends but which do not form part of polygon
    /// Invalid Ring Lines - edges which form rings which are invalid
    /// (e.g. the component lines contain a self-intersection).
    /// </summary>
    public class Polygonizer
    {
        #region Private Variables

        private readonly PolygonizeGraph _graph = new PolygonizeGraph(new GeometryFactory());
        private readonly LineStringAdder _lineStringAdder; // Default adder
        private IList _cutEdges = new ArrayList();
        private IList _dangles = new ArrayList();
        private IList _holeList;
        private IList _invalidRingLines = new ArrayList();
        private IList _polyList;
        private IList _shellList;

        #endregion

        /// <summary>
        /// Create a polygonizer with the same {GeometryFactory}
        /// as the input <c>Geometry</c>s.
        /// </summary>
        public Polygonizer()
        {
            _lineStringAdder = new LineStringAdder(this);
        }

        /// <summary>
        /// Compute and returns the list of polygons formed by the polygonization.
        /// </summary>
        public virtual IList Polygons
        {
            get
            {
                Polygonize();
                return _polyList;
            }
        }

        /// <summary>
        /// Compute and returns the list of dangling lines found during polygonization.
        /// </summary>
        public virtual IList Dangles
        {
            get
            {
                Polygonize();
                return _dangles;
            }
            protected set
            {
                _dangles = value;
            }
        }

        /// <summary>
        /// Compute and returns the list of cut edges found during polygonization.
        /// </summary>
        public virtual IList CutEdges
        {
            get
            {
                Polygonize();
                return _cutEdges;
            }
            protected set
            {
                _cutEdges = value;
            }
        }

        /// <summary>
        /// Compute and returns the list of lines forming invalid rings found during polygonization.
        /// </summary>
        public virtual IList InvalidRingLines
        {
            get
            {
                Polygonize();
                return _invalidRingLines;
            }
            protected set
            {
                _invalidRingLines = value;
            }
        }

        /// <summary>
        /// Add a collection of geometries to be polygonized.
        /// May be called multiple times.
        /// Any dimension of Geometry may be added;
        /// the constituent linework will be extracted and used.
        /// </summary>
        /// <param name="geomList">A list of <c>Geometry</c>s with linework to be polygonized.</param>
        public virtual void Add(IList geomList)
        {
            for (IEnumerator i = geomList.GetEnumerator(); i.MoveNext(); )
            {
                Geometry geometry = (Geometry)i.Current;
                Add(geometry);
            }
        }

        /// <summary>
        /// Add a point to the linework to be polygonized.
        /// May be called multiple times.
        /// Any dimension of Geometry may be added;
        /// the constituent linework will be extracted and used
        /// </summary>
        /// <param name="g">A <c>Geometry</c> with linework to be polygonized.</param>
        public virtual void Add(IGeometry g)
        {
            g.Apply(_lineStringAdder);
        }

        /// <summary>
        /// Perform the polygonization, if it has not already been carried out.
        /// </summary>
        private void Polygonize()
        {
            // check if already computed
            if (_polyList != null) return;
            _polyList = new ArrayList();

            // if no geometries were supplied it's possible graph could be null
            if (_graph == null) return;

            _dangles = _graph.DeleteDangles();
            _cutEdges = _graph.DeleteCutEdges();
            IList edgeRingList = _graph.GetEdgeRings();

            IList validEdgeRingList = new ArrayList();
            _invalidRingLines = new ArrayList();
            FindValidRings(edgeRingList, validEdgeRingList, _invalidRingLines);

            FindShellsAndHoles(validEdgeRingList);
            AssignHolesToShells(_holeList, _shellList);

            _polyList = new ArrayList();
            for (IEnumerator i = _shellList.GetEnumerator(); i.MoveNext(); )
            {
                EdgeRing er = (EdgeRing)i.Current;
                _polyList.Add(er.Polygon);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edgeRingList"></param>
        /// <param name="validEdgeRingList"></param>
        /// <param name="invalidRingList"></param>
        private static void FindValidRings(IEnumerable edgeRingList, IList validEdgeRingList, IList invalidRingList)
        {
            for (IEnumerator i = edgeRingList.GetEnumerator(); i.MoveNext(); )
            {
                EdgeRing er = (EdgeRing)i.Current;
                if (er.IsValid)
                    validEdgeRingList.Add(er);
                else invalidRingList.Add(er.LineString);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edgeRingList"></param>
        private void FindShellsAndHoles(IEnumerable edgeRingList)
        {
            _holeList = new ArrayList();
            _shellList = new ArrayList();
            for (IEnumerator i = edgeRingList.GetEnumerator(); i.MoveNext(); )
            {
                EdgeRing er = (EdgeRing)i.Current;
                if (er.IsHole)
                    _holeList.Add(er);
                else _shellList.Add(er);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="holeList"></param>
        /// <param name="shellList"></param>
        private static void AssignHolesToShells(IEnumerable holeList, IList shellList)
        {
            for (IEnumerator i = holeList.GetEnumerator(); i.MoveNext(); )
            {
                EdgeRing holeEr = (EdgeRing)i.Current;
                AssignHoleToShell(holeEr, shellList);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="holeEr"></param>
        /// <param name="shellList"></param>
        private static void AssignHoleToShell(EdgeRing holeEr, IList shellList)
        {
            EdgeRing shell = EdgeRing.FindEdgeRingContaining(holeEr, shellList);
            if (shell != null)
                shell.AddHole(holeEr.Ring);
        }
    }
}