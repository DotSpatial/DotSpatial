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
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Index.Strtree
{
    /// <summary>
    /// A node of the STR tree. The children of this node are either more nodes
    /// (AbstractNodes) or real data (ItemBoundables). If this node contains real data
    /// (rather than nodes), then we say that this node is a "leaf node".
    /// </summary>
    public abstract class AbstractNode : IBoundable
    {
        private readonly ArrayList _childBoundables = new ArrayList();
        private readonly int _level;
        private object _bounds;

        /// <summary>
        /// Constructs an AbstractNode at the given level in the tree
        /// </summary>
        /// <param name="level">
        /// 0 if this node is a leaf, 1 if a parent of a leaf, and so on; the
        /// root node will have the highest level.
        /// </param>
        protected AbstractNode(int level)
        {
            _level = level;
        }

        /// <summary>
        /// Returns either child AbstractNodes, or if this is a leaf node, real data (wrapped
        /// in ItemBoundables).
        /// </summary>
        public virtual IList ChildBoundables
        {
            get
            {
                return _childBoundables;
            }
        }

        /// <summary>
        /// Returns 0 if this node is a leaf, 1 if a parent of a leaf, and so on; the
        /// root node will have the highest level.
        /// </summary>
        public virtual int Level
        {
            get
            {
                return _level;
            }
        }

        #region IBoundable Members

        /// <summary>
        /// Returns object fromComputeBounds()
        /// </summary>
        public virtual object Bounds
        {
            get
            {
                if (_bounds == null)
                {
                    _bounds = ComputeBounds();
                }
                return _bounds;
            }
        }

        #endregion

        /// <summary>
        /// Returns a representation of space that encloses this Boundable,
        /// preferably not much bigger than this Boundable's boundary yet fast to
        /// test for intersection with the bounds of other Boundables. The class of
        /// object returned depends on the subclass of AbstractSTRtree.
        /// </summary>
        /// <returns>
        /// An Envelope (for STRtrees), an Interval (for SIRtrees), or other
        /// object (for other subclasses of AbstractSTRtree).
        /// </returns>
        protected abstract object ComputeBounds();

        /// <summary>
        /// Adds either an AbstractNode, or if this is a leaf node, a data object
        /// (wrapped in an ItemBoundable).
        /// </summary>
        /// <param name="childBoundable"></param>
        public virtual void AddChildBoundable(IBoundable childBoundable)
        {
            Assert.IsTrue(_bounds == null);
            _childBoundables.Add(childBoundable);
        }
    }
}