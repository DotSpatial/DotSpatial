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
using System.Data;
using System.Globalization;

namespace DotSpatial.Data
{
    /// <summary>
    /// Represents one table of data.
    /// </summary>
    public interface IDataTable
    {
        /// <summary>
        /// Gets the collection of columns that belong to this table.
        /// </summary>
        IDataColumnCollection Columns 
        { 
            get; 
        }

        /// <summary>
        /// Gets the collection of rows that belong to this table.
        /// </summary>
        IDataRowCollection Rows
        {
            get;
        }

        /// <summary>
        /// Gets or sets an array of columns that function as primary keys for the data table.
        /// </summary>
        DataColumn[] PrimaryKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the locale information used to compare strings within the table.
        /// </summary>
        CultureInfo Locale 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets the System DataTable associated to the IDataTable
        /// </summary>
        DataTable Table
        {
            get;
        }


        /// <summary>
        /// Commits all the changes made to this table since the last time AcceptChanges was called.
        /// </summary>
        void AcceptChanges();

        /// <summary>
        /// Turns off notifications, index maintenance, and constraints while loading data.
        /// </summary>
        void BeginLoadData();

        /// <summary>
        /// Turns on notifications, index maintenance, and constraints after loading data.
        /// </summary>
        void EndLoadData();

        /// <summary>
        /// Clears the DataTable of all data.
        /// </summary>
        void Clear();

        /// <summary>
        /// Releases all resources used by the MarshalByValueComponent.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Creates a new IDataRow with the same schema as the table.
        /// </summary>
        /// <returns>An IDataRow with the same schema as the IDataTable.</returns>
        IDataRow NewRow();

        /// <summary>
        /// Gets an array of all IDataRow objects that match the filter criteria.
        /// </summary>
        /// <param name="filterExpression">The criteria to use to filter the rows.</param>
        /// <returns>An array of IDataRow objects.</returns>
        IDataRow[] Select(string filterExpression);

        /// <summary>
        /// Occurs after a row in the table has been deleted.
        /// </summary>
        event IDataRowChangeEventHandler RowDeleted;

    }
}