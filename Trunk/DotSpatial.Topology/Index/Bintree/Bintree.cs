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

namespace DotSpatial.Topology.Index.Bintree
{
    public class BinTree : Bintree<object>
    {}

    /// <summary>
    /// An <c>BinTree</c> (or "Binary Interval Tree") is a 1-dimensional version of a quadtree.
    /// It indexes 1-dimensional intervals (which may be the projection of 2-D objects on an axis).
    /// It supports range searching (where the range may be a single point).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This structure is dynamic - new items can be added at any time, and it will support deletion of items 
    /// (although this is not currently implemented).
    /// </para>
    /// <para>
    /// This implementation does not require specifying the extent of the inserted
    /// items beforehand. It will automatically expand to accomodate any extent of dataset.</para>
    /// <para>This index is different to the Interval Tree of Edelsbrunner or the Segment Tree of Bentley.</para>
    /// </remarks>
    public class Bintree<T>
    {
        #region Fields

        private readonly Root<T> _root;
        /*
        * Statistics:
        * minExtent is the minimum extent of all items
        * inserted into the tree so far. It is used as a heuristic value
        * to construct non-zero extents for features with zero extent.
        * Start with a non-zero extent, in case the first feature inserted has
        * a zero extent in both directions.  This value may be non-optimal, but
        * only one feature will be inserted with this value.
        **/
        private double _minExtent = 1.0;

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        public Bintree()
        {
            _root = new Root<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public int Count
        {
            get
            {
                return _root != null ? _root.Count : 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int Depth
        {
            get
            {
                return _root != null ? _root.Depth : 0;
            }
        }

        /// <summary>
        /// Compute the total number of nodes in the tree.
        /// </summary>
        /// <returns>The number of nodes in the tree.</returns>
        public int NodeSize
        {
            get
            {
                return _root != null ? _root.NodeCount : 0;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        private void CollectStats(Interval interval)
        {
            double del = interval.Width;
            if (del < _minExtent && del > 0.0)
                _minExtent = del;
        }

        /// <summary>
        /// Ensure that the Interval for the inserted item has non-zero extents.
        /// Use the current minExtent to pad it, if necessary.
        /// </summary>
        public static Interval EnsureExtent(Interval itemInterval, double minExtent)
        {
            var min = itemInterval.Min;
            var max = itemInterval.Max;
            // has a non-zero extent
            if (min != max) 
                return itemInterval;
            // pad extent
            if (min == max)
            {
                min = min - minExtent / 2.0;
                max = min + minExtent / 2.0;
            }
            return new Interval(min, max);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            IList<T> foundItems = new List<T>();
            _root.AddAllItems(foundItems);
            return foundItems.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="itemInterval"></param>
        /// <param name="item"></param>
        public void Insert(Interval itemInterval, T item)
        {
            CollectStats(itemInterval);
            var insertInterval = EnsureExtent(itemInterval, _minExtent);            
            _root.Insert(insertInterval, item);            
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public IList<T> Query(double x)
        {
            return Query(new Interval(x, x));
        }

        /// <summary>
        /// Queries the tree to find all candidate items which 
        /// may overlap the query interval.
        /// If the query interval is <tt>null</tt>, all items in the tree are found.
        /// <c>min</c> and <c>max</c> may be the same value.
        /// </summary>
        /// <param name="interval"></param>
        public IList<T> Query(Interval interval)
        {
            /*
             * the items that are matched are all items in intervals
             * which overlap the query interval
             */
            IList<T> foundItems = new List<T>();
            Query(interval, foundItems);
            return foundItems;
        }

        /// <summary>
        /// Adds items in the tree which potentially overlap the query interval
        /// to the given collection.
        /// If the query interval is <c>null</c>, add all items in the tree.
        /// </summary>
        /// <param name="interval">A query interval, or <c>null</c></param>
        /// <param name="foundItems">The candidate items found</param>
        public void Query(Interval interval, ICollection<T> foundItems)
        {
            _root.AddAllItemsFromOverlapping(interval, foundItems);
        }

        /// <summary>
        /// Removes a single item from the tree.
        /// </summary>
        /// <param name="itemInterval">itemEnv the Envelope of the item to be removed</param>
        /// <param name="item">the item to remove</param>
        /// <returns><c>true</c> if the item was found (and thus removed)</returns>
        public bool Remove(Interval itemInterval, T item)
        {
            Interval insertInterval = EnsureExtent(itemInterval, _minExtent);
            return _root.Remove(insertInterval, item);
        }

        #endregion
    }
}