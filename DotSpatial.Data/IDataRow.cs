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

namespace DotSpatial.Data
{
    /// <summary>
    /// Represents a row of data in a IDataTable.
    /// </summary>
    public interface IDataRow
    {
        /// <summary>
        /// Gets or sets the data stored in the specified DataColumn.
        /// </summary>
        /// <param name="column">A DataColumn that contains the data. </param>
        /// <returns>An Object that contains the data.</returns>
        Object this[DataColumn column] 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the data stored in the column specified by index.
        /// </summary>
        /// <param name="columnIndex">The zero-based index of the column. </param>
        /// <returns>An Object that contains the data.</returns>
        Object this[int columnIndex]
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data stored in the column specified by name.
        /// </summary>
        /// <param name="columnName">The name of the column. </param>
        /// <returns>An Object that contains the data.</returns>
        Object this[string columnName]
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets all the values for this row through an array.
        /// </summary>
        Object[] ItemArray 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets the IDataTable for which this row has a schema.
        /// </summary>
        IDataTable Table 
        { 
            get; 
        }

    }
}