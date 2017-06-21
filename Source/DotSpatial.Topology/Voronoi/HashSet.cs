// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from a code project example:
// http://www.codeproject.com/KB/recipes/fortunevoronoi.aspx
// which is protected under the Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored original code for working with laser data
// Ted Dunsford      | August 26, 2009    |  Changed some formatting and used a generic collection
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Topology.Voronoi
{
    /// <summary>
    /// The HashSet is a dictionary that provides the optimized
    /// </summary>
    public class HashSet<T> : ICollection<T>
    {
        readonly Dictionary<T, T> _h = new Dictionary<T, T>();

        #region ICollection<T> Members

        ///<summary>
        /// Adds the specified object to the hash set collection
        ///</summary>
        ///<param name="item">The object to add</param>
        public void Add(T item)
        {
            _h.Add(item, item);
        }

        /// <summary>
        /// Gets the enumerator of the items in the set
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _h.Keys.GetEnumerator();
        }

        /// <summary>
        /// Gets the integer count of members in this set
        /// </summary>
        public int Count
        {
            get
            {
                return _h.Count;
            }
        }

        /// <summary>
        /// Copies the members of this hash set to the specified array, starting at the specified index
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(T[] array, int index)
        {
            _h.Keys.CopyTo(array, index);
        }

        /// <summary>
        /// Clears the entire set
        /// </summary>
        public void Clear()
        {
            _h.Clear();
        }

        /// <summary>
        /// Gets a boolean indicating if the specified item is in the set
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _h.ContainsKey(item);
        }

        /// <summary>
        /// These sets can be modified
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return _h.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _h.Keys.GetEnumerator();
        }

        #endregion
    }
}