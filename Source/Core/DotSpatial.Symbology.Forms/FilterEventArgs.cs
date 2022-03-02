// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This class in an EventArgs that also supports a filter expression.
    /// </summary>
    public class FilterEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterEventArgs"/> class.
        /// </summary>
        /// <param name="filterExpression">String, the filter expression to add.</param>
        public FilterEventArgs(string filterExpression)
        {
            FilterExpression = filterExpression;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the string filter expression.
        /// </summary>
        public string FilterExpression { get; }

        #endregion
    }
}