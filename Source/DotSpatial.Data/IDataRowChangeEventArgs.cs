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
    /// Provides data for the RowChanged, RowChanging, OnRowDeleting, and OnRowDeleted events.
    /// </summary>
    public interface IDataRowChangeEventArgs
    {
        /// <summary>
        /// Gets the action that has occurred on a IDataRow.
        /// </summary>
        DataRowAction Action 
        { 
            get;
        }
        
        /// <summary>
        /// Gets the row upon which an action has occurred.
        /// </summary>
        IDataRow Row 
        { 
            get; 
        }

    }
}