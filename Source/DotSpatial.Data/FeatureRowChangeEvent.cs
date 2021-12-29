// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;

namespace DotSpatial.Data
{
    /// <summary>
    /// Row event argument class.
    /// </summary>
    public class FeatureRowChangeEvent : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureRowChangeEvent"/> class.
        /// </summary>
        /// <param name="row">The FeatureRow of the event.</param>
        /// <param name="action">The action occurring for this event.</param>
        public FeatureRowChangeEvent(FeatureRow row, DataRowAction action)
        {
            Row = row;
            Action = action;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the action for this event.
        /// </summary>
        public DataRowAction Action { get; }

        /// <summary>
        /// Gets the FeatureRow for this event.
        /// </summary>
        public FeatureRow Row { get; }

        #endregion
    }
}