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

namespace Iesi.Collections.Generic
{
    /// <summary>
    /// A wrapper that can wrap a ISet as a generic ISet&lt;T&gt;
    /// </summary>
    /// <typeparam name="T">The type of the set.</typeparam>
    /// <remarks>
    /// In most operations, there is no copying of collections. The wrapper just delegate the function to the wrapped.
    /// The following functions' implementation may involve collection copying:
    /// Union, Intersect, Minus, ExclusiveOr, ContainsAll, AddAll, RemoveAll, RetainAll
    /// </remarks>
    /// <exception cref="InvalidCastException">
    /// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
    /// </exception>
    public sealed class SetWrapper<T> : ISet<T>
    {
        private readonly ISet _innerSet;

        /// <summary>
        /// Sets the Wrapper
        /// </summary>
        /// <param name="toWrap"></param>
        public SetWrapper(ISet toWrap)
        {
            if (toWrap == null)
                throw new ArgumentNullException();
            _innerSet = toWrap;
        }

        #region Operators

        /// <summary>
        /// Combines the two Sets
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ISet<T> Union(ISet<T> a)
        {
            return GetSetCopy().Union(a);
        }

        /// <summary>
        /// Intersects the two sets
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ISet<T> Intersect(ISet<T> a)
        {
            return GetSetCopy().Intersect(a);
        }

        /// <summary>
        /// Subtracts a set from this set
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ISet<T> Minus(ISet<T> a)
        {
            return GetSetCopy().Minus(a);
        }

        /// <summary>
        /// Creates a new set that includes members in one group or the other but not both
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ISet<T> ExclusiveOr(ISet<T> a)
        {
            return GetSetCopy().ExclusiveOr(a);
        }

        #endregion

        #region ISet<T> Members

        /// <summary>
        /// Gets or sets a boolean that is true if this set contains o
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Contains(T o)
        {
            return _innerSet.Contains(o);
        }

        /// <summary>
        /// returns a boolean that is true if this set contains all the members of c
        /// </summary>
        /// <param name="c">The Collection to test</param>
        /// <returns></returns>
        public bool ContainsAll(ICollection<T> c)
        {
            return _innerSet.ContainsAll(GetSetCopy(c));
        }

        /// <summary>
        /// Gets a boolean that indicates if this set is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return _innerSet.IsEmpty; }
        }

        /// <summary>
        /// Attempts to add o to this set, and returns true if successful
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Add(T o)
        {
            return _innerSet.Add(o);
        }

        /// <summary>
        /// Attempts to add the collection c to this set and returns true if successful
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool AddAll(ICollection<T> c)
        {
            return _innerSet.AddAll(GetSetCopy(c));
        }

        /// <summary>
        /// Attempts to remove o from this set and returns true if successful
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Remove(T o)
        {
            return _innerSet.Remove(o);
        }

        /// <summary>
        /// Attempts to remove every member of c from this set and returns true if successful
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool RemoveAll(ICollection<T> c)
        {
            return _innerSet.RemoveAll(GetSetCopy(c));
        }

        /// <summary>
        /// Removes any members not in c from this set and returns true if successful
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool RetainAll(ICollection<T> c)
        {
            return _innerSet.RemoveAll(GetSetCopy(c));
        }

        /// <summary>
        /// Clears all the members of the set
        /// </summary>
        public void Clear()
        {
            _innerSet.Clear();
        }

        /// <summary>
        /// Creates a deep copy of all the members of this set
        /// </summary>
        /// <returns></returns>
        public ISet<T> Clone()
        {
            return new SetWrapper<T>((ISet)_innerSet.Clone());
        }

        /// <summary>
        /// Gets an integer specifying the number of members to this set
        /// </summary>
        public int Count
        {
            get
            {
                return _innerSet.Count;
            }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        /// <summary>
        /// Copies all the members of the specified arrayIndex to this set
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerSet.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets a boolean indicating if this set is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Creates an enumerator for this set
        /// </summary>
        /// <returns>A type specific EnumeratorWrapper </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new EnumeratorWrapper<T>(_innerSet.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerSet.GetEnumerator();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region private methods

        private static Set<T> GetSetCopy(ICollection<T> c)
        {
            return new HashedSet<T>(c);
        }

        private static Set<T> GetSetCopy(ICollection c)
        {
            Set<T> retVal = new HashedSet<T>();
            ((ISet)retVal).AddAll(c);
            return retVal;
        }

        private Set<T> GetSetCopy()
        {
            return GetSetCopy(_innerSet);
        }

        #endregion
    }
}