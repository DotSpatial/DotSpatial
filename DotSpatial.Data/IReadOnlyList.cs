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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/17/2009 1:22:46 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// IReadOnlyList
    /// </summary>
    public interface IReadOnlyList<T> : IEnumerable<T>
    {
        #region Methods

        /// <summary>
        /// Tests to see if the specified item is contained in the list.  This returns true if the item is contained in the list.
        /// </summary>
        /// <param name="item">The item to test for.</param>
        /// <returns>Boolean, true if the item is found in the list</returns>
        bool Contains(T item);

        /// <summary>
        /// Copies the specified memebers into the array, starting at the specified index.
        /// </summary>
        /// <param name="array">The array to copy values to.</param>
        /// <param name="arrayIndex">The array index where the 0 member of this list should be copied to.</param>
        void CopyTo(T[] array, int arrayIndex);

        /// <summary>
        /// Obtains the index of the specified item
        /// </summary>
        /// <param name="item">The item to find the index of</param>
        /// <returns>An integer representing the index of the specified item</returns>
        int IndexOf(T item);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer count of items in this list.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Return true because this is a read-only list.
        /// </summary>
        bool IsReadOnly
        {
            get;
        }

        /// <summary>
        /// Gets the item at the specified index.  Ideally, this ReadOnlyList is used with
        /// value types, or else this gives the user considerable power over the core content.
        /// </summary>
        /// <param name="index">The item to obtain from this list</param>
        /// <returns>The item at the specified index.</returns>
        T this[int index]
        {
            get;
        }

        #endregion
    }
}