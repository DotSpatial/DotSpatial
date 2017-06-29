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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/9/2009 2:07:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace DotSpatial.Data
{
    public class AttributeCache : IEnumerable<Dictionary<string, object>>
    {
        private static int _rowsPerPage;
        private readonly IAttributeSource _dataSupply;
        /// <summary>
        /// The row being edited
        /// </summary>
        public Dictionary<string, object> EditRow;

        /// <summary>
        /// The pages currently stored in the cache
        /// </summary>
        public DataPage[] Pages;

        private int _editRowIndex;

        /// <summary>
        /// Constructs a new Cache object that can create data tables by using a DataPageRetriever
        /// </summary>
        /// <param name="dataSupplier">Any structure that implements IDataPageRetriever</param>
        /// <param name="rowsPerPage">The rows per page</param>
        public AttributeCache(IAttributeSource dataSupplier, int rowsPerPage)
        {
            _dataSupply = dataSupplier;
            _rowsPerPage = rowsPerPage;
            _editRowIndex = -1;
            LoadFirstTwoPages();
        }

        /// <summary>
        /// The integer index of the row being edited
        /// </summary>
        public int EditRowIndex
        {
            get { return _editRowIndex; }
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
        /// The number of rows in the data supply
        /// </summary>
        public int NumRows
        {
            get { return _dataSupply.NumRows(); }
        }

        #region IEnumerable<Dictionary<string,object>> Members

        /// <inheritdoc />
        public IEnumerator<Dictionary<string, object>> GetEnumerator()
        {
            return new AttributeCacheEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new AttributeCacheEnumerator(this);
        }

        #endregion

        /// <summary>
        /// Saves the changes in the edit row to the tabular cache as well as the underlying database
        /// </summary>
        public void SaveChanges()
        {
            _dataSupply.Edit(EditRowIndex, EditRow);
            IDataRow dr = null; // CGX AERO GLZ
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

        // Sets the value of the element parameter if the value is in the cache.
        private bool IfPageCachedThenSetElement(int rowIndex,
            int columnIndex, ref object element)
        {
            element = string.Empty;
            int index = rowIndex % _rowsPerPage;
            if (IsRowCachedInPage(0, rowIndex))
            {
                if (Pages[0].Table.Rows.Count <= index) return true;
                element = Pages[0].Table.Rows[index][columnIndex];
                return true;
            }
            if (IsRowCachedInPage(1, rowIndex))
            {
                if (Pages[1].Table.Rows.Count <= index) return true;
                element = Pages[1].Table.Rows[index][columnIndex];
                return true;
            }

            return false;
        }

        // Sets the value of the element parameter if the value is in the cache.
        private bool IfPageCachedThenSetElement(int rowIndex, ref Dictionary<string, object> element)
        {
            if (IsRowCachedInPage(0, rowIndex))
            {
                IDataRow dr = Pages[0].Table.Rows[rowIndex % _rowsPerPage]; // CGX AERO GLZ
                foreach (DataColumn column in Pages[0].Table.Columns)
                {
                    string name = column.ColumnName;
                    element.Add(name, dr[name]);
                }
                return true;
            }
            if (IsRowCachedInPage(1, rowIndex))
            {
                IDataRow dr = Pages[1].Table.Rows[rowIndex % _rowsPerPage]; // CGX AERO GLZ
                foreach (DataColumn column in Pages[0].Table.Columns)
                {
                    string name = column.ColumnName;
                    element.Add(name, dr[name]);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Obtains the element at the specified index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Dictionary<string, object> RetrieveElement(int rowIndex)
        {
            if (EditRowIndex == rowIndex)
            {
                return EditRow;
            }
            Dictionary<string, object> element = new Dictionary<string, object>();
            if (IfPageCachedThenSetElement(rowIndex, ref element))
            {
                return element;
            }
            return RetrieveDataCacheItThenReturnElement(rowIndex);
        }

        /// <summary>
        /// Obtains the element at the specified index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public object RetrieveElement(int rowIndex, int columnIndex)
        {
            object element = null;
            DataColumn[] columns = _dataSupply.GetColumns();
            if (rowIndex == _editRowIndex && EditRow != null)
            {
                return EditRow[columns[columnIndex].ColumnName];
            }
            if (IfPageCachedThenSetElement(rowIndex, columnIndex, ref element))
            {
                return element;
            }
            return RetrieveDataCacheItThenReturnElement(
                rowIndex, columnIndex);
        }

        private void LoadFirstTwoPages()
        {
            DataPage p1 = new DataPage(_dataSupply.GetAttributes(DataPage.MapToLowerBoundary(0), _rowsPerPage), 0);
            DataPage p2 =
                new DataPage(_dataSupply.GetAttributes(DataPage.MapToLowerBoundary(_rowsPerPage), _rowsPerPage),
                             _rowsPerPage);
            Pages = new[] { p1, p2 };
        }

        private object RetrieveDataCacheItThenReturnElement(
            int rowIndex, int columnIndex)
        {
            // Retrieve a page worth of data containing the requested value.
            IDataTable table = _dataSupply.GetAttributes( // CGX AERO GLZ
                DataPage.MapToLowerBoundary(rowIndex), _rowsPerPage);

            // Replace the cached page furthest from the requested cell
            // with a new page containing the newly retrieved data.
            Pages[GetIndexToUnusedPage(rowIndex)] = new DataPage(table, rowIndex);

            return RetrieveElement(rowIndex, columnIndex);
        }

        private Dictionary<string, object> RetrieveDataCacheItThenReturnElement(int rowIndex)
        {
            // Retrieve a page worth of data containing the requested value.
            IDataTable table = _dataSupply.GetAttributes(DataPage.MapToLowerBoundary(rowIndex), _rowsPerPage); // CGX AERO GLZ

            // Replace the cached page furthest from the requested cell
            // with a new page containing the newly retrieved data.
            Pages[GetIndexToUnusedPage(rowIndex)] = new DataPage(table, rowIndex);

            return RetrieveElement(rowIndex);
        }

        // Returns the index of the cached page most distant from the given index
        // and therefore least likely to be reused.
        private int GetIndexToUnusedPage(int rowIndex)
        {
            if (rowIndex > Pages[0].HighestIndex &&
                rowIndex > Pages[1].HighestIndex)
            {
                int offsetFromPage0 = rowIndex - Pages[0].HighestIndex;
                int offsetFromPage1 = rowIndex - Pages[1].HighestIndex;
                if (offsetFromPage0 < offsetFromPage1)
                {
                    return 1;
                }
                return 0;
            }
            else
            {
                int offsetFromPage0 = Pages[0].LowestIndex - rowIndex;
                int offsetFromPage1 = Pages[1].LowestIndex - rowIndex;
                if (offsetFromPage0 < offsetFromPage1)
                {
                    return 1;
                }
                return 0;
            }
        }

        // Returns a value indicating whether the given row index is contained
        // in the given DataPage.
        private bool IsRowCachedInPage(int pageNumber, int rowIndex)
        {
            return rowIndex <= Pages[pageNumber].HighestIndex &&
                rowIndex >= Pages[pageNumber].LowestIndex;
        }

        #region Nested type: AttributeCacheEnumerator

        /// <summary>
        /// Enumerates the dictionaries that represent row content stored by field name.
        /// </summary>
        public class AttributeCacheEnumerator : IEnumerator<Dictionary<string, object>>
        {
            private readonly AttributeCache _cache;
            private int _row;
            private Dictionary<string, object> _rowContent;

            /// <summary>
            /// Creates a new AttributeCacheEnumerator
            /// </summary>
            /// <param name="cache"></param>
            public AttributeCacheEnumerator(AttributeCache cache)
            {
                _cache = cache;
            }

            #region IEnumerator<Dictionary<string,object>> Members

            /// <inheritdoc />
            public Dictionary<string, object> Current
            {
                get { return _rowContent; }
            }

            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            object IEnumerator.Current
            {
                get { return _rowContent; }
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                _row += 1;
                if (_row >= _cache._dataSupply.NumRows()) return false;
                _rowContent = _cache.RetrieveElement(_row);
                return true;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _row = -1;
            }

            #endregion
        }

        #endregion

        #region Nested type: DataPage

        /// <summary>
        /// Represents one page of data.
        /// </summary>
        public struct DataPage
        {
            /// <summary>
            /// The data Table
            /// </summary>
            public readonly IDataTable Table; // CGX AERO GLZ

            private readonly int _highestIndexValue;
            private readonly int _lowestIndexValue;

            /// <summary>
            /// A Data page representing one page of data-row values
            /// </summary>
            /// <param name="table">The DataTable that controls the content</param>
            /// <param name="rowIndex">The integer row index</param>
            public DataPage(IDataTable table, int rowIndex) // CGX AERO GLZ
            {
                Table = table;

                _lowestIndexValue = MapToLowerBoundary(rowIndex);
                _highestIndexValue = MapToUpperBoundary(rowIndex);
                Debug.Assert(_lowestIndexValue >= 0);
                Debug.Assert(_highestIndexValue >= 0);
            }

            /// <summary>
            /// The integer lowest index of the page
            /// </summary>
            public int LowestIndex
            {
                get
                {
                    return _lowestIndexValue;
                }
            }

            /// <summary>
            /// The integer highest index of the page
            /// </summary>
            public int HighestIndex
            {
                get
                {
                    return _highestIndexValue;
                }
            }

            /// <summary>
            /// Tests to see if the specified index is in this page.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public bool Contains(int index)
            {
                return index > _lowestIndexValue && index < _highestIndexValue;
            }

            /// <summary>
            /// Given an arbitrary row index, this calculates what the lower boundary would be for the page containing the index
            /// </summary>
            /// <param name="rowIndex"></param>
            /// <returns></returns>
            public static int MapToLowerBoundary(int rowIndex)
            {
                // Return the lowest index of a page containing the given index.
                return (rowIndex / _rowsPerPage) * _rowsPerPage;
            }

            /// <summary>
            /// Given an arbitrary row index, this calculates the upper boundary for the page containing the index
            /// </summary>
            /// <param name="rowIndex"></param>
            /// <returns></returns>
            private static int MapToUpperBoundary(int rowIndex)
            {
                // Return the highest index of a page containing the given index.
                return MapToLowerBoundary(rowIndex) + _rowsPerPage - 1;
            }
        }

        #endregion
    }
}