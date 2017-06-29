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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 4:51:07 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// SubclassList is brilliant if I do say so myself.  Let's say you have a list of IPointCategory, which is a subclass of IFeatureCategory.
    /// You can't have a strong typed List that is both without creating a special class that can deal with the specific implementations of
    /// each.  So I was writing all of the stupid type specific collection classes, when in reality, I just needed a class like this
    /// that could handle the business logic.  Then specific instances can just inherit from this class, but specify the two types.
    /// </summary>
    /// <typeparam name="TBase">The base type that is inherited</typeparam>
    /// <typeparam name="TSub">The sub type that inherits from the base type</typeparam>
    public class SubclassList<TBase, TSub>
        : ChangeEventList<TBase>, IChangeEventList<TSub>
        where TSub : class, TBase
        where TBase : class, IChangeItem
    {
        #region Enumerator

        /// <summary>
        /// I can't simply return the enumerator of the internal list because it would be the wrong type.
        /// Furthermore, I need to know both types so that I can cast them appropriately.
        /// </summary>
        /// <typeparam name="TB">The base type that is inherited</typeparam>
        /// <typeparam name="TS">The sub type that inherits from the base type</typeparam>
        public class SubclassListEnumerator<TB, TS> : IEnumerator<TS>
            where TS : class, TB
        {
            private readonly IEnumerator<TB> _internalEnumerator;

            /// <summary>
            /// Creates a new instance of the enumerator of the sub-type given an enumerator
            /// of the base type.
            /// </summary>
            /// <param name="internalEnumerator">The internal enumerator.</param>
            public SubclassListEnumerator(IEnumerator<TB> internalEnumerator)
            {
                _internalEnumerator = internalEnumerator;
            }

            #region IEnumerator<TS> Members

            /// <summary>
            /// Gets the current member returned as type TSub.
            /// </summary>
            public TS Current
            {
                get { return _internalEnumerator.Current as TS; }
            }

            object IEnumerator.Current
            {
                get { return _internalEnumerator.Current; }
            }

            /// <summary>
            /// Disposes any unmanaged memory objects
            /// </summary>
            public void Dispose()
            {
                _internalEnumerator.Dispose();
            }

            /// <summary>
            /// Advances the enumerator to the next position
            /// </summary>
            /// <returns>Boolean, false at the end of the list.</returns>
            public bool MoveNext()
            {
                return _internalEnumerator.MoveNext();
            }

            /// <summary>
            /// Resets the enumerator to the beginning of the list
            /// </summary>
            public void Reset()
            {
                _internalEnumerator.Reset();
            }

            #endregion
        }

        #endregion

        #region Private Variables

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Allows adding of the sub type
        /// </summary>
        /// <param name="item"></param>
        public void Add(TSub item)
        {
            Add(item as TBase);
        }

        /// <summary>
        /// Adds the entire range of enumerable elements.
        /// </summary>
        /// <param name="items">The items to add</param>
        public void AddRange(IEnumerable<TSub> items)
        {
            foreach (TSub item in items)
            {
                Add(item as TBase);
            }
        }

        /// <summary>
        /// Tests to see if the list contains the specified item.
        /// </summary>
        /// <param name="item">The item to check the list for.</param>
        /// <returns>Boolean, true if the item is contained in the list.</returns>
        public bool Contains(TSub item)
        {
            return Contains(item as TBase);
        }

        /// <summary>
        /// Copies each of the members to the specified array.
        /// </summary>
        /// <param name="array">The array to copy items to.</param>
        /// <param name="arrayIndex">The zero based integer index in the destination array where the first item should be saved.</param>
        public void CopyTo(TSub[] array, int arrayIndex)
        {
            int index = arrayIndex;
            for (int i = 0; i < Count; i++)
            {
                if (index > array.GetUpperBound(0)) break;
                array[index] = base[i] as TSub;
                index++;
            }
        }

        /// <summary>
        /// Gets an enumerator of type TSub for cycling through the list.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<TSub> GetEnumerator()
        {
            return new SubclassListEnumerator<TBase, TSub>(base.GetEnumerator());
        }

        /// <summary>
        /// Gets the zero based integer index of the specified item of type TSub
        /// </summary>
        /// <param name="item">The item to obtain the index of</param>
        /// <returns>The zero based integer index of the specified item</returns>
        public int IndexOf(TSub item)
        {
            return IndexOf(item as TBase);
        }

        /// <summary>
        /// inserts the specified item into the specified index
        /// </summary>
        /// <param name="index">The zero based integer index where insertion can take place</param>
        /// <param name="item">The item to insert of type TSub</param>
        public void Insert(int index, TSub item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// Removes the specified item from the list.
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>Boolean, true if the item was found in the list and removed</returns>
        public bool Remove(TSub item)
        {
            return Remove(item as TBase);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the specific item in the list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new TSub this[int index]
        {
            get
            {
                return base[index] as TSub;
            }
            set
            {
                base[index] = value;
            }
        }

        #endregion
    }
}