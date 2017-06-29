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
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// A list that also includes several events during its existing activities.
    /// List is fussy about inheritance, unfortunately, so this wraps a list
    /// and then makes this class much more inheritable
    /// </summary>
    public class ChangeEventList<T> : CopyList<T>, IChangeEventList<T> where T : class, IChangeItem
    {
        #region Events

        /// <summary>
        /// This event is for when it is necessary to do something if any of the internal
        /// members changes.  It will also forward the original item calling the message.
        /// </summary>
        public event EventHandler ItemChanged;

        /// <summary>
        /// Occurs when this list should be removed from its container
        /// </summary>
        public event EventHandler RemoveItem;

        #endregion

        #region variables

        private bool _hasChanged;
        private int _suspension;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #region Add

        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.List&lt;T&gt;
        /// </summary>
        /// <param name="collection">collection: The collection whose elements should be added to the end of the
        /// System.Collections.Generic.List&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null,
        /// if type T is a reference type.</param>
        /// <exception cref="System.ApplicationException">Unable to add while the ReadOnly property is set to true.</exception>
        public virtual void AddRange(IEnumerable<T> collection)
        {
            SuspendEvents();
            foreach (T item in collection)
            {
                Add(item);
            }
            ResumeEvents();
        }

        #endregion

        /// <summary>
        /// Resumes event sending and fires a ListChanged event if any changes have taken place.
        /// This will not track all the individual changes that may have fired in the meantime.
        /// </summary>
        public void ResumeEvents()
        {
            _suspension--;
            if (_suspension == 0)
            {
                OnResumeEvents();
                if (_hasChanged)
                {
                    OnListChanged();
                }
            }
            if (_suspension < 0) _suspension = 0;
        }

        /// <summary>
        /// Temporarilly suspends notice events, allowing a large number of changes.
        /// </summary>
        public void SuspendEvents()
        {
            if (_suspension == 0) _hasChanged = false;
            _suspension++;
        }

        /// <summary>
        /// An overriding event handler so that we can signfiy the list has changed
        /// when the inner list has been set to a new list.
        /// </summary>
        protected override void OnInnerListSet()
        {
            OnItemChanged(this);
        }

        /// <summary>
        /// Overrides the normal clear situation so that we only update after all the members are cleared.
        /// </summary>
        protected override void OnClear()
        {
            SuspendEvents();
            base.OnClear();
            ResumeEvents();
        }

        /// <summary>
        /// Occurs during the copy process and overrides the base behavior so that events are suspended.
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(CopyList<T> copy)
        {
            ChangeEventList<T> myCopy = copy as ChangeEventList<T>;
            if (myCopy != null)
            {
                RemoveHandlers(myCopy);
                myCopy.SuspendEvents();
            }
            base.OnCopy(copy);
            if (myCopy != null) myCopy.ResumeEvents();
        }

        private static void RemoveHandlers(ChangeEventList<T> myCopy)
        {
            if (myCopy.ItemChanged != null)
            {
                foreach (var handler in myCopy.ItemChanged.GetInvocationList())
                {
                    myCopy.ItemChanged -= (EventHandler)handler;
                }
            }
            if (myCopy.RemoveItem != null)
            {
                foreach (var handler in myCopy.RemoveItem.GetInvocationList())
                {
                    myCopy.RemoveItem -= (EventHandler)handler;
                }
            }
        }

        /// <summary>
        /// Occurs when ResumeEvents has been called enough times so that events are re-enabled.
        /// </summary>
        protected virtual void OnResumeEvents()
        {
        }

        #region Reverse

        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the EventList&lt;T&gt;.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ApplicationException">Unable to reverse while the ReadOnly property is set to true.</exception>
        public virtual void Reverse(int index, int count)
        {
            InnerList.Reverse(index, count);
            OnListChanged();
        }

        /// <summary>
        /// Reverses the order of the elements in the entire EventList&lt;T&gt;.
        /// </summary>
        /// <exception cref="System.ApplicationException">Unable to reverse while the ReadOnly property is set to true.</exception>
        public virtual void Reverse()
        {
            InnerList.Reverse();
            OnListChanged();
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts the elements of a collection into the EventList&lt;T&gt; at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the EventList&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-index is greater than EventList&lt;T&gt;.Count.</exception>
        /// <exception cref="System.ArgumentNullException">collection is null.</exception>
        /// <exception cref="System.ApplicationException">Unable to insert while the ReadOnly property is set to true.</exception>
        public virtual void InsertRange(int index, IEnumerable<T> collection)
        {
            int c = index;
            SuspendEvents();
            foreach (T item in collection)
            {
                Include(item);
                InnerList.Insert(c, item);
                c++;
            }
            ResumeEvents();
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes a range of elements from the EventList&lt;T&gt;.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the EventList&lt;T&gt;.</exception>
        /// <exception cref="System.ApplicationException">Unable to remove while the ReadOnly property is set to true.</exception>
        public virtual void RemoveRange(int index, int count)
        {
            T[] temp = new T[count];
            InnerList.CopyTo(index, temp, 0, count);
            InnerList.RemoveRange(index, count);
            SuspendEvents();
            foreach (T item in temp)
            {
                Exclude(item);
            }
            ResumeEvents();
        }

        #endregion

        #region BinarySearch

        /// <summary>
        /// Searches the entire sorted System.Collections.Generic.List&lt;T&gt; for an element using the default comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <returns>The zero-based index of item in the sorted System.Collections.Generic.List&lt;T&gt;, if item is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, if there is no larger element, the bitwise complement of System.Collections.Generic.List&lt;T&gt;.Count.</returns>
        /// <exception cref="System.InvalidOperationException">The default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default cannot find an implementation of the System.IComparable&lt;T&gt; generic interface or the System.IComparable interface for type T.</exception>
        public virtual int BinarySearch(T item)
        {
            return InnerList.BinarySearch(item);
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the list is currently suspended
        /// </summary>
        public bool EventsSuspended
        {
            get
            {
                return (_suspension > 0);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// This is a notification that characteristics of one of the members of the list may have changed,
        /// requiring a refresh, but may not involve a change to the the list itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemItemChanged(object sender, EventArgs e)
        {
            OnItemChanged(sender);
        }

        /// <summary>
        /// Occurs when the item is changed.  If this list is not suspended, it will forward the change event
        /// on.  Otherwise, it will ensure that when resume events is called that the on change method
        /// is fired at that time.
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnItemChanged(object sender)
        {
            if (EventsSuspended)
            {
                _hasChanged = true;
            }
            else
            {
                if (ItemChanged != null) ItemChanged(sender, EventArgs.Empty);
            }
        }

        private void ItemRemoveItem(object sender, EventArgs e)
        {
            Remove((T)sender);
            OnListChanged();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the ListChanged Event
        /// </summary>
        protected virtual void OnListChanged()
        {
            if (EventsSuspended == false)
            {
                if (ItemChanged != null)
                {
                    // activate this remarked code to test if the handlers are getting copied somewhere.
                    //int count = ItemChanged.GetInvocationList().Length;
                    //if (count > 1) Debug.WriteLine(this + " has " + count + " item changed handlers.");
                    ItemChanged(this, EventArgs.Empty);
                }
            }
            else
            {
                _hasChanged = true;
            }
        }

        /// <summary>
        /// This is either a layer collection or a colorbreak collection, and so
        /// this won't be called by us, but someone might want to override this for their own reasons.
        /// </summary>
        protected virtual void OnRemoveItem()
        {
            if (RemoveItem != null) RemoveItem(this, EventArgs.Empty);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when wiring events on a new item
        /// </summary>
        /// <param name="item"></param>
        protected override void OnInclude(T item)
        {
            item.ItemChanged += ItemItemChanged;
            item.RemoveItem += ItemRemoveItem;
            OnListChanged();
        }

        /// <summary>
        /// Occurs when unwiring events on new items
        /// </summary>
        /// <param name="item"></param>
        protected override void OnExclude(T item)
        {
            item.ItemChanged -= ItemItemChanged;
            item.RemoveItem -= ItemRemoveItem;
            OnListChanged();
        }

        #endregion
    }
}