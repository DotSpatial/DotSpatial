// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Represents the argument for events that need an envelope.
    /// </summary>
    public class EnvelopeArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvelopeArgs"/> class.
        /// </summary>
        /// <param name="inEnvelope">Envelope of the event.</param>
        public EnvelopeArgs(Envelope inEnvelope)
        {
            Envelope = inEnvelope;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the envelope specific to this event.
        /// </summary>
        public Envelope Envelope { get; protected set; }

        #endregion
    }
}