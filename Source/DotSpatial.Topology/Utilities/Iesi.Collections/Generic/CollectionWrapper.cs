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
using System.Collections.Generic;
using DotSpatial.Serialization;

namespace Iesi.Collections.Generic
{
    /// <summary>
    /// Generic Collection wrapper
    /// </summary>
    /// <typeparam name="T">The type of the collection wrapper.</typeparam>
    public class CollectionWrapper<T> : EnumerableWrapper<T>, ICollection<T>
    {
        private readonly ICollection _innerCollection;

        /// <summary>
        /// Constructor for CollectionWrapper
        /// </summary>
        /// <param name="toWrap">The Collection to Wrap</param>
        public CollectionWrapper(ICollection toWrap)
            : base(toWrap)
        {
            _innerCollection = toWrap;
        }

        #region ICollection<T> Members

        /// <summary>
        /// Throws a readonly exception
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        ///  Throws a readonly exception
        /// </summary>
        public void Clear()
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Returns a boolean that is true if this collection contains item
        /// </summary>
        /// <param name="item">The item to test</param>
        /// <returns>A Boolean that is true if this collection contains item</returns>
        public bool Contains(T item)
        {
            foreach (object o in _innerCollection)
                if ((object)item == o) return true;
            return false;
        }

        /// <summary>
        /// Copies all the members of array to this set
        /// </summary>
        /// <param name="array">The array to copy</param>
        /// <param name="arrayIndex">The index with which to start copying</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The integer count
        /// </summary>
        public int Count
        {
            get { return _innerCollection.Count; }
        }

        /// <summary>
        /// Always returns True
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true; //always return true since the old ICollection does not support mutation
            }
        }

        /// <summary>
        /// Throws a readonly exception
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            throw new ReadOnlyException();
        }

        #endregion
    }
}