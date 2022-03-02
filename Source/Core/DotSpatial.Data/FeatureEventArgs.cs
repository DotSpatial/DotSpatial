// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// FeatureEventArgs.
    /// </summary>
    public class FeatureEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureEventArgs"/> class.
        /// </summary>
        /// <param name="inFeature">The feature of the event.</param>
        public FeatureEventArgs(IFeature inFeature)
        {
            Feature = inFeature;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature being referenced by this event.
        /// </summary>
        public IFeature Feature { get; protected set; }

        #endregion
    }
}