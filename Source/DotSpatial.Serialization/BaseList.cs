// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is  Ted Dunsford. Created 2/20/2009 4:24:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|---------|---------------------------------------------------------------------
// |      Name       |  Date   |                        Comments
// |-----------------|---------|----------------------------------------------------------------------
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// BaseList
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    [Serializable]
    public class BaseList<T> : BaseCollection<T>, IList<T>
        where T : class
    {
        #region Indexers

        /// <summary>
        /// Gets or sets the value of type T at the specified index
        /// </summary>
        /// <param name="index">The zero-base integer index marking the position of the item</param>
        /// <returns>The item</returns>
        public T this[int index]
        {
            get
            {
                return InnerList[index];
            }

            set
            {
                Exclude(InnerList[index]);
                Include(value);
                InnerList[index] = value;
                OnIncludeComplete(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the integer index of the specified item.
        /// </summary>
        /// <param name="item">The implementation of T to obtain the zero based integer index of.</param>
        /// <returns>An integer that is -1 if the item is not found, or the zero based index.</returns>
        public int IndexOf(T item)
        {
            return InnerList.IndexOf(item);
        }

        /// <summary>
        /// If the read-only property is false, then this inserts the item to the specified index.
        /// </summary>
        /// <param name="index">The zero based integer index describing the target index for the item.</param>
        /// <param name="item">The implementation of T to insert into this index.</param>
        /// <exception cref="ReadOnlyException">If ReadOnly is true, then this method will cause an exception</exception>
        public virtual void Insert(int index, T item)
        {
            DoInsert(index, item);
        }

        /// <summary>
        /// Removes the item from the specified index
        /// </summary>
        /// <param name="index">The zero based integer index</param>
        public void RemoveAt(int index)
        {
            T item = InnerList[index];
            InnerList.RemoveAt(index);
            OnExclude(item);
        }

        /// <summary>
        /// This happens after an item of type T was added to the list.
        /// </summary>
        /// <param name="index">Index where the item was inserted.</param>
        /// <param name="item">Item that was inserted.</param>
        protected virtual void OnInsert(int index, T item)
        {
        }

        /// <summary>
        /// This happens after a value has been updated in the list.
        /// </summary>
        /// <param name="index">The index where the replacement took place.</param>
        /// <param name="oldItem">The item that was removed from the position.</param>
        /// <param name="newItem">The new item that is replacing it in the list.</param>
        protected virtual void OnItemSet(int index, T oldItem, T newItem)
        {
        }

        /// <summary>
        /// The happens after an item was removed from the specified index.
        /// </summary>
        /// <param name="index">The index the item was removed from.</param>
        /// <param name="item">The item that was removed</param>
        protected virtual void OnRemoveAt(int index, T item)
        {
        }

        private void DoInsert(int index, T item)
        {
            if (IsReadOnly) throw new ReadOnlyException();

            Include(item);
            InnerList.Insert(index, item);
            OnInsert(index, item);
            OnIncludeComplete(item);
        }

        #endregion
    }
}