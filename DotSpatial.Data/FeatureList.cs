// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 7:36:02 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DotSpatial.Data
{
    /// <summary>
    /// A list that keeps track of a "parent" body that is also of type IFeature.
    /// Whenever a member is added to the list, it sets the parent property.
    /// </summary>
    public class FeatureList : IFeatureList
    {
        #region IFeatureList Members

        /// <summary>
        /// Occurs when a new feature is added to the list
        /// </summary>
        public event EventHandler<FeatureEventArgs> FeatureAdded;

        /// <summary>
        /// Occurs when a feature is removed from the list.
        /// </summary>
        public event EventHandler<FeatureEventArgs> FeatureRemoved;

        #endregion

        #region variables

        private readonly IFeatureSet _parent;
        private bool _includeAttributes;
        private List<IFeature> _list;
        private int _suspension;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of a FeatureList where no parent is specified.
        /// This will basically simply act as a list and will not allow the features
        /// to be aware of the parent featureset.
        /// </summary>
        public FeatureList()
        {
            Configure();
        }

        /// <summary>
        /// Instantiates a new instance of a FeatureList where the ParentFeatureSet
        /// is specified so that each feature will be aware of its parent.
        /// </summary>
        /// <param name="parent">The ParentFeatureSet of the specified item</param>
        public FeatureList(IFeatureSet parent)
        {
            _parent = parent;
            Configure();
        }

        private void Configure()
        {
            _suspension = 0;
            _list = new List<IFeature>();
            _includeAttributes = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes all elements from the EventList&lt;IFeature&gt;.
        /// </summary>
        /// <exception cref="System.ApplicationException">Unable to clear while the ReadOnly property is set to true.</exception>
        public virtual void Clear()
        {
            foreach (IFeature feature in _list)
            {
                ExcludeFeature(feature);
            }
            _list.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the System.Collections.Generic.List&lt;IFeature&gt;.
        /// </summary>
        /// <param name="item"> The object to locate in the System.Collections.Generic.List&lt;IFeature&gt;. The value can be null for reference types.</param>
        /// <returns>true if item is found in the System.Collections.Generic.List&lt;IFeature&gt; otherwise, false.</returns>
        public virtual bool Contains(IFeature item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// Gets or sets whether or not the events have been suspended
        /// </summary>
        public bool EventsSuspended
        {
            get { return (_suspension > 0); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through this list
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IFeature> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        // if this is cast to IEnumerable or whatever
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Resumes events
        /// </summary>
        public void ResumeEvents()
        {
            _suspension -= 1;
        }

        /// <summary>
        /// Temporarilly disables events
        /// </summary>
        public void SuspendEvents()
        {
            _suspension += 1;
        }

        /// <summary>
        /// Gets the internal list of features.  This is quite fast in this case because
        /// the actual internal data store is a list.  This does not create a new collection.
        /// </summary>
        /// <returns>The internal list.</returns>
        public virtual List<IFeature> ToList()
        {
            return _list;
        }

        #region Reverse

        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the EventList&lt;IFeature&gt;.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ApplicationException">Unable to reverse while the ReadOnly property is set to true.</exception>
        public virtual void Reverse(int index, int count)
        {
            _list.Reverse(index, count);
        }

        /// <summary>
        /// Reverses the order of the elements in the entire EventList&lt;IFeature&gt;.
        /// </summary>
        /// <exception cref="System.ApplicationException">Unable to reverse while the ReadOnly property is set to true.</exception>
        public virtual void Reverse()
        {
            _list.Reverse();
        }

        #endregion

        #region Sort

        /// <summary>
        /// Sorts the elements in the entire EventList&lt;IFeature&gt; using the specified System.Comparison&lt;IFeature&gt;.
        /// </summary>
        /// <param name="comparison">The System.Comparison&lt;IFeature&gt; to use when comparing elements.</param>
        /// <exception cref="System.ArgumentException">The implementation of comparison caused an error during the sort. For example, comparison might not return 0 when comparing an item with itself.</exception>
        /// <exception cref="System.ArgumentNullException">comparison is null.</exception>
        /// <exception cref="System.ApplicationException">Unable to sort while the ReadOnly property is set to true.</exception>
        public virtual void Sort(Comparison<IFeature> comparison)
        {
            _list.Sort(comparison);
        }

        /// <summary>
        /// Sorts the elements in a range of elements in EventList&lt;IFeature&gt; using the specified comparer.
        /// </summary>
        /// <param name="index"> The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;IFeature&gt; implementation to use when comparing elements, or null to use the default comparer System.Collections.Generic.Comparer&lt;IFeature&gt;.Default.</param>
        /// <exception cref="System.ArgumentException">index and count do not specify a valid range in the EventList&lt;IFeature&gt;.-or-The implementation of comparer caused an error during the sort. For example, comparer might not return 0 when comparing an item with itself.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.InvalidOperationException"> comparer is null, and the default comparer
        /// System.Collections.Generic.Comparer&lt;IFeature&gt;.Default cannot find implementation of the System.IComparable&lt;IFeature&gt;
        /// generic interface or the System.IComparable interface for type IFeature.</exception>
        /// <exception cref="System.ApplicationException">Unable to sort while the ReadOnly property is set to true.</exception>
        public virtual void Sort(int index, int count, IComparer<IFeature> comparer)
        {
            _list.Sort(index, count, comparer);
        }

        /// <summary>
        /// Sorts the elements in the entire DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt; using the specified comparer.
        /// </summary>
        /// <param name="comparer"> The System.Collections.Generic.IComparer&lt;IFeature&gt; implementation to use when comparing elements, or null to use the default comparer System.Collections.Generic.Comparer&lt;IFeature&gt;.Default.</param>
        /// <exception cref="System.ArgumentException">The implementation of comparer caused an error during the sort. For example, comparer might not return 0 when comparing an item with itself.</exception>
        /// <exception cref="System.InvalidOperationException">comparer is null, and the default comparer System.Collections.Generic.Comparer&lt;IFeature&gt;.Default cannot find implementation of the System.IComparable&lt;IFeature&gt; generic interface or the System.IComparable interface for type IFeature.</exception>
        /// <exception cref="System.ApplicationException">Unable to sort while the ReadOnly property is set to true.</exception>
        public virtual void Sort(IComparer<IFeature> comparer)
        {
            _list.Sort(comparer);
        }

        /// <summary>
        /// Sorts the elements in the entire DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt; using the default comparer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The default comparer System.Collections.Generic.Comparer&lt;IFeature&gt;.Default cannot find an implementation of the System.IComparable&lt;IFeature&gt; generic interface or the System.IComparable interface for type IFeature.</exception>
        /// <exception cref="System.ApplicationException">Unable to sort while the ReadOnly property is set to true.</exception>
        public virtual void Sort()
        {
            _list.Sort();
        }

        #endregion

        #region CopyTo

        /// <summary>
        /// Copies the entire System.Collections.Generic.List&lt;IFeature&gt; to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.Generic.List&lt;IFeature&gt;. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex"> The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException: arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source System.Collections.Generic.List&lt;T&gt; is greater than the available space from arrayIndex to the end of the destination array. </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">arrayIndex is less than 0</exception>
        /// <exception cref="System.ArgumentNullException">array is null</exception>
        public virtual void CopyTo(IFeature[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copies a range of elements from the EventList&lt;IFeature&gt; to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="index">The zero-based index in the source EventList&lt;T&gt; at which copying begins</param>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from EventList&lt;IFeature&gt;. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> index is less than 0.-or-arrayIndex is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index is equal to or greater than the EventList&lt;IFeature&gt;.Count of the source EventList&lt;T&gt;.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements from index to the end of the source EventList&lt;IFeature&gt; is greater than the available space from arrayIndex to the end of the destination array.</exception>
        public virtual void CopyTo(int index, IFeature[] array, int arrayIndex, int count)
        {
            _list.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// Copies the entire EventList&lt;IFeature&gt; to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from EventList&lt;IFeature&gt;. The System.Array must have zero-based indexing.</param>
        /// <exception cref="System.ArgumentException">The number of elements in the source EventList&lt;IFeature&gt; is greater than the number of elements that the destination array can contain.</exception>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        public virtual void CopyTo(IFeature[] array)
        {
            _list.CopyTo(array);
        }

        #endregion

        private void IncludeFeature(IFeature item)
        {
            if (_list.Count == 1)
            {
                _parent.FeatureType = item.FeatureType;
            }
            item.ParentFeatureSet = _parent;
            if (_includeAttributes)
            {
                DataRow row = _parent.DataTable.NewRow();
                if (item.DataRow != null)
                {
                    for (int col = 0; col < _parent.DataTable.Columns.Count; col++)
                    {
                        string name = _parent.DataTable.Columns[col].ColumnName;
                        if (item.DataRow.Table.Columns.Contains(name)) //if the item happens to have a field with the same name, include the attribute.
                        {
                            row[col] = item.DataRow[name];
                        }
                    }
                }
                item.DataRow = row;
            }
            OnFeatureAdded(item);
        }

        private void ExcludeFeature(IFeature item)
        {
            item.ParentFeatureSet = null;
            _parent.DataTable.Rows.Remove(item.DataRow);
            OnFeatureRemoved(item);
        }

        /// <summary>
        /// Converts the elements in the current EventList&lt;IFeature&gt; to another type, and returns a list containing the converted elements.
        /// </summary>
        /// <typeparam name="TOutput">The output type to convert to</typeparam>
        /// <param name="converter">A System.Converter&lt;TInput, TOutput&gt; delegate that converts each element from one type to another type.</param>
        /// <returns>A List&lt;T&gt; of the target type containing the converted elements from the current EventList&lt;T&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">converter is null.</exception>
        public virtual List<TOutput> ConvertAll<TOutput>(Converter<IFeature, TOutput> converter)
        {
            return _list.ConvertAll(converter);
        }

        /// <summary>
        /// Determines whether the EventList&lt;IFeature&gt; contains elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the elements to search for.</param>
        /// <returns>true if the EventList&lt;T&gt; contains one or more elements that match the conditions defined by the specified predicate; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public virtual bool Exists(Predicate<IFeature> match)
        {
            return _list.Exists(match);
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source EventList&lt;IFeature&gt;.
        /// </summary>
        /// <param name="index">The zero-based EventList&lt;IFeature&gt; index at which the range starts.</param>
        /// <param name="count"> The number of elements in the range.</param>
        /// <returns>A shallow copy of a range of elements in the source EventList&lt;IFeature&gt;.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the EventList&lt;IFeature&gt;.</exception>
        public virtual List<IFeature> GetRange(int index, int count)
        {
            return _list.GetRange(index, count);
        }

        /// <summary>
        /// Copies the elements of the DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt; to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt;.</returns>
        public virtual IFeature[] ToArray()
        {
            return _list.ToArray();
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt;, if that number is less than a threshold value.
        /// </summary>
        public virtual void TrimExcess()
        {
            _list.TrimExcess();
        }

        /// <summary>
        /// Determines whether every element in the DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt; matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;IFeature&gt; delegate that defines the conditions to check against the elements.</param>
        /// <returns>true if every element in the DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt; matches the conditions defined by the specified predicate; otherwise, false. If the list has no elements, the return value is true.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public virtual bool TrueForAll(Predicate<IFeature> match)
        {
            return _list.TrueForAll(match);
        }

        #region IndexOf

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire System.Collections.Generic.List&lt;IFeature&gt;.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List&lt;IFeature&gt;. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire System.Collections.Generic.List&lt;IFeature&gt;, if found; otherwise, –1.</returns>
        public virtual int IndexOf(IFeature item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the EventList&lt;IFeature&gt; that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="item">The object to locate in the EventList&lt;IFeature&gt;. The value can be null for reference types.</param>
        /// <param name="index"> The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the EventList&lt;IFeature&gt; that starts at index and contains count number of elements, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"> index is outside the range of valid indexes for the EventList&lt;IFeature&gt;.-or-count is less than 0.-or-index and count do not specify a valid section in the EventList&lt;IFeature&gt;.</exception>
        public virtual int IndexOf(IFeature item, int index, int count)
        {
            return _list.IndexOf(item, index, count);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the EventList&lt;IFeature&gt; that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item">The object to locate in the EventList&lt;IFeature&gt;. The value can be null for reference types.</param>
        /// <param name="index"> The zero-based starting index of the search.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the EventList&lt;IFeature&gt; that extends from index to the last element, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the EventList&lt;IFeature&gt;.</exception>
        public virtual int IndexOf(IFeature item, int index)
        {
            return _list.IndexOf(item, index);
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts an element into the System.Collections.Generic.List&lt;IFeature&gt; at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-index is greater than System.Collections.Generic.List&lt;IFeature&gt;.Count.</exception>
        /// <exception cref="System.ApplicationException">Unable to insert while the ReadOnly property is set to true.</exception>
        public virtual void Insert(int index, IFeature item)
        {
            _list.Insert(index, item);
            IncludeFeature(item);
            if (_includeAttributes) _parent.DataTable.Rows.InsertAt(item.DataRow, index);
        }

        /// <summary>
        /// Inserts the elements of a collection into the EventList&lt;IFeature&gt; at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the EventList&lt;IFeature&gt;. The collection itself cannot be null, but it can contain elements that are null, if type IFeature is a reference type.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-index is greater than EventList&lt;IFeature&gt;.Count.</exception>
        /// <exception cref="System.ArgumentNullException">collection is null.</exception>
        /// <exception cref="System.ApplicationException">Unable to insert while the ReadOnly property is set to true.</exception>
        public virtual void InsertRange(int index, IEnumerable<IFeature> collection)
        {
            int indx = index;
            foreach (IFeature item in collection)
            {
                _list.Insert(indx, item);
                indx++;
                IncludeFeature(item);
                if (_includeAttributes) _parent.DataTable.Rows.InsertAt(item.DataRow, indx);
            }
        }

        #endregion

        #region LastIndexOf

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the EventList&lt;IFeature&gt; that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the EventList&lt;IFeature&gt;. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the EventList&lt;IFeature&gt; that contains count number of elements and ends at index, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the EventList&lt;IFeature&gt;.-or-count is less than 0.-or-index and count do not specify a valid section in the EventList&lt;IFeature&gt;.</exception>
        public virtual int LastIndexOf(IFeature item, int index, int count)
        {
            return _list.LastIndexOf(item, index, count);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the EventList&lt;IFeature&gt; that extends from the first element to the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the EventList&lt;IFeature&gt;. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the EventList&lt;IFeature&gt; that extends from the first element to index, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the EventList&lt;IFeature&gt;.</exception>
        public virtual int LastIndexOf(IFeature item, int index)
        {
            return _list.LastIndexOf(item, index, Count);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the entire EventList&lt;IFeature&gt;.
        /// </summary>
        /// <param name="item">The object to locate in the EventList&lt;IFeature&gt;. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the last occurrence of item within the entire the EventList&lt;IFeature&gt;, if found; otherwise, –1.</returns>
        public virtual int LastIndexOf(IFeature item)
        {
            return _list.LastIndexOf(item);
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.List&lt;IFeature&gt;.
        /// </summary>
        /// <param name="item">The object to remove from the System.Collections.Generic.List&lt;IFeature&gt;. The value can be null for reference types.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not
        /// found in the System.Collections.Generic.List&lt;IFeature&gt;.</returns>
        /// <exception cref="System.ApplicationException">Unable to remove while the ReadOnly property is set to true.</exception>
        public virtual bool Remove(IFeature item)
        {
            if (_list.Remove(item))
            {
                ExcludeFeature(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the element at the specified index of the System.Collections.Generic.List&lt;IFeature&gt;.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="System.ApplicationException">Unable to remove while the ReadOnly property is set to true.</exception>
        public virtual void RemoveAt(int index)
        {
            IFeature item = _list[index];
            ExcludeFeature(item);
            _list.RemoveAt(index);
        }

        /// <summary>
        /// Removes the all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed from the EventList&lt;IFeature&gt;</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        /// <exception cref="System.ApplicationException">Unable to remove while the ReadOnly property is set to true.</exception>
        public virtual int RemoveAll(Predicate<IFeature> match)
        {
            List<IFeature> matches = FindAll(match);

            int numRemoved = matches.Count;
            foreach (IFeature item in matches)
            {
                if (_list.Remove(item))
                {
                    ExcludeFeature(item);
                }
                else
                {
                    numRemoved--;
                }
            }
            return numRemoved;
        }

        /// <summary>
        /// Removes a range of elements from the EventList&lt;IFeature&gt;.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the EventList&lt;IFeature&gt;.</exception>
        /// <exception cref="System.ApplicationException">Unable to remove while the ReadOnly property is set to true.</exception>
        public virtual void RemoveRange(int index, int count)
        {
            IFeature[] temp = new IFeature[count];
            _list.CopyTo(index, temp, 0, count);
            _list.RemoveRange(index, count);
            foreach (IFeature item in temp)
            {
                ExcludeFeature(item);
            }
        }

        #endregion

        #region Find

        /// <summary>
        /// Retrieves all the elements that match the conditions described by the specified predicate
        /// </summary>
        /// <param name="match">The System.Predicate that defines the conditions to search for</param>
        /// <returns>A List of matches</returns>
        public virtual List<IFeature> FindAll(Predicate<IFeature> match)
        {
            return _list.FindAll(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the EventList&lt;IFeature&gt; that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match"> The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for the EventList&lt;IFeature&gt;.-or-count is less than 0.-or-startIndex and count do not specify a valid section in the EventList&lt;IFeature&gt;.</exception>
        /// <exception cref="System.ArgumentNullException">match is null</exception>
        public virtual int FindIndex(int startIndex, int count, Predicate<IFeature> match)
        {
            return _list.FindIndex(startIndex, count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the EventList&lt;IFeature&gt; that extends from the specified index to the last element.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for the EventList&lt;IFeature&gt;.</exception>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public virtual int FindIndex(int startIndex, Predicate<IFeature> match)
        {
            return _list.FindIndex(startIndex, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire EventList&lt;IFeature&gt;.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public virtual int FindIndex(Predicate<IFeature> match)
        {
            return _list.FindIndex(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire EventList&lt;IFeature&gt;.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type IFeature.</returns>
        /// <exception cref= "System.ArgumentNullException">match is null."</exception>
        public virtual IFeature FindLast(Predicate<IFeature> match)
        {
            return _list.FindLast(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the EventList&lt;IFeature&gt; that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match"></param>
        /// <returns>The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the element to search for.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for the EventList&lt;IFeature&gt;.-or-count is less than 0.-or-startIndex and count do not specify a valid section in the EventList&lt;IFeature&gt;.</exception>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public virtual int FindLastIndex(int startIndex, int count, Predicate<IFeature> match)
        {
            return _list.FindLastIndex(startIndex, count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the EventList&lt;IFeature&gt; that extends from the first element to the specified index.
        /// </summary>
        /// <param name="startIndex"> The zero-based starting index of the backward search.</param>
        /// <param name="match">The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">: startIndex is outside the range of valid indexes for the EventList&lt;IFeature&gt;.</exception>
        public virtual int FindLastIndex(int startIndex, Predicate<IFeature> match)
        {
            return _list.FindLastIndex(startIndex, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire EventList&lt;IFeature&gt;.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;IFeature&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public virtual int FindLastIndex(Predicate<IFeature> match)
        {
            return _list.FindLastIndex(match);
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds the item to the list, setting the parent to be the list's parent
        /// </summary>
        /// <param name="item"></param>
        public void Add(IFeature item)
        {
            _list.Add(item);
            IncludeFeature(item);
            if (_includeAttributes) _parent.DataTable.Rows.Add(item.DataRow);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.List&lt;T&gt;
        /// </summary>
        /// <param name="collection">collection: The collection whose elements should be added to the end of the
        /// System.Collections.Generic.List&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null,
        /// if type T is a reference type.</param>
        /// <exception cref="System.ApplicationException">Unable to add while the ReadOnly property is set to true.</exception>
        public virtual void AddRange(IEnumerable<IFeature> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        #endregion

        #region BinarySearch

        /// <summary>
        /// Searches the entire sorted System.Collections.Generic.List&lt;IFeature&gt; for an element using the default comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <returns>The zero-based index of item in the sorted System.Collections.Generic.List&lt;IFeature&gt;, if item is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, if there is no larger element, the bitwise complement of System.Collections.Generic.List&lt;IFeature&gt;.Count.</returns>
        /// <exception cref="System.InvalidOperationException">The default comparer System.Collections.Generic.Comparer&lt;IFeature&gt;.Default cannot find an implementation of the System.IComparable&lt;IFeature&gt; generic interface or the System.IComparable interface for type IFeature.</exception>
        public virtual int BinarySearch(IFeature item)
        {
            return _list.BinarySearch(item);
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <returns>
        /// The number of elements that the DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt; can contain before resizing is required.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt;.Capacity is set to a value that is less than DotSpatial.Interfaces.Framework.IEventList&lt;IFeature&gt;.Count.</exception>
        public virtual int Capacity
        {
            get
            {
                return _list.Capacity;
            }
            set
            {
                _list.Capacity = value;
            }
        }

        /// <summary>
        /// Gets the count of the members in the list
        /// </summary>
        public virtual int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// The default, indexed value of type IFeature
        /// </summary>
        /// <param name="index">The numeric index</param>
        /// <returns>An object of type IFeature corresponding to the index value specified</returns>
        public virtual IFeature this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                // Don't add handlers if the items are already members of this list
                ExcludeFeature(_list[index]);
                _list[index] = value;
                IncludeFeature(value);
                if (_includeAttributes) _parent.DataTable.Rows.RemoveAt(index);
                if (_includeAttributes) _parent.DataTable.Rows.InsertAt(value.DataRow, index);
            }
        }

        /// <summary>
        /// Gets a boolean property indicating whether this list can be written to.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// If this is false, then features will be added to the list without copying over attribute Table information.
        /// This will allow the attributes to be loaded in a more on-demand later.
        /// </summary>
        public bool IncludeAttributes
        {
            get { return _includeAttributes; }
            set { _includeAttributes = value; }
        }

        /// <summary>
        /// Gets the parent featureset for this list of features.
        /// </summary>
        public IFeatureSet Parent
        {
            get { return _parent; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs each time a feature is added to the list.  This should only be called
        /// if suspend events is false.
        /// </summary>
        /// <param name="feature">The feature that was added.</param>
        protected virtual void OnFeatureAdded(IFeature feature)
        {
            if (EventsSuspended == false)
            {
                if (FeatureAdded != null) FeatureAdded(this, new FeatureEventArgs(feature));
            }
        }

        /// <summary>
        /// Occurs each time a feature is removed from the list.  This should only be
        /// called if suspend events is false.
        /// </summary>
        /// <param name="feature">he feature that was removed</param>
        protected virtual void OnFeatureRemoved(IFeature feature)
        {
            if (EventsSuspended == false)
            {
                if (FeatureRemoved != null) FeatureRemoved(this, new FeatureEventArgs(feature));
            }
        }

        #endregion
    }
}