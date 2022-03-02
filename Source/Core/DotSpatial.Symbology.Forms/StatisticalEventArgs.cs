// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// StatisticalEventArgs.
    /// </summary>
    public class StatisticalEventArgs : EventArgs
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticalEventArgs"/> class.
        /// </summary>
        /// <param name="statistics">The statistics of this event.</param>
        public StatisticalEventArgs(Statistics statistics)
        {
            Statistics = statistics;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the set of statistics related to this event.
        /// </summary>
        public Statistics Statistics { get; protected set; }

        #endregion
    }
}