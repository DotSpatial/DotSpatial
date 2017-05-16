// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An EventArgs specifically tailored to FeatureSet.
    /// </summary>
    public class FeatureSetEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSetEventArgs"/> class.
        /// </summary>
        /// <param name="featureSet">The IFeatureSet that is involved in this event.</param>
        public FeatureSetEventArgs(IFeatureSet featureSet)
        {
            FeatureSet = featureSet;
        }

        /// <summary>
        /// Gets or sets the FeatureSet associated with this event.
        /// </summary>
        public IFeatureSet FeatureSet { get; protected set; }
    }
}