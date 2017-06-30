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
using System.Data;

namespace DotSpatial.Data
{
    public class DS_DataRow : IDataRow
    {
        public DataRow dataRow;

        /// <summary>
        /// Initializes a new instance of the DS_DataRow from a DataRow
        /// </summary>
        /// <param name="row">The specified DataRow</param>
        public DS_DataRow(DataRow row)
        {
            dataRow = row;
        }

        /// <summary>
        /// Gets or sets the data stored in the specified DataColumn.
        /// </summary>
        /// <param name="column">A DataColumn that contains the data. </param>
        /// <returns>An Object that contains the data.</returns>
        public Object this[DataColumn column]
        {
            get
            {
                return dataRow[column];
            }
            set
            {
                dataRow[column] = value;
            }
        }

        /// <summary>
        /// Gets or sets the data stored in the column specified by index.
        /// </summary>
        /// <param name="columnIndex">The zero-based index of the column. </param>
        /// <returns>An Object that contains the data.</returns>
        public Object this[int columnIndex]
        {
            get
            {
                return dataRow[columnIndex];
            }
            set
            {
                dataRow[columnIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the data stored in the column specified by name.
        /// </summary>
        /// <param name="columnName">The name of the column. </param>
        /// <returns>An Object that contains the data.</returns>
        public Object this[string columnName]
        {
            get
            {
                return dataRow[columnName];
            }
            set
            {
                dataRow[columnName] = value;
            }
        }

        /// <summary>
        /// Gets or sets all the values for this row through an array.
        /// </summary>
        public object[] ItemArray
        {
            get
            {
                return dataRow.ItemArray;
            }
            set
            {
                dataRow.ItemArray = value;
            }
        }

        /// <summary>
        /// Gets the IDataTable for which this row has a schema.
        /// </summary>
        public IDataTable Table
        {
            get 
            {
                return new DS_DataTable(dataRow.Table);
            }
        }

        // CGX SLM
        public override bool Equals(object Obj)
        {
            DS_DataRow other = (DS_DataRow)Obj;
            return this == other;
        }

        public static bool operator ==(DS_DataRow dr1, DS_DataRow dr2)
        {
            return dr1.dataRow == dr2.dataRow;
            /*bool ret = true;
            if (dr1.ItemArray.Length != dr2.ItemArray.Length)
                return false;
            for (int i = 0; i < dr1.ItemArray.Length; i++)
            {
                if (!dr1.ItemArray[i].Equals(dr2.ItemArray[i]))
                    return false;
            }
            return ret;*/
        }

        public static bool operator !=(DS_DataRow dr1, DS_DataRow dr2)
        {
            return dr1.dataRow != dr2.dataRow;
            /*bool ret = false;
            if (dr1.ItemArray.Length != dr2.ItemArray.Length)
                return true;
            for (int i = 0; i < dr1.ItemArray.Length; i++)
            {
                if (!dr1.ItemArray[i].Equals(dr2.ItemArray[i]))
                    return true;
            }
            return ret;*/
        }

        // FIN CGX
    }
}