// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace DotSpatial.Data
{
    /// <summary>
    /// The attribute cache caches the attributes from the dbf file.
    /// </summary>
    public class AttributeCache : IEnumerable<Dictionary<string, object>>
    {
        private static int rowsPerPage;
        private readonly IAttributeSource _dataSupply;
        private int _editRowIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCache"/> class that can create data tables by using a DataPageRetriever.
        /// </summary>
        /// <param name="dataSupplier">Any structure that implements IDataPageRetriever.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        public AttributeCache(IAttributeSource dataSupplier, int rowsPerPage)
        {
            _dataSupply = dataSupplier;
            AttributeCache.rowsPerPage = rowsPerPage;
            _editRowIndex = -1;
            LoadFirstTwoPages();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the row being edited.
        /// </summary>
        public Dictionary<string, object> EditRow { get; set; }

        /// <summary>
        /// Gets or sets the integer index of the row being edited.
        /// </summary>
        public int EditRowIndex
        {
            get
            {
                return _editRowIndex;
            }

            set
            {
                if (value < 0 || value >= _dataSupply.NumRows())
                {
                    EditRow = null;
                    return;
                }

                EditRow = RetrieveElement(value);
                _editRowIndex = value;
            }
        }

        /// <summary>
        /// Gets the number of rows in the data supply.
        /// </summary>
        public int NumRows => _dataSupply.NumRows();

        /// <summary>
        /// Gets or sets the pages currently stored in the cache.
        /// </summary>
        public DataPage[] Pages { get; set; }

        #endregion

        /// <inheritdoc />
        public IEnumerator<Dictionary<string, object>> GetEnumerator()
        {
            return new AttributeCacheEnumerator(this);
        }

        #region Methods

        /// <summary>
        /// Obtains the element at the specified index.
        /// </summary>
        /// <param name="rowIndex">Index of the row that should be retrieved.</param>
        /// <returns>The retrieved row.</returns>
        public Dictionary<string, object> RetrieveElement(int rowIndex)
        {
            if (EditRowIndex == rowIndex)
            {
                return EditRow;
            }

            Dictionary<string, object> element = new();
            if (IfPageCachedThenSetElement(rowIndex, ref element))
            {
                return element;
            }

            return RetrieveDataCacheItThenReturnElement(rowIndex);
        }

        /// <summary>
        /// Obtains the element at the specified index.
        /// </summary>
        /// <param name="rowIndex">Index of the row that contains the element.</param>
        /// <param name="columnIndex">Index of the column that contains the element.</param>
        /// <returns>Element found at the specified index.</returns>
        public object RetrieveElement(int rowIndex, int columnIndex)
        {
            DataColumn[] columns = _dataSupply.GetColumns();
            if (rowIndex == _editRowIndex && EditRow != null)
            {
                return EditRow[columns[columnIndex].ColumnName];
            }

            object element;
            if (IfPageCachedThenSetElement(rowIndex, columnIndex, out element))
            {
                return element;
            }

            return RetrieveDataCacheItThenReturnElement(rowIndex, columnIndex);
        }

        /// <summary>
        /// Saves the changes in the edit row to the tabular cache as well as the underlying database.
        /// </summary>
        public void SaveChanges()
        {
            _dataSupply.Edit(EditRowIndex, EditRow);
            DataRow dr = null;
            if (IsRowCachedInPage(0, _editRowIndex))
            {
                dr = Pages[0].Table.Rows[_editRowIndex];
            }

            if (IsRowCachedInPage(1, _editRowIndex))
            {
                dr = Pages[1].Table.Rows[_editRowIndex];
            }

            if (dr == null) return;
            foreach (KeyValuePair<string, object> pair in EditRow)
            {
                dr[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Gets an enumperator that enumerates the dictionaries that represent the row content stored by field name.
        /// </summary>
        /// <returns>An enumperator that enumerates the dictionaries that represent the row content stored by field name.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new AttributeCacheEnumerator(this);
        }

        /// <summary>
        /// Returns the index of the cached page most distant from the given index and therefore least likely to be reused.
        /// </summary>
        /// <param name="rowIndex">Row index that is used to find the most distant page.</param>
        /// <returns>The index of the cached page most distant from the given index and therefore least likely to be reused.</returns>
        private int GetIndexToUnusedPage(int rowIndex)
        {
            if (rowIndex > Pages[0].HighestIndex && rowIndex > Pages[1].HighestIndex)
            {
                int offsetFromPage0 = rowIndex - Pages[0].HighestIndex;
                int offsetFromPage1 = rowIndex - Pages[1].HighestIndex;
                return offsetFromPage0 < offsetFromPage1 ? 1 : 0;
            }
            else
            {
                int offsetFromPage0 = Pages[0].LowestIndex - rowIndex;
                int offsetFromPage1 = Pages[1].LowestIndex - rowIndex;
                return offsetFromPage0 < offsetFromPage1 ? 1 : 0;
            }
        }

        /// <summary>
        /// Sets the value of the element parameter if the value is in the cache.
        /// </summary>
        /// <param name="rowIndex">Index of the row that contains the data that gets assigned to element.</param>
        /// <param name="columnIndex">Index of the column that contains the data that gets assigned to element.</param>
        /// <param name="element">Element whose data gets set.</param>
        /// <returns>True, if the element was set or the index was bigger than the number of rows in the page.</returns>
        private bool IfPageCachedThenSetElement(int rowIndex, int columnIndex, out object element)
        {
            element = string.Empty;
            int index = rowIndex % rowsPerPage;

            // jany_: why check only the first 2 pages if Pages could have more than 2?
            for (int i = 0; i <= 1; i++)
            {
                if (IsRowCachedInPage(i, rowIndex))
                {
                    if (Pages[i].Table.Rows.Count <= index) return true;
                    element = Pages[i].Table.Rows[index][columnIndex];
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the value of the element parameter if the value is in the cache.
        /// </summary>
        /// <param name="rowIndex">Index of the row that contains the data that gets assigned to element.</param>
        /// <param name="element">Element whose data gets set.</param>
        /// <returns>True, if the data was set.</returns>
        private bool IfPageCachedThenSetElement(int rowIndex, ref Dictionary<string, object> element)
        {
            // jany_: why check only the first 2 pages if Pages could have more than 2?
            for (int i = 0; i <= 1; i++)
            {
                if (IsRowCachedInPage(i, rowIndex))
                {
                    DataRow dr = Pages[i].Table.Rows[rowIndex % rowsPerPage];
                    foreach (DataColumn column in Pages[0].Table.Columns)
                    {
                        string name = column.ColumnName;
                        element.Add(name, dr[name]);
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether the given row index is contained in the DataPage with the given pageNumber.
        /// </summary>
        /// <param name="pageNumber">Number of the page, that should contain the index.</param>
        /// <param name="rowIndex">Row index that should be contained by the page.</param>
        /// <returns>True, if the row index was found in the page with the given number.</returns>
        private bool IsRowCachedInPage(int pageNumber, int rowIndex)
        {
            return rowIndex <= Pages[pageNumber].HighestIndex && rowIndex >= Pages[pageNumber].LowestIndex;
        }

        /// <summary>
        /// Loads the first to Pages.
        /// </summary>
        private void LoadFirstTwoPages()
        {
            DataPage p1 = new(_dataSupply.GetAttributes(DataPage.MapToLowerBoundary(0), rowsPerPage), 0);
            DataPage p2 = new(_dataSupply.GetAttributes(DataPage.MapToLowerBoundary(rowsPerPage), rowsPerPage), rowsPerPage);
            Pages = new[] { p1, p2 };
        }

        private object RetrieveDataCacheItThenReturnElement(int rowIndex, int columnIndex)
        {
            // Retrieve a page worth of data containing the requested value.
            DataTable table = _dataSupply.GetAttributes(DataPage.MapToLowerBoundary(rowIndex), rowsPerPage);

            // Replace the cached page furthest from the requested cell
            // with a new page containing the newly retrieved data.
            Pages[GetIndexToUnusedPage(rowIndex)] = new DataPage(table, rowIndex);

            return RetrieveElement(rowIndex, columnIndex);
        }

        private Dictionary<string, object> RetrieveDataCacheItThenReturnElement(int rowIndex)
        {
            // Retrieve a page worth of data containing the requested value.
            DataTable table = _dataSupply.GetAttributes(DataPage.MapToLowerBoundary(rowIndex), rowsPerPage);

            // Replace the cached page furthest from the requested cell
            // with a new page containing the newly retrieved data.
            Pages[GetIndexToUnusedPage(rowIndex)] = new DataPage(table, rowIndex);

            return RetrieveElement(rowIndex);
        }

        #endregion

        #region Classes

        /// <summary>
        /// Represents one page of data.
        /// </summary>
        public struct DataPage
        {
            /// <summary>
            /// The data Table.
            /// </summary>
            public readonly DataTable Table;

            /// <summary>
            /// Initializes a new instance of the <see cref="DataPage"/> struct representing one page of data-row values.
            /// </summary>
            /// <param name="table">The DataTable that controls the content.</param>
            /// <param name="rowIndex">The integer row index.</param>
            public DataPage(DataTable table, int rowIndex)
            {
                Table = table;

                LowestIndex = MapToLowerBoundary(rowIndex);
                HighestIndex = MapToUpperBoundary(rowIndex);
                Debug.Assert(LowestIndex >= 0, "LowestIndex must >= 0");
                Debug.Assert(HighestIndex >= 0, "HighestIndex must >= 0");
            }

            /// <summary>
            /// Gets the integer lowest index of the page.
            /// </summary>
            public int LowestIndex { get; }

            /// <summary>
            /// Gets the integer highest index of the page.
            /// </summary>
            public int HighestIndex { get; }

            /// <summary>
            /// Given an arbitrary row index, this calculates what the lower boundary would be for the page containing the index.
            /// </summary>
            /// <param name="rowIndex">Row index used for calculation.</param>
            /// <returns>The lowest index of the page containing the given index.</returns>
            public static int MapToLowerBoundary(int rowIndex)
            {
                // Return the lowest index of a page containing the given index.
                return (rowIndex / rowsPerPage) * rowsPerPage;
            }

            /// <summary>
            /// Tests to see if the specified index is in this page.
            /// </summary>
            /// <param name="index">Index that should be checked.</param>
            /// <returns>True, if the page contains the index.</returns>
            public bool Contains(int index)
            {
                return index > LowestIndex && index < HighestIndex;
            }

            /// <summary>
            /// Given an arbitrary row index, this calculates the upper boundary for the page containing the index.
            /// </summary>
            /// <param name="rowIndex">Row index used for calculation.</param>
            /// <returns>The highest index of the page containing the given index.</returns>
            private static int MapToUpperBoundary(int rowIndex)
            {
                // Return the highest index of a page containing the given index.
                return MapToLowerBoundary(rowIndex) + rowsPerPage - 1;
            }
        }

        /// <summary>
        /// Enumerates the dictionaries that represent row content stored by field name.
        /// </summary>
        private class AttributeCacheEnumerator : IEnumerator<Dictionary<string, object>>
        {
            private readonly AttributeCache _cache;
            private int _row;

            /// <summary>
            /// Initializes a new instance of the <see cref="AttributeCacheEnumerator"/> class.
            /// </summary>
            /// <param name="cache">Cache that contains the data.</param>
            public AttributeCacheEnumerator(AttributeCache cache)
            {
                _cache = cache;
            }

            /// <inheritdoc />
            public Dictionary<string, object> Current { get; private set; }

            /// <inheritdoc />
            object IEnumerator.Current => Current;

            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                _row += 1;
                if (_row >= _cache._dataSupply.NumRows()) return false;
                Current = _cache.RetrieveElement(_row);
                return true;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _row = -1;
            }
        }
        #endregion
    }
}