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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/9/2009 1:23:02 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Data;

namespace DotSpatial.Data
{
    /// <summary>
    /// IDataPageRetriever
    /// </summary>
    public interface IAttributeSource
    {
        /// <summary>
        /// Supplies a page of content in the form of a data Table.
        /// </summary>
        /// <param name="startIndex">The integer lower page boundary</param>
        /// <param name="numRows">The integer number of rows to return for the page</param>
        /// <returns>A DataTable made up with the virtual row content.</returns>
        DataTable GetAttributes(int startIndex, int numRows);

        /// <summary>
        /// Reads just the content requested in order to satisfy the paging ability of VirtualMode for the DataGridView
        /// </summary>
        /// <param name="startIndex">The integer lower page boundary</param>
        /// <param name="numRows">The integer number of attribute values to return for the page</param>
        /// <param name="fieldNames">The list or array of fieldnames to return.</param>
        /// <returns>A DataTable populated with data rows with only the specified values.</returns>
        DataTable GetAttributes(int startIndex, int numRows, IEnumerable<string> fieldNames);

        /// <summary>
        /// Converts a page of content from a DataTable format, saving it back to the source.
        /// </summary>
        /// <param name="startIndex">The 0 based integer index representing the first row in the file (corresponding to the 0 row of the data table)</param>
        /// <param name="pageValues">The DataTable representing the rows to set.  If the row count is larger than the dataset, this will add the rows instead.</param>
        void SetAttributes(int startIndex, DataTable pageValues);

        /// <summary>
        /// The number of rows
        /// </summary>
        /// <returns></returns>
        int NumRows();

        /// <summary>
        /// Gets the column with the specified name
        /// </summary>
        /// <param name="name">the name to search</param>
        /// <returns>the Field matching the specified name</returns>
        DataColumn GetColumn(string name);

        /// <summary>
        /// Gets a copy of the fields used to build the data structure.  This is useful for learning
        /// about the existing fields, but doesn't allow direct changes to the underlying data structure.
        /// </summary>
        DataColumn[] GetColumns();

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        void AddRow(Dictionary<string, object> values);

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The dictionary of object values by string field name holding the new values to store.</param>
        void Edit(int index, Dictionary<string, object> values);

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        void AddRow(DataRow values);

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The dictionary of object values by string field name holding the new values to store.</param>
        void Edit(int index, DataRow values);

        /// <summary>
        /// Given a string expression, this returns the count of the members that satisfy that expression.
        /// </summary>
        /// <param name="expressions">The array of string expressions to test</param>
        /// <param name="progressHandler">The progress handler that might also instruct this step to be canceled.</param>
        /// <param name="maxSampleSize">The integer maximum sample size from which to draw counts.  If this is negative, it will not be used.</param>
        /// <returns>An array of integer counts of the members that match the expression</returns>
        int[] GetCounts(string[] expressions, ICancelProgressHandler progressHandler, int maxSampleSize);
    }
}