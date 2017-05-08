// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2008 2:00:57 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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