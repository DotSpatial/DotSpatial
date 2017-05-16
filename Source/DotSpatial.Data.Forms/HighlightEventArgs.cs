// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// Indicates whether the control is now highlighted.
    /// </summary>
    public class HighlightEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEventArgs"/> class.
        /// </summary>
        /// <param name="isHighlighted">Indicates whether the control is now highlighted.</param>
        public HighlightEventArgs(bool isHighlighted)
        {
            IsHighlighted = isHighlighted;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the control is now highlighted.
        /// </summary>
        public bool IsHighlighted { get; protected set; }

        #endregion
    }
}