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
    /// The base class for all graph component classes.
    /// Maintains flags of use in generic graph algorithms.
    /// Provides two flags:
    /// marked - typically this is used to indicate a state that persists
    /// for the course of the graph's lifetime.  For instance, it can be
    /// used to indicate that a component has been logically deleted from the graph.
    /// visited - this is used to indicate that a component has been processed
    /// or visited by an single graph algorithm.  For instance, a breadth-first traversal of the
    /// graph might use this to indicate that a node has already been traversed.
    /// The visited flag may be set and cleared many times during the lifetime of a graph.
    /// </summary>
    public abstract class GraphComponent
    {
        #region Private variables

        private bool _isMarked;
        private bool _isVisited;

        #endregion

        #region Static

        /// <summary>
        /// Sets the <see cref="GraphComponent.IsVisited" /> state
        /// for all <see cref="GraphComponent" />s in an <see cref="IEnumerator" />.
        /// </summary>
        /// <param name="i">A <see cref="IEnumerator" /> to scan.</param>
        /// <param name="visited">The state to set the <see cref="GraphComponent.IsVisited" /> flag to.</param>
        public static void SetVisited(IEnumerator i, bool visited)
        {
            while (i.MoveNext())
            {
                GraphComponent comp = (GraphComponent)i.Current;
                comp.IsVisited = visited;
            }
        }

        /// <summary>
        /// Sets the <see cref="GraphComponent.IsMarked" /> state
        /// for all <see cref="GraphComponent" />s in an <see cref="IEnumerator" />.
        /// </summary>
        /// <param name="i">A <see cref="IEnumerator" /> to scan.</param>
        /// <param name="marked">The state to set the <see cref="GraphComponent.IsMarked" /> flag to.</param>
        public static void SetMarked(IEnumerator i, bool marked)
        {
            while (i.MoveNext())
            {
                GraphComponent comp = (GraphComponent)i.Current;
                comp.IsMarked = marked;
            }
        }

        /// <summary>
        /// Finds the first <see cref="GraphComponent" />
        /// in a <see cref="IEnumerator" /> set
        /// which has the specified <see cref="GraphComponent.IsVisited" /> state.
        /// </summary>
        /// <param name="i">A <see cref="IEnumerator" /> to scan.</param>
        /// <param name="visitedState">The <see cref="GraphComponent.IsVisited" /> state to test.</param>
        /// <returns>The first <see cref="GraphComponent" /> found, or <c>null</c> if none found.</returns>
        public static GraphComponent GetComponentWithVisitedState(IEnumerator i, bool visitedState)
        {
            while (i.MoveNext())
            {
                GraphComponent comp = (GraphComponent)i.Current;
                if (comp.IsVisited == visitedState)
                    return comp;
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Tests if a component has been visited during the course of a graph algorithm.
        /// </summary>
        public virtual bool IsVisited
        {
            get
            {
                return _isVisited;
            }
            set
            {
                _isVisited = true;
            }
        }

        /// <summary>
        /// Tests if a component has been marked at some point during the processing
        /// involving this graph.
        /// </summary>
        public virtual bool IsMarked
        {
            get
            {
                return _isMarked;
            }
            set
            {
                _isMarked = true;
            }
        }

        /// <summary>
        /// Tests whether this component has been removed from its containing graph.
        /// </summary>
        public abstract bool IsRemoved { get; }

        /*

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is GraphComponent))
                return false;
            if (!base.Equals(obj))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            GraphComponent other = obj as GraphComponent;
            if (IsMarked != other.IsMarked)
                return false;
            if (IsVisited != other.IsVisited)
                return false;
            if (IsRemoved != other.IsRemoved)
                return false;
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int result = 29 * IsMarked.GetHashCode();
            result += 14 + 29 * IsVisited.GetHashCode();
            result += 14 + 29 * IsRemoved.GetHashCode();
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator ==(GraphComponent o1, GraphComponent o2)
        {
            return Object.Equals(o1, o2);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator !=(GraphComponent o1, GraphComponent o2)
        {
            return !(o1 == o2);
        }

        */
    }
}