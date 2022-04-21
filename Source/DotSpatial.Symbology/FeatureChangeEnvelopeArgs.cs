// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Represents the argument for events that need to report changed features and the corresponding envelope.
    /// </summary>
    public class FeatureChangeEnvelopeArgs : FeatureChangeArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureChangeEnvelopeArgs"/> class.
        /// </summary>
        /// <param name="inChangedFeatures">Features that were changed.</param>
        /// <param name="inEnvelope">Geographic envelope for the most recent selection event.</param>
        public FeatureChangeEnvelopeArgs(List<int> inChangedFeatures, Envelope inEnvelope)
            : base(inChangedFeatures)
        {
            Envelope = inEnvelope;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic envelope for the most recent selection event.
        /// </summary>
        public Envelope Envelope { get; protected set; }

        #endregion
    }
}