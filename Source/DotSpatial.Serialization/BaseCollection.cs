// ********************************************************************************************************
// Product Name: DotSpatial.Common.dll
// Description:  A shared module for DotSpatial libraries
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/1/2009 9:41:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|---------|---------------------------------------------------------------------
// |      Name       |  Date   |                        Comments
// |-----------------|---------|----------------------------------------------------------------------
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// BaseCollection
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection.</typeparam>
    public class BaseCollection<T> : ICollection<T>
        where T : class
    {
        #region Fields

        /// <summary>
        /// Private storage for the inner list.
        /// </summary>
        private List<T> _innerList;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCollection{T}"/> class.
        /// </summary>
        public BaseCollection()
        {
            _innerList = new List<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the count of the inner list.
        /// </summary>
        public int Count => InnerList.Count;

        /// <summary>
        /// Gets a value indicating whether this is readonly.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets the inner list of T that actually controls the content for this BaseCollection.
        /// </summary>
        [Serialize("InnerList")]
        protected List<T> InnerList
        {
            get
            {
                return _innerList;
            }

            set
            {
                if (_innerList != null && _innerList.Count > 0)
                {
                    foreach (T item in _innerList)
                    {
                        OnExclude(item);
                    }
                }

                _innerList = value;
                foreach (T item in _innerList)
                {
                    OnInclude(item);
                }

                OnInnerListSet();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an item to this base collection.
        /// </summary>
        /// <param name="item">Item that gets added.</param>
        public void Add(T item)
        {
            Include(item);
            InnerList.Add(item);
            OnIncludeComplete(item);
            OnInsert(InnerList.IndexOf(item), item);
        }

        /// <summary>
        /// Clears the list.
        /// </summary>
        public void Clear()
        {
            OnClear();
        }

        /// <summary>
        /// A boolean that is true if this set contains the specified item.
        /// </summary>
        /// <param name="item">Item that gets checked.</param>
        /// <returns>True, if the item is contained.</returns>
        public bool Contains(T item)
        {
            return InnerList.Contains(item);
        }

        /// <summary>
        /// Copies the items from this collection to the specified array
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="arrayIndex">The zero based integer array index to start copying to</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns a type specific enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        /// <summary>
        /// Moves the given item to the new position.
        /// </summary>
        /// <param name="item">Item that gets moved.</param>
        /// <param name="newPosition">Position the item is moved to.</param>
        public void Move(T item, int newPosition)
        {
            if (!InnerList.Contains(item)) return;

            int index = InnerList.IndexOf(item);
            if (index == newPosition) return;

            InnerList.RemoveAt(index);

            if (InnerList.Count <= newPosition)
            {
                // position past list end
                InnerList.Add(item);
            }
            else if (newPosition < 0)
            {
                // position before list start
                InnerList.Insert(0, item);
            }
            else
            {
                InnerList.Insert(newPosition, item);
            }

            // position inside list
            OnMoved(item, InnerList.IndexOf(item));
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection</param>
        /// <returns>Boolean, true if the remove operation is successful. </returns>
        public bool Remove(T item)
        {
            if (InnerList.Contains(item))
            {
                int index = InnerList.IndexOf(item);
                InnerList.Remove(item);

                // Removed "Exclude(item) because calling OnRemoveComplete
                OnRemoveComplete(index, item);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        /// <summary>
        /// Exclude should be called AFTER the item is successfully removed from the list.
        /// This allows any handlers that connect to the item to be removed in the event
        /// that the item is no longer anywhere in the list.
        /// </summary>
        /// <param name="item">The item to be removed</param>
        protected void Exclude(T item)
        {
            if (!InnerList.Contains(item))
            {
                OnExclude(item);
            }
        }

        /// <summary>
        /// Includes the specified item. This should be called BEFORE an item
        /// is added to the list.
        /// </summary>
        /// <param name="item">Item to be included.</param>
        protected void Include(T item)
        {
            if (!InnerList.Contains(item))
            {
                OnInclude(item);
            }
        }

        /// <summary>
        /// Clears this collection.
        /// </summary>
        protected virtual void OnClear()
        {
            List<T> deleteList = new List<T>();

            foreach (T item in InnerList)
            {
                deleteList.Add(item);
            }

            foreach (T item in deleteList)
            {
                Remove(item);
            }
        }

        /// <summary>
        /// Occurs any time an item is removed from the collection and no longer
        /// exists anywhere in the collection
        /// </summary>
        /// <param name="item">Item that gets excluded.</param>
        protected virtual void OnExclude(T item)
        {
        }

        /// <summary>
        /// Occurs any time an item is added or inserted into the collection
        /// and did not previously exist in the collection
        /// </summary>
        /// <param name="item">Item that gets included.</param>
        protected virtual void OnInclude(T item)
        {
        }

        /// <summary>
        /// Occurs after the item has been included either by set, insert, or addition.
        /// </summary>
        /// <param name="item">Item that was included.</param>
        protected virtual void OnIncludeComplete(T item)
        {
        }

        /// <summary>
        /// After serialization, the inner list is directly set. This is uzed by the FeatureLayer, for instance,
        /// to apply the new scheme.
        /// </summary>
        protected virtual void OnInnerListSet()
        {
        }

        /// <summary>
        /// Occurs when items are inserted.
        /// </summary>
        /// <param name="index">Index where the item should be inserted.</param>
        /// <param name="value">Item that should be inserted.</param>
        protected virtual void OnInsert(int index, object value)
        {
            T item = value as T;
            Include(item);
        }

        /// <summary>
        /// Occurs after a new item has been inserted, and fires IncludeComplete
        /// </summary>
        /// <param name="index">Index where the item should be inserted.</param>
        /// <param name="value">Item that should be inserted.</param>
        protected virtual void OnInsertComplete(int index, object value)
        {
            T item = value as T;
            OnIncludeComplete(item);
        }

        /// <summary>
        /// Occurs whenever a layer is moved.
        /// </summary>
        /// <param name="item">Layer that is moved.</param>
        /// <param name="newPosition">Position the layer is moved to.</param>
        protected virtual void OnMoved(T item, int newPosition)
        {
        }

        /// <summary>
        /// Fires after the remove operation, ensuring that OnExclude gets called
        /// </summary>
        /// <param name="index">Index where the item should be removed.</param>
        /// <param name="value">Item that should be removed.</param>
        protected virtual void OnRemoveComplete(int index, object value)
        {
            T item = value as T;
            Exclude(item);
        }

        /// <summary>
        /// Fires before the set operation ensuring the new item is included if necessary.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnSet(int index, object oldValue, object newValue)
        {
            T item = newValue as T;
            Include(item);
        }

        /// <summary>
        /// Fires after the set operation, ensuring that the item is removed
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnSetComplete(int index, object oldValue, object newValue)
        {
            T item = oldValue as T;
            Exclude(item);
            item = newValue as T;
            OnIncludeComplete(item);
        }

        #endregion
    }
}