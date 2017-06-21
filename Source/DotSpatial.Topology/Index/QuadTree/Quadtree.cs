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

namespace DotSpatial.Topology.Index.Quadtree
{
    /// <summary>
    /// A Quadtree is a spatial index structure for efficient querying
    /// of 2D rectangles.  If other kinds of spatial objects
    /// need to be indexed they can be represented by their
    /// envelopes
    /// The quadtree structure is used to provide a primary filter
    /// for range rectangle queries.  The Query() method returns a list of
    /// all objects which may intersect the query rectangle.  Notice that
    /// it may return objects which do not in fact intersect.
    /// A secondary filter is required to test for exact intersection.
    /// Of course, this secondary filter may consist of other tests besides
    /// intersection, such as testing other kinds of spatial relationships.
    /// This implementation does not require specifying the extent of the inserted
    /// items beforehand.  It will automatically expand to accomodate any extent
    /// of dataset.
    /// This data structure is also known as an <c>MX-CIF quadtree</c>
    /// following the usage of Samet and others.
    /// </summary>
    public class Quadtree : ISpatialIndex
    {
        /// <summary>
        /// Root of Quadtree
        /// </summary>
        protected readonly Root Root;

        /// <summary>
        /// minExtent is the minimum envelope extent of all items
        /// inserted into the tree so far. It is used as a heuristic value
        /// to construct non-zero envelopes for features with zero X and/or Y extent.
        /// Start with a non-zero extent, in case the first feature inserted has
        /// a zero extent in both directions.  This value may be non-optimal, but
        /// only one feature will be inserted with this value.
        /// </summary>
        private double _minExtent = 1.0;

        /// <summary>
        /// Constructs a Quadtree with zero items.
        /// </summary>
        public Quadtree()
        {
            Root = new Root();
        }

        /// <summary>
        /// Returns the number of levels in the tree.
        /// </summary>
        public virtual int Depth
        {
            get
            {
                //I don't think it's possible for root to be null. Perhaps we should
                //remove the check. [Jon Aquino]
                //Or make an assertion [Jon Aquino 10/29/2003]
                if (Root != null)
                    return Root.Depth;
                return 0;
            }
        }

        /// <summary>
        /// Returns the number of items in the tree.
        /// </summary>
        public virtual int Count
        {
            get
            {
                if (Root != null)
                    return Root.Count;
                return 0;
            }
        }

        #region ISpatialIndex Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="itemEnv"></param>
        /// <param name="item"></param>
        public virtual void Insert(IEnvelope itemEnv, object item)
        {
            CollectStats(itemEnv);
            IEnvelope insertEnv = EnsureExtent(itemEnv, _minExtent);
            Root.Insert(insertEnv, item);
        }

        /// <summary>
        /// Removes a single item from the tree.
        /// </summary>
        /// <param name="itemEnv">The Envelope of the item to remove.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was found.</returns>
        public virtual bool Remove(IEnvelope itemEnv, object item)
        {
            IEnvelope posEnv = EnsureExtent(itemEnv, _minExtent);
            return Root.Remove(posEnv, item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        public virtual IList Query(IEnvelope searchEnv)
        {
            /*
            * the items that are matched are the items in quads which
            * overlap the search envelope
            */
            var visitor = new ArrayListVisitor();
            Query(searchEnv, visitor);
            return visitor.Items;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="visitor"></param>
        public virtual void Query(IEnvelope searchEnv, IItemVisitor visitor)
        {
            /*
            * the items that are matched are the items in quads which
            * overlap the search envelope
            */
            Root.Visit(searchEnv, visitor);
        }

        #endregion

        /// <summary>
        /// Ensure that the envelope for the inserted item has non-zero extents.
        /// Use the current minExtent to pad the envelope, if necessary.
        /// </summary>
        /// <param name="itemEnv"></param>
        /// <param name="minExtent"></param>
        public static IEnvelope EnsureExtent(IEnvelope itemEnv, double minExtent)
        {
            //The names "ensureExtent" and "minExtent" are misleading -- sounds like
            //this method ensures that the extents are greater than minExtent.
            //Perhaps we should rename them to "ensurePositiveExtent" and "defaultExtent".
            //[Jon Aquino]
            double minx = itemEnv.Minimum.X;
            double maxx = itemEnv.Maximum.X;
            double miny = itemEnv.Minimum.Y;
            double maxy = itemEnv.Maximum.Y;
            // has a non-zero extent
            if (minx != maxx && miny != maxy)
                return itemEnv;
            // pad one or both extents
            if (minx == maxx)
            {
                minx = minx - minExtent / 2.0;
                maxx = minx + minExtent / 2.0;
            }
            if (miny == maxy)
            {
                miny = miny - minExtent / 2.0;
                maxy = miny + minExtent / 2.0;
            }
            return new Envelope(minx, maxx, miny, maxy);
        }

        /// <summary>
        /// Return a list of all items in the Quadtree.
        /// </summary>
        public virtual IList QueryAll()
        {
            IList foundItems = new ArrayList();
            Root.AddAllItems(ref foundItems);
            return foundItems;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="itemEnv"></param>
        private void CollectStats(IRectangle itemEnv)
        {
            double delX = itemEnv.Width;
            if (delX < _minExtent && delX > 0.0)
                _minExtent = delX;

            double delY = itemEnv.Width;
            if (delY < _minExtent && delY > 0.0)
                _minExtent = delY;
        }
    }
}