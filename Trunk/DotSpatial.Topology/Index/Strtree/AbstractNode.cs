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

using System;
using System.Collections.Generic;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Index.Strtree
{
    /// <summary> 
    /// A node of an <see cref="AbstractStRtree{T,TItem}"/>. A node is one of:
    /// <list type="Bullet">
    /// <item>empty</item>
    /// <item>an <i>interior node</i> containing child <see cref="AbstractNode{T, TItem}"/>s</item>
    /// <item>a <i>leaf node</i> containing data items (<see cref="ItemBoundable{T, TItem}"/>s).</item>
    /// </list>
    /// A node stores the bounds of its children, and its level within the index tree.
    /// </summary>
    [Serializable]
    public abstract class AbstractNode<T, TItem> : IBoundable<T, TItem> where T : IIntersectable<T>, IExpandable<T>
    {
        #region Fields

        private readonly List<IBoundable<T, TItem>> _childBoundables = new List<IBoundable<T, TItem>>();
        private T _bounds;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an AbstractNode at the given level in the tree
        /// </summary>
        /// <param name="level">
        /// 0 if this node is a leaf, 1 if a parent of a leaf, and so on; the
        /// root node will have the highest level.
        /// </param>
        protected AbstractNode(int level)
        {
            Level = level;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the bounds of this node
        /// </summary>
        public T Bounds
        {
            get
            {
                if (_bounds == null) _bounds = ComputeBounds();
                return _bounds;
            }
        }

        /// <summary> 
        /// Returns either child <see cref="AbstractNode{T, TItem}"/>s, or if this is a leaf node, real data (wrapped
        /// in <see cref="ItemBoundable{T, TItem}"/>s).
        /// </summary>
        public IList<IBoundable<T, TItem>> ChildBoundables
        {
            get
            {
                return _childBoundables;
            }
        }

        /// <summary>
        /// Gets the count of the <see cref="IBoundable{T, TItem}"/>s at this node.
        /// </summary>
        public int Count
        {
            get {return _childBoundables.Count;}
        }

        /// <summary>
        /// Tests whether there are any <see cref="IBoundable{T, TItem}"/>s at this node.
        /// </summary>
        public bool IsEmpty
        {
            get { return _childBoundables.Count == 0; }
    }

        public TItem Item { get { return default(TItem); } }

        /// <summary>
        /// Returns 0 if this node is a leaf, 1 if a parent of a leaf, and so on; 
        /// the root node will have the highest level.
        /// </summary>
        public int Level { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds either an AbstractNode, or if this is a leaf node, a data object
        /// (wrapped in an ItemBoundable).
        /// </summary>
        /// <param name="childBoundable"></param>
        public void AddChildBoundable(IBoundable<T, TItem> childBoundable) 
        {
            Assert.IsTrue(_bounds == null);
            _childBoundables.Add(childBoundable);
        }

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
        protected abstract T ComputeBounds();

        #endregion
    }
}