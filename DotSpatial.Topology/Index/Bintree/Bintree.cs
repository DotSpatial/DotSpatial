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

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// An <c>BinTree</c> (or "Binary Interval Tree")
    /// is a 1-dimensional version of a quadtree.
    /// It indexes 1-dimensional intervals (which of course may
    /// be the projection of 2-D objects on an axis).
    /// It supports range searching
    /// (where the range may be a single point).
    /// This implementation does not require specifying the extent of the inserted
    /// items beforehand.  It will automatically expand to accomodate any extent
    /// of dataset.
    /// This index is different to the Interval Tree of Edelsbrunner
    /// or the Segment Tree of Bentley.
    /// </summary>
    public class Bintree
    {
        private readonly Root _root;

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

        /// <summary>
        ///
        /// </summary>
        public Bintree()
        {
            _root = new Root();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Depth
        {
            get
            {
                if (_root != null)
                    return _root.Depth;
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Count
        {
            get
            {
                if (_root != null)
                    return _root.Count;
                return 0;
            }
        }

        /// <summary>
        /// Compute the total number of nodes in the tree.
        /// </summary>
        /// <returns>The number of nodes in the tree.</returns>
        public virtual int NodeSize
        {
            get
            {
                if (_root != null)
                    return _root.NodeCount;
                return 0;
            }
        }

        /// <summary>
        /// Ensure that the Interval for the inserted item has non-zero extents.
        /// Use the current minExtent to pad it, if necessary.
        /// </summary>
        public static Interval EnsureExtent(Interval itemInterval, double minExtent)
        {
            double min = itemInterval.Min;
            double max = itemInterval.Max;
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
        /// <param name="itemInterval"></param>
        /// <param name="item"></param>
        public virtual void Insert(Interval itemInterval, object item)
        {
            CollectStats(itemInterval);
            Interval insertInterval = EnsureExtent(itemInterval, _minExtent);
            _root.Insert(insertInterval, item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEnumerator()
        {
            IList foundItems = new ArrayList();
            _root.AddAllItems(foundItems);
            return foundItems.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public virtual IList Query(double x)
        {
            return Query(new Interval(x, x));
        }

        /// <summary>
        /// min and max may be the same value.
        /// </summary>
        /// <param name="interval"></param>
        public virtual IList Query(Interval interval)
        {
            /*
             * the items that are matched are all items in intervals
             * which overlap the query interval
             */
            IList foundItems = new ArrayList();
            Query(interval, foundItems);
            return foundItems;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="foundItems"></param>
        public virtual void Query(Interval interval, IList foundItems)
        {
            _root.AddAllItemsFromOverlapping(interval, foundItems);
        }

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
    }
}