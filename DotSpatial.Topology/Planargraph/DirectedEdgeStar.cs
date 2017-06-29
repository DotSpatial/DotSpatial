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

namespace DotSpatial.Topology.Planargraph
{
    /// <summary>
    /// A sorted collection of <c>DirectedEdge</c>s which leave a <c>Node</c>
    /// in a <c>PlanarGraph</c>.
    /// </summary>
    public class DirectedEdgeStar
    {
        /// <summary>
        /// The underlying list of outgoing DirectedEdges.
        /// </summary>
        private IList _outEdges = new ArrayList();

        private bool _sorted;

        /// <summary>
        /// The underlying list of outgoing Directed Edges
        /// </summary>
        protected IList OutEdges
        {
            get { return _outEdges; }
            set { _outEdges = value; }
        }

        /// <summary>
        /// Returns the number of edges around the Node associated with this DirectedEdgeStar.
        /// </summary>
        public virtual int Degree
        {
            get
            {
                return _outEdges.Count;
            }
        }

        /// <summary>
        /// Returns the coordinate for the node at wich this star is based.
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                IEnumerator it = GetEnumerator();
                if (!it.MoveNext())
                    return null;
                DirectedEdge e = (DirectedEdge)it.Current;
                return e.Coordinate;
            }
        }

        /// <summary>
        /// Returns the DirectedEdges, in ascending order by angle with the positive x-axis.
        /// </summary>
        public virtual IList Edges
        {
            get
            {
                SortEdges();
                return _outEdges;
            }
        }

        /// <summary>
        /// Adds a new member to this DirectedEdgeStar.
        /// </summary>
        /// <param name="de"></param>
        public virtual void Add(DirectedEdge de)
        {
            _outEdges.Add(de);
            _sorted = false;
        }

        /// <summary>
        /// Drops a member of this DirectedEdgeStar.
        /// </summary>
        /// <param name="de"></param>
        public virtual void Remove(DirectedEdge de)
        {
            _outEdges.Remove(de);
        }

        /// <summary>
        /// Returns an Iterator over the DirectedEdges, in ascending order by angle with the positive x-axis.
        /// </summary>
        public virtual IEnumerator GetEnumerator()
        {
            SortEdges();
            return _outEdges.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        private void SortEdges()
        {
            if (!_sorted)
            {
                ArrayList list = (ArrayList)_outEdges;
                list.Sort();
                _sorted = true;
            }
        }

        /// <summary>
        /// Returns the zero-based index of the given Edge, after sorting in ascending order
        /// by angle with the positive x-axis.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public virtual int GetIndex(Edge edge)
        {
            SortEdges();
            for (int i = 0; i < _outEdges.Count; i++)
            {
                DirectedEdge de = (DirectedEdge)_outEdges[i];
                if (de.Edge == edge)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns the zero-based index of the given DirectedEdge, after sorting in ascending order
        /// by angle with the positive x-axis.
        /// </summary>
        /// <param name="dirEdge"></param>
        /// <returns></returns>
        public virtual int GetIndex(DirectedEdge dirEdge)
        {
            SortEdges();
            for (int i = 0; i < _outEdges.Count; i++)
            {
                DirectedEdge de = (DirectedEdge)_outEdges[i];
                if (de == dirEdge)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns the remainder when i is divided by the number of edges in this
        /// DirectedEdgeStar.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual int GetIndex(int i)
        {
            int modi = i % _outEdges.Count;
            //I don't think modi can be 0 (assuming i is positive) [Jon Aquino 10/28/2003]
            if (modi < 0)
                modi += _outEdges.Count;
            return modi;
        }

        /// <summary>
        /// Returns the DirectedEdge on the left-hand side of the given DirectedEdge (which
        /// must be a member of this DirectedEdgeStar).
        /// </summary>
        /// <param name="dirEdge"></param>
        /// <returns></returns>
        public virtual DirectedEdge GetNextEdge(DirectedEdge dirEdge)
        {
            int i = GetIndex(dirEdge);
            return (DirectedEdge)_outEdges[GetIndex(i + 1)];
        }
    }
}