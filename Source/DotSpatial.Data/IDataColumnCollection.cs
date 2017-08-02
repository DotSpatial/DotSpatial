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
using System.Data;

namespace DotSpatial.Data
{
    /// <summary>
    /// Represents a collection of DataColumn objects for a IDataTable.
    /// </summary>
    public interface IDataColumnCollection : IEnumerable
    {
        /// <summary>
        /// Gets the total number of objects in this collection.
        /// </summary>
        int Count 
        { 
            get; 
        }

        /// <summary>
        /// Gets the DataColumn from the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the column to return. </param>
        /// <returns>The DataColumn at the specified index.</returns>
        DataColumn this[int index] 
        { 
            get;
        }

        /// <summary>
        /// Gets the DataColumn from the collection with the specified name.
        /// </summary>
        /// <param name="name">The ColumnName of the column to return. </param>
        /// <returns>The DataColumn in the collection with the specified ColumnName; otherwise a null value if the DataColumn does not exist.</returns>
        DataColumn this[string name]
        {
            get;
        }

        /// <summary>
        /// Creates and adds a DataColumn object to the IDataColumnCollection.
        /// </summary>
        /// <returns>The newly created DataColumn.</returns>
        DataColumn Add();

        /// <summary>
        /// Creates and adds the specified DataColumn object to the IDataColumnCollection.
        /// </summary>
        /// <param name="column">The DataColumn to add.</param>
        void Add(DataColumn column);

        /// <summary>
        /// Creates and adds a DataColumn object that has the specified name to the IDataColumnCollection.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The newly created DataColumn.</returns>
        DataColumn Add(string columnName);

        /// <summary>
        /// Creates and adds a DataColumn object that has the specified name and type to the IDataColumnCollection.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="type">The DataType of the new column.</param>
        /// <returns>The newly created DataColumn.</returns>
        DataColumn Add(string columnName, Type type);

        /// <summary>
        /// Creates and adds a DataColumn object that has the specified name, type, and expression to the IDataColumnCollection.
        /// </summary>
        /// <param name="columnName">The name to use when you create the column. </param>
        /// <param name="type">The DataType of the new column. </param>
        /// <param name="expression">The expression to assign to the Expression property. </param>
        /// <returns>The newly created DataColumn.</returns>
        DataColumn Add(string columnName, Type type, string expression);

        /// <summary>
        /// Copies the elements of the specified DataColumn array to the end of the collection.
        /// </summary>
        /// <param name="columns">The array of DataColumn objects to add to the collection.</param>
        void AddRange(DataColumn[] columns);

        /// <summary>
        /// Clears the collection of any columns.
        /// </summary>
        void Clear();

        /// <summary>
        /// Checks whether the collection contains a column with the specified name.
        /// </summary>
        /// <param name="name">The ColumnName of the column to look for. </param>
        /// <returns>true if a column exists with this name; otherwise, false.</returns>
        bool Contains(string name);

        /// <summary>
        /// Gets the index of a column specified by name.
        /// </summary>
        /// <param name="column">The name of the column to return. </param>
        /// <returns>The index of the column specified by column if it is found; otherwise, -1.</returns>
        int IndexOf(DataColumn column);

        /// <summary>
        /// Gets the index of the column with the specific name (the name is not case sensitive).
        /// </summary>
        /// <param name="columnName">The name of the column to find. </param>
        /// <returns>The zero-based index of the column with the specified name, or -1 if the column does not exist in the collection.</returns>
        int IndexOf(string columnName);

        /// <summary>
        /// Removes the specified DataColumn object from the collection.
        /// </summary>
        /// <param name="column">The DataColumn to remove. </param>
        void Remove(DataColumn column);

        /// <summary>
        /// Removes the DataColumn object that has the specified name from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.  </param>
        void Remove(string name);
    }
}