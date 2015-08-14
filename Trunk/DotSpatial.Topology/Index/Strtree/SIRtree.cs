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

namespace DotSpatial.Topology.Index.Strtree
{
    /// <summary>
    /// One-dimensional version of an STR-packed R-tree. SIR stands for
    /// "Sort-Interval-Recursive". STR-packed R-trees are described in:
    /// P. Rigaux, Michel Scholl and Agnes Voisard. Spatial Databases With
    /// Application To GIS. Morgan Kaufmann, San Francisco, 2002.
    /// </summary>
    public class SiRtree<TItem> : AbstractStRtree<Interval, TItem> 
    {
        #region Fields

        private static readonly IComparer<IBoundable<Interval, TItem>> Comparator = new AnnonymousComparerImpl();
        private static readonly IIntersectsOp IntersectsOperation = new AnonymousIntersectsOpImpl();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an SIRtree with the default (10) node capacity.
        /// </summary>
        public SiRtree() : this(10) { }

        /// <summary>
        /// Constructs an SIRtree with the given maximum number of child nodes that
        /// a node may have.
        /// </summary>
        public SiRtree(int nodeCapacity) : base(nodeCapacity) { }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        protected override IIntersectsOp IntersectsOp
        {
            get
            {
                return IntersectsOperation;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected override AbstractNode<Interval, TItem> CreateNode(int level) 
        {                
            return new AnonymousAbstractNodeImpl(level);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override IComparer<IBoundable<Interval, TItem>> GetComparer() 
        {
            return Comparator;
        }

        /// <summary>
        /// Inserts an item having the given bounds into the tree.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="item"></param>
        public void Insert(double x1, double x2, TItem item) 
        {
            Insert(new Interval(Math.Min(x1, x2), Math.Max(x1, x2)), item);
        }

        /// <summary>
        /// Returns items whose bounds intersect the given value.
        /// </summary>
        /// <param name="x"></param>
        public IList<TItem> Query(double x) 
        {
            return Query(x, x);
        }

        /// <summary>
        /// Returns items whose bounds intersect the given bounds.
        /// </summary>
        /// <param name="x1">Possibly equal to x2.</param>
        /// <param name="x2">Possibly equal to x1.</param>
        public IList<TItem> Query(double x1, double x2) 
        {
            return Query(new Interval(Math.Min(x1, x2), Math.Max(x1, x2)));
        }

        #endregion

        #region Classes

        private class AnnonymousComparerImpl : IComparer<IBoundable<Interval, TItem>>
        {
            #region Methods

            public int Compare(IBoundable<Interval, TItem> o1, IBoundable<Interval, TItem> o2)
            {
                var c1 = o1.Bounds.Centre;
                var c2 = o2.Bounds.Centre;
                return c1.CompareTo(c2);
            }

            #endregion
        }

        private class AnonymousAbstractNodeImpl : AbstractNode<Interval, TItem>
        {
            #region Constructors

            /// <summary>
            ///
            /// </summary>
            /// <param name="nodeCapacity"></param>
            public AnonymousAbstractNodeImpl(int nodeCapacity) : base(nodeCapacity) { }

            #endregion

            #region Methods

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            protected override Interval ComputeBounds()
            {
                Interval bounds = null;
                //var bounds = Interval.Create();
                foreach (var childBoundable in ChildBoundables)
                {
                    if (bounds == null)
                         bounds = new Interval(childBoundable.Bounds);
                    else 
                        bounds.ExpandToInclude(childBoundable.Bounds);
                    //bounds = bounds.ExpandedByInterval((Interval) childBoundable.Bounds);
                }
                return bounds;
            }

            #endregion
        }

        private class AnonymousIntersectsOpImpl : IIntersectsOp
        {
            #region Methods

            public bool Intersects(Interval aBounds, Interval bBounds) 
            {
                return aBounds.Intersects(bBounds);
            }

            #endregion
        }

        #endregion
    }
}