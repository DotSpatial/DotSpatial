// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
// The Original Code is DotSpatial.dll
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace DotSpatial.Data
{
    public class DS_DataTable : IDataTable
    {
        private DataTable _dataTable;

        /// <summary>
        /// Initializes a new instance of the DS_DataTable class with no arguments.
        /// </summary>
        public DS_DataTable()
        {
            _dataTable = new DataTable();
            _dataTable.RowDeleted += new DataRowChangeEventHandler(Deleted);
        }

        /// <summary>
        /// Initializes a new instance of the DS_DataTable class with the specified table name.
        /// </summary>
        /// <param name="tableName">The name to give the table.</param>
        public DS_DataTable(string tableName)
        {
            _dataTable = new DataTable(tableName);
            _dataTable.RowDeleted += new DataRowChangeEventHandler(Deleted);
        }

        /// <summary>
        /// Initializes a new instance of the DS_DataTable class using the specified table name and namespace.
        /// </summary>
        /// <param name="tableName">The name to give the table.</param>
        /// <param name="tableNamespace">The namespace for the XML representation of the data stored in the DataTable. </param>
        public DS_DataTable(string tableName, string tableNamespace)
        {
            _dataTable = new DataTable(tableName, tableNamespace);
            _dataTable.RowDeleted += new DataRowChangeEventHandler(Deleted);
        }

        /// <summary>
        /// Initializes a new instance of the DS_DataTable class from a DataTable
        /// </summary>
        /// <param name="table"></param>
        public DS_DataTable(DataTable table)
        {
            _dataTable = table;
            _dataTable.RowDeleted += new DataRowChangeEventHandler(Deleted);
        }

        /// <summary>
        /// Gets the collection of columns that belong to this table.
        /// </summary>
        public IDataColumnCollection Columns
        {
            get 
            {
                return new DS_DataColumnCollection(_dataTable.Columns);
            }
        }

        /// <summary>
        /// Gets the collection of rows that belong to this table.
        /// </summary>
        public IDataRowCollection Rows
        {
            get 
            {
                return new DS_DataRowCollection(_dataTable.Rows);
            }
        }

        public DataTable Table
        {
            get
            {
                return _dataTable;
            }
        }

        /// <summary>
        /// Gets or sets an array of columns that function as primary keys for the data table.
        /// </summary>
        public DataColumn[] PrimaryKey
        {
            get
            {
                return _dataTable.PrimaryKey;
            }
            set
            {
                _dataTable.PrimaryKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the locale information used to compare strings within the table.
        /// </summary>
        public CultureInfo Locale
        {
            get
            {
                return _dataTable.Locale;
            }
            set
            {
                _dataTable.Locale = value;
            }
        }

        /// <summary>
        /// Commits all the changes made to this table since the last time AcceptChanges was called.
        /// </summary>
        public void AcceptChanges()
        {
            _dataTable.AcceptChanges();
        }

        /// <summary>
        /// Turns off notifications, index maintenance, and constraints while loading data.
        /// </summary>
        public void BeginLoadData()
        {
            _dataTable.BeginLoadData();
        }

        /// <summary>
        /// Turns on notifications, index maintenance, and constraints after loading data.
        /// </summary>
        public void EndLoadData()
        {
            _dataTable.EndLoadData();
        }

        /// <summary>
        /// Clears the DS_DataTable of all data.
        /// </summary>
        public void Clear()
        {
            _dataTable.Clear();
        }

        /// <summary>
        /// Releases all resources used by the MarshalByValueComponent.
        /// </summary>
        public void Dispose()
        {
            _dataTable.Dispose();
        }

        /// <summary>
        /// Creates a new IDataRow with the same schema as the table.
        /// </summary>
        /// <returns>An IDataRow with the same schema as the IDataTable.</returns>
        public IDataRow NewRow()
        {
            return new DS_DataRow(_dataTable.NewRow());
        }

        /// <summary>
        /// Gets an array of all IDataRow objects that match the filter criteria.
        /// </summary>
        /// <param name="filterExpression">The criteria to use to filter the rows.</param>
        /// <returns>An array of IDataRow objects.</returns>
        public IDataRow[] Select(string filterExpression)
        {
            List<IDataRow> rows = new List<IDataRow>();
            foreach (DataRow row in _dataTable.Select(filterExpression))
            {
                rows.Add(new DS_DataRow(row));
            }
            return rows.ToArray();
        }

        //
        public event IDataRowChangeEventHandler RowDeleted;
        private void Deleted(object sender, DataRowChangeEventArgs e)
        {
            if (RowDeleted != null)
                RowDeleted(this, new DS_DataRowChangeEventArgs(new DS_DataRow(e.Row), e.Action));
        }
        
    }
}