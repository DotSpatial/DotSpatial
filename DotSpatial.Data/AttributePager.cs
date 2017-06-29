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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/16/2009 11:35:54 AM
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
    /// AttributePager
    /// </summary>
    public class AttributePager : IEnumerable<DataTable>, IEnumerator<DataTable>
    {
        #region Private Variables

        private readonly int _numRows;
        private readonly IAttributeSource _source;
        private DataTable _currentTable;
        private int _pageIndex;
        private int _pageSize;

        #endregion

        /// <summary>
        /// Creates a new instance of AttributePager
        /// </summary>
        public AttributePager(IAttributeSource source, int pageSize)
        {
            _numRows = source.NumRows();
            _source = source;
            _pageSize = pageSize;
        }

        #region IEnumerable<DataTable> Members

        /// <inheritdoc />
        public IEnumerator<DataTable> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion

        #region IEnumerator<DataTable> Members

        /// <summary>
        /// Gets the current table
        /// </summary>
        public DataTable Current
        {
            get { return _currentTable; }
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public void Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return _currentTable; }
        }

        /// <summary>
        /// Advances to the next attribute
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            _pageIndex += 1;
            if (_pageIndex >= NumPages()) return false;
            _currentTable = this[_pageIndex];
            return true;
        }

        /// <summary>
        /// reset the attribute pager
        /// </summary>
        public void Reset()
        {
            _currentTable = null;
            _pageIndex = -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the starting row index of the current page.
        /// </summary>
        public int StartIndex
        {
            get { return _pageIndex * _pageSize; }
        }

        /// <summary>
        /// Gets the pages size as a count of the number of rows each data table page should hold
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            protected set { _pageSize = value; }
        }

        /// <summary>
        /// This returns the data table for the corresponding page index, but also sets the
        /// Pager so that it is sitting on the specified page index.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public DataTable this[int pageIndex]
        {
            get
            {
                if (_pageIndex != pageIndex || _currentTable == null)
                {
                    _currentTable = _source.GetAttributes(pageIndex * PageSize, RowCount(pageIndex));
                    _pageIndex = pageIndex;
                }
                return _currentTable;
            }
        }

        /// <summary>
        /// The integer number of pages
        /// </summary>
        /// <returns>the number of pages</returns>
        public int NumPages()
        {
            return (int)Math.Ceiling((double)_numRows / _pageSize);
        }

        /// <summary>
        /// Gets the number of rows on the specified page.
        /// </summary>
        /// <param name="pageindex">The page index</param>
        /// <returns>The number of rows that should be on that page.</returns>
        public int RowCount(int pageindex)
        {
            if (pageindex == NumPages() - 1) return _numRows % _pageSize;
            return PageSize;
        }

        /// <summary>
        /// Loads the appropriate page if it isn't loaded already and returns the DataRow that
        /// matches the specified index
        /// </summary>
        /// <param name="rowIndex">The integer row index</param>
        /// <returns>The DataRow</returns>
        public DataRow Row(int rowIndex)
        {
            int page = PageOfRow(rowIndex);
            if (_pageIndex != page || _currentTable == null)
            {
                _currentTable = _source.GetAttributes(page * PageSize, RowCount(page));
                _pageIndex = page;
            }
            return _currentTable.Rows[rowIndex % PageSize];
        }

        #endregion

        /// <summary>
        /// Returns the page that the specified row is on
        /// </summary>
        /// <param name="rowIndex">The integer row index</param>
        /// <returns>The page of the row in question</returns>
        public int PageOfRow(int rowIndex)
        {
            return (int)Math.Floor((double)rowIndex / _pageSize);
        }
    }
}