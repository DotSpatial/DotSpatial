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
using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Utilities;
using Wintellect.PowerCollections;

namespace DotSpatial.Topology.Index.Strtree
{
    /// <summary>
    /// Base class for STRtree and SIRtree. STR-packed R-trees are described in:
    /// P. Rigaux, Michel Scholl and Agnes Voisard. <i>Spatial Databases With
    /// Application To GIS</i>. Morgan Kaufmann, San Francisco, 2002.
    /// <para>
    /// This implementation is based on <see cref="IBoundable{T,TItem}"/>s rather than just <see cref="AbstractNode{T, TItem}"/>s,
    /// because the STR algorithm operates on both nodes and
    /// data, both of which are treated as <see cref="IBoundable{T, TItem}"/>s.
    /// </para>
    /// </summary>
    [Serializable]
    public abstract class AbstractStRtree<T, TItem>
        where T: IIntersectable<T>, IExpandable<T>
    {
        #region Fields

        private readonly object _buildLock = new object();
        private readonly int _nodeCapacity;
        private volatile bool _built, _building;
        /**
         * Set to <tt>null</tt> when index is built, to avoid retaining memory.
         */
        private List<IBoundable<T, TItem>> _itemBoundables = new List<IBoundable<T,TItem>>();
        private volatile AbstractNode<T, TItem> _root;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an AbstractSTRtree with the specified maximum number of child
        /// nodes that a node may have.
        /// </summary>
        /// <param name="nodeCapacity"></param>
        protected AbstractStRtree(int nodeCapacity)
        {
            Assert.IsTrue(nodeCapacity > 1, "Node capacity must be greater than 1");
            _nodeCapacity = nodeCapacity;
        }

        #endregion

        #region Interfaces

        /// <returns>
        /// A test for intersection between two bounds, necessary because subclasses
        /// of AbstractSTRtree have different implementations of bounds.
        /// </returns>
        protected interface IIntersectsOp
        {
            #region Methods

            /// <summary>
            /// For STRtrees, the bounds will be Envelopes;
            /// for SIRtrees, Intervals;
            /// for other subclasses of AbstractSTRtree, some other class.
            /// </summary>
            /// <param name="aBounds">The bounds of one spatial object.</param>
            /// <param name="bBounds">The bounds of another spatial object.</param>
            /// <returns>Whether the two bounds intersect.</returns>
            bool Intersects(T aBounds, T bBounds);

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements in the tree
        /// </summary>
        public virtual int Count
        {
            get
            {
                if (IsEmpty)
                    return 0;

                Build();
                return GetSize(_root);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Depth
        {
            get
            {

                if (IsEmpty)
                {
                    return 0;
                }
                Build();
                return _itemBoundables.Count == 0 ? 0 : GetDepth(_root);
            }
        }

        /// <summary>
        /// Tests whether the index contains any items.
        /// This method does not build the index,
        /// so items can still be inserted after it has been called.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (!_built) return _itemBoundables.Count == 0;
                return Root.IsEmpty;
            }
        }

        /// <summary> 
        /// Returns the maximum number of child nodes that a node may have.
        /// </summary>
        public int NodeCapacity
        {
            get { return _nodeCapacity; }
        }

        /// <returns>
        /// A test for intersection between two bounds, necessary because subclasses
        /// of AbstractSTRtree have different implementations of bounds.
        /// </returns>
        protected abstract IIntersectsOp IntersectsOp { get; }

        protected AbstractNode<T, TItem> Root
        {
            get
            {
                Build();
                return _root;
            }
            set { _root = value; }
        }

        #endregion

        #region Methods

        protected IList<IBoundable<T, TItem>> BoundablesAtLevel(int level)
        {
            var boundables = new List<IBoundable<T, TItem>>();
            BoundablesAtLevel(level, _root, boundables);
            return boundables;
        }

        private static void BoundablesAtLevel(int level, AbstractNode<T, TItem> top, ICollection<IBoundable<T, TItem>>  boundables)
        {
            Assert.IsTrue(level > -2);
            if (top.Level == level)
            {
                boundables.Add(top);
                return;
            }
            foreach (var boundable in top.ChildBoundables )
            {
                if (boundable is AbstractNode<T, TItem>)
                    BoundablesAtLevel(level, (AbstractNode<T, TItem>)boundable, boundables);
                else
                {
                    Assert.IsTrue(boundable is ItemBoundable<T, TItem>);
                    if (level == -1)
                        boundables.Add(boundable);
                }
            }
        }

        /// <summary>
        /// Creates parent nodes, grandparent nodes, and so forth up to the root
        /// node, for the data that has been inserted into the tree. Can only be
        /// called once, and thus can be called only after all of the data has been
        /// inserted into the tree.
        /// </summary>
        public void Build()
        {
            if (_built)
                return;

            lock (_buildLock)
                if (!_built)
                {
                    _building = true;
                    _root = (_itemBoundables.Count == 0)
                                ? CreateNode(0)
                                : CreateHigherLevels(_itemBoundables, -1);

                    // the item list is no longer needed
                    _itemBoundables = null;
                    _built = true;
                    _building = false;
                }
        }

        protected static int CompareDoubles(double a, double b)
        {
            return a.CompareTo(b);
        }

        /// <summary>
        /// Creates the levels higher than the given level.
        /// </summary>
        /// <param name="boundablesOfALevel">The level to build on.</param>
        /// <param name="level">the level of the Boundables, or -1 if the boundables are item
        /// boundables (that is, below level 0).</param>
        /// <returns>The root, which may be a ParentNode or a LeafNode.</returns>
        private AbstractNode<T, TItem> CreateHigherLevels(IList<IBoundable<T, TItem>>  boundablesOfALevel, int level)
        {
            Assert.IsTrue(boundablesOfALevel.Count != 0);
            var parentBoundables = CreateParentBoundables(boundablesOfALevel, level + 1);
            if (parentBoundables.Count == 1)
                return (AbstractNode<T, TItem>)parentBoundables[0];
            return CreateHigherLevels(parentBoundables, level + 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected abstract AbstractNode<T, TItem> CreateNode(int level);

        /// <summary>
        /// Sorts the childBoundables then divides them into groups of size M, where
        /// M is the node capacity.
        /// </summary>
        /// <param name="childBoundables"></param>
        /// <param name="newLevel"></param>
        protected virtual IList<IBoundable<T, TItem>> CreateParentBoundables(IList<IBoundable<T, TItem>> childBoundables, int newLevel)
        {
            Assert.IsTrue(childBoundables.Count != 0);
            var parentBoundables = new List<IBoundable<T, TItem>>();
            parentBoundables.Add(CreateNode(newLevel));
            var castedChildBoundables = PlatformUtilityEx.CastPlatform(childBoundables);
            var sortedChildBoundables =
                new BigList<IBoundable<T, TItem>>(castedChildBoundables);
            sortedChildBoundables.Sort(GetComparer());
            foreach (IBoundable<T, TItem> childBoundable in sortedChildBoundables)
            {
                if (LastNode(parentBoundables).ChildBoundables.Count == NodeCapacity)
                    parentBoundables.Add(CreateNode(newLevel));
                LastNode(parentBoundables).AddChildBoundable(childBoundable);
            }
            return parentBoundables;
        }

        protected abstract IComparer<IBoundable<T, TItem>> GetComparer();

        protected int GetDepth(AbstractNode<T, TItem> node)
        {
            var maxChildDepth = 0;
            foreach (var childBoundable in node.ChildBoundables)
            {
                if (!(childBoundable is AbstractNode<T, TItem>))
                    continue;
                var childDepth = GetDepth((AbstractNode<T, TItem>)childBoundable);
                if (childDepth > maxChildDepth)
                    maxChildDepth = childDepth;
            }
            return maxChildDepth + 1;
        }

        protected int GetSize(AbstractNode<T, TItem> node)
        {
            var size = 0;
            foreach (var childBoundable in node.ChildBoundables)
            {
                if (childBoundable is AbstractNode<T, TItem>)
                    size += GetSize((AbstractNode<T, TItem>)childBoundable);
                else if (childBoundable is ItemBoundable<T, TItem>)
                    size += 1;
            }
            return size;
        }

        protected void Insert(T bounds, TItem item)
        {
            Assert.IsTrue(!(_built || _building), "Cannot insert items into an STR packed R-tree after it has been built.");
            _itemBoundables.Add(new ItemBoundable<T, TItem>(bounds, item));
        }

        /// <summary>
        /// Gets a tree structure (as a nested list) 
        /// corresponding to the structure of the items and nodes in this tree.
        /// The returned Lists contain either Object items, 
        /// or Lists which correspond to subtrees of the tree
        /// Subtrees which do not contain any items are not included.
        /// Builds the tree if necessary.
        /// </summary>
        /// <returns>a List of items and/or Lists</returns>
        public IList<object> ItemsTree()
        {
            Build();

            var valuesTree = ItemsTree(_root);
            return valuesTree ?? new List<object>();
        }

        private static IList<object> ItemsTree(AbstractNode<T, TItem> node)
        {
            var valuesTreeForNode = new List<object>();

            foreach (var childBoundable in node.ChildBoundables)
            {
                if (childBoundable is AbstractNode<T, TItem>)
                {
                    var valuesTreeForChild = ItemsTree((AbstractNode<T, TItem>)childBoundable);
                    // only add if not null (which indicates an item somewhere in this tree
                    if (valuesTreeForChild != null)
                        valuesTreeForNode.Add(valuesTreeForChild);
                }
                else if (childBoundable is ItemBoundable<T, TItem>)
                    valuesTreeForNode.Add(((ItemBoundable<T, TItem>)childBoundable).Item);
                else Assert.ShouldNeverReachHere();
            }
            
            return valuesTreeForNode.Count <= 0 ? null : valuesTreeForNode;
        }

        protected AbstractNode<T, TItem> LastNode(IList<IBoundable<T, TItem>>  nodes)
        {
            return (AbstractNode<T, TItem>)nodes[nodes.Count - 1];
        }

        /// <summary>
        /// Also builds the tree, if necessary.
        /// </summary>
        /// <param name="searchBounds"></param>
        protected IList<TItem> Query(T searchBounds)
        {
            Build();

            var matches = new List<TItem>();
            if (IsEmpty)
                return matches;

            if (IntersectsOp.Intersects(_root.Bounds, searchBounds))
                Query(searchBounds, _root, matches);
            return matches;
        }

        protected void Query(T searchBounds, IItemVisitor<TItem> visitor)
        {
            Build();

            if (IsEmpty)
                return;

            if (IntersectsOp.Intersects(_root.Bounds, searchBounds))
                Query(searchBounds, _root, visitor);
        }

        private void Query(T searchBounds, AbstractNode<T, TItem> node, IList<TItem> matches)
        {
            foreach (var childBoundable in node.ChildBoundables)
            {
                if (!IntersectsOp.Intersects(childBoundable.Bounds, searchBounds))
                    continue;

                if (childBoundable is AbstractNode<T, TItem>)
                    Query(searchBounds, (AbstractNode<T, TItem>)childBoundable, matches);
                else if (childBoundable is ItemBoundable<T, TItem>)
                    matches.Add(((ItemBoundable<T, TItem>)childBoundable).Item);
                else Assert.ShouldNeverReachHere();
            }
        }

        private void Query(T searchBounds, AbstractNode<T, TItem> node, IItemVisitor<TItem> visitor)
        {
            foreach (var childBoundable in node.ChildBoundables)
            {
                if (!IntersectsOp.Intersects(childBoundable.Bounds, searchBounds))
                    continue;
                if (childBoundable is AbstractNode<T, TItem>)
                    Query(searchBounds, (AbstractNode<T, TItem>)childBoundable, visitor);
                else if (childBoundable is ItemBoundable<T, TItem>)
                    visitor.VisitItem(((ItemBoundable<T, TItem>)childBoundable).Item);
                else Assert.ShouldNeverReachHere();
            }
        }

        /// <summary>
        /// Removes an item from the tree.
        /// (Builds the tree, if necessary.)
        /// </summary>
        /// <param name="searchBounds"></param>
        /// <param name="item"></param>
        protected bool Remove(T searchBounds, TItem item)
        {
            Build();
            return IntersectsOp.Intersects(_root.Bounds, searchBounds) && Remove(searchBounds, _root, item);
        }

        private bool Remove(T searchBounds, AbstractNode<T, TItem> node, TItem item)
        {
            // first try removing item from this node
            var found = RemoveItem(node, item);
            if (found)
                return true;
            AbstractNode<T, TItem> childToPrune = null;
            // next try removing item from lower nodes
            foreach (var childBoundable in node.ChildBoundables)
            {
                if (!IntersectsOp.Intersects(childBoundable.Bounds, searchBounds))
                    continue;
                if (!(childBoundable is AbstractNode<T, TItem>))
                    continue;
                found = Remove(searchBounds, (AbstractNode<T, TItem>)childBoundable, item);
                // if found, record child for pruning and exit
                if (!found)
                    continue;
                childToPrune = (AbstractNode<T, TItem>)childBoundable;
                break;
            }
            // prune child if possible
            if (childToPrune != null)
                if (childToPrune.ChildBoundables.Count == 0)
                    node.ChildBoundables.Remove(childToPrune);
            return found;
        }

        private static bool RemoveItem(AbstractNode<T, TItem> node, TItem item)
        {
            IBoundable<T, TItem> childToRemove = null;
            for (var i = node.ChildBoundables.GetEnumerator(); i.MoveNext(); )
            {
                var childBoundable = i.Current as ItemBoundable<T, TItem>;
                if (childBoundable != null && ReferenceEquals(childBoundable.Item, item))
                        childToRemove = childBoundable;
            }
            if (childToRemove != null)
            {
                node.ChildBoundables.Remove(childToRemove);
                return true;
            }
            return false;
        }

        #endregion
    }
}