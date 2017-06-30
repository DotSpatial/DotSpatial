// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
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
    /// Represents a collection of rows for a IDataTable.
    /// </summary>
    public interface IDataRowCollection: IEnumerable
    {
        /// <summary>
        /// Gets the total number of IDataRow objects in this collection.
        /// </summary>
        int Count 
        { 
            get; 
        }

        /// <summary>
        /// Gets the row at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the row to return. </param>
        /// <returns>The specified IDataRow.</returns>
        IDataRow this[int index] 
        { 
            get;
        }

        /// <summary>
        /// Adds the specified IDataRow to the IDataRowCollection object.
        /// </summary>
        /// <param name="row">The IDataRow to add.</param>
        void Add(IDataRow row);

        /// <summary>
        /// Creates a row using specified values and adds it to the IDataRowCollection.
        /// </summary>
        /// <param name="values">The array of values that are used to create the new row. </param>
        /// <returns></returns>
        IDataRow Add(params object[] values);

        /// <summary>
        /// Clears the collection of all rows.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets a value that indicates whether the primary key of any row in the collection contains the specified value.
        /// </summary>
        /// <param name="key">The value of the primary key to test for. </param>
        /// <returns>true if the collection contains a DataRow with the specified primary key value; otherwise, false.</returns>
        bool Contains(object key);

        /// <summary>
        /// Gets a value that indicates whether the primary key columns of any row in the collection contain the values specified in the object array.
        /// </summary>
        /// <param name="keys">An array of primary key values to test for. </param>
        /// <returns>true if the IDataRowCollection contains a IDataRow with the specified key values; otherwise, false.</returns>
        bool Contains(object[] keys);

        /// <summary>
        /// Copies all the IDataRow objects from the collection into the given array, starting at the given destination array index.
        /// </summary>
        /// <param name="ar">The one-dimensional array that is the destination of the elements copied from the IDataRowCollection.</param>
        /// <param name="index">The zero-based index in the array at which copying begins.</param>
        void CopyTo(Array ar, int index);

        /// <summary>
        /// Copies all the IDataRow objects from the collection into the given array, starting at the given destination array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the IDataRowCollection.</param>
        /// <param name="index">The zero-based index in the array at which copying begins.</param>
        void CopyTo(IDataRow[] array, int index);

        /// <summary>
        /// Gets the row specified by the primary key value.
        /// </summary>
        /// <param name="key">The primary key value of the DataRow to find. </param>
        /// <returns>An IDataRow that contains the primary key value specified; otherwise a null value if the primary key value does not exist in the IDataRowCollection.</returns>
        IDataRow Find(object key);

        /// <summary>
        /// Gets the row that contains the specified primary key values.
        /// </summary>
        /// <param name="keys">An array of primary key values to find.</param>
        /// <returns>A IDataRow object that contains the primary key values specified; otherwise a null value if the primary key value does not exist in the IDataRowCollection.</returns>
        IDataRow Find(object[] keys);

        /// <summary>
        /// Gets the index of the specified IDataRow object.
        /// </summary>
        /// <param name="row">The IDataRow to search for.</param>
        /// <returns>The zero-based index of the row, or -1 if the row is not found in the collection.</returns>
        int IndexOf(IDataRow row);

        /// <summary>
        /// Inserts a new row into the collection at the specified location.
        /// </summary>
        /// <param name="row">The IDataRow to add. </param>
        /// <param name="pos">The (zero-based) location in the collection where you want to add the IDataRow. </param>
        void InsertAt(IDataRow row, int pos);

        /// <summary>
        /// Removes the specified IDataRow from the collection.
        /// </summary>
        /// <param name="row">The IDataRow to remove. </param>
        void Remove(IDataRow row);

        /// <summary>
        /// Removes the row at the specified index from the collection.
        /// </summary>
        /// <param name="index">The index of the row to remove. </param>
        void RemoveAt(int index);
    }
}