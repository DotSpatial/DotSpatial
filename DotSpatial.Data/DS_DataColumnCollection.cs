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
    /// Represents a collection of DataColumn objects for a IDataTable.
    /// </summary>
    public class DS_DataColumnCollection: IDataColumnCollection
    {
        private DataColumnCollection _dataColumnCollection;

        /// <summary>
        /// Constructor of DS_DataColumnCollection from a DataColumnCollection
        /// </summary>
        /// <param name="collection">The specified DataColumnCollection</param>
        public DS_DataColumnCollection(DataColumnCollection collection)
        {
            _dataColumnCollection = collection;
        }
    
        /// <summary>
        /// Gets the total number of objects in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _dataColumnCollection.Count;
            }
        }

        /// <summary>
        /// Gets the DataColumn from the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the column to return. </param>
        /// <returns>The DataColumn at the specified index.</returns>
        public DataColumn this[int index]
        {
            get { 
                return _dataColumnCollection[index]; 
            }
        }

        /// <summary>
        /// Gets the DataColumn from the collection with the specified name.
        /// </summary>
        /// <param name="name">The ColumnName of the column to return. </param>
        /// <returns>The DataColumn in the collection with the specified ColumnName; otherwise a null value if the DataColumn does not exist.</returns>
        public DataColumn this[string name]
        {
	        get {
                return _dataColumnCollection[name];
            }
        }

        /// <summary>
        /// Creates and adds a DataColumn object to the IDataColumnCollection.
        /// </summary>
        /// <returns>The newly created DataColumn.</returns>
        public DataColumn Add()
        {
            return _dataColumnCollection.Add();
        }

        /// <summary>
        /// Creates and adds the specified DataColumn object to the IDataColumnCollection.
        /// </summary>
        /// <param name="column">The DataColumn to add.</param>
        public void Add(DataColumn column)
        {
            _dataColumnCollection.Add(column);
        }

        /// <summary>
        /// Creates and adds a DataColumn object that has the specified name to the IDataColumnCollection.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The newly created DataColumn.</returns>
        public DataColumn Add(string columnName)
        {
            return _dataColumnCollection.Add(columnName);
        }

        /// <summary>
        /// Creates and adds a DataColumn object that has the specified name and type to the IDataColumnCollection.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="type">The DataType of the new column.</param>
        /// <returns>The newly created DataColumn.</returns>
        public DataColumn Add(string columnName, Type type)
        {
            return _dataColumnCollection.Add(columnName, type);
        }

        /// <summary>
        /// Creates and adds a DataColumn object that has the specified name, type, and expression to the IDataColumnCollection.
        /// </summary>
        /// <param name="columnName">The name to use when you create the column. </param>
        /// <param name="type">The DataType of the new column. </param>
        /// <param name="expression">The expression to assign to the Expression property. </param>
        /// <returns>The newly created DataColumn.</returns>
        public DataColumn Add(string columnName, Type type, string expression)
        {
            return _dataColumnCollection.Add(columnName, type, expression);
        }

        /// <summary>
        /// Copies the elements of the specified DataColumn array to the end of the collection.
        /// </summary>
        /// <param name="columns">The array of DataColumn objects to add to the collection.</param>
        public void AddRange(DataColumn[] columns)
        {
            _dataColumnCollection.AddRange(columns);
        }

        /// <summary>
        /// Clears the collection of any columns.
        /// </summary>
        public void Clear()
        {
            _dataColumnCollection.Clear();
        }

        /// <summary>
        /// Checks whether the collection contains a column with the specified name.
        /// </summary>
        /// <param name="name">The ColumnName of the column to look for. </param>
        /// <returns>true if a column exists with this name; otherwise, false.</returns>
        public bool Contains(string name)
        {
            return _dataColumnCollection.Contains(name);
        }

        /// <summary>
        /// Gets an IEnumerator for the collection.
        /// </summary>
        /// <returns>An IEnumerator for the collection.</returns>
        public virtual IEnumerator GetEnumerator()
        {
            return _dataColumnCollection.GetEnumerator();
        }

        /// <summary>
        /// Gets the index of a column specified by name.
        /// </summary>
        /// <param name="column">The name of the column to return. </param>
        /// <returns>The index of the column specified by column if it is found; otherwise, -1.</returns>
        public int IndexOf(DataColumn column)
        {
            return _dataColumnCollection.IndexOf(column);
        }

        /// <summary>
        /// Gets the index of the column with the specific name (the name is not case sensitive).
        /// </summary>
        /// <param name="columnName">The name of the column to find. </param>
        /// <returns>The zero-based index of the column with the specified name, or -1 if the column does not exist in the collection.</returns>
        public int IndexOf(string columnName)
        {
            return _dataColumnCollection.IndexOf(columnName);
        }

        /// <summary>
        /// Removes the specified DataColumn object from the collection.
        /// </summary>
        /// <param name="column">The DataColumn to remove. </param>
        public void Remove(DataColumn column)
        {
            _dataColumnCollection.Remove(column);
        }

        /// <summary>
        /// Removes the DataColumn object that has the specified name from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.  </param>
        public void Remove(string name)
        {
            _dataColumnCollection.Remove(name);
        }
    }
}