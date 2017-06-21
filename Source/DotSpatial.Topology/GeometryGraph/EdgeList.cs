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
using System.IO;
using DotSpatial.Topology.Index;
using DotSpatial.Topology.Index.Quadtree;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A EdgeList is a list of Edges.  It supports locating edges
    /// that are pointwise equals to a target edge.
    /// </summary>
    public class EdgeList
    {
        private readonly IList _edges = new ArrayList();

        /// <summary>
        /// An index of the edges, for fast lookup.
        /// a Quadtree is used, because this index needs to be dynamic
        /// (e.g. allow insertions after queries).
        /// An alternative would be to use an ordered set based on the values
        /// of the edge coordinates.
        /// </summary>
        private readonly ISpatialIndex _index = new Quadtree();

        /// <summary>
        ///
        /// </summary>
        public virtual IList Edges
        {
            get
            {
                return _edges;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Edge this[int index]
        {
            get
            {
                return Get(index);
            }
        }

        /// <summary>
        /// Remove the selected Edge element from the list if present.
        /// </summary>
        /// <param name="e">Edge element to remove from list</param>
        public virtual void Remove(Edge e)
        {
            _edges.Remove(e);
        }

        /// <summary>
        /// Insert an edge unless it is already in the list.
        /// </summary>
        /// <param name="e"></param>
        public virtual void Add(Edge e)
        {
            _edges.Add(e);
            _index.Insert(e.Envelope, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edgeColl"></param>
        public virtual void AddAll(ICollection edgeColl)
        {
            for (IEnumerator i = edgeColl.GetEnumerator(); i.MoveNext(); )
                Add((Edge)i.Current);
        }

        // <FIX> fast lookup for edges
        /// <summary>
        /// If there is an edge equal to e already in the list, return it.
        /// Otherwise return null.
        /// </summary>
        /// <param name="e"></param>
        /// <returns>
        /// equal edge, if there is one already in the list,
        /// null otherwise.
        /// </returns>
        public virtual Edge FindEqualEdge(Edge e)
        {
            ICollection testEdges = _index.Query(e.Envelope);
            for (IEnumerator i = testEdges.GetEnumerator(); i.MoveNext(); )
            {
                Edge testEdge = (Edge)i.Current;
                if (testEdge.Equals(e))
                    return testEdge;
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEnumerator()
        {
            return _edges.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual Edge Get(int i)
        {
            return (Edge)_edges[i];
        }

        /// <summary>
        /// If the edge e is already in the list, return its index.
        /// </summary>
        /// <param name="e"></param>
        /// <returns>
        /// Index, if e is already in the list,
        /// -1 otherwise.
        /// </returns>
        public virtual int FindEdgeIndex(Edge e)
        {
            for (int i = 0; i < _edges.Count; i++)
                if (_edges[i].Equals(e))
                    return i;
            return -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void Write(StreamWriter outstream)
        {
            outstream.Write("MULTILINESTRING ( ");
            for (int j = 0; j < _edges.Count; j++)
            {
                Edge e = (Edge)_edges[j];
                if (j > 0)
                    outstream.Write(",");
                outstream.Write("(");
                IList<Coordinate> pts = e.Coordinates;
                for (int i = 0; i < pts.Count; i++)
                {
                    if (i > 0)
                        outstream.Write(",");
                    outstream.Write(pts[i].X + " " + pts[i].Y);
                }
                outstream.WriteLine(")");
            }
            outstream.Write(")  ");
        }
    }
}