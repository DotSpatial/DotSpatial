// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        private readonly int _numRows;
        private readonly IAttributeSource _source;
        private int _pageIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributePager"/> class.
        /// </summary>
        /// <param name="source">IAttributeSource to get the attribues from.</param>
        /// <param name="pageSize">Number of rows each data table page should hold.</param>
        public AttributePager(IAttributeSource source, int pageSize)
        {
            _numRows = source.NumRows();
            _source = source;
            PageSize = pageSize;
        }

        #region Properties

        /// <summary>
        /// Gets the current table.
        /// </summary>
        public DataTable Current { get; private set; }

        /// <summary>
        /// Gets or sets the pages size as a count of the number of rows each data table page should hold.
        /// </summary>
        public int PageSize { get; protected set; }

        /// <summary>
        /// Gets the starting row index of the current page.
        /// </summary>
        public int StartIndex => _pageIndex * PageSize;

        /// <summary>
        /// Gets the current table.
        /// </summary>
        object IEnumerator.Current => Current;

        #endregion

        /// <summary>
        /// This returns the data table for the given page index, but also sets the Pager so that it is sitting on the specified page index.
        /// </summary>
        /// <param name="pageIndex">Index of the page that should be returned.</param>
        /// <returns>The data table for the given page index.</returns>
        public DataTable this[int pageIndex]
        {
            get
            {
                if (_pageIndex != pageIndex || Current == null)
                {
                    Current = _source.GetAttributes(pageIndex * PageSize, RowCount(pageIndex));
                    _pageIndex = pageIndex;
                }

                return Current;
            }
        }

        #region Methods

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public IEnumerator<DataTable> GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// Advances to the next attribute.
        /// </summary>
        /// <returns>False if the index is bigger then NumPages.</returns>
        public bool MoveNext()
        {
            _pageIndex += 1;
            if (_pageIndex >= NumPages()) return false;
            Current = this[_pageIndex];
            return true;
        }

        /// <summary>
        /// The integer number of pages.
        /// </summary>
        /// <returns>the number of pages</returns>
        public int NumPages()
        {
            return (int)Math.Ceiling((double)_numRows / PageSize);
        }

        /// <summary>
        /// Returns the page that the specified row is on
        /// </summary>
        /// <param name="rowIndex">The integer row index</param>
        /// <returns>The page of the row in question</returns>
        public int PageOfRow(int rowIndex)
        {
            return (int)Math.Floor((double)rowIndex / PageSize);
        }

        /// <summary>
        /// reset the attribute pager
        /// </summary>
        public void Reset()
        {
            Current = null;
            _pageIndex = -1;
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
            if (_pageIndex != page || Current == null)
            {
                Current = _source.GetAttributes(page * PageSize, RowCount(page));
                _pageIndex = page;
            }

            return Current.Rows[rowIndex % PageSize];
        }

        /// <summary>
        /// Gets the number of rows on the specified page.
        /// </summary>
        /// <param name="pageindex">The page index</param>
        /// <returns>The number of rows that should be on that page.</returns>
        public int RowCount(int pageindex)
        {
            if (pageindex == NumPages() - 1) return _numRows % PageSize;
            return PageSize;
        }

        #endregion

        /// <summary>
        /// Returns this as IEnumerator.
        /// </summary>
        /// <returns>This as IEnumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}