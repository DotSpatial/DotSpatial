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
    public class DS_DataRowChangeEventArgs : IDataRowChangeEventArgs
    {
        private DataRowChangeEventArgs _args;
        private DS_DataRow _row;

        /// <summary>
        /// Initializes a new instance of the DS_DataRowChangeEventArgs class.
        /// </summary>
        /// <param name="row">The IDataRow upon which an action is occuring. </param>
        /// <param name="action">One of the DataRowAction values. </param>
        public DS_DataRowChangeEventArgs(DS_DataRow row, DataRowAction action)
        {
            _args = new DataRowChangeEventArgs(row.dataRow, action);
            _row = row;
        }

        /// <summary>
        /// Gets the action that has occurred on a IDataRow.
        /// </summary>
        public DataRowAction Action
        {
            get
            {
                return _args.Action;
            }
        }

        /// <summary>
        /// Gets the row upon which an action has occurred.
        /// </summary>
        public IDataRow Row
        {
            get
            {
                return _row;
            }
        }

        
    }
}