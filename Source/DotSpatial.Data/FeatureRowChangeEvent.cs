// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
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
        /// <param name="action">The action occurring for this event.</param>
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