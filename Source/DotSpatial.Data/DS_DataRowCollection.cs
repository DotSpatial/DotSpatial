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
    public class DS_DataRowCollection: IDataRowCollection
    {
        private DataRowCollection _dataRowCollection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public DS_DataRowCollection(DataRowCollection collection)
        {
            _dataRowCollection = collection;
        }


        /// <summary>
        /// Gets the total number of IDataRow objects in this collection.
        /// </summary>
        public int Count 
        {
            get
            {
                return _dataRowCollection.Count;
            }
        }

        /// <summary>
        /// Gets the row at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the row to return. </param>
        /// <returns>The specified IDataRow.</returns>
        public IDataRow this[int index] 
        {
            get
            {
                return new DS_DataRow(_dataRowCollection[index]);
            }
        }

        /// <summary>
        /// Adds the specified IDataRow to the IDataRowCollection object.
        /// </summary>
        /// <param name="row">The IDataRow to add.</param>
        public void Add(IDataRow row)
        {
            if(row is DS_DataRow)
                _dataRowCollection.Add(((DS_DataRow)row).dataRow);
            else
                throw new Exception("Data row type is not supported");
        }

        /// <summary>
        /// Creates a row using specified values and adds it to the IDataRowCollection.
        /// </summary>
        /// <param name="values">The array of values that are used to create the new row. </param>
        /// <returns></returns>
        public IDataRow Add(params object[] values)
        {
            return new DS_DataRow(_dataRowCollection.Add(values));
        }

        /// <summary>
        /// Clears the collection of all rows.
        /// </summary>
        public void Clear()
        {
            _dataRowCollection.Clear();
        }

        /// <summary>
        /// Gets a value that indicates whether the primary key of any row in the collection contains the specified value.
        /// </summary>
        /// <param name="key">The value of the primary key to test for. </param>
        /// <returns>true if the collection contains a DataRow with the specified primary key value; otherwise, false.</returns>
        public bool Contains(object key)
        {
            return _dataRowCollection.Contains(key);
        }

        /// <summary>
        /// Gets a value that indicates whether the primary key columns of any row in the collection contain the values specified in the object array.
        /// </summary>
        /// <param name="keys">An array of primary key values to test for. </param>
        /// <returns>true if the IDataRowCollection contains a IDataRow with the specified key values; otherwise, false.</returns>
        public bool Contains(object[] keys)
        {
            return _dataRowCollection.Contains(keys);
        }

        /// <summary>
        /// Copies all the IDataRow objects from the collection into the given array, starting at the given destination array index.
        /// </summary>
        /// <param name="ar">The one-dimensional array that is the destination of the elements copied from the IDataRowCollection.</param>
        /// <param name="index">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(Array ar, int index)
        {
            _dataRowCollection.CopyTo(ar, index);
        }

        /// <summary>
        /// Copies all the IDataRow objects from the collection into the given array, starting at the given destination array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the IDataRowCollection.</param>
        /// <param name="index">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(IDataRow[] array, int index)
        {
            DataRow[] rowArray = new DataRow[array.Length];
            _dataRowCollection.CopyTo(rowArray, index);
            for (int iRow = 0; iRow < rowArray.Length; iRow++)
            {
                array[iRow] = new DS_DataRow(rowArray[iRow]);
            }
        }

        /// <summary>
        /// Gets the row specified by the primary key value.
        /// </summary>
        /// <param name="key">The primary key value of the DataRow to find. </param>
        /// <returns>An IDataRow that contains the primary key value specified; otherwise a null value if the primary key value does not exist in the IDataRowCollection.</returns>
        public IDataRow Find(object key)
        {
            return new DS_DataRow(_dataRowCollection.Find(key));
        }

        /// <summary>
        /// Gets the row that contains the specified primary key values.
        /// </summary>
        /// <param name="keys">An array of primary key values to find.</param>
        /// <returns>A IDataRow object that contains the primary key values specified; otherwise a null value if the primary key value does not exist in the IDataRowCollection.</returns>
        public IDataRow Find(object[] keys)
        {
            return new DS_DataRow(_dataRowCollection.Find(keys));
        }

        /// <summary>
        /// Gets an IEnumerator for this collection.
        /// </summary>
        /// <returns>An IEnumerator for this collection.</returns>
        public IEnumerator GetEnumerator()
        {
            List<DS_DataRow> list = new List<DS_DataRow>();

            foreach (var dr in _dataRowCollection)
            {
                list.Add(new DS_DataRow((DataRow)dr));
            }

            return list.GetEnumerator();
        }

        /// <summary>
        /// Gets the index of the specified IDataRow object.
        /// </summary>
        /// <param name="row">The IDataRow to search for.</param>
        /// <returns>The zero-based index of the row, or -1 if the row is not found in the collection.</returns>
        public int IndexOf(IDataRow row)
        {
            if(row is DS_DataRow)
                return _dataRowCollection.IndexOf(((DS_DataRow)row).dataRow);
            else
                throw new Exception("Data row type is not supported");
        }

        /// <summary>
        /// Inserts a new row into the collection at the specified location.
        /// </summary>
        /// <param name="row">The IDataRow to add. </param>
        /// <param name="pos">The (zero-based) location in the collection where you want to add the IDataRow. </param>
        public void InsertAt(IDataRow row, int pos)
        {
            if (row is DS_DataRow)
                _dataRowCollection.InsertAt(((DS_DataRow)row).dataRow, pos);
            else
                throw new Exception("Data row type is not supported");
        }

        /// <summary>
        /// Removes the specified IDataRow from the collection.
        /// </summary>
        /// <param name="row">The IDataRow to remove. </param>
        public void Remove(IDataRow row)
        {
            if (row is DS_DataRow)
                _dataRowCollection.Remove(((DS_DataRow)row).dataRow);
            else
                throw new Exception("Data row type is not supported");
        }

        /// <summary>
        /// Removes the row at the specified index from the collection.
        /// </summary>
        /// <param name="index">The index of the row to remove. </param>
        public void RemoveAt(int index)
        {
            _dataRowCollection.RemoveAt(index);
        }
    }
}