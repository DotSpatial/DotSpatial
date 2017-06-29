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
using DotSpatial.Topology.Planargraph;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Operation.Linemerge
{
    /// <summary>
    /// Sews together a set of fully noded LineStrings. Sewing stops at nodes of degree 1
    /// or 3 or more -- the exception is an isolated loop, which only has degree-2 nodes,
    /// in which case a node is simply chosen as a starting point. The direction of each
    /// merged LineString will be that of the majority of the LineStrings from which it
    /// was derived.
    /// Any dimension of Geometry is handled -- the constituent linework is extracted to
    /// form the edges. The edges must be correctly noded; that is, they must only meet
    /// at their endpoints.  The LineMerger will still run on incorrectly noded input
    /// but will not form polygons from incorrected noded edges.
    /// </summary>
    public class LineMerger
    {
        private readonly LineMergeGraph _graph = new LineMergeGraph();
        private IList _edgeStrings;
        private IGeometryFactory _factory;
        private IList _mergedLineStrings;

        /// <summary>
        /// Returns the LineStrings built by the merging process.
        /// </summary>
        public virtual IList MergedLineStrings
        {
            get
            {
                Merge();
                return _mergedLineStrings;
            }
        }

        /// <summary>
        /// Adds a collection of Geometries to be processed. May be called multiple times.
        /// Any dimension of Geometry may be added; the constituent linework will be
        /// extracted.
        /// </summary>
        /// <param name="geometries"></param>
        public virtual void Add(IList geometries)
        {
            IEnumerator i = geometries.GetEnumerator();
            while (i.MoveNext())
            {
                Geometry geometry = (Geometry)i.Current;
                Add(geometry);
            }
        }

        /// <summary>
        /// Adds a Geometry to be processed. May be called multiple times.
        /// Any dimension of Geometry may be added; the constituent linework will be
        /// extracted.
        /// </summary>
        /// <param name="geometry"></param>
        public virtual void Add(Geometry geometry)
        {
            geometry.Apply(new AnonymousGeometryComponentFilterImpl(this));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lineString"></param>
        private void Add(LineString lineString)
        {
            if (_factory == null)
                _factory = lineString.Factory;
            _graph.AddEdge(lineString);
        }

        /// <summary>
        ///
        /// </summary>
        private void Merge()
        {
            if (_mergedLineStrings != null)
                return;
            _edgeStrings = new ArrayList();
            BuildEdgeStringsForObviousStartNodes();
            BuildEdgeStringsForIsolatedLoops();
            _mergedLineStrings = new ArrayList();
            for (IEnumerator i = _edgeStrings.GetEnumerator(); i.MoveNext(); )
            {
                EdgeString edgeString = (EdgeString)i.Current;
                _mergedLineStrings.Add(edgeString.ToLineString());
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void BuildEdgeStringsForObviousStartNodes()
        {
            BuildEdgeStringsForNonDegree2Nodes();
        }

        /// <summary>
        ///
        /// </summary>
        private void BuildEdgeStringsForIsolatedLoops()
        {
            BuildEdgeStringsForUnprocessedNodes();
        }

        /// <summary>
        ///
        /// </summary>
        private void BuildEdgeStringsForUnprocessedNodes()
        {
            IEnumerator i = _graph.Nodes.GetEnumerator();
            while (i.MoveNext())
            {
                Node node = (Node)i.Current;
                if (!node.IsMarked)
                {
                    Assert.IsTrue(node.Degree == 2);
                    BuildEdgeStringsStartingAt(node);
                    node.IsMarked = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void BuildEdgeStringsForNonDegree2Nodes()
        {
            IEnumerator i = _graph.Nodes.GetEnumerator();
            while (i.MoveNext())
            {
                Node node = (Node)i.Current;
                if (node.Degree != 2)
                {
                    BuildEdgeStringsStartingAt(node);
                    node.IsMarked = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        private void BuildEdgeStringsStartingAt(Node node)
        {
            IEnumerator i = node.OutEdges.GetEnumerator();
            while (i.MoveNext())
            {
                LineMergeDirectedEdge directedEdge = (LineMergeDirectedEdge)i.Current;
                if (directedEdge.Edge.IsMarked)
                    continue;
                _edgeStrings.Add(BuildEdgeStringStartingWith(directedEdge));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private EdgeString BuildEdgeStringStartingWith(LineMergeDirectedEdge start)
        {
            EdgeString edgeString = new EdgeString(_factory);
            LineMergeDirectedEdge current = start;
            do
            {
                edgeString.Add(current);
                current.Edge.IsMarked = true;
                current = current.Next;
            }
            while (current != null && current != start);
            return edgeString;
        }

        #region Nested type: AnonymousGeometryComponentFilterImpl

        /// <summary>
        ///
        /// </summary>
        private class AnonymousGeometryComponentFilterImpl : IGeometryComponentFilter
        {
            private readonly LineMerger _container;

            /// <summary>
            ///
            /// </summary>
            /// <param name="container"></param>
            public AnonymousGeometryComponentFilterImpl(LineMerger container)
            {
                _container = container;
            }

            #region IGeometryComponentFilter Members

            /// <summary>
            ///
            /// </summary>
            /// <param name="component"></param>
            public void Filter(IGeometry component)
            {
                if (component is LineString)
                    _container.Add((LineString)component);
            }

            #endregion
        }

        #endregion
    }
}