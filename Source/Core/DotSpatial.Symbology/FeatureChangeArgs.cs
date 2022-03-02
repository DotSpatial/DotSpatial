// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Represents the argument for events that need to report changed features.
    /// </summary>
    public class FeatureChangeArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureChangeArgs"/> class.
        /// </summary>
        /// <param name="inChangedFeatures">The changed features.</param>
        public FeatureChangeArgs(List<int> inChangedFeatures)
        {
            ChangedFeatures = inChangedFeatures;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of features that were changed by this event.
        /// </summary>
        public List<int> ChangedFeatures { get; protected set; }

        #endregion
    }
}