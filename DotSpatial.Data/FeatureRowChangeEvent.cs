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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2010 6:59:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Data;

namespace DotSpatial.Data
{
    /// <summary>
    ///Row event argument class
    ///</summary>
    public class FeatureRowChangeEvent : EventArgs
    {
        private readonly DataRowAction _eventAction;
        private readonly FeatureRow _eventRow;

        /// <summary>
        /// A new event argument for events in a FeatureTable.
        /// </summary>
        /// <param name="row">The FeatureRow of the event.</param>
        /// <param name="action">The action occuring for this event.</param>
        public FeatureRowChangeEvent(FeatureRow row, DataRowAction action)
        {
            _eventRow = row;
            _eventAction = action;
        }

        /// <summary>
        /// The FeatureRow for this event.
        /// </summary>
        public FeatureRow Row
        {
            get
            {
                return _eventRow;
            }
        }

        /// <summary>
        /// The action for this event.
        /// </summary>
        public DataRowAction Action
        {
            get
            {
                return _eventAction;
            }
        }
    }
}