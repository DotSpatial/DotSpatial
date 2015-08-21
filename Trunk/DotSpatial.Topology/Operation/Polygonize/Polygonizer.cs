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

using System.Collections.Generic;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Operation.Polygonize
{
    /// <summary>
    /// Polygonizes a set of <see cref="IGeometry"/>s which contain linework that
    /// represents the edges of a planar graph.
    /// </summary>
    /// <remarks>
    /// <para>All types of Geometry are accepted as input;
    /// the constituent linework is extracted as the edges to be polygonized.
    /// The processed edges must be correctly noded; that is, they must only meet
    /// at their endpoints.  The Polygonizer will run on incorrectly noded input
    /// but will not form polygons from non-noded edges, 
    /// and will report them as errors.
    /// </para><para>
    /// The Polygonizer reports the follow kinds of errors:
    /// Dangles - edges which have one or both ends which are not incident on another edge endpoint
    /// Cut Edges - edges which are connected at both ends but which do not form part of polygon
    /// Invalid Ring Lines - edges which form rings which are invalid
    /// (e.g. the component lines contain a self-intersection).</para>
    /// </remarks>
    public class Polygonizer
    {
        #region Fields

        /// <summary>
        /// Default linestring adder.
        /// </summary>
        private readonly LineStringAdder _lineStringAdder;

        private ICollection<ILineString> _cutEdges = new List<ILineString>();
        // Initialized with empty collections, in case nothing is computed
        private ICollection<ILineString> _dangles = new List<ILineString>();
        private PolygonizeGraph _graph;
        private IList<EdgeRing> _holeList;
        private IList<IGeometry> _invalidRingLines = new List<IGeometry>();
        private bool _isCheckingRingsValid = true;
        private ICollection<IGeometry> _polyList;
        private IList<EdgeRing> _shellList;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a polygonizer with the same {GeometryFactory}
        /// as the input <c>Geometry</c>s.
        /// </summary>
        public Polygonizer()
        {
            _lineStringAdder = new LineStringAdder(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Allows disabling the valid ring checking, 
        /// to optimize situations where invalid rings are not expected.
        /// </summary>
        /// <remarks>The default is <c>true</c></remarks>
        public bool IsCheckingRingsValid
        {
            get { return _isCheckingRingsValid; }
            set { _isCheckingRingsValid = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a collection of <see cref="IGeometry"/>s to be polygonized.
        /// May be called multiple times.
        /// Any dimension of Geometry may be added;
        /// the constituent linework will be extracted and used.
        /// </summary>
        /// <param name="geomList">A list of <c>Geometry</c>s with linework to be polygonized.</param>
        public void Add(ICollection<IGeometry> geomList)
        {
            foreach (var geometry in geomList)
                Add(geometry);
        }

        /// <summary>
        /// Adds a <see cref="IGeometry"/> to the linework to be polygonized.
        /// May be called multiple times.
        /// Any dimension of Geometry may be added;
        /// the constituent linework will be extracted and used
        /// </summary>
        /// <param name="g">A <c>Geometry</c> with linework to be polygonized.</param>
		public void Add(IGeometry g)
        {
            g.Apply(_lineStringAdder);
        }

        /// <summary>
        /// Adds a  to the graph of polygon edges.
        /// </summary>
        /// <param name="line">The <see cref="ILineString"/> to add.</param>
        private void Add(ILineString line)
        {
            // create a new graph using the factory from the input Geometry
            if (_graph == null)
                _graph = new PolygonizeGraph(line.Factory);
            _graph.AddEdge(line);
        }

        private static void AssignHolesToShells(IEnumerable<EdgeRing> holeList, IList<EdgeRing> shellList)
        {
            foreach (EdgeRing holeEdgeRing in holeList)
                AssignHoleToShell(holeEdgeRing, shellList);
        }

        private static void AssignHoleToShell(EdgeRing holeEdgeRing, IList<EdgeRing> shellList)
        {
            var shell = EdgeRing.FindEdgeRingContaining(holeEdgeRing, shellList);
            if (shell != null)
                shell.AddHole(holeEdgeRing.Ring);
        }

        private void FindShellsAndHoles(IEnumerable<EdgeRing> edgeRingList)
        {
            _holeList = new List<EdgeRing>();
            _shellList = new List<EdgeRing>();
            foreach (var er in edgeRingList)
            {
                if (er.IsHole)
                    _holeList.Add(er);
                else _shellList.Add(er);

            }
        }

        private static void FindValidRings(IEnumerable<EdgeRing> edgeRingList, ICollection<EdgeRing> validEdgeRingList, ICollection<IGeometry> invalidRingList)
        {
            foreach (var er in edgeRingList)
            {
                if (er.IsValid)
                    validEdgeRingList.Add(er);
                else invalidRingList.Add(er.LineString);
            }
        }

        /// <summary>
        /// Gets the list of cut edges found during polygonization.
        /// </summary>
        public ICollection<ILineString> GetCutEdges()
        {
            Polygonize();
            return _cutEdges;
        }

        /// <summary> 
        /// Gets the list of dangling lines found during polygonization.
        /// </summary>
        public ICollection<ILineString> GetDangles()
        {
            Polygonize();
            return _dangles;
        }

        /// <summary>
        /// Gets the list of lines forming invalid rings found during polygonization.
        /// </summary>
        public IList<IGeometry> GetInvalidRingLines()
        {
            Polygonize();
            return _invalidRingLines;
        }

        /// <summary>
        /// Gets the list of polygons formed by the polygonization.
        /// </summary>        
        public ICollection<IGeometry> GetPolygons()
        {
            Polygonize();
            return _polyList;
        }

        /// <summary>
        /// Performs the polygonization, if it has not already been carried out.
        /// </summary>
        private void Polygonize()
        {
            // check if already computed
            if (_polyList != null)
                return;

            _polyList = new List<IGeometry>();

            // if no geometries were supplied it's possible that graph is null
            if (_graph == null)
                return;

            _dangles = _graph.DeleteDangles();
            _cutEdges = _graph.DeleteCutEdges();
            var edgeRingList = _graph.GetEdgeRings();

            IList<EdgeRing> validEdgeRingList = new List<EdgeRing>();
            _invalidRingLines = new List<IGeometry>();
            if (IsCheckingRingsValid)
                FindValidRings(edgeRingList, validEdgeRingList, _invalidRingLines);
            else validEdgeRingList = edgeRingList;

            FindShellsAndHoles(validEdgeRingList);
            AssignHolesToShells(_holeList, _shellList);

            _polyList = new List<IGeometry>();
            foreach (EdgeRing er in _shellList)
                _polyList.Add(er.Polygon);
        }

        #endregion

        #region Classes

        /// <summary>
        /// Adds every linear element in a <see cref="IGeometry"/> into the polygonizer graph.
        /// </summary>
        private class LineStringAdder : IGeometryComponentFilter
        {
            #region Fields

            private readonly Polygonizer _container;

            #endregion

            #region Constructors

            public LineStringAdder(Polygonizer container)
            {
                _container = container;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Filters all <see cref="ILineString"/> geometry instances
            /// </summary>
            /// <param name="g">The geometry instance</param>
            public void Filter(IGeometry g)
            {
                var lineString = g as ILineString;
                if (lineString != null)
                    _container.Add(lineString);
            }

            #endregion
        }

        #endregion
    }
}