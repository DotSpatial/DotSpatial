// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FeatureSymbolizerEventArgs
    /// </summary>
    public class FeatureSymbolizerEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSymbolizerEventArgs"/> class.
        /// </summary>
        /// <param name="symbolizer">The feature symbolizer of the event.</param>
        public FeatureSymbolizerEventArgs(IFeatureSymbolizer symbolizer)
        {
            Symbolizer = symbolizer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature symbolizer for this event.
        /// </summary>
        public IFeatureSymbolizer Symbolizer { get; protected set; }

        #endregion
    }
}