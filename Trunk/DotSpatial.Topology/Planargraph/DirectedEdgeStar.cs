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
using Wintellect.PowerCollections;

namespace DotSpatial.Topology.Planargraph
{
    /// <summary>
    /// A sorted collection of <c>DirectedEdge</c>s which leave a <c>Node</c>
    /// in a <c>PlanarGraph</c>.
    /// </summary>
    public class DirectedEdgeStar
    {
        #region Fields

        /// <summary>
        /// The underlying list of outgoing DirectedEdges.
        /// </summary>
        private readonly BigList<DirectedEdge> _outEdges = new BigList<DirectedEdge>();

        private bool _sorted;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the coordinate for the node at wich this star is based.
        /// </summary>
        public Coordinate Coordinate
        {
            get
            {
                if (_outEdges.Count == 0 || _outEdges[0] == null)
                    return null;
                return _outEdges[0].Coordinate;
            }
        }

        /// <summary>
        /// Returns the number of edges around the Node associated with this DirectedEdgeStar.
        /// </summary>
        public int Degree
        {
            get
            {
                return _outEdges.Count;
            }
        }

        /// <summary>
        /// Returns the DirectedEdges, in ascending order by angle with the positive x-axis.
        /// </summary>
        public IList<DirectedEdge> Edges
        {
            get
            {
                SortEdges();
                return _outEdges;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new member to this DirectedEdgeStar.
        /// </summary>
        /// <param name="de"></param>
        public void Add(DirectedEdge de)
        {            
            _outEdges.Add(de);
            _sorted = false;
        }

        /// <summary>
        /// Returns an Iterator over the DirectedEdges, in ascending order by angle with the positive x-axis.
        /// </summary>
        public IEnumerator<DirectedEdge> GetEnumerator()
        {            
            SortEdges();
            return _outEdges.GetEnumerator();
        }

        /// <summary>
        /// Returns the zero-based index of the given Edge, after sorting in ascending order
        /// by angle with the positive x-axis.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public int GetIndex(Edge edge)
        {
            SortEdges();
            for (int i = 0; i < _outEdges.Count; i++)
            {
                DirectedEdge de = _outEdges[i];
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
        public int GetIndex(DirectedEdge dirEdge)
        {
            SortEdges();
            for (int i = 0; i < _outEdges.Count; i++)
            {
                DirectedEdge de = _outEdges[i];
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
        public int GetIndex(int i)
        {
            int modi = i % _outEdges.Count;
            //I don't think modi can be 0 (assuming i is positive) [Jon Aquino 10/28/2003] 
            if (modi < 0) 
                modi += _outEdges.Count;
            return modi;
        }

        ///<summary>
        /// Returns the <see cref="DirectedEdge"/> on the right-hand (CW) 
        /// side of the given <see cref="DirectedEdge"/>
        /// (which must be a member of this DirectedEdgeStar).
        /// </summary>
        public DirectedEdge GetNextCWEdge(DirectedEdge dirEdge)
        {
            int i = GetIndex(dirEdge);
            return _outEdges[GetIndex(i - 1)];
        }

        /// <summary>
        /// Returns the <see cref="DirectedEdge"/> on the left-hand 
        /// side of the given <see cref="DirectedEdge"/> 
        /// (which  must be a member of this DirectedEdgeStar). 
        /// </summary>
        /// <param name="dirEdge"></param>
        /// <returns></returns>
        public DirectedEdge GetNextEdge(DirectedEdge dirEdge)
        {
            int i = GetIndex(dirEdge);
            return _outEdges[GetIndex(i + 1)];
        }

        /// <summary>
        /// Drops a member of this DirectedEdgeStar.
        /// </summary>
        /// <param name="de"></param>
        public void Remove(DirectedEdge de)
        {
            _outEdges.Remove(de);
        }

        /// <summary>
        ///
        /// </summary>
        private void SortEdges()
        {
            if (!_sorted)
            {
                _outEdges.Sort();
                _sorted = true;                
            }
        }

        #endregion
    }
}